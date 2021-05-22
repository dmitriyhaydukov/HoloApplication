using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using HoloCommon.MemoryManagement;
using HoloCommon.Serialization.Imaging;

namespace ImageProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteableBitmap image = MemoryReader.Read<WriteableBitmap>(new WriteableBitmapSerialization());
            image = image.Rotate(90);
            MemoryWriter.Write<WriteableBitmap>(image, new WriteableBitmapSerialization());
        }
    }
}
