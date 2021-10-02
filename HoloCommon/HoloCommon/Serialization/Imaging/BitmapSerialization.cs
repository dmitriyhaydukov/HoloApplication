using System;
using System.IO;
using System.Drawing;
using HoloCommon.MemoryManagement;

using HoloCommon.Interfaces;

namespace HoloCommon.Serialization.Imaging
{
    public class BitmapSerialization : IBinarySerialization<Bitmap>
    {
        public byte[] Serialize(Bitmap obj)
        {
            byte[] resBytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                obj.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                resBytes = stream.ToArray();
            }

            return resBytes; 
        }

        public Bitmap Deserialize(byte[] bytes)
        {
            Bitmap resBitmap = null;
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                Image image = Image.FromStream(stream);
                resBitmap = new Bitmap(image);
            }
            return resBitmap;
        }
    }
}