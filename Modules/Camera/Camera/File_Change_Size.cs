using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;
using System.Diagnostics;
using rab1.Forms;

namespace Camera
{
    class File_Change_Size
    {
        private static int min4x(int X1, int X2, int X3, int X4)
        {
            int min = int.MaxValue;

            if (X1 < min) min = X1;
            if (X2 < min) min = X2;
            if (X3 < min) min = X3;
            if (X4 < min) min = X4;
            return min;
        }

        private static int max4x(int X1, int X2, int X3, int X4)
        {
            int max = 0;

            if (X1 > max) max = X1;
            if (X2 > max) max = X2;
            if (X3 > max) max = X3;
            if (X4 > max) max = X4;
            return max;
        }
        /// <summary>
        /// /////////////////////////////////////////// Удаление трапеции
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// 
       
        private static void RVSR(ref Form1.Coords X, ref Form1.Coords Y)  // Перестановка местами 2 структур
        {
            Form1.Coords R;
            R = X; X = Y; Y = R;
        }
        private static void Sort4( Form1.Coords[] X)
        {
            Form1.Coords[] R = new Form1.Coords[4];
            //int min = int.MaxValue;
            //int max = int.MaxValue;

            for (int i = 0; i < 3; i++)                                 // Сортировка методом всплывающего пузырька (В порядке возрастания)
                for (int j = 0; j < 3; j++)
                {
                    if (X[j].x > X[j + 1].x) RVSR(ref X[j], ref X[j + 1]);
                }

            if (X[0].y > X[1].y) RVSR(ref X[0], ref X[1]);                      // Сортировка по Y
            if (X[2].y > X[3].y) RVSR(ref X[2], ref X[3]);
        }

        // Биленейная интерполяция интенсивности в единичном квадрате 
        private static double Intens(ZArrayDescriptor zArrayDescriptor, double x, double y ) 
        {
            int ix0 = (int)x;
            int iy0 = (int)y;
            int  ix1 = ix0 + 1,  iy1 = iy0 + 1;

            double f00, f01=100, f10=100, f11=100;

            //MessageBox.Show(" x " + ix0 + " y " + iy0);
            f00 = zArrayDescriptor.array[ix0, iy0];
            
            f01 = zArrayDescriptor.array[ix0, iy1];
            f10 = zArrayDescriptor.array[ix1, iy0];
            f11 = zArrayDescriptor.array[ix1, iy1];

            x = x - ix0; y = iy0 - y;
            double i =(f00 * (1 - x) * (1 - y) + f10 * x * (1 - y) + f01 * (1 - x) * y + f11 * x * y);
            return i;
        }
        // Нахождение Y по двум вершинам (max_x - максимальное число точек, ny  - номер точки по Y)
        private static Form1.Coords YYY(Form1.Coords X0, Form1.Coords X1, int max_y, int ny)  
        {
            Form1.Coords R;

            double xx = (X1.x - X0.x);
            double y, xk;
            if (xx == 0)
                { xk = X0.x;  y = X0.y + (X1.y - X0.y) * ny / max_y; }
            else {
                xk = X0.x + xx * ny / max_y;
                y = (xk - X0.x) * ((X1.y - X0.y)) / xx + X0.y;
                 }
         
            R.x = xk;
            R.y = y;
            return R;
        }
        private static double Y(Form1.Coords X0, Form1.Coords X1,  int xk, int max_y)
        {
            double yy = (X1.y - X0.y);
            double y;
            if (yy == 0) { y = X0.y;  }
                else    {  y = (xk - X0.x) * ((X1.y - X0.y)) / (X1.x - X0.x) + X0.y; }
            
            return y;
        }
        private static int Max_y(Form1.Coords[] X)  // Максимальное число точек по Y
        {
            double x = X[0].x - X[1].x;
            double y = X[0].y - X[1].y;
            int max1 = (int) Math.Sqrt(x*x+y*y);
             x = X[2].x - X[3].x;
             y = X[2].y - X[3].y;
            int max2 = (int)Math.Sqrt(x * x + y * y);

            if (max1 > max2) return max1; else return max2;
        }

        private static int Max_x(Form1.Coords[] X)  // Максимальное число точек по X
        {
            double x = X[0].x - X[2].x;
            double y = X[0].y - X[2].y;
            int max1 = (int)Math.Sqrt(x * x + y * y);
            x = X[1].x - X[3].x;
            y = X[1].y - X[3].y;
            int max2 = (int)Math.Sqrt(x * x + y * y);

            if (max1 > max2) return max1; else return max2;
        }

