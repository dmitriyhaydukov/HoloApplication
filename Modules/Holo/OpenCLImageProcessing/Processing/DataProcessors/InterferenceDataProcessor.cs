using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Processing.Computing;
using Processing.DataBinding;
using Processing.Utils;

namespace Processing.DataProcessors
{
    [DataProcessor(Group = "Interferometry", Name = "Интерференция")]
    public class InterferenceDataProcessor : SingleImageOutputDataProcessorBase
    {
        private string _title = NamingUtil.IndexedTitle("Phase retrieval result ");

        [ImageSlotWithSubfields("Волновой фронт 1", "Default", OnPropertyChanged = nameof(Compute), OnImageUpdated = nameof(Compute))]
        public IImageHandler Input1 { get; set; }

        [ImageSlotWithSubfields("Волновой фронт 2", "Default", OnPropertyChanged = nameof(Compute), OnImageUpdated = nameof(Compute))]
        public IImageHandler Input2 { get; set; }

        public override void Awake()
        {
            Compute();
        }

        public override void FreeResources()
        {
        }

        public void Compute()
        {
            if (Input1 == null || Input2 == null)
                return;

            CreateOrUpdateOutputWithSameParametres(Input1, _title);

            using (StartOperationScope(Input1, Input2, Output))
            {
                ImageProcessing.Interference(Input1, Input2, Output);
            }
        }
    }
}
