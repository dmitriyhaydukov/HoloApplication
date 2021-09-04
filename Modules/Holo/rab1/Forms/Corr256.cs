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
    //public delegate void VisualRegImageDelegate(int k);                  // Для VisualRegImage 

    public partial class Corr256 : Form
    {
        public static VisualRegImageDelegate VisualRegImage = null;    // Визуализация одного кадра от 0 до 11 из main
        private static int k1 = 1;
        private static int k2 = 2;

        private static int k3 = 1;
        private static int k4 = 2;
        private static int k5 = 3;

        private static int k6 = 3;
        private static int k7 = 4;

        private static int k8 = 1;
        private static int k9 = 6;
        private static int k10 = 7;

        private static int k11 = 1;
        private static int k12 = 2;
        private static int k13 = 3;
        private static int k14 = 4;

        public Corr256()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(k1);     // Номер кадра
            textBox2.Text = Convert.ToString(k2);     // Номер кадра

            textBox3.Text = Convert.ToString(k3);     // Номер кадра
            textBox4.Text = Convert.ToString(k4);     // Номер кадра
            textBox5.Text = Convert.ToString(k5);

            textBox6.Text = Convert.ToString(k6);     // Номер кадра
            textBox7.Text = Convert.ToString(k7);     // Номер кадра

            textBox8.Text = Convert.ToString(k8);     // Номер кадра
            textBox9.Text = Convert.ToString(k9);     // Номер кадра
            textBox10.Text = Convert.ToString(k10);

            textBox11.Text = Convert.ToString(k11);     // Номер кадра
            textBox12.Text = Convert.ToString(k12);     // Номер кадра
            textBox13.Text = Convert.ToString(k13);
            textBox14.Text = Convert.ToString(k14);
        }
        /// <summary>
        /// Зарегистрированные синусоиды => идеальные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox2.Text);

            if (Form1.zArrayDescriptor[k1 - 1] == null) { MessageBox.Show("Corr256 zArrayDescriptor[" + (k1 - 1) + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k1 - 1].width;
            int ny = 256;


            ZArrayDescriptor rez = new ZArrayDescriptor(nx, ny);

            double[] summ_sin = new double[nx];
            double[] summ_cos = new double[nx];
            double[] s1 = new double[nx];

            double[] fzr = new double[ny];
            double[] sn  = new double[ny];

            //double  max_s=0;
            //double  min_s=0;
            double max = double.MinValue; 
            double min = double.MaxValue;
            /*      
                                    for (int i = 0; i < nx; i++)                                                          
                                    {
                                       //max = double.MinValue; min = double.MaxValue;
                                       for (int j = 0; j < ny; j++)
                                        {
                                          double a = Form1.zArrayDescriptor[k1 - 1].array[i, j];
                                          if (a > max) max = a; if (a < min) min = a;
                                        }
                                       //max_s += max; min_s += min;
                                   }
                                //max = max_s / nx; min = min_s / nx;
           */
            max = 245; min = 0;
            MessageBox.Show("Max = " + max + "Min = " + min);

            for (int j = 0; j < ny; j++) { int i = j; if (i >= 256) { i = i - 256; } fzr[i] = 2 * Math.PI * i / 256; }

            for (int i = 0; i < nx; i++)                                                           // После нагрузки
            {
                for (int j = 0; j < ny; j++)
                {
                    summ_sin[i] += Form1.zArrayDescriptor[k1 - 1].array[i, j] * Math.Sin(fzr[j]);
                    summ_cos[i] += Form1.zArrayDescriptor[k1 - 1].array[i, j] * Math.Cos(fzr[j]);
                }
                s1[i] = Math.Atan2(summ_cos[i], summ_sin[i]);
            } 

            for (int i = 0; i < nx; i++)
            {
              
                for (int k = 0; k < ny; k++) { sn[k] = (max-min)* (Math.Sin(2.0 * Math.PI *k / 256 + s1[i]) + 1.0) / 2.0 + min; }

                for (int j = 0; j < ny; j++)
                {
                    //   cmpl.array[i, j] = s1[i];
                    rez.array[i, j] = sn[j];
                }
            }


            Form1.zArrayDescriptor[k2 - 1] = rez;
            VisualRegImage(k2 - 1);
          //  Close();
        }
