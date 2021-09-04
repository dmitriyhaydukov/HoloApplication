using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Cloo;
using Infrastructure;
using Processing.Computing;
using Processing.DataBinding;

namespace Processing.DataProcessors
{
    [DataProcessor(Group = "Generators", Name = "Flat Wavefront")]
    public class FlatWavefrontGenerator : SingleImageOutputDataProcessorBase
    {
        private const string KernelName = "flatWavefront";

        private ComputeKernel _kernel;
        private OpenClApplication OpenClApplication => Singleton.Get<OpenClApplication>();

        [Input]
        [Number("Ширина изображения", 256, 10000, 2)]
        public float ImageWidth { get; set; } = 768;

        [Input]
        [Number("Высота изображения", 256, 10000, 2)]
        public float ImageHeight { get; set; } = 768;

        [Action(TooltipText = "Обновить")]
        public void UpdateImage()
        {
            if (Output.Width == (int) ImageWidth && Output.Height == (int) ImageHeight)
                return;

            Output?.FreeComputingDevice();
            Output = ImageHandler.Create("test", (int)ImageWidth, (int)ImageHeight, ImageFormat.RealImaginative, ImagePixelFormat.Float);
            Output.UploadToComputingDevice();
            Compute();
        }

        [Input]
        [Number("Угол", 0, 90, 0.1f, OnPropertyChanged = "Compute")]
        public float Alpha { get; set; }

        [Input]
        [Number("Амплитуда", -10000, 10000, 0.01f, OnPropertyChanged = "Compute")]
        public float Amplitude { get; set; } = 1f;

        public override void Initialize()
        {
            if (Initialized)
                return;

            base.Initialize();

            Output = ImageHandler.Create("test", (int)ImageWidth, (int)ImageHeight, ImageFormat.RealImaginative, ImagePixelFormat.Float);
        }
      

        private void PrepareResources()
        {
            Output.UploadToComputingDevice();
            _kernel = OpenClApplication.Program.CreateKernel(KernelName);
        }

        public void Compute()
        {
            OpenClApplication.Queue.AcquireGLObjects(new[] {Output.ComputeBuffer}, null);

            _kernel.SetValueArgument(0, Alpha / 180f * (float)Math.PI);
            _kernel.SetValueArgument(1, Amplitude);
            _kernel.SetMemoryArgument(2, Output.ComputeBuffer);

            OpenClApplication.ExecuteInQueue(_kernel, Output.Width, Output.Height);
            OpenClApplication.Queue.Finish();

            OpenClApplication.Queue.ReleaseGLObjects(new[] {Output.ComputeBuffer}, null);
            OnUpdated();
            Output.Update();
        }

        public override void Awake()
        {
            Initialize();
            PrepareResources();
            Compute();
        }

        public override void FreeResources()
        {
            Output.FreeComputingDevice();
        }
    }
}