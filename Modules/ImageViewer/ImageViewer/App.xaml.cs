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
            MainViewModel mainViewModel = new MainViewModel();

            Action actionPictureTaken = () =>
            {
                Application.Current.Dispatcher.BeginInvoke
                    (DispatcherPriority.Background, new Action(
                        () =>
                        {
                            mainViewModel.MainImageSource = ReadForPictureTakenEvent();
                        })
                    );
            };

            Action actionImageCreated = () =>
            {
                Application.Current.Dispatcher.BeginInvoke
                    (DispatcherPriority.Background, new Action(
                        () =>
                        {
                            mainViewModel.MainImageSource = ReadForImageCreatedEvent();
                            HoloCommon.Synchronization.SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);
                        })
                    );
            };

            Thread threadPictureTaken = null;
            Thread threadImageCreated = null;

            if (e.Args.Length > 0)
            {
                if (e.Args.Contains(HoloCommon.Synchronization.Events.Camera.PICTURE_TAKEN))
                {
                    threadPictureTaken =
                        HoloCommon.Synchronization.SynchronizationManager.RunActionOnSignal(actionPictureTaken, HoloCommon.Synchronization.Events.Camera.PICTURE_TAKEN);
                }

                if (e.Args.Contains(HoloCommon.Synchronization.Events.Image.IMAGE_CREATED))
                {
                    threadImageCreated =
                        HoloCommon.Synchronization.SynchronizationManager.RunActionOnSignal(actionImageCreated, HoloCommon.Synchronization.Events.Image.IMAGE_CREATED);
                }
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.ViewModel = mainViewModel;

            mainWindow.Closed += (object ss, EventArgs args) =>
            {
                if (threadPictureTaken != null)
                {
                    threadPictureTaken.Abort();
                }

                if (threadImageCreated != null)
                {
                    threadImageCreated.Abort();
                }
            };

            mainWindow.Show();
        }
                
        private WriteableBitmap ReadForPictureTakenEvent()
        {
            System.Drawing.Bitmap image = MemoryReader.Read<System.Drawing.Bitmap>(new BitmapSerialization());

            BitmapSource bitmapSource =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                image.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapSource);

            return writeableBitmap;
        }

        private WriteableBitmap ReadForImageCreatedEvent()
        {
            WriteableBitmap writeableBitmap = MemoryReader.Read<WriteableBitmap>(new WriteableBitmapSerialization());
            return writeableBitmap;
        }
    }
}