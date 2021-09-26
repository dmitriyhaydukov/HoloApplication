using System;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using HoloCommon.MemoryManagement;

using HoloCommon.Interfaces;

namespace HoloCommon.Serialization.Imaging
{
    public class ImageSerialization : IBinarySerialization<Image>
    {
        public byte[] Serialize(Image obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                obj.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public Image Deserialize(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return Image.FromStream(stream);
            }
        }  
    }
}