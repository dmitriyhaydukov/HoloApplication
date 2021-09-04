using System;
using System.IO;
using System.IO.Ports;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Linq;
using EDSDKLib;

namespace rab1
{
    public class CameraController
    {
        private SDKHandler CameraHandler = null;

        private short startImageNumber = 0;
        private short currentImageNumber = 0;
        private short groupNumber = 0;
        private ColorModeEnum colorMode = ColorModeEnum.Gray;

        private ImageForm imageForm = null;

        public Form1 MainForm { get; set; }

        //Events
        public PictureTakenHandler PictureTaken;
               
        public CameraController(SDKHandler sdkHandler, Form1 mainForm)
        {
            this.CameraHandler = sdkHandler;
            this.MainForm = mainForm;
        }

        private void SDK_ImageDownloaded(Bitmap bitmap, ImageType imageType)
        {
            if (PictureTaken != null)
            {
                short number = currentImageNumber;

                PictureTakenEventArgs eventArgs = new PictureTakenEventArgs()
                {
                    Image = bitmap,
                    StartImageNumber = startImageNumber,
                    Number = number,
                    GroupNumber = groupNumber,
                    ColorMode = colorMode
                };

                PictureTaken(eventArgs);
            }

            /*
            if (CameraHandler != null)
            {
                CameraHandler.Dispose();
                CameraHandler = null;
            }
            */
        }
        
        private void ShowBackgroundWindow(int imageNumber)
        {
            Screen currentScreen = Screen.FromControl(this.MainForm);
            Screen targetScreen = Screen.AllScreens.FirstOrDefault(s => !s.Equals(currentScreen)) ?? currentScreen;

            Point location = new Point();
            location.X = targetScreen.WorkingArea.Left;
            location.Y = targetScreen.WorkingArea.Top;

            this.imageForm = new ImageForm();
            this.imageForm.StartPosition = FormStartPosition.Manual;
            this.imageForm.Location = location;
            this.imageForm.WindowState = FormWindowState.Maximized;
            this.imageForm.Show();
            this.imageForm.SetImage(this.MainForm.GetImageFromPictureBox(imageNumber));
        }

        public void FastTakePhoto(short fromImageNumber, short toImageNumber)
        {
            //CameraHandler = new SDKHandler();

            //Handler of image download

            CameraHandler.ImageDownloaded -= new SDKHandler.BitmapUpdate(SDK_ImageDownloaded);
            CameraHandler.ImageDownloaded += new SDKHandler.BitmapUpdate(SDK_ImageDownloaded);
            
            List<Camera> cameraList = CameraHandler.GetCameraList();
            if (cameraList == null || cameraList.Count == 0)
            {
                return;
            }
                        
            Camera camera = cameraList[0];
            if (!CameraHandler.CameraSessionOpen)
            {
                CameraHandler.OpenSession(camera);
            }

            int delay = 1000;
            ShowBackgroundWindow(fromImageNumber);
            Thread.Sleep(delay);

            this.startImageNumber = 1;
            this.currentImageNumber = toImageNumber;

            //Transfer image to computer
            CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Host);
            CameraHandler.SetCapacity();

            //Take photo
            CameraHandler.TakePhoto();
            Thread.Sleep(delay);

            this.imageForm.Close();
            this.imageForm = null;
        }

        public void Dispose()
        {
            /*
            if (this.CameraHandler != null)
            {
                this.CameraHandler.Dispose();
            }
            */
        }
    }
}
