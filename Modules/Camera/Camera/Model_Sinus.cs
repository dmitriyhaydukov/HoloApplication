using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using ClassLibrary;

namespace Camera
{
    class Model_Sinus
    {


        public static ZComplexDescriptor Exponenta(double g, int N)
        {
            ZComplexDescriptor cmpl = new ZComplexDescriptor(N, 256);

            double kx = 2 * Math.PI*g / N;
            double am = 1;

            for (int i=0; i<N; i++)
                for (int j=0; j<256; j++)
                {
                    double fz = -kx * i;
                    cmpl.array[i,j] = Complex.FromPolarCoordinates(am, fz);

                }
           

            return cmpl;
        }

        // 4 синусоиды с фазовым сдвигом

        public static ZArrayDescriptor Sinus(double fz, double a, double n_polos,  double gamma, int kr, int Nx, int Ny, double noise)
        {
            int kr1=kr+1;
           
            int NX = Nx * kr1;
            int NY = Ny * kr1;
            //MessageBox.Show(" nx " + NX + " a " + a + " noise " + noise);

            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт
           
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
           // Random rnd = new Random();
           
            double kx = 2.0 * Math.PI / NX;
           
            // a = (a - min) * 2.0 * Math.PI / (max - min);   -pi +pi

           
            for (int i = 0; i < NX; i+=kr1)
              for (int j = 0; j < NY; j++)
                {
                   
                    double fz1 = a * (Math.Sin(kx * i * n_polos - fz) + 1) / 2;
                    //double fz1 = a * (Math.Sin(2.0 * Math.PI * i  / n_polos + fz) + 1) / 2;
                    double fa = (0.5 - rnd.NextDouble()) * a * noise;   //rnd.NextDouble() 0-1  
                  
                    fz1 = fz1 + fa;
                   
                    cmpl.array[i, j] =  Math.Pow(fz1, gamma);

                }

            return cmpl;
        }
        /// <summary>
        /// Моделирование синусоидальной картины с заданным в точках размером полос 
        /// </summary>
        /// <param name="fz"></param>           фазовый сдвиг полос
        /// <param name="a"></param>            амплитуда
        /// <param name="n_polos"></param>      размер полосы в точках
        /// <param name="gamma"></param>        гамма искажения        
        /// <param name="Nx"></param>           размер по X
        /// <param name="Ny"></param>           размер по Y
        /// <param name="noise"></param>        шум в долях амплитуды (0,1)*a
        /// <returns></returns>
        public static ZArrayDescriptor Sinus1
        (
            double fz,
            double a,
            double n_polos,
            double gamma,
            int kr,
            int Nx,
            int Ny,
            double noise,
            double minIntensity,
            double[] clinArray = null
        )
        {
            int kr1 = kr + 1;

            int NX = Nx * kr1;
            int NY = Ny * kr1;
            //MessageBox.Show(" n_polos " + n_polos);

            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт

            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            // Random rnd = new Random();

            //double kx = 2.0 * Math.PI / NX;

            // a = (a - min) * 2.0 * Math.PI / (max - min);   -pi +pi

            double a1 = a - minIntensity; 
            
            double[] sn = new double[NX];

            for (int i = 0; i < NX; i += kr1)
            {
                double v = minIntensity + a1 * (Math.Sin(2.0 * Math.PI * i / n_polos - fz) + 1.0) / 2.0;          // синусоида от 0 до 1
                sn[i] = CorrectBr.CorrectValueByClin(v, clinArray, 255);
            }
          
           // double max = double.MinValue;
           //  double min = double.MaxValue;

            for (int j = 0; j < NY; j++)
              for (int i = 0; i < NX; i += kr1)
                {
                    double fz1 = sn[i];                                                             // синусоида от 0 до 1
                    double fa = (0.5 - rnd.NextDouble()) * a * noise;                               //rnd.NextDouble() 0-1  
                    double s = fz1 + fa;
                    //double s = Math.Pow(fz1, gamma);                                                // Гамма искажения
                    //double s = fz1;
                    //if (s > max) max = s;  if (s < min) min = s;
                    cmpl.array[i, j] = s;
                }
            //double maxmin = max - min;

            //for (int j = 0; j < NY; j++)
            //    for (int i = 0; i < NX; i += kr1)
            //    {
            //        cmpl.array[i, j] = (cmpl.array[i, j] - min) * a / maxmin;
            //    }


            return cmpl;
        }

