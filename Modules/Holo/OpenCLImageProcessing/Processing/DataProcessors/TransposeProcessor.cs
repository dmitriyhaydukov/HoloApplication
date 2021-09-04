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
    [DataProcessor(Group = "Image Transform", Name = "Транспонирование")]
    public class TransposeProcessor : SingleImageOutputDataProcessorBase
    {
        [Input]
        [ImageSlotWithSubfields("Ввод", "Default", OnPropertyChanged = "InputImageChanged")]
        public IImageHandler Input { get; set; }

        public override void Awake()
        {
            InputImageChanged();
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


            var app = Singleton.Get<OpenClApplication>();
            app.Acquire(Input, Output);

            Output = ImageUtils.Transpose(Input);

            app.Release(Input, Output);

            OnUpdated();
            Output.Update();
            OnImageUpdated(new ImageUpdatedEventData(true));
        }
    }
}
