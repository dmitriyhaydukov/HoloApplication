using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using rab1;
using System.Numerics;
using System.Threading;
using ClassLibrary;


namespace rab1
{
    class Model_interf
    {
        // dx  микрометрах
        // Углы в радианах

        public static ZComplexDescriptor Model_pl(double am, double AngleX, double AngleY, double Lambda, int NX, int NY, double dx)
        {
            ZComplexDescriptor cmpl  = new ZComplexDescriptor(NX, NY);       // Первый фронт перпендикулярный к плоскости наблюдения
            ZComplexDescriptor cmpl1 = new ZComplexDescriptor(NX, NY);      // Второй фронт

            double kx = dx / NX;
            double ky = dx  / NY;
            


            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j ++)
                {                                      
                    cmpl.array[i, j]  = new Complex(am, 0.0);
                    cmpl1.array[i, j] = Complex.FromPolarCoordinates(am, 2 * Math.PI * (Math.Sin(AngleX) * kx * i + Math.Sin(AngleY) * ky * j) / Lambda);
                }

            for (int i = 0; i < NX; i ++)
                for (int j = 0; j < NY; j ++)
                {
                    cmpl.array[i, j] += cmpl1.array[i, j];
                }

            return cmpl;
        }

        public static ZComplexDescriptor Model_Noise(double noise, ZComplexDescriptor cmpl0)
        {
            //MessageBox.Show("noise = " + noise);
            int NX = cmpl0.width;
            int NY = cmpl0.height;
      
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);      // Результирующий фронт
           
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double am = cmpl0.array[i, j].Magnitude;
                    double fz = cmpl0.array[i, j].Phase;
                    double fa = (rnd.NextDouble() * 2.0 * Math.PI - Math.PI) * noise;
                    Complex a = Complex.FromPolarCoordinates(am,  fz + fa );
                    cmpl.array[i, j] =  a;
                }

            return cmpl;
        }

        public static ZComplexDescriptor Model_pl_ADD(double am, ZComplexDescriptor cmpl0, 
                                                      double AngleX, double AngleY, double Lambda, double dx, double noise)
        {

            int NX = cmpl0.width;
            int NY = cmpl0.height;
           
            //ZComplexDescriptor cmpl1 = new ZComplexDescriptor(NX, NY);    // Фронт под углом AngleX,  AngleY
            ZComplexDescriptor cmpl  = new ZComplexDescriptor(NX, NY);      // Результирующий фронт
            double kx = dx  / NX;
            double ky = dx  / NY;
           
            // a = (a - min) * 2.0 * Math.PI / (max - min);   -pi +pi

            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double fz = 2 * Math.PI * (Math.Sin(AngleX) * kx * i + Math.Sin(AngleY) * ky * j) / Lambda;
                    double fa = rnd.NextDouble() * 2.0 * Math.PI - Math.PI;
                    Complex a = Complex.FromPolarCoordinates(am, fz + fa * noise );
                    cmpl.array[i, j] = cmpl0.array[i, j] +  a;
                }

            return cmpl;
        }


        public static ZComplexDescriptor Model_pl_ADD_Interf(double am, ZComplexDescriptor cmpl0,
                                                           double AngleX, double AngleY, double Lambda, double dx, double noise)
        {
            MessageBox.Show("am = " + am);
            int NX = cmpl0.width;
            int NY = cmpl0.height;

            //ZComplexDescriptor cmpl1 = new ZComplexDescriptor(NX, NY);      // Фронт под углом AngleX,  AngleY
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);             // Результирующая интенсивность
            double kx = dx / NX;
            double ky = dx / NY;

            // a = (a - min) * 2.0 * Math.PI / (max - min);   -pi +pi
            //am = SumClass.getAverage(cmpl0);  // Средняя интенсивность комплексного массива

            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double fz = 2 * Math.PI * (Math.Sin(AngleX) * kx * i + Math.Sin(AngleY) * ky * j) / Lambda;
                    double fa = rnd.NextDouble() * 2.0 * Math.PI - Math.PI;
                    double f0 = fz + fa * noise ;
                    //Complex Pl = Complex.FromPolarCoordinates(am, f0);

                    //cmpl.array[i, j] = cmpl0.array[i, j] + Pl;
                    double a = cmpl0.array[i, j].Magnitude;
                    double f = cmpl0.array[i, j].Phase;
                    a = a * a + am * am + 2 * a * am * Math.Cos(f - f0);
                    cmpl.array[i, j] = Complex.FromPolarCoordinates(a, f);
                }
            //cmpl = SumClass.Range_Array1(cmpl, am);      // Приведение к диапазону от 0 до am
            return cmpl;
        }
        public static ZComplexDescriptor Model_pl_SUB(double am, ZComplexDescriptor cmpl0, double AngleX, double AngleY, double Lambda, double dx)
        {

            int NX = cmpl0.width;
            int NY = cmpl0.height;
      
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);      // Результирующий фронт
            double kx = dx / NX;
            double ky = dx / NY;


            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double f1 = cmpl0.array[i, j].Phase;
                    double f2 = 2 * Math.PI * (Math.Sin(AngleX) * kx * i + Math.Sin(AngleY) * ky * j) / Lambda;
                    double am1 = cmpl0.array[i, j].Magnitude;
                    Complex a = Complex.FromPolarCoordinates(am1, f1-f2);
                    cmpl.array[i, j] =  a;
                }

            return cmpl;
        }
        //
        //              Устранение наклона из фазы
        //
        public static ZArrayDescriptor Model_pl_SUB1(ZArrayDescriptor cmpl0, double AngleX, double AngleY)
        {

            int NX = cmpl0.width;
            int NY = cmpl0.height;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);
            double tangens_x = Math.Tan(AngleX);
            //double tangens_y = Math.Tan(2 * AngleY);

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {                   
                    cmpl.array[i, j] = cmpl0.array[i, j] + tangens_x * i;
                }

            return cmpl;
        }


        // Сложение с плоской волной --------------------------------------------------------------------

        public static ZComplexDescriptor Model_pl_ADD_PSI(double am, ZComplexDescriptor cmpl0,
                       double AngleX, double AngleY, double Lambda, double dx, double noise, double fzr)
        {

            int NX = cmpl0.width;
            int NY = cmpl0.height;

            //ZComplexDescriptor cmpl1 = new ZComplexDescriptor(NX, NY);     // Фронт под углом AngleX,  AngleY
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);        // Результирующий фронт
            double kx = dx / NX;
            double ky = dx / NY;

            am = SumClass.getAverage(cmpl0);                                 // Среднее значение
            // a = (a - min) * 2.0 * Math.PI / (max - min);   -pi +pi

            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double fz = 2 * Math.PI * (Math.Sin(AngleX) * kx * i + Math.Sin(AngleY) * ky * j) / Lambda;
                    double fa = rnd.NextDouble() * 2.0 * Math.PI - Math.PI;
                    double f0 = fz + fa * noise + fzr;
                    Complex Pl = Complex.FromPolarCoordinates(am, f0);

                    cmpl.array[i, j] = cmpl0.array[i, j] + Pl;
                }

            return cmpl;
        }

        // Сложение с плоской волной => интенсивность в центральное окно

        public static ZArrayDescriptor Model_pl_ADD_I(double am, ZComplexDescriptor cmpl0,
                       double AngleX, double AngleY, double Lambda, double dx, double noise, double fzr)
        {
            MessageBox.Show("am = " + am);
            int NX = cmpl0.width;
            int NY = cmpl0.height;

            //ZComplexDescriptor cmpl1 = new ZComplexDescriptor(NX, NY);      // Фронт под углом AngleX,  AngleY
            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);             // Результирующая интенсивность
            double kx = dx / NX;
            double ky = dx / NY;

            // a = (a - min) * 2.0 * Math.PI / (max - min);   -pi +pi
            //am = SumClass.getAverage(cmpl0);  // Средняя интенсивность комплексного массива

            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double fz = 2 * Math.PI * (Math.Sin(AngleX) * kx * i + Math.Sin(AngleY) * ky * j) / Lambda;
                    double fa = rnd.NextDouble() * 2.0 * Math.PI - Math.PI;
                    double f0 = fz + fa * noise + fzr;
                    //Complex Pl = Complex.FromPolarCoordinates(am, f0);

                    //cmpl.array[i, j] = cmpl0.array[i, j] + Pl;
                    double a = cmpl0.array[i, j].Magnitude;
                    double f = cmpl0.array[i, j].Phase;
                    cmpl.array[i, j] =a*a + am*am + 2*a*am*Math.Cos(f-f0);
                }
            cmpl = SumClass.Range_Array1(cmpl, am);      // Приведение к диапазону от 0 до am
            return cmpl;
        }

        //
        //                     Добавление фазового сдвига  fz и получение голограммы
        //
        public static ZArrayDescriptor Model_pl_PSI(double am, ZComplexDescriptor cmpl0,
                                double AngleX, double AngleY, double Lambda, double dx, double noise, double fz)
        {
            int NX = cmpl0.width;
            int NY = cmpl0.height;
            double kx = dx / NX;
            double ky = dx / NY;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт

            am = SumClass.getAverage(cmpl0);                          // Амплитуда опорного пучка равна средней амплитуды объектного
            //MessageBox.Show("am = " + am + " fz= " + fz);

            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double fz1 = 2 * Math.PI * (Math.Sin(AngleX) * kx * i + Math.Sin(AngleY) * ky * j) / Lambda;
                    double fa = rnd.NextDouble() * 2.0 * Math.PI - Math.PI;                 
                   
                    double f0 = fz1 + fa * noise + fz;
                  
                    double Ap = cmpl0.array[i, j].Magnitude;                                   // амплитуда объектного пучка
                    double Fp = cmpl0.array[i, j].Phase;                                       // Фаза объектного пучка
                    cmpl.array[i, j] = Ap * Ap + am * am + 2 * Ap * am * Math.Cos(Fp - f0);    // Интенсивность
                }
          
            return cmpl;
        }

       
        //
        //                     Умножение на фазовый фронт
        //
        public static ZComplexDescriptor Model_pl_MUL(double am, ZComplexDescriptor cmpl0, 
                                                      double AngleX, double AngleY, double Lambda, double dx, double fz=0)
        {

            int NX = cmpl0.width;
            int NY = cmpl0.height;

            am = 1;

            //ZComplexDescriptor cmpl1 = new ZComplexDescriptor(NX, NY);      // Фронт под углом AngleX,  AngleY
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);         // Результирующий фронт
            double kx = dx / NX;
            double ky = dx / NY;

            // a = (a - min) * 2.0 * Math.PI / (max - min);   -pi +pi


            for (int i = 0; i < NX; i++)
              for (int j = 0; j < NY; j++)
                {
                    Complex a = Complex.FromPolarCoordinates(am, 2 * Math.PI * (Math.Sin(AngleX) * kx * i + Math.Sin(AngleY) * ky * j) / Lambda + fz);
                    cmpl.array[i, j] = cmpl0.array[i, j] * a;
                }

            return cmpl;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //            Интерференция сферических волн
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static ZComplexDescriptor Model_Sp(double am,  double Lambda, int NX, int NY, double dx, double d)
        {

            ZComplexDescriptor cmpl1 = new ZComplexDescriptor(NX, NY);      // Второй фронт

            double kx = dx / NX;
            double ky = dx / NY;

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    int i1 = i - NX / 2;
                    int j1 = j - NY / 2;
                    double x2 = kx * kx * i1 * i1;
                    double y2 = ky * ky * j1 * j1;
                    double sqr1 = Math.Sqrt(x2 + y2 + d * d);
  //                  cmpl.array[i, j] = new Complex(am , 0.0);
                    //cmpl1.array[i, j] = Complex.FromPolarCoordinates(am / sqr1, 2 * Math.PI * sqr1 / Lambda);
                    double c = am * Math.Cos(2 * Math.PI * sqr1 / Lambda + Math.PI)  / sqr1;
                    double s = -am * Math.Sin(2 * Math.PI * sqr1 / Lambda + Math.PI) / sqr1; ;
                    cmpl1.array[i, j] = new Complex(c, s);
                }
            cmpl1.array[NX / 2, NY / 2] = cmpl1.array[NX / 2 - 1, NY / 2 - 1];
            return cmpl1;
        }

        // Параксиальное приближение (Гауссов пучок)

        public static ZComplexDescriptor Model_Sp1(double am, double Lambda, int NX, int NY, double dx, double d)
        {
           
            ZComplexDescriptor cmpl1 = new ZComplexDescriptor(NX, NY);      // Второй фронт

            double kx = dx / ( NX);
            double ky = dx / ( NY);

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    int i1 = i - NX / 2;
                    int j1 = j - NY / 2;
                    double x2 = kx * kx * i1 * i1;
                    double y2 = ky * ky * j1 * j1;
                    double sqr1 = x2 + y2 ;
            
                    double c =  am * Math.Cos(Math.PI * sqr1 / (Lambda*d) + Math.PI);
                    double s = -am * Math.Sin( Math.PI * sqr1 / (Lambda*d) + Math.PI);
                    cmpl1.array[i, j] = new Complex(c, s);
                }
            cmpl1.array[NX / 2, NY / 2] = cmpl1.array[NX / 2 - 1, NY / 2 - 1];
            return cmpl1;
        }


        public static ZComplexDescriptor Model_SpSUB(double am, double Lambda, int NX, int NY, double dx, double d)
        {

            ZComplexDescriptor cmpl1 = Model_Sp(am, Lambda, NX, NY, dx, d);
            ZComplexDescriptor cmpl2 = Model_Sp1(am, Lambda, NX, NY, dx, d);
            ZArrayDescriptor sub =new  ZArrayDescriptor(NX, NY);

            double s, s1 ;
            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                 {
                     s = cmpl1.array[i, j].Phase;
                     s1 = cmpl2.array[i, j].Phase;
                     sub.array[i, j] = Math.Abs( s1 - s);
                 }
            
            double max = (SumClass.getMax(sub) - SumClass.getMin(sub)) / Math.PI;
            MessageBox.Show(" Разброс в PI = " + max); 
            return cmpl1;
        }

        public static ZComplexDescriptor ADD_PHASE(ZComplexDescriptor cmpl0, double kx)   // Фаза - пила
        {
            //MessageBox.Show("kx = " + kx);
           int NX = cmpl0.width;
           int NY = cmpl0.height;

            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);      // Фронт под углом kx,  2PI

            double[] pf = new double[NX];
            double dx = 2 * Math.PI / kx;

            double s = 0;
            for (int i = 0; i < NX; i++, s += dx) pf[i] = s;

            int n2 = NX / 2;
            for (int i = 0; i < n2; i++) { pf[i] = pf[i + n2]; pf[i + n2] = pf[i]; }  // Инверсия

             for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double f0 = cmpl0.array[i, j].Phase;
                    double r0 = cmpl0.array[i, j].Real;
                    f0 = f0 + pf[i];
                    cmpl.array[i, j] = Complex.FromPolarCoordinates(r0, f0);
                }
               
            return cmpl;
        }


    }
}
