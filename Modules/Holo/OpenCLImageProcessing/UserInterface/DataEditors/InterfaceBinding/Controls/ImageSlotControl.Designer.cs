namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    partial class ImageSlotControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miRemoveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.IconPictureBox = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point(30, 14);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(59, 26);
            this.InfoLabel.TabIndex = 5;
            this.InfoLabel.Text = "[Info line 1]\r\n[line2]\r\n";
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TitleLabel.Location = new System.Drawing.Point(0, 2);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(47, 13);
            this.TitleLabel.TabIndex = 4;
            this.TitleLabel.Text = "[Name]";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRemoveImage});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 26);
            // 
            // miRemoveImage
            // 
            this.miRemoveImage.Name = "miRemoveImage";
            this.miRemoveImage.Size = new System.Drawing.Size(112, 22);
            this.miRemoveImage.Text = "Убрать";
            this.miRemoveImage.Click += new System.EventHandler(this.miRemoveImage_Click);
            // 
            // IconPictureBox
            // 
            this.IconPictureBox.BackgroundImage = global::UserInterface.Properties.Resources.icons8_add_new_16__1_;
            this.IconPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.IconPictureBox.Location = new System.Drawing.Point(0, 16);
            this.IconPictureBox.Name = "IconPictureBox";
            this.IconPictureBox.Size = new System.Drawing.Size(24, 24);
            this.IconPictureBox.TabIndex = 3;
            this.IconPictureBox.TabStop = false;
            this.IconPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IconPictureBox_MouseClick);
            // 
            // ImageSlotControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.IconPictureBox);
            this.Name = "ImageSlotControl";
            this.Size = new System.Drawing.Size(313, 43);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageSlotControl_Paint);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IconPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.PictureBox IconPictureBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miRemoveImage;
    }
}
