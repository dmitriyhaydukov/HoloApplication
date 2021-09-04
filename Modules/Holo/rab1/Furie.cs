using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;
using ClassLibrary;


//    Complex[,] cmpl = new Complex[NX, NY];
//    cmpl[i, j]= new Complex (100.0,0.0); 
//    complex1.Magnitude  | a + bi | = Math.Sqrt(a * a + b * b)
//    c1.Phase                         Math.Atan2(b, a).
//    value.Real, value.Imaginary
//    Complex c1 = Complex.FromPolarCoordinates(10, 45 * Math.PI / 180);  Задание комплексного числа в полярных координатах

namespace rab1
{
    class Furie
    {
        
        public static double[,] Amplituda(Complex[,] array)
        {
            int nx = array.GetLength(0);
            int ny = array.GetLength(1);
            double[,] resultArray = new double[nx, ny];
             for (int i = 0; i<nx; i++)
                 for (int j = 0; j < ny; j++)
                     resultArray[i, j] = array[i,j].Magnitude;
           
            return resultArray;
        }


        public static ZArrayDescriptor zAmplituda(ZComplexDescriptor zComplex)
        {
            int nx  = zComplex.width;
            int ny  = zComplex.height;
            ZArrayDescriptor amp1 = new ZArrayDescriptor(nx, ny); 
            
            
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    amp1.array[i, j] = zComplex.array[i, j].Magnitude;

            return amp1;
        }

        public static double[,] Faza(Complex[,] array)
        {
            int nx = array.GetLength(0);
            int ny = array.GetLength(1);
            double[,] resultArray = new double[nx, ny];
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    resultArray[i, j] = array[i, j].Phase;

            return resultArray;
        }
        public static double[,] Re(Complex[,] array)
        {
            int nx = array.GetLength(0);
            int ny = array.GetLength(1);
            double[,] resultArray = new double[nx, ny];
          
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    resultArray[i, j] = array[i, j].Real;

            return resultArray;
        }

        public static ZArrayDescriptor Re(ZComplexDescriptor zComplex)
        {
            int nx = zComplex.width;
            int ny = zComplex.height;
            ZArrayDescriptor amp1 = new ZArrayDescriptor(nx, ny); 

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    amp1.array[i, j] = zComplex.array[i, j].Real;

            return amp1;
        }

        public static double[,] Im(Complex[,] array)
        {
            int nx = array.GetLength(0);
            int ny = array.GetLength(1);
            double[,] resultArray = new double[nx, ny];
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    resultArray[i, j] = array[i, j].Imaginary;

            return resultArray;
        }

        public static ZArrayDescriptor Im(ZComplexDescriptor zComplex)
        {
            int nx = zComplex.width;
            int ny = zComplex.height;
            ZArrayDescriptor amp1 = new ZArrayDescriptor(nx, ny); 
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    amp1.array[i, j] = zComplex.array[i, j].Imaginary;

            return amp1;
        }

        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        public static int PowerOfTwo(int n)
         {
            int m = 1;
            int nn = 2;
            for (int i = 1; ; i++) { nn = nn * 2; if (nn > n) { n = nn / 2; m = i; break; } }
            return m;
         }




