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
using System.IO;

using HoloCommon.Serialization.Imaging;
using HoloCommon.MemoryManagement;

using ExtraLibrary.ImageProcessing;

namespace ImageViewer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string DEFAULT_SAVE_IMAGE_PATH = @"d:\Images\!\";

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = e.Args;

            MainViewModel mainViewModel = new MainViewModel();

            int imageNumber = 1;

            Action actionPictureTaken = () =>
            {
                Application.Current.Dispatcher.BeginInvoke
                    (DispatcherPriority.Background, new Action(
                        () =>
                        {
                            WriteableBitmap bitmap = ReadForPictureTakenEvent();
                            mainViewModel.MainImageSource = bitmap;

                            if (args.Contains(HoloCommon.Synchronization.Events.Action.SAVE_IMAGE))
                            {
                                WriteableBitmapWrapper bitmapWrapper = WriteableBitmapWrapper.Create(bitmap);
                                string fileName = Path.Combine(DEFAULT_SAVE_IMAGE_PATH, imageNumber.ToString() + ".png");
                                bitmapWrapper.SaveToPngFile(fileName);
                                imageNumber++;
                                HoloCommon.Synchronization.SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_SAVED);
                            }
                        })
                    );
            };

            Action actionImageCreated = () =>
            {
                Application.Current.Dispatcher.BeginInvoke
                    (DispatcherPriority.Background, new Action(
                        () =>
                        {
                            WriteableBitmap bitmap = ReadForImageCreatedEvent();
                            mainViewModel.MainImageSource = bitmap;
                            HoloCommon.Synchronization.SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);
                            
                            /*
                            WriteableBitmapWrapper bitmapWrapper = WriteableBitmapWrapper.Create(bitmap);
                            string fileName = Path.Combine(DEFAULT_SAVE_IMAGE_PATH, imageNumber.ToString() + ".png");
                            bitmapWrapper.SaveToPngFile(fileName);
                            imageNumber++;
                            HoloCommon.Synchronization.SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_SAVED);
                            */

                        })
                    );
            };

            Thread threadPictureTaken = null;
            Thread threadImageCreated = null;

            MainWindow mainWindow = new MainWindow();
            mainWindow.ViewModel = mainViewModel;

            if (e.Args.Length > 0)
            {
                mainWindow.Title = string.Join(" ", e.Args);

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

            mainWindow.Closed += (object ss, EventArgs eventArgs) =>
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