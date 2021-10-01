using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using HoloCommon.Synchronization;

namespace Camera
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CameraForm cameraForm = new CameraForm();

            Action takePictureAction = () =>
            {
                cameraForm.TakePhoto();
            };

            Thread thread = SynchronizationManager.RunActionOnSignal(takePictureAction, Events.Camera.TAKE_PICTURE);

            cameraForm.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                thread.Abort();
            };

            Application.Run(cameraForm);
        }       
    }
}
