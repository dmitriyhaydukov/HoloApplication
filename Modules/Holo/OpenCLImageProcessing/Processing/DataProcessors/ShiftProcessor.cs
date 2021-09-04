using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Processing.Computing;
using Processing.DataBinding;
using Processing.Utils;

namespace Processing.DataProcessors
{
    [DataProcessor(Group = "Image Transform", Name = "Сдвиг")]
    public class ShiftProcessor : SingleImageOutputDataProcessorBase
    {
        private string _title = NamingUtil.IndexedTitle("Shifted image ");

        [ImageSlotWithSubfields("Input", "Default", OnPropertyChanged = nameof(Compute), OnImageUpdated = nameof(Compute))]
        public IImageHandler Input { get; set; }

        [Number("Сдвиг по X", -1024, 1024, 1, OnPropertyChanged = nameof(Compute))]
        public float ShiftX { get; set; }

        [Number("Сдвиг по Y", -1024, 1024, 1, OnPropertyChanged = nameof(Compute))]
        public float ShiftY { get; set; }

        public override void Awake()
        {
            Compute();
        }

        public override void FreeResources()
        {
        }

        public void Compute()
        {
            if (Input == null)
                return;

            CreateOrUpdateOutputWithSameParametres(Input, _title);

            using (StartOperationScope(Input, Output))
            {
                ImageProcessing.Shift(Input, Output, (int) ShiftX, (int) ShiftY, true);
            }
        }
    }
}
