using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using Processing.Computing;
using Processing.DataBinding;
using Processing.Utils;

namespace Processing.DataProcessors
{
    [DataProcessor(Group = "CommonTransforms", Name = "Fourier", Tooltip = "Преобразование Фурье")]
    public class FourierTransformProcessor : SingleImageOutputDataProcessorBase
    {
        private Fourier _fourier;
        private IImageHandler _oldInput;

        [Input]
        [ImageSlotWithSubfields("Ввод", "Default", OnPropertyChanged = "InputImageChanged")]
        public IImageHandler Input { get; set; }

        public override void Awake()
        {
            InputImageChanged();
            Process();
        }

        public override void FreeResources()
        {
            
        }

        public void InputImageChanged()
        {
            if (Input == null)
            {
                Output?.FreeComputingDevice();
                Output = null;
                OnImageUpdated(new ImageUpdatedEventData(true));
                return;
            }

            if (Input != _oldInput)
            {
                if (_oldInput != null)
                    _oldInput.ImageUpdated -= InputOnImageUpdated;

                Input.ImageUpdated += InputOnImageUpdated;
                _oldInput = Input;
            }

            if (Output != null && Output.SizeEquals(Input))
                return;

            Output = ImageHandler.Create("fft result", Input.Width, Input.Height, ImageFormat.RealImaginative, ImagePixelFormat.Float);
            Output.UploadToComputingDevice();
            OnImageUpdated(new ImageUpdatedEventData(true));

            Process();
        }

        [Action(TooltipText = "Циклический сдвиг")]
        public void CyclicShift()
        {
            if (Output != null && Output.Ready)
            {
                var app = Singleton.Get<OpenClApplication>();
                app.Acquire(Output);
                ImageUtils.CyclicShift(Output);
                app.Release(Output);
                OnUpdated();
                Output.Update();
            }
        }

        private void InputOnImageUpdated(ImageUpdatedEventData imageUpdatedEventData)
        {
            Process();
        }

        private void Process()
        {
            if (Input != null && Output != null && Input.Ready && Output.Ready)
            {
                Fourier.Transform(Input, Output);
                OnUpdated();
                Output.Update();
            }
        }
    }
}