        private static int MinX(Form1.Coords[] X)  // Максимальное число точек по X
        {
            int minX =(int) X[0].x;
            for (int i = 0; i < 3; i++)
            {
                if (X[i].x < minX) minX =(int) X[i].x;
            }
            return minX;
        }
        private static int MaxX(Form1.Coords[] X)  // Максимальное число точек по X
        {
            int maxX = (int)X[0].x;
            for (int i = 0; i < 3; i++)
            {
                if (X[i].x > maxX) maxX = (int)X[i].x;
            }
            return maxX;
        }
        public static int MinY(Form1.Coords[] X)  // Максимальное число точек по X
        {
            int minY = (int)X[0].y;
            for (int i = 0; i < 3; i++)
            {
                if (X[i].y < minY) minY = (int)X[i].y;
            }
            return minY;
        }
        public static int MaxY(Form1.Coords[] X)  // Максимальное число точек по X
        {
            int maxY = (int)X[0].y;
            for (int i = 0; i < 3; i++)
            {
                if (X[i].y > maxY) maxY = (int)X[i].y;
            }
            return maxY;
        }
        public static ZArrayDescriptor Change_rectangle(ZArrayDescriptor zArrayDescriptor,  Form1.Coords[] X )
        {
          
            if (zArrayDescriptor == null) { MessageBox.Show(" Change_trapezium: zArrayDescriptor == null"); return null; }
            int w1 = zArrayDescriptor.width;
            int h1 = zArrayDescriptor.height;
            
            /*
                        int X0 = MinX(X); if (X0 < 0)  X0 = 0;
                        int X1 = MaxX(X); if (X1 > w1) X1 = w1;

                        int Y0 = MinY(X); if (Y0 < 0) Y0 = 0;
                        int Y1 = MaxY(X); if (Y1 > h1) Y1 = h1;
            */

            int X0 = (int)X[0].x; if (X0 < 0)  X0 = 0;
            int X1 = (int)X[1].x; if (X1 > w1) X1 = w1;

            int Y0 = (int)X[2].y; if (Y0 < 0)  Y0 = 0;
            int Y1 = (int)X[3].y; if (Y1 > h1) Y1 = h1;

            ZArrayDescriptor zArray1 = new ZArrayDescriptor(X1 - X0, Y1 - Y0);

            for (int j = Y0;   j < Y1; j++)
              for (int i = X0; i < X1; i++)
                {
                    zArray1.array[i-X0, j-Y0] = zArrayDescriptor.array[i , j ];
                }

            return zArray1;

        }

        
       public static ZArrayDescriptor Change_trapezium(ZArrayDescriptor zArrayDescriptor, Form1.Coords[] X)
        {
            Sort4(X);
            // int k = regComplex * 4;

            if (zArrayDescriptor == null) { MessageBox.Show(" Change_rectangle: zArrayDescriptor == null"); return null; }
            int w1 = zArrayDescriptor.width;
            int h1 = zArrayDescriptor.height;

            int max_x = Max_x(X);
            int max_y = Max_y(X);


            ZArrayDescriptor zArray1 = new ZArrayDescriptor(max_x, max_y);


            for (int j = 0; j < max_y; j++)
              for (int i = 0; i < max_x; i++)
                {
                    Form1.Coords R1 = YYY(X[0], X[1], max_y, j);
                    Form1.Coords R2 = YYY(X[2], X[3], max_y, j);
                    double x = X[0].x + i;
                    double y = Y(R1, R2, (int)x, max_y);
                    int ix = (int)x;
                    int iy = (int)y;
                    //MessageBox.Show(" x " + x + " y " + y);
                    if (iy < 0 || iy > h1) continue;
                    if (ix < 0 || ix > w1) continue;
                    zArray1.array[i, j] = zArrayDescriptor.array[ix, iy];
                    //zArray1.array[i, j] = Intens(zArrayDescriptor, x, y);
                }


            return zArray1;

            //MessageBox.Show(" max_x " + max_x + "max_y " + max_y);
            // if (iy < 0 || iy > h1)                       MessageBox.Show(" iy " + iy + " x= " + x + " y= " + y + " R1.y= " + R1.y + " R2.y= " + R2.y);
            // if (ix<0 || ix > w1)  MessageBox.Show(" ix " + ix + " x= " + x + "y= " + y + " i= " + i + "j= " + j);
            // 
        }

