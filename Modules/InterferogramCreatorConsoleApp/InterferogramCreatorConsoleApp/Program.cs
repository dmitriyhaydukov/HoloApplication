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
            if (args.Length != 3)
            {
                return;
            }

            double phaseShift = double.Parse(args[0], CultureInfo.InvariantCulture);

            double? maxRange = null;
            int? moduleValue = null;
            
            double parsedMaxRange;
            int parsedModuleValue;
            
            if (double.TryParse(args[1], out parsedMaxRange))
            {
                maxRange = parsedMaxRange;
            }

            if (int.TryParse(args[2], out parsedModuleValue))
            {
                moduleValue = parsedModuleValue;
            }

            int width = 4096;
            int height = 1024;
            double percentNoise = 0;

            int fringeCount = 10;
            double minIntensity = 0;

            InterferogramInfo interferogramInfo = new InterferogramInfo(width, height, percentNoise, minIntensity, maxRange, moduleValue);
            LinearFringeInterferogramCreator interferogramCreator = new LinearFringeInterferogramCreator(interferogramInfo, fringeCount);
                        
            RealMatrix interferogramMatrix = interferogramCreator.CreateInterferogram(phaseShift);

            WriteableBitmap writeableBitmap =
                WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix(interferogramMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY);

            MemoryWriter.Write<WriteableBitmap>(writeableBitmap, new WriteableBitmapSerialization());
                        
            SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_CREATED);
        }
    }
}
