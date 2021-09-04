using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Processing.Computing;
using Processing.DataBinding;
using Processing.Utils;

namespace Processing.DataProcessors
{
    [DataProcessor(Group = "Image Operations", Name = "Сумма")]
    public class SummProcessor : SingleImageOutputDataProcessorBase
    {
        private string _title = NamingUtil.IndexedTitle("Summ result ");

        [ImageSlotWithSubfields("Слагаемое 1", "Default", OnPropertyChanged = nameof(Compute), OnImageUpdated = nameof(Compute))]
        public IImageHandler Image1 { get; set; }

        [ImageSlotWithSubfields("Слагаемое 2", "Default", OnPropertyChanged = nameof(Compute), OnImageUpdated = nameof(Compute))]
        public IImageHandler Image2 { get; set; }

        public override void Awake()
        {
            Compute();
        }

        public override void FreeResources()
        {
        }

        public void Compute()
        {
            if (Image1 == null || Image2 == null)
                return;

            if (!Image1.SizeEquals(Image2))
                throw new InvalidOperationException("Размеры изображений должны совпадать.");

            CreateOrUpdateOutputWithSameParametres(Image1, _title);

            using (StartOperationScope(Image1, Image2, Output))
            {
                ImageProcessing.Sum(Image1, Image2, Output);
                ImageProcessing.Divide(Output, 2, Output);
            }
        }
    }
}
