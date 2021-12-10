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
            this.SuspendLayout();
            // 
            // btnCorrectedGraph
            // 
            this.btnCorrectedGraph.Location = new System.Drawing.Point(16, 15);
            this.btnCorrectedGraph.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCorrectedGraph.Name = "btnCorrectedGraph";
            this.btnCorrectedGraph.Size = new System.Drawing.Size(152, 44);
            this.btnCorrectedGraph.TabIndex = 0;
            this.btnCorrectedGraph.Text = "CorrectedGraph";
            this.btnCorrectedGraph.UseVisualStyleBackColor = true;
            this.btnCorrectedGraph.Click += new System.EventHandler(this.btnCorrectedGraph_Click);
            // 
            // btnCreateInterferogram
            // 
            this.btnCreateInterferogram.Location = new System.Drawing.Point(16, 79);
            this.btnCreateInterferogram.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCreateInterferogram.Name = "btnCreateInterferogram";
            this.btnCreateInterferogram.Size = new System.Drawing.Size(152, 48);
            this.btnCreateInterferogram.TabIndex = 1;
            this.btnCreateInterferogram.Text = "Create interferogram";
            this.btnCreateInterferogram.UseVisualStyleBackColor = true;
            this.btnCreateInterferogram.Click += new System.EventHandler(this.btnCreateInterferogram_Click);
            // 
            // btnTakePicture
            // 
            this.btnTakePicture.Location = new System.Drawing.Point(16, 146);
            this.btnTakePicture.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTakePicture.Name = "btnTakePicture";
            this.btnTakePicture.Size = new System.Drawing.Size(152, 44);
            this.btnTakePicture.TabIndex = 2;
            this.btnTakePicture.Text = "Take picture";
            this.btnTakePicture.UseVisualStyleBackColor = true;
            this.btnTakePicture.Click += new System.EventHandler(this.btnTakePicture_Click);
            // 
            // btnTakeSeries
            // 
            this.btnTakeSeries.Location = new System.Drawing.Point(16, 209);
            this.btnTakeSeries.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTakeSeries.Name = "btnTakeSeries";
            this.btnTakeSeries.Size = new System.Drawing.Size(152, 48);
            this.btnTakeSeries.TabIndex = 3;
            this.btnTakeSeries.Text = "Take series";
            this.btnTakeSeries.UseVisualStyleBackColor = true;
            this.btnTakeSeries.Click += new System.EventHandler(this.btnTakeSeries_Click);
            // 
            // btnGraphFromImages
            // 
            this.btnGraphFromImages.Location = new System.Drawing.Point(16, 279);
            this.btnGraphFromImages.Name = "btnGraphFromImages";
            this.btnGraphFromImages.Size = new System.Drawing.Size(152, 50);
            this.btnGraphFromImages.TabIndex = 4;
            this.btnGraphFromImages.Text = "Graph from images";
            this.btnGraphFromImages.UseVisualStyleBackColor = true;
            this.btnGraphFromImages.Click += new System.EventHandler(this.btnGraphFromImages_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 352);
            this.Controls.Add(this.btnGraphFromImages);
            this.Controls.Add(this.btnTakeSeries);
            this.Controls.Add(this.btnTakePicture);
            this.Controls.Add(this.btnCreateInterferogram);
            this.Controls.Add(this.btnCorrectedGraph);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "Holo manager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCorrectedGraph;
        private System.Windows.Forms.Button btnCreateInterferogram;
        private System.Windows.Forms.Button btnTakePicture;
        private System.Windows.Forms.Button btnTakeSeries;
        private System.Windows.Forms.Button btnGraphFromImages;
    }
}

