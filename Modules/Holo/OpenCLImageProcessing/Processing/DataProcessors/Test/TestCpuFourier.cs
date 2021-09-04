using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using OpenTK;

namespace Processing.DataProcessors.Test
{
    // TODO: отвратительная копипаста для тестов ("эталон"), удалить когда отпадет надобность
    public static class TestCpuFourier
    {
        private static int M(int n)                                    // Определение нечетного делителя числа
        {
            int m = n;
            for (int i = 1; ; i++) { if (m % 2 == 0) m = m / 2; else break; }
            return m;
        }

        private static int L(int n)                                     // Определение четного делителя числа
        {
            int m = M(n);
            return n / m;
        }

        private static int T(int l)                                    // Определение степени l = 2**t
        {
            int m = 1;
            int nn = 2;
            for (int i = 1; ; i++) { nn = nn * 2; if (nn > l) { m = i; break; } }
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
                        double k1 = k * im * (r + s * l);
                        array_exp2[r + s * l, im] = new Complex(Math.Cos(k1), Math.Sin(k1));

                    }

                }
            }
            return array_exp2;
        }
        public static Complex[] BPF_N(Complex[] array, int n, int m, int l, int t, Complex[] array_exp, Complex[,] array_exp2, int[] array_inv)   // Одномерное преобразование БПФ с произвольным числом точек
        {

            // ----------------------------------------------------------1 - этап
            Complex[] tmpArray = new Complex[l];                                       // Промежуточный массив


            for (int h = 0; h < m; h++)
            {
                for (int g = 0; g < l; g++) tmpArray[g] = array[h + g * m];
               // tmpArray = BPF_Q(tmpArray, t, array_exp, array_inv);                 // BPF l = 2**t
                for (int g = 0; g < l; g++) array[h + g * m] = tmpArray[g];
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
                    X[rsl] = sum;
                }
            }
            return X;
        }

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
            for (int ii = 0; ii < n; ii++)
            {
                int k = 1;
                int k1 = k << (t - 1);
                int b1 = 0;
                for (int i = 1; i <= t / 2 + 1; i++)
                {
                    if ((ii & k) != 0) b1 = b1 | k1;
                    if ((ii & k1) != 0) b1 = b1 | k;
                    k = k << 1;
                    k1 = k1 >> 1;
                }
                array_inv[ii] = b1;
            }
            return array_inv;
        }

        public static Complex[] BPF_Q(Complex[] array, int powerOfTwo, Complex[] array_exp, int[] array_inv)
        {

            Complex[] resultArray = new Complex[array.Length];                      // Результирующий массив

            Complex u;
            Complex u1 = new Complex(1.0, 0.0);
            Complex w, t;

            int i, j, ip, l;

            int n = 1 << powerOfTwo;                                                          //int n = Convert.ToInt32(Math.Pow(2.0, powerOfTwo));
            for (i = 0; i < n; i++) { resultArray[array_inv[i]] = array[i]; }   // Инверсия

            for (l = 1; l <= powerOfTwo; l++)
            {
                int ll = 1 << l;                                                    //int ll = Convert.ToInt32(Math.Pow(2.0, l));
                int ll1 = ll >> 1;
                u = u1;
                w = array_exp[l];
                //w = new Complex(Math.Cos(Math.PI / ll1), Math.Sin(Math.PI / ll1));
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

            return resultArray;
        }

        public static Complex[] Split(Complex[] array)
        {
            int nx = array.Length;

            int m = M(nx);                                               // Определение нечетного делителя числа
            int l = L(nx);                                               // Определение четного делителя числа
            int t = T(l);                                                // Определение показателя степени l = 2**t

            Complex[] output = new Complex[nx];
            int o = 0;
            int[] array_inv = Invers(t, nx);

            for (int h = 0; h < m; h++)
            {
                for (int g = 0; g < l; g++) output[h * l + array_inv[g]] = array[h + g * m];
            }

            return output;
        }

        public static Complex[] SplitAndFft(Complex[] array)
        {
            int nx = array.Length;

            int m = M(nx);                                               // Определение нечетного делителя числа
            int l = L(nx);                                               // Определение четного делителя числа
            int t = T(l);                                                // Определение показателя степени l = 2**t

            Complex[] output = new Complex[nx];
            int o = 0;

            Complex[] tmpArray = new Complex[l];                                       // Промежуточный массив
            Complex[] array_exp = new Complex[t + 1];                    // Экспонента для BPF
            array_exp = P_EXP(t + 1);
            int[] array_inv = Invers(t, nx);                             // Инверсия элементов массива для БПФ

            for (int h = 0; h < m; h++)
            {
                for (int g = 0; g < l; g++) tmpArray[g] = array[h + g * m];
                tmpArray = BPF_Q(tmpArray, t, array_exp, array_inv);                 // BPF l = 2**t
                for (int g = 0; g < l; g++) output[o++] = tmpArray[g];
            }

            return output;
        }

        public static Complex[] BPF_Step1(Complex[] array)
        {
            int nx = array.Length;

            int m = M(nx);                                               // Определение нечетного делителя числа
            int l = L(nx);                                               // Определение четного делителя числа
            int t = T(l);                                                // Определение показателя степени l = 2**t

            Complex[] tmpArray = new Complex[l];                                       // Промежуточный массив
            Complex[] array_exp = new Complex[t + 1];                    // Экспонента для BPF
            array_exp = P_EXP(t + 1);
            int[] array_inv = Invers(t, nx);                             // Инверсия элементов массива для БПФ

            for (int h = 0; h < m; h++)
            {
                for (int g = 0; g < l; g++) tmpArray[g] = array[h + g * m];
                 tmpArray = BPF_Q(tmpArray, t, array_exp, array_inv);                 // BPF l = 2**t
                for (int g = 0; g < l; g++) array[h + g * m] = tmpArray[g];
            }

            return array;
        }

        public static Complex[] BPF_Step2(Complex[] step1Result)
        {
            int nx = step1Result.Length;
            int m = M(nx);                                               // Определение нечетного делителя числа
            int l = L(nx);                                               // Определение четного делителя числа
            int t = T(l);                                                // Определение показателя степени l = 2**t

            Complex sum;
            Complex[] X = new Complex[step1Result.Length];
            Complex s0 = new Complex(0.0, 0.0);
            Complex[,] array_exp2 = new Complex[nx, m];              // Экспонента для второго прохода
            array_exp2 = P_EXP2(nx, m, l);

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
                        sum += step1Result[im + rm] * array_exp2[rsl, im];
                    }
                    X[rsl] = sum;
                }
            }
            return X;
        }

        public static Vector2[] ToVector2(this Complex[] self)
        {
            return self.Select(c => new Vector2((float) c.Real, (float) c.Imaginary)).ToArray();
        }

        public static Complex[] ToComplex(this Vector2[] self)
        {
            return self.Select(v => new Complex(v.X, v.Y)).ToArray();
        }
    }
}