        /// <summary>
        /// Моделирование синусоидальной картины с заданным в точках размером полос 
        /// c фазовыми сдвинами от 0 до 256 с числом повторений в kr
        /// </summary>
      
      
        /// <param name="n_polos"></param>      размер полосы в точках
       
        /// <param name="kr"></param>           разрядка нулями для моделирования сверхразрешения (0 - нет разрядки)
        /// <param name="Nx"></param>           размер по X
        
        /// <returns></returns>
        public static ZArrayDescriptor Sinus2(double n_polos, int kr, int Nx)
        {

            int kr1 = kr + 1;
            int NX = Nx;
            int NY = 0;
            int j = 0;
            double a = 256;        // Амплитуда
            double fz = 0;
            double Pi3 = 3 * Math.PI;
            double Pi2 = 2 * Math.PI;
            //MessageBox.Show(" n_polos " + n_polos);

            double[] sn = new double[NX];
            for (fz = 0; fz <= Pi3; fz = fz + Pi2 / 256) { NY++; }

            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY+1);      // Результирующий фронт 

            for (fz = 0, j=0; fz <= Pi3; fz=fz+Pi2/256, j=j+1)
            {
                for (int i = 0; i < NX; i++) { sn[i] = a * (Math.Sin(2.0 * Math.PI * i / n_polos + fz) + 1.0) / 2.0; }        // синусоида от 0 до 1 
                for (int i = 0; i < NX; i++) { cmpl.array[i, j] = sn[i]; }
                //for (int k = 0; k < kr; k++)
                //     {  }
                //for (int i = 0; i < NX; i++) { cmpl.array[i, j + kr] = 0; }

            }
            
            return cmpl;
        }


        public static ZArrayDescriptor Sinus3(double n_polos, int Nx, int Ny)
        {

            int NX = Nx*2;
            double a = 256;        // Амплитуда
           
            //MessageBox.Show(" n_polos " + n_polos);

            double[] sn  = new double[Nx];
            double[] sn2 = new double[NX];

            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, Ny);      // Результирующий фронт 

            
           for (int i = 0; i < NX; i++)   { sn2[i] = a * (Math.Sin(2.0 * Math.PI * i / n_polos) + 1.0) / 2.0; }
           
          
            for (int j = 0; j < Ny; j++)
               { for (int i = 0; i < NX; i+=2) { cmpl.array[i, j] = (sn2[i] + sn2[i + 1]) / 2; ; } }

          
            for (int j = 0; j < Ny; j++)
               { for (int i = 1; i < NX-1; i+=2) { cmpl.array[i, j] = (sn2[i] + sn2[i + 1]) / 2; } }

