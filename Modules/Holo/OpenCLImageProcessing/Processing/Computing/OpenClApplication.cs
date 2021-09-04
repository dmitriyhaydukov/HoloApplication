using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Cloo;
using Common;
using Infrastructure;
using OpenTK.Input;

namespace Processing.Computing
{
    public class OpenClApplication
    {
        private List<IImageHandler> _acquiredImages = new List<IImageHandler>();
        private List<IImageHandler> _singleOperationContextReservedImages = new List<IImageHandler>();

        private const string MainSourceFileName = "program";

        // We will need the device context, which is obtained through an OS specific function.
        [DllImport("opengl32.dll")]
        static extern IntPtr wglGetCurrentDC();

        private IntPtr OpenTkContextHandle => ((OpenTK.Graphics.IGraphicsContextInternal) OpenTK.Graphics.GraphicsContext.CurrentContext).Context.Handle;

        public string ProgramCode
        {
            get => Singleton.Get<IOpenClSourcesProvider>().GetProgram(MainSourceFileName, false);
            set => Singleton.Get<IOpenClSourcesProvider>().SaveProgram(value, MainSourceFileName);
        }

        public ComputeContext ComputeContext { get; private set; }
        public ComputeProgram Program { get; private set; }
        public ComputeCommandQueue Queue { get; private set; }
        public void Setup()
        {
            if (ComputeContext != null)
                return;

            var platforms = ComputePlatform.Platforms;

            var contextSettings = new OpenClContextSettings();
            if (!SettingsEventsDispatcherProvider.Get<OpenClContextSettings>().RequestSettings(contextSettings))
            {
                contextSettings.ComputePlatformName = platforms.First(p => p.Devices.Any(d => d.Type == ComputeDeviceTypes.Gpu)).Name;
            }

            var platform = platforms.Single(p => p.Name == contextSettings.ComputePlatformName);
            var properties = new ComputeContextPropertyList(platform);
            ComputeContext = new ComputeContext(ComputeDeviceTypes.Gpu, properties, null, IntPtr.Zero);
        }

        public void SetupUsingOpenGLContext()
        {
            if (ComputeContext != null)
                return;

            // http://www.cmsoft.com.br/opencl-tutorial/openclopengl-interoperation/
            // https://github.com/RedpointGames/Cloo/blob/master/Cloo/ComputeContext.cs

            var handle = OpenTkContextHandle;
            var p1 = new ComputeContextProperty(ComputeContextPropertyName.CL_GL_CONTEXT_KHR, OpenTkContextHandle);
            var p2 = new ComputeContextProperty(ComputeContextPropertyName.CL_WGL_HDC_KHR, wglGetCurrentDC());

            var computePlatform =
                ComputePlatform.Platforms.FirstOrDefault(p => p.Devices.Any(d => d.Type == ComputeDeviceTypes.Gpu)) ??
                ComputePlatform.Platforms[0];

            var p3 = new ComputeContextProperty(ComputeContextPropertyName.Platform, computePlatform.Handle.Value);
            var props = new List<ComputeContextProperty> { p1, p2, p3 };
            var properties = new ComputeContextPropertyList(props);
            ComputeContext = new ComputeContext(ComputeDeviceTypes.Gpu, properties, null, IntPtr.Zero);

            BuildProgram();

            Queue = new ComputeCommandQueue(ComputeContext, ComputeContext.Devices.First(), ComputeCommandQueueFlags.None);
        }

        public void BuildProgram()
        {
            var programCode = ProgramCode;
            var fftCode = Singleton.Get<IOpenClSourcesProvider>().GetProgram("fft", false);

            var program = TryBuild(programCode + fftCode);

            if (program.GetBuildStatus(ComputeContext.Devices.First()) != ComputeProgramBuildStatus.Success)
                throw new Exception(program.GetBuildLog(ComputeContext.Devices.First()));

            Program = program;
        }

