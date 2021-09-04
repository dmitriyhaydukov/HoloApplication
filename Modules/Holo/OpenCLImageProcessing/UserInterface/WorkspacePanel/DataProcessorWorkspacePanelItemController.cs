using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Processing;
using Processing.DataProcessors;

namespace UserInterface.WorkspacePanel
{
    public class DataProcessorWorkspacePanelItemController : WorkspacePanelItemControllerBase
    {
        private IDataProcessor DataProcessor { get; set; }
        private DataProcessorInfo DataProcessorInfo { get; set; }

        public DataProcessorWorkspacePanelItemController(IDataProcessor dataProcessor, WorkspacePanelItem view): base(view)
        {
            DataProcessor = dataProcessor;
            DataProcessorInfo = new DataProcessorInfo(dataProcessor);

            dataProcessor.Updated += UpdateView;
        }

        public void UpdateView()
        {
            View.SetTitle(DataProcessorInfo.DisplayName);
            View.SetInfo(DataProcessorInfo.Type.Name + "\n" + DataProcessorInfo.DisplayTooltip);

            var imageHandler = DataProcessorInfo.Outputs.SingleOrDefault(o => typeof(IImageHandler).IsAssignableFrom(o.Type))?.Get<IImageHandler>() ??
                               DataProcessorInfo.Inputs.FirstOrDefault(o => typeof(IImageHandler).IsAssignableFrom(o.Type))?.Get<IImageHandler>();

            if (imageHandler == null)
                return;

            if (imageHandler.Tags.TryGetValue(ImageHandlerTagKeys.Thumbnail, out object thumbnail))
                View.SetImage((Image)thumbnail);
        }
    }
}
