using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public partial class SubfieldGroupControl : UserControl
    {
        public string Title
        {
            get => lblTitle.Text;
            set => lblTitle.Text = value;
        }

        public int SubControlsPadding { get; set; } = 5;

        public SubfieldGroupControl()
        {
            InitializeComponent();
        }

        public void FillControls(Binder subfieldBinder, bool rootLevel = false)
        {
            subfieldsPanel.Controls.Clear();
            subfieldBinder.FillControls(subfieldsPanel);
            var visible = false;
            foreach (Control subfieldsPanelControl in subfieldsPanel.Controls)
            {
                subfieldsPanelControl.Resize += (sender, args) => RecalculateControlsSize();
                if (subfieldsPanelControl.Visible)
                    visible = true;
            }

            Visible = visible;
            Resize += (sender, args) => RecalculateControlsSize();

            Toggle(rootLevel);
        }

        public void Toggle(bool? show)
        {
            var showContent = show.HasValue && show.Value || !show.HasValue && !subfieldsPanel.Visible;

            if (showContent)
                ShowContent();
            else
                HideContent();
        }

        private void ShowContent()
        {
            btnExpand.Image = Properties.Resources.arrow_state_blue_collapsed_8599;
            subfieldsPanel.Show();
            OnResize(new EventArgs());
        }

        private void HideContent()
        {
            btnExpand.Image = Properties.Resources.arrow_state_blue_expanded_4097;
            subfieldsPanel.Hide();
            OnResize(new EventArgs());
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            Toggle(null);
        }

        private void RecalculateControlsSize()
        {
            var height = 0;
            if (subfieldsPanel.Visible)
            {
                foreach (Control subfieldsPanelControl in subfieldsPanel.Controls)
                {
                    height += subfieldsPanelControl.Height + SubControlsPadding;
                    subfieldsPanelControl.Width = subfieldsPanel.ClientSize.Width - subfieldsPanelControl.Padding.Right - subfieldsPanelControl.Padding.Left - 10;
                }
            }
            Height = subfieldsPanel.Top + height;
        }
    }
}
