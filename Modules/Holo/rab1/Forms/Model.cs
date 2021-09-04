using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public delegate void ModelBox(double sdvg, double noise, double Lambda);
public delegate void ModelBox1(double sdvg0, double sdvg, double noise, double Lambda);
public delegate void ModelBox2(double sdvg0, double sdvg, double noise, double Lambda, double lm, double dx, double AngleX, double AngleY);

//public delegate void ModelBoxPSI(double sdvg0, double sdvg1, double noise, double Lambda, double dx, double[] fz);
public delegate void ModelBoxPSI_Fr(double sdvg0, double sdvg1, double noise, double Lambda, double d, double dx, double[] fz, double ax, double ay);
public delegate void ModelBoxPSI_Fr8(double sdvg0, double sdvg1, double noise, double Lambda, double d, double dx, double[] fz, double ax, double dy);
//public delegate void ModelBoxPSI8_Fr(double sdvg0, double sdvg1, double noise, double Lambda, double d, double dx, double[] fz);
public delegate void ModelBox_Fr(double sdvg0, double sdvg, double noise, double Lambda, double dx, double dy, double Ax, double Ay);
public delegate void ModelBox_Cos(double[] fz);
public delegate void ModelBox_Balka(double L, double Y, int  N);



namespace rab1.Forms
{
    public partial class Model : Form
    {
        public event ModelBox1                    OnModel;
        //public event ModelBox OnInterf;
        public event ModelBox2                    OnInterf2;           // Двойная экспозиция
        //public event ModelBox OnInterf3;
        //public event ModelBoxPSI OnInterfPSI;
        public event ModelBoxPSI_Fr               OnInterfPSI_Fr;
        public event ModelBoxPSI_Fr8              OnInterf8PSI_Fr;
        public event ModelBox_Fr                  OnInterf_Fr;
        public event ModelBox_Cos                 OnInterf_Cos;
        public event ModelBox_Balka               OnInterf_Balka;

        private static double sdvg = 3;
        private static double sdvg0 = 0;
        private static double noise = 0.02;
        private static double Lambda = 0.5;
        private static double d  = 135;
        private static double dx = 7;
        private static double AngleX = 0;
        private static double AngleY = 0.7;
        private static double[] fz = { 0.0, 90.0, 180.0, 270.0 };
        private static int k1 = 4, k2=7;
        private static double L = 200;  // mm
        private static double Y = 20;   // mm
        private static int N = 200;           // точек на полосу

        public Model()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(sdvg);    // Сдвиг после деформации
            textBox4.Text = Convert.ToString(sdvg0);   // Сдвиг до деформации
            textBox2.Text = Convert.ToString(Lambda);
            textBox3.Text = Convert.ToString(noise);
            textBox5.Text = Convert.ToString(fz[0]); textBox6.Text = Convert.ToString(fz[1]); textBox7.Text = Convert.ToString(fz[2]); textBox8.Text = Convert.ToString(fz[3]);
            textBox9.Text = Convert.ToString(dx);
            textBox10.Text = Convert.ToString(d);
            textBox11.Text = Convert.ToString(AngleX);
            textBox12.Text = Convert.ToString(AngleY);
            textBox13.Text = Convert.ToString(k1);
            textBox14.Text = Convert.ToString(k2);

            textBox15.Text = Convert.ToString(L);
            textBox16.Text = Convert.ToString(Y);
            textBox17.Text = Convert.ToString(N);
        }


        private void button1_Click(object sender, EventArgs e)  
        {
            sdvg = Convert.ToDouble(textBox1.Text);  // Сдвиг после деформации
            sdvg0 = Convert.ToDouble(textBox4.Text); // Сдвиг до деформации
            Lambda = Convert.ToDouble(textBox2.Text);
            noise = Convert.ToDouble(textBox3.Text);
            OnModel(sdvg0, sdvg, noise, Lambda);
            Close();
        }
/*
        private void button2_Click(object sender, EventArgs e)
        {
            sdvg = Convert.ToDouble(textBox1.Text);
            Lambda = Convert.ToDouble(textBox2.Text);
            noise = Convert.ToDouble(textBox3.Text);
            OnInterf(sdvg, noise, Lambda);
            Close();
        }
*/
        private void button3_Click(object sender, EventArgs e)  // -------------------- Двойная экспозиция
        {
            sdvg0 = Convert.ToDouble(textBox4.Text);            // Сдвиг до деформации
            sdvg = Convert.ToDouble(textBox1.Text);             // Сдвиг после деформации
            Lambda = Convert.ToDouble(textBox2.Text);
            noise = Convert.ToDouble(textBox3.Text);
            dx = Convert.ToDouble(textBox9.Text);               // Размер апертуры
            d = Convert.ToDouble(textBox10.Text);               // Расстояние
            AngleX = Convert.ToDouble(textBox11.Text);  double Ax = Math.PI * AngleX / 180.0;
            AngleY = Convert.ToDouble(textBox12.Text);  double Ay = Math.PI * AngleY / 180.0;
            OnInterf2(sdvg0, sdvg, noise, Lambda, d*1000, dx*1000, Ax, Ay);
            Close();
        }
/*
        private void button4_Click(object sender, EventArgs e)  // Ldt ujkjuhfvvs
        {
            sdvg = Convert.ToDouble(textBox1.Text);
            Lambda = Convert.ToDouble(textBox2.Text);
            noise = Convert.ToDouble(textBox3.Text);
            OnInterf3(sdvg, noise, Lambda);
            Close();
        }
 * */
        //  Двойная экспозиция(PSI) из 5 голограмм
        //   Область Фраунгофера
        /*       private void button5_Click(object sender, EventArgs e)  // PSI
               {
                   sdvg  = Convert.ToDouble(textBox1.Text);
                   sdvg0 = Convert.ToDouble(textBox4.Text);
                   Lambda = Convert.ToDouble(textBox2.Text);
                   noise = Convert.ToDouble(textBox3.Text);
                   dx = Convert.ToDouble(textBox9.Text);

                   double[] fzrad = new double[4];
                   fz[0] = Convert.ToDouble(textBox5.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
                   fz[1] = Convert.ToDouble(textBox6.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
                   fz[2] = Convert.ToDouble(textBox7.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
                   fz[3] = Convert.ToDouble(textBox8.Text); fzrad[3] = Math.PI * fz[3] / 180.0;

                   OnInterfPSI(sdvg0, sdvg, noise, Lambda, dx*1000, fzrad);
                   Close();

               }
               */
        //  Формирование 4 интерферограмм (PSI)-> 8,9,10,11
        //  Область Френеля -> результат в zComplex[2]
        
