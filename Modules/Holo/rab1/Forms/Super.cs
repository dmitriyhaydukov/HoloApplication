using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rab1.Forms
{
    //public delegate void VisualComplexDelegate(int k);
    //public delegate void VisualArrayDelegate();
    public partial class Super : Form
    {
        public static VisualComplexDelegate VisualComplex = null;
        public static VisualArrayDelegate   VisualArray = null;
        private static int n0 = 1;
        private static int k1 = 1;
        private static int k2 = 2;
        public Super()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(n0);
            textBox2.Text = Convert.ToString(k1);
            textBox3.Text = Convert.ToString(k2);
        }
/// <summary>
/// Добавление нулей по строкам
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            n0 = Convert.ToInt32(textBox1.Text);
            k1 = Convert.ToInt32(textBox2.Text);
            k2 = Convert.ToInt32(textBox3.Text);

            int nx = Form1.zComplex[k1 - 1].width;
            int ny = Form1.zComplex[k1 - 1].height;

            int n = n0 + 1;
            int nx2 = n * nx;
            ZComplexDescriptor rez = new ZComplexDescriptor(nx2, ny);

            for (int i = 0; i < nx2; i += n)
              for (int j = 0; j < ny; j++)
                { rez.array[i, j] = Form1.zComplex[k1-1].array[i / n, j]; }


            Form1.zComplex[k2 - 1] = rez;
            VisualComplex(k2 - 1);
        }
    }
}
