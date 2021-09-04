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
    public delegate void VisualRegImageDelegate(int k);        // Для VisualRegImage 
    public delegate void Photo12Delegate();                    // TakePhoto12
    public delegate void PhotoDelegate(short k1, short k2);    // TakePhoto



    public delegate void CorrectBr1(int k1, int k2, int n);
    public delegate void CorrectBr2(int k1, int k2);
    // public delegate void CorrectBr3(int k1, int k2, int N, int nx, int ny);
               
    public partial class CorrectBr : Form
    {
        public Form1 MainForm = null;

        public static VisualRegImageDelegate VisualRegImage = null;    // Визуализация одного кадра от 0 до 11 из main
        public static Photo12Delegate TakePhoto12             = null;  // Ввести кадр из 1 в 2 из main
        public static PhotoDelegate TakePhoto = null;                  // Ввести кадр из k1 в k2 из main

        public event CorrectBr1 On_CorrectX;
        public event CorrectBr1 On_CorrectG;
       // public event CorrectBr1 On_CorrectClin;
        public event CorrectBr2 On_CorrectSumm;
       // public event CorrectBr3 On_CorrectGxy;

        private static int n = 3996;                  // Размер массива
        private static int k1 = 1;
        private static int k2 = 2;

        private static int k11 = 1;
        private static int k21 = 2;
        private static int k31 = 5;

        private static int k12 = 1;
        //private static int I0 = 0;
        private static double gamma = 1;
       
        private static int nx = 3996;                  // Размер массива
        private static int ny = 2160;                  // Размер массива

        private static int kv = 16;

        private static int X0   = 50;
        private static int STEP = 128;
        //private static int k18 = 1;

        private static int dx = 50;

        private static int porog = 250;

        //double[] cl = { 1,  2,  3,  4,  5,   6,   7,   8,   9,  10,  11,  12,  13,  14,  15,  16,  17,  18,  19,  20,  21,  22,  23,  24,  25,  26,  27,  28, 29,   30,  31, 32 };
        //double[] cl = { 0,  1,  2,  3,  4,   5,   6,   7,   8,   9,  10,  11,  12,  13,  14,  15,  16,  17,  18,  19,  20,  21,  22,  23,  24,  25,  26,  27, 28,   29,  30, 31 };
        //double[] cl = { 7, 15, 23, 31, 39,  47,  55,  63,  71,  79,  87,  95, 103, 111, 119, 127, 135, 143, 151, 159, 167, 175, 183, 191, 199, 207, 215, 223, 231, 239, 247, 255 };
         


         double[] cl = { 35, 50, 58, 65, 72, 78, 85, 94, 100, 108, 118, 132, 149, 168, 192, 255 };    //Правильные значения
         

        


        public CorrectBr()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(k1);     // Номер кадра
            textBox3.Text = Convert.ToString(k2);     // Номер кадра
            textBox2.Text = Convert.ToString(n);      // Размер массива

            textBox4.Text = Convert.ToString(k11);
            textBox5.Text = Convert.ToString(k21);
            textBox6.Text = Convert.ToString(k31);

            textBox7.Text = Convert.ToString(gamma);
            textBox8.Text = Convert.ToString(k12);

            textBox9.Text = Convert.ToString(porog);

            textBox14.Text = Convert.ToString(nx);
            textBox15.Text = Convert.ToString(ny);

            textBox16.Text = Convert.ToString(kv);  // Число градаций 16, 32, 64, 128, 256

            textBox17.Text = Convert.ToString(X0);    // Начало для вертикальных линий
            textBox19.Text = Convert.ToString(STEP);  // Шаг для вертикальных линий
           // textBox18.Text = Convert.ToString(k18 );  // В какой массив
          
            textBox20.Text = Convert.ToString(dx);
        }
        
     
                            
        /// <summary>
        /// Массив меняет длину от X1 до X2 -> 0 до 4096      
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)    // Button1 "Выравнивание по длине массива"
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox3.Text);
            n = Convert.ToInt32(textBox2.Text);

            On_CorrectX(k1-1, k2-1, n);
            Close();
        }
        /// <summary>
        ///  Определения клина с обратными гамма-искажениями
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            k11 = Convert.ToInt32(textBox4.Text);
            k21 = Convert.ToInt32(textBox5.Text);
            k31 = Convert.ToInt32(textBox6.Text);
            On_CorrectG(k11 - 1, k21 - 1, k31-1);
            Close();
        }
        //------------------------------------------------------------------------------------------- Клин
        /// <summary>
        /// Клин от I0 до 255 размер текущий размер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            k12 = Convert.ToInt32(textBox8.Text);
            gamma = Convert.ToDouble(textBox7.Text);
            nx = Convert.ToInt32(textBox14.Text);                       // Текущий размер
            ny = Convert.ToInt32(textBox15.Text);
            dx = Convert.ToInt32(textBox20.Text);                       // Полосы по краям

            int nu = 255;                                               // Число уровней
            Form1.zArrayDescriptor[k12-1] = Model_Sinus.Intensity1(nu,  nx, ny, dx, gamma);
            VisualRegImage(k12-1);

           // Close();

        }
        private void button7_Click(object sender, EventArgs e)          // ---------------------------------   Обратный клин
        {
            k12 = Convert.ToInt32(textBox8.Text);
            gamma = Convert.ToDouble(textBox7.Text);
            nx = Convert.ToInt32(textBox14.Text);                       // Текущий размер
            ny = Convert.ToInt32(textBox15.Text);
            dx = Convert.ToInt32(textBox20.Text);                       // Полосы по краям

            int nu = 255;                                               // Число уровней
            Form1.zArrayDescriptor[k12 - 1] = Model_Sinus.Intensity2(nu, nx, ny, dx, gamma);
            VisualRegImage(k12 - 1);

            //Close();
        }
  /// <summary>
  ///  Вертикальные линии для определения геометрических искажений
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            int k = Convert.ToInt32(textBox8.Text) - 1;
            if (Form1.zArrayDescriptor[k] == null) { MessageBox.Show("CorrectBr zArrayDescriptor == NULL"); return; }
            int Nx = Form1.zArrayDescriptor[k].width;
            int Ny = Form1.zArrayDescriptor[k].height;

            X0 = Convert.ToInt32(textBox17.Text);    // Полосы рисуются начиная с X0
            STEP = Convert.ToInt32(textBox19.Text);  //Шаг полос

            for (int i = X0; i < Nx; i += STEP)
            {
                for (int j = 0; j < Ny; j++)
                {
                    double z = Form1.zArrayDescriptor[k].array[i, j];
                    if (z < 128) Form1.zArrayDescriptor[k].array[i, j] = 255; else Form1.zArrayDescriptor[k].array[i, j] = 0;
                }
            }
            VisualRegImage(k);
        }


        /// <summary>
        /// Ч/Б полосы с заданным числом градаций
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            k12 = Convert.ToInt32(textBox8.Text);
            nx = Convert.ToInt32(textBox14.Text);                       // Текущий размер
            ny = Convert.ToInt32(textBox15.Text);
            kv = Convert.ToInt32(textBox16.Text);                       // Число градация
            
            Form1.zArrayDescriptor[k12 - 1] = BW_Line( nx, ny, kv);
            VisualRegImage(k12 - 1);

        }
        /// <summary>
        /// Выделение белых и черных полос выше порога - белая ниже - черная.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox3.Text);
            porog = Convert.ToInt32(textBox9.Text);

            if (Form1.zArrayDescriptor[k1 - 1] == null) { MessageBox.Show("CorrectBr zArrayDescriptor == NULL"); return; }
           
            Form1.zArrayDescriptor[k2 - 1] = BW_Line_255(Form1.zArrayDescriptor[k1 - 1], porog);
            VisualRegImage(k2 - 1);
            Close();
        }

        /// <summary>
        /// Усреднение клина по X
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox3.Text);
           // porog = Convert.ToInt32(textBox9.Text);

            if (Form1.zArrayDescriptor[k1 - 1] == null) { MessageBox.Show("CorrectBr zArrayDescriptor 1 == NULL"); return; }
            if (Form1.zArrayDescriptor[k2 - 1] == null) { MessageBox.Show("CorrectBr zArrayDescriptor 2 == NULL"); return; }

            Form1.zArrayDescriptor[k2] = Summ_Y(Form1.zArrayDescriptor[k2 - 1], Form1.zArrayDescriptor[k1 - 1]); //  => 3
            VisualRegImage(k2);

            Close();
        }

        private ZArrayDescriptor Summ_Y(ZArrayDescriptor zArrayCl, ZArrayDescriptor zArrayP) // Суммирование по X
        {
            int nx = zArrayCl.width;
            int ny = zArrayCl.height;

            ZArrayDescriptor cmpl_1 = new ZArrayDescriptor(nx, ny);      // Новый клин
          

            double[] am1 = new double[nx];
            double[] am2 = new double[nx];
            double[] am3 = new double[nx];

            int N_line = 128;

            for (int i = 0; i < nx; i++)
            {
                am1[i] = zArrayP.array[i, N_line];
                am2[i] = zArrayCl.array[i, N_line];
            }

            int ii = 0, i2 = 0, i1 = 0;
            double s = 0;

            do
            {
                i1 = ii;
                s = 0; while (am1[ii] == 0) { s = s + am2[ii]; ii++; if (ii >= nx) break; }
                i2 = ii--; s = s / (i2 - i1);
                for (int i = i1; i < i2; i++) am3[i] = s;

                ii++; i1 = ii; if (ii >= nx) break;
                s = 0; while (am1[ii] != 0) { s = s + am2[ii]; ii++; if (ii >= nx) break; }
                i2 = ii--; s = s / (i2 - i1);
                for (int i = i1; i < i2; i++) am3[i] = s;
                ii++;
            }
            while (ii < nx);


            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                   {  cmpl_1.array[i, j] = am3[i];  }
            
            return cmpl_1; 
        }


        private ZArrayDescriptor BW_Line( int nx, int ny, int kv )  // Ч/Б полосы с заданным числом градаций
        {
           
            int nx1 = nx + dx * 2;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx1, ny);

            int step = nx / kv;

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    if ((i / step) % 2 == 0) cmpl.array[i + dx, j] = 0; else cmpl.array[i + dx, j] = 255;
                }

            cmpl = Model_Sinus.Intens(255, 0, dx, cmpl);     // Белая и черная полоса по краям

            return cmpl;
        }

        private ZArrayDescriptor BW_Line_255(ZArrayDescriptor zArray, int porog) // Повышение контраста полос
        {
            int nx = zArray.width;
            int ny = zArray.height;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);      // Результирующий фронт

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    double a = zArray.array[i, j];
                    if (a > porog) cmpl.array[i, j] = 255; else cmpl.array[i, j] = 0;
                }
           
            return cmpl;
        }

        private void button13_Click(object sender, EventArgs e)          // Нумерация полос полос
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox3.Text);
          
            if (Form1.zArrayDescriptor[k1 - 1] == null) { MessageBox.Show("CorrectBr Нумерация полос zArrayDescriptor 1 == NULL"); return; }

            int N_line = 128;
            double[] am = BW_Num( Form1.zArrayDescriptor[k1 - 1], N_line);          //  => k2

            int nx = Form1.zArrayDescriptor[k1 - 1].width;
            int ny = Form1.zArrayDescriptor[k1 - 1].height;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);      // Результирующий фронт

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                   { cmpl.array[i, j] = am[i]; }
            Form1.zArrayDescriptor[k2 - 1] = cmpl;
            VisualRegImage(k2-1);

            Close();
        }

        private double [] BW_Num(ZArrayDescriptor zArray, int N_line)       // Нумерация полос
        {
            int nx = zArray.width;
            int ny = zArray.height;

            double[] am1 = new double[nx];
            double[] am3 = new double[nx];

            for (int i = 0; i < nx; i++)
            {
                am1[i] = zArray.array[i, N_line];
            }

            int ii = 0, i2 = 0, i1 = 0, k1 = 0;

            do
            {
                i1 = ii;
                while (am1[ii] == 0) {  ii++; if (ii >= nx) break; }
                i2 = ii--; 
                for (int i = i1; i < i2; i++) am3[i] = k1;

                k1++;

                ii++; i1 = ii; if (ii >= nx) break;
                while (am1[ii] != 0) {  ii++; if (ii >= nx) break; }
                i2 = ii--; 
                for (int i = i1; i < i2; i++) am3[i] = k1;
                ii++;
                k1++;
            }
            while (ii < nx);

            return am3;
        }

        private ZArrayDescriptor Bright(int nx, int ny, int c)  // Однотонный цвет
        {
          
            int nx1 = nx + dx * 2;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx1, ny);

        

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                 {
                    cmpl.array[i + dx, j] = c;
                 }

            cmpl = Model_Sinus.Intens(255, 0, dx, cmpl);     // Белая и черная полоса по краям

            return cmpl;
        }

        /// <summary>
        /// Клин с 16 значениями интенсивности из массива
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
   
            k12 = Convert.ToInt32(textBox8.Text);
          
            nx = Convert.ToInt32(textBox14.Text);                       // Текущий размер
            ny = Convert.ToInt32(textBox15.Text);          

            double[] am = new double[nx];
            //for (int i = 0; i < nx; i++) { am[i] = cl[i / 256]; }      // строка клина 128 -  0-31  256 -  0-15

            
            kv = Convert.ToInt32(textBox16.Text);                      //  Число градаций
            am = Clin(cl, kv, nx);                                     // Формирование клина

            if (am == null ) { MessageBox.Show("kv != 16, 32, 64, 128  kv= " + kv); return; }

            int Nx1 = nx + dx * 2; 
            
            ZArrayDescriptor cmpl = new ZArrayDescriptor(Nx1, ny);
            // ----------------------------------------------------------------------------------- Клин
            for (int i = 0; i < nx; i++)
               for (int j = 0; j < ny; j++)
                   {  cmpl.array[i + dx, j] = am[i];  }

            cmpl = Model_Sinus.Intens(255, 0, dx, cmpl);     // Белая и черная полоса по краям
            Form1.zArrayDescriptor[k12 - 1] = cmpl;
            VisualRegImage(k12 - 1);
            // ----------------------------------------------------------------------------------- Обратный клин
          //  ZArrayDescriptor cmpl1 = new ZArrayDescriptor(Nx1, ny);
          //  for (int i = 0; i < nx; i++)
          //      for (int j = 0; j < ny; j++)
          //      { cmpl1.array[i + dx, j] = am[nx-1-i]; }

          //  cmpl1 = Model_Sinus.Intens(0, 256, dx, cmpl1);     // Белая и черная полоса по краям
          //  Form1.zArrayDescriptor[k12 - 1 + 1] = cmpl1;
          //  VisualRegImage(k12 - 1 + 1);

        }


        private double[] Clin(double[] cl, int kv, int nx)
        {
            int nx1 = cl.GetLength(0);
            double[] cl1 = new double[nx1];
            for (int i = 0; i < nx1; i++) cl1[i] = cl[i];

            int nx2 = nx / 16;

            double[] am = new double[nx];
            int nsd = 0;
            switch (kv)
            {
                case 16: for (int i = 0; i < nx; i++) { am[i] = cl1[i / nx2]; } break;    // 
                case 32:
                    cl1 = MasX2(cl1);
                    nx2 = nx / 32;
                    nsd = nx / 32;
                    break;  // 4096-128
                case 64:
                    cl1 = MasX2(cl1); cl1 = MasX2(cl1);
                    nx2 = nx / 64;
                    nsd = nx / 32 + nx / 64;
                    break;
                case 128:
                    cl1 = MasX2(cl1); cl1 = MasX2(cl1); cl1 = MasX2(cl1);
                    nx2 = nx / 128;
                    nsd = nx / 32 + nx / 64 + nx / 128;
                    break;
                case 256:
                    cl1 = MasX2(cl1); cl1 = MasX2(cl1); cl1 = MasX2(cl1); cl1 = MasX2(cl1);
                    nx2 = nx / 256;
                    nsd = nx / 32 + nx / 64 + nx / 128 + nx / 256;
                    break;
                default: return null;
            }
            for (int i = 0; i < nx; i++) { am[i] = cl1[i / nx2]; }
            am = Strerch(am, nsd);
            return am;

        }

        /// <summary>
        /// Растяжение массива 0 - n-nx -> 0 - n
        /// </summary>
        /// <param name="am"></param>
        /// <returns></returns>
        private double[] Strerch(double[] am, int nx)
        {
           int n = am.Length;        double[] am2 = new double[n];
           int n1 = n - nx;

           for (int i=0; i < n; i++)   
            { 
              int i1 = (n1 - 1) * i / n ;
              am2[i] = am[i1];
            }
            return am2;
        }
        private double[] MasX2(double[] am)
        {
            int nx  = am.Length;
            int nx2 = nx * 2;
            double[] am2 = new double[nx2];

           for (int i = 0; i < nx2-1; i++)
              {
                 if (i % 2 == 0) am2[i] =  am[i/2];
                 if (i % 2 != 0) am2[i] = (am[i/2] + am[i / 2 + 1])/2;
              }

            return am2;
        }
        public double[] InterpolateClin(double[] clin)
        {
            if (clin == null)  { return null;  }

            double[] resClin = clin;

            for (int i = 1; i <= 4; i++)
            {
                resClin = MasX2(resClin);
            }

            return resClin;
        }
        public static double CorrectValueByClin(double idealValue, double[] clinArray, int idealCount)
        {
            if (clinArray == null) { return idealValue; }
            double clinArrayCount = 240;
            
            int value = Convert.ToInt32(idealValue * clinArrayCount / idealCount);  // от 0 до 240
            double resValue = clinArray[value];
            return resValue;
        }
        /// <summary>
        /// Сложение строк от Y1 до Y2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox3.Text);
            On_CorrectSumm(k1-1, k2 - 1);
            Close();

        }
        /// <summary>
        ///  Растяжение массива для проекции
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       // private void button5_Click(object sender, EventArgs e)
      //  {
           // textBox9.Text = Convert.ToString(k14);    // Сложение строк
          
      //      k1 = Convert.ToInt32(textBox1.Text);
     //       k2 = Convert.ToInt32(textBox3.Text);
      //      N_Line = Convert.ToInt32(textBox13.Text);
      //      nx= Convert.ToInt32(textBox14.Text);
       //     ny = Convert.ToInt32(textBox15.Text);
      //      On_CorrectGxy(k1 - 1, k2 - 1, N_Line, nx, ny);
      //      Close();

     //   }
        /// <summary>
        /// Выбрасывание белой и черной полосы слева и справа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
           
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox3.Text);
            dx    = Convert.ToInt32(textBox20.Text);

            if (Form1.zArrayDescriptor[k1-1] == null) { MessageBox.Show("CorrectBr zArrayDescriptor == NULL"); return; }
           
            Form1.zArrayDescriptor[k2-1] = Minus100(Form1.zArrayDescriptor[k1-1], dx);
            VisualRegImage(k2-1);
            Close();
        }

        private ZArrayDescriptor Minus100(ZArrayDescriptor Array, int dx)  // Убирается по 100 точек с обоих сторон
        {
            int nx1 = Array.width;
            int ny  = Array.height;
            nx = nx1 - dx * 2;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);      // Результирующий фронт

            for (int i = dx; i < nx1 - dx; i++)
                for (int j = 0; j < ny; j++)
                {
                    cmpl.array[i - dx, j] = Array.array[i, j];
                }

            return cmpl;
        }


        //-------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Ввод клина с камеры и обработка с усреднением
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            int nx = 3996;
            int ny = 2160;

            int nx1 = nx + dx * 2;              // Размер для изображения с добавленными полосами
           
            int kv = 16;                        //  Число градаций


            // -------------------------------------------------------------------------------------------- Клин => 0  (16 градаций)
            double[] am = Clin(cl, kv, nx);                                                               // Формирование клина
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx1, ny);
            
            for (int i = 0; i < nx; i++) for (int j = 0; j < ny; j++)  { cmpl.array[i + dx, j] = am[i]; } // ----------------- Клин
            cmpl = Model_Sinus.Intens(255, 0, dx, cmpl);                                                  // Белая и черная полоса по краям
            Form1.zArrayDescriptor[0] = cmpl;                                                            // Клин в 1 массив
            VisualRegImage(0);
          
                     TakePhoto12();                                                                                //-------- Фото => 1

                     DialogResult dialogResult = MessageBox.Show("Заданы границы X0, X1, X2, X3 ?", "Ограничение по размеру", MessageBoxButtons.YesNo);
                     if (dialogResult == DialogResult.No)  { return;           }

                     Form1.Coords[] X = MainForm.GetCoordinates();
          
                               //MessageBox.Show(" X1 - " + Form1.X1 + " Y1 - " + Form1.Y1);

                                           //---------------------------------------------------------------------------------------------- Ограничение клина по размеру => 2
                                           Form1.zArrayDescriptor[2] = File_Change_Size.Change_rectangle(Form1.zArrayDescriptor[1], X);
                                           VisualRegImage(2);
                                           //MessageBox.Show("Ограничение клина по размеру прошло");
                                           int nx2 = Form1.zArrayDescriptor[2].width;                                       // Размер массива после ограничения
                                           int ny2 = Form1.zArrayDescriptor[2].height;

                                           int N_Line = ny2/2;                                                              // Полоса по центру

                                           //int y1 = File_Change_Size.MinY(X);   // Минимальное значение по Y
                                           //int y2 = File_Change_Size.MaxY(X);   // Максимальное значение по Y
                                           Form1.zArrayDescriptor[2] = SumClass.Sum_zArrayY_ALL(Form1.zArrayDescriptor[2]);  //------------------------- Усреднение по Y => 2
                                           VisualRegImage(2); 
                                           //MessageBox.Show("Усреднение по Y прошло");
  
                                           //---------------------------------------------------------------------------------------------- Черно белые полосы => 0
                                           Form1.zArrayDescriptor[0] = BW_Line(nx, ny, kv);     // Полосы с kv градациями
                                           VisualRegImage(0);

                                           TakePhoto12();                                                                               //---------------------------- Фото => 1
                                           MessageBox.Show("Полосы введены");

                                           Form1.zArrayDescriptor[3] = File_Change_Size.Change_rectangle(Form1.zArrayDescriptor[1], X); //----------------- Ограничение по размеру => 2
                                           VisualRegImage(3);

                                           //Form1.zArrayDescriptor[3] = SumClass.Sum_zArrayY_ALL(Form1.zArrayDescriptor[3]);
                                           Form1.zArrayDescriptor[4] = BW_Line_255(Form1.zArrayDescriptor[3], 210);                     // --------------Выше порога 255 ниже 0
                                           VisualRegImage(4);
                                           double[] am_BW = BW_Num(Form1.zArrayDescriptor[4], N_Line);                                  // размер новый после прямоугольного ограничения
                                           //MessageBox.Show("Контраст прошло");
                                           //--------------------------------------------------------------------------------------------- Усреднение по X клина => 6
                    Form1.zArrayDescriptor[5] = Summ_Y(Form1.zArrayDescriptor[2], Form1.zArrayDescriptor[4]);                      
                    VisualRegImage(5);
                              
        }
        /// <summary>
        /// Определение  клина 16 градаций 
        /// </summary>
        /// <param name="am_Clin"></param>           Отклик от идеального клина размер от 0 до nx
        /// <param name="am_BW"></param>             Нумерация полос от 0 до 15 размер от 0 до nx
        /// <param name="am_Clin_Ideal"></param>     Клин от 0 до 255           размер от 0 до nx
        /// <param name=" Black,  White"></param> Уровни черного и белого при отражении от объекта
        /// <returns></returns>
        private double [] NewClin( double [] am_Clin,  double[] am_BW, double[] am_Clin_Ideal)
        {
            double[] cl1 = new double[16];
            //double[] cl0 = new double[16];
            int nx = am_Clin.GetLength(0);
            //int White = 255;
            //int Black = 0;
            double[] cl0 = { 0, 36, 48, 64, 80, 96, 112, 128, 144, 160, 174, 190, 206, 222, 238, 255 };  // шаг 16
            //cl0[0] = 0; cl0[15] = 255;
            cl1[0] = 0; cl1[15] = 255;
            //double kv =( White - Black) / 15;
            //for (int k = 1; k < 15; k++) cl0[k] = kv * k;

            for (int k = 1; k < 15; k++)
            { 
                for (int i = 0; i < nx; i++)
                {
                  if ( (int)am_BW[i] == k)
                    {
                        int br = (int)cl0[k];                       // должен быть такой цвет
                        MessageBox.Show("Уровень " + k + " идеальное значение" + br);
                        int flag = 0;
                        for (int ii = 0; ii < nx; ii++)
                           { if ((int)am_Clin[ii] == br)           // Отклик от идеального клина 0-255
                                {
                                  cl1[k] = am_Clin_Ideal[ii];
                                  flag = 1; goto M;
                                }
                           }
                        if (flag == 0)  { MessageBox.Show("Уровень " + k + " не найден"); goto M; }
                    }
                }
                M: continue;
            }

            return cl1;
        }
        /// <summary>
        /// Автоматическое определение клина
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {
            int nx = 3096;
            int ny = 2160;

          
            int nx1 = nx + dx * 2;              // Размер для изображения с добавленными полосами

            int kv = 16;                        //  Число градаций

            Form1.zArrayDescriptor[0] = Model_Sinus.Intensity1(255, nx, ny, dx, 1);                             // Идеальная интенсивностсть => 1
            VisualRegImage(0);
            TakePhoto12();
            VisualRegImage(1);
            DialogResult dialogResult = MessageBox.Show("Заданы границы X0, X1, X2, X3 ?", "Ограничение по размеру", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No) { return; }

            Form1.Coords[] X = MainForm.GetCoordinates();
           
            Form1.zArrayDescriptor[6] = File_Change_Size.Change_rectangle(Form1.zArrayDescriptor[1], X);    // Ограничение по размеру 2 => 7
            Form1.zArrayDescriptor[6] = SumClass.Sum_zArrayY_ALL(Form1.zArrayDescriptor[6]);                // Суммирование по Y      7 => 7
            VisualRegImage(6);
            int nx2 = Form1.zArrayDescriptor[6].width;                                                      // Размер массива после ограничения
            int ny2 = Form1.zArrayDescriptor[6].height;
            int N_Line = ny2 / 2;                                                                           // Линия по центральной строке


            //---------------------------------------------------------------------------------------------- Идеальный клин без ввода с новыми размерами  

            Form1.zArrayDescriptor[0] = Model_Sinus.Intensity1(255, nx2, ny2, dx, 1);
            Form1.zArrayDescriptor[1] = Minus100(Form1.zArrayDescriptor[0], 100);
            VisualRegImage(1);
            double[] am_Clin_Ideal = new double[nx2];                                                           // Идеальный клин от 0 до 255 
            for (int i = 0; i < nx2; i++) { am_Clin_Ideal[i] = Form1.zArrayDescriptor[1].array[i, N_Line]; }

            double[] am_Clin = new double[nx2];                                                                 // Отклик от идеального клина от 0 до 255 
            for (int i = 0; i < nx2; i++) { am_Clin[i] = Form1.zArrayDescriptor[6].array[i, N_Line]; }

           

            //---------------------------------------------------------------------------------------------- Черно белые полосы => 0
            Form1.zArrayDescriptor[0] = BW_Line(nx, ny, kv);     // Полосы с kv градациями
            VisualRegImage(0);
            TakePhoto12();                                                                               //---------------------------- Фото => 1
            MessageBox.Show("Ч/Б полосы введены");

            Form1.zArrayDescriptor[3] = File_Change_Size.Change_rectangle(Form1.zArrayDescriptor[1], X); //----------------- Ограничение по размеру => 2
            VisualRegImage(3);

            Form1.zArrayDescriptor[3] = SumClass.Sum_zArrayY_ALL(Form1.zArrayDescriptor[3]);
            Form1.zArrayDescriptor[4] = BW_Line_255(Form1.zArrayDescriptor[3], 210);                     // --------------Выше порога 255 ниже 0
            VisualRegImage(4);
            double[] am_BW = BW_Num(Form1.zArrayDescriptor[4], N_Line);                                  // размер новый после прямоугольного ограничения

            //----------------------------------------------------------------------------------------------    Определение нового клина

            double[] cl1 = NewClin(am_Clin, am_BW, am_Clin_Ideal);

            //----------------------------------------------------------------------------------------------Отображение нового клина
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx1, ny);
            double[] am = Clin(cl1, kv, nx);
            for (int i = 0; i < nx; i++) for (int j = 0; j < ny; j++) { cmpl.array[i + dx, j] = am[i]; }  // ----------------- Клин в рабочий массив
            cmpl = Model_Sinus.Intens(255, 0, dx, cmpl);                                                  // Белая и черная полоса по краям
            Form1.zArrayDescriptor[7] = cmpl;                                                             // новый клин в 7 массив
            VisualRegImage(7);

            for (int i = 0; i < 16; i++) cl[i] = cl1[i];
        }

        private void LoadWedge()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.txt)|*.txt";
            openFileDialog.DefaultExt = "txt";

            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                if (!string.IsNullOrEmpty(filePath))
                {
                    List<double> valuesList = new List<double>();
                    using (FileStream fs = File.OpenRead(filePath))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            while (!sr.EndOfStream)
                            {
                                string stringValue = sr.ReadLine();
                                if (!string.IsNullOrEmpty(stringValue))
                                {
                                    valuesList.Add(double.Parse(stringValue));
                                }
                            }
                        }
                    }

                    cl = valuesList.ToArray();
                    MessageBox.Show("Клин загружен (cl)");
                }
            }
        }

        private void SaveWedge()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "(*.txt)|*.txt";
            saveFileDialog.DefaultExt = "txt";
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                if (!string.IsNullOrEmpty(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            if (cl != null)
                            {
                                for (int i = 0; i < cl.Length; i++)
                                {
                                    sw.WriteLine(cl[i]);
                                }
                            }
                            sw.Flush();
                        }
                    }
                    MessageBox.Show("Клин сохранен (cl)");
                }
            }
        }

        private void SaveWedgeButton_Click(object sender, EventArgs e)
        {
            SaveWedge();
        }

        private void LoadWedgeButton_Click(object sender, EventArgs e)
        {
            LoadWedge();
        }
        /// <summary>
        /// Клин 16 => 1 Клин 256 => 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button16_Click(object sender, EventArgs e)
        {
            int nx = 3840;
            int ny = 2160;
            //double[] cl = { 16, 32, 48, 64, 80, 96, 112, 128, 144, 160, 174, 190, 206, 222, 238, 255 };

            int k1 = 8;
            for (int k = 0; k < 5; k++)
            {
                k1 = k1 * 2;
                double[] am = Clin(cl, k1, nx);
                ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);
                for (int i = 0; i < nx; i++) for (int j = 0; j < ny; j++) {  cmpl.array[i,j]   = am[i];  }
                Form1.zArrayDescriptor[k] = cmpl;
                VisualRegImage(k);
            }

           

        }
        
       

    }
}
