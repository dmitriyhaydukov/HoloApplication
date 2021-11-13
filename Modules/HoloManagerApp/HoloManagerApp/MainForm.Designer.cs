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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 400);
            this.Controls.Add(this.btnCorrectedGraph);
            this.Name = "MainForm";
            this.Text = "Holo manager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCorrectedGraph;
    }
}