        //Быстрое преобразование Фурье
        public static Complex[] GetFourierTransform(Complex[] array, int powerOfTwo)
        {
           // if (!MathHelper.IsPowerOfTwo(array.Length))
           // {
           //     throw new FourierTransformException();
           // }
           // int powerOfTwo = MathHelper.GetNextHighestPowerOfTwo(array.Length);
           

            Complex[] resultArray = new Complex[array.Length];
            array.CopyTo(resultArray, 0);
            Complex u, w, t;

            int i, j, ip, k, l;
            int n = Convert.ToInt32(Math.Pow(2.0, powerOfTwo));
            int n1 = n >> 1;

            for (i = 0, j = 0, k = n1; i < n - 1; i++, j = j + k)
            {
                if (i < j)
                {
                    t = resultArray[j];
                    resultArray[j] = resultArray[i];
                    resultArray[i] = t;
                }
                k = n1;
                while (k <= j)
                {
                    j = j - k;
                    k = k >> 1;
                }
            }

            for (l = 1; l <= powerOfTwo; l++)
            {
                int ll = Convert.ToInt32(Math.Pow(2.0, l));
                int ll1 = ll >> 1;
                u = new Complex(1.0, 0.0);
                w = new Complex(Math.Cos(Math.PI / ll1), Math.Sin(Math.PI / ll1));
                for (j = 1; j <= ll1; j++)
                {
                    for (i = j - 1; i < n; i = i + ll)
                    {
                        ip = i + ll1;
                        t = resultArray[ip] * u;
                        resultArray[ip] = resultArray[i] - t;
                        resultArray[i] = resultArray[i] + t;
                    }
                    u = u * w;
                }
            }

           for (i = 0; i < n; i++)
            {
                resultArray[i] = resultArray[i] / Math.Sqrt(n);
            }
            return resultArray;
        }
        //----------------------------------------------------------------------------------------------
        //Быстрое обратное преобразование Фурье
        public static Complex[] GetInverseFourierTransform(Complex[] array, int powerOfTwo)
        {
            //if (!MathHelper.IsPowerOfTwo(array.Length))
            //{
             //   throw new FourierTransformException();
            //}
            //int powerOfTwo = MathHelper.GetNextHighestPowerOfTwo(array.Length);
            //int powerOfTwo = 8;
            Complex[] resultArray = new Complex[array.Length];
            array.CopyTo(resultArray, 0);
            Complex u, w, t;

            int i, j, ip, k, l;
            int n = Convert.ToInt32(Math.Pow(2.0, powerOfTwo));
            int n1 = n >> 1;

            for (i = 0, j = 0, k = n1; i < n - 1; i++, j = j + k)
            {
                if (i < j)
                {
                    t = resultArray[j];
                    resultArray[j] = resultArray[i];
                    resultArray[i] = t;
                }
                k = n1;
                while (k <= j)
                {
                    j = j - k;
                    k = k >> 1;
                }
            }

            for (l = 1; l <= powerOfTwo; l++)
            {
                int ll = Convert.ToInt32(Math.Pow(2.0, l));
                int ll1 = ll >> 1;
                u = new Complex(1.0, 0.0);
                w = new Complex(Math.Cos(Math.PI / ll1), Math.Sin(-Math.PI / ll1));
                for (j = 1; j <= ll1; j++)
                {
                    for (i = j - 1; i < n; i = i + ll)
                    {
                        ip = i + ll1;
                        t = resultArray[ip] * u;
                        resultArray[ip] = resultArray[i] - t;
                        resultArray[i] = resultArray[i] + t;
                    }
                    u = u * w;
                }
            }

            for (i = 0; i < n; i++)
            {
                resultArray[i] = resultArray[i] / Math.Sqrt(n);
            }

            return resultArray;
        }

        //----------------------------------------------------------------------------------------------
        //     Двухмерное быстрое преобразование Фурье
        //----------------------------------------------------------------------------------------------
        public static ZComplexDescriptor FourierTransform(ZComplexDescriptor zarray, int powerOfTwo)
        {
            int nx = zarray.width;
            int ny = zarray.height;
            Complex[] Array = new Complex[nx];
            Complex[] Array1 = new Complex[nx];
            ZComplexDescriptor resultArray = new ZComplexDescriptor(nx, ny);

            //for (int i = 0; i < nx; i++) Array[i] = new Complex(0.0, 0.0);

            //Complex[] Array_exp = new Complex[nx];
            
            //for (int i = 0; i < nx; i++) Array_exp[i] = new Complex(Math.Cos(i *  Math.PI ), -Math.Sin(i *  Math.PI ));
            
                        for (int j=0; j<ny; j++)
                        {
                            //for (int i = 0; i < nx; i++) Array[i] = zarray.array[i, j] * Array_exp[i];
                            for (int i = 0; i < nx; i++) Array[i] = zarray.array[i, j] ;
                            Array1 = GetFourierTransform(Array, powerOfTwo);
                            for (int i = 0; i < nx; i++) resultArray.array[i, j] = Array1[i];
                        }

                    
              for (int i = 0; i < nx; i++)
                 {
                     //for (int j = 0; j < ny; j++) Array[j] = resultArray.array[i, j]*Array_exp[j];
                     for (int j = 0; j < ny; j++) Array[j] = resultArray.array[i, j] ;
                     Array1 = GetFourierTransform(Array, powerOfTwo);
                     for (int j = 0; j < ny; j++) resultArray.array[i, j] = Array1[j];
                 }
                   
              return resultArray;
        }

