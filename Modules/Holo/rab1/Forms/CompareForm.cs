using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rab1.Forms
{
    public partial class CompareForm : Form
    {
        public CompareForm()
        {
            InitializeComponent();
        }

        private void phaseMapImage_MouseClick(object sender, MouseEventArgs e)
        {
            ImageHelper.drawGraph(((PictureBox)sender).Image, e.X, e.Y);
        }
    }
}
