using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HoloCommon.Models.Charting;
using HoloCommon.Serialization.Charting;
using HoloCommon.MemoryManagement;

namespace AppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteChart();
        }

        static void WriteChart()
        {
            Chart chart = new Chart();
            ChartSeries chartSeries = new ChartSeries()
            {
                Points = new List<ChartPoint>()
            };
            Random rnd = new Random();
            
            int count = 6000;
            int maxValue = 256;
            for (int x = 0; x < count; x++)
            {
                ChartPoint point = new ChartPoint(x, rnd.Next(maxValue));
                chartSeries.Points.Add(point);
            }
                        
            chart.SeriesCollection = new List<ChartSeries>()
            {
                chartSeries
            };

            ChartSerialization cs = new ChartSerialization();
            MemoryWriter.Write<Chart>(chart, cs);           
        }
    }
}
