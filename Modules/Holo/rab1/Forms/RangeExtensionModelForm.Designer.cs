namespace rab1
{
    partial class RangeExtensionModelForm
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
            this.ValueM1TextBox = new System.Windows.Forms.TextBox();
            this.ValueM1Label = new System.Windows.Forms.Label();
            this.ValueM2TextBox = new System.Windows.Forms.TextBox();
            this.ValueM2Label = new System.Windows.Forms.Label();
            this.MaxRangeLabel = new System.Windows.Forms.Label();
            this.MaxRangeValueLabel = new System.Windows.Forms.Label();
            this.BuildTableButton = new System.Windows.Forms.Button();
            this.RangeTextBox = new System.Windows.Forms.TextBox();
            this.RangeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ValueM1TextBox
            // 
            this.ValueM1TextBox.Location = new System.Drawing.Point(81, 22);
            this.ValueM1TextBox.Name = "ValueM1TextBox";
            this.ValueM1TextBox.Size = new System.Drawing.Size(122, 20);
            this.ValueM1TextBox.TabIndex = 0;
            this.ValueM1TextBox.TextChanged += new System.EventHandler(this.ValueM1TextBox_TextChanged);
            // 
            // ValueM1Label
            // 
            this.ValueM1Label.AutoSize = true;
            this.ValueM1Label.Location = new System.Drawing.Point(43, 25);
            this.ValueM1Label.Name = "ValueM1Label";
            this.ValueM1Label.Size = new System.Drawing.Size(25, 13);
            this.ValueM1Label.TabIndex = 1;
            this.ValueM1Label.Text = "M1:";
            // 
            // ValueM2TextBox
            // 
            this.ValueM2TextBox.Location = new System.Drawing.Point(81, 61);
            this.ValueM2TextBox.Name = "ValueM2TextBox";
            this.ValueM2TextBox.Size = new System.Drawing.Size(122, 20);
            this.ValueM2TextBox.TabIndex = 2;
            this.ValueM2TextBox.TextChanged += new System.EventHandler(this.ValueM2TextBox_TextChanged);
            // 
            // ValueM2Label
            // 
            this.ValueM2Label.AutoSize = true;
            this.ValueM2Label.Location = new System.Drawing.Point(42, 64);
            this.ValueM2Label.Name = "ValueM2Label";
            this.ValueM2Label.Size = new System.Drawing.Size(25, 13);
            this.ValueM2Label.TabIndex = 3;
            this.ValueM2Label.Text = "M2:";
            // 
            // MaxRangeLabel
            // 
            this.MaxRangeLabel.AutoSize = true;
            this.MaxRangeLabel.Location = new System.Drawing.Point(236, 25);
            this.MaxRangeLabel.Name = "MaxRangeLabel";
            this.MaxRangeLabel.Size = new System.Drawing.Size(60, 13);
            this.MaxRangeLabel.TabIndex = 4;
            this.MaxRangeLabel.Text = "Max range:";
            // 
            // MaxRangeValueLabel
            // 
            this.MaxRangeValueLabel.AutoSize = true;
            this.MaxRangeValueLabel.Location = new System.Drawing.Point(309, 25);
            this.MaxRangeValueLabel.Name = "MaxRangeValueLabel";
            this.MaxRangeValueLabel.Size = new System.Drawing.Size(86, 13);
            this.MaxRangeValueLabel.TabIndex = 5;
            this.MaxRangeValueLabel.Text = "Max range value";
            // 
            // BuildTableButton
            // 
            this.BuildTableButton.Location = new System.Drawing.Point(273, 96);
            this.BuildTableButton.Name = "BuildTableButton";
            this.BuildTableButton.Size = new System.Drawing.Size(122, 23);
            this.BuildTableButton.TabIndex = 6;
            this.BuildTableButton.Text = "Построить таблицу";
            this.BuildTableButton.UseVisualStyleBackColor = true;
            this.BuildTableButton.Click += new System.EventHandler(this.BuildTableButton_Click);
            // 
            // RangeTextBox
            // 
            this.RangeTextBox.Location = new System.Drawing.Point(81, 98);
            this.RangeTextBox.Name = "RangeTextBox";
            this.RangeTextBox.Size = new System.Drawing.Size(122, 20);
            this.RangeTextBox.TabIndex = 7;
            // 
            // RangeLabel
            // 
            this.RangeLabel.AutoSize = true;
            this.RangeLabel.Location = new System.Drawing.Point(26, 102);
            this.RangeLabel.Name = "RangeLabel";
            this.RangeLabel.Size = new System.Drawing.Size(42, 13);
            this.RangeLabel.TabIndex = 8;
            this.RangeLabel.Text = "Range:";
            // 
            // RangeExtensionModelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 132);
            this.Controls.Add(this.RangeLabel);
            this.Controls.Add(this.RangeTextBox);
            this.Controls.Add(this.BuildTableButton);
            this.Controls.Add(this.MaxRangeValueLabel);
            this.Controls.Add(this.MaxRangeLabel);
            this.Controls.Add(this.ValueM2Label);
            this.Controls.Add(this.ValueM2TextBox);
            this.Controls.Add(this.ValueM1Label);
            this.Controls.Add(this.ValueM1TextBox);
            this.Name = "RangeExtensionModelForm";
            this.Text = "RangeExtensionModelForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ValueM1TextBox;
        private System.Windows.Forms.Label ValueM1Label;
        private System.Windows.Forms.TextBox ValueM2TextBox;
        private System.Windows.Forms.Label ValueM2Label;
        private System.Windows.Forms.Label MaxRangeLabel;
        private System.Windows.Forms.Label MaxRangeValueLabel;
        private System.Windows.Forms.Button BuildTableButton;
        private System.Windows.Forms.TextBox RangeTextBox;
        private System.Windows.Forms.Label RangeLabel;
    }
}