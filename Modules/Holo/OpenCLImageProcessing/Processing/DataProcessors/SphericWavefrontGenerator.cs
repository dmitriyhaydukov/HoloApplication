using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloo;
using Infrastructure;
using Processing.Computing;
using Processing.DataBinding;

namespace Processing.DataProcessors
{
    [DataProcessor(Group = "Generators", Name = "Spheric Wavefront")]
    public class SphericWavefrontGenerator : SingleImageOutputDataProcessorBase
    {
        private const string KernelName = "sphericWavefront";

        private ComputeKernel _kernel;
        private OpenClApplication OpenClApplication => Singleton.Get<OpenClApplication>();

        [Input]
        [Number("Ширина изображения", 256, 10000, 2)]
        public float ImageWidth { get; set; } = 2816;

        [Input]
        [Number("Высота изображения", 256, 10000, 2)]
        public float ImageHeight { get; set; } = 2816;

        [Action(TooltipText = "Обновить")]
        public void UpdateImage()
        {
            if (Output.Width == (int)ImageWidth && Output.Height == (int)ImageHeight)
                return;

            Output?.FreeComputingDevice();
            Output = ImageHandler.Create("test", (int)ImageWidth, (int)ImageHeight, ImageFormat.RealImaginative, ImagePixelFormat.Float);
            Output.UploadToComputingDevice();
            Compute();
        }

        [Input]
        [Number("Длина волны, нм", 320, 800, 0.1f, OnPropertyChanged = "Compute")]
        public float Lambda { get; set; } = 532;

        [Input]
        [Number("Расстояние, мм", 0, 10000, 1, OnPropertyChanged = "Compute")]
        public float Distance { get; set; } = 200;

        [Input]
        [Number("Размер по горизонтали, мм", 0, 1000, 0.01f, OnPropertyChanged = "Compute")]
        public float SizeX { get; set; } = 5;

        [Input]
        [Number("Размер по вертикали, мм", 0, 1000, 0.01f, OnPropertyChanged = "Compute")]
        public float SizeY { get; set; } = 5;

        [Input]
        [Number("Угол X", 0, 360, 0.01f, OnPropertyChanged = "Compute")]
        public float AlphaX { get; set; } = 5;

        [Input]
        [Number("Угол Y", 0, 360, 0.01f, OnPropertyChanged = "Compute")]
        public float AlphaY { get; set; } = 5;

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
            OpenClApplication.Queue.AcquireGLObjects(new[] { Output.ComputeBuffer }, null);
            
            _kernel.SetMemoryArgument(0, Output.ComputeBuffer);
            _kernel.SetValueArgument(1, Lambda / 1000f);
            _kernel.SetValueArgument(2, Distance * 1000f);
            _kernel.SetValueArgument(3, SizeX * 1000f);
            _kernel.SetValueArgument(4, SizeY * 1000f);
            _kernel.SetValueArgument(5, AlphaX / 360f * (float)Math.PI);
            _kernel.SetValueArgument(6, AlphaY / 360f * (float)Math.PI);
            _kernel.SetValueArgument(7, Amplitude);

            OpenClApplication.ExecuteInQueue(_kernel, Output.Width, Output.Height);

            OpenClApplication.Queue.ReleaseGLObjects(new[] { Output.ComputeBuffer }, null);
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