        private void button6_Click(object sender, EventArgs e) // PSI Frenel
        {
            sdvg = Convert.ToDouble(textBox1.Text);
            sdvg0 = Convert.ToDouble(textBox4.Text);
            Lambda = Convert.ToDouble(textBox2.Text);
            noise = Convert.ToDouble(textBox3.Text);
            dx = Convert.ToDouble(textBox9.Text);
            d = Convert.ToDouble(textBox10.Text);
            AngleX = Convert.ToDouble(textBox11.Text); double Ax = Math.PI * AngleX / 180.0;
            AngleY = Convert.ToDouble(textBox12.Text); double Ay = Math.PI * AngleY / 180.0;

            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox5.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox6.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox7.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox8.Text); fzrad[3] = Math.PI * fz[3] / 180.0;

            OnInterfPSI_Fr(sdvg0, sdvg, noise, Lambda, dx*1000, d*1000, fzrad, Ax, Ay);
            Close();
        }

             //  Прямое сравнение волновых фронтов (PSI)
             //  
        private void button7_Click(object sender, EventArgs e)
        {
            sdvg = Convert.ToDouble(textBox1.Text);
            sdvg0 = Convert.ToDouble(textBox4.Text);
            Lambda = Convert.ToDouble(textBox2.Text);
            noise = Convert.ToDouble(textBox3.Text);
            dx = Convert.ToDouble(textBox9.Text);
            d = Convert.ToDouble(textBox10.Text);
            AngleX = Convert.ToDouble(textBox11.Text); double Ax = Math.PI * AngleX / 180.0;
            AngleY = Convert.ToDouble(textBox12.Text); double Ay = Math.PI * AngleY / 180.0;

            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox5.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox6.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox7.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox8.Text); fzrad[3] = Math.PI * fz[3] / 180.0;



            OnInterf8PSI_Fr(sdvg0, sdvg, noise, Lambda, dx * 1000, d * 1000, fzrad, Ax, Ay);
            Close();
        }

        // Формирование преобразований Френеля
        //  до деформации    => zComplex[1]
        //  после деформации => zComplex[2]

        private void button5_Click(object sender, EventArgs e)  
        {
            sdvg   = Convert.ToDouble(textBox1.Text);
            sdvg0  = Convert.ToDouble(textBox4.Text);
            Lambda = Convert.ToDouble(textBox2.Text);
            noise  = Convert.ToDouble(textBox3.Text);
            dx     = Convert.ToDouble(textBox9.Text);   // Размер апертуры
            d      = Convert.ToDouble(textBox10.Text);  // Расстояние
            AngleX = Convert.ToDouble(textBox11.Text); double Ax = Math.PI * AngleX / 180.0;
            AngleY = Convert.ToDouble(textBox12.Text); double Ay = Math.PI * AngleY / 180.0;

            OnInterf_Fr(sdvg0, sdvg, noise, Lambda, d * 1000, dx * 1000, Ax, Ay);
            Close();
        }

        private void Model_Load(object sender, EventArgs e)
        {

        }

        // Прогиб балки
        private void button4_Click(object sender, EventArgs e)
        {
            L = Convert.ToDouble(textBox15.Text);
            Y = Convert.ToDouble(textBox16.Text);
            N = Convert.ToInt32(textBox17.Text);
            OnInterf_Balka( L,  Y, N);
            Close();
        }

        // Моделирование интерференционной картины  разность фаз
        private void button2_Click(object sender, EventArgs e)
        {
            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox5.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox6.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox7.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox8.Text); fzrad[3] = Math.PI * fz[3] / 180.0;
            OnInterf_Cos(fzrad);
            Close();
        }
        //
        //              Сдвиг голограмм
        //
    }
}
