using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace rab1
{
    public class Pi_Class1
    {
        //MessageBox.Show(" n1= " + N1.ToString() + " n2= " + N2.ToString());

        static Int32 M1 = -1;
        static Int32 M2 = -1;
        static Int32 N1 = -1;
        static Int32 N2 = -1;
        static Int32 n1;         
        static Int32 n2;
        static int NMAX=1600;
        static int[] glbl_faze  = new int[NMAX];               // Номера для прослеживания полос (номера линий)
        static int[] glbl_faze1 = new int[NMAX];               // Количество добавлений b1 (Для расшифровки)
        static int[] number_2pi = new int[200];                // Максимум 200 полос (пока) ------------------------------
        static Form  f_sin;
        static PictureBox pc1;
        static int scale = 4;
        static int x0 = 46, y0 = 16;
        static double A=0, B=0, C=0, D=0;

        static Int32[,] Z;                                     // Глобальный массив результирующих фаз (Размер задается при расшифровке)

        /*      
        Назначение: Нахождение наибольшего общего делителя двух чисел N и M по рекуррентному соотношению
        N0 = max(|N|, |M|) N1 = min(|N|, |M|)
        N k = N k-2 - INT(N k-2 / N k-1)*N                   k-1 k=2,3 ...
       
        Если Nk = 0 => НОД = N k-1
        (N=23345 M=9135 => 1015 N=238 M=347 => 34)
        */
        private static Int32 Evklid(Int32 N1, Int32 N2)
        {
           Int32 n0 = Math.Max(N1, N2);
           Int32 n1 = Math.Min(N1, N2);
          
           do { Int32 n = n0 - (int)((n0 / n1) * n1); n0 = n1; n1 = n; } while (n1 != 0);
          
           return n0;

        }

        private static Int32 Dot(string sN1)
        {
            Int32 N1, n;
            

            if (sN1.Contains("."))
            {
                n = sN1.IndexOf(".");
                sN1 = sN1.Substring(0, n) + sN1.Substring(n + 1);  
            }
            if (sN1.Contains(","))
            {
                n = sN1.IndexOf(",");
                sN1 = sN1.Substring(0, n) + sN1.Substring(n + 1);  
            }
            N1 = Convert.ToInt32(sN1);
            return N1;
        }

        private static void China(string sN1, string sN2)
        {
            int n;
            n1 = Dot(sN1);         // Убирает точку и запятую
            n2 = Dot(sN2);
            Int32 NOD = Evklid(n1, n2);   // Если NOD == 1 числа взаимно просты
            if (NOD != 1) { MessageBox.Show("Числа не взаимно просты"); return; }

            M1 = n2;
            M2 = n1;
            N1 = -1;
            N2 = -1;
            for (int i = 0; i < M1; i++) { n = (M1 * i) % n1; if (n == 1) { N1 = i; break; } } if (N1 < 0) N1 = N1 + n1;
            for (int i = 0; i < M2; i++) { n = (M2 * i) % n2; if (n == 1) { N2 = i; break; } } if (N2 < 0) N2 = N2 + n2;
        }
// --------------------------------------------------------------------------------------------------------------------------- Рисование таблицы  (параметры) (b2, b1)
        private static void Graph_Prmtr(Image[] img, int Diag, Point p)
        {

            int max_x = NMAX, max_y = 800;
            
            int w1 = n2, hh = n1;
           
            f_sin = new Form();
            f_sin.Size = new Size(max_x + 8, max_y + 8);
            f_sin.StartPosition = FormStartPosition.Manual;

            f_sin.Location = p;

            pc1 = new PictureBox();
            pc1.BackColor = Color.White;
            pc1.Location = new System.Drawing.Point(0, 8);
            pc1.Size = new Size(max_x, max_y);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;
            pc1.BorderStyle = BorderStyle.Fixed3D;

            Bitmap btmBack = new Bitmap(max_x + 8, max_y + 8);      //изображение          
            Graphics grBack = Graphics.FromImage(btmBack);

            pc1.BackgroundImage = btmBack;


            f_sin.Controls.Add(pc1);

            Pen p1 = new Pen(Color.Black, 1);
            Pen p2 = new Pen(Color.Red, 1);
            Pen p3 = new Pen(Color.Blue, 1);
            Pen p4 = new Pen(Color.Yellow, 1);
            Font font = new Font("Arial", 16, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            //Font font = new Font(FontFamily.GenericSansSerif, 12.0F, FontStyle.Regular);
            //Font font = new Font("Verdana", 14, FontStyle.Regular);

            grBack.DrawLine(p1, x0, y0, x0, hh * scale + y0);
            grBack.DrawLine(p1, x0, hh * scale + y0, 2 * w1 * scale + x0, hh * scale + y0);

            grBack.DrawLine(p1, w1 * scale + x0, hh * scale + y0, w1 * scale + x0, y0);
            grBack.DrawLine(p1, w1 * scale + x0, y0, x0, y0);

           

            StringFormat drawFormat = new StringFormat(StringFormatFlags.NoClip);
            //drawFormat.LineAlignment = StringAlignment.Near ;  //   .Center;
            // drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

            string s = n2.ToString();  grBack.DrawString("b2 " + s, font, new SolidBrush(Color.Black), w1 * scale + x0 + 8, y0 - 8, drawFormat);
                   s = n1.ToString();         grBack.DrawString("b1 " + s, font, new SolidBrush(Color.Black), x0, hh * scale + 20, drawFormat);
                   s = M1.ToString() + "*" + N1.ToString() + " b1  + " + M2.ToString() + "*" + N2.ToString() + " b2 mod " + (n1 * n2).ToString();
                   grBack.DrawString(s, font, new SolidBrush(Color.Black), x0 + w1 * scale + 5 * scale, y0 + 5 * scale, drawFormat);


            
            Int32 A = Diag * Math.Max(n1, n2);
             Int32 pf;
             for (int b2 = 0; b2 < n2; b2++)                                                                    // Диагонали
             {
                 pf = M2 * N2 * b2 % (n1 * n2);
                 if (pf < A) 
                 {
                     int x1 = x0 + b2 * scale; 
                     grBack.DrawLine(p4, x1, y0, x1 , hh * scale + y0);

                     grBack.DrawLine(p2, x0 + b2 * scale, y0, x0 + b2 * scale + n1 * scale, hh * scale + y0);
                     pf = pf / n1;     s = pf.ToString();
                     grBack.DrawString(s, font, new SolidBrush(Color.Black), x0 + b2 * scale, y0 - 4 * scale, drawFormat);
                 }
             }
             for (int b1 = 0; b1 < n1; b1++)
             {
                 pf = M1 * N1 * b1 % (n1 * n2);
                 if (pf < A)
                 {
                     int y1 = y0 + b1 * scale;
                     grBack.DrawLine(p4, x0, y1, x0 + w1 * scale, y1); 

                     grBack.DrawLine(p3, x0, y0 + b1 * scale, x0 + n1 * scale - b1 * scale, hh * scale + y0); 
                     pf = pf / n1; s = pf.ToString();
                     grBack.DrawString(s, font, new SolidBrush(Color.Black), x0 - 10 * scale, y0 + b1 * scale, drawFormat);
                 }
             }


             pc1.Refresh();
             f_sin.Show();

        }

      
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------ Построение таблицы
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void Graph_China(Image[] img, PictureBox pictureBox01, int Diag, Point p, int x0_end, int x1_end, int y0_end, int y1_end, int rb, int pr_obr)
        {
            int max_x = (n1+n2)*scale, max_y = 800;     
            int w1 = n2, hh = n1;

            f_sin = new Form();
            f_sin.Size = new Size(max_x + 8, max_y + 8);
            f_sin.StartPosition = FormStartPosition.Manual;          
            f_sin.Location = p;

            pc1 = new PictureBox();
            pc1.BackColor = Color.White;
            pc1.Location = new System.Drawing.Point(0, 8);
            pc1.Size = new Size(max_x, max_y);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;
            pc1.BorderStyle = BorderStyle.Fixed3D;

            Bitmap btmBack = new Bitmap(max_x + 8, max_y + 8);      //изображение          
            Graphics grBack = Graphics.FromImage(btmBack);
            
            pc1.BackgroundImage = btmBack;
          

            f_sin.Controls.Add(pc1);

            Pen p1 = new Pen(Color.Black, 1);
            Pen p2 = new Pen(Color.Red, 1);
            Pen p3 = new Pen(Color.Blue, 1);
            Pen p4 = new Pen(Color.Gold, 1);
            Pen p5 = new Pen(Color.Yellow, 1);
            Font font = new Font("Arial", 16, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            
            grBack.DrawLine(p1, x0, y0, x0, hh * scale + y0);
            grBack.DrawLine(p1, x0, hh * scale + y0, 2 * w1 * scale + x0, hh * scale + y0);

            grBack.DrawLine(p1, w1 * scale + x0, hh * scale + y0, w1 * scale + x0, y0);
            grBack.DrawLine(p1, w1 * scale + x0, y0, x0, y0);

            StringFormat drawFormat = new StringFormat(StringFormatFlags.NoClip);

            string s = n2.ToString(); grBack.DrawString("b2 " + s, font, new SolidBrush(Color.Black), w1 * scale + x0 + 8, y0 - 8, drawFormat);
            s = n1.ToString(); grBack.DrawString("b1 " + s, font, new SolidBrush(Color.Black), x0, hh * scale + 20 + 10 * scale, drawFormat);
// ----------------------------------------------------------------------------------------------------------------
            for (int i = 0; i < n1 + n2; i++) { glbl_faze[i] = -1; glbl_faze1[i] = -1; }                                              // Массив для расшифровки
                                                
            Int32 A = Diag * Math.Max(n1, n2);
            Int32 pf;
            for (int b2 = 0; b2 < n2; b2++)                                                                    // Диагонали   
            {
                pf = M2 * N2 * b2 % (n1 * n2);
                if (pf < A) { grBack.DrawLine(p2, x0 + b2 * scale, y0, x0 + b2 * scale + n1 * scale, hh * scale + y0);
                              pf = pf / n1; 
                              glbl_faze[n1 + b2] = pf;
                              s = pf.ToString();    grBack.DrawString(s, font, new SolidBrush(Color.Black), x0 + b2 * scale, y0 - 4 * scale, drawFormat);
                            }
            }
            for (int b1 = 0; b1 < n1; b1++)
            {
                pf = M1 * N1 * b1 % (n1 * n2);
                if (pf < A) { grBack.DrawLine(p3, x0, y0 + b1 * scale, x0 + n1 * scale - b1 * scale, hh * scale + y0);
                              pf = pf / n1;
                              glbl_faze[n1 - b1] = pf;
                              s = pf.ToString();    grBack.DrawString(s, font, new SolidBrush(Color.Black), x0 - 10 * scale, y0 + b1 * scale, drawFormat);
                            }
            }

            for (int i = 0; i < n1 + n2; i++)
            {
                int bb = glbl_faze[i];
                if (bb >= 0) { s = bb.ToString(); grBack.DrawString(s, font, new SolidBrush(Color.Black), x0 + i * scale, y0 + n1*scale + 8 * scale, drawFormat); }
            }

            int mxx = 0, mxx_x = 0, mnx = 0, mnx_x = 0, cntr = 0;
            for (;;)
            {
                for (int i = mnx_x; i < n1 + n2; i++)
                {
                    cntr = i;
                    int bb = glbl_faze[i]; if (bb >= 0 && bb != mnx) { mxx = bb; mxx_x = i; break; }
                }
                if (cntr >= n1 + n2 - 1) break;                    
               int m = (mxx_x - mnx_x) /2;
               for (int j = mnx_x; j < mnx_x + m; j++) glbl_faze1[j] = mnx;
               for (int j = mnx_x + m; j < mxx_x; j++) glbl_faze1[j] = mxx;
               mnx_x = mxx_x; 
               mnx = mxx; 
               
            }


            mnx = glbl_faze1[0];
            for (int i = 0; i < n1 + n2; i++)
            {
                int bb = glbl_faze1[i];
                if (bb != mnx) 
                   { mnx = bb;
                     grBack.DrawLine(p4, x0 + i * scale, y0 + hh * scale, x0, y0 + hh * scale - i * scale);
                   }
            }
           
//--------------------------------------------------------------------------------------------------------------- 
            int r, g, b;
            int w = img[0].Width;
            int h = img[0].Height;
            Bitmap bmp1 = new Bitmap(img[0], w, h);
            Bitmap bmp2 = new Bitmap(img[1], w, h);
            Bitmap bmp3 = new Bitmap(img[2], w, h);
            Bitmap bmp  = new Bitmap(pictureBox01.Image, w, h);

            int[,] bmp_r = new int[n2+3, n1+3];
            int[,] bmp_line = new int[n2 + 3, n1 + 3];
            int[] ims1 = new int[h];
            int[] ims2 = new int[h];
            int[] ims3 = new int[h];      

            Color c, c1;

            int xx0 = 0,  yy0 = 0;
            int xx1 = w,  yy1 = h;

            if ((x0_end < x1_end) && (y0_end < y1_end)) { xx0 = x0_end; xx1 = x1_end; yy0 = y0_end; yy1 = y1_end; }

            Int32 count = 0;
            if (rb == 1)                                             // ------------- По фигуре из 3 квадрата
                for (int i = xx0; i < xx1; i++)
                {
                    
                    for (int j = yy0; j < yy1; j++)
                    {
                            c1 = bmp3.GetPixel(i, j);
                            if (c1.R == 0) ims3[j] = 0; 
                            else
                            {
                                ims3[j] = 1;
                                c = bmp1.GetPixel(i, j); r = c.R; ims1[j] = (int)((double)(r * (n1 - 1)) / 255);  //(b2)
                                c = bmp2.GetPixel(i, j); r = c.R; ims2[j] = (int)((double)(r * (n2 - 1)) / 255);  //(b1) 
                            }
                       
                    }
                    
                    for (int j = yy0; j < yy1; j++)
                    {
                        if (ims3[j] != 0) { r = ims1[j]; g = ims2[j]; bmp_r[g, r]++; count++; }
                    }
                }
            if (rb == 0)                                               // --------- По квадратной области
                for (int i = xx0; i < xx1; i++)
                {
                   
                    for (int j = yy0; j < yy1; j++)
                    {                                          
                            c = bmp1.GetPixel(i, j); r = c.R; ims1[j] = (int)((double)(r * (n1 - 1)) / 255);  //(b2)
                            c = bmp2.GetPixel(i, j); r = c.R; ims2[j] = (int)((double)(r * (n2 - 1)) / 255);  //(b1)   
                    }

                    for (int j = yy0; j < yy1; j++)
                    {
                         r = ims1[j]; g = ims2[j]; bmp_r[g, r]++; count++; 
                    }
                }

          

            Int32 ib2=0, ib1=0, max_count = 0;
             for (ib2 = 0; ib2 < n2-1; ib2++)
              {
                 for (ib1 = 0; ib1 < n1 - 1; ib1++) { b = bmp_r[ib2, ib1]; if (b > max_count) max_count = b; }
              }
            
            int mn1 = pr_obr;
                      
            int mn = (max_count)/12;
            int mn2 = mn1 + mn;
            int mn3 = mn2 + mn;
             MessageBox.Show(" count =  " + count.ToString() + " max =  " + max_count.ToString());


            for (ib2 = 0; ib2 < n2-1; ib2++)
            {
              for ( ib1 = 0; ib1 < n1-1; ib1++)
                    { b = bmp_r[ib2, ib1]; 
                      if (b > 0) 
                            {                               
                                if (b > mn3 )            grBack.DrawRectangle(new Pen(Color.FromArgb(255, 0, 0)), x0 + ib2 * scale , y0 + ib1 * scale, 1, 1);
                                if (b > mn2 && b <=mn3)  grBack.DrawRectangle(new Pen(Color.FromArgb(0, 0, 255)), x0 + ib2 * scale , y0 + ib1 * scale, 1, 1);
                                if (b > mn1 && b <=mn2)  grBack.DrawRectangle(new Pen(Color.FromArgb(32, 32, 32)), x0 + ib2 * scale , y0 + ib1 * scale, 1, 1);                                
                           }
                    }
              }
         
            pc1.Refresh();
            f_sin.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void Graph_China2(Image[] img, PictureBox pictureBox01, int Diag, Point p, int x0_end, int x1_end, int y0_end, int y1_end, int rb, int pr_obr, PictureBox firstImage, PictureBox secondImage)
        {
            int max_x = (n1 + n2) * scale, max_y = 800;
            int w1 = n2, hh = n1;

            f_sin = new Form();
            f_sin.Size = new Size(max_x + 8, max_y + 8);
            f_sin.StartPosition = FormStartPosition.Manual;
            f_sin.Location = p;

            pc1 = new PictureBox();
            pc1.BackColor = Color.White;
            pc1.Location = new System.Drawing.Point(0, 8);
            pc1.Size = new Size(max_x, max_y);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;
            pc1.BorderStyle = BorderStyle.Fixed3D;
            pc1.Image = new Bitmap(max_x + 8, max_y + 8);

            for (int i = 0; i < pc1.Image.Width; i++)
            {
                for (int j = 0; j < pc1.Image.Height; j++)
                {
                    ((Bitmap)pc1.Image).SetPixel(i, j, Color.White);
                }
            }

            //Bitmap btmBack = new Bitmap(max_x + 8, max_y + 8);      //изображение          
            Graphics grBack = Graphics.FromImage(pc1.Image);

           // pc1.BackgroundImage = btmBack;


            f_sin.Controls.Add(pc1);

            Pen p1 = new Pen(Color.Black, 1);
            Pen p2 = new Pen(Color.Red, 1);
            Pen p3 = new Pen(Color.Blue, 1);
            Pen p4 = new Pen(Color.Gold, 1);
            Pen p5 = new Pen(Color.Yellow, 1);
            Font font = new Font("Arial", 16, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

            grBack.DrawLine(p1, x0, y0, x0, hh * scale + y0);
            grBack.DrawLine(p1, x0, hh * scale + y0, 2 * w1 * scale + x0, hh * scale + y0);

            grBack.DrawLine(p1, w1 * scale + x0, hh * scale + y0, w1 * scale + x0, y0);
            grBack.DrawLine(p1, w1 * scale + x0, y0, x0, y0);

            StringFormat drawFormat = new StringFormat(StringFormatFlags.NoClip);

            string s = n2.ToString(); grBack.DrawString("b2 " + s, font, new SolidBrush(Color.Black), w1 * scale + x0 + 8, y0 - 8, drawFormat);
            s = n1.ToString(); grBack.DrawString("b1 " + s, font, new SolidBrush(Color.Black), x0, hh * scale + 20 + 10 * scale, drawFormat);
            // ----------------------------------------------------------------------------------------------------------------
            for (int i = 0; i < n1 + n2; i++) { glbl_faze[i] = -1; glbl_faze1[i] = -1; }                                              // Массив для расшифровки

            Int32 A = Diag * Math.Max(n1, n2);
            Int32 pf;
            for (int b2 = 0; b2 < n2; b2++)                                                                    // Диагонали   
            {
                pf = M2 * N2 * b2 % (n1 * n2);
                if (pf < A)
                {
                    grBack.DrawLine(p2, x0 + b2 * scale, y0, x0 + b2 * scale + n1 * scale, hh * scale + y0);
                    pf = pf / n1;
                    glbl_faze[n1 + b2] = pf;
                    s = pf.ToString(); grBack.DrawString(s, font, new SolidBrush(Color.Black), x0 + b2 * scale, y0 - 4 * scale, drawFormat);
                }
            }
            for (int b1 = 0; b1 < n1; b1++)
            {
                pf = M1 * N1 * b1 % (n1 * n2);
                if (pf < A)
                {
                    grBack.DrawLine(p3, x0, y0 + b1 * scale, x0 + n1 * scale - b1 * scale, hh * scale + y0);
                    pf = pf / n1;
                    glbl_faze[n1 - b1] = pf;
                    s = pf.ToString(); grBack.DrawString(s, font, new SolidBrush(Color.Black), x0 - 10 * scale, y0 + b1 * scale, drawFormat);
                }
            }

            for (int i = 0; i < n1 + n2; i++)
            {
                int bb = glbl_faze[i];
                if (bb >= 0) { s = bb.ToString(); grBack.DrawString(s, font, new SolidBrush(Color.Black), x0 + i * scale, y0 + n1 * scale + 8 * scale, drawFormat); }
            }

            int mxx = 0, mxx_x = 0, mnx = 0, mnx_x = 0, cntr = 0;
            for (; ; )
            {
                for (int i = mnx_x; i < n1 + n2; i++)
                {
                    cntr = i;
                    int bb = glbl_faze[i]; if (bb >= 0 && bb != mnx) { mxx = bb; mxx_x = i; break; }
                }
                if (cntr >= n1 + n2 - 1) break;
                int m = (mxx_x - mnx_x) / 2;
                for (int j = mnx_x; j < mnx_x + m; j++) glbl_faze1[j] = mnx;
                for (int j = mnx_x + m; j < mxx_x; j++) glbl_faze1[j] = mxx;
                mnx_x = mxx_x;
                mnx = mxx;

            }


            mnx = glbl_faze1[0];
            for (int i = 0; i < n1 + n2; i++)
            {
                int bb = glbl_faze1[i];
                if (bb != mnx)
                {
                    mnx = bb;
                    grBack.DrawLine(p4, x0 + i * scale, y0 + hh * scale, x0, y0 + hh * scale - i * scale);
                }
            }

            //--------------------------------------------------------------------------------------------------------------- 
            int r, g, b;
            int w = img[0].Width;
            int h = img[0].Height;
            Bitmap bmp1 = (Bitmap)img[0];
            Bitmap bmp2 = (Bitmap)img[1];
            Bitmap bmp3 = (Bitmap)img[2];
            Bitmap bmp = (Bitmap)pictureBox01.Image;

            BitmapData bmp1Data = ImageProcessor.getBitmapData(bmp1);
            BitmapData bmp2Data = ImageProcessor.getBitmapData(bmp2);

            int[,] bmp_r = new int[n2 + 3, n1 + 3];
            int[,] bmp_line = new int[n2 + 3, n1 + 3];
            int[] ims1 = new int[h];
            int[] ims2 = new int[h];
            int[] ims3 = new int[h];

            Color c, c1;

            int xx0 = 0, yy0 = 0;
            int xx1 = w, yy1 = h;

            if ((x0_end < x1_end) && (y0_end < y1_end)) { xx0 = x0_end; xx1 = x1_end; yy0 = y0_end; yy1 = y1_end; }

            Int32 count = 0;
            if (rb == 1)
            {// ------------- По фигуре из 3 квадрата
                for (int i = xx0; i < xx1; i++)
                {

                    for (int j = yy0; j < yy1; j++)
                    {
                        c1 = bmp3.GetPixel(i, j);
                        if (c1.R == 0) ims3[j] = 0;
                        else
                        {
                            ims3[j] = 1;
                            //c = bmp1.GetPixel(i, j); 
                            c = ImageProcessor.getPixel(i, j, bmp1Data);
                            r = c.R; 
                            ims1[j] = (int)((double)(r * (n1 - 1)) / 255);  //(b2)

                            //c = bmp2.GetPixel(i, j); 
                            c = ImageProcessor.getPixel(i, j, bmp2Data);
                            r = c.R; 
                            ims2[j] = (int)((double)(r * (n2 - 1)) / 255);  //(b1) 
                        }

                    }

                    for (int j = yy0; j < yy1; j++)
                    {
                        if (ims3[j] != 0) { r = ims1[j]; g = ims2[j]; bmp_r[g, r]++; count++; }
                    }
                }
            }


            if (rb == 0)
            {// --------- По квадратной области
                for (int i = xx0; i < xx1; i++)
                {

                    for (int j = yy0; j < yy1; j++)
                    {
                        //c = bmp1.GetPixel(i, j);
                        c = ImageProcessor.getPixel(i, j, bmp1Data);
                        r = c.R; 
                        ims1[j] = (int)((double)(r * (n1 - 1)) / 255);  //(b2)

                        //c = bmp2.GetPixel(i, j);
                        c = ImageProcessor.getPixel(i, j, bmp2Data);
                        r = c.R; 
                        ims2[j] = (int)((double)(r * (n2 - 1)) / 255);  //(b1)   
                    }

                    for (int j = yy0; j < yy1; j++)
                    {
                        r = ims1[j]; g = ims2[j]; bmp_r[g, r]++; count++;
                    }
                }
            }

            (bmp1).UnlockBits(bmp1Data);
            (bmp2).UnlockBits(bmp2Data);



            Int32 ib2 = 0, ib1 = 0, max_count = 0;
            for (ib2 = 0; ib2 < n2 - 1; ib2++)
            {
                for (ib1 = 0; ib1 < n1 - 1; ib1++) { b = bmp_r[ib2, ib1]; if (b > max_count) max_count = b; }
            }

            int mn1 = pr_obr;

            int mn = (max_count) / 12;
            int mn2 = mn1 + mn;
            int mn3 = mn2 + mn;

            pc1.Refresh();
            f_sin.Show();





            Color currentFirstColor;
            Color currentSecondColor;


            BitmapData data1 = ImageProcessor.getBitmapData((Bitmap)firstImage.Image);
            BitmapData data2 = ImageProcessor.getBitmapData((Bitmap)secondImage.Image);

            BitmapData resultData = ImageProcessor.getBitmapData((Bitmap)pc1.Image);

            int firstCoordinate = 0;
            int secondCoordinate = 0;
            Color currentColor;
            int currentColorIntence = 0;

            for (int i = 0; i < firstImage.Image.Width; i++)
            {
                for (int j = 0; j < firstImage.Image.Height; j++)
                {
                    currentFirstColor = ImageProcessor.getPixel(i, j, data1);
                    currentSecondColor = ImageProcessor.getPixel(i, j, data2);

                    firstCoordinate = (int)((float)currentFirstColor.R * 241.0 / (float)255) * scale;
                    secondCoordinate = (int)((float)currentSecondColor.R * 167.0 / (float)255) * scale;

                    currentColor = ImageProcessor.getPixel(x0 + firstCoordinate, y0 + secondCoordinate, resultData);

                    if (currentColor.ToArgb() == Color.White.ToArgb())
                    {
                        currentColor = Color.Black;
                    }

                    currentColorIntence = currentColor.R;
                    currentColorIntence++;

                    if (currentColorIntence > 255)
                    {
                        currentColorIntence = 255;
                    }

                    ImageProcessor.setPixel(resultData, x0 + firstCoordinate, y0 + secondCoordinate, Color.FromArgb(currentColorIntence, 0, 0));
                }
            }

            ((Bitmap)firstImage.Image).UnlockBits(data1);
            ((Bitmap)secondImage.Image).UnlockBits(data2);

            ((Bitmap)pc1.Image).UnlockBits(resultData);


            pc1.Refresh();
            f_sin.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// ----------------------------------------------------------------------------------------------------------------------------------------------------------------          
// ---------------------------------------------         Определение коэффициентов плоскости  z[i,j] = A*i + B*j +C   методом наименьших квадратов     
// ----------------------------------------------------------------------------------------------------------------------------------------------------------------  

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public class Plane
        {
            public double a;
            public double b;
            public double c;
            public double d;
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public Plane()
            {
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public Plane(double newA, double newB, double newC)
            {
                a = newA;
                b = newB;
                c = newC;
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public Plane(double newA, double newB, double newC, double newD)
            {
                a = newA;
                b = newB;
                c = newC;
                d = newD;
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Plane getPlaneParams(Int32[,] Z, int xx0, int xx1, int yy0, int yy1)                                                  // По строке
        {
            double width = xx1 - xx0;
            double height = yy1 - yy0;

            double a1 = (width * (height - 1) * (2 * (height - 1) * (height - 1) + 3 * height - 2)) / 6;
            double b1 = (width * (width - 1) * height * (height - 1)) / 4;
            double b2 = (height * (width - 1) * (2 * (width - 1) * (width - 1) + 3 * width - 2)) / 6;

            double c1 = (width * height * (height - 1)) / 2;
            double c2 = (width * height * (width - 1)) / 2;
            double c3 = width * height;

            double d1 = 0, d2 = 0, d3 = 0;
            for (int j = yy0; j < yy1; j++)
            {
                for (int i = xx0; i < xx1; i++)
                {
                    d1 += (j * Z[i, j]);
                    d2 += (i * Z[i, j]);
                    d3 += Z[i, j];
                }
            }

            double k1 = -b1 / a1;
            double k2 = -c1 / a1;
            double k3 = -(c2 + k2 * b1) / (b2 + k1 * b1);

            Plane result = new Plane();

            result.c = (d3 + k2 * d1 + k3 * (d2 + k1 * d1)) / (c3 + k2 * c1 + k3 * (c2 + k1 * c1));
            result.b = (d2 + k1 * d1 - (c2 + k1 * c1) * C) / (b2 + k1 * b1);
            result.a = (d1 - b1 * B - c1 * C) / a1;

            //result.b = (d3 + k2 * d1 + k3 * (d2 + k1 * d1)) / (c3 + k2 * c1 + k3 * (c2 + k1 * c1));
            //result.a = (d2 + k1 * d1 - (c2 + k1 * c1) * C) / (b2 + k1 * b1);
            //result.c = (d1 - b1 * B - c1 * C) / a1;

            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ABC(Int32 [,] Z,  int xx0, int xx1, int yy0, int yy1)                                                  // По строке
        {
            double M = xx1-xx0;
            double N = yy1-yy0;
            //MessageBox.Show(" N = " + N.ToString() + " M =  " + M.ToString() );
            double a1 = (M * (N - 1)*(2 * (N - 1) * (N - 1) + 3 * N - 2)) / 6;
            double b1 = (M * (M - 1) * N * (N - 1)) / 4;
            double b2 = (N*(M-1)*(2*(M-1)*(M-1)+3*M-2) )/6;
           
            double c1 = (M * N * (N - 1)) / 2;
            double c2 = (M * N * (M - 1)) / 2;
            double c3 = M * N;
            //MessageBox.Show(" c1 = " + c1.ToString() + " c2 =  " + c2.ToString() + " c3 =  " + c3.ToString() + " b2 =  " + b2.ToString());
            double d1 = 0, d2 = 0, d3 = 0;
            for (int j = yy0; j < yy1; j++) for (int i = xx0; i < xx1; i++) { d1 += (j * Z[i, j]); d2 += (i * Z[i, j]); d3 += Z[i, j]; }
            //MessageBox.Show(" d1 = " + d1.ToString() + " d2 =  " + d2.ToString() + " d3 =  " + d3.ToString());
            double k1 = - b1 / a1;
            double k2 = - c1 / a1;
            double k3 = - (c2+k2*b1 )/ ( b2+k1*b1);
            //MessageBox.Show(" k1 = " + k1.ToString() + " k2 =  " + k2.ToString() + " k3 =  " + k3.ToString());
            C = (d3+k2*d1+k3*(d2+k1*d1) )/(c3+k2*c1+k3*(c2+k1*c1) );
            B = (d2+k1*d1-(c2+k1*c1)*C)/(b2+k1*b1);
            A = (d1 - b1 * B - c1 * C) / a1;
            
        }

        // -----------------------------------------------------------------------------------------------------------------------------------
        static void Z_bmp( Bitmap bmp, Int32[,] Z, int w, int h)               // -------------------------- Z -> BMP
        {
            Int32 b2_min = Z[ 1,  1], b2_max = Z[ 1,  1];
            int b2;
            for (int i = 0; i < w; i++) for (int j = 0; j < h; j++) { b2_max = Math.Max(b2_max, Z[i, j]); b2_min = Math.Min(b2_min, Z[i, j]); }
            
            MessageBox.Show(" Max = " + b2_max.ToString() + " Min =  " + b2_min.ToString());
            b2_max = b2_max - b2_min;             
            if (b2_max == 0) return; 
           
            for (int i = 0; i < w; i++)                                                                   //  Отображение точек на pictureBox01
            {
                for (int j = 0; j < h; j++)
                {
                    b2 = (Z[i, j] - b2_min) * 255 / b2_max;
                    if (b2 < 0 || b2 > 255) b2 = 0;
                    bmp.SetPixel(i, j, Color.FromArgb(b2, b2, b2));

                }
            }
        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------        
        // -----------------------------------------------------------------------------------------------------------------------------------           
        // -----------------------------------         Расшифровка            ----------------------------------------------------------------          
        // -----------------------------------------------------------------------------------------------------------------------------------          
        private static void GLBL_FAZE(int Diag)
        {
           
            for (int i = 0; i < n1 + n2; i++) { glbl_faze[i] = -1; glbl_faze1[i] = -1; }                       // Массив для расшифровки

            Int32 A = Diag * Math.Max(n1, n2);
            Int32 pf;
            for (int b2 = 0; b2 < n2; b2++)                                                                    // Диагонали   
            {
                pf = M2 * N2 * b2 % (n1 * n2);
                if (pf < A) { pf = pf / n1; glbl_faze[n1 + b2] = pf; }
            }
            for (int b1 = 0; b1 < n1; b1++)
            {
                pf = M1 * N1 * b1 % (n1 * n2);
                if (pf < A) { pf = pf / n1; glbl_faze[n1 - b1] = pf; }
            }
            int mxx = 0, mxx_x = 0, mnx = 0, mnx_x = 0, cntr = 0;
            for (; ; )
            {
                for (int i = mnx_x; i < n1 + n2; i++)
                {
                    cntr = i;
                    int bb = glbl_faze[i]; if (bb >= 0 && bb != mnx) { mxx = bb; mxx_x = i; break; }
                }
                if (cntr >= n1 + n2 - 1) break;
                //MessageBox.Show(" mnx =  " + mnx.ToString() + " mxx =  " + mxx.ToString());                    
                int m = (mxx_x - mnx_x) / 2;
                for (int j = mnx_x; j < mnx_x + m; j++) glbl_faze1[j] = mnx;
                for (int j = mnx_x + m; j < mxx_x; j++) glbl_faze1[j] = mxx;
                mnx_x = mxx_x;
                mnx = mxx;

            }
           

        }

        private static void rash_2pi(Bitmap bmp, Bitmap bmp1, Bitmap bmp2, int w, int h, int Diag)
        {
            GLBL_FAZE(Diag);                         // Заполнение массива glbl_faze[] для расшифровки
            int b,  ib1, ib2;
           
            Color c;                                                 
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    c = bmp1.GetPixel(i, j); b = c.R; ib1 = (int)((double)(b * (n1 - 1)) / 255); // --------------------- (b2)
                    c = bmp2.GetPixel(i, j); b = c.R; ib2 = (int)((double)(b * (n2 - 1)) / 255); // --------------------- (b1)              
                    b = glbl_faze1[ib2 + (n1 - ib1)];
                    Z[i, j] = b  * n1 - (n1 - ib1);
                }
            }
    
           
        }     
        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------          
        // ---------------------------------------------                   Прослеживание линий              ---------------------------------------------------------------        
        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------  
        static int x1, x2;                                                            // Затравочная область для следующей строки
        static int y1, y2;                                                            // Границы горизонтальные

        private static int Num_Line(int[,] bmp_r, int[,] bmp_line, Graphics grBack)                                                  // По строке
        {
            int w1 = n2, hh = n1;
            int[] type_line1 = new int[200];
            int[] type_line = new int[200];       // Тип линии ( Начало на левой границе - 2;  Начало на верхней - 1)
            Pen p1 = new Pen(Color.Black, 1);
            //------------------------------------------------------------------------------------------------------------- 1 линия
            int y00 = 0;
            int cntr = 0, b = 0;
            int ii = 1;                // Номер линии
            int ib1 = 0, ib2 = 0;
            x1 = 0; x2 = 0;            // Затравочные границы
            for (ib2 = 0; ib2 < n2 - 1; ib2++) { b = bmp_r[ib2, ib1]; if (b > 0) break; } x1 = ib2;
            for (ib2 = x1; ib2 < n2 - 1; ib2++) { b = bmp_r[ib2, ib1]; if (b == 0) break; } x2 = ib2 - 1;
            y00 = 0;
            y1 = 0; y2 = 1;

            type_line[100] = 1;
            for (ii = 1; ii < 80; ii++)
            {
                cntr = 0; for (ib1 = y1; ib1 < y2; ib1++) if (Num(ib1, ii, bmp_r, bmp_line) != 0) { cntr = 1; y00 = ib1; break; }
                if (cntr == 0) break; // Нет начальных точек

                for (ib1 = y00; ib1 < n1 - 1; ib1++) if (Num(ib1, ii, bmp_r, bmp_line) == 0) break;  // Не найдено в строке ни одной точки

                cntr = Num_N(ii, bmp_line);  // Пересечение с границами
                if (cntr == 0)
                {
                    //MessageBox.Show(" Конец----------------- " + ii.ToString());
                    //type_line[100 + ii] = ii;
                    break;
                }    // Нет пресечения с границами (Конечная точка)
                if (cntr == 2)
                {
                    //MessageBox.Show(" Правая y1: " + y1.ToString() + " y2: " + y2.ToString()+  " ii: "+ii.ToString());
                    grBack.DrawLine(p1, x0, y0 + y1 * scale, x0 + w1 * scale, y0 + y1 * scale);
                    grBack.DrawLine(p1, x0, y0 + y2 * scale, x0 + w1 * scale, y0 + y2 * scale);
                    x1 = 0; x2 = 0;
                    type_line[100 + ii + 1] = 2;
                    continue;
                }

                if (cntr == 1)
                {
                    //MessageBox.Show(" Нижняя x1: " + x1.ToString() + " x2: " + x2.ToString()+  " ii: "+ii.ToString());

                    grBack.DrawLine(p1, x0 + x1 * scale, y0, x0 + x1 * scale, hh * scale + y0);
                    grBack.DrawLine(p1, x0 + x2 * scale, y0, x0 + x2 * scale, hh * scale + y0);
                    y1 = 0; y2 = 2;
                    type_line[100 + ii + 1] = 1;
                    continue;
                }

            }

            //----------------------------------------------------------------------------------------------------------------------------------- Начальные линии
            ib1 = 0;
            x1 = 0; x2 = 0;            // Затравочные границы
            for (ib2 = 0; ib2 < n2 - 1; ib2++) { b = bmp_r[ib2, ib1]; if (b > 0) break; }
            x1 = ib2;
            for (ib2 = x1; ib2 < n2 - 1; ib2++) { b = bmp_r[ib2, ib1]; if (b == 0) break; }
            x2 = ib2 - 1;
            //grBack.DrawLine(p1, x0 + x1 * scale, y0, x0 + x1 * scale, hh * scale + y0);
            //grBack.DrawLine(p1, x0 + x2 * scale, y0, x0 + x2 * scale, hh * scale + y0);
            y1 = n1 - 2; y2 = n1 - 1;
            type_line[99] = 1;
            for (ii = -1; ii > -100; ii--)
            {
                cntr = 0; for (ib1 = y1; ib1 < y2; ib1++) if (Num(ib1, ii, bmp_r, bmp_line) != 0) { cntr = 1; y00 = ib1 - 1; break; }

                if (cntr != 0)
                {
                    for (ib1 = y00; ib1 >= 0; ib1--) if (Num(ib1, ii, bmp_r, bmp_line) == 0) break;   // Не найдено в строке ни одной точки

                    cntr = Num_N2(ii, bmp_line);                   // Поиск границ пересечения
                    if (cntr == 2)                                 // Пересечение с левой границей
                    {
                        //MessageBox.Show(" Левая " + y1.ToString() + " x2: " + y2.ToString() + " i " + ii.ToString());
                        grBack.DrawLine(p1, x0, y0 + y1 * scale, x0 + w1 * scale, y0 + y1 * scale);
                        grBack.DrawLine(p1, x0, y0 + y2 * scale, x0 + w1 * scale, y0 + y2 * scale);
                        x1 = n2 - 3; x2 = n2 - 1;
                        //y1 = y1 - 1; y2 = y2 + 1;
                        type_line[100 + ii] = 2;
                        continue;
                    }

                    if (cntr == 1)                               // Пересечение с верхней границей
                    {
                        //MessageBox.Show(" Верхняя x1: " + x1.ToString() + " x2: " + x2.ToString());

                        grBack.DrawLine(p1, x0 + x1 * scale, y0, x0 + x1 * scale, hh * scale + y0);
                        grBack.DrawLine(p1, x0 + x2 * scale, y0, x0 + x2 * scale, hh * scale + y0);
                        y1 = n1 - 2; y2 = n1 - 1;
                        type_line[100 + ii] = 1;
                        continue;
                    }
                    if (cntr == 0)
                    {
                        type_line[100 + ii] = 1;
                        break;
                    }
                }
            }

            //MessageBox.Show(" Начало " + ii.ToString());

            // ------------------------------------------------------------------------------------------------------------------------- Нумерация полос
            int ii1 = -ii;
            int ii_max = 1;                                                // ---------------------------------------------------------- Максимальное количество полос
            for (ib2 = 0; ib2 < n2; ib2++)
                for (ib1 = 0; ib1 < n1; ib1++)
                {
                    b = bmp_line[ib2, ib1];
                    if (b < 0) { b = b - ii + 1; bmp_line[ib2, ib1] = b; continue; }
                    if (b > 0) { b = b - ii; if (b >= ii_max) ii_max = b; bmp_line[ib2, ib1] = b; continue; }

                }
            for (int i = 0; i < ii_max; i++)
            {
                int k;
                if (i <= ii1) k = type_line[i + 100 + ii]; else k = type_line[i + 100 + ii + 1];
                type_line1[i] = k;
            }

            //int[] number_2pi = new int[ii_max + 1];
            for (int i = 1; i <= ii_max; i++) number_2pi[i] = i;
            int nmb_2pi = 0;
            for (int i = 1; i <= ii_max; i++)
            {
                if (type_line1[i - 1] == 1) nmb_2pi--;
                number_2pi[i] = nmb_2pi + ii_max - 2;
            }

            //    MessageBox.Show(" Всего " + ii_max.ToString() + " полос ");

            // -------------------------------------------------------------------------------------------Заполнение глобальных маccивов glbl_faze[n1+n2], glbl_faze1[n1+n2]
            for (int j = 0; j < n2+n1; j++) { glbl_faze[j] = 0; glbl_faze1[j] = 0; }

            for (ib2 = 0; ib2 < n2; ib2++)
                for (ib1 = 0; ib1 < n1; ib1++)
                { b = bmp_line[ib2, ib1]; if (b > 0) { ii1 = ib2 + (n1 - ib1); glbl_faze[ii1] = b; glbl_faze1[ii1] = number_2pi[b]; } }


            return ii_max;
        }

// ----------------------------------------------------------------------------------------------------------------------  Расширение границ  ------------------------------  
        private static void Rash_glbl_faze()       
            {
                int b;
                int ix0 = 0, ix1 = n2 + n1 - 1;      // ----------------------------------------------------------------------------------------  1 промежуток и последний
                int ibx0 = 0, ibx1 = 0;
                int max;

                for (int i = 0; i < n2 + n1; i++)     { b = glbl_faze[i]; ix0 = i; if (b != 0) break; } ibx0 = glbl_faze[ix0];
                for (int i = n2 + n1 - 1; i > 0; i--) { b = glbl_faze[i]; ix1 = i; if (b != 0) break; } ibx1 = glbl_faze[ix1];
                max = (ix0 + (n2 + n1 - 1) - ix1) / 2; // Длина отрезка
                
                for (int i = ix0 - max; i < ix0; i++) glbl_faze[i] = ibx0;
                for (int i = 0; i < ix0 - max; i++)   glbl_faze[i] = ibx1;
           
                                if (max < ix0)
                                {
                                    for (int i = ix0 - max; i < ix0; i++) glbl_faze[i] = ibx0;
                                    for (int i = 0; i < ix0 - max; i++)   glbl_faze[i] = ibx1;
                                }
                                else
                                { 
                                    for (int i = 0; i < ix0; i++)         glbl_faze[i] = ibx0; 
                                }

                                if (max < ((n2 + n1 - 1) - ix1))
                                {
                                    for (int i = ix1; i < n2 + n1 - max; i++)     glbl_faze[i] = ibx1;
                                    for (int i = n2 + n1 - max; i < n2 + n1; i++) glbl_faze[i] = ibx0;
                                }
                                else { for (int i = ix1; i < n2 + n1; i++)    glbl_faze[i] = ibx1; }
              
                // Следующие промежутки

                int i0 = ix0; int cntr0, cntr1;
                for (; ; )
                {
                    cntr0 = 0; for (int i = i0; i < n2 + n1; i++) { b = glbl_faze[i]; if (b == 0) { cntr0 = 1; ix0 = i; ibx0 = glbl_faze[i - 1]; break; } }
                    if (cntr0 == 0) break;                                                              // { MessageBox.Show(" Не найден 0 "); break; }
                    cntr1 = 0; for (int i = ix0 + 1; i < n2 + n1; i++) { b = glbl_faze[i]; if (b != 0) { cntr1 = 1; ix1 = i; ibx1 = glbl_faze[i]; break; } }
                    if (cntr1 == 0) break;                                                              //{ MessageBox.Show(" Не найден 1 "); break; }
                    max = (ix1 - ix0) / 2; // Длина отрезка
                    for (int i = ix0; i < ix0 + max; i++) glbl_faze[i] = ibx0;
                    for (int i = ix0 + max; i < ix1; i++) glbl_faze[i] = ibx1;
                    i0 = ix1;

                }
 
                for (int i = 0; i < n2 + n1; i++) { b = glbl_faze[i]; glbl_faze1[i] = number_2pi[b]; }


        }

 //----------------------------------------------------------------------------------------------------------------------------------       
 //----------------------------------------------------------------------------------------------------------------------------------
 // ---------------------------------------------------------------------------------------------------------------------------------

        private static int Num(int ib1, int n, int[,] bmp_r, int[,]  bmp_line)                                                  // По строке
        {
            int b;
            
            int x20 = x2, x10 = x1;
            int x21 = x2, x11 = x1;
            int cntr1 = 0; for (int i = x1; i <= x2; i++)   { b = bmp_r[i, ib1]; if (b > 0) { x10 = i; cntr1 = 1; break; } }  if (cntr1 == 1) x1 = x10;
            int cntr2 = 0; for (int i = x1; i >= 0; i--)    { b = bmp_r[i, ib1]; if (b > 0) { x11 = i; cntr2 = 1; } else { if (Math.Abs(x1 - i) < 4) continue; else break; } } if (cntr2 == 1) x1 = x11;
            int cntr3 = 0; for (int i = x2; i >= x1; i--)   { b = bmp_r[i, ib1]; if (b > 0) { x20 = i; cntr3 = 1; break; } }  if (cntr3 == 1) x2 = x20;                               
            int cntr4 = 0; for (int i = x2; i < n2-1; i++)  { b = bmp_r[i, ib1]; if (b > 0) { x20 = i; cntr4 = 1; } else { if (Math.Abs(x1 - i) < 4) continue; else break; }; } if (cntr4 == 1) x2 = x20;
            for (int i = x1; i <= x2; i++) bmp_line[i, ib1] = n;
            if (cntr1 == 1 || cntr2 == 1 || cntr3 == 1 || cntr4 == 1)  return (1);  else  return 0;     
            
        }

        // -----------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------- Поиск пересечения с нижней и правой границей
        private static int Num_N(int n, int[,] bmp_line)                                                 
        {   
        int cntry = 0;                               // ----------------------------------- Поиск пересечения с правой  границей
            y1 = n1-1; y2 = 0;
            for (int ib1 = 0; ib1 < n1-1; ib1++)
            {
                int b1 = bmp_line[n2 - 1, ib1];
                int b2 = bmp_line[n2 - 2, ib1];
                int b3 = bmp_line[n2 - 3, ib1];
                if (b1 == n || b2==n || b3==n )
                {
                    if (ib1 > y2) y2 = ib1;
                    if (ib1 < y1) y1 = ib1;
                    cntry = 1;
                }
            }
            y2++;
            if (cntry == 1) return 2;               // -------------------------------- Пересечениe с правой горизонтальной границей
            // --------------------------------------------------------------------------------- Поиск пересечения с нижней   границей
            int cntrx = 0;
            x1 = n2 - 1; x2 = 0;
            for (int ib2 = 0; ib2 < n2 - 1; ib2++)
            {
                
                int b1 = bmp_line[ib2, n1 - 1];
                int b2 = bmp_line[ib2, n1 - 2];
                int b3 = bmp_line[ib2, n1 - 3];
                if (b1 == n || b2 == n || b3 == n)
                {
                    if (ib2 > x2) x2 = ib2;
                    if (ib2 < x1) x1 = ib2;
                    cntrx = 1;
                }
            }
            x2++;
            if (cntrx == 1) return 1;   // -------------------------------- Поиск пересечения с нижней вертикальной границей
           
 
            return 0;              
        }
        //--------------------------------------------------------------------------------- Для начальных точек
        private static int Num_N2(int n, int[,] bmp_line)                                                 
        {
            int b;
            // ------------------------------------------------------------------------------------- Поиск пересечения с левой вертикальной границей (y1,y1)                        
            int cntry = 0;
            y1 = n1 - 1; y2 = 0;
            for (int ib1 = 0; ib1 < n1 - 1; ib1++) 
            {
                b = bmp_line[0, ib1];
                if (b == n)
                {
                    if (ib1 > y2) y2 = ib1;
                    if (ib1 < y1) y1 = ib1;
                    cntry = 1;
                }
            }
            y2++;
            // ------------------------------------------------------------------------------------ Поиск пересечения с верхней горизонтальной границей (x1,x2)
            int cntrx = 0;
            x1 = n2 - 1; x2 = 0;
            for (int ib2 = 0; ib2 < n2 - 1; ib2++)
            {
                b = bmp_line[ib2, 0];
                if (b == n)
                {
                    if (ib2 > x2) x2 = ib2;
                    if (ib2 < x1) x1 = ib2;
                    cntrx = 1;
                }
            }
            x2++;
            if (cntrx == 1) return 1;  // пересечение с верхней границей
            if (cntry == 1) return 2;  // пересечение с левой границей

            return 0;
        }
        
// --------------------------------------------------------------------------------------------------------------------------------        
// --------------------------------------------------------------------------------------------------------------------------------        
// --------------------------------------------------------------------------------------------------------------------------------                
// --------------------------------------------------------------------------------------------------------------------------------                
// --------------------------------------------------------------------------------------------------------------------------------                
// --------------------------------------------------------------------------------------------------------------------------------
        public static void pi2_frml(Image[] img, PictureBox pictureBox01, string sN1, string sN2, int Diag, Point p, int x0_end, int x1_end, int y0_end, int y1_end,int rb, int pr_obr)
        {
            China(sN1, sN2);           // Вычисление формулы
            Graph_China(img, pictureBox01, Diag, p,  x0_end, x1_end, y0_end, y1_end, rb, pr_obr);                          // Построение таблицы
        }
        public static void pi2_frml2(Image[] img, PictureBox pictureBox01, string sN1, string sN2, int Diag, Point p, int x0_end, int x1_end, int y0_end, int y1_end, int rb, int pr_obr, PictureBox firstImage, PictureBox secondImage)
        {
            China(sN1, sN2);           // Вычисление формулы
            Graph_China2(img, pictureBox01, Diag, p, x0_end, x1_end, y0_end, y1_end, rb, pr_obr, firstImage, secondImage);                          // Построение таблицы
        }
        public static void pi2_prmtr(Image[] img, string sN1, string sN2, int Diag, Point p)                                             // Идеальная таблица с диагоналями
        {
            China(sN1, sN2);              // Вычисление формулы          
            Graph_Prmtr(img, Diag, p);    // Таблица
              
        }
//-----------------------------------------------------------------------------------------------------------------------------------
        

        public static void pi2_rshfr(Image[] img, PictureBox pictureBox01, int xx0, int xx1, int yy0, int yy1, string sN1, string sN2, int Diag) // Расшифровка
        {
            China(sN1, sN2);                                           // Вычисление формулы 
            int w = img[0].Width;
            int h = img[0].Height;
            Bitmap bmp1 = new Bitmap(img[0], w, h);
            Bitmap bmp2 = new Bitmap(img[1], w, h);
            Bitmap bmp = new Bitmap(pictureBox01.Image, w, h);
            Z = new int[w, h];
            rash_2pi(bmp, bmp1, bmp2, w, h,  Diag);     //  РАСШИФРОВКА (Заполнение Z[,])
            Z_bmp(bmp, Z, w, h);                  //  Z -> bmp с масштабированием
            pictureBox01.Size = new System.Drawing.Size(w, h);
            pictureBox01.Image = bmp;
        }

        public static void pi2_ABC( PictureBox pictureBox01, int xx0, int xx1, int yy0, int yy1) // Устранение тренда по методу наименьших квадратов
        {

            int w = pictureBox01.Width;
            int h = pictureBox01.Height;
            Z = new int[w, h];                //------------------------------- УБРАТЬ
            Color c;
            Bitmap bmp = new Bitmap(pictureBox01.Image, w, h);
            for (int i = 0; i < w; i++) for (int j = 0; j < h; j++) { c = bmp.GetPixel(i, j);  Z[i, j] = c.R; }
            ABC(Z, xx0, xx1, yy0, yy1);
            MessageBox.Show(" A "+ A.ToString() + " B " + B.ToString() + " C " + C.ToString());
            for (int i = 0; i < w; i++) for (int j = 0; j < h; j++) Z[i, j] = Z[i, j] - Convert.ToInt32(A * i + B * j + C);           

            Z_bmp(bmp, Z, w, h);                                                                          //  Z -> bmp с масштабированием
            pictureBox01.Size = new System.Drawing.Size(w, h);
            pictureBox01.Image = bmp;
        }
 // ------------------------------------------------------------------------------------------------------------------- Вычитание плоскости, проходящей через 3 точки
        public static void NKL(PictureBox pictureBox01, int x1, int y1, int x2, int y2, int x3, int y3)
        {   
            
            double ax=x2-x1, ay=y2-y1, az=Z[x2,y2]-Z[x1,y1];
            double bx=x3-x1, by=y3-y1, bz=Z[x3,y3]-Z[x1,y1];
                A = ay*bz-az*by;
                B = -(ax*bz-bx*az);
                C = ax*by-ay*bx;
                D = -(A*x1 + B*y1 + C*Z[x1,y1]);               
                A = A / C; B = B / C; D = D / C;
                MessageBox.Show(" A " + A.ToString() + " B " + B.ToString() + " C " + C.ToString() + " D " + D.ToString());
                int w = pictureBox01.Width;
                int h = pictureBox01.Height;

                for (int i = 0; i < w; i++) for (int j = 0; j < h; j++) Z[i, j] =Z[i, j] + Convert.ToInt32(A * i + B * j + D);
                Bitmap bmp = new Bitmap(pictureBox01.Image, w, h);
                Z_bmp(bmp, Z, w, h);                                                                          //  Z -> bmp с масштабированием
                pictureBox01.Size = new System.Drawing.Size(w, h);
                pictureBox01.Image = bmp;
        }




    }
}
