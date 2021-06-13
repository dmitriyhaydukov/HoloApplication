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
            ChartSeries chartSeries1 = new ChartSeries() { Name = "Sin", Points = new List<ChartPoint>() };
            ChartSeries chartSeries2 = new ChartSeries() { Name = "Cos", Points = new List<ChartPoint>() };

            Random rnd = new Random();
            
            int count = 50;
            for (double x = 0; x < count; x += 0.1)
            {
                ChartPoint point1 = new ChartPoint(x, Math.Sin(x));
                chartSeries1.Points.Add(point1);

                ChartPoint point2 = new ChartPoint(x, Math.Cos(x));
                chartSeries2.Points.Add(point2);
            }
                        
            chart.SeriesCollection = new List<ChartSeries>()
            {
                chartSeries1,
                chartSeries2
            };

            ChartSerialization cs = new ChartSerialization();
            MemoryWriter.Write<Chart>(chart, cs);           
        }
    }
}
