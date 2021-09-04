namespace rab1
{
    partial class GraphForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.hc = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.vc = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cb = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.hc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vc)).BeginInit();
            this.SuspendLayout();
            // 
            // hc
            // 
            this.hc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            chartArea3.Name = "ChartArea1";
            this.hc.ChartAreas.Add(chartArea3);
            this.hc.Location = new System.Drawing.Point(3, 0);
            this.hc.Name = "hc";
            series3.ChartArea = "ChartArea1";
            series3.Name = "Series1";
            this.hc.Series.Add(series3);
            this.hc.Size = new System.Drawing.Size(864, 300);
            this.hc.TabIndex = 0;
            this.hc.Text = "chart1";
            // 
            // vc
            // 
            this.vc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            chartArea4.Name = "ChartArea1";
            this.vc.ChartAreas.Add(chartArea4);
            this.vc.Location = new System.Drawing.Point(3, 306);
            this.vc.Name = "vc";
            series4.ChartArea = "ChartArea1";
            series4.Name = "Series1";
            this.vc.Series.Add(series4);
            this.vc.Size = new System.Drawing.Size(864, 300);
            this.vc.TabIndex = 1;
            this.vc.Text = "chart2";
            // 
            // cb
            // 
            this.cb.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cb.DisplayMember = "0";
            this.cb.FormattingEnabled = true;
            this.cb.Items.AddRange(new object[] {
            "Красный канал",
            "Зеленый канал",
            "Синий канал"});
            this.cb.Location = new System.Drawing.Point(320, 629);
            this.cb.Name = "cb";
            this.cb.Size = new System.Drawing.Size(219, 21);
            this.cb.TabIndex = 2;
            this.cb.ValueMember = "0";
            this.cb.SelectedIndexChanged += new System.EventHandler(this.cb_SelectedIndexChanged);
            // 
            // GraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 662);
            this.Controls.Add(this.cb);
            this.Controls.Add(this.vc);
            this.Controls.Add(this.hc);
            this.Name = "GraphForm";
            this.Text = "Form2";
            this.Resize += new System.EventHandler(this.GraphForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.hc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vc)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart hc;
        private System.Windows.Forms.DataVisualization.Charting.Chart vc;
        private System.Windows.Forms.ComboBox cb;
    }
}