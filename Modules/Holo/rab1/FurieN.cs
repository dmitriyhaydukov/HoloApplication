using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;
using ClassLibrary;

namespace rab1
{
    class FurieN
    {
        //public static Complex[] BPF_N(Complex[] array,  int powerOfTwo)
        /*
            Быстрое преобразование Фурье для произвольного N
         
         */

         private static int M(int n)                                    // Определение нечетного делителя числа
         {
             int m = n;
             for (int i = 1; ; i++) { if (m %2 == 0) m=m/2; else break; }
             return m;
         }  
 
        private static int L(int n)                                     // Определение четного делителя числа
         {
            int m = M(n);
            return n/m;
         }  

         private static int T(int l)                                    // Определение степени l = 2**t
         {
            int m = 1;
            int nn = 2;
            for (int i = 1; ; i++) { nn = nn * 2; if (nn > l) {  m = i; break; } }
            return m;
         }

         private static Complex[,] P_EXP2(int n, int m, int l)
         {
             double k = 2 * Math.PI / n;
             Complex[,] array_exp2 = new Complex[n, m];
             for (int s = 0; s < m; s++)   
                        { 
                          for (int r = 0; r < l; r++) 
                          {
                              
                               for (int im = 0; im < m; im++)
                               {
                                   double k1=k*im*(r+s*l);
                                   array_exp2[r+s*l,im] = new Complex(Math.Cos(k1), Math.Sin(k1));
 
                               }
                             
                          }
                        }
             return array_exp2;
         }

        ///           Одномерное преобразование БПФ с произвольным числом точек
        //            Complex[] array_exp   - экспоненты для обычного БПФ
        //            int[] array_inv       - инверсия для обычного БПФ
        //            Complex[,] array_exp2 - экспонента для 2 этапа БПФ с произвольным количеством точек
        //
        //
        public static Complex[] BPF_N(Complex[] array, int n, int m, int l, int t, Complex[] array_exp, Complex[,] array_exp2, int[] array_inv)   
        {
            
            // ----------------------------------------------------------1 - этап
            Complex[] tmpArray = new Complex[l];                                       // Промежуточный массив
           
                    
            for (int h = 0; h < m; h++)   
            { 
              for (int g = 0; g < l; g++)  tmpArray[g]=array[h+g*m];
                   tmpArray = BPF_Q(tmpArray, t, array_exp, array_inv);                 // BPF l = 2**t
              for (int g = 0; g<l; g++)  array[h+g*m]=tmpArray[g];
            }
        
           // ----------------------------------------------------------2 - этап 
           
           Complex sum;
           Complex[] X = new Complex[array.Length];
           Complex s0 = new Complex(0.0, 0.0); 
 
           for (int s = 0; s < m; s++)   
           {
              int sl = s * l;
              for (int r = 0; r < l; r++) 
               {
                   sum = s0;
                   int rm = r * m;
                   int rsl = r + sl;
                   for (int im = 0; im < m; im++)
                     {
                                   //double k1=k*im*(r+s*l);
                                   //w = new Complex(Math.Cos(k1), Math.Sin(k1));
                                   //w = array_exp2[r + s * l, im];
                        sum += array[im + rm] * array_exp2[rsl, im]; 
                     }
                    X[rsl]=sum;
              }
          }      
          return X;       
        }


