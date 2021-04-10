using System;
using System.Windows.Media.Imaging;

using HoloCommon.Interfaces;

namespace HoloCommon.Serialization.Imaging
{
    public class WriteableBitmapSerialization : IBinarySerialization<WriteableBitmap>
    {
        public byte[] Serialize(WriteableBitmap obj)
        {
            byte[] imageBytes = obj.ToByteArray();

            int width = obj.PixelWidth;
            int height = obj.PixelHeight;

            int intSize = sizeof(int);

            byte[] resBytes = new byte[intSize * 2 + imageBytes.Length];
            byte[] widthBytes = BitConverter.GetBytes(width);
            byte[] heightBytes = BitConverter.GetBytes(height);

            Array.Copy(widthBytes, 0, resBytes, 0, widthBytes.Length);
            Array.Copy(heightBytes, 0, resBytes, intSize, heightBytes.Length);
            Array.Copy(imageBytes, 0, resBytes, 2 * intSize, imageBytes.Length);          

            return resBytes;
        }

        public WriteableBitmap Deserialize(byte[] bytes)
        {
            int intSize = sizeof(int);

            byte[] widthBytes = new byte[intSize];
            byte[] heightBytes = new byte[intSize];

            Array.Copy(bytes, 0, widthBytes, 0, intSize);
            Array.Copy(bytes, intSize, heightBytes, 0, intSize);

            int width = BitConverter.ToInt32(widthBytes, 0);
            int height = BitConverter.ToInt32(heightBytes, 0);

            WriteableBitmap obj = BitmapFactory.New(width, height);
            int imageSize = width * height * (obj.Format.BitsPerPixel / 8);
            
            byte[] imageBytes = new byte[imageSize];
            Array.Copy(bytes, 2 * intSize, imageBytes, 0, imageSize);

            obj.FromByteArray(imageBytes);

            return obj;
        }
    }
}