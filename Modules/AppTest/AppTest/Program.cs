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

        }

        static void TestCharting()
        {
            Chart chart = new Chart();
            ChartSeries chartSeries = new ChartSeries()
            {
                Points = new List<ChartPoint>()
                {
                    new ChartPoint(1, 1),
                    new ChartPoint(2, 2),
                    new ChartPoint(3, 3)
                }
            };

            chart.SeriesCollection = new List<ChartSeries>()
            {
                chartSeries
            };

            ChartSerialization cs = new ChartSerialization();
            MemoryWriter.Write<Chart>(chart, cs);

            chart = MemoryReader.Read<Chart>(cs);
        }
    }
}
