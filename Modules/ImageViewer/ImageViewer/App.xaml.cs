using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Threading;

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
            //IEnumerable<WriteableBitmap> images = MemoryReader.ReadCollection(new WriteableBitmapSerialization());
            //MainViewModel mainViewModel = new MainViewModel();
            //mainViewModel.MainImageSource = images.First();          
            
            MainViewModel mainViewModel = new MainViewModel();

            Action action = () =>
            {
                Application.Current.Dispatcher.BeginInvoke
                    (DispatcherPriority.Background, new Action(
                        () =>
                        {
                            mainViewModel.MainImageSource = ReadImage();
                        })
                    );
            };

            Thread threadCamera = HoloCommon.Synchronization.SynchronizationManager.RunActionOnSignal(action, HoloCommon.Synchronization.Events.Camera.PICTURE_TAKEN);
            Thread threadImage = HoloCommon.Synchronization.SynchronizationManager.RunActionOnSignal(action, HoloCommon.Synchronization.Events.Image.IMAGE_CREATED);

            MainWindow mainWindow = new MainWindow();
            mainWindow.ViewModel = mainViewModel;

            mainWindow.Closed += (object ss, EventArgs args) =>
            {
                threadCamera.Abort();
                threadImage.Abort();
            };

            mainWindow.Show();
        }
                
        private WriteableBitmap ReadImage()
        {
            System.Drawing.Bitmap image = MemoryReader.Read<System.Drawing.Bitmap>(new BitmapSerialization());

            System.Windows.Media.Imaging.BitmapSource bitmapSource =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                image.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapSource);

            return writeableBitmap;
        }
    }
}