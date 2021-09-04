using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
//using System.Numerics;
using ClassLibrary;

namespace rab1.Forms
{
    class Unrup
    {
        private static int M1 ;
        private static int M2 ;
        private static int N1 ;
        private static int N2 ;
        /*      
        Назначение: Нахождение наибольшего общего делителя двух чисел N и M по рекуррентному соотношению
        N0 = max(|N|, |M|) N1 = min(|N|, |M|)
        N k = N k-2 - INT(N k-2 / N k-1)*N                   k-1 k=2,3 ...
       
        Если Nk = 0 => НОД = N k-1
        (N=23345 M=9135 => 1015 N=238 M=347 => 34)
        */
        private static Int32 Evklid(int N1, int N2)
        {
            int n0 = Math.Max(N1, N2);
            int n1 = Math.Min(N1, N2);

            do { Int32 n = n0 - (int)((n0 / n1) * n1); n0 = n1; n1 = n; } while (n1 != 0);

            return n0;
        }

        private static void China(int n1, int n2)
        {
            int n;
          
            int NOD = Evklid(n1, n2);   // Если NOD == 1 числа взаимно просты
            //if (NOD != 1) { MessageBox.Show("Числа не взаимно просты"); return; }
            while (NOD != 1) { n1 = n1 / NOD; n2 = n2 / NOD; NOD = Evklid(n1, n2); }

             M1 = n2;
             M2 = n1;
             N1 = 0;
             N2 = 0;
            for (int i = 0; i < n1; i++) { n = (M1 * i) % n1; if (n == 1) { N1 = i; break; } }
            for (int i = 0; i < n2; i++) { n = (M2 * i) % n2; if (n == 1) { N2 = i; break; } }
        }

        //  Рисование таблицы чисел по двум массивам k1 и k2 с сдвигом

        public static void Tabl_int(ZArrayDescriptor[] zArrayPicture, PictureBox pictureBox1, int k1, int k2, int N1_sin, int N2_sin, int scale, int d, int sdvig)
        {
            China(N1_sin, N2_sin);
            //MessageBox.Show(" M1 " + M1 + " N1 " + N1 + " M2 " + M2 + " N2 " + N2);
            
            if (zArrayPicture[k1] == null) { MessageBox.Show("zArrayPicture[k1] == NULL  Unrup.Tabl_int "); return ; }
            if (zArrayPicture[k2] == null) { MessageBox.Show("zArrayPicture[k2] == NULL  Unrup.Tabl_int "); return ; }
            
            int nx = zArrayPicture[k1].width;
            int ny = zArrayPicture[k1].height;

            //ZArrayDescriptor res = new ZArrayDescriptor(N1_sin, N2_sin);
            double max1 = SumClass.getMax(zArrayPicture[k1]);
            double min1 = SumClass.getMin(zArrayPicture[k1]);
            double max2 = SumClass.getMax(zArrayPicture[k2]);
            double min2 = SumClass.getMin(zArrayPicture[k2]); 
           // China(N1_sin, N2_sin);
            Graphics g = Graphics.FromHwnd(pictureBox1.Handle);
            g.Clear(Color.White);

            ZArrayDescriptor arr = new ZArrayDescriptor(N2_sin, N1_sin);
      
            int M = M1*N1;
            int N = M2*N2;
            int MN = N1_sin * N2_sin;

            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++)
                {
                    //double a = res.array[i, j];
                    int x = (int)((zArrayPicture[k1].array[i, j] - min1) * (N2_sin-1) / (max1 - min1));
                    int b2 = sdv(x, sdvig, N2_sin); 
                    int y = (int)((zArrayPicture[k2].array[i, j] - min1) * (N1_sin-1) / (max1 - min1));
                    int X0 = (M * b2 + N * y) % MN;
                    //if (X0 < d ) Point(b2 * scale, y * scale, g);
                    if (X0 < d) arr.array[b2, y] += 1;
                }
            }


