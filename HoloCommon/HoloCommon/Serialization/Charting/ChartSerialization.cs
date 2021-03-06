using System;
using System.Collections.Generic;
using System.Linq;
using HoloCommon.MemoryManagement;
using HoloCommon.Models.Charting;

using HoloCommon.Interfaces;

namespace HoloCommon.Serialization.Charting
{
    public class ChartSerialization : IBinarySerialization<Chart>
    {
        public byte[] Serialize(Chart obj)
        {
            List<byte> resBytes = new List<byte>();
            int seriesCount = obj.SeriesCollection.Count();
            byte[] seriesCountBytes = BitConverter.GetBytes(seriesCount);

            //series count
            resBytes.AddRange(seriesCountBytes);

            ChartSeriesSerialization css = new ChartSeriesSerialization();
            foreach(ChartSeries series in obj.SeriesCollection)
            {
                byte[] seriesBytes = css.Serialize(series);
                
                //length of series in bytes
                resBytes.AddRange(BitConverter.GetBytes(seriesBytes.Length));

                //series bytes
                resBytes.AddRange(seriesBytes);
            }
                        
            byte[] invertAxisXBytes = BitConverter.GetBytes(obj.InvertAxisX);
            byte[] invertAxisYBytes = BitConverter.GetBytes(obj.InvertAxisY);

            resBytes.AddRange(invertAxisXBytes);
            resBytes.AddRange(invertAxisYBytes);
            
            return resBytes.ToArray();
        }

        public Chart Deserialize(byte[] bytes)
        {
            Chart chart = new Chart()
            {
                SeriesCollection = new List<ChartSeries>()
            };

            ChartSeriesSerialization css = new ChartSeriesSerialization();

            byte[] seriesCountBytes = new byte[TypeSizes.SIZE_INT];
            Array.Copy(bytes, 0, seriesCountBytes, 0, TypeSizes.SIZE_INT);
            int seriesCount = BitConverter.ToInt32(seriesCountBytes, 0);

            int offset = TypeSizes.SIZE_INT;
            for(int k = 0; k < seriesCount; k++)
            {
                byte[] seriesSizeBytes = new byte[TypeSizes.SIZE_INT];
                Array.Copy(bytes, offset, seriesSizeBytes, 0, TypeSizes.SIZE_INT);
                int seriesSize = BitConverter.ToInt32(seriesSizeBytes, 0);

                offset += TypeSizes.SIZE_INT;
                byte[] seriesBytes = new byte[seriesSize];
                Array.Copy(bytes, offset, seriesBytes, 0, seriesSize);

                ChartSeries series = css.Deserialize(seriesBytes);
                chart.SeriesCollection.Add(series);

                offset += seriesSize;
            }

            byte[] invertAxisXBytes = new byte[TypeSizes.SIZE_BOOL];
            byte[] invertAxisYBytes = new byte[TypeSizes.SIZE_BOOL];

            Array.Copy(bytes, offset, invertAxisXBytes, 0, TypeSizes.SIZE_BOOL);
            offset += TypeSizes.SIZE_BOOL;
            Array.Copy(bytes, offset, invertAxisYBytes, 0, TypeSizes.SIZE_BOOL);

            chart.InvertAxisX = BitConverter.ToBoolean(invertAxisXBytes, 0);
            chart.InvertAxisY = BitConverter.ToBoolean(invertAxisYBytes, 0);

            return chart;
        }
    }
}
