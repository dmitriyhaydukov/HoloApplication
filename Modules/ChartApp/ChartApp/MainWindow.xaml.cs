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
using Microsoft.Win32;

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
        Crosshair currentCrossHair = null;
        bool isTracking = false;

        List<IPlottable> plottableList = null;
        List<ChartSeriesListItem> seriesViewList = null;

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
            this.plottableList = new List<IPlottable>();
            this.seriesViewList = new List<ChartSeriesListItem>();

            this.InitLegend();

            Chart chart = MemoryReader.Read<Chart>(new ChartSerialization());
            ChartSeries first = chart.SeriesCollection.First();

            double[] dataX = first.Points.Select(p => p.X).ToArray();
                        
            int count = chart.SeriesCollection.Count();
            for (int k = 0; k < count; k++)
            {
                IPlottable plottable = null;
                ChartSeries series = chart.SeriesCollection[k];
                switch(series.Type)
                {
                    case ChartSeriesType.Linear:
                        {
                            plottable = this.AddLinearPlot(series, dataX);
                            break;
                        }
                    case ChartSeriesType.Bubble:
                        {
                            plottable = this.AddBubblePlot(series);
                            break;
                        }
                }

                this.plottableList.Add(plottable);
                this.seriesViewList.Add(new ChartSeriesListItem() { Name = series.Name, IsVisible = true });
            }

            this.listViewSeries.ItemsSource = this.seriesViewList;
        }

        private IPlottable AddLinearPlot(ChartSeries series, double[] dataX)
        {
            double[] dataY = series.Points.Select(p => p.Y).ToArray();

            ScatterPlot scatterPlot = this.mainPlot.Plot.AddScatter(dataX, dataY);
            System.Drawing.Color color = GetColor(series.ColorDescriptor);
            scatterPlot.Color = color;
            scatterPlot.Label = series.Name;

            return scatterPlot;           
        }

        private IPlottable AddScatterPlot(ChartSeries series)
        {
            double[] dataX = series.Points.Select(p => p.X).ToArray();
            double[] dataY = series.Points.Select(p => p.Y).ToArray();

            ScatterPlot scatterPlot = this.mainPlot.Plot.AddScatter(dataX, dataY);
            System.Drawing.Color color = GetColor(series.ColorDescriptor);
            scatterPlot.Color = color;
            scatterPlot.Label = series.Name;

            return scatterPlot;           
        }

        private IPlottable AddBubblePlot(ChartSeries series)
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

            return bubblePlot;            
        }

        private void InitLegend()
        {
            Legend legend = this.mainPlot.Plot.Legend();
            legend.IsVisible = true;
            legend.FontName = "Arial";
            legend.FontSize = 12;
            legend.FontColor = System.Drawing.Color.DarkBlue;
        }

        private System.Drawing.Color GetColor(ColorDescriptor colorDescriptor)
        {
            int r = colorDescriptor.R;
            int g = colorDescriptor.G;
            int b = colorDescriptor.B;
            return System.Drawing.Color.FromArgb(r, g, b);
        }

        private void mainPlot_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isTracking)
            {
                IInputElement inputElement = sender as IInputElement;
                Point point = e.GetPosition(inputElement);

                double coordinateX = this.mainPlot.Plot.GetCoordinateX(Convert.ToSingle(point.X));
                double coordinateY = this.mainPlot.Plot.GetCoordinateY(Convert.ToSingle(point.Y));

                this.ClearCrossHair();
                this.currentCrossHair = this.mainPlot.Plot.AddCrosshair(coordinateX, coordinateY);
                this.currentCrossHair.LineColor = System.Drawing.Color.Black;
            }
        }

        private void ClearCrossHair()
        {
            if (this.currentCrossHair != null)
            {
                this.mainPlot.Plot.Remove(this.currentCrossHair);
                this.currentCrossHair = null;
            }
        }

        private void checkBoxIsTracking_Checked(object sender, RoutedEventArgs e)
        {
            this.isTracking = true;
        }

        private void checkBoxIsTracking_Unchecked(object sender, RoutedEventArgs e)
        {
            this.isTracking = false;
            this.ClearCrossHair();
        }

        private void menuItem_File_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            bool? dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult == true)
            {
                string filePath = saveFileDialog.FileName;                  
                this.mainPlot.Plot.SaveFig(filePath);
            }
        }

        private class ChartSeriesListItem
        {
            public string Name { get; set; }
            public bool IsVisible { get; set; }
        }
    }
   
}