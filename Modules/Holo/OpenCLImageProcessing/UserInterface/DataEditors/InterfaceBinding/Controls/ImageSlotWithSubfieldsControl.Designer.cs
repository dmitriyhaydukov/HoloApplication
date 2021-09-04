namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    partial class ImageSlotWithSubfieldsControl
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
            this.subfieldGroupControl1 = new UserInterface.DataEditors.InterfaceBinding.Controls.SubfieldGroupControl();
            this.imageSlotControl1 = new UserInterface.DataEditors.InterfaceBinding.Controls.ImageSlotControl();
            this.SuspendLayout();
            // 
            // subfieldGroupControl1
            // 
            this.subfieldGroupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subfieldGroupControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.subfieldGroupControl1.Location = new System.Drawing.Point(3, 52);
            this.subfieldGroupControl1.Name = "subfieldGroupControl1";
            this.subfieldGroupControl1.Size = new System.Drawing.Size(267, 95);
            this.subfieldGroupControl1.SubControlsPadding = 5;
            this.subfieldGroupControl1.TabIndex = 1;
            this.subfieldGroupControl1.Title = "Title";
            this.subfieldGroupControl1.Visible = false;
            this.subfieldGroupControl1.Resize += new System.EventHandler(this.subfieldGroupControl1_Resize);
            // 
            // imageSlotControl1
            // 
            this.imageSlotControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageSlotControl1.Location = new System.Drawing.Point(3, 3);
            this.imageSlotControl1.Name = "imageSlotControl1";
            this.imageSlotControl1.Size = new System.Drawing.Size(267, 43);
            this.imageSlotControl1.TabIndex = 0;
            this.imageSlotControl1.Title = "[Name]";
            this.imageSlotControl1.Value = null;
            // 
            // ImageSlotWithSubfieldsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.subfieldGroupControl1);
            this.Controls.Add(this.imageSlotControl1);
            this.Name = "ImageSlotWithSubfieldsControl";
            this.Size = new System.Drawing.Size(273, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageSlotControl imageSlotControl1;
        private SubfieldGroupControl subfieldGroupControl1;
    }
}
