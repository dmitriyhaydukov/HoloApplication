using System;
using System.Drawing;
using System.Windows.Forms;
using System.Numerics;

public delegate void FormSph(double am, double Lambda, int NX, int NY, double dx, double dl, int k1);
//public delegate void ImageBoxADD(double am,  double Lambda, double dx, int k1);
//public delegate void ImageBoxMUL(double am,  double Lambda, double dx, int k1);

namespace rab1.Forms
{
   

    public partial class InterFormSph : Form
    {
        public event FormSph SpBox;
        public event FormSph SpBox1;
        public event FormSph SpBox2;

       
        private static double Lambda = 0.6;
        private static int NX = 512;
        private static int NY = 512;
        private static double dx = 0.05;
        private static double dl = 1.5;
        private static int k1 = 0;
        private static double am = 100.0;

        public InterFormSph()
        {
            InitializeComponent();
             textBox1.Text = Convert.ToString(k1);
             textBox2.Text = Convert.ToString(NX);
             textBox3.Text = Convert.ToString(NY);
             textBox4.Text = Convert.ToString(dx);
             textBox5.Text = Convert.ToString(Lambda);
             textBox6.Text = Convert.ToString(am);
             textBox7.Text = Convert.ToString(dl);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox1.Text);
            NX = Convert.ToInt32(textBox2.Text);
            NY = Convert.ToInt32(textBox3.Text);
            dx = Convert.ToDouble(textBox4.Text);
            Lambda = Convert.ToDouble(textBox5.Text);
            am = Convert.ToDouble(textBox6.Text);
            dl = Convert.ToDouble(textBox7.Text);
            SpBox(am, Lambda, NX, NY, dx * 1000.0, dl * 1000.0, k1);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)   // Параксиальное приближение (Гаусов пучок)
        {
            k1 = Convert.ToInt32(textBox1.Text);
            NX = Convert.ToInt32(textBox2.Text);
            NY = Convert.ToInt32(textBox3.Text);
            dx = Convert.ToDouble(textBox4.Text);
            Lambda = Convert.ToDouble(textBox5.Text);
            am = Convert.ToDouble(textBox6.Text);
            dl = Convert.ToDouble(textBox7.Text);
            SpBox1(am, Lambda, NX, NY, dx * 1000.0, dl * 1000.0, k1);
            Close();
        }

        private void button3_Click(object sender, EventArgs e)   // Разность
        {
            k1 = Convert.ToInt32(textBox1.Text);
            NX = Convert.ToInt32(textBox2.Text);
            NY = Convert.ToInt32(textBox3.Text);
            dx = Convert.ToDouble(textBox4.Text);
            Lambda = Convert.ToDouble(textBox5.Text);
            am = Convert.ToDouble(textBox6.Text);
            dl = Convert.ToDouble(textBox7.Text);
            SpBox2(am, Lambda, NX, NY, dx * 1000.0, dl * 1000.0, k1);
            Close();
        }
    }
}
