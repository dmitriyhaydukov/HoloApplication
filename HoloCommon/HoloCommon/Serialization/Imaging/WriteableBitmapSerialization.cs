using System;
using System.Windows.Media.Imaging;
using HoloCommon.MemoryManagement;

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
                        
            byte[] resBytes = new byte[TypeSizes.SIZE_INT * 2 + imageBytes.Length];
            byte[] widthBytes = BitConverter.GetBytes(width);
            byte[] heightBytes = BitConverter.GetBytes(height);

            Array.Copy(widthBytes, 0, resBytes, 0, widthBytes.Length);
            Array.Copy(heightBytes, 0, resBytes, TypeSizes.SIZE_INT, heightBytes.Length);
            Array.Copy(imageBytes, 0, resBytes, 2 * TypeSizes.SIZE_INT, imageBytes.Length);    

            return resBytes;
        }

        public WriteableBitmap Deserialize(byte[] bytes)
        {
            byte[] widthBytes = new byte[TypeSizes.SIZE_INT];
            byte[] heightBytes = new byte[TypeSizes.SIZE_INT];

            Array.Copy(bytes, 0, widthBytes, 0, TypeSizes.SIZE_INT);
            Array.Copy(bytes, TypeSizes.SIZE_INT, heightBytes, 0, TypeSizes.SIZE_INT);

            int width = BitConverter.ToInt32(widthBytes, 0);
            int height = BitConverter.ToInt32(heightBytes, 0);

            WriteableBitmap obj = BitmapFactory.New(width, height);
            int imageSize = width * height * (obj.Format.BitsPerPixel / 8);
                        
            obj.FromByteArray(bytes, 2 * TypeSizes.SIZE_INT, imageSize);

            return obj;
        }
    }
}