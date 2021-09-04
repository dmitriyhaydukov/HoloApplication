using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.WorkspacePanel
{
    public partial class WorkspacePanelItem : UserControl
    {
        private Color RegularBackground = SystemColors.Control;
        private Color SelectedBackground = SystemColors.Highlight;
        
        public bool Selected { get; private set; }
        public WorkspacePanelItem()
        {
            InitializeComponent();
        }

        public void SetTitle(string title, Font font = null, Color? foreColor = null)
        {
            TitleLabel.Text = title;

            if (font != null)
                TitleLabel.Font = font;

            if (foreColor.HasValue)
                TitleLabel.ForeColor = foreColor.Value;
        }

        public void SetInfo(string info)
        {
            InfoLabel.Text = info;
        }

        public void SetSelectionState(bool selected)
        {
            BackColor = selected ? SelectedBackground : RegularBackground;
            Selected = selected;
        }

        public void SetImage(Image image)
        {
            IconPictureBox.Image = image;
            IconPictureBox.Refresh();
        }

        private void IconPictureBox_DoubleClick(object sender, EventArgs e)
        {
            OnDoubleClick(e);
        }

        private void IconPictureBox_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }
    }
}
