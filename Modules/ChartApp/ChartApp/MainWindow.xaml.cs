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
using HoloCommon.MemoryManagement;
using HoloCommon.Serialization.Charting;

using ScottPlot;
using ScottPlot.Plottable;

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
            Chart chart = MemoryReader.Read<Chart>(new ChartSerialization());
            ChartSeries first = chart.SeriesCollection.First();

            double[] dataX = first.Points.Select(p => p.X).ToArray();

            double annotationCoordinateX = 10;
            double annotationCoordinateY = 0;

            int count = chart.SeriesCollection.Count();
            for (int k = 0; k < count; k++)
            {
                ChartSeries series = chart.SeriesCollection[k];
                double[] dataY = series.Points.Select(p => p.Y).ToArray();

                ScatterPlot scatterPlot = this.mainPlot.Plot.AddScatter(dataX, dataY);
                int r = series.ColorDescriptor.R;
                int g = series.ColorDescriptor.G;
                int b = series.ColorDescriptor.B;

                System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);

                scatterPlot.Color = color;
                
                Annotation annotation = this.mainPlot.Plot.AddAnnotation(series.Name, annotationCoordinateX, annotationCoordinateY);
                annotation.BackgroundColor = color;
                annotation.Shadow = false;

                annotationCoordinateY += 25;
            }
        }
    }
}