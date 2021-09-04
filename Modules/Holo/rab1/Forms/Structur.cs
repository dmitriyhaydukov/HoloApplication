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
    public delegate void Correct(double L, double d, double d1);
    public delegate void Correct1(double L, double d, double d1, double x);
    public delegate void CorrectX(double d1, double X_max);
    public delegate void Scale(double x, int n);
    public delegate void Sub();
    public delegate void Sub_Line(int num);


    public partial class Structur : Form
    {
        public event Correct1 On_Corr;
        public event CorrectX  On_CorrX;
        public event Scale    On_Scale;
        public event Sub      On_Sub;
        public event Sub      On_Sub_Cos;
        public event Sub      On_Null;
        public event Sub_Line On_Sub_Line;
        public event Sub_Line On_Count_Line;  // Определить шаг полосы

   //     public event Correct1 On_Corr_Sub;

        private static double L = 1000;    // Расстояние до объекта
        private static double d = 900;    // Расстояние от начала объекта до камеры
        private static double d1 = 400;    // Размер объекта
        private static double x_max = 1;   // Максимальное смещение объекта

        private static int Number_line = 247;   // Максимальное смещение объекта

        public Structur()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(d);
            textBox2.Text = Convert.ToString(L);
            textBox3.Text = Convert.ToString(d1);
            textBox6.Text = Convert.ToString(x_max);
            textBox7.Text = Convert.ToString(Number_line);
        }
        /// <summary>
        /// L  - расстояние от  объекта до проектора
        /// d  - расстояние от камеры до начала объекта
        /// d1 - размер объекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void button2_Click(object sender, EventArgs e)  // Определить углы
        {
            d  = Convert.ToDouble(textBox1.Text);
            L  = Convert.ToDouble(textBox2.Text);
            d1 = Convert.ToDouble(textBox3.Text);

            double f1 = Math.Atan(L / d);       f1 = f1 * 180 / Math.PI;
            double f2 = Math.Atan(L / (d-d1));  f2 = f2 * 180 / Math.PI;
            textBox4.Text = Convert.ToString(f1);
            textBox5.Text = Convert.ToString(f2);
            Show();
        }

        private void button1_Click(object sender, EventArgs e)            // Скорректировать высоты
        {
            d = Convert.ToDouble(textBox1.Text);
            L = Convert.ToDouble(textBox2.Text);
            d1 = Convert.ToDouble(textBox3.Text);
            x_max = Convert.ToDouble(textBox6.Text);            // Максимальное смещение
            Number_line = Convert.ToInt32(textBox7.Text);         // Номер строки по которой проводится корректировка
            On_Corr(L, d, d1, x_max);
            Close();
        }
        // Теоретический прогиб
        private void button3_Click(object sender, EventArgs e)
        {
           
            d1    = Convert.ToDouble(textBox3.Text);  // Размер балки
            x_max = Convert.ToDouble(textBox6.Text);  // Максимальное отклонение
            On_CorrX(d1, x_max);
            Close();
        }

        private void button4_Click(object sender, EventArgs e)  // Масштабирование центрального окна
        {
            x_max       = Convert.ToDouble(textBox6.Text);
            Number_line = Convert.ToInt32(textBox7.Text);
            On_Scale(x_max, Number_line);
            Close();
        }
        // Разность полных фаз 1-2=>ArrayPicture
        private void button5_Click(object sender, EventArgs e) // Обычная разность 
        {
            On_Sub();
            Close();
        }
        private void button7_Click(object sender, EventArgs e)  // Разность  1-2=>ArrayPicture с помощью cos
        {
            On_Sub_Cos();
            Close();
        }
        // Разность полных фаз 1-2=>ArrayPicture c корректировкой значений
        private void button6_Click(object sender, EventArgs e)
        {
            d = Convert.ToDouble(textBox1.Text);
            L = Convert.ToDouble(textBox2.Text);
            d1 = Convert.ToDouble(textBox3.Text);
            x_max = Convert.ToDouble(textBox6.Text);   // Максимальное смещение
            //On_Corr_Sub(L, d, d1, x_max);
            Close();
        }

        private void button8_Click(object sender, EventArgs e)   // вычесть тренд
        {
            Number_line = Convert.ToInt32(textBox7.Text);
            On_Sub_Line(Number_line);
            Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Number_line = Convert.ToInt32(textBox7.Text);
            On_Count_Line(Number_line);
            Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            On_Null();
            Close();
        }
    }
}
