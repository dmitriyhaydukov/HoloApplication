using System;
using System.IO;
using System.IO.Ports;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Drawing.Imaging;

using EDSDKLib;
using HoloCommon.MemoryManagement;
using HoloCommon.Serialization.Imaging;

namespace Camera
{
    public partial class CameraForm : Form
    {
        #region Constants

        private const int FRAMES_256_MIN_INTENSITY = 70;

        #endregion

        #region Variables

        TakePhotoSeriesTypeEnum seriesType = TakePhotoSeriesTypeEnum.Uknown;

        PhaseShiftDeviceController phaseShiftDeviceController = null;
        double phaseShiftStep = 0;
        short phaseShiftCount = 0;
        short currentPhaseShiftNumber = 0;
        double currentPhaseShiftValue = 0;
        short zeroPhaseShiftValue = 0x2000;
        int delay = 0;
        bool takeNextPhoto = false;
        bool executeNextShift = false;
        bool isMakePhaseShiftsProcess = false;
        bool isLiveViewHandled = true;
        ColorModeEnum colorMode = ColorModeEnum.Uknown;
        ImageSaveModeEnum imageSaveMode = ImageSaveModeEnum.Uknown;

        SDKHandler CameraHandler;
        List<int> AvList;
        List<int> TvList;
        List<int> ISOList;
        List<EDSDKLib.Camera> CamList;
        Bitmap Evf_Bmp;
        int LVBw, LVBh; 
        
        int ErrCount;
        object ErrLock = new object();

        short groupNumber = 3;

        ImageForm imageForm = null;
        short startImageNumber = 0;
        short currentImageNumber = 0;
        float imageOffsetX = 0;
        float imageOffsetStep = 0;

        int imageWidth = 0;
        int imageHeight = 0;

        IList<Image> seriesImagesList = null;

        bool is256Frames = false;
        string frames256Directory = null;
        int count256Frames = 256 + 256; //512

        #endregion
                
        #region Events

        public PictureTakenHandler PictureTaken;
        public LiveViewUpdatedHandler LiveViewUpdated;

        #endregion

        public CameraForm()
        {
            try
            {
                InitializeComponent();
                CameraHandler = new SDKHandler();
                CameraHandler.CameraAdded += new SDKHandler.CameraAddedHandler(SDK_CameraAdded);
                CameraHandler.LiveViewUpdated += new SDKHandler.StreamUpdate(SDK_LiveViewUpdated);
                CameraHandler.ProgressChanged += new SDKHandler.ProgressHandler(SDK_ProgressChanged);
                CameraHandler.ImageDownloaded += new SDKHandler.BitmapUpdate(SDK_ImageDownloaded);
                CameraHandler.ImageSavedToDirectory += new EventHandler(SDK_ImageSavedToDirectory);
                CameraHandler.CameraHasShutdown += SDK_CameraHasShutdown;
                SavePathTextBox.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RemotePhoto");
                LVBw = LiveViewPicBox.Width;
                LVBh = LiveViewPicBox.Height;
                RefreshCamera();
            }
            catch (DllNotFoundException) { ReportError("Canon DLLs not found!", true); }
            catch (Exception ex) { ReportError(ex.Message, true); }

            try
            {
                InitSerialPorts();
            }
            catch(Exception ex)
            {
                ReportError(ex.Message, true);
            }
            
            InitColorModes();
            InitImageSaveModes();
            InitializeDefaultValues();
   
        }

        private void InitializeDefaultValues()
        {
            phaseShiftCountTextBox.Text = "4";
            phaseShiftStepTextBox.Text = "100";
            DelayTextBox.Text = "1500";
            ShiftsCountTextBox.Text = "4";
            startImageNumberTextBox.Text = "1";
        }
        
        private void InitColorModes()
        {
            colorComboBox.ValueMember = "ColorModeValue";
            colorComboBox.DisplayMember = "ColorModeName";
            colorComboBox.SelectedIndexChanged += colorComboBox_SelectedIndexChanged;


            colorComboBox.Items.Add(new ColorItem() { ColorModeValue = ColorModeEnum.Gray, ColorModeName = "Gray" });
            colorComboBox.Items.Add(new ColorItem() { ColorModeValue = ColorModeEnum.Red, ColorModeName = "Red" });
            colorComboBox.Items.Add(new ColorItem() { ColorModeValue = ColorModeEnum.Green, ColorModeName = "Green" });
            colorComboBox.Items.Add(new ColorItem() { ColorModeValue = ColorModeEnum.Blue, ColorModeName = "Blue" });

            colorComboBox.SelectedIndex = 0;
        }
                
