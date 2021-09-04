namespace rab1
{
    partial class CurvesGraph
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelArrow = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApplyAll = new System.Windows.Forms.Button();
            this.txtStartImageNumber = new System.Windows.Forms.TextBox();
            this.txtEndImageNumber = new System.Windows.Forms.TextBox();
            this.btnApplyPhaseDifferenceCalculation = new System.Windows.Forms.Button();
            this.txtPhaseShift4 = new System.Windows.Forms.TextBox();
            this.txtPhaseShift3 = new System.Windows.Forms.TextBox();
            this.txtPhaseShift2 = new System.Windows.Forms.TextBox();
            this.txtPhaseShift1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClinLoad = new System.Windows.Forms.Button();
            this.txtRowNumber = new System.Windows.Forms.TextBox();
            this.lblRowNumber = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPhaseDifferenceCalculationForRow = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(430, 430);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDoubleClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // labelFrom
            // 
            this.labelFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelFrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelFrom.Location = new System.Drawing.Point(208, 438);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(95, 23);
            this.labelFrom.TabIndex = 9;
            // 
            // labelTo
            // 
            this.labelTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelTo.Location = new System.Drawing.Point(345, 438);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(85, 23);
            this.labelTo.TabIndex = 8;
            // 
            // labelArrow
            // 
            this.labelArrow.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelArrow.Location = new System.Drawing.Point(310, 437);
            this.labelArrow.Name = "labelArrow";
            this.labelArrow.Size = new System.Drawing.Size(30, 24);
            this.labelArrow.TabIndex = 7;
            this.labelArrow.Text = "->";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(208, 464);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(95, 23);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "Применить";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(345, 464);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApplyAll
            // 
            this.btnApplyAll.Location = new System.Drawing.Point(9, 462);
            this.btnApplyAll.Name = "btnApplyAll";
            this.btnApplyAll.Size = new System.Drawing.Size(178, 23);
            this.btnApplyAll.TabIndex = 3;
            this.btnApplyAll.Text = "Применить ко всем";
            this.btnApplyAll.UseVisualStyleBackColor = true;
            this.btnApplyAll.Click += new System.EventHandler(this.btnApplyAll_Click);
            // 
            // txtStartImageNumber
            // 
            this.txtStartImageNumber.Location = new System.Drawing.Point(9, 436);
            this.txtStartImageNumber.Name = "txtStartImageNumber";
            this.txtStartImageNumber.Size = new System.Drawing.Size(75, 20);
            this.txtStartImageNumber.TabIndex = 2;
            // 
            // txtEndImageNumber
            // 
            this.txtEndImageNumber.Location = new System.Drawing.Point(121, 436);
            this.txtEndImageNumber.Name = "txtEndImageNumber";
            this.txtEndImageNumber.Size = new System.Drawing.Size(66, 20);
            this.txtEndImageNumber.TabIndex = 1;
            // 
            // btnApplyPhaseDifferenceCalculation
            // 
            this.btnApplyPhaseDifferenceCalculation.Location = new System.Drawing.Point(208, 562);
            this.btnApplyPhaseDifferenceCalculation.Name = "btnApplyPhaseDifferenceCalculation";
            this.btnApplyPhaseDifferenceCalculation.Size = new System.Drawing.Size(222, 23);
            this.btnApplyPhaseDifferenceCalculation.TabIndex = 11;
            this.btnApplyPhaseDifferenceCalculation.Text = "Разность фаз (вычисление)";
            this.btnApplyPhaseDifferenceCalculation.UseVisualStyleBackColor = true;
            this.btnApplyPhaseDifferenceCalculation.Click += new System.EventHandler(this.btnApplyPhaseDifferenceCalculation_Click);
            // 
            // txtPhaseShift4
            // 
            this.txtPhaseShift4.Location = new System.Drawing.Point(147, 566);
            this.txtPhaseShift4.Name = "txtPhaseShift4";
            this.txtPhaseShift4.Size = new System.Drawing.Size(40, 20);
            this.txtPhaseShift4.TabIndex = 15;
            // 
            // txtPhaseShift3
            // 
            this.txtPhaseShift3.Location = new System.Drawing.Point(101, 566);
            this.txtPhaseShift3.Name = "txtPhaseShift3";
            this.txtPhaseShift3.Size = new System.Drawing.Size(40, 20);
            this.txtPhaseShift3.TabIndex = 14;
            // 
            // txtPhaseShift2
            // 
            this.txtPhaseShift2.Location = new System.Drawing.Point(55, 566);
            this.txtPhaseShift2.Name = "txtPhaseShift2";
            this.txtPhaseShift2.Size = new System.Drawing.Size(40, 20);
            this.txtPhaseShift2.TabIndex = 13;
            // 
            // txtPhaseShift1
            // 
            this.txtPhaseShift1.Location = new System.Drawing.Point(9, 566);
            this.txtPhaseShift1.Name = "txtPhaseShift1";
            this.txtPhaseShift1.Size = new System.Drawing.Size(40, 20);
            this.txtPhaseShift1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 541);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Фазовые сдвиги";
            // 
            // btnClinLoad
            // 
            this.btnClinLoad.Location = new System.Drawing.Point(208, 497);
            this.btnClinLoad.Name = "btnClinLoad";
            this.btnClinLoad.Size = new System.Drawing.Size(222, 23);
            this.btnClinLoad.TabIndex = 17;
            this.btnClinLoad.Text = "Загрузить клин";
            this.btnClinLoad.UseVisualStyleBackColor = true;
            this.btnClinLoad.Click += new System.EventHandler(this.btnClinLoad_Click);
            // 
            // txtRowNumber
            // 
            this.txtRowNumber.Location = new System.Drawing.Point(101, 494);
            this.txtRowNumber.Name = "txtRowNumber";
            this.txtRowNumber.Size = new System.Drawing.Size(85, 20);
            this.txtRowNumber.TabIndex = 19;
            // 
            // lblRowNumber
            // 
            this.lblRowNumber.AutoSize = true;
            this.lblRowNumber.Location = new System.Drawing.Point(10, 497);
            this.lblRowNumber.Name = "lblRowNumber";
            this.lblRowNumber.Size = new System.Drawing.Size(79, 13);
            this.lblRowNumber.TabIndex = 20;
            this.lblRowNumber.Text = "Номер строки";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(94, 439);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "-";
            // 
            // cbPhaseDifferenceCalculationForRow
            // 
            this.cbPhaseDifferenceCalculationForRow.AutoSize = true;
            this.cbPhaseDifferenceCalculationForRow.Location = new System.Drawing.Point(212, 532);
            this.cbPhaseDifferenceCalculationForRow.Name = "cbPhaseDifferenceCalculationForRow";
            this.cbPhaseDifferenceCalculationForRow.Size = new System.Drawing.Size(213, 17);
            this.cbPhaseDifferenceCalculationForRow.TabIndex = 22;
            this.cbPhaseDifferenceCalculationForRow.Text = "Вычислять разность фаз для строки";
            this.cbPhaseDifferenceCalculationForRow.UseVisualStyleBackColor = true;
            // 
            // CurvesGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(436, 594);
            this.Controls.Add(this.cbPhaseDifferenceCalculationForRow);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblRowNumber);
            this.Controls.Add(this.txtRowNumber);
            this.Controls.Add(this.btnClinLoad);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPhaseShift4);
            this.Controls.Add(this.txtPhaseShift3);
            this.Controls.Add(this.txtPhaseShift2);
            this.Controls.Add(this.txtPhaseShift1);
            this.Controls.Add(this.btnApplyPhaseDifferenceCalculation);
            this.Controls.Add(this.txtEndImageNumber);
            this.Controls.Add(this.txtStartImageNumber);
            this.Controls.Add(this.btnApplyAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.labelArrow);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.labelFrom);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CurvesGraph";
            this.Opacity = 0.9D;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CurvesGraph_FormClosing);
            this.Load += new System.EventHandler(this.CurvesGraph_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelArrow;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApplyAll;
        private System.Windows.Forms.TextBox txtStartImageNumber;
        private System.Windows.Forms.TextBox txtEndImageNumber;
        private System.Windows.Forms.Button btnApplyPhaseDifferenceCalculation;
        private System.Windows.Forms.TextBox txtPhaseShift4;
        private System.Windows.Forms.TextBox txtPhaseShift3;
        private System.Windows.Forms.TextBox txtPhaseShift2;
        private System.Windows.Forms.TextBox txtPhaseShift1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClinLoad;
        private System.Windows.Forms.TextBox txtRowNumber;
        private System.Windows.Forms.Label lblRowNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbPhaseDifferenceCalculationForRow;
    }
}