        public ComputeProgram TryBuild(string code)
        {
            var program  = new ComputeProgram(ComputeContext, code);
            try
            {
                program.Build(ComputeContext.Devices, "", null, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                DebugLogger.Log("В процессе компиляции программы было брошено исключение:");
                DebugLogger.Log(ex);
            }

            return program;
        }

        public void ExecuteInQueue(ComputeKernel kernel, long width, long height)
        {
            try
            {
                Queue.Execute(kernel, null, new[] {width, height}, new[] {1L, 1L}, null);
                Queue.Finish();
            }
            catch (Exception ex)
            {
                DebugLogger.Exception(ex);
            }
        }

        public void ExecuteKernel(ComputeKernel kernel, long width, long height, params object[] args)
        {
            ExecuteKernelInternal(kernel, width, height, args);
        }

        public void ExecuteKernel(string kernelName, long width, long height, params object[] args)
        {
            ExecuteKernelInternal(Program.CreateKernel(kernelName), width, height, args);
        }

        private void ExecuteKernelInternal(ComputeKernel kernel, long width, long height, object[] args)
        {
            var i = 0;
            foreach (var arg in args)
            {
                if (arg is ComputeMemory computeMemory)
                    kernel.SetMemoryArgument(i++, computeMemory);
                else if (arg is IImageHandler imageHandler)
                    kernel.SetMemoryArgument(i++, imageHandler.ComputeBuffer);
                else if (arg is int iArg)
                    kernel.SetValueArgument(i++, iArg);
                else if (arg is float fArg)
                    kernel.SetValueArgument(i++, fArg);
                else
                    throw new NotImplementedException($"Тип {arg.GetType().Name} не поддерживается как аргумент ядра.");
            }

            //var computeImage2Ds = args.OfType<IImageHandler>().Select(h => h.ComputeBuffer).ToArray();
           // Queue.AcquireGLObjects(computeImage2Ds, null);

            ExecuteInQueue(kernel, width, height);
            Queue.Finish();

            //Queue.ReleaseGLObjects(computeImage2Ds, null);
        }

        public void Acquire(params IImageHandler[] images)
        {
            var notAcquiredImages = images.Except(_acquiredImages);
            Queue.AcquireGLObjects(notAcquiredImages.Select(i => i.ComputeBuffer).ToArray(), null);

            foreach (var notAcquiredImage in notAcquiredImages)
            {
                notAcquiredImage.InOperationScope = true;
            }

            _acquiredImages.AddRange(notAcquiredImages);
        }

        public void Release(params IImageHandler[] images)
        {
            var notReleasedImages = images.Intersect(_acquiredImages);
            var imagesToRelease = notReleasedImages.Except(_singleOperationContextReservedImages);
            Queue.ReleaseGLObjects(imagesToRelease.Select(i => i.ComputeBuffer).ToArray(), null);

            foreach (var image in imagesToRelease)
            {
                image.InOperationScope = false;
                _acquiredImages.Remove(image);
            }
        }

        public class SingleOperationContext : IDisposable
        {
            private readonly IImageHandler[] _contextReservedImages;

            /// <summary>
            /// Инициализирует контекст операции, в котором указанные изображения не должны возвращаться под управление OpenGL
            /// </summary>
            /// <param name="operationImages"></param>
            public SingleOperationContext(params IImageHandler[] operationImages)
            {
                var app = Singleton.Get<OpenClApplication>();
                var applicationSingleOperaionContextReservedImages = app._singleOperationContextReservedImages;
                _contextReservedImages = operationImages.Except(applicationSingleOperaionContextReservedImages).ToArray();
                applicationSingleOperaionContextReservedImages.AddRange(_contextReservedImages);
                app.Acquire(_contextReservedImages);
            }

            public void Dispose()
            {
                var app = Singleton.Get<OpenClApplication>();
                foreach (var contextReservedImage in _contextReservedImages)
                {
                    app._singleOperationContextReservedImages.Remove(contextReservedImage);
                    app.Release(contextReservedImage);
                }
            }
        }
    }
}