        private void InitImageSaveModes()
        {
            imageSaveComboBox.ValueMember = "ImageSaveModeValue";
            imageSaveComboBox.DisplayMember = "ImageSaveModeName";
            imageSaveComboBox.SelectedIndexChanged += imageSaveComboBox_SelectedIndexChanged;

            imageSaveComboBox.Items.Add(new ImageSaveItem() { ImageSaveModeValue = ImageSaveModeEnum.Memory, ImageSaveModeName = "Memory" });
            imageSaveComboBox.Items.Add(new ImageSaveItem() { ImageSaveModeValue = ImageSaveModeEnum.Directory, ImageSaveModeName = "Directory" });
            
            imageSaveComboBox.SelectedIndex = 0;
        }
        
        private void colorComboBox_SelectedIndexChanged(object sender, EventArgs args)
        {
            if (colorComboBox.SelectedItem != null)
            {
                colorMode = ((ColorItem)colorComboBox.SelectedItem).ColorModeValue;
            }
        }

        private void imageSaveComboBox_SelectedIndexChanged(object sender, EventArgs args)
        {
            if (imageSaveComboBox.SelectedItem != null)
            {
                imageSaveMode = ((ImageSaveItem)imageSaveComboBox.SelectedItem).ImageSaveModeValue;
            }
        }

        private void InitSerialPorts()
        {
            string[] portNames = SerialPort.GetPortNames();
            for (int index = 0; index < portNames.Length; index++)
            {
                string portName = portNames[index];
                phaseShiftSerialPortComboBox.Items.Add(portName);
            }
        }

