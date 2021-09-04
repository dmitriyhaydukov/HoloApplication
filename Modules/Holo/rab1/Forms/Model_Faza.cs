using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public delegate void ModelFz(double n1, double n2, double noise, double minIntensity);
public delegate void ModelFz3(double n1, double n2, double n3, double n12, double n23, double noise, double minIntensity);
public delegate void ModelFzS(int k1, int k2, int k3);

namespace rab1.Forms
{
    public partial class Model_Faza : Form
    {
        public event ModelFz OnModelFz;
        public event ModelFz OnModel_Sin_Fz;
        public event ModelFz3 OnModel_Sin_Fz3;

        public event ModelFzS OnModelFzSub;
        public event ModelFzS OnModelFzSubA;
        private static double n1 = 60;
        private static double n2 = 70;
        private static double n3 = 90;
        private static double n12 = 420;
        private static double n23 = 315;

        private static int k1 = 3;
        private static int k2 = 2;
        private static int k3 = 1;
        private static double noise = 0.2;

        
        public Model_Faza()
        {
            InitializeComponent();
            textBox1.Text  = Convert.ToString(n1);
            textBox5.Text  = Convert.ToString(n2);
            textBox9.Text  = Convert.ToString(n3);
            textBox10.Text = Convert.ToString(n12);
            textBox11.Text = Convert.ToString(n23);

            textBox2.Text = Convert.ToString(k1);
            textBox3.Text = Convert.ToString(k2);
            textBox4.Text = Convert.ToString(k3);
            textBox6.Text = Convert.ToString(noise);
        }

        private void button1_Click(object sender, EventArgs e)  // Сгенирировать фазу (пилу)
        {
            n1 = Convert.ToDouble(textBox1.Text);
            n2 = Convert.ToDouble(textBox5.Text);                       
            noise = Convert.ToDouble(textBox6.Text);

            double minIntensity = 0;

            OnModelFz(n1, n2, noise, minIntensity);
            Close();
        }

        private void button5_Click(object sender, EventArgs e)  // 4 синусоиды n1 и n2
        {
            n1 = Convert.ToDouble(textBox1.Text);
            n2 = Convert.ToDouble(textBox5.Text);
            noise = Convert.ToDouble(textBox6.Text);

            double minIntensity = 0;

            OnModel_Sin_Fz(n1, n2, noise, minIntensity);
            Close();
        }

        private void button6_Click(object sender, EventArgs e)  // 4 синусоиды n1, n2 и n3
        {
            n1 = Convert.ToDouble(textBox1.Text);
            n2 = Convert.ToDouble(textBox5.Text);
            n3 = Convert.ToDouble(textBox9.Text);
            noise = Convert.ToDouble(textBox6.Text);
            n12 = (double)(n1 * n2) / (Math.Abs(n1 - n2));  textBox10.Text = Convert.ToString(n12);
            n23 = (double)(n2 * n3) / (Math.Abs(n2 - n3));  textBox11.Text = Convert.ToString(n23);

            double minIntensity = 0;

            OnModel_Sin_Fz3(n1, n2, n3, n12, n23, noise, minIntensity);
            Close();
        }


        private void button2_Click(object sender, EventArgs e)  // Вычесть
        {
            k1 = Convert.ToInt32(textBox2.Text);
            k2 = Convert.ToInt32(textBox3.Text);
            k3 = Convert.ToInt32(textBox4.Text);
            OnModelFzSub(k1, k2, k3);
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox2.Text);
            k2 = Convert.ToInt32(textBox3.Text);
            k3 = Convert.ToInt32(textBox4.Text);
            OnModelFzSubA(k1, k2, k3);
            Close();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            n1 = Convert.ToDouble(textBox1.Text);
            n2 = Convert.ToDouble(textBox5.Text);
            double eq = (double)(n1 * n2) / (Math.Abs(n1 - n2));
            textBox7.Text = Convert.ToString(eq);
            double n = Math.Max(n1, n2);
            textBox8.Text = Convert.ToString(eq/n);
        }

       

        





    }
}
