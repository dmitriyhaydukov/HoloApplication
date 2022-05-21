using System;
using System.Collections.Generic;
using HoloCommon.MemoryManagement;
using HoloCommon.Models.Charting;

using HoloCommon.Interfaces;
using HoloCommon.Serialization.General;

namespace HoloCommon.Serialization.Charting
{
    public class ChartPointSerialization : IBinarySerialization<ChartPoint>
    {
        public int SizeInBytes
        {
            get
            {
                return
                    TypeSizes.SIZE_DOUBLE * 2 +
                    TypeSizes.SIZE_BYTE * 3; //Color
            }
        }

        /*
        public byte[] Serialize(ChartPoint obj)
        {
            byte[] resBytes = new byte[TypeSizes.SIZE_DOUBLE * 2];

            byte[] xBytes = BitConverter.GetBytes(obj.X);
            byte[] yBytes = BitConverter.GetBytes(obj.Y);

            Array.Copy(xBytes, 0, resBytes, 0, xBytes.Length);
            Array.Copy(yBytes, 0, resBytes, TypeSizes.SIZE_DOUBLE, yBytes.Length);           

            return resBytes;
        }
        */
        
        public byte[] Serialize(ChartPoint obj)
        {
            List<byte> resBytes = new List<byte>();

            byte[] xBytes = BitConverter.GetBytes(obj.X);
            byte[] yBytes = BitConverter.GetBytes(obj.Y);

            //Color
            //ColorDescriptorSerialization cds = new ColorDescriptorSerialization();
            //byte[] colorBytes = cds.Serialize(obj.Color);

            resBytes.AddRange(xBytes);
            resBytes.AddRange(yBytes);
            //resBytes.AddRange(colorBytes);

            return resBytes.ToArray();
        }

        public ChartPoint Deserialize(byte[] bytes)
        {
            byte[] xBytes = new byte[TypeSizes.SIZE_DOUBLE];
            byte[] yBytes = new byte[TypeSizes.SIZE_DOUBLE];

            //byte[] colorBytes = 

            Array.Copy(bytes, 0, xBytes, 0, TypeSizes.SIZE_DOUBLE);
            Array.Copy(bytes, TypeSizes.SIZE_DOUBLE, yBytes, 0, TypeSizes.SIZE_DOUBLE);

            double x = BitConverter.ToDouble(xBytes, 0);
            double y = BitConverter.ToDouble(yBytes, 0);

            ChartPoint chartPoint = new ChartPoint(x, y);
            
            return chartPoint;
        }
    }
}