        public static void Change_r(ZArrayDescriptor[] zArrayDescriptor,
                                    PictureBox pictureBox9, PictureBox pictureBox10, PictureBox pictureBox11, PictureBox pictureBox12,
                                    int X1, int X2, int X3, int X4, int Y1, int Y2, int Y3, int Y4)
        {
            for (int i = 8; i < 12; i++) 
                          { if (zArrayDescriptor[i] == null) { MessageBox.Show(" Change_r: zArrayDescriptor[" + i + "] == null"); return; } }
            
            int max_X = zArrayDescriptor[8].width;
            int max_Y = zArrayDescriptor[8].height;
            
            int minx = min4x(X1, X2, X3, X4);
            int maxx = max4x(X1, X2, X3, X4);
            int miny = min4x(Y1, Y2, Y3, Y4);
            int maxy = max4x(Y1, Y2, Y3, Y4);

            int rx = (maxx - minx) / 2;
            int ry = (maxy - miny) / 2;
            int rad = Math.Min(rx, ry);
            int x0 = minx + rad;
            int y0 = miny + rad;

            //Vizual.Vizual_Circle(zArrayPicture, pictureBox01, x0, y0, rad);    // Рисование круга
            // Заполнение нулями за пределами круга


            Vizual.Vizual_Circle(zArrayDescriptor[8],   pictureBox9,  x0, y0, rad); 
            Vizual.Vizual_Circle(zArrayDescriptor[9],   pictureBox10, x0, y0, rad);
            Vizual.Vizual_Circle(zArrayDescriptor[10],  pictureBox11, x0, y0, rad); 
            Vizual.Vizual_Circle(zArrayDescriptor[11],  pictureBox12, x0, y0, rad); 
           
            if ((maxx - minx) % 2 != 0) maxx++;
            if ((maxy - miny) % 2 != 0) maxy++;

            int X = maxx - minx, Y = maxy - miny;


            int k = X / 256;     X = (k + 1) * 256;        // Выравниваем на границe L=256 (для фурье с четным количеством точек)
                k = Y / 256;     Y = (k + 1) * 256;
            while (X + minx > max_X) X = X - 128;
            while (Y + miny > max_Y) Y = Y - 128;
            X = Math.Max(X, Y);
           
            MessageBox.Show("Реальный размер  minx=  " + (maxx - minx) + " miny=  " + (maxy - miny) + "Рекомендованный размер  =  " + X + " x " + Y);

            ZArrayDescriptor zArray = new ZArrayDescriptor(X, X);

            int x00 = 0, y00 = 0;
            int YY = X; int XX = X;

            if (   (X > (maxx - minx)) && (Y > (maxy - miny))   )
            {
                int kx = (X - (maxx - minx)) / 2; 
                int ky = (X - (maxy - miny)) / 2; 

                x00 = minx - kx; if (x00 > 0) XX = X - kx; else { x00 = 0; }
                y00 = miny - ky; if (y00 > 0) YY = X - ky; else { y00 = 0; }
               // MessageBox.Show(" kx=  " + kx + " x0=  " + x00 + " y0=  " + y00 + "X  =  " + X + "Y =  " + Y);
            }
           
            XX = Math.Max(XX, YY);

            if (XX + x00 > max_X) {   x00 = max_X - XX; }
            if (XX + y00 > max_Y) {   y00 = max_X - XX; }

            //int n = 8;
            for (int n = 8; n < 12; n++)
            {
                for (int i = x00; i < XX + x00; i++)
                    for (int j = y00; j < XX + y00; j++)
                       { zArray.array[i - x00, j - y00] = zArrayDescriptor[n].array[i, j]; }
                //zArrayDescriptor[n] = zArray;
                zArrayDescriptor[n] = new ZArrayDescriptor(zArray);   
            }

        }

        public static ZArrayDescriptor Change_4(ZArrayDescriptor zArrayPicture, int X1, int X2, int X3, int X4, int Y1, int Y2, int Y3, int Y4)
        {
            if (zArrayPicture == null) { MessageBox.Show(" zArrayPicture File_Change_Size.cs == null"); return null; }

           
            int max_X = zArrayPicture.width;
            int max_Y = zArrayPicture.height;

            int minx = min4x(X1, X2, X3, X4);
            int maxx = max4x(X1, X2, X3, X4);
            int miny = min4x(Y1, Y2, Y3, Y4);
            int maxy = max4x(Y1, Y2, Y3, Y4);
      
            if ((maxx - minx) % 2 != 0) maxx++;    // Четное количество точек
            if ((maxy - miny) % 2 != 0) maxy++;

            int X = maxx - minx, Y = maxy - miny;


            int k = X / 256; X = (k + 1) * 256;   // Выравниваем на границe L=256 (для фурье с четным количеством точек)
            k = Y / 256; Y = (k + 1) * 256;
            X = Math.Max(X, Y);
            while (X + minx > zArrayPicture.width) X = X - 128;
            while (X + miny > zArrayPicture.height) X = X - 128;

            MessageBox.Show("Реальный размер  minx=  " + (maxx - minx) + " miny=  " + (maxy - miny) + "Рекомендованный размер  =  " + X + " x " + X);


            ZArrayDescriptor zArray = new ZArrayDescriptor(X, X);
            int x0 = 0, y0 = 0;
            Y = X;
            if (X>(maxx-minx) && Y>(maxy - miny))
            {
                int kx = (X - (maxx - minx)) / 2; 
                int ky = (X - (maxy - miny)) / 2; 
           
              x0 = minx - kx; if (x0 > 0 ) X = X - kx; else { x0 = 0; }
              y0 = miny - ky; if (y0 > 0)  Y = X - ky; else { y0 = 0; }
            }
            X = Math.Max(X, Y);

           // MessageBox.Show("x0=  " + x0 + " y0=  " + y0 + "X  =  " + X );
            
            for (int i = x0; i < X+x0; i++)
                for (int j = y0; j < X+y0; j++)
                    zArray.array[i - x0, j - y0] = zArrayPicture.array[i, j];

            return zArray;

        }

       
    }
}
