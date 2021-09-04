using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.DataBinding
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ImageSlotWithSubfieldsAttribute : ImageSlotAttribute
    {
        public ImageSlotWithSubfieldsAttribute(string tooltipText, string @group, params ImageFormat[] allowedImageFormats) : base(tooltipText, @group, allowedImageFormats)
        {
        }

        public ImageSlotWithSubfieldsAttribute(string tooltipText, string @group, int? requiredChannelCount, ImagePixelFormat? allowedPixelFormat, params ImageFormat[] allowedImageFormats) : base(tooltipText, @group, requiredChannelCount, allowedPixelFormat, allowedImageFormats)
        {
        }
    }
}
