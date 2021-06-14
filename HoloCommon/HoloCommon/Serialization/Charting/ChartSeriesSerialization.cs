using System;
using System.Collections.Generic;
using System.Linq;
using HoloCommon.MemoryManagement;
using HoloCommon.Models.Charting;
using HoloCommon.Models.General;

using HoloCommon.Enumeration.Charting;

using HoloCommon.Interfaces;
using HoloCommon.Serialization.Text;
using HoloCommon.Serialization.General;

namespace HoloCommon.Serialization.Charting
{
    public class ChartSeriesSerialization : IBinarySerialization<ChartSeries>
    {
        public byte[] Serialize(ChartSeries obj)
        {
            List<byte> resBytes = new List<byte>();

            //Name
            StringSerialization ss = new StringSerialization();
            byte[] nameBytes = ss.Serialize(obj.Name);
            byte[] lengthBytes = BitConverter.GetBytes(nameBytes.Length);
            resBytes.AddRange(lengthBytes);
            resBytes.AddRange(nameBytes);

            //Type
            byte[] typeBytes = BitConverter.GetBytes((short)obj.Type);
            resBytes.AddRange(typeBytes);

            //Color
            ColorDescriptorSerialization cds = new ColorDescriptorSerialization();
            byte[] colorBytes = cds.Serialize(obj.ColorDescriptor);
            resBytes.AddRange(colorBytes);

            //Points
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
                ColorDescriptor = null,
                Points = new List<ChartPoint>()
            };

            //Name
            StringSerialization ss = new StringSerialization();
            byte[] nameLengthBytes = new byte[TypeSizes.SIZE_INT];
            Array.Copy(bytes, 0, nameLengthBytes, 0, TypeSizes.SIZE_INT);
            int length = BitConverter.ToInt32(nameLengthBytes, 0);

            byte[] nameBytes = new byte[length];
            Array.Copy(bytes, TypeSizes.SIZE_INT, nameBytes, 0, length);
            string name = ss.Deserialize(nameBytes);

            int typeOffset = TypeSizes.SIZE_INT + nameBytes.Length;

            //Type
            byte[] typeBytes = new byte[TypeSizes.SIZE_SHORT];
            Array.Copy(bytes, typeOffset, typeBytes, 0, TypeSizes.SIZE_SHORT);
            ChartSeriesType seriesType = (ChartSeriesType)BitConverter.ToInt16(typeBytes, 0);

            int colorOffset = typeOffset + TypeSizes.SIZE_SHORT;

            //Color
            ColorDescriptorSerialization cds = new ColorDescriptorSerialization();
            byte[] colorBytes = new byte[cds.SizeInBytes];
            Array.Copy(bytes, colorOffset, colorBytes, 0, cds.SizeInBytes);
            ColorDescriptor colorDescriptor = cds.Deserialize(colorBytes);

            //Points
            ChartPointSerialization cps = new ChartPointSerialization();
            int offset = colorOffset + cds.SizeInBytes;
            while (offset < bytes.Count())
            {
                byte[] pointBytes = new byte[cps.SizeInBytes];
                Array.Copy(bytes, offset, pointBytes, 0, cps.SizeInBytes);
                ChartPoint point = cps.Deserialize(pointBytes);
                chartSeries.Points.Add(point);
                offset += cps.SizeInBytes;
            }

            chartSeries.Name = name;
            chartSeries.ColorDescriptor = colorDescriptor;

            return chartSeries;
        }
    }
}