/// <summary>
/// Синусоиды с поправками
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            k11 = Convert.ToInt32(textBox11.Text);
            k12 = Convert.ToInt32(textBox12.Text);
            k13 = Convert.ToInt32(textBox13.Text);
            k14 = Convert.ToInt32(textBox14.Text);


            if (Form1.zArrayDescriptor[k1 - 1] == null) { MessageBox.Show("Corr256 zArrayDescriptor[" + (k1 - 1) + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k1 - 1].width;
            int ny = 256;

           

            double max = double.MinValue;
            double min = double.MaxValue;

            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++)
                {
                    double a = Form1.zArrayDescriptor[k11 - 1].array[i, j];
                    if (a > max) max = a; if (a < min) min = a;
                    a = Form1.zArrayDescriptor[k12 - 1].array[i, j];
                    if (a > max) max = a; if (a < min) min = a;
                }
            }

            double[] summ_sin = new double[nx];
            double[] summ_cos = new double[nx];
            double[] sn = new double[ny];
            double[] s1 = new double[nx];
            double[] s2 = new double[nx];
            double[] fzr = new double[ny];

            for (int j = 0; j < ny; j++) { int i = j; if (i >= 256) { i = i - 256; } fzr[i] = 2 * Math.PI * i / 256; }

            for (int i = 0; i < nx; i++)                                                           // До нагрузки
            {
                for (int j = 0; j < ny; j++)
                {
                    summ_sin[i] += Form1.zArrayDescriptor[k11 - 1].array[i, j] * Math.Sin(fzr[j]);
                    summ_cos[i] += Form1.zArrayDescriptor[k11 - 1].array[i, j] * Math.Cos(fzr[j]);
                }
                s2[i] = Math.Atan2(summ_cos[i], summ_sin[i]);
            }

            ZArrayDescriptor rez1 = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++) s1[i] = Form1.zArrayDescriptor[k13 - 1].array[i, 50];

            for (int i = 0; i < nx; i++)
            {
                for (int k = 0; k < ny; k++) { sn[k] = (max - min) * (Math.Sin(2.0 * Math.PI * k / 256 + s1[i]+ s2[i]) + 1.0) / 2.0 + min; }
                for (int j = 0; j < ny; j++) rez1.array[i, j] = sn[j];
            }

            Form1.zArrayDescriptor[k14 - 1] = rez1;

            ZArrayDescriptor rez2 = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)                                                           // После нагрузки
            {
                for (int j = 0; j < ny; j++)
                {
                    summ_sin[i] += Form1.zArrayDescriptor[k12 - 1].array[i, j] * Math.Sin(fzr[j]);
                    summ_cos[i] += Form1.zArrayDescriptor[k12 - 1].array[i, j] * Math.Cos(fzr[j]);
                }
                s2[i] = Math.Atan2(summ_cos[i], summ_sin[i]);
            }

            for (int i = 0; i < nx; i++) s1[i] = Form1.zArrayDescriptor[k13 - 1].array[i, 150];

            for (int i = 0; i < nx; i++)
            {
                for (int k = 0; k < ny; k++) { sn[k] = (max - min) * (Math.Sin(2.0 * Math.PI * k / 256 + s1[i]+  s2[i]) + 1.0) / 2.0 + min; }
                for (int j = 0; j < ny; j++) rez2.array[i, j] = sn[j];
            }

            Form1.zArrayDescriptor[k14 - 1 +1 ] = rez2;
            VisualRegImage(k14 - 1);
            VisualRegImage(k14 );
        }

        /// <summary>
        /// Гистограмм 256 значений (256х256)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button1_Click(object sender, EventArgs e)
        {
            k3 = Convert.ToInt32(textBox3.Text);
            k4 = Convert.ToInt32(textBox4.Text);
            k5 = Convert.ToInt32(textBox5.Text);

            if (Form1.zArrayDescriptor[k3 - 1] == null) { MessageBox.Show("Corr256 zArrayDescriptor[" + (k3 - 1) + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k4 - 1] == null) { MessageBox.Show("Corr256 zArrayDescriptor[" + (k4 - 1) + "] == NULL"); return; }


            int nx = Form1.zArrayDescriptor[k3 - 1].width;    // 
            int ny = 256;

            ZArrayDescriptor rez = new ZArrayDescriptor(256, 256);

            for (int i = 0; i < nx; i++)                                                           // После нагрузки
            {
                for (int j = 0; j < ny; j++)
                {
                    int intens0 = (Int32)Form1.zArrayDescriptor[k3 - 1].array[i, j];
                    int intens1 = (Int32)Form1.zArrayDescriptor[k4 - 1].array[i, j];
                    rez.array[intens1, intens0] += 1;
                }
               
            }

            Form1.zArrayDescriptor[k5 - 1] = rez;
            VisualRegImage(k5 - 1);
           // Close();
        }
