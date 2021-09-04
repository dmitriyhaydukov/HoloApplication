namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    partial class NumberControl
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtCurrent = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnAdd10 = new System.Windows.Forms.Button();
            this.btnSubstract = new System.Windows.Forms.Button();
            this.btnSubstract10 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitle.Location = new System.Drawing.Point(0, 2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(32, 13);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title";
            // 
            // txtCurrent
            // 
            this.txtCurrent.Location = new System.Drawing.Point(75, 22);
            this.txtCurrent.Name = "txtCurrent";
            this.txtCurrent.Size = new System.Drawing.Size(59, 20);
            this.txtCurrent.TabIndex = 4;
            this.txtCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(140, 20);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(29, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnAdd10
            // 
            this.btnAdd10.Location = new System.Drawing.Point(175, 20);
            this.btnAdd10.Name = "btnAdd10";
            this.btnAdd10.Size = new System.Drawing.Size(29, 23);
            this.btnAdd10.TabIndex = 6;
            this.btnAdd10.Text = "++";
            this.btnAdd10.UseVisualStyleBackColor = true;
            this.btnAdd10.Click += new System.EventHandler(this.btnAdd10_Click);
            // 
            // btnSubstract
            // 
            this.btnSubstract.Location = new System.Drawing.Point(40, 20);
            this.btnSubstract.Name = "btnSubstract";
            this.btnSubstract.Size = new System.Drawing.Size(29, 23);
            this.btnSubstract.TabIndex = 8;
            this.btnSubstract.Text = "-";
            this.btnSubstract.UseVisualStyleBackColor = true;
            this.btnSubstract.Click += new System.EventHandler(this.btnSubstract_Click);
            // 
            // btnSubstract10
            // 
            this.btnSubstract10.Location = new System.Drawing.Point(5, 20);
            this.btnSubstract10.Name = "btnSubstract10";
            this.btnSubstract10.Size = new System.Drawing.Size(29, 23);
            this.btnSubstract10.TabIndex = 7;
            this.btnSubstract10.Text = "--";
            this.btnSubstract10.UseVisualStyleBackColor = true;
            this.btnSubstract10.Click += new System.EventHandler(this.btnSubstract10_Click);
            // 
            // SliderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSubstract);
            this.Controls.Add(this.btnSubstract10);
            this.Controls.Add(this.btnAdd10);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtCurrent);
            this.Controls.Add(this.lblTitle);
            this.Name = "SliderControl";
            this.Size = new System.Drawing.Size(365, 46);
            this.Load += new System.EventHandler(this.SliderControl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SliderControl_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtCurrent;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnAdd10;
        private System.Windows.Forms.Button btnSubstract;
        private System.Windows.Forms.Button btnSubstract10;
    }
}
