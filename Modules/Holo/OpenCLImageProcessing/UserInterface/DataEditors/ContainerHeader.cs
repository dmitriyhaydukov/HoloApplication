using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataEditors
{
    public partial class ContainerHeader : UserControl
    {
        private bool _active;
        public Color ActiveFontColor => Color.White;
        public Color ActiveBackgroundColor => Color.SteelBlue;
        public Color InactiveFontColor => Color.Black;
        public Color InactiveBackgroundColor => Color.LightGray;

        public bool Active
        {
            get => _active;
            set
            {
                _active = value;

                HeaderLabel.ForeColor = _active ? ActiveFontColor : InactiveFontColor;
                BackColor = _active ? ActiveBackgroundColor : InactiveBackgroundColor;
            }
        }

        public override string Text
        {
            get => HeaderLabel.Text;
            set => HeaderLabel.Text = value;
        }

        public ContainerHeader()
        {
            InitializeComponent();
        }

        private void ContainerHeader_Load(object sender, EventArgs e)
        {
            Active = _active;
        }
    }
}