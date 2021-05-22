using System;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

using HoloCommon.Serialization.Imaging;
using HoloCommon.MemoryManagement;

using ImageReader.Imaging;

namespace ImageReader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("File apth for iamge is not specified");
            }

            List<WriteableBitmap> images = new List<WriteableBitmap>();
            for (int k = 0; k < args.Length; k++)
            {
                string filePath = args[k];
                WriteableBitmap writeableBitmap = WriteableBitmapCreator.CreateWriteableBitmapFromFile(filePath);
                images.Add(writeableBitmap);
            }

            //Write images to memory
            MemoryWriter.WriteCollection<WriteableBitmap>(images, new WriteableBitmapSerialization());
        }
    }
}
