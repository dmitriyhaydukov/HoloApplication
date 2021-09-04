using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Processing.DataBinding
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ImageSlotAttribute : MemberBindingAttributeBase
    {
        public IReadOnlyCollection<ImageFormat> AllowedImageFormats;
        public ImagePixelFormat? AllowedPixelFormat;
        public int? RequiredChannelCount;
        public bool OnlyImages;
        public string OnImageUpdated;

        public ImageSlotAttribute(string tooltipText, string group, params ImageFormat[] allowedImageFormats) : this(tooltipText, group, null, null, allowedImageFormats)
        {
        }

        public ImageSlotAttribute(string tooltipText, string group, int? requiredChannelCount, ImagePixelFormat? allowedPixelFormat, params ImageFormat[] allowedImageFormats)
        {
            AllowedImageFormats = allowedImageFormats;
            AllowedPixelFormat = allowedPixelFormat;
            RequiredChannelCount = requiredChannelCount;
            TooltipText = tooltipText;
            Group = group;
        }
    }
}
