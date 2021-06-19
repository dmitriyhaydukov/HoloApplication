using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

using HoloCommon.Serialization.Imaging;
using HoloCommon.MemoryManagement;

namespace ImageViewer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IEnumerable<WriteableBitmap> images = MemoryReader.ReadCollection(new WriteableBitmapSerialization());
            MainViewModel mainViewModel = new MainViewModel();
            mainViewModel.MainImageSource = images.First();

            MainWindow mainWindow = new MainWindow();
            mainWindow.ViewModel = mainViewModel;
            mainWindow.Show();
        }
    }
}