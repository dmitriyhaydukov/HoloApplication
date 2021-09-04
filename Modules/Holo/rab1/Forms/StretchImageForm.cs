using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public delegate void UserChoosedSize(Size newSize);

namespace rab1
{
    public partial class StretchImageForm : Form
    {
        public Size initialSize;
        public event UserChoosedSize userChoosedSize;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public StretchImageForm()
        {
            InitializeComponent();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            int newWidth = Convert.ToInt32(this.textBox1.Text);
            int newHeight = Convert.ToInt32(this.textBox2.Text);

            userChoosedSize(new Size(newWidth, newHeight));

            this.Close();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void StretchImageForm_Shown(object sender, EventArgs e)
        {
            if(initialSize != null)
            {
                this.label1.Text = initialSize.Width + " x " + initialSize.Height;

                this.textBox1.Text = initialSize.Width.ToString();
                this.textBox2.Text = initialSize.Height.ToString();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (initialSize == null)
            {
                return;
            }

            float range = 1 - (float)((float)this.trackBar1.Value / (float)this.trackBar1.Maximum);
            int stretchedWidth = (int)((float)initialSize.Width * range);
            int stretchedHeight = (int)((float)initialSize.Height * range);

            this.textBox1.Text = stretchedWidth.ToString();
            this.textBox2.Text = stretchedHeight.ToString();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            float range = (float)(Convert.ToSingle(this.textBox1.Text) / (float)initialSize.Width);
            int stretchedHeight = (int)((float)initialSize.Height * range);
            this.textBox2.Text = stretchedHeight.ToString();

            correctTrackBar();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            float range = (float)(Convert.ToSingle(this.textBox2.Text) / (float)initialSize.Height);
            int stretchedWidth = (int)((float)initialSize.Width * range);
            this.textBox1.Text = stretchedWidth.ToString();

            correctTrackBar();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void correctTrackBar()
        {
            float range = (float)(Convert.ToSingle(this.textBox2.Text) / (float)initialSize.Height);
            if (range > 1)
            {
                trackBar1.Value = trackBar1.Minimum;
                return;
            }

            range = 1 - range;

            trackBar1.Value = (int)(range * 100);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