            Point_N(arr, N1_sin, N2_sin, scale, g);
        }

        public static int sdv(int k, int sdvig, int N)
        {
            k = k + sdvig; 
            if (k > 0) k = k % N;
            if (k < 0) k = N + k;
                
            return k;
        }

        public static void Point_N(ZArrayDescriptor arr,  int N1_sin, int N2_sin, int scale, Graphics g)
        {
            double max = SumClass.getMax(arr);
            double c4= max/4;
            double c2 = max / 2;
            double c3 = c4 + c2;
            for (int i = 0; i < N2_sin; i++)
            {
                for (int j = 0; j < N1_sin; j++)
                   {
                      int x = i * scale;
                      int y = j * scale;
                      if (arr.array[i,j] == 0 ) continue;
                      if (arr.array[i, j] < c4) { g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), x, y, 1, 1); continue; }
                      if (arr.array[i, j] < c2) { g.FillRectangle(new SolidBrush(Color.FromArgb(0, 250, 0)), x, y, 1, 1); continue; }
                      if (arr.array[i, j] < c3) { g.FillRectangle(new SolidBrush(Color.FromArgb(250, 150, 0)), x, y, 1, 1); continue; }
                   }

                   
            }
        }

        //  Рисование таблицы диагоналей 

        public static void Tabl_Diag(ZArrayDescriptor[] zArrayPicture, PictureBox pictureBox1, int N1_sin, int N2_sin, int scale, int d)
        {
            China(N1_sin, N2_sin);
            MessageBox.Show(" M1 " + M1 + " N1 " + N1 + " M2 " + M2 + " N2 " + N2);

            Graphics g = Graphics.FromHwnd(pictureBox1.Handle);
            //g.Clear(Color.White);
            
            d = d * N1_sin;                     // Максимальный диапазон

            int M = M1 * N1;
            int N = M2 * N2;
            int MN = N1_sin * N2_sin;
            for (int i = 0; i < N2_sin; i++)
            {
                for (int j = 0; j < N1_sin; j++)
                {
                    int X0 = (M * i + N * j) % MN;
                    if (X0 < d) g.FillRectangle(new SolidBrush(Color.Red), i * scale, j * scale, 2, 2); 
                }
            }
        }

      
            public static void Tabl_Diag_Tab(ZArrayDescriptor[] zArrayPicture, PictureBox pictureBox1, int N1_sin, int N2_sin, int scale, int d)
        {

//            if (zArrayPicture[0] == null) { MessageBox.Show("zArrayPicture[k1] == NULL  Unrup.Tabl_int "); return; }
 //           if (zArrayPicture[1] == null) { MessageBox.Show("zArrayPicture[k2] == NULL  Unrup.Tabl_int "); return; }

 //           int nx = zArrayPicture[1].width;
//            int ny = zArrayPicture[1].height;
            
            China(N1_sin, N2_sin);
            MessageBox.Show(" M1 " + M1 + " N1 " + N1 + " M2 " + M2 + " N2 " + N2);

            Graphics g = Graphics.FromHwnd(pictureBox1.Handle);
            g.Clear(Color.White);

            int d1 = d * N1_sin;                     // Максимальный диапазон

            int M = M1 * N1;
            int N = M2 * N2;
            int MN = N1_sin * N2_sin;

//            double max1 = SumClass.getMax(zArrayPicture[0]);
//            double min1 = SumClass.getMin(zArrayPicture[0]);
//            double max2 = SumClass.getMax(zArrayPicture[1]);
//            double min2 = SumClass.getMin(zArrayPicture[1]); 
            ZIntDescriptor a = new ZIntDescriptor(N1_sin, N2_sin);
            a = Diag(a, N1_sin, N2_sin,d);
            a = Diag_array(a);
  /*          for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++)
                {
                    //double a = res.array[i, j];
                    int b2 = (int)((zArrayPicture[0].array[i, j] - min1) * (N2_sin - 1) / (max1 - min1));
                    int b1 = (int)((zArrayPicture[1].array[i, j] - min2) * (N1_sin - 1) / (max2 - min2));
                    int X0 =  a.GetValue(b2 - b1);
                    X0 = X0 * N1_sin + b1;
                    if (X0 < d1) g.FillRectangle(new SolidBrush(Color.Green), b2 * scale, b1 * scale, 1, 1); 
                }
            }
*/
           
            for (int i = 0; i < N2_sin; i++)
            {
                for (int j = 0; j < N1_sin; j++)
                {
                    int X0 = a.GetValue(i - j);
                    int c  = X0 * 93 % 250;
                    int c1 = X0 * 21 % 250;
                    int c3 = X0 * 80 % 250;
                    if (X0 != -1) g.FillRectangle(new SolidBrush(Color.FromArgb(c, c1, c3)), i * scale, j * scale, 2, 2);
                }
            }

            a = Diag(a, N1_sin, N2_sin, d);

            for (int i = 0; i < N2_sin; i++)
            {
                for (int j = 0; j < N1_sin; j++)
                {
                    int X0 = a.GetValue(i - j + 1);
                    if (X0 != -1) g.FillRectangle(new SolidBrush(Color.White), i * scale, j * scale, 2, 2);
                }
            }
            
        }
        public static void Point(int x, int y, Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.Black), x, y, 1, 1);
        }
        // ------------------------------------------------------------------------------------------------------------
        // ------------------------     Развертка
        //-------------------------------------------------------------------------------------------------------------
        public static ZArrayDescriptor Unrup_array(ZArrayDescriptor[] zArrayPicture, int k1, int k2, int N1_sin, int N2_sin, int N_diag, int sdvig, int Mngtl)
        {
            if (zArrayPicture[k1] == null) { MessageBox.Show("Unrup_array ZArrayDescriptor[k1] == NULL"); return null; }
            if (zArrayPicture[k2] == null) { MessageBox.Show("Unrup_array ZArrayDescriptor[k2] == NULL"); return null; }
            int NX = zArrayPicture[k1].width;
            int NY = zArrayPicture[k1].height;

            ZArrayDescriptor res = new ZArrayDescriptor(NX, NY);

            //MessageBox.Show("N1_sin " + N1_sin + " N2_sin " + N2_sin);

            ZIntDescriptor a = new ZIntDescriptor(N1_sin, N2_sin);   // класс для считывания с отрицательными индексами                   
            a = Diag(a, N1_sin, N2_sin, N_diag );                    // Формирование массива для развертки с -1
            a = Diag_array(a);                                       // Дополнение -1 до середины отрезка значениями диагонали

            ZIntDescriptor a1 = new ZIntDescriptor(N1_sin * Mngtl, N2_sin * Mngtl);   // класс для считывания с отрицательными индексами  
            a1 = Diag_array_Mngtl(a, Mngtl); 

          
          //a.SetValue(-N1_sin, 14);
          // double max1 = SumClass.getMax(zArrayPicture[k1]);
          // double min1 = SumClass.getMin(zArrayPicture[k1]); max1 = max1 - min1;
          // double max2 = SumClass.getMax(zArrayPicture[k2]);
          // double min2 = SumClass.getMin(zArrayPicture[k2]); max2 = max2 - min2;
           double max1 = 2 * Math.PI;
           double min1 = -Math.PI;
           double max2 = 2 * Math.PI;
           double min2 = -Math.PI;

           for (int i = 0; i < NX; i++)
               for (int j = 0; j < NY; j++)
               {
                   double b2x =(zArrayPicture[k1].array[i, j] - min1) * (N2_sin - 1) * Mngtl / max1;
                   int b2 = (int)b2x;
                   b2 = sdv(b2, sdvig, N2_sin * Mngtl);
                   double b1x = (zArrayPicture[k2].array[i, j] - min2) * (N1_sin - 1) * Mngtl / max2;
                   int b1 = (int)b1x;
                   //b1 = sdv(b1, 1, N1_sin);
                   int X = a1.GetValue(b2 - b1 );
                   res.array[i, j] = X * N1_sin * Mngtl + b1x;
                   //res.array[i, j] = X * N1_sin ; 
                   //int x = (int)((zArrayPicture[k1].array[i, j] - min1) * (N2_sin - 1) / (max1 - min1));
                   //int y = (int)((zArrayPicture[k2].array[i, j] - min2) * (N1_sin - 1) / (max2 - min2));
                   //x = sdv(x, sdvig, N2_sin); 
                   //y = y + 1;
                   //int X0 = (M * x + N * y) % MN;
                  // if (X0 < d1) res.array[i, j] = X0;
               }

           return res;
        }

        public static ZIntDescriptor Diag(ZIntDescriptor a, int N1_sin, int N2_sin, int N_diag)
        {
            //int M = M1 * N1;
            //int N = M2 * N2;
            int MN = N1_sin * N2_sin;

            China(N1_sin, N2_sin);
            int m1 = M1;
            int n1 = N1;
            int m2 = M2;
            int n2 = N2;

            for (int i = 0; i < N2_sin; i++)         // Строка
            {
                int b = ((m2 * n2 * i) % MN) / N1_sin;
                if (b < N_diag) a.SetValue(i, b); else a.SetValue(i, -1);
            }
            int i1 = N1_sin;
           
            for (int j = -N1_sin + 1; j < 0; j++)    // Столбец
            {
                int b = ((m1 * n1 * (-j)) % MN) / N1_sin;
                if (b < N_diag) a.SetValue(j, b); else a.SetValue(j, -1);
                i1--;
            }
            return a;
        }

       public static ZIntDescriptor Diag_array(ZIntDescriptor a)  // Заполнение -1 ближайшей не равной 0 величиной
       {
           int ind1 = a.ind1;
           int ind2 = a.ind2;
           ZIntDescriptor b = new ZIntDescriptor(ind1, ind2);    
           //MessageBox.Show(" ind1 -- " + ind1 + " ind2 -- " + ind2);
           for (int j = -ind1; j <= ind2; j++)
           {
              //a.SetValue(j, -2);
              //MessageBox.Show(" j= " + j + " a.GetValue(j) = " + a.GetValue(j)); 
               if ( a.GetValue(j) < 0)
               {
                  par par1 = Left(a,j);
                  par par2 = Right(a, j);
                  int bmin = a.GetValue(par1.i);                // Индекс левого элемента
                  int bmax = a.GetValue(par2.i);                // Индекс правого элемента
                  //MessageBox.Show(" j= " + j + " min_left= " + par1.s + " min_right= " + par2.s + " il= " + bmin + " ir= " + bmax);
                  if (par1.s < par2.s) b.SetValue(j, bmin); else b.SetValue(j, bmax);
                    
               }
               else b.SetValue(j, a.GetValue(j));

           }
           return b;
       }

       public static par Left(ZIntDescriptor a, int j)
         {
           int s=0;
           int ind1 = a.ind1;
           int ind2 = a.ind2;
           int b=j;
           int i1=0;
           int cntr = 0;
           for (int i = j; i > -ind1; i--, s++) { b = i; if (a.GetValue(i) >= 0) { cntr = 1; i1 = i; break; } }
           
           if (cntr != 1) 
                  { for (int i = ind2 - 1; i > -ind1; i--,s++) {  if (a.GetValue(i) >= 0) { i1 = i; break; } }  }

           par c;
           c.s = s;
           c.i = i1;
           return c;
         }

       public static par Right(ZIntDescriptor a, int j)
         {
           int s=0;
           int ind1 = a.ind1;
           int ind2 = a.ind2;
           int b=j;
           int i1=0;
           int cntr = 0;
           for (int i = j; i < ind2; i++, s++) { b = i; if (a.GetValue(i) >= 0) { cntr = 1; i1 = i; break; } }
           if (cntr != 1 )
              {  for (int i = -ind1+1; i < ind2; i++, s++) { if (a.GetValue(i) >= 0) { i1 = i; break; } } }

           par c;
           c.s = s;
           c.i = i1;
           return c;
         }
        
       public struct par
        {
            public int s;  // Расстояние
            public int i;  // Индекс
        }
         
        // Таблица увеличивается на множитель точности

         public static ZIntDescriptor  Diag_array_Mngtl(ZIntDescriptor a, int Mngtl)
         {
            if (Mngtl == 1) return a;
           
            int n1 = a.ind1 * Mngtl;
            int n2 = a.ind2 * Mngtl; 
            ZIntDescriptor b = new ZIntDescriptor(n1, n2);
            int i1 = a.ind1;
            for (int i = -n1; i < 0; i += Mngtl)        
              {
                  i1 = i / Mngtl;
                 //if (i1>a.ind2) MessageBox.Show("i " + i + " i1 " + i1);
                 int c = a.GetValue(i1);  
                       
                 for (int j = 0; j < Mngtl; j++)  b.SetValue( i + j, c);   
               }

            for (int i = 0; i <= n2 - Mngtl; i += Mngtl)
            {
                i1 = i / Mngtl;
                //if (i1>a.ind2) MessageBox.Show("i " + i + " i1 " + i1);
                int c = a.GetValue(i1);

                for (int j = 0; j < Mngtl; j++) b.SetValue(i + j, c);
            }

           return b;
         }

     // ------------------------------------------------------------------------------------------------------
     // таблица размером 256 на 256 для расшифровки

        public static ZArrayDescriptor Tabl_Diag_Tab256(  int N1_sin, int N2_sin, int d)
        {
            China(N1_sin, N2_sin);
            int nx = 256;
            int ny = 256;
            int M = M1 * N1;
            int N = M2 * N2;
            int MN = N1_sin * N2_sin;
            // MessageBox.Show(" M1 " + M1 + " N1 " + N1 + " M2 " + M2 + " N2 " + N2 + " MN = " + N1_sin * N2_sin);
            //MessageBox.Show(" N1_sin= " + N1_sin + " N2_sin= " + N2_sin);

            ZArrayDescriptor zArray = new ZArrayDescriptor(nx,ny);

            
            for (int i = 0; i < nx; i++)                                          // Строка 
              for (int j = 0; j < ny; j++)
                {
                   int  ix = (int)((double) i * N2_sin / nx)  ;
                   int  iy = (int)((double)j * N1_sin / ny);
                   int  b= ( (M1 * N1 * iy) + (M2 * N2 * (ix)) ) % MN;
                   //if (b < N1_sin)
                    
                    zArray.array[i, j] = b;
                }
             
            return zArray;

        }
        public static ZArrayDescriptor Tabl256(ZArrayDescriptor zTable, ZArrayDescriptor z1, ZArrayDescriptor z2, int b)
        {

            if (z1 == null) { MessageBox.Show("Unrup.cs   Tabl256 zArrayDescriptor[k1] == NULL"); return zTable; }
            if (z2 == null) { MessageBox.Show("Unrup.cs   Tabl256 zArrayDescriptor[k2] == NULL"); return zTable; }

            int nx = z1.width;
            int ny = z1.height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(nx, ny);
            double pi2 = Math.PI;

            for (int i = 0; i < nx; i++)                                          // Строка 
                for (int j = 0; j < ny; j++)
                {
                    int iy = (int) ((z1.array[i, j] + pi2) * 255 / (2*pi2));
                    int ix = (int) ((z2.array[i, j] + pi2) * 255 / (2 * pi2)) +b;
                    if (ix > 255) ix = 255; if (ix < 0) ix = 0;
                    if (iy > 255) iy = 255; if (iy < 0) iy = 0;
                    
                    zArray.array[i, j] = zTable.array[ix, iy] * 255 + ((z2.array[i, j] + pi2) * 255 / (2 * pi2));

                }
            return zArray;
        }
        public static ZArrayDescriptor Unrup_2pi(ZArrayDescriptor zTable, int b)
        {

            if (zTable == null) { MessageBox.Show("Unrup.cs    Unrup_2pi  zArrayDescriptor[regImage] == NULL"); return zTable; }
          

            int nx = zTable.width;
            int ny = zTable.height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(nx, ny);
            double pi2 = 2*Math.PI;

            for (int i = b; i < nx; i++)                                          // Строка 
                for (int j = 0; j < ny; j++)
                {
                    zArray.array[i, j] = zTable.array[i, j] + pi2;
                }

            for (int i = 0; i < b; i++)                                          // Строка 
                for (int j = 0; j < ny; j++)
                {
                    zArray.array[i, j] = zTable.array[i, j] ;
                }
            return zArray;
        }

        static double Line(int x, int x1, int x2, double y1, double y2)
        {
            double y = (x - x1) * (y2 - y1) / (x2 - x1) + y1;
            return y;

        }
        public static ZArrayDescriptor Unrup_Line(ZArrayDescriptor zTable, int X1, int Y1, int X2, int Y2)        // Устранение тренда по двум точкам
        {

            if (zTable == null) { MessageBox.Show("Unrup.cs    Unrup_2pi  zArrayDescriptor[regImage] == NULL"); return zTable; }


            int nx = zTable.width;
            int ny = zTable.height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(nx, ny);
            double y1 = zTable.array[X1, Y1];
            double y2 = zTable.array[X2, Y2];

            for (int i = 0; i < nx; i++)                                          // Строка 
                for (int j = 0; j < ny; j++)
                {
                    zArray.array[i, j] = zTable.array[i, j] - Line(i, X1, X2,  y1,  y2);
                }

           
            return zArray;
        }
        //------------------------------------------------------------------------------------
       
        public static ZArrayDescriptor Unrup_LineSub(ZArrayDescriptor zArray, int x0, double gr)        // Развертка по строкам
        {

            if (zArray == null) { MessageBox.Show("Unrup.cs    Unrup_Line  zArray == NULL"); return null; }


            int nx = zArray.width;
            int ny = zArray.height;

            ZArrayDescriptor zArray_res = new ZArrayDescriptor(nx, ny);
            double a;

            for (int i = 0; i < nx; i++)                                          // Строка 
                for (int j = 0; j < ny; j++)
                {
                    a = zArray.array[i, j];
                    if (i > x0) { if (a > gr) { a = a - 2 * Math.PI; } }
                    
                    zArray_res.array[i, j] = a;
                }


            return zArray_res;
        }

       // По убыванию
        public static ZArrayDescriptor Unrup_LinePluss(ZArrayDescriptor zArray, int x0, double gr)        // Развертка по строкам 
        {

            if (zArray == null) { MessageBox.Show("Unrup.cs    Unrup_Line  zArray == NULL"); return null; }


            int nx = zArray.width;
            int ny = zArray.height;

            ZArrayDescriptor zArray_res = new ZArrayDescriptor(nx, ny);
            double a;

            for (int j = 0; j < ny; j++)
                for (int i = 0; i < nx; i++)                                          // Строка 
                 {
                    a = zArray.array[i, j];
                    if (i > x0) { if (a < gr) { a = a + 2 * Math.PI; } }
                    zArray_res.array[i, j] = a;
                 }


            return zArray_res;
        }

        public static ZArrayDescriptor Unrup_Line2pi(ZArrayDescriptor zArray)        // Развертка по строкам 
        {

            if (zArray == null) { MessageBox.Show("Unrup.cs    Unrup_Line2pi  zArray == NULL"); return null; }

            int nx = zArray.width;
            int ny = zArray.height;

            ZArrayDescriptor zArray_res = new ZArrayDescriptor(nx, ny);

           
            for (int j = 0; j < ny; j++)                                          // Строка 
            {
                int n = 0;
                double a1 = zArray.array[0, j];
                zArray_res.array[0, j] = a1;
                for (int i = 1; i < nx; i++)
                {         
                    double a2 = zArray.array[i, j];
                    if (Math.Abs(a2 - a1) > 2 ) { n = n + 1; }
                    zArray_res.array[i, j] = 2*Math.PI*n + a2; 
                    //zArray_res.array[i, j] = n;
                    a1 = a2;
                }
            }

            return zArray_res;
        }

            // Переходы через 2PI
        public static ZArrayDescriptor Unrup_Line2pi_L(ZArrayDescriptor zArray)        // Развертка по строкам 
        {

            if (zArray == null) { MessageBox.Show("Unrup.cs    Unrup_Line2pi  zArray == NULL"); return null; }

            int nx = zArray.width;
            int ny = zArray.height;

            ZArrayDescriptor zArray_res = new ZArrayDescriptor(nx, ny);


            for (int j = 0; j < ny; j++)                                          // Строка 
            {
                //double n = 0;
                double a1 = zArray.array[0, j];
                for (int i = 1; i < nx; i++)
                {
                    double a2 = zArray.array[i, j];
                    //if (Math.Abs(a2 - a1) > 2) { n = n + 2* Math.PI; }
                    //zArray_res.array[i, j] = 2 * Math.PI * n ;
                    if (Math.Abs(a2 - a1) > 2) { zArray_res.array[i, j] = 255; } else zArray_res.array[i, j] = 0;
                    a1 = a2;
                }
            }

            return zArray_res;
        }
    }
}
//               Pen myPen = new Pen(Color.Black, 1);
//               Graphics g = Graphics.FromHwnd(pictureBox1.Handle);
//               g.DrawRectangle(myPen, 10, 10, 50, 50);
//               Point(100, 100, g);