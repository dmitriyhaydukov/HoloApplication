using System;
using HoloCommon.MemoryManagement;
using HoloCommon.Models.Charting;

using HoloCommon.Interfaces;

namespace HoloCommon.Serialization.Charting
{
    public class ChartPointSerialization : IBinarySerialization<ChartPoint>
    {
        public int SizeInBytes
        {
            get
            {
                return TypeSizes.SIZE_DOUBLE * 2;
            }
        }

        public byte[] Serialize(ChartPoint obj)
        {
            byte[] resBytes = new byte[TypeSizes.SIZE_DOUBLE * 2];

            byte[] xBytes = BitConverter.GetBytes(obj.X);
            byte[] yBytes = BitConverter.GetBytes(obj.Y);

            Array.Copy(xBytes, 0, resBytes, 0, xBytes.Length);
            Array.Copy(yBytes, 0, resBytes, TypeSizes.SIZE_DOUBLE, yBytes.Length);

            return resBytes;
        }

        public ChartPoint Deserialize(byte[] bytes)
        {
            byte[] xBytes = new byte[TypeSizes.SIZE_DOUBLE];
            byte[] yBytes = new byte[TypeSizes.SIZE_DOUBLE];

            Array.Copy(bytes, 0, xBytes, 0, TypeSizes.SIZE_DOUBLE);
            Array.Copy(bytes, TypeSizes.SIZE_DOUBLE, yBytes, 0, TypeSizes.SIZE_DOUBLE);

            double x = BitConverter.ToDouble(xBytes, 0);
            double y = BitConverter.ToDouble(yBytes, 0);

            ChartPoint chartPoint = new ChartPoint(x, y);
            
            return chartPoint;
        }
    }
}