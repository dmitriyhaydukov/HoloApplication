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
            byte[] resBytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                obj.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                resBytes = stream.ToArray();
            }

            return resBytes; 
        }

        public Image Deserialize(byte[] bytes)
        {
            Image resImage = null;
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                resImage = Image.FromStream(stream);
            }
            return resImage;
        }  
    }
}