using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Processing.Computing;
using Processing.DataBinding;
using Processing.Utils;

namespace Processing.DataProcessors
{
    [DataProcessor(Group = "Image Operations", Name = "Комбинирование каналов")]
    public class CombineChannelsProcessor : SingleImageOutputDataProcessorBase
    {
        private string _title = NamingUtil.IndexedTitle("Combination result ");

        [ImageSlotWithSubfields("Изображения для извлечения амплитуды", "Default", OnPropertyChanged = nameof(Compute), OnImageUpdated = nameof(Compute))]
        public IImageHandler AmplitudeImage { get; set; }

        [ImageSlotWithSubfields("Изображения для извлечения фазы", "Default", OnPropertyChanged = nameof(Compute), OnImageUpdated = nameof(Compute))]
        public IImageHandler PhaseImage { get; set; }

        public override void Awake()
        {
            Compute();
        }

        public override void FreeResources()
        {
        }

        public void Compute()
        {
            if (AmplitudeImage == null || PhaseImage == null)
                return;

            CreateOrUpdateOutputWithSameParametres(AmplitudeImage, _title);

            using (StartOperationScope(AmplitudeImage, PhaseImage, Output))
            {
                ImageProcessing.Combine(AmplitudeImage, PhaseImage, Output);
            }
        }
    }
}
