namespace rab1.Forms
{
    partial class CompareForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.rightImage = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.phaseMapImage = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightImage)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.phaseMapImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.rightImage);
            this.panel2.Location = new System.Drawing.Point(506, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(482, 590);
            this.panel2.TabIndex = 39;
            // 
            // rightImage
            // 
            this.rightImage.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.rightImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rightImage.Cursor = System.Windows.Forms.Cursors.Cross;
            this.rightImage.Location = new System.Drawing.Point(16, 21);
            this.rightImage.Name = "rightImage";
            this.rightImage.Size = new System.Drawing.Size(450, 554);
            this.rightImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.rightImage.TabIndex = 4;
            this.rightImage.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.phaseMapImage);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(481, 590);
            this.panel1.TabIndex = 38;
            // 
            // phaseMapImage
            // 
            this.phaseMapImage.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.phaseMapImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.phaseMapImage.Cursor = System.Windows.Forms.Cursors.Cross;
            this.phaseMapImage.Location = new System.Drawing.Point(16, 21);
            this.phaseMapImage.Name = "phaseMapImage";
            this.phaseMapImage.Size = new System.Drawing.Size(450, 554);
            this.phaseMapImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.phaseMapImage.TabIndex = 4;
            this.phaseMapImage.TabStop = false;
            this.phaseMapImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.phaseMapImage_MouseClick);
            // 
            // CompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 624);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "CompareForm";
            this.Text = "Сравнение";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.phaseMapImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.PictureBox rightImage;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.PictureBox phaseMapImage;
    }
}