using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processing.ImageReaders
{
    public static class ImageReaderProvider
    {
        public static IEnumerable<IImageReader> GetAll()
        {
            return new[]
            {
                new BitmapImageReader()
            };
        }
    }
}
