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
    [DataProcessor(Group = "Interferometry", Name = "Восстановление фазы (WIP)")]
    public class PhaseRecoveryProcessor : SingleImageOutputDataProcessorBase
    {
        private string _title = NamingUtil.IndexedTitle("Phase retrieval result ");

        [ImageSlotWithSubfields("Голограмма", "Default", OnPropertyChanged = nameof(Compute), OnImageUpdated = nameof(Compute))]
        public IImageHandler Input { get; set; }

        [Input]
        [Number("Размер объекта, мм", 0, 50, 0.01f, OnPropertyChanged = nameof(Compute))]
        public float ObjectSize { get; set; } = 7;

        [Input]
        [Number("Расстояние, мм", 1, 5000, 1, OnPropertyChanged = nameof(Compute))]
        public float Distance { get; set; } = 135;

        [Input]
        [Number("Длина волны, нм", 380, 760, 1, OnPropertyChanged = nameof(Compute))]
        public float Wavelength { get; set; } = 500f;

        private ImageHandler _temp;
        private ImageHandler _inputFreshnelTransform;

        [Number("Смещение", -1024, 1024, 1, OnPropertyChanged = nameof(Compute))]
        public float Shift { get; set; } = 1f;

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
            CreateOrUpdateImageWithSameParametres(Input, _title, ref _temp);
            CreateOrUpdateImageWithSameParametres(Input, _title, ref _inputFreshnelTransform);

            using (StartOperationScope(Input, Output, _temp, _inputFreshnelTransform))
            {
                int dx = 0, dy = 0;

                Freshnel.Transform(Input, _inputFreshnelTransform, Wavelength, Distance, ObjectSize, true);

                for (dx = 0; dx <= 2; dx++)
                {
                    for (dy = 0; dy <= 2; dy++)
                    {
                        if (dx == 0 && dy == 0)
                            continue;

                        ImageProcessing.Shift(Input, _temp, dx * (int)Shift, dy * (int)Shift, true);
                        Freshnel.Transform(_temp, _temp, Wavelength, Distance, ObjectSize, true);
                        ImageProcessing.Divide(_inputFreshnelTransform, _temp, _temp);
                        ImageProcessing.Sum(_temp, Output, Output);
                    }
                }

                ImageProcessing.Divide(Output, 8, Output);
            }
        }
    }
}