        //----------------------------------------------------------------------------------------------
        //     Двухмерное быстрое преобразование Фурье с произвольным числом точек  (Четное число)
        //----------------------------------------------------------------------------------------------
        public static ZComplexDescriptor BPF2_Line(ZComplexDescriptor zarray)  // По строкам
        {
            int nx = zarray.width;
            int ny = zarray.height;
            // int nx = 5120;
            // int ny = 3328;
            ZComplexDescriptor resultArray = new ZComplexDescriptor(nx, ny);

            // ----------------------------------------------------------    По строкам
            if (nx % 2 != 0) nx = nx - 1;                                // n - должно быть четным

            int m = M(nx);                                               // Определение нечетного делителя числа
            int l = L(nx);                                               // Определение четного делителя числа
            int t = T(l);                                                // Определение показателя степени l = 2**t

            Complex[] array_exp = new Complex[t + 1];                    // Экспонента для BPF
            array_exp = P_EXP(t + 1);
            int[] array_inv = Invers(t, nx);                             // Инверсия элементов массива для БПФ

            Complex[] Array = new Complex[nx];                           // Выходной массив 

            int n = Convert.ToInt32(Math.Pow(2.0, t));
            if (n == nx)                                                   // N=2**t
            {
                for (int j = 0; j < ny; j++)                              // --------------------------------------------------Обычное BPF
                {
                    for (int i = 0; i < nx; i++) Array[i] = zarray.array[i, j];
                    Array = BPF_Q(Array, t, array_exp, array_inv);
                    for (int i = 0; i < nx; i++) resultArray.array[i, j] = Array[i];
                }
            }
            else                                                         // ---------------------------------------------------Для произвольного числа точек
            {
                Complex[,] array_exp2 = new Complex[nx, m];              // Экспонента для второго прохода
                array_exp2 = P_EXP2(nx, m, l);

                for (int j = 0; j < ny; j++)
                {
                    for (int i = 0; i < nx; i++) Array[i] = zarray.array[i, j];
                    Array = BPF_N(Array, nx, m, l, t, array_exp, array_exp2, array_inv);
                    for (int i = 0; i < nx; i++) resultArray.array[i, j] = Array[i];
                }
            }
            return resultArray;
        }
         //----------------------------------------------------------------------------------------------------------------------------------------

         public static ZComplexDescriptor BPF2(ZComplexDescriptor zarray)
         {
            int nx = zarray.width;
            int ny = zarray.height;
            //int nx = 5120;
            // int ny = 3328;
            ZComplexDescriptor resultArray = new ZComplexDescriptor(nx, ny);
           
            // ----------------------------------------------------------    По строкам
            if (nx % 2 != 0) nx = nx - 1;                                // n - должно быть четным

            int m = M(nx);                                               // Определение нечетного делителя числа
            int l = L(nx);                                               // Определение четного делителя числа
            int t = T(l);                                                // Определение показателя степени l = 2**t
           
            Complex[] array_exp = new Complex[t + 1];                    // Экспонента для BPF
            array_exp = P_EXP(t + 1);
            int[] array_inv = Invers(t, nx);                             // Инверсия элементов массива для БПФ
           
            Complex[] Array = new Complex[nx];                           // Выходной массив 
        
            int n = Convert.ToInt32(Math.Pow(2.0, t));
            if (n==nx)                                                   // N=2**t
            {  
               for (int j = 0; j < ny; j++)                              // --------------------------------------------------Обычное BPF
                  {
                    for (int i = 0; i < nx; i++) Array[i] = zarray.array[i, j];
                        Array = BPF_Q(Array, t, array_exp, array_inv);
                    for (int i = 0; i < nx; i++) resultArray.array[i, j] = Array[i];  
                   }
            }
           else                                                         // ---------------------------------------------------Для произвольного числа точек
            {
               Complex[,] array_exp2 = new Complex[nx, m];              // Экспонента для второго прохода
               array_exp2 = P_EXP2(nx, m, l);
                
               for (int j = 0; j < ny; j++)
                  {
                    for (int i = 0; i < nx; i++) Array[i] = zarray.array[i, j];
                         Array = BPF_N(Array, nx, m, l, t, array_exp, array_exp2, array_inv);      
                    for (int i = 0; i < nx; i++) resultArray.array[i, j] = Array[i]; 
                  }           
            }

            // ----------------------------------------------------------    По столбцам
            if (ny % 2 != 0) ny = ny - 1;                            // n - должно быть четным
            m = M(ny);                                               // Определение нечетного делителя числа
            l = L(ny);                                               // Определение четного делителя числа
            t = T(l);                                                // Определение показателя степени l = 2**t
            Complex[] array_exp1 = new Complex[t + 1];   
           
            array_exp1 = P_EXP(t + 1);
            int[] array_inv1 = Invers(t, ny);  

            Complex[] Array1 = new Complex[ny];  
 
            n = Convert.ToInt32(Math.Pow(2.0, t));
            if (n==ny)                                                   // N=2**t
            {
               for (int i = 0; i < nx; i++)                              // Обычное BPF
                  {
                    for (int j = 0; j < ny; j++) Array1[j] = resultArray.array[i, j];
                       Array1 = BPF_Q(Array1, t, array_exp1, array_inv1);
                    for (int j = 0; j < ny; j++) resultArray.array[i, j] = Array1[j];  
                   }
            }
           else
            {
             Complex[,] array_exp2 = new Complex[ny, m];  
             array_exp2 = P_EXP2(ny, m, l);
             for (int i = 0; i < nx; i++)
              {             
                for (int j = 0; j < ny; j++) Array1[j] = resultArray.array[i, j];
                     Array1 = BPF_N(Array1, ny, m, l, t, array_exp1, array_exp2, array_inv1);
                for (int j = 0; j < ny; j++) resultArray.array[i, j] = Array1[j];
              }
            }

        return resultArray;
      }

