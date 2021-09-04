using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Processing;
using Processing.DataBinding;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class ImageSlotBinder : PropertyBindingBase
    {
        private MethodInfo _onImageUpdated;
        private readonly ImageSlotControl _imageSlot;
        private IImageHandler _imageSlotValue;

        public override Control Control => _imageSlot;

        public ImageSlotBinder(ImageSlotAttribute imageSlotAttribute, MemberInfo memberInfo,
            object target) : base(imageSlotAttribute, memberInfo, target)
        {
            Group = imageSlotAttribute.Group;

            if (imageSlotAttribute.OnImageUpdated != null)
                _onImageUpdated = target.GetType().GetMethod(imageSlotAttribute.OnImageUpdated);

            _imageSlot = new ImageSlotControl
            {
                Title = imageSlotAttribute.TooltipText,
                Value = (IImageHandler) _propertyInfo.GetValue(Target),
                AllowedImageFormats = imageSlotAttribute.AllowedImageFormats.ToList(),
                AllowedPixelFormats = imageSlotAttribute.AllowedPixelFormat.HasValue
                    ? new List<ImagePixelFormat>(new[] {imageSlotAttribute.AllowedPixelFormat.Value})
                    : new List<ImagePixelFormat>(),
                RequiredChannelCount = imageSlotAttribute.RequiredChannelCount
            };

            _imageSlot.OnValueChanged += () =>
            {
                if (_imageSlotValue != _imageSlot.Value && _imageSlotValue != null)
                    _imageSlotValue.ImageUpdated -= ImageUpdated;

                _imageSlotValue = _imageSlot.Value;

                if (_imageSlotValue != null)
                    _imageSlotValue.ImageUpdated += ImageUpdated;
                    
                _propertyInfo.SetValue(Target, _imageSlot.Value);
                OnPropertyChanged();
            };
        }

        public override void Set(object value)
        {
            _imageSlot.Value = (IImageHandler) value;
        }

        protected override void OnPropertyChangedByTarget()
        {
            _imageSlot.Value = (IImageHandler) _propertyInfo.GetValue(Target);
        }

        public void ImageUpdated(ImageUpdatedEventData e)
        {
            _onImageUpdated?.Invoke(Target, new object [] {});
        }
    }
}