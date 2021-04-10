using System.Windows.Media.Imaging;

using HoloCommon.Serialization.Imaging;
using HoloCommon.MemoryManagement;

using ImageReader.Imaging;

namespace ImageReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"D:\Images\canon_eos_m50_02.jpg";
            WriteableBitmap writeableBitmap = WriteableBitmapCreator.CreateWriteableBitmapFromFile(filePath);
            MemoryWriter.Write(writeableBitmap, new WriteableBitmapSerialization());
        }
    }
}
