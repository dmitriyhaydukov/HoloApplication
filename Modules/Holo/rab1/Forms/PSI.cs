using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public delegate void DelegatPSI(double[] fz, double am);
public delegate void DelegatIMAX(int imax);
//public delegate void DelegatMaska(int k1, int k2, int k3);
public delegate void DelegatLis(double[] fz);
public delegate void DelegatSdvg();

namespace rab1.Forms
{
   
    public partial class PSI : Form
    {

        public delegate void VisualRegImageDelegate(int k);
        public static VisualRegImageDelegate VisualRegImage = null;      // Визуализация одного кадра от 0 до 11

        public event DelegatPSI    OnPSI;
        public event DelegatLis    OnPSI1;
        public event DelegatLis    OnPSI3;
        public event DelegatLis    OnPSI4;
        public event DelegatLis    OnPSI5;
        public event DelegatLis    OnPSI6;
        public event DelegatLis    OnPSI7;
        public event DelegatLis    OnPSI8;
        public event DelegatSdvg   OnPSI_Carre;
        public event DelegatIMAX   OnIMAX;
        public event DelegatIMAX   OnIMAX1;
       // public event DelegatMaska  OnMaska;
        public event DelegatSdvg   OnSdvg;


        private static double[] fz = { 0.0, 90.0, 180.0, 270.0, 0.0, 90.0, 180.0, 270.0 };
        private static double am = 255;
        private static int imax = 255;

        private static int n_sdv = 4;   // Число фазовых сдвигов   textBox11

        //private static int km1 = 1;
        //private static int km2 = 2;
        //private static int km3 = 3;

        public PSI()
        {
            InitializeComponent();
            textBox1.Text  = Convert.ToString(fz[0]); 
            textBox2.Text  = Convert.ToString(fz[1]); 
            textBox3.Text  = Convert.ToString(fz[2]); 
            textBox4.Text  = Convert.ToString(fz[3]);
            textBox7.Text  = Convert.ToString(fz[4]);
            textBox8.Text  = Convert.ToString(fz[5]);
            textBox9.Text  = Convert.ToString(fz[6]);
            textBox10.Text = Convert.ToString(fz[7]);


            textBox5.Text = Convert.ToString(am);
            textBox6.Text = Convert.ToString(imax);

            textBox11.Text = Convert.ToString(n_sdv); // Число фазовых сдвигов 

            // textBox13.Text = Convert.ToString(km1);
            //  textBox14.Text = Convert.ToString(km2);
            // textBox7.Text = Convert.ToString(km3);

        }
       
        private void button1_Click(object sender, EventArgs e)  // PSI  (амплитуда и фаза)
        {
 
            fz[0] = Convert.ToDouble(textBox1.Text); 
            fz[1] = Convert.ToDouble(textBox2.Text); 
            fz[2] = Convert.ToDouble(textBox3.Text); 
            fz[3] = Convert.ToDouble(textBox4.Text);
            am = Convert.ToDouble(textBox5.Text);
            
            double[] fzrad = new double[4];                 // Фаза в радианах
            fzrad[0] = Math.PI * fz[0] / 180.0;
            fzrad[1] = Math.PI * fz[1] / 180.0;
            fzrad[2] = Math.PI * fz[2] / 180.0;
            fzrad[3] = Math.PI * fz[3] / 180.0;
            
            OnPSI( fzrad, am);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)  // Квантование 4 кадров (8,9,10,11)
        {
            imax = Convert.ToInt32(textBox6.Text);
            OnIMAX(imax);
            Close();
        }

        private void button3_Click(object sender, EventArgs e)  // Квантование одного кадра
        {
            imax = Convert.ToInt32(textBox6.Text);
            OnIMAX1(imax);
            Close();
        }

