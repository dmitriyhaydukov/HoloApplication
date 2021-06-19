using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

using HoloCommon.Serialization.Imaging;
using HoloCommon.MemoryManagement;

using ExtraLibrary.ImageProcessing;
using ExtraLibrary.OS;

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
                ExtraImageInfo imageInfo = CreateImageFromFile(filePath);
                images.Add(imageInfo.Image);
            }

            //Write images to memory
            MemoryWriter.WriteCollection<WriteableBitmap>(images, new WriteableBitmapSerialization());
        }

        public static ExtraImageInfo CreateImageFromFile(string fileName)
        {
            WriteableBitmap resultImage = null;

            double dpiX = OS.SystemDpiX;
            double dpiY = OS.SystemDpiY;

            ExtraImageInfo extraImageInfo = WriteableBitmapCreator.CreateWriteableBitmapFromFile(fileName, dpiX, dpiY);

            bool isImageFormatGrayScale = WriteableBitmapWrapper.IsImageFormatGrayScale(extraImageInfo.Image);

            if (isImageFormatGrayScale)
            {
                resultImage = extraImageInfo.Image;
            }
            else if (extraImageInfo.Image.Format != PixelFormats.Bgra32)
            {
                PixelFormat pixelFormat = PixelFormats.Bgra32;
                WriteableBitmap newImage = WriteableBitmapConverter.ConvertWriteableBitmap(extraImageInfo.Image, pixelFormat);
                resultImage = newImage;
                extraImageInfo.Image = resultImage;
            }

            return extraImageInfo;
        }
    }
}