        public static ZComplexDescriptor InverseFourierTransform(ZComplexDescriptor zarray, int powerOfTwo)
        {
            int nx = zarray.width;
            int ny = zarray.height;
            Complex[] Array = new Complex[nx];
            Complex[] Array1 = new Complex[nx];
            ZComplexDescriptor resultArray = new ZComplexDescriptor(nx, ny);

           

            for (int j = 0; j < ny; j++)
            {
                for (int i = 0; i < nx; i++) Array[i] = zarray.array[i, j];
                Array1 = GetInverseFourierTransform(Array, powerOfTwo);
                for (int i = 0; i < nx; i++) resultArray.array[i, j] = Array1[i];
                
            }


            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++) Array[j] = resultArray.array[i, j];
                Array1 = GetInverseFourierTransform(Array, powerOfTwo);
                for (int j = 0; j < ny; j++) resultArray.array[i, j] = Array1[j];
               
            }
           
            return resultArray;
        }

        //----------------------------------------------------------------------------------------------
        //                          Экспоненты  exp(ix)=cos(x)+isin(x)
        //----------------------------------------------------------------------------------------------
        //                     a =    i/(lambda*d) *exp(-i2pid/lambda)
        //                    
        //----------------------------------------------------------------------------------------------
        //                          Коэффициенты для преобразования Френеля
        //----------------------------------------------------------------------------------------------

        public static double[]  exp1(double lambda, double d, int nx, double dx)  // Экспоненета перед интегралом
        {
          
         Complex[] Array = new Complex[nx];
         double[] phase = new double[nx];
         double[] phase1 = new double[nx];

          double deltax = dx / nx;                                           //  Размер одного пикселя
          
          double d_equls = nx*deltax*deltax/lambda;
          double gamma = lambda * d / (nx * deltax * deltax);
          double x_max = dx / gamma;
          double deltaxx =  x_max/nx ;

         // MessageBox.Show(" dx = " + dx + " deltas = " + deltax + " d_equls = " + d_equls + "  gamma = " + gamma + " X_max = " + x_max + " deltax = " + deltaxx);
          //gamma = 1;
          //Complex a1 = new Complex(0, 1.0 / (lambda * d));                   //   Фаза -1.57
          //double  x1 = (2.0 * Math.PI * d / lambda + a1.Phase);              //  1 256 637 -1.57

        
          //Complex  c1 = Complex.FromPolarCoordinates(a1.Magnitude, a1.Phase + x1);
          //double b = Math.PI * d * lambda / (nx * nx * deltax * deltax);   //  0.001571
         // double b = Math.PI * lambda * d / (dx*dx ); 
          double b = Math.PI * deltax * deltax  / (lambda * d);
          int n2 = nx / 2;  

            for (int i=0; i<nx; i++)
            {     
                //Array[i] = Complex.FromPolarCoordinates(c1.Magnitude, c1.Phase + b * i*i); 
                //phase[i] = Array[i].Phase;
                //phase[i] = -(x1 + b * i * i);
                int i1 = i - n2;
                phase[i] =  -b * i1 * i1;
            }
/*
            double max = phase[0];
            double min = phase[0];
            for (int i = 0; i < nx/2; i++)
            {
                if (max < phase[i]) max = phase[i]; if (min > phase[i]) min = phase[i];

            }


            double gamma = Math.Sqrt(nx * deltax * deltax / (lambda * d));
            MessageBox.Show("x1 = " + x1 +"min = " + min + " max = " + max + " gamma = " + gamma);
          
*/

           
            
           // for (int i = 0; i < n2; i++) { phase1[i + n2 ] = phase[i]; phase1[n2-i-1] = phase[i]; }  // 
            //for (int i = 0; i < n2; i++) { phase1[i] = phase[i + n2]; phase1[i + n2] = phase[i]; }  // Циклический сдвиг
            return phase;
        }
        public static Complex[] exp2(double lambda, double d, int nx, double dx)
        {
            
            Complex[] Array = new Complex[nx];
            Complex[] Array1 = new Complex[nx];
            double deltax = dx / nx;  

            double b = Math.PI * deltax * deltax / (d * lambda );
            int n2 = nx / 2;  
            for (int i = 0; i < nx; i++)
            {
                int i1 = i - n2;
                double x = b * i1 * i1 ;
                Array[i] = new Complex(Math.Cos(x), Math.Sin(x));
            }
          
            //for (int i = 0; i < n2; i++) { Array1[i + n2] = Array[i]; Array1[n2 - i - 1] = Array[i]; }  // Циклический сдвиг
            return Array;
        }
        //----------------------------------------------------------------------------------------------
        //     Двухмерное быстрое преобразование Френеля
        //----------------------------------------------------------------------------------------------
        public static ZComplexDescriptor FrenelTransform(ZComplexDescriptor zarray, int powerOfTwo, double lambda, double d, double dx)
        {
            int nx = zarray.width;
            int ny = zarray.height;
            if (nx != ny) { MessageBox.Show("Frenel nx != ny"); }
            
            Complex[] Array    = new Complex[nx];
            
            double[] phase = exp1(lambda, d, nx, dx);
            Complex[] Array_с2 = exp2(lambda, d, nx, dx);
            ZComplexDescriptor resultArray = new ZComplexDescriptor(nx, ny);

            //Array_с1 = exp1(lambda, d, nx, dx);
            //Array_с2 = exp2(lambda, d, nx, dx);

            for (int j = 0; j < ny; j++)
            {
                for (int i = 0; i < nx; i++) Array[i] = zarray.array[i, j] * Array_с2[i];
                Array = GetFourierTransform(Array, powerOfTwo);
                for (int i = 0; i < nx; i++) resultArray.array[i, j] = Array[i]  ;
            }


            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++) Array[j] = resultArray.array[i, j] * Array_с2[j];
                Array = GetFourierTransform(Array, powerOfTwo);
                for (int j = 0; j < ny; j++) resultArray.array[i, j] = Array[j];
            }

             for (int i = 0; i < nx; i++)
                 for (int j = 0; j < ny; j++)
                   {  resultArray.array[i, j] = Complex.FromPolarCoordinates(resultArray.array[i, j].Magnitude, resultArray.array[i, j].Phase + phase[j]); }
            for (int j = 0; j < ny; j++) 
                for (int i = 0; i < nx; i++)
                   {  resultArray.array[i, j] = Complex.FromPolarCoordinates(resultArray.array[i, j].Magnitude, resultArray.array[i, j].Phase + phase[i]); }

            /*                  

                                             for (int i = 0; i < nx; i++)
                                                 for (int j = 0; j < ny; j++)
                                                   { resultArray.array[i, j] = new Complex( phase[j], 0);  }

                                             for (int j = 0; j < ny; j++)
                                                 for (int i = 0; i < nx; i++)
                                                  { resultArray.array[i, j] = resultArray.array[i, j].Real + phase[i]; }
           */
            // for (int i = 0; i < nx; i++) for (int j = 0; j < ny; j++) resultArray.array[i, j] =  Array_с1[j];
           // for (int j = 0; j < ny; j++) for (int i = 0; i < nx; i++) resultArray.array[i, j] = resultArray.array[i, j] * Array_с1[i];
            
            return resultArray;
        }

        //----------------------------------------------------------------------------------------------
        //                           Циклический сдвиг    Смещение изображения в центр
        //----------------------------------------------------------------------------------------------
        public static ZComplexDescriptor Invers(ZComplexDescriptor zarray )
        {
            int nx = zarray.width;
            int ny = zarray.height;
            if (nx != ny) { MessageBox.Show("Invers nx != ny"); }

            Complex[] Array = new Complex[nx];
            Complex[] Array1 = new Complex[nx];
           
            ZComplexDescriptor resultArray = new ZComplexDescriptor(nx, ny);
             int n2=nx/2;
           

            for (int j = 0; j < ny; j++)
            {
                for (int i = 0; i < nx; i++) Array[i] = zarray.array[i, j];
                for (int i = 0; i < n2; i++) { Array1[i] = Array[i+n2]; Array1[i+n2]=Array[i];}
                for (int i = 0; i < nx; i++) resultArray.array[i, j] = Array1[i];

            }


            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++) Array[j] = resultArray.array[i, j] ;
                for (int j = 0; j < n2; j++) { Array1[j] = Array[j+n2]; Array1[j+n2]=Array[j];}
                for (int j = 0; j < ny; j++) resultArray.array[i, j] = Array1[j];

            }
         
            return resultArray;
        }

        public static ZArrayDescriptor Invers_Double(ZArrayDescriptor zarray)
        {
            int nx = zarray.width;
            int ny = zarray.height;
            //if (nx != ny) { MessageBox.Show("Invers nx != ny"); }

            double[] Array = new double[nx];
            double[] Array1 = new double[nx];

            //ZArrayDescriptor resultArray = new ZArrayDescriptor(nx, ny);
            ZArrayDescriptor resultArray = new ZArrayDescriptor(zarray);
            int n2 = nx / 2;
           
/*
       for (int i = 0; i < ny; i++)
            {
                for (int j = 0; j < nx; j++) 
                    resultArray.array[i, j] = zarray.array[i, j];
             
            } 
*/
            
            for (int j = 0; j < ny; j++)
            {
                for (int i = 0; i < nx; i++) Array[i] = zarray.array[i, j];
                for (int i = 0; i < n2; i++) { Array1[i] = Array[i + n2]; Array1[i + n2] = Array[i]; }
                for (int i = 0; i < nx; i++) resultArray.array[i, j] = Array1[i];

            }

            n2 = ny / 2;
        
            Array = new double[ny];
           Array1 = new double[ny];
       
            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++) Array[j] = resultArray.array[i, j];
                for (int j = 0; j < n2; j++) { Array1[j] = Array[j + n2]; Array1[j + n2] = Array[j]; }
                //for (int j = 0; j < n2; j++) { Array1[j] = Array[j ];  Array1[j + n2] = Array[j + n2]; }
                for (int j = 0; j < ny; j++) resultArray.array[i, j] = Array1[j];

            }

            return resultArray;
        }
