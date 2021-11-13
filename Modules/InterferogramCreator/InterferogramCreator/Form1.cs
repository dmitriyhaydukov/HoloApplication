using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.ImageProcessing;
using ExtraLibrary.OS;

using HoloCommon.MemoryManagement;
using HoloCommon.ProcessManagement;
using HoloCommon.Serialization.Imaging;
using HoloCommon.Synchronization;

using Interferometry.InterferogramCreation;

namespace InterferogramCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int width = 4096;
            int height = 2048;
            double percentNoise = 0;

            int fringeCount = 20;

            InterferogramInfo interferogramInfo = new InterferogramInfo(width, height, percentNoise); 
            LinearFringeInterferogramCreator interferogramCreator = new LinearFringeInterferogramCreator(interferogramInfo, fringeCount);

            double phaseShift = 0;
            RealMatrix interferogramMatrix = interferogramCreator.CreateInterferogram(phaseShift);

            WriteableBitmap writeableBitmap = 
                WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix(interferogramMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY);

            MemoryWriter.Write<WriteableBitmap>(writeableBitmap, new WriteableBitmapSerialization());
            //ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ImageViewer\ImageViewer\bin\Release\ImageViewer.exe", null, false);
            SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_CREATED);

        }
    }
}
