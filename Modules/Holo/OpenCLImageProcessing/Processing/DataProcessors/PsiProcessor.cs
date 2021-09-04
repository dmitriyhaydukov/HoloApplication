using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Cloo;
using Common;
using Infrastructure;
using OpenTK;
using Processing.Computing;
using Processing.DataBinding;

namespace Processing.DataProcessors
{
    [DataProcessor(Group = "Interferometry", Name = "PSI", Tooltip = "Получение математической голограммы по 4 голографическим изображениям.")]
    public class PsiProcessor : SingleImageOutputDataProcessorBase
    {
        private const string KernelName = "psi4Kernel";
        private OpenClApplication OpenClApplication => Singleton.Get<OpenClApplication>();
        private IImageHandler[] _inputImages = new IImageHandler[4];
        private float[] _phaseShift = {0, 90, 180, 270}; // Фазовый сдвиг в градусах - "fz"
        private ComputeKernel _kernel;
        private float _amplitude = 1;

        #region Input Properties
        // TODO: make attributes for array rendering
        [Input]
        [Number("Phase Shift 1", 0, 360, 0.1f)]
        public float PhaseShift1 { get => _phaseShift[0]; set => SetPhaseShift(value, 0); }

        [Input]
        [ImageSlot("Image 1", "Default", ImageFormat.Greyscale)]
        public IImageHandler Image1 { get => _inputImages[0]; set => SetInputImage(value, 0); }

        [Input]
        [Number("Phase Shift 2", 0, 360, 0.1f)]
        public float PhaseShift2 { get => _phaseShift[1]; set => SetPhaseShift(value, 1); }

        [Input]
        [ImageSlot("Image 2", "Default", ImageFormat.Greyscale)]
        public IImageHandler Image2 { get => _inputImages[1]; set => SetInputImage(value, 1); }

        [Input]
        [Number("Phase Shift 3", 0, 360, 0.1f)]
        public float PhaseShift3 { get => _phaseShift[2]; set => SetPhaseShift(value, 2); }

        [Input]
        [ImageSlot("Image 3", "Default", ImageFormat.Greyscale)]
        public IImageHandler Image3 { get => _inputImages[2]; set => SetInputImage(value, 2); }

        [Input]
        [Number("Phase Shift 4", 0, 360, 0.1f)]
        public float PhaseShift4 { get => _phaseShift[3]; set => SetPhaseShift(value, 3); }

        [Input]
        [ImageSlot("Image 4", "Default", ImageFormat.Greyscale)]
        public IImageHandler Image4 { get => _inputImages[3]; set => SetInputImage(value, 3); }

        [Input]
        [Number("Amplitude", 0, 100, 0.1f)]
        public float Amplitude { get => _amplitude; set { _amplitude = value; Compute(); } }
        #endregion
        
        private void PrepareResources()
        {
            if (_inputImages.Any(i => i == null))
                return;

            foreach (var image in _inputImages)
            {
                image.UploadToComputingDevice();
            }

            if (Output == null || !Output.SizeEquals(_inputImages[0]))
            {
                Output?.FreeComputingDevice();
                Output = null;

                Output = ImageHandler.Create("Psi result", _inputImages[0].Width, _inputImages[0].Height, ImageFormat.RealImaginative, ImagePixelFormat.Float);
                Output.UploadToComputingDevice();
                OnImageUpdated(new ImageUpdatedEventData(true));
            }

            _kernel = OpenClApplication.Program.CreateKernel(KernelName);
        }

        private void Compute()
        {
            using (new Timer("Compute psi"))
            {

                if (_inputImages.Any(i => i == null))
                    return;

                OpenClApplication.Queue.AcquireGLObjects(new[] {_inputImages[0].ComputeBuffer, _inputImages[1].ComputeBuffer, _inputImages[2].ComputeBuffer, _inputImages[3].ComputeBuffer, Output.ComputeBuffer}, null);
                _kernel.SetMemoryArgument(0, _inputImages[0].ComputeBuffer);
                _kernel.SetMemoryArgument(1, _inputImages[1].ComputeBuffer);
                _kernel.SetMemoryArgument(2, _inputImages[2].ComputeBuffer);
                _kernel.SetMemoryArgument(3, _inputImages[3].ComputeBuffer);

                var phaseShiftInRadians = _phaseShift.Select(ps => (float) (Math.PI * ps / 180)).ToArray();
                var kSin = phaseShiftInRadians.Select(ps => (float) Math.Sin(ps)).ToArray();
                var sinOrto = kSin.OrtogonalVector();
                var kCos = phaseShiftInRadians.Select(ps => (float) Math.Cos(ps)).ToArray();
                var cosOrto = kCos.OrtogonalVector();
                var znmt = sinOrto.VectorMul(kCos);

                _kernel.SetValueArgument(4, new Vector4(sinOrto[0], sinOrto[1], sinOrto[2], sinOrto[3]));
                _kernel.SetValueArgument(5, new Vector4(cosOrto[0], cosOrto[1], cosOrto[2], cosOrto[3]));
                _kernel.SetValueArgument(6, Math.Abs(znmt));
                _kernel.SetValueArgument(7, _amplitude);

                _kernel.SetMemoryArgument(8, Output.ComputeBuffer);

                OpenClApplication.ExecuteInQueue(_kernel, Output.Width, Output.Height);
                OpenClApplication.Queue.Finish();
                OpenClApplication.Queue.ReleaseGLObjects(new[] {_inputImages[0].ComputeBuffer, _inputImages[1].ComputeBuffer, _inputImages[2].ComputeBuffer, _inputImages[3].ComputeBuffer, Output.ComputeBuffer}, null);
                OnUpdated();
                Output.Update();
            }
        }

        public override void Awake()
        {
            PrepareResources();
            Compute();
        }

        public override void FreeResources()
        {
        }

        private void SetInputImage(IImageHandler value, int index)
        {
            if (value == null)
            {
                _inputImages[index] = null;
                return;
            }

            var setImages = _inputImages.Where(i => i != null);
            if (setImages.Any(i => i.Width != value.Width || i.Height != value.Height))
                throw new InvalidOperationException("Все изображения должны быть одинакового размера");

            _inputImages[index] = value;

            PrepareResources();
            Compute();
        }

        private void SetPhaseShift(float value, int index)
        {
            _phaseShift[index] = value;
            Compute();
        }
    }
}