/// <summary>
/// Выделение максимумов
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            k6 = Convert.ToInt32(textBox6.Text);
            k7 = Convert.ToInt32(textBox7.Text);

            if (Form1.zArrayDescriptor[k6 - 1] == null) { MessageBox.Show("Corr256 zArrayDescriptor[" + (k6 - 1) + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k6 - 1].width;   
            int ny = Form1.zArrayDescriptor[k6 - 1].height;
            ZArrayDescriptor rez1 = new ZArrayDescriptor(256, 256);
            ZArrayDescriptor rez2 = new ZArrayDescriptor(256, 256);
            ZArrayDescriptor rez3 = new ZArrayDescriptor(256, 256);

            for (int j = 0; j < ny; j++)
            {
                double max = Form1.zArrayDescriptor[k6 - 1].array[0, j]; ;
                int i_max = int.MinValue;
                for (int i = 0; i < nx; i++)                                                           
                {
                    double a = Form1.zArrayDescriptor[k6 - 1].array[i, j];
                    if (a > max) { max = a; i_max = i; }             
                }
                if (i_max >= 0 ) rez1.array[i_max, j] = 255;

            }
            
            for (int i = 0; i < nx; i++)
            {
                double max = Form1.zArrayDescriptor[k6 - 1].array[i, 0]; ;
                int i_max = int.MinValue;
                for (int j = 0; j < ny; j++)                                                         
                {
                    double a = Form1.zArrayDescriptor[k6 - 1].array[i, j];
                    if (a > max) { max = a; i_max = j; }
                }
                if (i_max >= 0) rez2.array[i, i_max] = 255;

            }
            //MessageBox.Show("1");
           
            for (int i = 0; i < nx; i++)
            {
                int k = -2;
                int i_max1= -2, i_max2 = -2; 
                for (int j = 0; j < ny; j++) { if (rez1.array[i, j] == 255) { i_max1 = j; break; } }
                for (int j = 0; j < ny; j++) { if (rez2.array[i, j] == 255) { i_max2 = j; break; } }
               
                if (i_max1 != i_max2)
                 {
                    if (i_max1 > i_max2) k = i_max2 + (i_max1 - i_max2) / 2;
                    if (i_max1 < i_max2) k = i_max1 + (i_max2 - i_max1) / 2;
                 }
                if (i_max1 >= 0) { if (i_max2 < 0)  k = i_max1; }
                if (i_max2 >= 0) { if (i_max1 < 0)  k = i_max2; }
                if (i_max2 >= 0) { if (i_max1 >= 0) k = i_max1; }
                if (k >= 0 )  rez3.array[i, k] = 255;
                //if (i_max1 >= 0) rez3.array[i, i_max1] = 200;
                //if (i_max2 >= 0) rez3.array[i, i_max2] = 180;
            }
            //MessageBox.Show("2");
            Form1.zArrayDescriptor[k7 - 1] = rez1;
            Form1.zArrayDescriptor[k7 - 1+1] = rez2;
            Form1.zArrayDescriptor[k7 - 1 + 2] = rez3;

            VisualRegImage(k7 - 1);
            VisualRegImage(k7 - 1+1);
            VisualRegImage(k7 - 1+2);

           // Close();

        }