            return cmpl;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Моделирование клина интенсивности
        /// </summary>
        /// <param name="nu"></param> Число уровней интенсивности
        /// <param name="Nx"></param> Не используется
        /// <param name="Ny"></param> Не используется
        /// <returns></returns>
        /// 
        public static ZArrayDescriptor Intens(int nu1, int nu2, int dx, ZArrayDescriptor zArray)  // Прямоугольник с цветом nu1 в начале и nu2 в конце
        {

            int Nx = zArray.width;
            int Ny = zArray.height;

            for (int j = 0; j < Ny; j++) 
                for (int i = 0; i <dx; i++)
                    zArray.array[i, j] = nu1;

            for (int j = 0; j < Ny; j++)
                for (int i = Nx-dx; i < Nx; i++)
                    zArray.array[i, j] = nu2;

            return zArray;
        }
        //cmpl.array[i, j] = Math.Pow(fz1, gamma);
        /// <summary>
        /// Моделирование клина от I0 до nu-1
        /// </summary>
        /// <param name="nu"></param>    Число уровней интенсивности I0 - 255
        /// <param name="I0"></param>    Пьедистал
        /// <param name="Nx"></param>
        /// <param name="Ny"></param>
        /// <param name="dx"></param>  Полосы по краям
        /// <param name="gamma"></param> Гамма
        /// <returns></returns>
        public static ZArrayDescriptor Intensity1(double nu,  int Nx, int Ny, int dx, double gamma)  // от светлого к темному
        {
           
            int Nx1 = Nx;
            Nx = Nx + dx * 2;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(Nx, Ny);

            int k = (int)(Nx1 / (nu + 1));

            double[] ag = new double[Nx1];

            double max = double.MinValue;
            double min = double.MaxValue;

            for (int i = 0; i < Nx1; i++)
             {
                double a = Math.Pow(i / k, gamma);
                ag[i] = a;
                if (a > max) max = a;
                if (a < min) min = a;
             }

            for (int i = 0; i < Nx1; i++) ag[i] = (ag[i] - min) * nu / (max - min);


            for (int j = 0; j < Ny; j++)
                for (int i = 0; i < Nx1; i++)
                    cmpl.array[i + dx, j] =ag[i];

            cmpl = Intens(255, 0, dx, cmpl);     // Белая и черная полоса по краям
                                    
            return cmpl;
        }
        public static ZArrayDescriptor Intensity2(double nu, int Nx, int Ny, int dx, double gamma) // От черного к белому
        {
            int Nx1 = Nx;
            Nx = Nx + dx * 2;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(Nx, Ny);

            int k = (int)(Nx1 / (nu + 1));

            double[] ag = new double[Nx1];

            double max = double.MinValue;
            double min = double.MaxValue;

            for (int i = 0; i < Nx1; i++)
            {
                double a = Math.Pow(255 - i / k, gamma);
                ag[i] = a;
                if (a > max) max = a;
                if (a < min) min = a;
            }

            for (int i = 0; i < Nx1; i++) ag[i] = (ag[i] - min) * nu / (max - min);


            for (int j = 0; j < Ny; j++)
                for (int i = 0; i < Nx1; i++)
                    cmpl.array[i + dx, j] = ag[i];

            cmpl = Intens(0, 255, dx, cmpl);     // Белая и черная полоса по краям

            return cmpl;
        }
        /*
        public static ZArrayDescriptor Intensity3(double nu, int Nx, int Ny)  // от светлово к темному
        {
            Nx = 4096;
            Ny = 2048;

            int dx = 100;
            int Nx1 = Nx;
            Nx = Nx + dx * 2;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(Nx, Ny);

            int k = (int)(Nx1 / (nu + 1));
            int k1 = 0;
            int kk = 0;
            int ii = 0;

            
          
             for (int j = 0; j < Ny; j++, k1++)
               {
                 if (k1 == 8) { kk = kk + 8; k1 = 0; }
                 for (int i = 0; i < Nx - dx * 2; i++)
                       {
                         ii = i + dx + kk;
                         if (ii < Nx - dx * 2)   cmpl.array[ii, j] = i / k ; 
                      }
                }
            cmpl = Intens(255, 0, dx, cmpl);
            return cmpl;
        }
      
     
       public static ZArrayDescriptor Intensity4(double nu, int Nx, int Ny)
       {
           Nx = 4296;
           Ny = 2048;
           ZArrayDescriptor cmpl = new ZArrayDescriptor(Nx, Ny);
           int n = (int)(nu + 1);
           int ky = Ny / n;

           //int i1 = 0;
           for (int i = 0; i < Nx; i++)
           {
               // for (int ints = 0; ints < k; ints++)
               for (int j = 0; j < Ny; j++)
               {
                   cmpl.array[i, j] = (Ny-j) / ky;
               }
           }
           return cmpl;
       }
      
     
        /// <summary>
        /// -----------------------------   Вертикальные полосы
        /// </summary>
        /// <param name="nl"></param>       Период полос в точках
        /// <param name="zArray"></param>   Массив в который вносятся изменения
        /// <returns></returns>
        public static ZArrayDescriptor Intensity_Line(int nl, ZArrayDescriptor zArray)
        {
            if (zArray == null) { MessageBox.Show("Intensity_Line zArray == NULL"); return null; }
            int Nx = zArray.width;
            int Ny = zArray.height;

            for (int i = 0; i < Nx; i+=nl)
            {
               
                for (int j = 0; j < Ny; j++)
                {
                    double z = zArray.array[i, j];
                    if (z < 128) zArray.array[i, j] = 255; else zArray.array[i, j] = 0;


                }
            }
            return zArray;
        }

        public static ZArrayDescriptor Intensity_Line1(int nl, ZArrayDescriptor zArray)
        {
            if (zArray == null) { MessageBox.Show("Intensity_Line zArray == NULL"); return null; }
            int Nx = zArray.width;
            int Ny = zArray.height;

            for (int i = 0; i < Nx; i ++)
            {

                for (int j = 0; j < Ny; j += nl)
                {
                    double z = zArray.array[i, j];
                    if (z < 128) zArray.array[i, j] = 255; else zArray.array[i, j] = 0;


                }
            }
            return zArray;
        }
         */
        //----------------------------------------------------------------------------------------------------------Dithering

