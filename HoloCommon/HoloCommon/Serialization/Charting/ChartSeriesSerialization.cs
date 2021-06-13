using System;
using System.Collections.Generic;
using System.Linq;
using HoloCommon.MemoryManagement;
using HoloCommon.Models.Charting;

using HoloCommon.Interfaces;
using HoloCommon.Serialization.Text;

namespace HoloCommon.Serialization.Charting
{
    public class ChartSeriesSerialization : IBinarySerialization<ChartSeries>
    {
        public byte[] Serialize(ChartSeries obj)
        {
            List<byte> resBytes = new List<byte>();

            StringSerialization ss = new StringSerialization();
            byte[] nameBytes = ss.Serialize(obj.Name);
            byte[] lengthBytes = BitConverter.GetBytes(nameBytes.Length);
            resBytes.AddRange(lengthBytes);
            resBytes.AddRange(nameBytes);

            ChartPointSerialization cps = new ChartPointSerialization();
            for (int k = 0; k < obj.Points.Count(); k++)
            {
                ChartPoint point = obj.Points[k];    
                byte[] pointBytes = cps.Serialize(point);
                resBytes.AddRange(pointBytes);
            }
            return resBytes.ToArray();
        }

        public ChartSeries Deserialize(byte[] bytes)
        {
            ChartSeries chartSeries = new ChartSeries()
            {
                Name = null,
                Points = new List<ChartPoint>()
            };

            StringSerialization ss = new StringSerialization();
            byte[] nameLengthBytes = new byte[TypeSizes.SIZE_INT];
            Array.Copy(bytes, 0, nameLengthBytes, 0, TypeSizes.SIZE_INT);
            int length = BitConverter.ToInt32(nameLengthBytes, 0);

            byte[] nameBytes = new byte[length];
            Array.Copy(bytes, TypeSizes.SIZE_INT, nameBytes, 0, length);
            string name = ss.Deserialize(nameBytes);

            chartSeries.Name = name;

            ChartPointSerialization cps = new ChartPointSerialization();
            int offset = nameBytes.Length + TypeSizes.SIZE_INT;
            while (offset < bytes.Count())
            {
                byte[] pointBytes = new byte[cps.SizeInBytes];
                Array.Copy(bytes, offset, pointBytes, 0, cps.SizeInBytes);
                ChartPoint point = cps.Deserialize(pointBytes);
                chartSeries.Points.Add(point);
                offset += cps.SizeInBytes;
            }
            return chartSeries;
        }
    }
}