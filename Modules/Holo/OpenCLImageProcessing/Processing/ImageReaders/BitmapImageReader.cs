using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Processing.ImageReaders
{
    public class BitmapImageReader: ImageReaderBase
    {
        public override string FormatFilter => "*.bmp;*.jpg;*.png;*.jpeg";
        public override string FormatDescription => "Bitmap images";

        public override ImageHandler Read(BinaryReader reader)
        {
            var bmp = new Bitmap(reader.BaseStream);
            return ImageHandler.FromBitmapAsGreyscale(bmp); // ImageHandler.FromBitmap(bmp);
        }

        public override bool CanRead(BinaryReader reader)
        {
            return true;
        }
    }
}