        public static ZArrayDescriptor Dithering(double fz, double n_polos, int N_kvant, int N_urovn)
        {

            int NX = 1024;
            int NY = 1024;

            if (N_kvant < 2 && N_kvant > 256) { MessageBox.Show("Dithering  N_kvant"); return null; }

         
            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт
       
            double kx = 2.0 * Math.PI / NX;
        
            //double.MinValue

            for (int i = 0; i < NX; i++)                                     // Приведение к числу уровней         
                for (int j = 0; j < NY; j++)
                {
                    double fz1 = (Math.Sin(kx * i * n_polos + fz) + 1) / 2;              
                   
                    int ifz1 = (int)( fz1 * N_urovn)+1;                     
                    if (ifz1 > N_urovn) ifz1 = N_urovn;                      // Чтобы избежать sin=1

                    cmpl.array[i, j] = ifz1-1;
                }


            
            for (int i = 0; i < NX; i++)                                    // Приведение к числу квантов
            {
                for (int j = 0; j < NY; j++)
                {

                    int ifz0 = (int)cmpl.array[i, j];
                    int ifz1 = Kvant(cmpl.array[i, j], N_kvant, N_urovn);

                    cmpl.array[i, j] = ifz1;
                    double err = ifz0 - ifz1;

                    int j1 = j + 1;
                    if (j1 < NY)  { cmpl.array[i, j1] = cmpl.array[i, j1] + err * 7 / 16;  }
                    int i1 = i + 1;
                    int j0 = j - 1;
                    if (i1 < NX)
                    {
                        if (j1 < NY) cmpl.array[i1, j1] = cmpl.array[i1, j1] + err * 3 / 16;
                        cmpl.array[i1, j] = cmpl.array[i1, j] + err * 5 / 16;
                        if (j0 >= 0) cmpl.array[i1, j0] = cmpl.array[i1, j0] + err * 1 / 16;
                    }
                }
                i++;
                for (int j = NY-1; j >= 0; j--)
                {

                    int ifz0 = (int)cmpl.array[i, j];
                    int ifz1 = Kvant(cmpl.array[i, j], N_kvant, N_urovn);

                    cmpl.array[i, j] = ifz1;
                    double err = ifz0 - ifz1;

                    int j0 = j - 1;
                    if (j0 >= 0) { cmpl.array[i, j0] = cmpl.array[i, j0] + err * 7 / 16;  }
                    int i1 = i + 1;
                    int j1 = j + 1;
                    if (i1 < NX)
                    {
                        if (j0 >= 0) cmpl.array[i1, j0] = cmpl.array[i1, j0] + err * 3 / 16;
                        cmpl.array[i1, j] = cmpl.array[i1, j] + err * 5 / 16;
                        if (j1 < NY) cmpl.array[i1, j1] = cmpl.array[i1, j1] + err * 1 / 16;
                    }
                }

            }  

            return cmpl;
        }


       
        public static int Kvant(double a, int N_kvant, int N_urovn)
        {
            double fz1 = a / N_urovn;
            int ifz1 = (int)(fz1 * N_kvant) + 1;
            if (ifz1 > N_kvant) ifz1 = N_kvant;

            fz1 = (ifz1 - 1);
            ifz1 = (int)fz1 * N_urovn / (N_kvant - 1);
            return ifz1;
        }
//-----------------------------------------------------------------------------------------------------------------------------------
        public static ZArrayDescriptor DitheringVZ(double fz, double n_polos, int N_kvant, int N_urovn)  // Матрица возбуждения
        {

            int NX = 1024;
            int NY = 1024;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт
            double kx = 2.0 * Math.PI / NX;
             for (int i = 0; i < NX; i++)                                     // Приведение к числу уровней         
                for (int j = 0; j < NY; j++)
                {
                    double fz1 = (Math.Sin(kx * i * n_polos + fz) + 1) / 2;              
                   
                    int ifz1 = (int)( fz1 * N_urovn)+1;                     
                    if (ifz1 > N_urovn) ifz1 = N_urovn;                      // Чтобы избежать sin=1

                    cmpl.array[i, j] = ifz1-1;
                }

             int n = 8;// N_urovn / 32;
            double[,] D = new double[8, 8];
            D = Model_VZ(8);

            for (int x = 0; x < NX; x++)                                     // Приведение к числу уровней         
                for (int y = 0; y < NY; y++)
                {
                    int i = (x % n) ;
                    int j = (y % n) ;
                    if (cmpl.array[x, y]/4 < D[i, j]) cmpl.array[x, y] = 0; else cmpl.array[x, y] = 255;                  
                }
            return cmpl;

        }
        public static double[,] Model_VZ(int n)   // Генерация матрицы возбуждения
        {
            double[,] array = new double[n, n];
            if (n == 2)
            {
                array[0, 0] = 0; array[0, 1] = 2;
                array[1, 0] = 3; array[1, 1] = 1;
                return array;
            }
            int n2 = n / 2;
            double[,] array1 = new double[n2, n2];
            array1 = Model_VZ(n2);
            for (int i = 0; i < n2; i++)
            {
                for (int j = 0; j < n2; j++) array[i, j] = 4 * array1[i, j];
                for (int j = n2; j < n; j++) array[i, j] = 4 * array1[i, j - n2] + 2;
            }
            for (int i = n2; i < n; i++)
            {
                for (int j = 0; j < n2; j++) array[i, j] = 4 * array1[i - n2, j] + 3;
                for (int j = n2; j < n; j++) array[i, j] = 4 * array1[i - n2, j - n2] + 1;
            }

            return array;
        }

