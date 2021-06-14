using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HoloCommon.Enumeration.Charting;
using HoloCommon.Models.Charting;
using HoloCommon.Models.General;
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
            ChartSeries chartSeries1 = new ChartSeries()
            {
                Name = "Sin",
                Type = ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(255, 0, 0),
                Points = new List<ChartPoint>()
            };

            ChartSeries chartSeries2 = new ChartSeries()
            {
                Name = "Cos",
                Type = ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(0, 255, 0),
                Points = new List<ChartPoint>()
            };

            ChartSeries chartSeries3 = new ChartSeries()
            {
                Name = "scatter",
                Type = ChartSeriesType.Bubble,
                ColorDescriptor = new ColorDescriptor(0, 0, 255),
                Points = new List<ChartPoint>()
            };

            Random rnd = new Random();
            
            int count = 100000;
            for (double x = 0; x < count; x += 0.1)
            {
                ChartPoint point1 = new ChartPoint(x, Math.Sin(x));
                chartSeries1.Points.Add(point1);

                ChartPoint point2 = new ChartPoint(x, Math.Cos(x));
                chartSeries2.Points.Add(point2);

                ChartPoint point3 = new ChartPoint(x, x * rnd.NextDouble());
                chartSeries3.Points.Add(point3);
            }

            chart.SeriesCollection = new List<ChartSeries>()
            {
                //chartSeries1,
                //chartSeries2,
                chartSeries3
            };

            ChartSerialization cs = new ChartSerialization();
            MemoryWriter.Write<Chart>(chart, cs);           
        }
    }
}