        private void CameraHandler_ImageDownloaded(Bitmap bmp)
        {
            throw new NotImplementedException();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { if (CameraHandler != null) CameraHandler.Dispose(); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        #region SDK Events

        private void SDK_ProgressChanged(int Progress)
        {
            try
            {
                if (Progress == 100) Progress = 0;
                this.Invoke((Action)delegate { MainProgressBar.Value = Progress; });
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void SDK_ImageDownloaded(Bitmap bitmap, ImageType imageType)
        {
            if (PictureTaken != null)
            {
                short number = 0; 

                if (seriesType == TakePhotoSeriesTypeEnum.ImageSeries)
                {
                    number = currentImageNumber;
                }
                if (seriesType == TakePhotoSeriesTypeEnum.PhaseShifts)
                {
                    number = currentPhaseShiftNumber;
                }

                if (!is256Frames)
                {

                    PictureTakenEventArgs eventArgs = new PictureTakenEventArgs()
                    {
                        Image = bitmap,
                        StartImageNumber = startImageNumber,
                        Number = number,
                        GroupNumber = groupNumber,
                        PhaseShiftValue = Convert.ToInt16(currentPhaseShiftValue),
                        ColorMode = colorMode
                    };

                    PictureTaken(eventArgs);
                }
                else
                {
                    //Save bitmap
                    string fileName = string.Format("{0}.jpg", currentImageNumber.ToString());
                    string filePath = Path.Combine(frames256Directory, fileName);

                    Bitmap image = new Bitmap(bitmap);
                    image.Save(filePath);
                   
                }
            }

            TryTakeNextPhoto();
        }

        private void SDK_ImageSavedToDirectory(object sender, EventArgs args)
        {
            if (PictureTaken != null)
            {
                PictureTaken(null);
            }

            TryTakeNextPhoto();
        }
        
        private void TryTakeNextPhoto()
        {
            if (takeNextPhoto)
            {
                if (seriesType == TakePhotoSeriesTypeEnum.PhaseShifts)
                {
                    currentPhaseShiftNumber++;
                    currentPhaseShiftValue += phaseShiftStep;
                    if (currentPhaseShiftNumber == phaseShiftCount)
                    {
                        takeNextPhoto = false;
                    }
                    else
                    {
                        takeNextPhoto = true;
                    }

                    if (currentPhaseShiftNumber <= phaseShiftCount)
                    {
                        ExecutePhaseShiftAndTakePhoto();
                    }
                }

                if (seriesType == TakePhotoSeriesTypeEnum.ImageSeries)
                {
                    currentImageNumber++;

                    if (is256Frames)
                    {
                        currentPhaseShiftValue += phaseShiftStep;
                    }

                    if (currentImageNumber == GetMaxImagesCount())
                    {
                        takeNextPhoto = false;
                    }
                    else
                    {
                        takeNextPhoto = true;
                    }

                    if (currentImageNumber <= GetMaxImagesCount())
                    {
                        SetImageAndTakePhoto();
                    }
                }
            }
        }
        
        private void SDK_LiveViewUpdated(Stream img)
        {
            if (!isLiveViewHandled) return;

            isLiveViewHandled = false;

            try
            {
                Evf_Bmp = new Bitmap(img);

                if (LiveViewUpdated != null && isMakePhaseShiftsProcess)
                {
                    LiveViewUpdatedEventArgs args = new LiveViewUpdatedEventArgs()
                    {
                        Image = Evf_Bmp,
                        PhaseShiftNumber = currentPhaseShiftNumber,
                        PhaseShiftValue = Convert.ToInt16(currentPhaseShiftValue),
                        ColorMode = colorMode
                    };

                    LiveViewUpdated(args);
    
                }
                else
                {
                    using (Graphics g = LiveViewPicBox.CreateGraphics())
                    {
                        g.DrawImage(Evf_Bmp, 0, 0, Evf_Bmp.Width, Evf_Bmp.Height);
                    }
                }
                
                if (isMakePhaseShiftsProcess)
                {
                    if (executeNextShift)
                    {
                        currentPhaseShiftNumber++;
                        currentPhaseShiftValue += phaseShiftStep;
                        if (currentPhaseShiftNumber == phaseShiftCount)
                        {
                            executeNextShift = false;
                            isMakePhaseShiftsProcess = false;
                        }
                        else
                        {
                            executeNextShift = true;
                        }

                        if (currentPhaseShiftNumber <= phaseShiftCount)
                        {
                            ExecutePhaseShift();
                        }
                    }
                }

            }
            catch (Exception ex) { ReportError(ex.Message, false); }

            isLiveViewHandled = true;
        }

        private void SDK_CameraAdded()
        {
            try { RefreshCamera(); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void SDK_CameraHasShutdown(object sender, EventArgs e)
        {
            try { CloseSession(); }
            catch (Exception ex) { ReportError(ex.Message, false); }

        }

        #endregion

        #region Session

        private void SessionButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (CameraHandler.CameraSessionOpen) CloseSession();
                else OpenSession();
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            try { RefreshCamera(); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        #endregion

        #region Settings

                
        private void TakePhotoButton_Click(object sender, EventArgs e)
        {
            try
            {
                currentPhaseShiftNumber = 0;
                currentImageNumber = 0;
                imageOffsetX = 0;

                if ((string)TvCoBox.SelectedItem == "Bulb") CameraHandler.TakePhoto((uint)BulbUpDo.Value);
                else CameraHandler.TakePhoto();
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void RecordVideoButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CameraHandler.IsFilming)
                {
                    if (STComputerButton.Checked || STBothButton.Checked)
                    {
                        Directory.CreateDirectory(SavePathTextBox.Text);
                        CameraHandler.StartFilming(SavePathTextBox.Text);
                    }
                    else CameraHandler.StartFilming();
                    RecordVideoButton.Text = "Stop Video";
                }
                else
                {
                    CameraHandler.StopFilming();
                    RecordVideoButton.Text = "Record Video";
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(SavePathTextBox.Text)) SaveFolderBrowser.SelectedPath = SavePathTextBox.Text;
                if (SaveFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    SavePathTextBox.Text = SaveFolderBrowser.SelectedPath;
                    CameraHandler.ImageSaveDirectory = SavePathTextBox.Text;
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void AvCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { CameraHandler.SetSetting(EDSDK.PropID_Av, CameraValues.AV((string)AvCoBox.SelectedItem)); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void TvCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CameraHandler.SetSetting(EDSDK.PropID_Tv, CameraValues.TV((string)TvCoBox.SelectedItem));
                if ((string)TvCoBox.SelectedItem == "Bulb") BulbUpDo.Enabled = true;
                else BulbUpDo.Enabled = false;
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void ISOCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { CameraHandler.SetSetting(EDSDK.PropID_ISOSpeed, CameraValues.ISO((string)ISOCoBox.SelectedItem)); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void WBCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (WBCoBox.SelectedIndex)
                {
                    case 0: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Auto); break;
                    case 1: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Daylight); break;
                    case 2: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Cloudy); break;
                    case 3: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Tangsten); break;
                    case 4: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Fluorescent); break;
                    case 5: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Strobe); break;
                    case 6: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_WhitePaper); break;
                    case 7: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Shade); break;
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void SaveToButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (STCameraButton.Checked)
                {
                    CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Camera);
                    BrowseButton.Enabled = false;
                    SavePathTextBox.Enabled = false;
                }
                else
                {
                    if (STComputerButton.Checked) CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Host);
                    else if (STBothButton.Checked) CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Both);
                    CameraHandler.SetCapacity();
                    BrowseButton.Enabled = true;
                    SavePathTextBox.Enabled = true;
                    Directory.CreateDirectory(SavePathTextBox.Text);
                    CameraHandler.ImageSaveDirectory = SavePathTextBox.Text;
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        
        #endregion

        #region Live view

        private void LiveViewButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CameraHandler.IsLiveViewOn) { CameraHandler.StartLiveView(); LiveViewButton.Text = "Stop LV"; }
                else { CameraHandler.StopLiveView(); LiveViewButton.Text = "Start LV"; }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void LiveViewPicBox_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (CameraHandler.IsLiveViewOn && CameraHandler.IsCoordSystemSet)
                {
                    ushort x = (ushort)((e.X / (double)LiveViewPicBox.Width) * CameraHandler.Evf_CoordinateSystem.width);
                    ushort y = (ushort)((e.Y / (double)LiveViewPicBox.Height) * CameraHandler.Evf_CoordinateSystem.height);
                    CameraHandler.SetManualWBEvf(x, y);
                }
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void LiveViewPicBox_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                LVBw = LiveViewPicBox.Width;
                LVBh = LiveViewPicBox.Height;
            }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusNear3Button_Click(object sender, EventArgs e)
        {
            try { CameraHandler.SetFocus(EDSDK.EvfDriveLens_Near3); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusNear2Button_Click(object sender, EventArgs e)
        {
            try { CameraHandler.SetFocus(EDSDK.EvfDriveLens_Near2); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusNear1Button_Click(object sender, EventArgs e)
        {
            try { CameraHandler.SetFocus(EDSDK.EvfDriveLens_Near1); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusFar1Button_Click(object sender, EventArgs e)
        {
            try { CameraHandler.SetFocus(EDSDK.EvfDriveLens_Far1); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusFar2Button_Click(object sender, EventArgs e)
        {
            try { CameraHandler.SetFocus(EDSDK.EvfDriveLens_Far2); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        private void FocusFar3Button_Click(object sender, EventArgs e)
        {
            try { CameraHandler.SetFocus(EDSDK.EvfDriveLens_Far3); }
            catch (Exception ex) { ReportError(ex.Message, false); }
        }

        #endregion

        #region Subroutines

        private void CloseSession()
        {
            CameraHandler.CloseSession();
            AvCoBox.Items.Clear();
            TvCoBox.Items.Clear();
            ISOCoBox.Items.Clear();
            SettingsGroupBox.Enabled = false;
            LiveViewGroupBox.Enabled = false;
            SessionButton.Text = "Open Session";
            SessionLabel.Text = "No open session";
            RefreshCamera();//Closing the session invalidates the current camera pointer
        }

        private void RefreshCamera()
        {
            CameraListBox.Items.Clear();
            CamList = CameraHandler.GetCameraList();
            foreach (EDSDKLib.Camera cam in CamList) CameraListBox.Items.Add(cam.Info.szDeviceDescription);
            if (CameraHandler.CameraSessionOpen) CameraListBox.SelectedIndex = CamList.FindIndex(t => t.Ref == CameraHandler.MainCamera.Ref);
            else if (CamList.Count > 0) CameraListBox.SelectedIndex = 0;
        }

        private void OpenSession()
        {
            if (CameraListBox.SelectedIndex >= 0)
            {
                CameraHandler.OpenSession(CamList[CameraListBox.SelectedIndex]);
                SessionButton.Text = "Close Session";
                string cameraname = CameraHandler.MainCamera.Info.szDeviceDescription;
                SessionLabel.Text = cameraname;
                if (CameraHandler.GetSetting(EDSDK.PropID_AEMode) != EDSDK.AEMode_Manual) MessageBox.Show("Camera is not in manual mode. Some features might not work!");
                AvList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_Av);
                TvList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_Tv);
                ISOList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_ISOSpeed);
                foreach (int Av in AvList) AvCoBox.Items.Add(CameraValues.AV((uint)Av));
                foreach (int Tv in TvList) TvCoBox.Items.Add(CameraValues.TV((uint)Tv));
                foreach (int ISO in ISOList) ISOCoBox.Items.Add(CameraValues.ISO((uint)ISO));
                AvCoBox.SelectedIndex = AvCoBox.Items.IndexOf(CameraValues.AV((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_Av)));
                TvCoBox.SelectedIndex = TvCoBox.Items.IndexOf(CameraValues.TV((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_Tv)));
                ISOCoBox.SelectedIndex = ISOCoBox.Items.IndexOf(CameraValues.ISO((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_ISOSpeed)));
                int wbidx = (int)CameraHandler.GetSetting((uint)EDSDK.PropID_WhiteBalance);
                switch (wbidx)
                {
                    case EDSDK.WhiteBalance_Auto: WBCoBox.SelectedIndex = 0; break;
                    case EDSDK.WhiteBalance_Daylight: WBCoBox.SelectedIndex = 1; break;
                    case EDSDK.WhiteBalance_Cloudy: WBCoBox.SelectedIndex = 2; break;
                    case EDSDK.WhiteBalance_Tangsten: WBCoBox.SelectedIndex = 3; break;
                    case EDSDK.WhiteBalance_Fluorescent: WBCoBox.SelectedIndex = 4; break;
                    case EDSDK.WhiteBalance_Strobe: WBCoBox.SelectedIndex = 5; break;
                    case EDSDK.WhiteBalance_WhitePaper: WBCoBox.SelectedIndex = 6; break;
                    case EDSDK.WhiteBalance_Shade: WBCoBox.SelectedIndex = 7; break;
                    default: WBCoBox.SelectedIndex = -1; break;
                }
                SettingsGroupBox.Enabled = true;
                LiveViewGroupBox.Enabled = true;
            }
        }

        private void ReportError(string message, bool lockdown)
        {
            int errc;
            lock (ErrLock) { errc = ++ErrCount; }

            if (lockdown) EnableUI(false);

            if (errc < 4) MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (errc == 4) MessageBox.Show("Many errors happened!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            lock (ErrLock) { ErrCount--; }
        }

        private void EnableUI(bool enable)
        {
            if (InvokeRequired) Invoke((Action)delegate { EnableUI(enable); });
            else
            {
                SettingsGroupBox.Enabled = enable;
                InitGroupBox.Enabled = enable;
                LiveViewGroupBox.Enabled = enable;
            }
        }

        #endregion

        private void takeSeriesPhotoButton_Click(object sender, EventArgs e)
        {
            TakePhaseShiftsSeriesPhoto();
        }

        private void initSerialPortButton_Click(object sender, EventArgs e)
        {
            InitPhaseShiftDeviceController();
            initSerialPortButton.Enabled = false;
            closephaseShiftSerialPortButton.Enabled = true;
        }
        
        private void InitPhaseShiftDeviceController()
        {
            if (phaseShiftSerialPortComboBox != null && phaseShiftSerialPortComboBox.SelectedItem != null)
            {
                string serialPort = phaseShiftSerialPortComboBox.SelectedItem.ToString();

                phaseShiftDeviceController = new PhaseShiftDeviceController(serialPort);
                phaseShiftDeviceController.Initialize();
            }
        }
        
        private void DisposePhaseShiftDeviceController()
        {
            if (phaseShiftDeviceController != null)
            {
                phaseShiftDeviceController.Dispose();
                phaseShiftDeviceController = null;
            }
        }
        
        private void TakePhaseShiftsSeriesPhoto()
        {
            seriesType = TakePhotoSeriesTypeEnum.PhaseShifts;
            is256Frames = false;

            InitPhaseShiftParameters();
            takeNextPhoto = true;
            ExecutePhaseShiftAndTakePhoto();
        }

        private void TakeImagesSeriesPhoto()
        {
            seriesType = TakePhotoSeriesTypeEnum.ImageSeries;
            is256Frames = false;
            
            currentImageNumber = 1;
            imageOffsetX = 0;

            startImageNumber = short.Parse(startImageNumberTextBox.Text);
            delay = int.Parse(DelayTextBox.Text);
                        
            takeNextPhoto = true;
            SetImageAndTakePhoto();
        }

        private void Take256Images()
        {
            seriesType = TakePhotoSeriesTypeEnum.ImageSeries;
            is256Frames = true;

            currentPhaseShiftNumber = 1;
            currentPhaseShiftValue = 0;
            phaseShiftStep = Math.Round(360.0 / 256.0, 2);
            currentImageNumber = 1;
            imageOffsetX = 0;

            startImageNumber = short.Parse(startImageNumberTextBox.Text);
            delay = int.Parse(DelayTextBox.Text);

            frames256Directory = frames256DirectoryTextBox.Text;
            if (!Directory.Exists(frames256Directory))
            {
                Directory.CreateDirectory(frames256Directory);
            }

            takeNextPhoto = true;
            SetImageAndTakePhoto();
        }

        private void ExecutePhaseShiftAndTakePhoto()
        {
            ExecutePhaseShift();
            TakePhoto();
        }
                
        private void SetImageAndTakePhoto()
        {
            SetImage();
            MakeDelay();
            TakePhoto();
        }
                
        private void SetImage()
        {
            if (is256Frames)
            {
                double phaseShiftInRadians = Math.PI * currentPhaseShiftValue / 180.0;
                Image image = GenerateImage(phaseShiftInRadians);

                imageForm.SetImage(image);
            }
            else
            {
                Image image = MemoryReader.Read(new ImageSerialization());
                imageForm.SetImage(image);
            }
        }

        private int GetMaxImagesCount()
        {
            if (is256Frames)
            {
                return count256Frames;
            }
            else
            {
                return int.Parse(ShiftsCountTextBox.Text);
            }
        }

        private void ExecutePhaseShift()
        {
            if (phaseShiftDeviceController != null)
            {
                phaseShiftDeviceController.SetShift(Convert.ToInt16(currentPhaseShiftValue));
                //currentPhaseShiftLabel.Text = currentPhaseShiftValue.ToString();
            }

            MakeDelay();
        }

        private void TakePhoto()
        {
            CameraHandler.TakePhoto();
        }

        private void ClosePhaseShiftSerialPort()
        {
            DisposePhaseShiftDeviceController();
            initSerialPortButton.Enabled = true;
            closephaseShiftSerialPortButton.Enabled = false;
        }
        
        private void closephaseShiftSerialPortButton_Click(object sender, EventArgs e)
        {
            ClosePhaseShiftSerialPort();    
        }

        private void MakePhaseShiftsButton_Click(object sender, EventArgs e)
        {
            MakePhaseShifts();
        }

        private void backGroundWindowButton_Click(object sender, EventArgs e)
        {
            ShowBackgroundWindow(1);
        }

        private void ShowBackgroundWindow(int imageNumber)
        {
            Screen currentScreen = Screen.FromControl(this);
            Screen targetScreen = Screen.AllScreens.FirstOrDefault(s => !s.Equals(currentScreen)) ?? currentScreen;

            Point location = new Point();
            location.X = targetScreen.WorkingArea.Left;
            location.Y = targetScreen.WorkingArea.Top;

            this.imageForm = new ImageForm();
            this.imageForm.StartPosition = FormStartPosition.Manual;
            this.imageForm.Location = location;
            this.imageForm.WindowState = FormWindowState.Maximized;
            this.imageForm.Show();

            Image image = MemoryReader.Read(new ImageSerialization());

            this.imageForm.SetImage(image);
        }

        private void ShowBackgroundFromMainImage()
        {
            Screen currentScreen = Screen.FromControl(this);
            Screen targetScreen = Screen.AllScreens.FirstOrDefault(s => !s.Equals(currentScreen)) ?? currentScreen;

            Point location = new Point();
            location.X = targetScreen.WorkingArea.Left;
            location.Y = targetScreen.WorkingArea.Top;

            this.imageForm = new ImageForm();
            this.imageForm.StartPosition = FormStartPosition.Manual;
            this.imageForm.Location = location;
            this.imageForm.WindowState = FormWindowState.Maximized;
            this.imageForm.Show();

            Image image = MemoryReader.Read(new ImageSerialization());
            this.imageForm.SetImage(image);
        }

        private void takeSeriesFromPictureBoxesButton_Click(object sender, EventArgs e)
        {
            TakeImagesSeriesPhoto();
        }

        private void DelayTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void MakePhaseShifts()
        {
            isMakePhaseShiftsProcess = true;
            InitPhaseShiftParameters();
            executeNextShift = true;
            ExecutePhaseShift();
        }
 
        private void MakeDelay()
        {
            if (delay > 0)
            {
                Thread.Sleep(delay);
            }
        }
        
        private void InitPhaseShiftParameters()
        {
            currentPhaseShiftNumber = 1;
            currentPhaseShiftValue = zeroPhaseShiftValue;

            phaseShiftStep = short.Parse(phaseShiftStepTextBox.Text, NumberStyles.HexNumber);
            phaseShiftCount = short.Parse(phaseShiftCountTextBox.Text);
            delay = int.Parse(DelayTextBox.Text);
        }
                
        private void CameraForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            /*
            if (this.CameraHandler != null)
            {
                this.CameraHandler.Dispose();
            }
            */

            if (phaseShiftDeviceController != null)
            {
                phaseShiftDeviceController.Dispose();
            }
        }

        /*
        private void FastTakePhoto()
        {
            List<Camera> cameraList = CameraHandler.GetCameraList();
            if (cameraList == null && cameraList.Count == 0)
            {
                return;
            }

            Camera camera = cameraList[0];
            if (!CameraHandler.CameraSessionOpen)
            {
                CameraHandler.OpenSession(camera);
            }

            int delay = 1000;
            short fromImageNumber = short.Parse(fromImageNumberTextBox.Text);
            short toImageNumber = short.Parse(toImageNumberTextBox.Text);

            ShowBackgroundWindow(fromImageNumber);
            Thread.Sleep(delay);

            this.seriesType = TakePhotoSeriesTypeEnum.ImageSeries;
            this.currentImageNumber = toImageNumber;

            CameraHandler.TakePhoto();
            Thread.Sleep(delay);
            
            this.imageForm.Close();
            this.imageForm = null;
        }
        */

        private Image GenerateImage(double phaseShift)
        {
            Image resImage = null;

            int width = 4096;
            int height = 2160;

            double pointsPerStripCount = 256;
            double maxIntensity = 255;
            double gamma = 1;
            double noise = 0;
            double minIntensity = FRAMES_256_MIN_INTENSITY;

            int kr = 0;

            ZArrayDescriptor resArray = Model_Sinus.Sinus1(phaseShift, maxIntensity, pointsPerStripCount, gamma, kr, width, height, noise, minIntensity);

            resImage = CreateImageFromArrayDescriptor(resArray);

            return resImage;
        }

        private void frames256Button_Click(object sender, EventArgs e)
        {
            Take256Images();
        }

        private void backGroundFromMainPicture_Click(object sender, EventArgs e)
        {
            ShowBackgroundFromMainImage();
        }

        private Image CreateImageFromArrayDescriptor(ZArrayDescriptor zArrayDescriptor)
        {
            int width = zArrayDescriptor.width;
            int height = zArrayDescriptor.height;

            Bitmap bitmap = new Bitmap(width, height);
            BitmapData bitmapData = ImageProcessor.getBitmapData(bitmap);

            int intensity = 0;

            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    intensity = Convert.ToInt32(zArrayDescriptor.array[j, i]);

                    Color color = Color.FromArgb(intensity, intensity, intensity);
                    ImageProcessor.setPixel(bitmapData, j, i, color);
                }
            }
            
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }
    }
}
