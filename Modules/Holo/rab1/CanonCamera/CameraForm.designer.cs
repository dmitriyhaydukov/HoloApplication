namespace rab1
{
    partial class CameraForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.LiveViewButton = new System.Windows.Forms.Button();
            this.CameraListBox = new System.Windows.Forms.ListBox();
            this.SessionButton = new System.Windows.Forms.Button();
            this.SessionLabel = new System.Windows.Forms.Label();
            this.InitGroupBox = new System.Windows.Forms.GroupBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.FocusFar3Button = new System.Windows.Forms.Button();
            this.FocusFar2Button = new System.Windows.Forms.Button();
            this.FocusFar1Button = new System.Windows.Forms.Button();
            this.FocusNear1Button = new System.Windows.Forms.Button();
            this.FocusNear2Button = new System.Windows.Forms.Button();
            this.FocusNear3Button = new System.Windows.Forms.Button();
            this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.frames256DirectoryTextBox = new System.Windows.Forms.TextBox();
            this.frames256Button = new System.Windows.Forms.Button();
            this.stratImageNumberLabel = new System.Windows.Forms.Label();
            this.startImageNumberTextBox = new System.Windows.Forms.TextBox();
            this.ShiftsCountLabel = new System.Windows.Forms.Label();
            this.ShiftsCountTextBox = new System.Windows.Forms.TextBox();
            this.backGroundWindowButton = new System.Windows.Forms.Button();
            this.takeSeriesFromPictureBoxesButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.currentPhaseShiftLabel = new System.Windows.Forms.Label();
            this.phaseShiftCountTextBox = new System.Windows.Forms.TextBox();
            this.phaseShiftCountLabel = new System.Windows.Forms.Label();
            this.closephaseShiftSerialPortButton = new System.Windows.Forms.Button();
            this.initSerialPortButton = new System.Windows.Forms.Button();
            this.MakePhaseShiftsButton = new System.Windows.Forms.Button();
            this.phaseShiftStepLabel = new System.Windows.Forms.Label();
            this.SerialPortLabel = new System.Windows.Forms.Label();
            this.phaseShiftStepTextBox = new System.Windows.Forms.TextBox();
            this.phaseShiftSerialPortComboBox = new System.Windows.Forms.ComboBox();
            this.SaveToGroupBox = new System.Windows.Forms.GroupBox();
            this.imageSaveComboBox = new System.Windows.Forms.ComboBox();
            this.STBothButton = new System.Windows.Forms.RadioButton();
            this.STComputerButton = new System.Windows.Forms.RadioButton();
            this.STCameraButton = new System.Windows.Forms.RadioButton();
            this.lblColor = new System.Windows.Forms.Label();
            this.colorComboBox = new System.Windows.Forms.ComboBox();
            this.DelayPhaseShiftLabel = new System.Windows.Forms.Label();
            this.DelayTextBox = new System.Windows.Forms.TextBox();
            this.takeSeriesPhotoButton = new System.Windows.Forms.Button();
            this.MainProgressBar = new System.Windows.Forms.ProgressBar();
            this.WBCoBox = new System.Windows.Forms.ComboBox();
            this.SavePathTextBox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.RecordVideoButton = new System.Windows.Forms.Button();
            this.TakePhotoButton = new System.Windows.Forms.Button();
            this.BulbUpDo = new System.Windows.Forms.NumericUpDown();
            this.ISOCoBox = new System.Windows.Forms.ComboBox();
            this.TvCoBox = new System.Windows.Forms.ComboBox();
            this.AvCoBox = new System.Windows.Forms.ComboBox();
            this.SaveFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.LiveViewPicBox = new System.Windows.Forms.PictureBox();
            this.LiveViewGroupBox = new System.Windows.Forms.GroupBox();
            this.backGroundFromMainPicture = new System.Windows.Forms.Button();
            this.InitGroupBox.SuspendLayout();
            this.SettingsGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SaveToGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BulbUpDo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LiveViewPicBox)).BeginInit();
            this.LiveViewGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // LiveViewButton
            // 
            this.LiveViewButton.Location = new System.Drawing.Point(6, 19);
            this.LiveViewButton.Name = "LiveViewButton";
            this.LiveViewButton.Size = new System.Drawing.Size(90, 22);
            this.LiveViewButton.TabIndex = 2;
            this.LiveViewButton.Text = "Start LV";
            this.LiveViewButton.UseVisualStyleBackColor = true;
            this.LiveViewButton.Click += new System.EventHandler(this.LiveViewButton_Click);
            // 
            // CameraListBox
            // 
            this.CameraListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CameraListBox.FormattingEnabled = true;
            this.CameraListBox.Location = new System.Drawing.Point(6, 44);
            this.CameraListBox.Name = "CameraListBox";
            this.CameraListBox.Size = new System.Drawing.Size(121, 134);
            this.CameraListBox.TabIndex = 6;
            // 
            // SessionButton
            // 
            this.SessionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SessionButton.Location = new System.Drawing.Point(6, 185);
            this.SessionButton.Name = "SessionButton";
            this.SessionButton.Size = new System.Drawing.Size(84, 23);
            this.SessionButton.TabIndex = 7;
            this.SessionButton.Text = "Open Session";
            this.SessionButton.UseVisualStyleBackColor = true;
            this.SessionButton.Click += new System.EventHandler(this.SessionButton_Click);
            // 
            // SessionLabel
            // 
            this.SessionLabel.AutoSize = true;
            this.SessionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SessionLabel.Location = new System.Drawing.Point(6, 16);
            this.SessionLabel.Name = "SessionLabel";
            this.SessionLabel.Size = new System.Drawing.Size(110, 16);
            this.SessionLabel.TabIndex = 8;
            this.SessionLabel.Text = "No open session";
            // 
            // InitGroupBox
            // 
            this.InitGroupBox.Controls.Add(this.RefreshButton);
            this.InitGroupBox.Controls.Add(this.CameraListBox);
            this.InitGroupBox.Controls.Add(this.SessionLabel);
            this.InitGroupBox.Controls.Add(this.SessionButton);
            this.InitGroupBox.Location = new System.Drawing.Point(12, 11);
            this.InitGroupBox.Name = "InitGroupBox";
            this.InitGroupBox.Size = new System.Drawing.Size(135, 214);
            this.InitGroupBox.TabIndex = 9;
            this.InitGroupBox.TabStop = false;
            this.InitGroupBox.Text = "Init";
            // 
            // RefreshButton
            // 
            this.RefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RefreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshButton.Location = new System.Drawing.Point(98, 185);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(31, 23);
            this.RefreshButton.TabIndex = 9;
            this.RefreshButton.Text = "↻";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // FocusFar3Button
            // 
            this.FocusFar3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FocusFar3Button.Location = new System.Drawing.Point(243, 18);
            this.FocusFar3Button.Name = "FocusFar3Button";
            this.FocusFar3Button.Size = new System.Drawing.Size(28, 23);
            this.FocusFar3Button.TabIndex = 6;
            this.FocusFar3Button.Text = ">>>";
            this.FocusFar3Button.UseVisualStyleBackColor = true;
            this.FocusFar3Button.Click += new System.EventHandler(this.FocusFar3Button_Click);
            // 
            // FocusFar2Button
            // 
            this.FocusFar2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FocusFar2Button.Location = new System.Drawing.Point(277, 18);
            this.FocusFar2Button.Name = "FocusFar2Button";
            this.FocusFar2Button.Size = new System.Drawing.Size(28, 23);
            this.FocusFar2Button.TabIndex = 6;
            this.FocusFar2Button.Text = ">>";
            this.FocusFar2Button.UseVisualStyleBackColor = true;
            this.FocusFar2Button.Click += new System.EventHandler(this.FocusFar2Button_Click);
            // 
            // FocusFar1Button
            // 
            this.FocusFar1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FocusFar1Button.Location = new System.Drawing.Point(204, 18);
            this.FocusFar1Button.Name = "FocusFar1Button";
            this.FocusFar1Button.Size = new System.Drawing.Size(28, 23);
            this.FocusFar1Button.TabIndex = 6;
            this.FocusFar1Button.Text = ">";
            this.FocusFar1Button.UseVisualStyleBackColor = true;
            this.FocusFar1Button.Click += new System.EventHandler(this.FocusFar1Button_Click);
            // 
            // FocusNear1Button
            // 
            this.FocusNear1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FocusNear1Button.Location = new System.Drawing.Point(170, 18);
            this.FocusNear1Button.Name = "FocusNear1Button";
            this.FocusNear1Button.Size = new System.Drawing.Size(28, 23);
            this.FocusNear1Button.TabIndex = 6;
            this.FocusNear1Button.Text = "<";
            this.FocusNear1Button.UseVisualStyleBackColor = true;
            this.FocusNear1Button.Click += new System.EventHandler(this.FocusNear1Button_Click);
            // 
            // FocusNear2Button
            // 
            this.FocusNear2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FocusNear2Button.Location = new System.Drawing.Point(136, 19);
            this.FocusNear2Button.Name = "FocusNear2Button";
            this.FocusNear2Button.Size = new System.Drawing.Size(28, 23);
            this.FocusNear2Button.TabIndex = 6;
            this.FocusNear2Button.Text = "<<";
            this.FocusNear2Button.UseVisualStyleBackColor = true;
            this.FocusNear2Button.Click += new System.EventHandler(this.FocusNear2Button_Click);
            // 
            // FocusNear3Button
            // 
            this.FocusNear3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FocusNear3Button.Location = new System.Drawing.Point(99, 19);
            this.FocusNear3Button.Name = "FocusNear3Button";
            this.FocusNear3Button.Size = new System.Drawing.Size(28, 23);
            this.FocusNear3Button.TabIndex = 6;
            this.FocusNear3Button.Text = "<<<";
            this.FocusNear3Button.UseVisualStyleBackColor = true;
            this.FocusNear3Button.Click += new System.EventHandler(this.FocusNear3Button_Click);
            // 
            // SettingsGroupBox
            // 
            this.SettingsGroupBox.Controls.Add(this.backGroundFromMainPicture);
            this.SettingsGroupBox.Controls.Add(this.frames256DirectoryTextBox);
            this.SettingsGroupBox.Controls.Add(this.frames256Button);
            this.SettingsGroupBox.Controls.Add(this.stratImageNumberLabel);
            this.SettingsGroupBox.Controls.Add(this.startImageNumberTextBox);
            this.SettingsGroupBox.Controls.Add(this.ShiftsCountLabel);
            this.SettingsGroupBox.Controls.Add(this.ShiftsCountTextBox);
            this.SettingsGroupBox.Controls.Add(this.backGroundWindowButton);
            this.SettingsGroupBox.Controls.Add(this.takeSeriesFromPictureBoxesButton);
            this.SettingsGroupBox.Controls.Add(this.groupBox1);
            this.SettingsGroupBox.Controls.Add(this.SaveToGroupBox);
            this.SettingsGroupBox.Controls.Add(this.lblColor);
            this.SettingsGroupBox.Controls.Add(this.colorComboBox);
            this.SettingsGroupBox.Controls.Add(this.DelayPhaseShiftLabel);
            this.SettingsGroupBox.Controls.Add(this.DelayTextBox);
            this.SettingsGroupBox.Controls.Add(this.takeSeriesPhotoButton);
            this.SettingsGroupBox.Controls.Add(this.MainProgressBar);
            this.SettingsGroupBox.Controls.Add(this.WBCoBox);
            this.SettingsGroupBox.Controls.Add(this.SavePathTextBox);
            this.SettingsGroupBox.Controls.Add(this.BrowseButton);
            this.SettingsGroupBox.Controls.Add(this.label4);
            this.SettingsGroupBox.Controls.Add(this.label3);
            this.SettingsGroupBox.Controls.Add(this.label2);
            this.SettingsGroupBox.Controls.Add(this.label5);
            this.SettingsGroupBox.Controls.Add(this.label1);
            this.SettingsGroupBox.Controls.Add(this.RecordVideoButton);
            this.SettingsGroupBox.Controls.Add(this.TakePhotoButton);
            this.SettingsGroupBox.Controls.Add(this.BulbUpDo);
            this.SettingsGroupBox.Controls.Add(this.ISOCoBox);
            this.SettingsGroupBox.Controls.Add(this.TvCoBox);
            this.SettingsGroupBox.Controls.Add(this.AvCoBox);
            this.SettingsGroupBox.Enabled = false;
            this.SettingsGroupBox.Location = new System.Drawing.Point(153, 12);
            this.SettingsGroupBox.MinimumSize = new System.Drawing.Size(407, 158);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(837, 213);
            this.SettingsGroupBox.TabIndex = 11;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "Settings";
            // 
            // frames256DirectoryTextBox
            // 
            this.frames256DirectoryTextBox.Location = new System.Drawing.Point(434, 159);
            this.frames256DirectoryTextBox.Name = "frames256DirectoryTextBox";
            this.frames256DirectoryTextBox.Size = new System.Drawing.Size(226, 20);
            this.frames256DirectoryTextBox.TabIndex = 32;
            // 
            // frames256Button
            // 
            this.frames256Button.Location = new System.Drawing.Point(434, 185);
            this.frames256Button.Name = "frames256Button";
            this.frames256Button.Size = new System.Drawing.Size(226, 23);
            this.frames256Button.TabIndex = 31;
            this.frames256Button.Text = "256 frames";
            this.frames256Button.UseVisualStyleBackColor = true;
            this.frames256Button.Click += new System.EventHandler(this.frames256Button_Click);
            // 
            // stratImageNumberLabel
            // 
            this.stratImageNumberLabel.AutoSize = true;
            this.stratImageNumberLabel.Location = new System.Drawing.Point(666, 56);
            this.stratImageNumberLabel.Name = "stratImageNumberLabel";
            this.stratImageNumberLabel.Size = new System.Drawing.Size(89, 13);
            this.stratImageNumberLabel.TabIndex = 30;
            this.stratImageNumberLabel.Text = "Start image num.:";
            // 
            // startImageNumberTextBox
            // 
            this.startImageNumberTextBox.Location = new System.Drawing.Point(759, 53);
            this.startImageNumberTextBox.Name = "startImageNumberTextBox";
            this.startImageNumberTextBox.Size = new System.Drawing.Size(71, 20);
            this.startImageNumberTextBox.TabIndex = 29;
            // 
            // ShiftsCountLabel
            // 
            this.ShiftsCountLabel.AutoSize = true;
            this.ShiftsCountLabel.Location = new System.Drawing.Point(688, 15);
            this.ShiftsCountLabel.Name = "ShiftsCountLabel";
            this.ShiftsCountLabel.Size = new System.Drawing.Size(63, 13);
            this.ShiftsCountLabel.TabIndex = 28;
            this.ShiftsCountLabel.Text = "Shits count:";
            // 
            // ShiftsCountTextBox
            // 
            this.ShiftsCountTextBox.Location = new System.Drawing.Point(759, 10);
            this.ShiftsCountTextBox.Name = "ShiftsCountTextBox";
            this.ShiftsCountTextBox.Size = new System.Drawing.Size(71, 20);
            this.ShiftsCountTextBox.TabIndex = 27;
            // 
            // backGroundWindowButton
            // 
            this.backGroundWindowButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backGroundWindowButton.Location = new System.Drawing.Point(676, 74);
            this.backGroundWindowButton.Name = "backGroundWindowButton";
            this.backGroundWindowButton.Size = new System.Drawing.Size(156, 23);
            this.backGroundWindowButton.TabIndex = 26;
            this.backGroundWindowButton.Text = "Background Window";
            this.backGroundWindowButton.UseVisualStyleBackColor = true;
            this.backGroundWindowButton.Click += new System.EventHandler(this.backGroundWindowButton_Click);
            // 
            // takeSeriesFromPictureBoxesButton
            // 
            this.takeSeriesFromPictureBoxesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.takeSeriesFromPictureBoxesButton.Location = new System.Drawing.Point(676, 132);
            this.takeSeriesFromPictureBoxesButton.Name = "takeSeriesFromPictureBoxesButton";
            this.takeSeriesFromPictureBoxesButton.Size = new System.Drawing.Size(154, 45);
            this.takeSeriesFromPictureBoxesButton.TabIndex = 25;
            this.takeSeriesFromPictureBoxesButton.Text = "Take Series From 1,2,3,4";
            this.takeSeriesFromPictureBoxesButton.UseVisualStyleBackColor = true;
            this.takeSeriesFromPictureBoxesButton.Click += new System.EventHandler(this.takeSeriesFromPictureBoxesButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.currentPhaseShiftLabel);
            this.groupBox1.Controls.Add(this.phaseShiftCountTextBox);
            this.groupBox1.Controls.Add(this.phaseShiftCountLabel);
            this.groupBox1.Controls.Add(this.closephaseShiftSerialPortButton);
            this.groupBox1.Controls.Add(this.initSerialPortButton);
            this.groupBox1.Controls.Add(this.MakePhaseShiftsButton);
            this.groupBox1.Controls.Add(this.phaseShiftStepLabel);
            this.groupBox1.Controls.Add(this.SerialPortLabel);
            this.groupBox1.Controls.Add(this.phaseShiftStepTextBox);
            this.groupBox1.Controls.Add(this.phaseShiftSerialPortComboBox);
            this.groupBox1.Location = new System.Drawing.Point(434, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 137);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Phase shift";
            // 
            // currentPhaseShiftLabel
            // 
            this.currentPhaseShiftLabel.AutoSize = true;
            this.currentPhaseShiftLabel.Location = new System.Drawing.Point(144, 19);
            this.currentPhaseShiftLabel.Name = "currentPhaseShiftLabel";
            this.currentPhaseShiftLabel.Size = new System.Drawing.Size(61, 13);
            this.currentPhaseShiftLabel.TabIndex = 17;
            this.currentPhaseShiftLabel.Text = "Phase Shift";
            // 
            // phaseShiftCountTextBox
            // 
            this.phaseShiftCountTextBox.Location = new System.Drawing.Point(79, 16);
            this.phaseShiftCountTextBox.Name = "phaseShiftCountTextBox";
            this.phaseShiftCountTextBox.Size = new System.Drawing.Size(58, 20);
            this.phaseShiftCountTextBox.TabIndex = 14;
            // 
            // phaseShiftCountLabel
            // 
            this.phaseShiftCountLabel.AutoSize = true;
            this.phaseShiftCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phaseShiftCountLabel.Location = new System.Drawing.Point(3, 19);
            this.phaseShiftCountLabel.Name = "phaseShiftCountLabel";
            this.phaseShiftCountLabel.Size = new System.Drawing.Size(78, 16);
            this.phaseShiftCountLabel.TabIndex = 15;
            this.phaseShiftCountLabel.Text = "Shifts count:";
            // 
            // closephaseShiftSerialPortButton
            // 
            this.closephaseShiftSerialPortButton.Location = new System.Drawing.Point(151, 70);
            this.closephaseShiftSerialPortButton.Name = "closephaseShiftSerialPortButton";
            this.closephaseShiftSerialPortButton.Size = new System.Drawing.Size(73, 23);
            this.closephaseShiftSerialPortButton.TabIndex = 16;
            this.closephaseShiftSerialPortButton.Text = "Close";
            this.closephaseShiftSerialPortButton.UseVisualStyleBackColor = true;
            this.closephaseShiftSerialPortButton.Click += new System.EventHandler(this.closephaseShiftSerialPortButton_Click);
            // 
            // initSerialPortButton
            // 
            this.initSerialPortButton.Location = new System.Drawing.Point(69, 98);
            this.initSerialPortButton.Name = "initSerialPortButton";
            this.initSerialPortButton.Size = new System.Drawing.Size(76, 23);
            this.initSerialPortButton.TabIndex = 11;
            this.initSerialPortButton.Text = "Init";
            this.initSerialPortButton.UseVisualStyleBackColor = true;
            this.initSerialPortButton.Click += new System.EventHandler(this.initSerialPortButton_Click);
            // 
            // MakePhaseShiftsButton
            // 
            this.MakePhaseShiftsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MakePhaseShiftsButton.Location = new System.Drawing.Point(151, 98);
            this.MakePhaseShiftsButton.Name = "MakePhaseShiftsButton";
            this.MakePhaseShiftsButton.Size = new System.Drawing.Size(73, 23);
            this.MakePhaseShiftsButton.TabIndex = 19;
            this.MakePhaseShiftsButton.Text = "Execute shifts";
            this.MakePhaseShiftsButton.UseVisualStyleBackColor = true;
            this.MakePhaseShiftsButton.Click += new System.EventHandler(this.MakePhaseShiftsButton_Click);
            // 
            // phaseShiftStepLabel
            // 
            this.phaseShiftStepLabel.AutoSize = true;
            this.phaseShiftStepLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phaseShiftStepLabel.Location = new System.Drawing.Point(3, 42);
            this.phaseShiftStepLabel.Name = "phaseShiftStepLabel";
            this.phaseShiftStepLabel.Size = new System.Drawing.Size(72, 16);
            this.phaseShiftStepLabel.TabIndex = 13;
            this.phaseShiftStepLabel.Text = "Shifts step:";
            // 
            // SerialPortLabel
            // 
            this.SerialPortLabel.AutoSize = true;
            this.SerialPortLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SerialPortLabel.Location = new System.Drawing.Point(3, 73);
            this.SerialPortLabel.Name = "SerialPortLabel";
            this.SerialPortLabel.Size = new System.Drawing.Size(65, 16);
            this.SerialPortLabel.TabIndex = 18;
            this.SerialPortLabel.Text = "Com port:";
            // 
            // phaseShiftStepTextBox
            // 
            this.phaseShiftStepTextBox.Location = new System.Drawing.Point(79, 42);
            this.phaseShiftStepTextBox.Name = "phaseShiftStepTextBox";
            this.phaseShiftStepTextBox.Size = new System.Drawing.Size(58, 20);
            this.phaseShiftStepTextBox.TabIndex = 12;
            // 
            // phaseShiftSerialPortComboBox
            // 
            this.phaseShiftSerialPortComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.phaseShiftSerialPortComboBox.FormattingEnabled = true;
            this.phaseShiftSerialPortComboBox.Location = new System.Drawing.Point(69, 71);
            this.phaseShiftSerialPortComboBox.Name = "phaseShiftSerialPortComboBox";
            this.phaseShiftSerialPortComboBox.Size = new System.Drawing.Size(76, 21);
            this.phaseShiftSerialPortComboBox.TabIndex = 9;
            // 
            // SaveToGroupBox
            // 
            this.SaveToGroupBox.Controls.Add(this.imageSaveComboBox);
            this.SaveToGroupBox.Controls.Add(this.STBothButton);
            this.SaveToGroupBox.Controls.Add(this.STComputerButton);
            this.SaveToGroupBox.Controls.Add(this.STCameraButton);
            this.SaveToGroupBox.Location = new System.Drawing.Point(300, 16);
            this.SaveToGroupBox.Name = "SaveToGroupBox";
            this.SaveToGroupBox.Size = new System.Drawing.Size(128, 110);
            this.SaveToGroupBox.TabIndex = 4;
            this.SaveToGroupBox.TabStop = false;
            this.SaveToGroupBox.Text = "Save To";
            // 
            // imageSaveComboBox
            // 
            this.imageSaveComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imageSaveComboBox.FormattingEnabled = true;
            this.imageSaveComboBox.Location = new System.Drawing.Point(6, 77);
            this.imageSaveComboBox.Name = "imageSaveComboBox";
            this.imageSaveComboBox.Size = new System.Drawing.Size(83, 21);
            this.imageSaveComboBox.TabIndex = 23;
            // 
            // STBothButton
            // 
            this.STBothButton.AutoSize = true;
            this.STBothButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STBothButton.Location = new System.Drawing.Point(6, 57);
            this.STBothButton.Name = "STBothButton";
            this.STBothButton.Size = new System.Drawing.Size(53, 20);
            this.STBothButton.TabIndex = 0;
            this.STBothButton.Text = "Both";
            this.STBothButton.UseVisualStyleBackColor = true;
            this.STBothButton.CheckedChanged += new System.EventHandler(this.SaveToButton_CheckedChanged);
            // 
            // STComputerButton
            // 
            this.STComputerButton.AutoSize = true;
            this.STComputerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STComputerButton.Location = new System.Drawing.Point(6, 36);
            this.STComputerButton.Name = "STComputerButton";
            this.STComputerButton.Size = new System.Drawing.Size(84, 20);
            this.STComputerButton.TabIndex = 0;
            this.STComputerButton.Text = "Computer";
            this.STComputerButton.UseVisualStyleBackColor = true;
            this.STComputerButton.CheckedChanged += new System.EventHandler(this.SaveToButton_CheckedChanged);
            // 
            // STCameraButton
            // 
            this.STCameraButton.AutoSize = true;
            this.STCameraButton.Checked = true;
            this.STCameraButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STCameraButton.Location = new System.Drawing.Point(6, 14);
            this.STCameraButton.Name = "STCameraButton";
            this.STCameraButton.Size = new System.Drawing.Size(74, 20);
            this.STCameraButton.TabIndex = 0;
            this.STCameraButton.TabStop = true;
            this.STCameraButton.Text = "Camera";
            this.STCameraButton.UseVisualStyleBackColor = true;
            this.STCameraButton.CheckedChanged += new System.EventHandler(this.SaveToButton_CheckedChanged);
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColor.Location = new System.Drawing.Point(303, 134);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(43, 16);
            this.lblColor.TabIndex = 23;
            this.lblColor.Text = "Color:";
            // 
            // colorComboBox
            // 
            this.colorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorComboBox.FormattingEnabled = true;
            this.colorComboBox.Location = new System.Drawing.Point(362, 132);
            this.colorComboBox.Name = "colorComboBox";
            this.colorComboBox.Size = new System.Drawing.Size(66, 21);
            this.colorComboBox.TabIndex = 22;
            // 
            // DelayPhaseShiftLabel
            // 
            this.DelayPhaseShiftLabel.AutoSize = true;
            this.DelayPhaseShiftLabel.Location = new System.Drawing.Point(712, 34);
            this.DelayPhaseShiftLabel.Name = "DelayPhaseShiftLabel";
            this.DelayPhaseShiftLabel.Size = new System.Drawing.Size(37, 13);
            this.DelayPhaseShiftLabel.TabIndex = 21;
            this.DelayPhaseShiftLabel.Text = "Delay:";
            // 
            // DelayTextBox
            // 
            this.DelayTextBox.Location = new System.Drawing.Point(759, 31);
            this.DelayTextBox.Name = "DelayTextBox";
            this.DelayTextBox.Size = new System.Drawing.Size(71, 20);
            this.DelayTextBox.TabIndex = 20;
            this.DelayTextBox.TextChanged += new System.EventHandler(this.DelayTextBox_TextChanged);
            // 
            // takeSeriesPhotoButton
            // 
            this.takeSeriesPhotoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.takeSeriesPhotoButton.Location = new System.Drawing.Point(676, 100);
            this.takeSeriesPhotoButton.Name = "takeSeriesPhotoButton";
            this.takeSeriesPhotoButton.Size = new System.Drawing.Size(154, 26);
            this.takeSeriesPhotoButton.TabIndex = 10;
            this.takeSeriesPhotoButton.Text = "Take Series";
            this.takeSeriesPhotoButton.UseVisualStyleBackColor = true;
            this.takeSeriesPhotoButton.Click += new System.EventHandler(this.takeSeriesPhotoButton_Click);
            // 
            // MainProgressBar
            // 
            this.MainProgressBar.Location = new System.Drawing.Point(6, 100);
            this.MainProgressBar.Name = "MainProgressBar";
            this.MainProgressBar.Size = new System.Drawing.Size(130, 20);
            this.MainProgressBar.TabIndex = 8;
            // 
            // WBCoBox
            // 
            this.WBCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WBCoBox.FormattingEnabled = true;
            this.WBCoBox.Items.AddRange(new object[] {
            "Auto",
            "Daylight",
            "Cloudy",
            "Tungsten",
            "Fluorescent",
            "Strobe",
            "White Paper",
            "Shade"});
            this.WBCoBox.Location = new System.Drawing.Point(142, 46);
            this.WBCoBox.Name = "WBCoBox";
            this.WBCoBox.Size = new System.Drawing.Size(76, 21);
            this.WBCoBox.TabIndex = 7;
            this.WBCoBox.SelectedIndexChanged += new System.EventHandler(this.WBCoBox_SelectedIndexChanged);
            // 
            // SavePathTextBox
            // 
            this.SavePathTextBox.Enabled = false;
            this.SavePathTextBox.Location = new System.Drawing.Point(6, 126);
            this.SavePathTextBox.Name = "SavePathTextBox";
            this.SavePathTextBox.Size = new System.Drawing.Size(130, 20);
            this.SavePathTextBox.TabIndex = 6;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Enabled = false;
            this.BrowseButton.Location = new System.Drawing.Point(142, 125);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(76, 23);
            this.BrowseButton.TabIndex = 5;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(241, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Bulb (s)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(106, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "ISO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(106, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tv";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(224, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "WB";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(106, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Av";
            // 
            // RecordVideoButton
            // 
            this.RecordVideoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordVideoButton.Location = new System.Drawing.Point(224, 72);
            this.RecordVideoButton.Name = "RecordVideoButton";
            this.RecordVideoButton.Size = new System.Drawing.Size(70, 47);
            this.RecordVideoButton.TabIndex = 2;
            this.RecordVideoButton.Text = "Rec. Video";
            this.RecordVideoButton.UseVisualStyleBackColor = true;
            this.RecordVideoButton.Click += new System.EventHandler(this.RecordVideoButton_Click);
            // 
            // TakePhotoButton
            // 
            this.TakePhotoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TakePhotoButton.Location = new System.Drawing.Point(142, 72);
            this.TakePhotoButton.Name = "TakePhotoButton";
            this.TakePhotoButton.Size = new System.Drawing.Size(76, 48);
            this.TakePhotoButton.TabIndex = 2;
            this.TakePhotoButton.Text = "Take Photo";
            this.TakePhotoButton.UseVisualStyleBackColor = true;
            this.TakePhotoButton.Click += new System.EventHandler(this.TakePhotoButton_Click);
            // 
            // BulbUpDo
            // 
            this.BulbUpDo.Location = new System.Drawing.Point(141, 19);
            this.BulbUpDo.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.BulbUpDo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BulbUpDo.Name = "BulbUpDo";
            this.BulbUpDo.Size = new System.Drawing.Size(94, 20);
            this.BulbUpDo.TabIndex = 1;
            this.BulbUpDo.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // ISOCoBox
            // 
            this.ISOCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ISOCoBox.FormattingEnabled = true;
            this.ISOCoBox.Location = new System.Drawing.Point(6, 73);
            this.ISOCoBox.Name = "ISOCoBox";
            this.ISOCoBox.Size = new System.Drawing.Size(94, 21);
            this.ISOCoBox.TabIndex = 0;
            this.ISOCoBox.SelectedIndexChanged += new System.EventHandler(this.ISOCoBox_SelectedIndexChanged);
            // 
            // TvCoBox
            // 
            this.TvCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TvCoBox.FormattingEnabled = true;
            this.TvCoBox.Location = new System.Drawing.Point(6, 46);
            this.TvCoBox.Name = "TvCoBox";
            this.TvCoBox.Size = new System.Drawing.Size(94, 21);
            this.TvCoBox.TabIndex = 0;
            this.TvCoBox.SelectedIndexChanged += new System.EventHandler(this.TvCoBox_SelectedIndexChanged);
            // 
            // AvCoBox
            // 
            this.AvCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AvCoBox.FormattingEnabled = true;
            this.AvCoBox.Location = new System.Drawing.Point(6, 19);
            this.AvCoBox.Name = "AvCoBox";
            this.AvCoBox.Size = new System.Drawing.Size(94, 21);
            this.AvCoBox.TabIndex = 0;
            this.AvCoBox.SelectedIndexChanged += new System.EventHandler(this.AvCoBox_SelectedIndexChanged);
            // 
            // SaveFolderBrowser
            // 
            this.SaveFolderBrowser.Description = "Save Images To...";
            // 
            // LiveViewPicBox
            // 
            this.LiveViewPicBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LiveViewPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LiveViewPicBox.Location = new System.Drawing.Point(6, 47);
            this.LiveViewPicBox.Name = "LiveViewPicBox";
            this.LiveViewPicBox.Size = new System.Drawing.Size(960, 640);
            this.LiveViewPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LiveViewPicBox.TabIndex = 1;
            this.LiveViewPicBox.TabStop = false;
            this.LiveViewPicBox.SizeChanged += new System.EventHandler(this.LiveViewPicBox_SizeChanged);
            this.LiveViewPicBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LiveViewPicBox_MouseDown);
            // 
            // LiveViewGroupBox
            // 
            this.LiveViewGroupBox.Controls.Add(this.LiveViewPicBox);
            this.LiveViewGroupBox.Controls.Add(this.FocusFar2Button);
            this.LiveViewGroupBox.Controls.Add(this.LiveViewButton);
            this.LiveViewGroupBox.Controls.Add(this.FocusNear3Button);
            this.LiveViewGroupBox.Controls.Add(this.FocusFar1Button);
            this.LiveViewGroupBox.Controls.Add(this.FocusNear2Button);
            this.LiveViewGroupBox.Controls.Add(this.FocusNear1Button);
            this.LiveViewGroupBox.Controls.Add(this.FocusFar3Button);
            this.LiveViewGroupBox.Location = new System.Drawing.Point(12, 231);
            this.LiveViewGroupBox.Name = "LiveViewGroupBox";
            this.LiveViewGroupBox.Size = new System.Drawing.Size(978, 697);
            this.LiveViewGroupBox.TabIndex = 12;
            this.LiveViewGroupBox.TabStop = false;
            this.LiveViewGroupBox.Text = "Live view";
            // 
            // backGroundFromMainPicture
            // 
            this.backGroundFromMainPicture.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backGroundFromMainPicture.Location = new System.Drawing.Point(675, 183);
            this.backGroundFromMainPicture.Name = "backGroundFromMainPicture";
            this.backGroundFromMainPicture.Size = new System.Drawing.Size(156, 23);
            this.backGroundFromMainPicture.TabIndex = 33;
            this.backGroundFromMainPicture.Text = "Background from main";
            this.backGroundFromMainPicture.UseVisualStyleBackColor = true;
            this.backGroundFromMainPicture.Click += new System.EventHandler(this.backGroundFromMainPicture_Click);
            // 
            // CameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 936);
            this.Controls.Add(this.LiveViewGroupBox);
            this.Controls.Add(this.SettingsGroupBox);
            this.Controls.Add(this.InitGroupBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(588, 638);
            this.Name = "CameraForm";
            this.Text = "Canon SDK Tutorial";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CameraForm_FormClosed);
            this.InitGroupBox.ResumeLayout(false);
            this.InitGroupBox.PerformLayout();
            this.SettingsGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.SaveToGroupBox.ResumeLayout(false);
            this.SaveToGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BulbUpDo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LiveViewPicBox)).EndInit();
            this.LiveViewGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button LiveViewButton;
        private System.Windows.Forms.ListBox CameraListBox;
        private System.Windows.Forms.Button SessionButton;
        private System.Windows.Forms.Label SessionLabel;
        private System.Windows.Forms.GroupBox InitGroupBox;
        private System.Windows.Forms.GroupBox SettingsGroupBox;
        private System.Windows.Forms.Button TakePhotoButton;
        private System.Windows.Forms.NumericUpDown BulbUpDo;
        private System.Windows.Forms.ComboBox ISOCoBox;
        private System.Windows.Forms.ComboBox TvCoBox;
        private System.Windows.Forms.ComboBox AvCoBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox SaveToGroupBox;
        private System.Windows.Forms.RadioButton STBothButton;
        private System.Windows.Forms.RadioButton STComputerButton;
        private System.Windows.Forms.RadioButton STCameraButton;
        private System.Windows.Forms.TextBox SavePathTextBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.FolderBrowserDialog SaveFolderBrowser;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.ComboBox WBCoBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button RecordVideoButton;
        private System.Windows.Forms.Button FocusFar3Button;
        private System.Windows.Forms.Button FocusFar2Button;
        private System.Windows.Forms.Button FocusFar1Button;
        private System.Windows.Forms.Button FocusNear1Button;
        private System.Windows.Forms.Button FocusNear2Button;
        private System.Windows.Forms.Button FocusNear3Button;
        private System.Windows.Forms.ProgressBar MainProgressBar;
        private System.Windows.Forms.ComboBox phaseShiftSerialPortComboBox;
        private System.Windows.Forms.Button takeSeriesPhotoButton;
        private System.Windows.Forms.Button initSerialPortButton;
        private System.Windows.Forms.TextBox phaseShiftStepTextBox;
        private System.Windows.Forms.Label phaseShiftCountLabel;
        private System.Windows.Forms.TextBox phaseShiftCountTextBox;
        private System.Windows.Forms.Label phaseShiftStepLabel;
        private System.Windows.Forms.Button closephaseShiftSerialPortButton;
        private System.Windows.Forms.Label currentPhaseShiftLabel;
        private System.Windows.Forms.Label SerialPortLabel;
        private System.Windows.Forms.Button MakePhaseShiftsButton;
        private System.Windows.Forms.Label DelayPhaseShiftLabel;
        private System.Windows.Forms.TextBox DelayTextBox;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.ComboBox colorComboBox;
        private System.Windows.Forms.ComboBox imageSaveComboBox;
        private System.Windows.Forms.PictureBox LiveViewPicBox;
        private System.Windows.Forms.GroupBox LiveViewGroupBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button backGroundWindowButton;
        private System.Windows.Forms.Button takeSeriesFromPictureBoxesButton;
        private System.Windows.Forms.Label ShiftsCountLabel;
        private System.Windows.Forms.TextBox ShiftsCountTextBox;
        private System.Windows.Forms.Label stratImageNumberLabel;
        private System.Windows.Forms.TextBox startImageNumberTextBox;
        private System.Windows.Forms.Button frames256Button;
        private System.Windows.Forms.TextBox frames256DirectoryTextBox;
        private System.Windows.Forms.Button backGroundFromMainPicture;
    }
}

