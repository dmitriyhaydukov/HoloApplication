using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;
using ClassLibrary;
using System.Diagnostics;


namespace rab1.Forms
{
    public class ATAN_PSI
    {
        // Нахождение ортогонального вектора

        public static double[] Vector_orto(double[] vect)
        {
            int n = vect.Length;
            double[] v_sdv = new double[n];

            v_sdv[0] = vect[1] - vect[n - 1];
            v_sdv[n - 1] = vect[0] - vect[n - 2];
            for (int ii = 1; ii < n - 1; ii++) { v_sdv[ii] = vect[ii + 1] - vect[ii - 1]; }

            return v_sdv;
        }

        // Умножение векторов

        public static double Vector_Mul(double[] vect1, double[] vect2)
        {
            int n = vect1.Length;
            double s = 0;

            for (int ii = 0; ii < n; ii++) { s += vect1[ii] * vect2[ii]; }

            return s;
        }
        // Определение амплитуды и фазы по 4 картинам (PSI), записанным в 9, 10, 11, и 12 окнах
        public static ZComplexDescriptor ATAN_891011(ZArrayDescriptor[] zArrayPicture, ProgressBar progressBar1, double[] fzz, double amplit=0.5)
        {
            // 8, 9, 10, 11   ->    Complex[1] 

            int w1 = zArrayPicture[8].width;
            int h1 = zArrayPicture[8].height;
            // MessageBox.Show("width= " + w1 + "height= " + h1);
            //MessageBox.Show("fzz[i]= " + fzz[0] + "  " + fzz[1] + "  " + fzz[2] + "  " + fzz[3]);

            //double Ar = SumClass.getAverage(zArrayPicture[8]);
            double Ar = amplit;

            ZComplexDescriptor faza = new ZComplexDescriptor(w1, h1);

            int n_sdv = 4;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];
          

            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fzz[i]);
                k_cos[i] = Math.Cos(fzz[i]);
            }

            double[] sin_orto = Vector_orto(k_sin);                              // Ортогональные вектора
            double[] cos_orto = Vector_orto(k_cos);
            double znmt = Math.Abs(Vector_Mul(k_cos, sin_orto));

            //ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture[1 * 4 + 2]);
            //ZArrayDescriptor ampf = new ZArrayDescriptor(zArrayPicture[1 * 4 + 3]);

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1 - 1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    i_sdv[0] = zArrayPicture[8].array[i, j];
                    i_sdv[1] = zArrayPicture[9].array[i, j];
                    i_sdv[2] = zArrayPicture[10].array[i, j];
                    i_sdv[3] = zArrayPicture[11].array[i, j];

                    double[] i_sdv_orto = Vector_orto(i_sdv);                              // Ортогональные вектора
                   
                    double[] v_sdv = Vector_orto(i_sdv);                 // ------  Формула расшифровки фазы
                    double fz1 = Vector_Mul(v_sdv, k_sin);
                    double fz2 = Vector_Mul(v_sdv, k_cos);
                    
                    //double fz1 = Vector_Mul(i_sdv, sin_orto);
                    //double fz2 = Vector_Mul(i_sdv, cos_orto);]

                    double f = Math.Atan2(fz2, fz1);

                    //double[] cos_orto = Vector_orto(k_cos);              // ------  Формула расшифровки амплитуды                   
                    //double znmt = Vector_Mul(sin_orto, k_cos);
                    double am = Math.Sqrt(fz1 * fz1 + fz2 * fz2) / znmt;
                    //am = am / (2 * amplit);
                    am = am / (2 * Ar);
                   

                    faza.array[i, j] = Complex.FromPolarCoordinates(am, f);
                    
                }
                progressBar1.PerformStep();
            }
            progressBar1.Value = 1;
            return faza;
        }

        // Определение фазы от 0 до 2pi

        public static ZArrayDescriptor ATAN(ZArrayDescriptor[] zArrayPicture, int regComplex, double[] fz)
        {
            // regComplex   ->    Главное окно

            int w1 = zArrayPicture[regComplex * 4].width;
            int h1 = zArrayPicture[regComplex * 4].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            int n_sdv = fz.Length;                                                       // Число фазовых сдвигов
            //MessageBox.Show(" fz.Length= " + n );

            double[] i_sdv = new double[n_sdv];
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];


            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fz[i]);
                k_cos[i] = Math.Cos(fz[i]);
            }
            k_sin = Vector_orto(k_sin);  // Получение ортогональных векторов для синуса и косинуса
            k_cos = Vector_orto(k_cos);

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    for (int ii = 0; ii < n_sdv; ii++) { i_sdv[ii] = zArrayPicture[regComplex * 4 + ii].array[i, j]; }

                    //double[] v_sdv = Vector_orto(i_sdv);                // ------  Формула расшифровки фазы
                    //double fz1 = Vector_Mul(v_sdv, k_sin);              // +3 * Math.PI / 2;
                    //double fz2 = Vector_Mul(v_sdv, k_cos);
                    //faza.array[i, j] = 2 * Math.PI - (Math.Atan2(fz1, fz2) + Math.PI);
                    double fz1 = Vector_Mul(i_sdv, k_sin);              // +3 * Math.PI / 2;
                    double fz2 = Vector_Mul(i_sdv, k_cos);
                    //faza.array[i, j] = Math.Atan2(fz2, fz1);
                    //faza.array[i, j] = 2 * Math.PI - (Math.Atan2(fz1, fz2) + Math.PI);
                    faza.array[i, j] = Math.Atan2(fz1, fz2);
                }
            }

            return faza;
        }
      

        public static ZArrayDescriptor ATAN_Faza(ZArrayDescriptor[] zArray, double[] fz)
        {
            // regComplex   ->    Главное окно

            int w1 = zArray[0].width;
            int h1 = zArray[0].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            int n_sdv = 4;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];


            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fz[i]);
                k_cos[i] = Math.Cos(fz[i]);
            }

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    i_sdv[0] = zArray[0].array[i, j];
                    i_sdv[1] = zArray[1].array[i, j];
                    i_sdv[2] = zArray[2].array[i, j];
                    i_sdv[3] = zArray[3].array[i, j];

                    double[] v_sdv = Vector_orto(i_sdv);                // ------  Формула расшифровки фазы
                    double fz1 = Vector_Mul(v_sdv, k_sin);              // +3 * Math.PI / 2;
                    double fz2 = Vector_Mul(v_sdv, k_cos);
                    //faza.array[i, j] = 2 * Math.PI - (Math.Atan2(fz1, fz2) + Math.PI);
                    faza.array[i, j] = Math.Atan2(fz1, fz2);
                }
            }

            return faza;
        }

        public static double[] ATAN(double[][] arrays, int regComplex, double[] fz)
        {
            int w1 = arrays[regComplex * 4].Length;
            
            double[] faza = new double[w1];

            int n_sdv = fz.Length;      // Число фазовых сдвигов
            
            double[] i_sdv = new double[n_sdv];
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];

            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fz[i]);
                k_cos[i] = Math.Cos(fz[i]);
            }
            k_sin = Vector_orto(k_sin);  // Получение ортогональных векторов для синуса и косинуса
            k_cos = Vector_orto(k_cos);

            for (int i = 0; i < w1; i++)
            {
                for (int ii = 0; ii < n_sdv; ii++)
                {
                    i_sdv[ii] = arrays[regComplex * 4 + ii][i];
                }

                double fz1 = Vector_Mul(i_sdv, k_sin);
                double fz2 = Vector_Mul(i_sdv, k_cos);
                    
                faza[i] = Math.Atan2(fz1, fz2);
            }

            return faza;
        }

        /// <summary>
        ///  Угол по формуле Carre
        /// </summary>
        /// <param name="zArrayDescriptor"></param>
        /// <param name="regComplex"></param>
        /// <returns></returns>
        public static ZArrayDescriptor ATAN_Sdvg(ZArrayDescriptor[] zArrayDescriptor, int regComplex)
        {
            int w1 = zArrayDescriptor[regComplex * 4].width;
            int h1 = zArrayDescriptor[regComplex * 4].height;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(w1, h1);        // Массив для углов
            double s1 = 0, s = 0, n = 0;
            double kf = 180 / Math.PI;
            double b = 0;
            for (int j = 0; j < h1; j++)
            {
                //double s = 0;
                for (int i = 0; i < w1; i++)
                {
                    double a1 = zArrayDescriptor[regComplex * 4].array[i, j];
                    double a2 = zArrayDescriptor[regComplex * 4 + 1].array[i, j];
                    double a3 = zArrayDescriptor[regComplex * 4 + 2].array[i, j];
                    double a4 = zArrayDescriptor[regComplex * 4 + 3].array[i, j];
                    double ch = 3 * (a2 - a3) - (a1 - a4);
                    double zn = (a1 - a4) + (a2 - a3);
                    b = 2 * Math.Atan(Math.Sqrt(Math.Abs(ch / zn)));
                    //try { b = Math.Sqrt(Math.Abs(ch / zn));  } catch { b = 1.57; }
                    //b = Math.Sqrt(Math.Abs(ch / zn));
                    if (Double.IsNaN(b)) { b = 1.57; }
                    cmpl.array[i, j] = b;
                    n = n + 1;
                    s = s + b;
                }
            }
           
            s1 = s / n;

            double s2, d = 0.4;  // d*180/pi  в градусах 0.1 - 5,7 градусов   0.2  - 11    0.4 - 22
            n = 0; s = 0;

            for (int j = 0; j < h1; j++)
            {
                //double s = 0;
                for (int i = 0; i < w1; i++)
                {
                    double a1 = zArrayDescriptor[regComplex * 4].array[i, j];
                    double a2 = zArrayDescriptor[regComplex * 4 + 1].array[i, j];
                    double a3 = zArrayDescriptor[regComplex * 4 + 2].array[i, j];
                    double a4 = zArrayDescriptor[regComplex * 4 + 3].array[i, j];
                    double ch = 3 * (a2 - a3) - (a1 - a4);
                    double zn = (a1 - a4) + (a2 - a3);
                    b = 2 * Math.Atan(Math.Sqrt(Math.Abs(ch / zn)));
                  
                    if (Double.IsNaN(b)) {  b = 1.57; }
                    if (b > (s1 + d)) { b = 1.57; }
                    if (b < (s1 - d)) { b = 1.57; }
                    //cmpl.array[i, j] = b;
                    n = n + 1;
                    s = s + b;
                }

            }

            s2 = s / n;

            MessageBox.Show("Средний угол = " + s1 * kf + "Средний угол +-10 = " + s2 * kf);
          
            return cmpl;
        }

        public static ZArrayDescriptor ATAN_Faza_Carre(ZArrayDescriptor[] zArray, int regComplex, ProgressBar progressBar1)
        {
            // regComplex   ->    Главное окно

            int w1 = zArray[regComplex * 4].width;
            int h1 = zArray[regComplex * 4].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);


            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1 - 1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    double i1 = zArray[regComplex * 4].array[i, j];
                    double i2 = zArray[regComplex * 4 + 1].array[i, j];
                    double i3 = zArray[regComplex * 4 + 2].array[i, j];
                    double i4 = zArray[regComplex * 4 + 3].array[i, j];

                    double i23 = i2 - i3;
                    double i14 = i1 - i4;
                    int zsn = Math.Sign(-i23);  
                    //int zcn = Math.Sign((i2+i3) - (i1+i4));

                    double sn = Math.Sqrt( Math.Abs((i14 + i23)*(3*i23 - i14))  ) ;
                    double cn = (i2+i3)-(i1+i4);

                    double fi = Math.Atan2(zsn * sn, cn);
                    fi = Math.Atan2(zsn*sn, cn) - 3* Math.PI /  4;
                    if (fi < -Math.PI) fi = fi + 2*Math.PI;
                    faza.array[i, j] = fi;
                }
                progressBar1.PerformStep();


            }
            progressBar1.Value = 1;
            return faza;
        }
        /// <summary>
        /// 3-точечный и 4-точечный алгоритмы
        /// </summary>
        /// <param name="zArray"></param>
        /// <param name="regComplex"></param>
        /// <param name="fz"></param>
        /// <returns></returns>
        public static ZArrayDescriptor ATAN_3(ZArrayDescriptor[] zArray, int regComplex, double[] fz) // regComplex   ->    Главное окно
        {


            int w1 = zArray[regComplex * 4].width;
            int h1 = zArray[regComplex * 4].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            int n_sdv = fz.Length;                                                       // Число фазовых сдвигов
            //MessageBox.Show(" fz.Length= " + n );

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    double i1 = zArray[regComplex * 4].array[i, j];
                    double i2 = zArray[regComplex * 4 + 1].array[i, j];
                    double i3 = zArray[regComplex * 4 + 2].array[i, j];
                    

                    double fz1 = i3 - i2;
                    double fz2 = i1 - i2;
                    double fi = Math.Atan2(fz2, fz1);
                    //double fi = Math.Atan2(fz1, fz2) - Math.PI / 2;
                    //if (fi < -Math.PI) fi = fi + 2 * Math.PI;
                    faza.array[i, j] = fi;
                }
            }

            return faza;
        }
        public static ZArrayDescriptor ATAN_4(ZArrayDescriptor[] zArray, int regComplex, double[] fz) // regComplex   ->    Главное окно
        {


            int w1 = zArray[regComplex * 4].width;
            int h1 = zArray[regComplex * 4].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            int n_sdv = fz.Length;                                                       // Число фазовых сдвигов
            //MessageBox.Show(" fz.Length= " + n );

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    double i1 = zArray[regComplex * 4].array[i, j];
                    double i2 = zArray[regComplex * 4 + 1].array[i, j];
                    double i3 = zArray[regComplex * 4 + 2].array[i, j];
                    double i4 = zArray[regComplex * 4 + 3].array[i, j];
                    

                    double fz1 = i4 - i2;
                    double fz2 = i1 - i3;
                    double fi = Math.Atan2(fz2, fz1);
                    //double fi = Math.Atan2(fz1, fz2) - Math.PI / 2;
                    //if (fi < -Math.PI) fi = fi + 2 * Math.PI;
                    faza.array[i, j] = fi;
                }
            }

            return faza;
        }
        /// <summary>
        /// Формула Харихарана
        /// </summary>
        /// <param name="zArrayPicture"></param>
        /// <param name="regComplex"></param>
        /// <param name="fz"></param>
        /// <returns></returns>
        public static ZArrayDescriptor ATAN5(ZArrayDescriptor[] zArray, int regComplex, double[] fz) // regComplex   ->    Главное окно
        {
            

            int w1 = zArray[regComplex * 4].width;
            int h1 = zArray[regComplex * 4].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            int n_sdv = fz.Length;                                                       // Число фазовых сдвигов
            //MessageBox.Show(" fz.Length= " + n );

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    double i1 = zArray[regComplex * 4].array[i, j];
                    double i2 = zArray[regComplex * 4 + 1].array[i, j];
                    double i3 = zArray[regComplex * 4 + 2].array[i, j];
                    double i4 = zArray[regComplex * 4 + 3].array[i, j];
                    double i5 = zArray[regComplex * 4 + 4].array[i, j];

                    double fz1 = 2*(i2-i4);              
                    double fz2 = i1-2*i3+i5;
                    //double fi = Math.Atan2(i1-128, i2-128);
                    //double fi = Math.Atan2(fz1, fz2);
                    double fi = Math.Atan2(fz1, fz2) + Math.PI / 2;
                    if (fi > Math.PI) fi = fi - 2 * Math.PI;
                    faza.array[i, j] = fi;
                }
            }

            return faza;
        }
        public static ZArrayDescriptor ATAN6(ZArrayDescriptor[] zArray, int regComplex, double[] fz) // regComplex   ->    Главное окно
        {


            int w1 = zArray[regComplex * 4].width;
            int h1 = zArray[regComplex * 4].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            int n_sdv = fz.Length;                                                       // Число фазовых сдвигов
            //MessageBox.Show(" fz.Length= " + n );

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    double i1 = zArray[regComplex * 4].array[i, j];
                    double i2 = zArray[regComplex * 4 + 1].array[i, j];
                    double i3 = zArray[regComplex * 4 + 2].array[i, j];
                    double i4 = zArray[regComplex * 4 + 3].array[i, j];
                    double i5 = zArray[regComplex * 4 + 4].array[i, j];
                    double i6 = zArray[regComplex * 4 + 5].array[i, j];

                    double fz1 = 3 * i2 - 4 * i4 + i6;
                    double fz2 = i1 - 4 * i3 + 3*i5;
                    //double fi = Math.Atan2(fz1, fz2);
                    double fi = Math.Atan2(fz1, fz2) + Math.PI / 2;
                    if (fi > Math.PI) fi = fi - 2 * Math.PI;
                    faza.array[i, j] = fi;
                }
            }

            return faza;
        }
        /// <summary>
        /// 7 точечный алгоритм
        /// </summary>
        /// <param name="zArray"></param>
        /// <param name="regComplex"></param>
        /// <param name="fz"></param>
        /// <returns></returns>
        public static ZArrayDescriptor ATAN7(ZArrayDescriptor[] zArray, int regComplex, double[] fz) // regComplex   ->    Главное окно
        {

            int w1 = zArray[regComplex * 4].width;
            int h1 = zArray[regComplex * 4].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            int n_sdv = fz.Length;                                                       // Число фазовых сдвигов
            //MessageBox.Show(" fz.Length= " + n );

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    double i1 = zArray[regComplex * 4].array[i, j];
                    double i2 = zArray[regComplex * 4 + 1].array[i, j];
                    double i3 = zArray[regComplex * 4 + 2].array[i, j];
                    double i4 = zArray[regComplex * 4 + 3].array[i, j];
                    double i5 = zArray[regComplex * 4 + 4].array[i, j];
                    double i6 = zArray[regComplex * 4 + 5].array[i, j];
                    double i7 = zArray[regComplex * 4 + 6].array[i, j];

                    double fz1 = 4 * i2 - 8*i4 + 4*i6;
                    double fz2 = i1 - 7 * i3 + 7*i5 - i7;
                    double fi = Math.Atan2(fz1, fz2) + Math.PI / 2;
                    if (fi > Math.PI) fi = fi - 2 * Math.PI; ;
                 
                    faza.array[i, j] = fi;
                }
            }

            return faza;
        }
        /// <summary>
        /// 8 точечный алгоритм
        /// </summary>
        /// <param name="zArray"></param>
        /// <param name="regComplex"></param>
        /// <param name="fz"></param>
        /// <returns></returns>
        public static ZArrayDescriptor ATAN8(ZArrayDescriptor[] zArray, int regComplex, double[] fz) // regComplex   ->    Главное окно
        {

            int w1 = zArray[regComplex * 4].width;
            int h1 = zArray[regComplex * 4].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            int n_sdv = fz.Length;                                                       // Число фазовых сдвигов
            //MessageBox.Show(" fz.Length= " + n );

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    double i1 = zArray[regComplex * 4].array[i, j];
                    double i2 = zArray[regComplex * 4 + 1].array[i, j];
                    double i3 = zArray[regComplex * 4 + 2].array[i, j];
                    double i4 = zArray[regComplex * 4 + 3].array[i, j];
                    double i5 = zArray[regComplex * 4 + 4].array[i, j];
                    double i6 = zArray[regComplex * 4 + 5].array[i, j];
                    double i7 = zArray[regComplex * 4 + 6].array[i, j];
                    double i8 = zArray[regComplex * 4 + 7].array[i, j];

                    double fz1 = 5 * i2 - 15 * i4 + 11* i6 - i8;
                    double fz2 = i1 - 11 * i3 + 15 * i5 - 5 * i7;
                    double fi = Math.Atan2(fz1, fz2) + Math.PI / 2;
                    if (fi > Math.PI) fi = fi - 2 * Math.PI; ;
                    faza.array[i, j] = fi;
                }
            }

            return faza;
        }

        public static ZComplexDescriptor ATAN_8_11(int k1, int k2, int k3, int k4, ZArrayDescriptor[] zArrayPicture,  double[] fzz, double amplit = 255)
        {
            // 8, 9, 10, 11   ->    Complex[1] 

            int w1 = zArrayPicture[k1].width;
            int h1 = zArrayPicture[k1].height;
            // MessageBox.Show("width= " + w1 + "height= " + h1);

            ZComplexDescriptor faza = new ZComplexDescriptor(w1, h1);

            int n_sdv = 4;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];


            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fzz[i]);
                k_cos[i] = Math.Cos(fzz[i]);
            }

            double[] sin_orto = Vector_orto(k_sin);                              // Ортогональные вектора
            double[] cos_orto = Vector_orto(k_cos);
            double znmt = Math.Abs(Vector_Mul(sin_orto, k_cos));

            //ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture[1 * 4 + 2]);
            //ZArrayDescriptor ampf = new ZArrayDescriptor(zArrayPicture[1 * 4 + 3]);

           
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    i_sdv[0] = zArrayPicture[k1].array[i, j];
                    i_sdv[1] = zArrayPicture[k2].array[i, j];
                    i_sdv[2] = zArrayPicture[k3].array[i, j];
                    i_sdv[3] = zArrayPicture[k4].array[i, j];

                    //double[] v_sdv = Vector_orto(i_sdv);                 // ------  Формула расшифровки фазы
                    //double fz1 = Vector_Mul(v_sdv, k_sin);
                    //double fz2 = Vector_Mul(v_sdv, k_cos);
                    double fz1 = Vector_Mul(i_sdv, sin_orto);
                    double fz2 = Vector_Mul(i_sdv, cos_orto);
                    double a = Math.Atan2(fz2, fz1);

                    //double[] cos_orto = Vector_orto(k_cos);              // ------  Формула расшифровки амплитуды                   
                    //double znmt = Vector_Mul(sin_orto, k_cos);
                    double am = Math.Sqrt(fz1 * fz1 + fz2 * fz2) / znmt;
                    //am = am / (2 * amplit);

                    //a= ampf.array[i, j];
                    // am = amp.array[i, j];
                    //am = 255;

                    faza.array[i, j] = Complex.FromPolarCoordinates(am, a);

                }
                
            }
          
            return faza;
        }

        // Отличается от предыдущего тем, что массивы 0,1,2,3 (а не 8,9,10,11)
  
        public static ZComplexDescriptor ATAN_ar(ZArrayDescriptor[] zArrayPicture, double[] fzz, double amplit=255)
        {
            
            int w1 = zArrayPicture[0].width;
            int h1 = zArrayPicture[0].height;
            //MessageBox.Show(" ATAN_ar w1 " + w1+ "  w1 "+ h1);
            ZComplexDescriptor faza = new ZComplexDescriptor(w1, h1);

            int n_sdv = 4;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];


            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fzz[i]);
                k_cos[i] = Math.Cos(fzz[i]);
            }
            //ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture[1 * 4 + 2]);
            //ZArrayDescriptor ampf = new ZArrayDescriptor(zArrayPicture[1 * 4 + 3]);
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    i_sdv[0] = zArrayPicture[0].array[i, j];
                    i_sdv[1] = zArrayPicture[1].array[i, j];
                    i_sdv[2] = zArrayPicture[2].array[i, j];
                    i_sdv[3] = zArrayPicture[3].array[i, j];

                    double[] v_sdv = Vector_orto(i_sdv);                 // ------  Формула расшифровки фазы
                    double fz1 = Vector_Mul(v_sdv, k_sin);
                    double fz2 = Vector_Mul(v_sdv, k_cos);
                    double a = Math.Atan2(fz1, fz2);

                    double[] cos_orto = Vector_orto(k_cos);              // ------  Формула расшифровки амплитуды                   
                    double znmt = Vector_Mul(cos_orto, k_sin);
                    double am = Math.Sqrt(fz1 * fz1 + fz2 * fz2) / Math.Abs(znmt);
                    //am = am / (2 * amplit);

                    //a= ampf.array[i, j];
                    // am = amp.array[i, j];
                    //am = 255;

                    faza.array[i, j] = Complex.FromPolarCoordinates(am, a);

                }
            }

            return faza;
        }

       


      

        /// <summary>
        ///  Быстрое преобразование PSI для четырех сдвигов
        /// </summary>
        /// <param name="zArrayPicture"></param>
        /// <param name="progressBar1"></param>
        /// <param name="fzz"></param>
        /// <param name="amplit"></param>
        /// <returns></returns>
        public static ZArrayDescriptor ATAN_quick( ZArrayDescriptor[] zArrayDescriptor,  ProgressBar progressBar1, double[] fzz, double xmax, double lambda, double d, double amplit)
        {

       
            // 8, 9, 10, 11   ->    Complex[1] 

            int w1 = zArrayDescriptor[8].width;
            int h1 = zArrayDescriptor[8].height;
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
            //MessageBox.Show("width= " + w1 + "height= " + h1);

            ZComplexDescriptor zComplex_rab = new ZComplexDescriptor(w1, h1);
            /*
                        double i_sdv1;                                                   //PSI
                        double i_sdv2;
                        double i_sdv3;
                        double i_sdv4;
                        //double[] i_s = new double[w1];              // Числитель
                        //double[] i_c = new double[w1];              // Знаменатель

                        double k_sin1 = Math.Sin(fzz[0]);
                        double k_sin2 = Math.Sin(fzz[1]);
                        double k_sin3 = Math.Sin(fzz[2]);
                        double k_sin4 = Math.Sin(fzz[3]);

                        double k_cos1 = Math.Cos(fzz[0]);
                        double k_cos2 = Math.Cos(fzz[1]);
                        double k_cos3 = Math.Cos(fzz[2]);
                        double k_cos4 = Math.Cos(fzz[3]);

                        //double znmt = Math.Abs((k_sin2 - k_sin4) * k_cos1 + (k_sin3 - k_sin1) * k_cos2 + (k_sin4 - k_sin2) * k_cos3 + (k_sin1 - k_sin3) * k_cos4);
                        double znmt = Math.Abs((k_cos2 - k_cos4) * k_sin1 + (k_cos3 - k_cos1) * k_sin2 + (k_cos4 - k_cos2) * k_sin3 + (k_cos1 - k_cos3) * k_sin4);
                        znmt = znmt * (2 * amplit);        

                        progressBar1.Visible = true;
                        progressBar1.Minimum = 1;
                        progressBar1.Maximum = w1 - 1;
                        progressBar1.Value = 1;
                        progressBar1.Step = 1;

                        


                        for (int j = 0; j < h1; j++) 
                        {
                            for (int i = 0; i < w1; i++)
                            {
                                i_sdv1 = zArrayDescriptor[8].array[i, j];
                                i_sdv2 = zArrayDescriptor[9].array[i, j];
                                i_sdv3 = zArrayDescriptor[10].array[i, j];
                                i_sdv4 = zArrayDescriptor[11].array[i, j];

                                double i1 = i_sdv2 - i_sdv4;
                                double i2 = i_sdv3 - i_sdv1;
                                double i3 = i_sdv4 - i_sdv2;
                                double i4 = i_sdv1 - i_sdv3;

                                double i_s = i1 * k_sin1 + i2 * k_sin2 + i3 * k_sin3 + i4 * k_sin4;
                                double i_c = i1 * k_cos1 + i2 * k_sin2 + i3 * k_sin3 + i4 * k_sin4;

                               double faza = Math.Atan2(i_c, i_s);                            // Фаза
                               //double faza = Math.Atan2(i_s, i_c); 
                               double am = Math.Sqrt(i_s * i_s + i_c * i_c) / znmt;           // Амплитуда

                               zComplex_rab.array[i, j] = Complex.FromPolarCoordinates(am, faza);

                            }
                           progressBar1.PerformStep();                            
                         }
                        progressBar1.Value = 1;
            */
           
            zComplex_rab = ATAN_PSI.ATAN_891011(zArrayDescriptor, progressBar1, fzz, amplit);  // PSI
            //zComplex[1] = zComplex_rab;

            ZArrayDescriptor zArrayPicture = new ZArrayDescriptor(w1, h1);

            zComplex_rab = FurieN.FrenelTransformN(zComplex_rab, lambda, d, xmax);              // Преобразование Френеля с четным количеством точек

            for (int i = 0; i < w1; i++)                                                        // Амплитуду в главное окно
                for (int j = 0; j < h1; j++)
                    zArrayPicture.array[i, j] = zComplex_rab.array[i, j].Magnitude;

            zArrayPicture = Furie.Invers_Double(zArrayPicture);
            
           
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            MessageBox.Show("PSI+Frenel Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);

            //MessageBox.Show(" Lambda= " + lambda + " d= " + d + " dx= " + xmax);

            return zArrayPicture;
            }
           

        //-----------------------------------------------------------------------------------------
        //          Приведение к диапазону
        //-----------------------------------------------------------------------------------------
        public static void Diapazon(ZArrayDescriptor[] zArrayPicture, int imax)
        {
            
            int nx = zArrayPicture[8].width;
            int ny = zArrayPicture[8].height;
             //MessageBox.Show("max= " + max + "min= "+min);
            for (int k = 0; k < 4; k++)
            {
                double max = SumClass.getMax(zArrayPicture[8 + k]);
                double min = SumClass.getMin(zArrayPicture[8 + k]);
                for (int i = 0; i < nx; i++)
                    for (int j = 0; j < ny; j++)
                    {   
                        int c =(int)( (zArrayPicture[8 + k].array[i, j] - min) * imax / (max - min));
                        zArrayPicture[8 + k].array[i, j] = c;
                    }
            }
         }
        public static void Diapazon1(ZArrayDescriptor[] zArrayPicture, int regImage, int imax)
        {

            int nx = zArrayPicture[regImage].width;
            int ny = zArrayPicture[regImage].height;
            //MessageBox.Show("max= " + max + "min= "+min);

            double max = SumClass.getMax(zArrayPicture[regImage]);
            double min = SumClass.getMin(zArrayPicture[regImage]);
                for (int i = 0; i < nx; i++)
                    for (int j = 0; j < ny; j++)
                    {
                        int c = (int)((zArrayPicture[regImage].array[i, j] - min) * imax / (max - min));
                        zArrayPicture[regImage].array[i, j] = c;
                    }
            
        }

/*        public static ZArrayDescriptor Maska(ZArrayDescriptor[] zArrayPicture, int k1, int k2)
        {

            int nx = zArrayPicture[k2].width;
            int ny = zArrayPicture[k2].height;
            
            //MessageBox.Show("max= " + max + "min= "+min);
            ZArrayDescriptor res = new ZArrayDescriptor(nx, ny);
            
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    double c = zArrayPicture[k1].array[i, j];
                    double k = zArrayPicture[k2].array[i, j];
                    if (k != 0) res.array[i, j] = c;
                }
            return res;
        }
*/
        //------------------------------------------------------------------------------------------
    }
}
