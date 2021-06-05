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

using LiveCharts;
using LiveCharts.Wpf;

using HoloCommon.Models.Charting;
using HoloCommon.MemoryManagement;
using HoloCommon.Serialization.Charting;


namespace ChartApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitChart();
        }

        private void InitChart()
        {
            ReadSeriesCollection();
        }

        private void ReadSeriesCollection()
        {
            Chart chart = MemoryReader.Read<Chart>(new ChartSerialization());
            ChartSeries first = chart.SeriesCollection.First();

            SeriesCollection lvSeriesCollection = new SeriesCollection();
            for (int k = 0; k < chart.SeriesCollection.Count(); k++)
            {
                ChartSeries chartSeries = chart.SeriesCollection[k];
                LineSeries lineSeries = new LineSeries()
                {
                    Values = new ChartValues<double>(chartSeries.Points.Select(x => x.Y)),
                    PointGeometry = null
                };
                lvSeriesCollection.Add(lineSeries);
            }

            Labels = first.Points.Select(x => x.X.ToString()).ToArray();
            SeriesCollection = lvSeriesCollection;
            YFormatter = value => value.ToString();
        }
    }
}