        private void button4_Click(object sender, EventArgs e)  // PSI (Основная формула) regCpmplex -> Главное окно
        {

            n_sdv = Convert.ToInt32(textBox11.Text);
            double[] fzrad = new double[n_sdv];

            fz[0] = Convert.ToDouble(textBox1.Text); 
            fz[1] = Convert.ToDouble(textBox2.Text); 
            fz[2] = Convert.ToDouble(textBox3.Text); 
            fz[3] = Convert.ToDouble(textBox4.Text); 
            fz[4] = Convert.ToDouble(textBox7.Text); 
            fz[5] = Convert.ToDouble(textBox8.Text); 
            fz[6] = Convert.ToDouble(textBox9.Text); 
            fz[7] = Convert.ToDouble(textBox10.Text);

            for (int i=0; i < n_sdv; i++) fzrad[i] = Math.PI * fz[i] / 180.0;

            //Form1.zArrayPicture = ATAN_PSI.ATAN(Form1.zArrayDescriptor, Form1.regComplex, fz);
            //Vizual.Vizual_Picture(Form1.zArrayPicture, Form1.pictureBox01);

            OnPSI1(fzrad);
           
            Close();
        }
        private void button5_Click(object sender, EventArgs e) // PSI Carre regCpmplex -> Главное окно
        { 
            OnPSI_Carre();
            Close();
        }
        private void button14_Click(object sender, EventArgs e) // 3-точечный regCpmplex -> Главное окно
        {
            int n = 3;
            double[] fzrad = new double[n];

            fz[0] = Convert.ToDouble(textBox1.Text);
            fz[1] = Convert.ToDouble(textBox2.Text);
            fz[2] = Convert.ToDouble(textBox3.Text);
            //fz[3] = Convert.ToDouble(textBox4.Text);
            //fz[4] = Convert.ToDouble(textBox7.Text);
            //fz[5] = Convert.ToDouble(textBox8.Text);
            //fz[6] = Convert.ToDouble(textBox9.Text);
            //fz[7] = Convert.ToDouble(textBox10.Text);

            for (int i = 0; i < n; i++) fzrad[i] = Math.PI * fz[i] / 180.0;

            //Form1.zArrayPicture = ATAN_PSI.ATAN(Form1.zArrayDescriptor, Form1.regComplex, fz);
            //Vizual.Vizual_Picture(Form1.zArrayPicture, Form1.pictureBox01);

            OnPSI3(fzrad);
            Close();
        }
        private void button13_Click(object sender, EventArgs e) // 4-точечный regCpmplex -> Главное окно
        {
            int n = 4;
            double[] fzrad = new double[n];

            fz[0] = Convert.ToDouble(textBox1.Text);
            fz[1] = Convert.ToDouble(textBox2.Text);
            fz[2] = Convert.ToDouble(textBox3.Text);
            fz[3] = Convert.ToDouble(textBox4.Text);
            //fz[4] = Convert.ToDouble(textBox7.Text);
            //fz[5] = Convert.ToDouble(textBox8.Text);
            //fz[6] = Convert.ToDouble(textBox9.Text);
            //fz[7] = Convert.ToDouble(textBox10.Text);

            for (int i = 0; i < n; i++) fzrad[i] = Math.PI * fz[i] / 180.0;

            //Form1.zArrayPicture = ATAN_PSI.ATAN(Form1.zArrayDescriptor, Form1.regComplex, fz);
            //Vizual.Vizual_Picture(Form1.zArrayPicture, Form1.pictureBox01);

            OnPSI4(fzrad);
            Close();
        }
        private void button9_Click(object sender, EventArgs e) // 5-точечный regCpmplex -> Главное окно
        {
            int n = 5;
            double[] fzrad = new double[n];

            fz[0] = Convert.ToDouble(textBox1.Text);
            fz[1] = Convert.ToDouble(textBox2.Text);
            fz[2] = Convert.ToDouble(textBox3.Text);
            fz[3] = Convert.ToDouble(textBox4.Text);
            fz[4] = Convert.ToDouble(textBox7.Text);
            //fz[5] = Convert.ToDouble(textBox8.Text);
            //fz[6] = Convert.ToDouble(textBox9.Text);
            //fz[7] = Convert.ToDouble(textBox10.Text);

            for (int i = 0; i < n; i++) fzrad[i] = Math.PI * fz[i] / 180.0;

            //Form1.zArrayPicture = ATAN_PSI.ATAN(Form1.zArrayDescriptor, Form1.regComplex, fz);
            //Vizual.Vizual_Picture(Form1.zArrayPicture, Form1.pictureBox01);

            OnPSI5(fzrad);
            Close();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            int n = 6;
            double[] fzrad = new double[n];

            fz[0] = Convert.ToDouble(textBox1.Text);
            fz[1] = Convert.ToDouble(textBox2.Text);
            fz[2] = Convert.ToDouble(textBox3.Text);
            fz[3] = Convert.ToDouble(textBox4.Text);
            fz[4] = Convert.ToDouble(textBox7.Text);
            fz[5] = Convert.ToDouble(textBox8.Text);
            fz[6] = Convert.ToDouble(textBox9.Text);
            //fz[7] = Convert.ToDouble(textBox10.Text);

            for (int i = 0; i < n; i++) fzrad[i] = Math.PI * fz[i] / 180.0;

            //Form1.zArrayPicture = ATAN_PSI.ATAN(Form1.zArrayDescriptor, Form1.regComplex, fz);
            //Vizual.Vizual_Picture(Form1.zArrayPicture, Form1.pictureBox01);

            OnPSI6(fzrad);
            Close();
        }
        private void button10_Click(object sender, EventArgs e)  // 7-точечный regCpmplex -> Главное окно
        {
            int n = 7;
            double[] fzrad = new double[n];

            fz[0] = Convert.ToDouble(textBox1.Text);
            fz[1] = Convert.ToDouble(textBox2.Text);
            fz[2] = Convert.ToDouble(textBox3.Text);
            fz[3] = Convert.ToDouble(textBox4.Text);
            fz[4] = Convert.ToDouble(textBox7.Text);
            fz[5] = Convert.ToDouble(textBox8.Text);
            fz[6] = Convert.ToDouble(textBox9.Text);
            //fz[7] = Convert.ToDouble(textBox10.Text);

            for (int i = 0; i < n; i++) fzrad[i] = Math.PI * fz[i] / 180.0;

            //Form1.zArrayPicture = ATAN_PSI.ATAN(Form1.zArrayDescriptor, Form1.regComplex, fz);
            //Vizual.Vizual_Picture(Form1.zArrayPicture, Form1.pictureBox01);

            OnPSI7(fzrad);
            Close();
        }
        private void button12_Click(object sender, EventArgs e) // 8-точечный regCpmplex -> Главное окно
        {
            int n = 8;
            double[] fzrad = new double[n];

            fz[0] = Convert.ToDouble(textBox1.Text);
            fz[1] = Convert.ToDouble(textBox2.Text);
            fz[2] = Convert.ToDouble(textBox3.Text);
            fz[3] = Convert.ToDouble(textBox4.Text);
            fz[4] = Convert.ToDouble(textBox7.Text);
            fz[5] = Convert.ToDouble(textBox8.Text);
            fz[6] = Convert.ToDouble(textBox9.Text);
            fz[7] = Convert.ToDouble(textBox10.Text);
            //fz[7] = Convert.ToDouble(textBox10.Text);

            for (int i = 0; i < n; i++) fzrad[i] = Math.PI * fz[i] / 180.0;

            //Form1.zArrayPicture = ATAN_PSI.ATAN(Form1.zArrayDescriptor, Form1.regComplex, fz);
            //Vizual.Vizual_Picture(Form1.zArrayPicture, Form1.pictureBox01);

            OnPSI8(fzrad);
            Close();
        }