        // -----------------------------------------------------------------------------------------
        //    Быстрое  преобразование Фурье с экономией массивов
        // -----------------------------------------------------------------------------------------
        private static Complex[] P_EXP(int powerOfTwo)                  // Экспоненты для бабочки
        {
            Complex[] A = new Complex[powerOfTwo + 1];
             for (int l = 1; l <= powerOfTwo; l++)
             {
                 int ll = Convert.ToInt32(Math.Pow(2.0, l));
                 int ll1 = ll >> 1;
                 A[l] = new Complex(Math.Cos(Math.PI / ll1), Math.Sin(Math.PI / ll1)); ;
             }
             return A;
        }
        /*
         private static Complex[,] P_EXP(int powerOfTwo) 
         {
             Complex w, u;
             //Complex[] array_exp = new Complex[powerOfTwo + 1];
             Complex[] array_exp =P_EXP1( powerOfTwo);
             int n2 = Convert.ToInt32(Math.Pow(2.0, powerOfTwo));
             Complex[,] B = new Complex[powerOfTwo + 1, (n2>>1) + 1];

             for (int l = 1; l <= powerOfTwo; l++)
             {
                 int ll = Convert.ToInt32(Math.Pow(2.0, l));
                 int ll1 = ll >> 1;
                 u = new Complex(1.0, 0.0);
                 w = array_exp[l];
                
                 for (int j = 1; j <= ll1; j++)
                 {
                     u = u * w;
                     B[l,j]=u;
                 }
             }
            return B;
         }
*/

        private static int[] Invers(int t, int n)                  // Инверсия элементов
        {
            int[] array_inv = new int[n];   
            for (int ii=0; ii < n; ii++)
             {
                int k = 1;
                int k1 = k << (t - 1);
                int b1 = 0;
                   for (int i = 1; i <= t/2+1; i++ )
                     {
                       if ((ii & k)!=0)    b1 = b1 | k1;
                       if ((ii & k1)!=0)   b1 = b1 | k;
                       k  = k << 1;
                       k1 = k1 >> 1;
                     }
                   array_inv[ii]= b1;
             }
            return array_inv;
        }

