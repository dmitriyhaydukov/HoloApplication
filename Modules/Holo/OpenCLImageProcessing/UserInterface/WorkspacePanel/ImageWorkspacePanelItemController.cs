using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Processing;

namespace UserInterface.WorkspacePanel
{
    public class ImageWorkspacePanelItemController: WorkspacePanelItemControllerBase
    {
        private IImageHandler ImageHandler { get; set; }

        public ImageWorkspacePanelItemController(IImageHandler imageHandler, WorkspacePanelItem view) : base(view)
        {
            ImageHandler = imageHandler;

            if (ImageHandler != null)
                ImageHandler.ImageUpdated += data => UpdateView();
        }

        public void UpdateView()
        {
            ImageHandler.Tags.TryGetValue(ImageHandlerTagKeys.Title, out object title);

            View.SetTitle((string)title ?? "Image");
            View.SetInfo($"{ImageHandler.Width} x {ImageHandler.Height}\n{ImageHandler.Format} {ImageHandler.PixelFormat}");

            if (ImageHandler.Tags.TryGetValue(ImageHandlerTagKeys.Thumbnail, out object thumbnail))
                View.SetImage((Image) thumbnail);
        }
    }
}