        /*      private void button5_Click(object sender, EventArgs e)  // Наложение маски
               {
                   km1 = Convert.ToInt32(textBox13.Text);
                   km2 = Convert.ToInt32(textBox14.Text);
                   km3 = Convert.ToInt32(textBox7.Text);
                  // OnMaska(km1-1 , km2 - 1, km3 - 1);
                   Close();
               }
       */
        private void button6_Click(object sender, EventArgs e)  //  Угол из regComplex
        {
            OnSdvg();
            Close();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            n_sdv = Convert.ToInt32(textBox11.Text);   // Число сдвигов
            double n = 360.0 / n_sdv;

            textBox1.Text = Convert.ToString(0);
            textBox2.Text = Convert.ToString(n);
            textBox3.Text = Convert.ToString(2 * n);
            textBox4.Text = Convert.ToString(3 * n);

            textBox7.Text = Convert.ToString(4 * n);
            textBox8.Text = Convert.ToString(5 * n);
            textBox9.Text = Convert.ToString(6 * n);
            textBox10.Text = Convert.ToString(7 * n);
        }
        //------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// cos от разности фаз + фазовый сдвиг => 0,1,2,3,4 .....
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            if (Form1.zArrayDescriptor[0] == null) { MessageBox.Show("PSI zArrayDescriptor[" + 0 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[1] == null) { MessageBox.Show("PSI zArrayDescriptor[" + 1 + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[0].width;
            int ny = Form1.zArrayDescriptor[0].height;

            ZArrayDescriptor fz1 = new ZArrayDescriptor(nx, ny);
            ZArrayDescriptor fz2 = new ZArrayDescriptor(nx, ny);
           

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    fz1.array[i , j] = Form1.zArrayDescriptor[0].array[i, j];
                    fz2.array[i , j] = Form1.zArrayDescriptor[1].array[i, j];
                }

            n_sdv = Convert.ToInt32(textBox11.Text);
            double[] fzrad = new double[n_sdv];

            fz[0] = Convert.ToDouble(textBox1.Text);
            fz[1] = Convert.ToDouble(textBox2.Text);
            fz[2] = Convert.ToDouble(textBox3.Text);
            fz[3] = Convert.ToDouble(textBox4.Text);
            fz[4] = Convert.ToDouble(textBox7.Text);
            fz[5] = Convert.ToDouble(textBox8.Text);
            fz[6] = Convert.ToDouble(textBox9.Text);
            fz[7] = Convert.ToDouble(textBox10.Text);

            for (int i = 0; i < n_sdv; i++) fzrad[i] = Math.PI * fz[i] / 180.0;

            for (int k = 0; k < n_sdv; k++)
            {
              ZArrayDescriptor rez = new ZArrayDescriptor(nx, ny);
             for (int i = 0; i < nx; i++)
              for (int j = 0; j < ny; j++)
                {          
                   rez.array[i, j] = Math.Cos(fz1.array[i,j] - fz2.array[i,j] + fzrad[k]);
                }
              Form1.zArrayDescriptor[k] = rez;
              VisualRegImage(k);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// PSI 256 или более сдвигов по строке
        /// zArrayDescriptor[0] до нагрузки => zArrayDescriptor[3]
        /// zArrayDescriptor[1] после       => zArrayDescriptor[3]
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        public static double[] Razn_faz(double[] res1, double[] res2, int nx, int ny)  // Разность двух фаз
        {

            int n_sdv = 4;
            double[]  fzrad = new double[n_sdv];                                     
            double[,] rezz  = new double[nx, n_sdv];
            double[]  faza  = new double[nx];

            for (int i = 0; i < n_sdv; i++) fzrad[i] = Math.PI * Math.PI * 90*i / 180.0;

            for (int k = 0; k < n_sdv; k++)
            {
                for (int i = 0; i < nx; i++)
                    for (int j = 0; j < ny; j++)
                    {
                        rezz[i, k] = Math.Cos(res1[i] - res2[i] + fzrad[k]);
                    }
            }
            // MessageBox.Show("PSI 256 ");

            // psi

            double[] i_sdv = new double[n_sdv];
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];


            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fzrad[i]); k_cos[i] = Math.Cos(fzrad[i]); }
            k_sin = ATAN_PSI.Vector_orto(k_sin); k_cos = ATAN_PSI.Vector_orto(k_cos);

            for (int i = 0; i < nx; i++)
            {
                i_sdv[0] = rezz[i, 0];
                i_sdv[1] = rezz[i, 1];
                i_sdv[2] = rezz[i, 2];
                i_sdv[3] = rezz[i, 3];
                //double[] v_sdv = ATAN_PSI.Vector_orto(i_sdv);                // ------  Формула расшифровки фазы
                double fz1 = ATAN_PSI.Vector_Mul(i_sdv, k_sin);
                double fz2 = ATAN_PSI.Vector_Mul(i_sdv, k_cos);

                faza[i] = Math.Atan2(fz1, fz2);
            }

            return faza;
        }

/// <summary>
/// PSI по одной строке
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>

