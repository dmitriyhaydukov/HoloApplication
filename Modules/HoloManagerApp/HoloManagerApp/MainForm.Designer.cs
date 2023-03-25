namespace HoloManagerApp
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCorrectedGraph = new System.Windows.Forms.Button();
            this.btnCreateInterferogram = new System.Windows.Forms.Button();
            this.btnTakePicture = new System.Windows.Forms.Button();
            this.btnTakeSeries = new System.Windows.Forms.Button();
            this.btnGraphFromImages = new System.Windows.Forms.Button();
            this.btnIntensityIncrease = new System.Windows.Forms.Button();
            this.btnBuildTable = new System.Windows.Forms.Button();
            this.btnCreateInterferogram2 = new System.Windows.Forms.Button();
            this.btnBuildRealTable = new System.Windows.Forms.Button();
            this.txtPhaseShift = new System.Windows.Forms.TextBox();
            this.lblPhaseShift = new System.Windows.Forms.Label();
            this.btnCreateTriangleImage = new System.Windows.Forms.Button();
            this.btnTakePicturesWithPhaseShifts = new System.Windows.Forms.Button();
            this.btnDecode = new System.Windows.Forms.Button();
            this.btnSinusOriginal = new System.Windows.Forms.Button();
            this.btnClinCurve = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCorrectedGraph
            // 
            this.btnCorrectedGraph.Location = new System.Drawing.Point(12, 12);
            this.btnCorrectedGraph.Name = "btnCorrectedGraph";
            this.btnCorrectedGraph.Size = new System.Drawing.Size(114, 36);
            this.btnCorrectedGraph.TabIndex = 0;
            this.btnCorrectedGraph.Text = "CorrectedGraph";
            this.btnCorrectedGraph.UseVisualStyleBackColor = true;
            this.btnCorrectedGraph.Click += new System.EventHandler(this.btnCorrectedGraph_Click);
            // 
            // btnCreateInterferogram
            // 
            this.btnCreateInterferogram.Location = new System.Drawing.Point(12, 64);
            this.btnCreateInterferogram.Name = "btnCreateInterferogram";
            this.btnCreateInterferogram.Size = new System.Drawing.Size(114, 39);
            this.btnCreateInterferogram.TabIndex = 1;
            this.btnCreateInterferogram.Text = "Create interferogram";
            this.btnCreateInterferogram.UseVisualStyleBackColor = true;
            this.btnCreateInterferogram.Click += new System.EventHandler(this.btnCreateInterferogram_Click);
            // 
            // btnTakePicture
            // 
            this.btnTakePicture.Location = new System.Drawing.Point(265, 12);
            this.btnTakePicture.Name = "btnTakePicture";
            this.btnTakePicture.Size = new System.Drawing.Size(114, 36);
            this.btnTakePicture.TabIndex = 2;
            this.btnTakePicture.Text = "Take picture";
            this.btnTakePicture.UseVisualStyleBackColor = true;
            this.btnTakePicture.Click += new System.EventHandler(this.btnTakePicture_Click);
            // 
            // btnTakeSeries
            // 
            this.btnTakeSeries.Location = new System.Drawing.Point(265, 64);
            this.btnTakeSeries.Name = "btnTakeSeries";
            this.btnTakeSeries.Size = new System.Drawing.Size(114, 39);
            this.btnTakeSeries.TabIndex = 3;
            this.btnTakeSeries.Text = "Take series";
            this.btnTakeSeries.UseVisualStyleBackColor = true;
            this.btnTakeSeries.Click += new System.EventHandler(this.btnTakeSeries_Click);
            // 
            // btnGraphFromImages
            // 
            this.btnGraphFromImages.Location = new System.Drawing.Point(265, 113);
            this.btnGraphFromImages.Margin = new System.Windows.Forms.Padding(2);
            this.btnGraphFromImages.Name = "btnGraphFromImages";
            this.btnGraphFromImages.Size = new System.Drawing.Size(114, 41);
            this.btnGraphFromImages.TabIndex = 4;
            this.btnGraphFromImages.Text = "Graph from images";
            this.btnGraphFromImages.UseVisualStyleBackColor = true;
            this.btnGraphFromImages.Click += new System.EventHandler(this.btnGraphFromImages_Click);
            // 
            // btnIntensityIncrease
            // 
            this.btnIntensityIncrease.Location = new System.Drawing.Point(146, 12);
            this.btnIntensityIncrease.Name = "btnIntensityIncrease";
            this.btnIntensityIncrease.Size = new System.Drawing.Size(103, 36);
            this.btnIntensityIncrease.TabIndex = 5;
            this.btnIntensityIncrease.Text = "Intensity Increasing";
            this.btnIntensityIncrease.UseVisualStyleBackColor = true;
            this.btnIntensityIncrease.Click += new System.EventHandler(this.btnIntensityIncrease_Click);
            // 
            // btnBuildTable
            // 
            this.btnBuildTable.Location = new System.Drawing.Point(146, 64);
            this.btnBuildTable.Name = "btnBuildTable";
            this.btnBuildTable.Size = new System.Drawing.Size(103, 39);
            this.btnBuildTable.TabIndex = 6;
            this.btnBuildTable.Text = "Build table";
            this.btnBuildTable.UseVisualStyleBackColor = true;
            this.btnBuildTable.Click += new System.EventHandler(this.btnBuildTable_Click);
            // 
            // btnCreateInterferogram2
            // 
            this.btnCreateInterferogram2.Location = new System.Drawing.Point(12, 115);
            this.btnCreateInterferogram2.Name = "btnCreateInterferogram2";
            this.btnCreateInterferogram2.Size = new System.Drawing.Size(114, 37);
            this.btnCreateInterferogram2.TabIndex = 7;
            this.btnCreateInterferogram2.Text = "Create interferogram 2";
            this.btnCreateInterferogram2.UseVisualStyleBackColor = true;
            this.btnCreateInterferogram2.Click += new System.EventHandler(this.btnCreateInterferogram2_Click);
            // 
            // btnBuildRealTable
            // 
            this.btnBuildRealTable.Location = new System.Drawing.Point(146, 115);
            this.btnBuildRealTable.Name = "btnBuildRealTable";
            this.btnBuildRealTable.Size = new System.Drawing.Size(103, 37);
            this.btnBuildRealTable.TabIndex = 8;
            this.btnBuildRealTable.Text = "Build Real table";
            this.btnBuildRealTable.UseVisualStyleBackColor = true;
            this.btnBuildRealTable.Click += new System.EventHandler(this.btnBuildRealTable_Click);
            // 
            // txtPhaseShift
            // 
            this.txtPhaseShift.Location = new System.Drawing.Point(12, 237);
            this.txtPhaseShift.Name = "txtPhaseShift";
            this.txtPhaseShift.Size = new System.Drawing.Size(114, 20);
            this.txtPhaseShift.TabIndex = 9;
            // 
            // lblPhaseShift
            // 
            this.lblPhaseShift.AutoSize = true;
            this.lblPhaseShift.Location = new System.Drawing.Point(12, 221);
            this.lblPhaseShift.Name = "lblPhaseShift";
            this.lblPhaseShift.Size = new System.Drawing.Size(58, 13);
            this.lblPhaseShift.TabIndex = 10;
            this.lblPhaseShift.Text = "PhaseShift";
            // 
            // btnCreateTriangleImage
            // 
            this.btnCreateTriangleImage.Location = new System.Drawing.Point(12, 263);
            this.btnCreateTriangleImage.Name = "btnCreateTriangleImage";
            this.btnCreateTriangleImage.Size = new System.Drawing.Size(114, 42);
            this.btnCreateTriangleImage.TabIndex = 11;
            this.btnCreateTriangleImage.Text = "Create triangle";
            this.btnCreateTriangleImage.UseVisualStyleBackColor = true;
            this.btnCreateTriangleImage.Click += new System.EventHandler(this.btnCreateTriangleImage_Click);
            // 
            // btnTakePicturesWithPhaseShifts
            // 
            this.btnTakePicturesWithPhaseShifts.Location = new System.Drawing.Point(265, 168);
            this.btnTakePicturesWithPhaseShifts.Name = "btnTakePicturesWithPhaseShifts";
            this.btnTakePicturesWithPhaseShifts.Size = new System.Drawing.Size(114, 41);
            this.btnTakePicturesWithPhaseShifts.TabIndex = 12;
            this.btnTakePicturesWithPhaseShifts.Text = "Take pictures with phase shifts";
            this.btnTakePicturesWithPhaseShifts.UseVisualStyleBackColor = true;
            this.btnTakePicturesWithPhaseShifts.Click += new System.EventHandler(this.btnTakePicturesWithPhaseShifts_Click);
            // 
            // btnDecode
            // 
            this.btnDecode.Location = new System.Drawing.Point(146, 168);
            this.btnDecode.Name = "btnDecode";
            this.btnDecode.Size = new System.Drawing.Size(103, 42);
            this.btnDecode.TabIndex = 13;
            this.btnDecode.Text = "Decode";
            this.btnDecode.UseVisualStyleBackColor = true;
            this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
            // 
            // btnSinusOriginal
            // 
            this.btnSinusOriginal.Location = new System.Drawing.Point(12, 168);
            this.btnSinusOriginal.Name = "btnSinusOriginal";
            this.btnSinusOriginal.Size = new System.Drawing.Size(114, 42);
            this.btnSinusOriginal.TabIndex = 14;
            this.btnSinusOriginal.Text = "Sinus Original";
            this.btnSinusOriginal.UseVisualStyleBackColor = true;
            this.btnSinusOriginal.Click += new System.EventHandler(this.btnSinusOriginal_Click);
            // 
            // btnClinCurve
            // 
            this.btnClinCurve.Location = new System.Drawing.Point(12, 311);
            this.btnClinCurve.Name = "btnClinCurve";
            this.btnClinCurve.Size = new System.Drawing.Size(114, 46);
            this.btnClinCurve.TabIndex = 15;
            this.btnClinCurve.Text = "Clin curve";
            this.btnClinCurve.UseVisualStyleBackColor = true;
            this.btnClinCurve.Click += new System.EventHandler(this.btnClinCurve_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 378);
            this.Controls.Add(this.btnClinCurve);
            this.Controls.Add(this.btnSinusOriginal);
            this.Controls.Add(this.btnDecode);
            this.Controls.Add(this.btnTakePicturesWithPhaseShifts);
            this.Controls.Add(this.btnCreateTriangleImage);
            this.Controls.Add(this.lblPhaseShift);
            this.Controls.Add(this.txtPhaseShift);
            this.Controls.Add(this.btnBuildRealTable);
            this.Controls.Add(this.btnCreateInterferogram2);
            this.Controls.Add(this.btnBuildTable);
            this.Controls.Add(this.btnIntensityIncrease);
            this.Controls.Add(this.btnGraphFromImages);
            this.Controls.Add(this.btnTakeSeries);
            this.Controls.Add(this.btnTakePicture);
            this.Controls.Add(this.btnCreateInterferogram);
            this.Controls.Add(this.btnCorrectedGraph);
            this.Name = "MainForm";
            this.Text = "Holo manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCorrectedGraph;
        private System.Windows.Forms.Button btnCreateInterferogram;
        private System.Windows.Forms.Button btnTakePicture;
        private System.Windows.Forms.Button btnTakeSeries;
        private System.Windows.Forms.Button btnGraphFromImages;
        private System.Windows.Forms.Button btnIntensityIncrease;
        private System.Windows.Forms.Button btnBuildTable;
        private System.Windows.Forms.Button btnCreateInterferogram2;
        private System.Windows.Forms.Button btnBuildRealTable;
        private System.Windows.Forms.TextBox txtPhaseShift;
        private System.Windows.Forms.Label lblPhaseShift;
        private System.Windows.Forms.Button btnCreateTriangleImage;
        private System.Windows.Forms.Button btnTakePicturesWithPhaseShifts;
        private System.Windows.Forms.Button btnDecode;
        private System.Windows.Forms.Button btnSinusOriginal;
        private System.Windows.Forms.Button btnClinCurve;
    }
}

