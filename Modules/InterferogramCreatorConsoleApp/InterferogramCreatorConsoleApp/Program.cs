using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Globalization;

using HoloCommon.Serialization.Imaging;
using HoloCommon.MemoryManagement;
using HoloCommon.Synchronization;

using ExtraLibrary.OS;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.ImageProcessing;
using Interferometry.InterferogramCreation;

namespace InterferogramCreatorConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            double phaseShift = double.Parse(args[0], CultureInfo.InvariantCulture);

            int width = 2048;
            int height = 512;
            double percentNoise = 0;

            int fringeCount = 10;
            double minIntensity = 35;

            InterferogramInfo interferogramInfo = new InterferogramInfo(width, height, percentNoise, minIntensity);
            LinearFringeInterferogramCreator interferogramCreator = new LinearFringeInterferogramCreator(interferogramInfo, fringeCount);
                        
            RealMatrix interferogramMatrix = interferogramCreator.CreateInterferogram(phaseShift);

            WriteableBitmap writeableBitmap =
                WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix(interferogramMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY);

            MemoryWriter.Write<WriteableBitmap>(writeableBitmap, new WriteableBitmapSerialization());
                        
            SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_CREATED);
        }
    }
}
