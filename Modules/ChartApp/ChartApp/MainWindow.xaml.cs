using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.MemoryMappedFiles;

using HoloCommon.Models.Charting;
using HoloCommon.Models.General;
using HoloCommon.Enumeration.Charting;
using HoloCommon.MemoryManagement;
using HoloCommon.Serialization.Charting;

using ScottPlot;
using ScottPlot.Plottable;
using ScottPlot.Renderable;

namespace ChartApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitChart();
        }

        private void InitChart()
        {
            ReadChart();
        }

        private void ReadChart()
        {
            this.InitLegend();

            Chart chart = MemoryReader.Read<Chart>(new ChartSerialization());
            ChartSeries first = chart.SeriesCollection.First();

            double[] dataX = first.Points.Select(p => p.X).ToArray();
                        
            int count = chart.SeriesCollection.Count();
            for (int k = 0; k < count; k++)
            {
                ChartSeries series = chart.SeriesCollection[k];
                switch(series.Type)
                {
                    case ChartSeriesType.Linear:
                        {
                            this.AddLinearPlot(series, dataX);
                            break;
                        }
                    case ChartSeriesType.Bubble:
                        {
                            this.AddBubblePlot(series);
                            break;
                        }
                }                         
            }          
        }

        private void AddLinearPlot(ChartSeries series, double[] dataX)
        {
            double[] dataY = series.Points.Select(p => p.Y).ToArray();

            ScatterPlot scatterPlot = this.mainPlot.Plot.AddScatter(dataX, dataY);
            System.Drawing.Color color = GetColor(series.ColorDescriptor);
            scatterPlot.Color = color;
            scatterPlot.Label = series.Name;
        }
                
        private void AddBubblePlot(ChartSeries series)
        {
            BubblePlot bubblePlot = this.mainPlot.Plot.AddBubblePlot();
            System.Drawing.Color color = GetColor(series.ColorDescriptor);
            double radius = 1;

            int count = series.Points.Count();
            for (int j = 0; j < count; j++)
            {
                ChartPoint point = series.Points[j];
                bubblePlot.Add(point.X, point.Y, radius, color, 0, color);
            }
        }

        private void InitLegend()
        {
            Legend legend = this.mainPlot.Plot.Legend();
            legend.IsVisible = true;
            legend.FontName = "comic sans ms";
            legend.FontSize = 18;
            legend.FontColor = System.Drawing.Color.DarkBlue;
        }

        private System.Drawing.Color GetColor(ColorDescriptor colorDescriptor)
        {
            int r = colorDescriptor.R;
            int g = colorDescriptor.G;
            int b = colorDescriptor.B;
            return System.Drawing.Color.FromArgb(r, g, b);
        }
    }
}