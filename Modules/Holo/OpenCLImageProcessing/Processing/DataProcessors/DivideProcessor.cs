using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Processing.Computing;
using Processing.DataBinding;
using Processing.Utils;

namespace Processing.DataProcessors
{
    [DataProcessor(Group = "Image Operations", Name = "Деление")]
    public class DivideProcessor : SingleImageOutputDataProcessorBase
    {
        private string _title = NamingUtil.IndexedTitle("Division result ");

        [ImageSlotWithSubfields("Делимое", "Default", OnPropertyChanged = nameof(Compute), OnImageUpdated = nameof(Compute))]
        public IImageHandler Numerator { get; set; }

        [ImageSlotWithSubfields("Делитель", "Default", OnPropertyChanged = nameof(Compute), OnImageUpdated = nameof(Compute))]
        public IImageHandler Denumerator { get; set; }

        public override void Awake()
        {
            Compute();
        }

        public override void FreeResources()
        {
        }

        public void Compute()
        {
            if (Numerator == null || Denumerator == null)
                return;

            if (!Numerator.SizeEquals(Denumerator))
                throw new InvalidOperationException("Размеры изображений должны совпадать.");

            CreateOrUpdateOutputWithSameParametres(Numerator, _title);

            using (StartOperationScope(Numerator, Denumerator, Output))
            {
                ImageProcessing.Divide(Numerator, Denumerator, Output);
            }
        }
    }
}
