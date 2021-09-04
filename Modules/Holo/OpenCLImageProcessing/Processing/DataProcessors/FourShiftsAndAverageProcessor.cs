using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processing.Computing;
using Processing.DataBinding;
using Processing.Utils;

namespace Processing.DataProcessors
{
    [DataProcessor(Group = "Test", Name = "4 сдвига и усреднение")]
    public class FourShiftsAndAverageProcessor : SingleImageOutputDataProcessorBase
    {
        private string _title = NamingUtil.IndexedTitle("Test ");

        [ImageSlotWithSubfields("Расшифрованное изображение 1", "Default", OnPropertyChanged = nameof(Compute), OnImageUpdated = nameof(Compute))]
        public IImageHandler Reference { get; set; }

        [ImageSlotWithSubfields("Расшифрованное изображение 2", "Default", OnPropertyChanged = nameof(Compute), OnImageUpdated = nameof(Compute))]
        public IImageHandler Input { get; set; }

        private ImageHandler _temp;

        public override void Awake()
        {
            Compute();
        }

        public override void FreeResources()
        {
        }

        public void Compute()
        {
            if (Input == null || Reference == null)
                return;

            if (!Input.SizeEquals(Reference))
                throw new InvalidOperationException("Изображения должны быть одного размера");

            CreateOrUpdateImageWithSameParametres(Reference, _title, ref _temp);
            CreateOrUpdateOutputWithSameParametres(Reference, _title);

            using (StartOperationScope(Input, Reference, _temp, Output))
            {
                int dx, dy, cnt = 0;
                for (dx = 0; dx <= 1; dx++)
                {
                    for (dy = 0; dy <= 1; dy++)
                    {
                        ImageProcessing.Shift(Input, _temp, dx, dy, true);
                        ImageProcessing.Divide(Reference, _temp, _temp);
                        ImageProcessing.Sum(_temp, Output, Output);
                        cnt++;
                    }
                }

                ImageProcessing.Divide(Output, cnt, Output);
            }

        }
    }
}