//-------------------------------------------------------------------------------------------------------------------------
       



      

        public static ZArrayDescriptor ATAN_am(ZArrayDescriptor[] zArrayPicture,  double[] fzz)
        {
            // 8, 9, 10, 11   ->    Complex[1] 

            int w1 = zArrayPicture[0].width;
            int h1 = zArrayPicture[0].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            int n_sdv = 4;                                                       // Число фазовых сдвигов

            double[] i_sdv = new double[4];
            double[] v_sdv = new double[4];                                  // Вектор коэффициентов
            double[] k_sin = new double[4];
            double[] k_cos = new double[4];


            for (int i = 0; i < n_sdv; i++)
            {
                k_sin[i] = Math.Sin(fzz[i]);
                k_cos[i] = Math.Cos(fzz[i]);
            }


            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    i_sdv[0] = zArrayPicture[0].array[i, j];
                    i_sdv[1] = zArrayPicture[1].array[i, j];
                    i_sdv[2] = zArrayPicture[2].array[i, j];
                    i_sdv[3] = zArrayPicture[3].array[i, j];

                    // ------                                     Формула расшифровки

                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];

                    for (int ii = 1; ii < n_sdv - 1; ii++) { v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1]; }

                    double fz1 = 0;
                    double fz2 = 0;

                    for (int ii = 0; ii < n_sdv; ii++)
                    {
                        fz1 += v_sdv[ii] * k_sin[ii];
                        fz2 += v_sdv[ii] * k_cos[ii];
                    }


                    faza.array[i, j] = Math.Atan2(fz1, fz2);   
                }
            }

            return faza;
        }




        public static ZComplexDescriptor ATAN_OLD4(ZArrayDescriptor[] zArrayPicture, double[] fzz)
        {
            // 8, 9, 10, 11   ->    Complex[1] 

            int w1 = zArrayPicture[8].width;
            int h1 = zArrayPicture[8].height;

            ZComplexDescriptor faza = new ZComplexDescriptor(w1, h1);

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    double i1 = zArrayPicture[8].array[i, j];    //   0
                    double i2 = zArrayPicture[9].array[i, j];    //  90
                    double i3 = zArrayPicture[10].array[i, j];   // 180
                    double i4 = zArrayPicture[11].array[i, j];   // 270

                    // ------                                     Формула расшифровки                  

                    faza.array[i, j] = new Complex(i1-i2, i2-i4);
                }
            }

            return faza;
        }


    }
}
