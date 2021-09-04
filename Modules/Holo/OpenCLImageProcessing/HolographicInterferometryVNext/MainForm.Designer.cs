namespace HolographicInterferometryVNext
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataEditor1 = new UserInterface.DataEditors.DataEditor();
            this.workspacePanel1 = new UserInterface.WorkspacePanel.WorkspacePanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.inputMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editOpenClProgramCode = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataEditor1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.workspacePanel1);
            this.splitContainer1.Size = new System.Drawing.Size(1167, 571);
            this.splitContainer1.SplitterDistance = 937;
            this.splitContainer1.TabIndex = 1;
            // 
            // dataEditor1
            // 
            this.dataEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataEditor1.Location = new System.Drawing.Point(0, 0);
            this.dataEditor1.Name = "dataEditor1";
            this.dataEditor1.Size = new System.Drawing.Size(937, 571);
            this.dataEditor1.TabIndex = 0;
            // 
            // workspacePanel1
            // 
            this.workspacePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workspacePanel1.Location = new System.Drawing.Point(0, 0);
            this.workspacePanel1.Name = "workspacePanel1";
            this.workspacePanel1.SelectItemOnClick = false;
            this.workspacePanel1.ShowToolbar = true;
            this.workspacePanel1.Size = new System.Drawing.Size(226, 571);
            this.workspacePanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputMenuItem,
            this.processingMenuItem,
            this.settingsMenuItem,
            this.helpMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1167, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // inputMenuItem
            // 
            this.inputMenuItem.Name = "inputMenuItem";
            this.inputMenuItem.Size = new System.Drawing.Size(45, 20);
            this.inputMenuItem.Text = "Ввод";
            // 
            // processingMenuItem
            // 
            this.processingMenuItem.Name = "processingMenuItem";
            this.processingMenuItem.Size = new System.Drawing.Size(79, 20);
            this.processingMenuItem.Text = "Обработка";
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editOpenClProgramCode});
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Size = new System.Drawing.Size(79, 20);
            this.settingsMenuItem.Text = "Настройки";
            // 
            // editOpenClProgramCode
            // 
            this.editOpenClProgramCode.Name = "editOpenClProgramCode";
            this.editOpenClProgramCode.Size = new System.Drawing.Size(145, 22);
            this.editOpenClProgramCode.Text = "OpenCl Code";
            this.editOpenClProgramCode.ToolTipText = "Редактировать исходный код приложения OpenCl";
            this.editOpenClProgramCode.Click += new System.EventHandler(this.editOpenClProgramCode_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(65, 20);
            this.helpMenuItem.Text = "Справка";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 595);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1167, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 617);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "HI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private UserInterface.WorkspacePanel.WorkspacePanel workspacePanel1;
        private UserInterface.DataEditors.DataEditor dataEditor1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem processingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editOpenClProgramCode;
        private System.Windows.Forms.ToolStripMenuItem inputMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

