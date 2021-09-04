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
    public partial class Form_Filtr : Form
    {
        public delegate void VisualRegImageDelegate(int k);
        public static VisualRegImageDelegate VisualRegImage = null;      // Визуализация одного кадра от 0 до 11

        private static int k_filt = 3;                                   // Окно сглаживания
        private static int n = 1;                                        // Число сглаживаемых кадров
        public Form_Filtr()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(n);
            textBox2.Text = Convert.ToString(k_filt);                    // Окно сглаживания
        }

        private void button1_Click(object sender, EventArgs e)           // Cглаживание
        {
            n      = Convert.ToInt32(textBox1.Text);                     // По n кадрам начиная с regImage;
            k_filt = Convert.ToInt32(textBox2.Text);                     // Окно сглаживания;
            int k = Form1.regImage;                                      // Начальный кадр

            for (int i = k; i < n+k; i++)
             {
                if (Form1.zArrayDescriptor[i] == null) { MessageBox.Show("PSI zArrayDescriptor[" + i + "] == NULL"); return; }
                int nx = Form1.zArrayDescriptor[i].width;
                int ny = Form1.zArrayDescriptor[i].height;

                ZArrayDescriptor array = new ZArrayDescriptor(nx, ny);

                array = FiltrClass.Filt_smothingSM(Form1.zArrayDescriptor[i], k_filt);
                Form1.zArrayDescriptor[i] = array;
                VisualRegImage(i);
             }

            Close();
        }
    }
}