        public static Complex[] BPF_Q(Complex[] array, int powerOfTwo, Complex[] array_exp, int[] array_inv)
        {

            Complex[] resultArray = new Complex[array.Length];                      // Результирующий массив

            Complex u ;
            Complex u1 = new Complex(1.0, 0.0);
            Complex w, t;

            int i, j, ip, l;

            int n = 1 << powerOfTwo;                                                          //int n = Convert.ToInt32(Math.Pow(2.0, powerOfTwo));
            for ( i = 0; i < n ; i++) {  resultArray[array_inv[i]] = array[i];  }   // Инверсия

            for ( l = 1; l <= powerOfTwo; l++)
            {
                int ll = 1 << l;                                                    //int ll = Convert.ToInt32(Math.Pow(2.0, l));
                int ll1 = ll >> 1;
                u = u1;
                w = array_exp[l];
                //w = new Complex(Math.Cos(Math.PI / ll1), Math.Sin(Math.PI / ll1));
                for ( j = 1; j <= ll1; j++)
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

            return resultArray;
        }
        /// <summary>
        /// BPF от реального массива по строкам
        /// </summary>
        /// <param name="zarray"></param>
        /// <param name="sdvig"></param>   Сдвиг 
        /// <returns></returns>
        public static ZComplexDescriptor BPF_Real(ZArrayDescriptor zarray, int sdvig)
        {
            int nx = zarray.width;
            int ny = zarray.height;

            int m = 1;
            int nn = 2;
            for (int i = 1; ; i++) { nn = nn * 2; if (nn > nx) { m = i; break; } }
            int n = Convert.ToInt32(Math.Pow(2.0, m));                                  // N=2**m


            // MessageBox.Show("nx: " + nx  + "   m: " + m + "   n: " + n);

            //Complex[] array_exp = P_EXP(m + 1);                             // Экспонента для BPF
            //int[] array_inv = Invers(m, nx);                                // Инверсия элементов массива для БПФ

            Complex[] Array = new Complex[n];                                // Выходной массив 
            Complex[] ArrayX = new Complex[n];                               // Комплексный массив экспоненты сдвига 

            for (int i = 0; i < n; i++)
               {
                double a = 2 * Math.PI * sdvig * i / n;
                ArrayX[i] = new Complex(Math.Cos(a), -Math.Sin(a));
               }
            
            ZComplexDescriptor resultArray = new ZComplexDescriptor(n, ny);  // Re=zArrayPicture Im=0
            for (int j = 0; j < ny; j++)                             
             {
                for (int i = 0; i < n; i++) { Array[i] = new Complex(zarray.array[i, j], 0.0); }
                for (int i = 0; i < n; i++) { Array[i] = Array[i]* ArrayX[i]; }
                //Array = BPF_Q(Array, m, array_exp, array_inv);
                Array = Furie.GetFourierTransform(Array, m);
                for (int i = 0; i < n; i++) resultArray.array[i, j] = Array[i];
             }   

            return resultArray;

        }

        public static ZComplexDescriptor Inverse_BPF(int k2, int sdvig, int DX)
        {
            int nx = 4096;
            int ny = 200;

            int m = 1;
            int nn = 2;
            for (int i = 1; ; i++) { nn = nn * 2; if (nn > nx) { m = i; break; } }
            int n = Convert.ToInt32(Math.Pow(2.0, m));                                  // N=2**m


             MessageBox.Show("nx: " + nx  + "   m: " + m );

            Complex[] Array = new Complex[nx];
            for (int i = 0; i < DX; i++) { Array[i] = new Complex(1, 0); }
            for (int i = nx - DX; i < nx; i++) { Array[i] = new Complex(1, 0); }

            for (int i = 0; i < DX; i++) { Array[i] = new Complex(DX - i, 0); }
            for (int i = nx - DX, k = 0; i < nx; i++, k++) { Array[i] = new Complex(k, 0); }
            for (int i = 0; i < DX; i++) { Array[i] = Array[i] / DX; }
            for (int i = nx - DX; i < nx; i++) { Array[i] = Array[i] / DX; }

        
            Array = Furie.GetInverseFourierTransform(Array, m);

            ZComplexDescriptor resultArray = new ZComplexDescriptor(nx, ny);  // Re=zArrayPicture Im=0
            for (int j = 0; j < ny; j++)
            {
                for (int i = 0; i < nx; i++) resultArray.array[i, j] = Array[i];
            }

            return resultArray;
        }
/// <summary>
/// 1)В области Фурье сдвиг на -sdvig в центр, 
/// 2)вырезаем область частот от DX до -DX по треугольнику, 
/// 3)обратное преобразование Фурье
/// </summary>
/// <param name="sdvig"></param>
/// <param name="DX"></param>
/// <returns></returns>
        public static ZComplexDescriptor Inverse1_BPF(int sdvig, int DX)
        {
            //if (Form1.zComplex[k1] == null) { MessageBox.Show("FurieN zComplex[k2] == NULL"); return null; }
           

            //int nx = Form1.zArrayPicture.width;
            //if (nx % 2 != 0) nx = nx - 1;                            // n - должно быть четным
            //int ny = 50;
            //nx = 4874;

            int nx = 4096;
            int ny = 200;
            int m = 1;
            int nn = 2;
            for (int i = 1; ; i++) { nn = nn * 2; if (nn > nx) { m = i; break; } }
            int n = Convert.ToInt32(Math.Pow(2.0, m));                                  // N=2**m

            Complex[] ArrayX = new Complex[nx];                               // Комплексный массив экспоненты сдвига 
            Complex[] Array = new Complex[nx];
            Complex[] ArrayY = new Complex[nx];

           
            ZComplexDescriptor resultArray = new ZComplexDescriptor(nx, ny);  // Re=zArrayPicture Im=0

            for (int i = 0; i < nx; i++)
            {
                double a = 2 * Math.PI * sdvig * i / nx;
                ArrayX[i] = new Complex(Math.Cos(a), -Math.Sin(a));
            }

            for (int j = 0; j < ny; j++)                                        // Преобразование Фурье с сдвигом в центр
            {
                for (int i = 0; i < nx; i++) { Array[i] = new Complex(Form1.zArrayPicture.array[i, j], 0.0); }
                for (int i = 0; i < nx; i++) { Array[i] = Array[i] * ArrayX[i]; }
                Array = Furie.GetFourierTransform(Array, m);
                for (int i = 0; i < nx; i++) { resultArray.array[i, j] = Array[i]; }
            }
          
            for (int i = 0; i < DX; i++)                    { ArrayY[i] = new Complex(DX-i, 0); }
            for (int i = nx - DX,  k = 0; i < nx; i++, k++) { ArrayY[i] = new Complex(k, 0);    }
            for (int i = 0; i < DX; i++)                    { ArrayY[i] = ArrayY[i] / DX;       }
            for (int i = nx - DX; i < nx; i++)              { ArrayY[i] = ArrayY[i] / DX;       }

            
            for (int j = 0; j < ny; j++)
            {
                for (int i = 0; i < nx; i++) { Array[i] = resultArray.array[i, j] * ArrayY[i]; }
                Array = Furie.GetInverseFourierTransform(Array, m);
                for (int i = 0; i < nx; i++) resultArray.array[i, j] = Array[i];
            }

            return resultArray;
        }
        public static ZComplexDescriptor Inverse2_BPF(int sdvig, int DX)
        {
            //if (Form1.zComplex[k1] == null) { MessageBox.Show("FurieN zComplex[k2] == NULL"); return null; }


            //int nx = Form1.zArrayPicture.width;
            //if (nx % 2 != 0) nx = nx - 1;                            // n - должно быть четным
            //int ny = 50;
            //nx = 4874;

            int nx = 4096;
            int ny = 200;
            int m = 1;
            int nn = 2;
            for (int i = 1; ; i++) { nn = nn * 2; if (nn > nx) { m = i; break; } }
            int n = Convert.ToInt32(Math.Pow(2.0, m));                                  // N=2**m

            Complex[] ArrayX = new Complex[nx];                               // Комплексный массив экспоненты сдвига 
            Complex[] Array = new Complex[nx];
            


            ZComplexDescriptor resultArray = new ZComplexDescriptor(nx, ny);  // Re=zArrayPicture Im=0

           // for (int i = 0; i < nx; i++)
           // {
           //     double a = 2 * Math.PI * sdvig * i / nx;
           //     ArrayX[i] = new Complex(Math.Cos(a), -Math.Sin(a));
           // }

            for (int j = 0; j < ny; j++)                                        // Преобразование Фурье
            {
                for (int i = 0; i < nx; i++) { Array[i] = new Complex(Form1.zArrayPicture.array[i, j], 0.0); }
                //for (int i = 0; i < nx; i++) { Array[i] = Array[i] * ArrayX[i]; }
                Array = Furie.GetFourierTransform(Array, m);
                for (int i = 0; i < nx; i++) { resultArray.array[i, j] = Array[i]; }
            }

            Complex[] ArrayY = new Complex[nx];
            for (int i = sdvig, j = 0; i < sdvig + DX; i++, j++) { ArrayY[i] = new Complex(DX - j, DX - j); }
            for (int i = sdvig, j = 0; i > sdvig - DX; i--, j++) { ArrayY[i] = new Complex(DX - j, DX - j); }

            //for (int i = sdvig, j = 0; i < sdvig + DX; i++, j++) { ArrayY[nx-i] = new Complex(DX - j, DX - j); }
            //for (int i = sdvig, j = 0; i > sdvig - DX; i--, j++) { ArrayY[nx-i] = new Complex(DX - j, DX - j); }

            //for (int i = nx - DX, k = 0; i < nx; i++, k++) { ArrayY[i] = new Complex(k, 0); }
            //for (int i = 0; i < DX; i++) { ArrayY[i] = ArrayY[i] / DX; }
            //for (int i = nx - DX; i < nx; i++) { ArrayY[i] = ArrayY[i] / DX; }

            //for (int i = 4004; i < 4028; i++) { ArrayY[i] = new Complex(1, 0); }
            //for (int i = nx - DX; i < nx; i++) { Array[i] = new Complex(1, 0); }

            for (int j = 0; j < ny; j++)
            {
                for (int i = 0; i < nx; i++) { Array[i] = resultArray.array[i, j] * ArrayY[i]; }
                //for (int i = 0; i < nx; i++) { Array[i] = resultArray.array[i, j] ; }
                Array = Furie.GetInverseFourierTransform(Array, m);
                for (int i = 0; i < nx; i++) resultArray.array[i, j] = Array[i];
            }

            return resultArray;
        }
        //----------------------------------------------------------------------------------------------
        //                     a =    i/(lambda*d) *exp(-i2pid/lambda)
        //                    
        //----------------------------------------------------------------------------------------------
        //                          Коэффициенты для преобразования Френеля
        //----------------------------------------------------------------------------------------------

        public static double[] fexp1(double lambda, double d, int nx, double dx)  // Экспоненета перед интегралом
        {

            Complex[] Array = new Complex[nx];
            double[] phase = new double[nx];
            double[] phase1 = new double[nx];

            double deltax = dx / nx;                                           //  Размер одного пикселя

            double d_equls = nx * deltax * deltax / lambda;
            double gamma = lambda * d / (nx * deltax * deltax);
            double x_max = dx / gamma;
            double deltaxx = x_max / nx;

        
            double b = Math.PI * deltax * deltax / (lambda * d);
            int n2 = nx / 2;

            for (int i = 0; i < nx; i++)
            {
               
                int i1 = i - n2;
                phase[i] = -b * i1 * i1;
            }
         
            return phase;
        }
        public static Complex[] fexp2(double lambda, double d, int nx, double dx)
        {

            Complex[] Array = new Complex[nx];
            Complex[] Array1 = new Complex[nx];
            double deltax = dx / nx;

            double b = Math.PI * deltax * deltax / (d * lambda);
            int n2 = nx / 2;
            for (int i = 0; i < nx; i++)
            {
                int i1 = i - n2;
                double x = b * i1 * i1;
                Array[i] = new Complex(Math.Cos(x), Math.Sin(x));
            }

            return Array;
        }
  
        //----------------------------------------------------------------------------------------------
        //     Двухмерное быстрое преобразование Френеля
        //----------------------------------------------------------------------------------------------

        public static ZComplexDescriptor FrenelTransformN(ZComplexDescriptor zarray, double lambda, double d, double dx)
        {
            int nx = zarray.width;
            int ny = zarray.height;
            //if (nx != ny) { MessageBox.Show("Frenel nx != ny"); }
            ZComplexDescriptor resultArray = new ZComplexDescriptor(nx, ny);

          
            Complex[] Array_с2x = fexp2(lambda, d, nx, dx);
            Complex[] Array_с2y = fexp2(lambda, d, ny, dx);

          
            
            for (int j = 0; j < ny; j++)
            { 
                for (int i = 0; i < nx; i++) resultArray.array[i, j] = zarray.array[i, j] * Array_с2x[i] * Array_с2y[j];
               
            }
           // Array_с2 = fexp2(lambda, d, ny, dx);

           // for (int i = 0; i < nx; i++)
          //     for (int j = 0; j < ny; j++) resultArray.array[i, j] = resultArray.array[i, j] * Array_с2[j];
          
            resultArray = BPF2(resultArray);                                                    // Преобразование Фурье с произвольным количеством точек

            double[] phase_y = fexp1(lambda, d, ny, dx);
            double[] phase_x = fexp1(lambda, d, nx, dx);
   

            for (int i = 0; i < nx; i++)
            { 
              for (int j = 0; j < ny; j++)
                { resultArray.array[i, j] = Complex.FromPolarCoordinates(resultArray.array[i, j].Magnitude, resultArray.array[i, j].Phase + phase_y[j] + phase_x[i]); }
             
            }
          //  phase = fexp1(lambda, d, nx, dx);

          //  for (int j = 0; j < ny; j++)
          //      for (int i = 0; i < nx; i++)
          //          { resultArray.array[i, j] = Complex.FromPolarCoordinates(resultArray.array[i, j].Magnitude, resultArray.array[i, j].Phase + phase[i]); }
          
            return resultArray;
        }



    }
}
