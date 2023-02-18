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
                    TypeSizes.SIZE_DOUBLE * 2;
            }
        }
        
        public byte[] Serialize(ChartPoint obj)
        {
            List<byte> resBytes = new List<byte>();

            byte[] xBytes = null;
            byte[] yBytes = null;        

            if (obj != null)
            {
                xBytes = BitConverter.GetBytes(obj.X);
                yBytes = BitConverter.GetBytes(obj.Y);
            }
            else
            {
                xBytes = BitConverter.GetBytes(Double.MinValue);
                yBytes = BitConverter.GetBytes(Double.MinValue);
            }

            resBytes.AddRange(xBytes);
            resBytes.AddRange(yBytes);

            return resBytes.ToArray();
        }

        public ChartPoint Deserialize(byte[] bytes)
        {
            byte[] xBytes = new byte[TypeSizes.SIZE_DOUBLE];
            byte[] yBytes = new byte[TypeSizes.SIZE_DOUBLE];

            Array.Copy(bytes, 0, xBytes, 0, TypeSizes.SIZE_DOUBLE);
            Array.Copy(bytes, TypeSizes.SIZE_DOUBLE, yBytes, 0, TypeSizes.SIZE_DOUBLE);

            double x = BitConverter.ToDouble(xBytes, 0);
            double y = BitConverter.ToDouble(yBytes, 0);
                        
            ChartPoint chartPoint = null;
            
            if (x != Double.MinValue && y != Double.MinValue)
            {
                chartPoint = new ChartPoint(x, y);
            }

            return chartPoint;
        }
    }
}