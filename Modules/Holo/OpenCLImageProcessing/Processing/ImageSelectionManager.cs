using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;

namespace Processing
{
    // TODO: rename to repository?
    public class ImageSelectionManager
    {
        // TODO: может дикшенари и не нужен тут
        private Dictionary<IImageHandler, ImageSelection> _imageSelections = new Dictionary<IImageHandler, ImageSelection>();

        public void SetSelection(IImageHandler imageHandler, ImageSelection selection)
        {
            if (_imageSelections.ContainsKey(imageHandler))
                _imageSelections[imageHandler] = selection;
            else
                _imageSelections.Add(imageHandler, selection);
        }

        public bool TryGetSelection(IImageHandler imageHandler, out ImageSelection selection)
        {
            return _imageSelections.TryGetValue(imageHandler, out selection);
        }

        public bool HasSelection(IImageHandler imageHandler)
        {
            return TryGetSelection(imageHandler, out _);
        }

        public ImageSelection GetSelection(IImageHandler imageHandler)
        {
            if (!_imageSelections.TryGetValue(imageHandler, out ImageSelection selection))
                throw new InvalidOperationException("Отсутствует область выделения для заданного изображения.");

            return selection;
        }

        public IReadOnlyCollection<ImageSelection> GetAllSelections()
        {
            return _imageSelections.Values.Distinct().ToArray();
        }
    }
}