/// <summary>
/// Выделение границ
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {

            k6 = Convert.ToInt32(textBox6.Text);
            k7 = Convert.ToInt32(textBox7.Text);

            if (Form1.zArrayDescriptor[k6 - 1] == null) { MessageBox.Show("Corr256 zArrayDescriptor[" + (k6 - 1) + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k6 - 1].width;
            int ny = Form1.zArrayDescriptor[k6 - 1].height;
            ZArrayDescriptor rez1 = new ZArrayDescriptor(256, 256);
            ZArrayDescriptor rez2 = new ZArrayDescriptor(256, 256);
            ZArrayDescriptor rez3 = new ZArrayDescriptor(256, 256);

            for (int i = 0; i < nx; i++) { rez1.array[i, i] = 128; }
           
            for (int i = 0; i < nx; i++)
            {
                int j_max1 = -1;
                int j_max2 = -1;
                int k = 0;
                for (int j = 0; j < ny; j++)
                {
                    double a = Form1.zArrayDescriptor[k6 - 1].array[i, j];
                    if (a !=0 ) { rez1.array[i, j] = 200; j_max1 = j;  break; }
                }

                for (int j = ny-1; j >= 0; j--)
                {
                    double a = Form1.zArrayDescriptor[k6 - 1].array[i, j];
                    if (a != 0) { rez1.array[i, j] = 200; j_max2 = j;  break; }
                }
                if ((j_max1 > 0) && (j_max2 > 0))
                {
                    k = j_max1 + (j_max2 - j_max1) / 2;
                    rez1.array[i, k] = 255;
                }
            }


            for (int i = 0; i < nx; i++)   // Убираем границы
            {
                for (int j = 0; j < ny; j++)
                {
                    double a = rez1.array[i, j];
                    if (a == 200) { rez2.array[i, j] = 0; } else { rez2.array[i, j] = a; }
                }
            }

            for (int i = 0; i < nx; i++)   // Убираем среднюю линию
            {
                int j_max1 = -1;
                int j_max2 = -1;
                for (int j = 0; j < ny; j++)
                {
                    double a = rez2.array[i, j];
                    if (a == 255) { j_max1 = j; }
                    if (a == 128) { j_max2 = j; }
                }
                if ((j_max1 > 0) && (j_max2 > 0)) {  rez3.array[i, j_max1] = 255; }
                  else
                    {
                       if ((j_max1 < 0) && (j_max2 > 0)) { rez3.array[i, j_max2] = 255; }
                    }
            }

            Form1.zArrayDescriptor[k7 - 1] = rez1;
            VisualRegImage(k7 - 1);
            Form1.zArrayDescriptor[k7 - 1 +1 ] = rez2;
            VisualRegImage(k7 - 1 + 1);
            Form1.zArrayDescriptor[k7 - 1 + 2] = rez3;
            VisualRegImage(k7 - 1 + 2);
            // Close();
        }




        /// <summary>
        /// Коррекция исходной синусоиды
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            k8 = Convert.ToInt32(textBox8.Text);
            k9 = Convert.ToInt32(textBox9.Text);
            k10 = Convert.ToInt32(textBox10.Text);

            if (Form1.zArrayDescriptor[k8 - 1] == null) { MessageBox.Show("Corr256 zArrayDescriptor[" + (k8 - 1) + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k9 - 1] == null) { MessageBox.Show("Corr256 zArrayDescriptor[" + (k9 - 1) + "] == NULL"); return; }


            int nx = Form1.zArrayDescriptor[k8 - 1].width;    // 
            int ny = Form1.zArrayDescriptor[k8 - 1].height;

            ZArrayDescriptor rez = new ZArrayDescriptor(nx, ny);
            int[] c1 = new int[256];

            for (int j = 0; j < 255; j++) // Таблица перекодировок (0-255) из массива 256х256
            {
                
                for (int i = 0; i < 255; i++)
                {
                    c1[j] = -2;
                    int a = (int)Form1.zArrayDescriptor[k9 - 1].array[i, j];
                    if (a != 0) { c1[j] = i; break; }
                }
            }
 /*           ZArrayDescriptor rez = new ZArrayDescriptor(256, 256);
            for (int j = 0; j < 255; j++)                                  // Таблица перекодировок (0-255)
                for (int i = 0; i < 255; i++)
                {
                   
                    int a = c1[j];
                    if (a > 0) { rez.array[a, j] = 255;  }
                }
                */
     
                        for (int i = 0; i < nx; i++)                                                           
                        {
                            for (int j = 0; j < ny; j++)
                            {
                                int a =(int) Form1.zArrayDescriptor[k8 - 1].array[i, j];                        // a (0-255)
                                int b = c1[a];

                                if (b < 0) rez.array[i, j] = a;   else    rez.array[i, j] = b;
                            }

                        }

            Form1.zArrayDescriptor[k10 - 1] = rez;
            VisualRegImage(k10 - 1);
            Close();
            // MessageBox.Show(" a " + a + " c1[a] " + b);
            //MessageBox.Show("j " + j + " i " + i + " a " + a);
        }

       
    }
}
