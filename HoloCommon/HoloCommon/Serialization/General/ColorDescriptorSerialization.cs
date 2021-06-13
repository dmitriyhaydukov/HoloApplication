using System;
using HoloCommon.MemoryManagement;
using HoloCommon.Models.General;

using HoloCommon.Interfaces;

namespace HoloCommon.Serialization.General
{
    public class ColorDescriptorSerialization : IBinarySerialization<ColorDescriptor>
    {
        public int SizeInBytes
        {
            get
            {
                return TypeSizes.SIZE_BYTE * 3;
            }
        }

        public byte[] Serialize(ColorDescriptor obj)
        {
            byte[] resBytes = new byte[SizeInBytes];

            resBytes[0] = obj.R;
            resBytes[1] = obj.G;
            resBytes[2] = obj.B;

            return resBytes;
        }

        public ColorDescriptor Deserialize(byte[] bytes)
        {
            byte r = bytes[0];
            byte g = bytes[1];
            byte b = bytes[2];
                        
            ColorDescriptor colorDescriptor = new ColorDescriptor(r, g, b);
            
            return colorDescriptor;
        }
    }
}