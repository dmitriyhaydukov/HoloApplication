using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cloo;
using Infrastructure;
using Processing.Computing;

namespace UserInterface.Utility
{
    public partial class OpenClProgramCodeEditor : Form
    {
        private OpenClApplication OpenClApplication => Singleton.Get<OpenClApplication>();

        public OpenClProgramCodeEditor()
        {
            InitializeComponent();

            txtProgram.Click += (sender, args) => UpdateEditorInfo();

            txtProgram.TextChanged += (sender, args) => UpdateEditorInfo();
        }

        public void ReloadProgram()
        {
            txtProgram.Text = OpenClApplication.ProgramCode;
        }

        private void ProgramCodeEditor_Load(object sender, EventArgs e)
        {
            ReloadProgram();
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            ReloadProgram();
        }

        private void UpdateEditorInfo()
        {
            var cusorPos = txtProgram.SelectionStart;
            var line = txtProgram.Text.Substring(0, cusorPos).Count(c => c == '\n');
            lblEditorInfo.Text = "Line: " + line;
        }

        private void btnCheckAndSave_Click(object sender, EventArgs e)
        {
            var program = OpenClApplication.TryBuild(txtProgram.Text);

            txtLog.Text = program.GetBuildLog(OpenClApplication.ComputeContext.Devices.First()).Replace("\n", "\r\n");
            var buildStatus = program.GetBuildStatus(OpenClApplication.ComputeContext.Devices.First());
            txtLog.Text += "\r\n" + buildStatus;

            if (buildStatus == ComputeProgramBuildStatus.Success)
                OpenClApplication.ProgramCode = txtProgram.Text;
        }
    }
}