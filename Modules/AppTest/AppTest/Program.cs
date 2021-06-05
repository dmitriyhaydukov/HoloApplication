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
                {
                    new ChartPoint(1, 8),
                    new ChartPoint(2, 2),
                    new ChartPoint(3, 3),
                    new ChartPoint(4, 1),
                    new ChartPoint(5, 2),
                    new ChartPoint(6, 7)
                }
            };

            chart.SeriesCollection = new List<ChartSeries>()
            {
                chartSeries
            };

            ChartSerialization cs = new ChartSerialization();
            MemoryWriter.Write<Chart>(chart, cs);           
        }
    }
}