        private void button15_Click(object sender, EventArgs e)
        {
            if (Form1.zArrayDescriptor[0] == null) { MessageBox.Show("PSI 256 zArrayDescriptor[" + 0 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[1] == null) { MessageBox.Show("PSI 256 zArrayDescriptor[" + 1 + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[0].width;
            //int ny = Form1.zArrayDescriptor[0].height;
            int ny = 256;


            //ZArrayDescriptor fz2 = new ZArrayDescriptor(nx, ny);

            ZArrayDescriptor rez = new ZArrayDescriptor(nx, 500);
            double[] summ_sin = new double[nx];
            double[] summ_cos = new double[nx];
            double[] fzr = new double[ny];
          
           
            double[] res1 = new double[nx];
            double[] res2 = new double[nx];
            double[] faza = new double[nx];


            for (int j = 0; j < ny; j++) { int i = j; if (i >= 256) { i = i - 256; }  fzr[j] = 2*Math.PI * i / 256;  }


            for (int i = 0; i < nx; i++)                                                           // До нагрузки
            {
                for (int j = 0; j < ny; j++)
                {
                    summ_sin[i] += Form1.zArrayDescriptor[0].array[i, j] * Math.Sin(fzr[j]);
                    summ_cos[i] += Form1.zArrayDescriptor[0].array[i, j] * Math.Cos(fzr[j]);
                 
                }
                res1[i]= Math.Atan2(summ_sin[i], summ_cos[i]);
            }
            for (int i = 0; i < nx; i++) for (int j = 0; j < 100; j++) {  rez.array[i, j] = res1[i];  } // -----------


            for (int i = 0; i < nx; i++)                                                           // После нагрузки
            {
                for (int j = 0; j < ny; j++)
                {
                    summ_sin[i] += Form1.zArrayDescriptor[1].array[i, j] * Math.Sin(fzr[j]);
                    summ_cos[i] += Form1.zArrayDescriptor[1].array[i, j] * Math.Cos(fzr[j]);
                }
                res2[i] = Math.Atan2(summ_sin[i], summ_cos[i]);
            }
            for (int i = 0; i < nx; i++) for (int j = 100; j < 200; j++) { rez.array[i, j] = res2[i]; } // --------------

            //MessageBox.Show("SUB 256 ");

            faza = Razn_faz(res1, res2, nx, ny);
            for (int i = 0; i < nx; i++)  for (int j = 220; j < 300; j++) { rez.array[i, j] = faza[i];  }  //---------------
/*
            double fi;
            for (int i = 0; i < nx; i++)                                       // 6- точечный алгоритм до нагрузки
            {
                
                    double i1 = Form1.zArrayDescriptor[0].array[i, 0];
                    double i2 = Form1.zArrayDescriptor[0].array[i, 63];
                    double i3 = Form1.zArrayDescriptor[0].array[i, 64 * 2-1];
                    double i4 = Form1.zArrayDescriptor[0].array[i, 64 * 3-1];
                    double i5 = Form1.zArrayDescriptor[0].array[i, 64 * 4-1];
                    double i6 = Form1.zArrayDescriptor[0].array[i, 64 * 5-1];

                    double fz1 = 3 * i2 - 4 * i4 + i6;
                    double fz2 = i1 - 4 * i3 + 3 * i5;
                    //double fi = Math.Atan2(fz1, fz2);
                    fi = Math.Atan2(fz1, fz2) + Math.PI / 2;
                    if (fi > Math.PI) fi = fi - 2 * Math.PI;
                   
                    res1[i] = fi;
            }

            for (int i = 0; i < nx; i++)                                       // 6- точечный алгоритм после 
            {

                double i1 = Form1.zArrayDescriptor[1].array[i, 0];
                double i2 = Form1.zArrayDescriptor[1].array[i, 63];
                double i3 = Form1.zArrayDescriptor[1].array[i, 64 * 2-1];
                double i4 = Form1.zArrayDescriptor[1].array[i, 64 * 3-1];
                double i5 = Form1.zArrayDescriptor[1].array[i, 64 * 4-1];
                double i6 = Form1.zArrayDescriptor[1].array[i, 64 * 5-1];

                double fz1 = 3 * i2 - 4 * i4 + i6;
                double fz2 = i1 - 4 * i3 + 3 * i5;
                //double fi = Math.Atan2(fz1, fz2);
                fi = Math.Atan2(fz1, fz2) + Math.PI / 2;
                if (fi > Math.PI) fi = fi - 2 * Math.PI;

                res2[i] = fi;
            }
            faza = Razn_faz(res1, res2, nx, ny);
            for (int i = 0; i < nx; i++) for (int j = 320; j < 400; j++) { rez.array[i, j] = faza[i]; }

            for (int i = 0; i < nx; i++)                                       // 7- точечный алгоритм
            {

                double i1 = Form1.zArrayDescriptor[1].array[i, 0];
                double i2 = Form1.zArrayDescriptor[1].array[i, 63];
                double i3 = Form1.zArrayDescriptor[1].array[i, 64 * 2 - 1];
                double i4 = Form1.zArrayDescriptor[1].array[i, 64 * 3 - 1];
                double i5 = Form1.zArrayDescriptor[1].array[i, 64 * 4 - 1];
                double i6 = Form1.zArrayDescriptor[1].array[i, 64 * 5 - 1];
                double i7 = Form1.zArrayDescriptor[1].array[i, 64 * 6 - 1];

                double fz1 = 4 * i2 - 8 * i4 + 4 * i6;
                double fz2 = i1 - 7 * i3 + 7 * i5 - i7;
                fi = Math.Atan2(fz1, fz2) + Math.PI / 2;
                if (fi > Math.PI) fi = fi - 2 * Math.PI; ;

                res2[i] = fi;
            }
            faza = Razn_faz(res1, res2, nx, ny);
            for (int i = 0; i < nx; i++) for (int j = 420; j < 500; j++) { rez.array[i, j] = faza[i]; }
*/
            Form1.zArrayDescriptor[2] = rez;
            VisualRegImage(2);
            Close();
        }
    }
}
