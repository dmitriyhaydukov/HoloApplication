using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace rab1.Forms
{

    public partial class Graphic2D : Form
    {

        public Form1 MainForm = null;
        private static double[][] buf = new double[4][];        // массивы для строк из 4 окон
        private static double[][] buf_gl = new double[4][];     // массивы для вывода из 4 окон

        double maxx = double.MinValue, minx = double.MaxValue;  // Диапазон графиков
       

        private static int[] k   = {1, 0, 0, 0 };         // Флаги для вывода графиков
       




        //private double[] bf_gl;           // Масштабированные значений (не меняются)
        //private double[] bf1_gl;          // Истинные значения (не меняются)

        //private double[] buf_gl;           // Масштабированные значений (меняются от значения step)
        //private double[] buf1_gl;          // Истинные значения (меняются от значения step)

         private static int w;                     // Размер массива (меняются от значения step)
         private static int ww;                    // Размер массива (не меняются)
       
        int ixx = 0;                      //  Начальное значение (меняется ScrollBar)
        int step = 1;                     //  Шаг для уменьшения или увеличения графика

        
        Graphics grBack;

       

        //static int k = 0;                   // Для рисования зеленой линии, двигающейся по y
        //static int xc = 0;                   // 
        //static int yc = 0;                   // 



        public Graphic2D(Form1 mainForm)
        {
            
            InitializeComponent();

            this.MainForm = mainForm;


        // w  = w1;
        // ww = w1;

        // buf_gl = new double[w1];                                 // Масштабированные значений
        // buf1_gl = new double[w1];                                // Истинные значения
        // bf_gl = new double[w1];                                  // Масштабированные значений
        // bf1_gl = new double[w1];                                 // Истинные значения






        // for (int i = 0; i < w1; i++) { double b = buf[i]; buf1_gl[i] = b; if (b < minx) minx = b; if (b > maxx) maxx = b; buf1_gl[i] = b; }


        // for (int i = 0; i < w1; i++) { buf_gl[i] = (buf[i] - minx) * hh / (maxx - minx); }

        // for (int i = 0; i < w1; i++) { bf_gl[i] = buf_gl[i]; }
        // for (int i = 0; i < w1; i++) { bf1_gl[i] = buf1_gl[i]; }
        // ixx = 0;


    }
        /// <summary>
        /// Обработка флажков для задания кадров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
           { CheckBox checkBox = (CheckBox)sender; if (checkBox.Checked == true) { k[0] = 1; } else { k[0] = 0; } }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
           { CheckBox checkBox = (CheckBox)sender; if (checkBox.Checked == true) { k[1] = 1; } else { k[1] = 0; } }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
           { CheckBox checkBox = (CheckBox)sender; if (checkBox.Checked == true) { k[2] = 1; } else { k[2] = 0; } }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
           { CheckBox checkBox = (CheckBox)sender; if (checkBox.Checked == true) { k[3] = 1; } else { k[3] = 0; } }

        /// <summary>
        /// Обработка кнопки "Построить график из файлов расположенных в zArrayDescriptor[0], [1], [2], [3]"
        /// </summary>
        /// В Form1 должна быть установлена X1 - координаты графиков
        /// <param name="sender"></param>
        /// <param name="e"></param>       

        private void button3_Click(object sender, EventArgs e)                      // из файлов
            {
            if (checkBox1.Checked == true) { k[0] = 1; } else { k[0] = 0; }
            if (checkBox2.Checked == true) { k[1] = 1; } else { k[1] = 0; }
            if (checkBox3.Checked == true) { k[2] = 1; } else { k[2] = 0; }
            if (checkBox4.Checked == true) { k[3] = 1; } else { k[3] = 0; }

            int[] wid = new int[4];
            for (int i = 0; i < 4; i++)
               {  if (k[i] == 1 )
                {
                    if (Form1.zArrayDescriptor[Form1.regComplex * 4 + i] == null) { MessageBox.Show("Graphic2D zArrayDescriptor[" + i + "] == NULL"); return; }
                      else { wid[i] = Form1.zArrayDescriptor[Form1.regComplex * 4 + i].width; }
                }
               }
            int maxw = int.MinValue;                                            // Поиск максимального размера
            for (int i = 0; i < 4; i++) { if (wid[i] > maxw) maxw = wid[i];  }
            int w1 = maxw;
            ww = w = w1;                                                        // Глобальные размеры

            // Глобальный размер
           
             int N_line = 0;
             Form1.Coords[] X = MainForm.GetCoordinates();
           
          
             N_line = (int)X[0].y; if (N_line < 0) N_line = 0; if (N_line > w1) N_line = w1;    // Координаты графика из Form1
             //MessageBox.Show("Graphic2D N_line = " + N_line);
             //N_line = 512;

            for (int i = 0; i < 4; i++)                                                           // 4 массива для графиков
              {
                 buf[i]    = new double[w1];
                 buf_gl[i] = new double[w1];
                if (k[i] == 1)
                    for (int j = 0; j < wid[i]; j++)
                    {
                        buf[i][j] = Form1.zArrayDescriptor[Form1.regComplex * 4 + i].array[j, N_line];
                    }
              }

             for (int i = 0; i < 4; i++)                                                           // Определение максимума и минимума
              {
                if (k[i] == 1) for (int j = 0; j < w1; j++) { if (buf[i][j] > maxx) maxx = buf[i][j]; if (buf[i][j] < minx) minx = buf[i][j]; }    
              }

              if (maxx == minx) { MessageBox.Show("max == min = " + Convert.ToString(maxx)); return; }
              label3.Text = minx.ToString();          label9.Text = maxx.ToString();


            for (int i = 0; i < 4; i++)                                                           // Переопределение массивов
            {
                if (k[i] == 1)
                   for (int j = 0; j < w1; j++)
                    {
                        buf[i][j] = (buf[i][j] - minx) * 255 / (maxx - minx);
                        buf_gl[i][j] = buf[i][j];
                    }
            }


                //MessageBox.Show("n - "+ buf1.Length);
                hScrollBar2.Minimum = 0;
                hScrollBar2.Maximum = w1;
                label6.Text = hScrollBar2.Minimum.ToString(); ;
                label7.Text = hScrollBar2.Maximum.ToString(); ;
                label8.Text = hScrollBar2.Value.ToString();
                label13.Text = N_line.ToString();

                Gr(ixx);
            }
        /// <summary>
        /// Обработка кнопки "Построить график из одного файла"
        /// </summary>
        /// В Form1 должна быть установлена X1, X2, X3, X4 - координаты графиков
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void button4_Click(object sender, EventArgs e)      // из центрального окна --------------------------
        {
            if (checkBox1.Checked == true) { k[0] = 1; } else { k[0] = 0; }
            if (checkBox2.Checked == true) { k[1] = 1; } else { k[1] = 0; }
            if (checkBox3.Checked == true) { k[2] = 1; } else { k[2] = 0; }
            if (checkBox4.Checked == true) { k[3] = 1; } else { k[3] = 0; }

            if (Form1.zArrayPicture == null) { MessageBox.Show("Graphic2D zArrayPicture == NULL"); return; }
            int nx = Form1.zArrayPicture.width;
            int ny = Form1.zArrayPicture.height;
            ww = w = nx;                                                        // Глобальные размеры

            // Глобальный размер

            int N_line = 0;
            Form1.Coords[] X = MainForm.GetCoordinates();

            for (int i = 0; i < 4; i++)                                                           // 4 массива для графиков
            {
                if (k[i] != 1) continue;
                N_line = (int)X[i].y; if (N_line < 0) N_line = 0; if (N_line > nx) N_line = ny;
                buf[i] = new double[nx];
                buf_gl[i] = new double[nx]; 
                for (int j = 0; j < nx; j++) {  buf[i][j] = Form1.zArrayPicture.array[j, N_line];  }
            }

            for (int i = 0; i < 4; i++)                                                           // Определение максимума и минимума
            {
                if (k[i] != 1) continue;
                for (int j = 0; j < nx; j++) { if (buf[i][j] > maxx) maxx = buf[i][j]; if (buf[i][j] < minx) minx = buf[i][j]; }
            }

            if (maxx == minx) { MessageBox.Show("max == min = " + Convert.ToString(maxx)); return; }
            label3.Text = minx.ToString(); label9.Text = maxx.ToString();

            for (int i = 0; i < 4; i++)                                                           // Переопределение массивов
            {
                if (k[i] != 1) continue;
                for (int j = 0; j < nx; j++)
                    {
                        buf[i][j] = (buf[i][j] - minx) * 255 / (maxx - minx);
                        buf_gl[i][j] = buf[i][j];
                    }
            }


            //MessageBox.Show("n - "+ buf1.Length);
            hScrollBar2.Minimum = 0;
            hScrollBar2.Maximum = nx;
            label6.Text = hScrollBar2.Minimum.ToString(); ;
            label7.Text = hScrollBar2.Maximum.ToString(); ;
            label8.Text = hScrollBar2.Value.ToString();
            label13.Text = N_line.ToString();

            Gr(ixx);
        }
        private void Gr(int x)
           {
            int x0 = 70;
            int hh = 256;                     //  Размер по оси Y
            int h = 256 + 20;                 //  hh -размер 256 , рисуем немного ниже

            pc1.BackColor = Color.White;                              // PictureBox pc1 - белый фон
            //pc1.Location = new System.Drawing.Point(0, 8);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;          // Возможность растяжения

            Bitmap btmBack = new Bitmap(pc1.Width, hh + 64);
            grBack = Graphics.FromImage(btmBack);
            pc1.BackgroundImage = btmBack;
            pc1.Invalidate();

            Pen[] p1 = { new Pen(Color.Black, 1), new Pen(Color.Red, 1),  new Pen(Color.Green, 1), new Pen(Color.Purple, 1) };

            //Pen p1 = new Pen(Color.Black, 1);
            //Pen p2 = new Pen(Color.Black, 1);   // Red
            //Pen p3 = new Pen(Color.Green, 1);

            //  -----------------------------------------------------------------------------------------------------Ось x
            Font drawFont = new Font("Courier", 6, FontStyle.Regular, GraphicsUnit.Point);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            grBack.DrawLine(p1[0], x0, h, w + x0, h);
           
            for (int i = 0; i < w; i += 8) grBack.DrawLine(p1[0], i + x0, h, i + x0, h + 8);

            for (int i = x; i < w; i += 32)
            {
                string sx = (i * step).ToString();
                grBack.DrawString(sx, drawFont, drawBrush, i + x0 - 8 - x, h + 20); //, drawFormat);
            }
           

            //  -----------------------------------------------------------------------------------------------------Ось y
            grBack.DrawLine(p1[0], x0, 8, x0, h + 8);
            for (int i = 8; i < hh + 8; i += 8) grBack.DrawLine(p1[0], x0, i, x0 - 5, i);



            double kk = (hh) / 32;
            double kx = (maxx - minx) / kk;
            double nf = minx;
            double kf;
            for (int i = 0; i <= hh; i += 32)
            {
                kf = nf;
                string sx = kf.ToString("0.00");                        // 0.00 это формат
                grBack.DrawString(sx, drawFont, drawBrush, 2, h - i); //, drawFormat);
                nf += kx;
                // grBack.DrawLine(p1, x0, i, x0 + w1, i);
            }


            //           grBack.DrawLine(p3, x0, 0, x0, h + 9);                                                                     // Значение координаты

            //  ----------------------------------------------------------------------------------------------------- Сам график
            for (int i = 0; i < 4; i++)                                                           // Переопределение массивов
            {
                if (k[i] == 1)
                    for (int j = 0; j < w - 1 - x; j++)
                    {
                        int y1 = (int)(h - buf_gl[i][j + x]);
                        int y2 = (int)(h - buf_gl[i][j + 1 + x]);
                        grBack.DrawLine(p1[i], j + x0, y1, j + 1 + x0, y2);
                    }
             }

            pc1.Refresh();

            // Controls.Add(pc1);

        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            label6.Text = hScrollBar2.Minimum.ToString(); ;
            label7.Text = hScrollBar2.Maximum.ToString(); ;
            label8.Text = hScrollBar2.Value.ToString();
            ixx = hScrollBar2.Value;
            Gr(ixx);
        }

        private void button1_Click(object sender, EventArgs e)  // <<  Уменьшение
        {
            step = step + 1;
            w = ww / step; if (step > 10) step = 10;
         
            for (int m = 0; m < 4; m++)                                                           // Переопределение массивов
            {
                if (k[m] == 1) for (int i = 0, j = 0; i < ww; i += step, j++) { buf_gl[m][j] = buf[m][i]; }
            }
            ixx = 0;      Gr(ixx);
        }

       

        private void button2_Click(object sender, EventArgs e) // >> Увеличение
        {
            step = step - 1; if (step < 1) step = 1;
            w = ww / step;
          
            for (int m = 0; m < 4; m++)                                                           // Переопределение массивов
            {
                if (k[m] == 1)  for (int i = 0, j=0; i < ww; i += step, j++) { buf_gl[m][j] = buf[m][i]; } 
            }
            ixx = 0;   Gr(ixx);
        }

       
       
    }
}
