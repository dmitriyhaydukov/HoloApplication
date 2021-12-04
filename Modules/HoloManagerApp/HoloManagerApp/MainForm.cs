using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

using HoloCommon.ProcessManagement;
using HoloCommon.Synchronization;

namespace HoloManagerApp
{
    public partial class MainForm : Form
    {
        private const int PICTURE_TAKEN_DELAY = 2000;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnCorrectedGraph_Click(object sender, EventArgs e)
        {
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\CorrectedGraph\CorrectedGraph\bin\Debug\CorrectedGraph.exe", null, false);
        }

        private void btnCreateInterferogram_Click(object sender, EventArgs e)
        {
            double phaseShift = 0;
            string arguments = phaseShift.ToString(CultureInfo.InvariantCulture);

            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\InterferogramCreatorConsoleApp\InterferogramCreatorConsoleApp\bin\Debug\InterferogramCreatorConsoleApp.exe", arguments, false);
        }

        private void btnTakePicture_Click(object sender, EventArgs e)
        {
            SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Camera.TAKE_PICTURE);
        }       
        private void btnTakeSeries_Click(object sender, EventArgs e)
        {
            double phaseShift = 0;
            double phaseShiftStep = GetPhaseShiftStep();

            Action takePictureAction = () =>
            {
                Thread.Sleep(PICTURE_TAKEN_DELAY);
                SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Camera.TAKE_PICTURE);
            };

            Action pictureTakenAction = () =>
            {
                if (phaseShift < 2 * Math.PI)
                {
                    CreateInterferogram(phaseShift);
                    phaseShift += phaseShiftStep;
                }
            };

            Thread thread1 = SynchronizationManager.RunActionOnSignal(pictureTakenAction, HoloCommon.Synchronization.Events.Image.IMAGE_SAVED);
            Thread thread2 = SynchronizationManager.RunActionOnSignal(takePictureAction, HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);

            SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);
        }
        

        /*
        private void btnTakeSeries_Click(object sender, EventArgs e)
        {
            double phaseShift = 0;
            double phaseShiftStep = GetPhaseShiftStep();

            Action action = () =>
            {
                Thread.Sleep(PICTURE_TAKEN_DELAY);
                if (phaseShift < 2 * Math.PI)
                {
                    CreateInterferogram(phaseShift);
                    phaseShift += phaseShiftStep;
                }
            };
            
            Thread thread2 = SynchronizationManager.RunActionOnSignal(action, HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);

            SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);
        }
        */

        private void CreateInterferogram(double phaseShift)
        {
            string arguments = phaseShift.ToString(CultureInfo.InvariantCulture);
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\InterferogramCreatorConsoleApp\InterferogramCreatorConsoleApp\bin\Debug\InterferogramCreatorConsoleApp.exe", arguments, false, false);
        }

        private double GetPhaseShiftStep()
        {
            return  2 * Math.PI / 220;
        }
    }
}