        //---------------------------------------------------------------------------------------------------------------WB

        public static ZArrayDescriptor WB(double fz, double n_polos)
        {

            int NX = 1024;
            int NY = 1024;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт

            double a = 255;
            double kx = 2.0 * Math.PI / NX;

            // a = (a - min) * 2.0 * Math.PI / (max - min);   -pi +pi


            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double fz1 = Math.Sin(kx * i * n_polos + fz) ;
                    if (fz1 < 0) a = 0; else a = 255;
                    cmpl.array[i, j] = a;
                }

            return cmpl;
        }
//------------------------------------------------------------------------------------------------------------------------
     //Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
    
    //                double fa = (0.5- rnd.NextDouble() )* Math.PI * noise;   //rnd.NextDouble() 0-1    


        public static ZArrayDescriptor Model_FAZA(double n, double noise)
        {
            int NX = 1024;
            int NY = 1024;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт       
           
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
           // Random rnd = new Random();
            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    
                    double  fz1 = (i % n);
                    fz1 = fz1 * 2*Math.PI / (n ) ;
                    double fa = (0.5 - rnd.NextDouble()) * fz1 * noise;   //rnd.NextDouble() 0-1  
                    fz1 = fz1 + fa;
                    //if (fz1 > 2 * Math.PI) fz1 = 2 * Math.PI;  //fz1 - 2 * Math.PI;
                    //if (fz1 < 0)           fz1 = 0;            // + 2 * Math.PI;
                    cmpl.array[i, j] = fz1;
                }
          
            return cmpl;
        }
        public static ZArrayDescriptor Model_FAZA_SUB(ZArrayDescriptor z2, ZArrayDescriptor z3)
        {
            int NX = z2.width;
            int NY = z2.height;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт       

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double fz1 = (z2.array[i, j] - z3.array[i, j]);
                    if (fz1 < 0 )           fz1 = fz1 + 2 * Math.PI;
                    //if (fz1 > 2 * Math.PI ) fz1 = fz1 - 2 * Math.PI;
                    cmpl.array[i, j] = fz1;
                }

            return cmpl;
        }

        public static ZArrayDescriptor Model_FAZA_SUBN(ZArrayDescriptor z2, ZArrayDescriptor z3, double noise)
        {
            int NX  = z2.width;
            int NY = z2.height;

            
            double n0 = -Math.PI * noise/3;
            double n1 = 2 * Math.PI +  Math.PI * noise/3;


            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт       

            for (int i = 0; i < NX; i++)
            {
                for (int j = 0; j < NY; j++)
                {
                    double fz1 = (z2.array[i, j] - z3.array[i, j]);
                    if (fz1 < n0) fz1 = fz1 + 2 * Math.PI;
                    if (fz1 > n1)  fz1 = fz1 - n1;
                    cmpl.array[i, j] = fz1;
                }
              
            }
            return cmpl;
        }

        public static ZArrayDescriptor Model_FAZA_SUBA(ZArrayDescriptor z2, ZArrayDescriptor z3)
        {
            int NX = z2.width;
            int NY = z2.height;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт       

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double fz1 = Math.Abs((z2.array[i, j] - z3.array[i, j]));
                    //if (fz1 < 0) fz1 = fz1 + 180;
                    cmpl.array[i, j] = fz1;
                }
            double[,] a = new double[8, 8];
            a = Model_VZ(8);
            for (int i = 0; i < 8; i++)
            {
                MessageBox.Show(" "+a[i,0]+" "+a[i,1]+" "+a[i,2]+" "+a[i,3]+" "+a[i,4]+" "+a[i,5]+" "+a[i,6]+" "+a[i,7]);
            }

            return cmpl;
        }


        // Вычитание двух профилей

        public static ZArrayDescriptor Model_FAZA_T(ZArrayDescriptor z1, ZArrayDescriptor z2, double n123, double n23)
        {
            int NX = z2.width;
            int NY = z2.height;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт  
            //double n123 = n12 * n23 / (Math.Abs(n12 - n23));
            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double rzn = z1.array[i, j] * n123 - z2.array[i, j] * n23;
                    int k = Convert.ToInt32(rzn / (Math.PI * n23));  //Convert.ToInt32
                    cmpl.array[i, j] = k * Math.PI * n23;
                }

         
            return cmpl;
        }
        // Сложение с минимальным

        public static ZArrayDescriptor Model_FAZA_SUM(ZArrayDescriptor z1, ZArrayDescriptor z2, double n1)
        {
            int NX = z2.width;
            int NY = z2.height;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт  
            
            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {

                    cmpl.array[i, j] = z1.array[i, j] + z2.array[i, j]*n1;
                }


            return cmpl;
        }
        
        public static ZArrayDescriptor Model_FAZA_T1(ZArrayDescriptor z2, double n1, double n2)
        {
            int NX = z2.width;
            int NY = z2.height;

            double n12 = n1 * n2 / (Math.Abs(n1 - n2));


            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт       

            //for (int i = 0; i < NX; i++) for (int j = 0; j < 400; j++) cmpl.array[i, j] = z2.array[i, j];

            double[] b = new double[NX];
            //for (int i = 0; i < NX; i++) { b[i] = z2.array[i, 350] + z2.array[i, 250]; }  // Абсолютная фаза
            //for (int i = 0; i < NX; i++) for (int j = 0; j < 100; j++)   cmpl.array[i, j]       = z2.array[i, j] * n1;
            //for (int i = 0; i < NX; i++) for (int j = 0; j < 100; j++)   cmpl.array[i, j+100]   = z2.array[i, j+100] * n2;
            //  Для первой длины волны
            for (int i = 0; i < NX; i++) for (int j = 0; j < 100; j++)   cmpl.array[i, j] = z2.array[i, j + 300] * n12  - z2.array[i, j] * n1; //+ z2.array[i, j + 100] * n2;
            
            for (int i = 0; i < NX; i++) for (int j = 0; j < 100; j++)
                {
                    int k = Convert.ToInt32(cmpl.array[i, j] / (Math.PI * n1));  //Convert.ToInt32
                    cmpl.array[i, j + 100] =  k * Math.PI * n1;
                }



            for (int i = 0; i < NX; i++) for (int j = 0; j < 100; j++) cmpl.array[i, j + 200] = z2.array[i, j] * n1 + cmpl.array[i, j + 100]; //+ z2.array[i, j + 100] * n2;

            //  Для второй длины волны
            for (int i = 0; i < NX; i++) for (int j = 0; j < 100; j++) cmpl.array[i, j+300] = z2.array[i, j + 300] * n12  - z2.array[i, j+100] * n2; 
            for (int i = 0; i < NX; i++) for (int j = 0; j < 100; j++)
                {
                    int k = Convert.ToInt32(cmpl.array[i, j+300] / ( Math.PI * n2));  //Convert.ToInt32 с округлением
                    cmpl.array[i, j + 400] =  k * Math.PI * n2;
                }
            for (int i = 0; i < NX; i++) for (int j = 0; j < 100; j++) cmpl.array[i, j + 500] = z2.array[i, j+100] * n2 + cmpl.array[i, j + 400]; //+ z2.array[i, j + 100] * n2;
            //  Сумма
            for (int i = 0; i < NX; i++) for (int j = 0; j < 100; j++) cmpl.array[i, j + 600] = (cmpl.array[i, j + 500] + cmpl.array[i, j + 200]) / 2;
            return cmpl;
        }

      
    }
}
