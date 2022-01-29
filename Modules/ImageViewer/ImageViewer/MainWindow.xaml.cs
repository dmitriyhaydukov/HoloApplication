using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Forms;
using System.IO;

using ExtraLibrary.ImageProcessing;
using ExtraLibrary.OS;

using HoloCommon.Models.Charting;
using HoloCommon.Models.General;

using HoloCommon.Enumeration.Charting;
using HoloCommon.Serialization.Imaging;
using HoloCommon.Serialization.Charting;
using HoloCommon.MemoryManagement;
using HoloCommon.ProcessManagement;

namespace ImageViewer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mainViewModel;

        //private string CHART_APP_PATH = @"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe";
        private string CHART_APP_PATH = @"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Debug\ChartApp.exe";

        public MainWindow()
        {
            InitializeComponent();    
        }

        public MainViewModel ViewModel
        {
            set
            {
                this.mainViewModel = value;
                this.DataContext = this.mainViewModel;
            }
        }

        private void mainImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Image imageControl = sender as Image;
                Point point = e.GetPosition(imageControl);
                int row = (int)point.Y;
                                
                WriteableBitmap mainBitmap = this.mainViewModel.MainImageSource as WriteableBitmap;
                
                double[] yValues = GetRowGrayScaleValues(mainBitmap, row);

                Chart chart = new Chart()
                {
                    SeriesCollection = new List<ChartSeries>()
                };

                ChartSeries series = new ChartSeries()
                {
                    Name = "Row " + row.ToString(),
                    Type = ChartSeriesType.Linear,
                    ColorDescriptor = new ColorDescriptor(255, 0, 0),
                    Points = new List<ChartPoint>()
                };

                for (int x = 0; x < yValues.Length; x++)
                {
                    ChartPoint chartPoint = new ChartPoint(x, yValues[x]);
                    series.Points.Add(chartPoint);
                }

                chart.SeriesCollection.Add(series);

                MemoryWriter.Write<Chart>(chart, new ChartSerialization());
                ProcessManager.RunProcess(CHART_APP_PATH, null, false);
            }
        }

        public static double[] GetRowGrayScaleValues(WriteableBitmap bitmap, int row)
        {
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create(bitmap);
            if (wrapper.IsFormatGrayScale)
            {
                double[] grayScaleValues = wrapper.GetRowGrayValues(row);
                return grayScaleValues;
            }
            else
            {
                Color[] rowColors = wrapper.GetRowColors(row);
                double[] grayScaleValues = new double[rowColors.Length];
                for (int index = 0; index < rowColors.Length; index++)
                {
                    Color color = rowColors[index];
                    double grayIntensity = ColorWrapper.GetGrayIntensity(color);
                    grayScaleValues[index] = grayIntensity;
                }
                return grayScaleValues;
            }
        }

        private void mainImage_Loaded(object sender, RoutedEventArgs e)
        {
            //HoloCommon.Synchronization.SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);
        }

        private void mainImage_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            //HoloCommon.Synchronization.SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);
        }

        private void saveImageButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"D:\Images\!\Image.png";

            BitmapSource image = mainViewModel.MainImageSource;
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image as BitmapSource));
                encoder.Save(fileStream);
            }
        }

        private void loadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;

                double dpiX = OS.SystemDpiX;
                double dpiY = OS.SystemDpiY;

                ExtraImageInfo extraImageInfo = WriteableBitmapCreator.CreateWriteableBitmapFromFile(fileName, dpiX, dpiY);
                mainViewModel.MainImageSource = extraImageInfo.Image;
            }
        }
    }
}
