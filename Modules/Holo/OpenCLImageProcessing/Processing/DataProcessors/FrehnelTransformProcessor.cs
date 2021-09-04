using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloo;
using Common;
using Processing.Computing;
using Processing.DataBinding;
using Processing.Utils;

namespace Processing.DataProcessors
{
    [DataProcessor(Group = "Interferometry", Name="Преобразование Френеля", Tooltip = "Преобразование Френеля")]
    public class FrehnelTransformProcessor : SingleImageOutputDataProcessorBase
    {
        private string _title = NamingUtil.IndexedTitle("Freshnel transform result ");
        
        [Input]
        [ImageSlotWithSubfields("Голограмма", "Default", OnPropertyChanged = nameof(Process), OnImageUpdated = nameof(Process))]
        public IImageHandler Input { get; set; }

        [Input]
        [Number("Размер объекта, мм", 0, 50, 0.01f, OnPropertyChanged = nameof(Process))]
        public float ObjectSize { get; set; } = 6.35f;

        [Input]
        [Number("Расстояние, мм", 1, 5000, 1, OnPropertyChanged = nameof(Process))]
        public float Distance { get; set; } = 135;

        [Input]
        [Number("Длина волны, нм", 380, 760, 1, OnPropertyChanged = nameof(Process))]
        public float Wavelength { get; set; } = 532f;

        public override void Awake()
        {
        }

        public override void FreeResources()
        {
        }
        
        public void Process()
        {
            if (Input == null)
                return;

            CreateOrUpdateOutputWithSameParametres(Input, _title, ImagePixelFormat.Float, ImageFormat.RealImaginative);
            if (Input != null && Output != null && Input.Ready && Output.Ready)
            {
                using (new Timer("Freshnel"))
                {
                    using (StartOperationScope(Input, Output))
                    {
                        try
                        {
                            Freshnel.Transform(Input, Output, Wavelength, Distance, ObjectSize, true);
                        }
                        catch (Exception ex)
                        {
                            DebugLogger.Log(ex, DebugLogger.ImportanceLevel.Exception);
                        }
                    }
                }
            }
        }
    }
}
