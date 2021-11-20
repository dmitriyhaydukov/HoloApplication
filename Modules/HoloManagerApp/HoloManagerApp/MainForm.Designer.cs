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
            this.btnTakePicture.Location = new System.Drawing.Point(12, 119);
            this.btnTakePicture.Name = "btnTakePicture";
            this.btnTakePicture.Size = new System.Drawing.Size(114, 36);
            this.btnTakePicture.TabIndex = 2;
            this.btnTakePicture.Text = "Take picture";
            this.btnTakePicture.UseVisualStyleBackColor = true;
            this.btnTakePicture.Click += new System.EventHandler(this.btnTakePicture_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 400);
            this.Controls.Add(this.btnTakePicture);
            this.Controls.Add(this.btnCreateInterferogram);
            this.Controls.Add(this.btnCorrectedGraph);
            this.Name = "MainForm";
            this.Text = "Holo manager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCorrectedGraph;
        private System.Windows.Forms.Button btnCreateInterferogram;
        private System.Windows.Forms.Button btnTakePicture;
    }
}

