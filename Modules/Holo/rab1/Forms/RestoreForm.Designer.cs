namespace rab1.Forms
{
    partial class RestoreForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.phaseMapImage = new System.Windows.Forms.PictureBox();
            this.buildButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rightImage = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.phaseMapImage)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightImage)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Загрузить";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(94, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Сохранить";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.phaseMapImage);
            this.panel1.Location = new System.Drawing.Point(12, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(481, 590);
            this.panel1.TabIndex = 34;
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
            // 
            // buildButton
            // 
            this.buildButton.Location = new System.Drawing.Point(286, 13);
            this.buildButton.Name = "buildButton";
            this.buildButton.Size = new System.Drawing.Size(75, 23);
            this.buildButton.TabIndex = 35;
            this.buildButton.Text = "Построить";
            this.buildButton.UseVisualStyleBackColor = true;
            this.buildButton.Click += new System.EventHandler(this.buildButton_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.rightImage);
            this.panel2.Location = new System.Drawing.Point(499, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(482, 590);
            this.panel2.TabIndex = 35;
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
            this.rightImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rightImage_MouseDown);
            // 
            // RestoreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 646);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.buildButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "RestoreForm";
            this.Text = "Восстановление трехмерного профиля";
            this.Shown += new System.EventHandler(this.RestoreForm_Shown);
            this.SizeChanged += new System.EventHandler(this.RestoreForm_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.phaseMapImage)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.PictureBox phaseMapImage;
        private System.Windows.Forms.Button buildButton;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.PictureBox rightImage;
    }
}