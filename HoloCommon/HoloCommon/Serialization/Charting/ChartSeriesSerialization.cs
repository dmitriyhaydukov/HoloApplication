using System;
using System.Collections.Generic;
using System.Linq;
using HoloCommon.MemoryManagement;
using HoloCommon.Models.Charting;

using HoloCommon.Interfaces;

namespace HoloCommon.Serialization.Charting
{
    public class ChartSeriesSerialization : IBinarySerialization<ChartSeries>
    {
        public byte[] Serialize(ChartSeries obj)
        {
            List<byte> resBytes = new List<byte>();
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
                Points = new List<ChartPoint>()
            };

            ChartPointSerialization cps = new ChartPointSerialization();
            int offset = 0;
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