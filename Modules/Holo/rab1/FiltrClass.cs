using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.Drawing;
using System.Windows.Forms;
using ClassLibrary;

namespace rab1
{
    public class FiltrClass
    {

/// <summary>
/// Добавление нулей между значениями 01010101
/// </summary>
/// <param name="amp"></param>
/// <param name="k"></param>
/// <returns></returns>
        public static ZArrayDescriptor Decim01(ZArrayDescriptor amp, int  k)
        {
           
            int w1 = amp.width;
            int h1 = amp.height;
            k++;
            int w2 = k * w1;
            int h2 = h1;
            MessageBox.Show(" 01 k = " + k  + " 01 w2 = " + w2);
            ZArrayDescriptor res_array = new ZArrayDescriptor(w2, h1);

            for (int i = 0; i < w2-1; i += k)
               for (int j = 0; j < h1; j++)
                   {  res_array.array[i+1, j] = amp.array[i/k, j]; }
            return res_array;

        }

        public static ZArrayDescriptor Decim10(ZArrayDescriptor amp, int k)
        {
           
            int w1 = amp.width;
            int h1 = amp.height;
            k++;
            int w2 = k * w1;
            int h2 = h1;
            MessageBox.Show(" 10 k = " + k + " w2 = " + w2);
            ZArrayDescriptor res_array = new ZArrayDescriptor(w2, h1);

            for (int i = 0; i < w2; i+=k)
                for (int j = 0; j < h1; j++)
                 { res_array.array[i , j] = amp.array[i/k, j]; }
            return res_array;

        }
        public static ZArrayDescriptor Decim1(ZArrayDescriptor amp, int k)
        {
            int w1 = amp.width;
            int h1 = amp.height;
            k++;
            int w2 = k * w1;
            int h2 = h1;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w2, h2);

            for (int i = 0; i < w2; i ++)
                for (int j = 0; j < h2; j++)
                {
                    res_array.array[i, j] = amp.array[i / k, j];
                }
            return res_array;

        }
        //  Увеличение файла со сдвигом на половину пикселя

        public static double[] set_1_2(double[] ar1)
        {
            int h = ar1.Length;

             int h2 = h * 2;
           // int k1 = h * 2;
            double[] ar = new double[h2];

            for (int i = 1, j = 0; i < h - 1; i++, j += 2)
            {
                ar[j] = (ar1[i-1] + ar1[i]) / 2;
                ar[j + 1] = (ar1[i] + ar1[i + 1]) / 2;
            }

            return ar;
        }


        public static ZArrayDescriptor Filt_2_1_2(ZArrayDescriptor amp)   // int kx, int ky - сдвиг массива точек
        {

            int w1 = amp.width;
            int h1 = amp.height;
            int w2 = 2*w1;
            int h2 = 2*h1;
           
            ZArrayDescriptor tmp_array = new ZArrayDescriptor(w2, h2);
            ZArrayDescriptor res_array = new ZArrayDescriptor(w2, h2);
            double[] ar1;
            double[] ar2; 

            for (int i = 0; i < w1; i++)
            {
                ar1 = new double[h1];
                ar2 = new double[h2];
                for (int j = 0; j < h1; j++) { ar1[j] = amp.array[i, j]; }
                ar2 = set_1_2(ar1);

                for (int j = 0; j < h2; j++) { tmp_array.array[i, j] = ar2[j]; }
            }

            for (int j = 0; j < h2; j++)
            {
                ar1 = new double[w1];
                ar2 = new double[w2];
                for (int i = 0; i < w1; i++) { ar1[i] = tmp_array.array[i, j]; }

                ar2 = set_1_2(ar1);

                for (int i = 0; i < w2; i++) { res_array.array[i, j] = ar2[i]; }
            }

            return res_array;
        }

/// <summary>
/// Увеличение массива в 2 раза с добавлением рамки
/// </summary>
/// <param name="amp"></param>
/// <returns></returns>

        public static ZArrayDescriptor Filt_2_Ramka(ZArrayDescriptor amp)   
        {

            int w1 = amp.width;
            int h1 = amp.height;
            int w2 = 2 * w1;
            int h2 = 2 * h1;
          
            ZArrayDescriptor res_array = new ZArrayDescriptor(w2, h2);

            int x0, y0;
            x0 = w2 / 2 - w1 / 2;
            y0 = h2 / 2 - h1 / 2;
                        
            for (int i = 0; i < w1; i++)
              for (int j = 0; j < h1; j++)
                {
                    res_array.array[i+x0, j+y0] = amp.array[i,j];
                }
            
            return res_array;
        }


        //  Усреднение 2х2 и генерация нового файла из 4 усредненных со сдвигом на половину пикселя

        public static ZArrayDescriptor ADD_razresh_2х2(ZArrayDescriptor amp)   // int kx, int ky - сдвиг массива точек
        {

            int w1 = amp.width;
            int h1 = amp.height;
            int w2 = w1 / 2;
            int h2 = h1 / 2;
            ZArrayDescriptor tmp = new ZArrayDescriptor(w2, h2);
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);

            tmp = Filt_2х2(amp, 0, 0);
            for (int i = 0; i < w2; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2, j * 2] = tmp.array[i, j]; }
            tmp = Filt_2х2(amp, 1, 0);
            for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2 + 1, j * 2] = tmp.array[i, j]; }
            tmp = Filt_2х2(amp, 0, 1);
            for (int i = 0; i < w2; i++) for (int j = 0; j < h2 - 1; j++) { res_array.array[i * 2, j * 2 + 1] = tmp.array[i, j]; }
            tmp = Filt_2х2(amp, 1, 1);
            for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2 - 1; j++) { res_array.array[i * 2 + 1, j * 2 + 1] = tmp.array[i, j]; }


            return res_array;
        }

        //  Усреднение 2х2 точек 

        public static double Sum_2х2(ZArrayDescriptor amp, int i, int j)   // int kx, int ky - сдвиг массива точек
        {
            int w1 = amp.width;
            int h1 = amp.height;

            double a = 0;
          
            a = amp.array[i , j ] + amp.array[i + 1, j ] + amp.array[i , j + 1] + amp.array[i + 1, j + 1];
   
            return a/4;
        }
      
        public static ZArrayDescriptor Filt1_2х2(ZArrayDescriptor amp)   
        {
            int w1 = amp.width;
            int h1 = amp.height;
          
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);

            for (int i = 0; i < w1; i++)   for (int j = 0; j < h1; j++)  {  res_array.array[i, j] = amp.array[i, j];   }

            for (int i = 0; i < w1-1; i++) for (int j = 0; j < h1-1; j++){  res_array.array[i , j ] = Sum_2х2( amp, i,  j); }

            return res_array;
        }

        public static ZArrayDescriptor Filt1_NхN(ZArrayDescriptor amp, int k) // Для нечетных k
        {
            int w1 = amp.width;
            int h1 = amp.height;

            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);

            for (int i = 0; i < w1; i++) for (int j = 0; j < h1; j++) { res_array.array[i, j] = amp.array[i, j]; }

            for (int i = k/2; i < w1 - k/2-1; i++) 
                for (int j = k / 2; j < h1 - k / 2 - 1; j++) { res_array.array[i, j] = Sum_NхN(amp, k, i, j); }

            return res_array;
        }
        public static double Sum_NхN(ZArrayDescriptor amp, int k, int i, int j)   // int kx, int ky - сдвиг массива точек
        {
            int w1 = amp.width;
            int h1 = amp.height;

            //MessageBox.Show("k_filt = " + k);
            double a = 0;
            for (int ix = -k/2; ix < k/2; ix++)
                for (int iy = -k / 2; iy < k / 2; iy++)
                     { a += amp.array[i + ix, j + iy]; }

            return a / (k * k);
        }

        //  Усреднение 2х2 точек с уменьшением размера файла со сдвигом

        public static ZArrayDescriptor Filt_2х2(ZArrayDescriptor amp, int kx, int ky)   // int kx, int ky - сдвиг массива точек
        {
            int w1 = amp.width;
            int h1 = amp.height;
            int w2 = w1 / 2;
            int h2 = h1 / 2;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w2, h2);

            for (int i = 0; i < w1 ; i += 2)
                for (int j = 0; j < h1 ; j += 2)
                {    
                    res_array.array[i / 2, j / 2] = amp.array[i, j];
                }

            for (int i = 0; i < w1-kx-1;   i+=2)
              for (int j = 0; j < h1-ky-1; j+=2)
                {
                    double a = (amp.array[i +     kx, j + ky] + amp.array[i +     kx, j + 1 + ky] +
                                amp.array[i + 1 + kx, j + ky] + amp.array[i + 1 + kx, j + 1 + ky]);
                    res_array.array[i/2, j/2] = a / 4;
                }
            return res_array;
        }
        public static ZArrayDescriptor Filt_2(ZArrayDescriptor amp, int kx)   // int kx, int ky - сдвиг массива точек
        {

            int w1 = amp.width;
            int h1 = amp.height;
            int w2 = w1 / 2;
            int h2 = h1 / 2;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w2, h2);

            for (int i = 0; i < w1 - kx; i += 2)
                for (int j = 0; j < h1; j++)
                {
                    res_array.array[i / 2, j / 2] =  (amp.array[i, j] + amp.array[i + kx, j])/2; 
                }
            return res_array;
        }
        //  Усреднение 4х4 точек с уменьшением размера файла в 4 раза со сдвигом

        public static ZArrayDescriptor Filt_4х4(ZArrayDescriptor amp, int kx, int ky)   // int kx, int ky - сдвиг массива точек
        {

            int w1 = amp.width;
            int h1 = amp.height;
            int w2 = w1 / 4;
            int h2 = h1 / 4;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w2, h2);

            for (int i = 0; i < w1 - 3 - kx; i += 2)
              for (int j = 0; j < h1 - 3 - ky; j += 2)
                {
                    double a = 
 (amp.array[i+kx,   j+ky] + amp.array[i+kx,   j+1+ky] + amp.array[i+kx,   j+2+ky] + amp.array[i+kx,   j+3+ky] +
  amp.array[i+1+kx, j+ky] + amp.array[i+1+kx, j+1+ky] + amp.array[i+1+kx, j+2+ky] + amp.array[i+1+kx, j+3+ky] +
  amp.array[i+2+kx, j+ky] + amp.array[i+2+kx, j+1+ky] + amp.array[i+2+kx, j+2+ky] + amp.array[i+2+kx, j+3+ky] +
  amp.array[i+3+kx, j+ky] + amp.array[i+3+kx, j+1+ky] + amp.array[i+3+kx, j+2+ky] + amp.array[i+3+kx, j+3+ky]);
                   res_array.array[i / 4, j / 4] = a / 8;
                }
            return res_array;
        }


        //  Уменьшением размера файла прореживанием в 2 раза

        public static ZArrayDescriptor Filt_1_2(ZArrayDescriptor amp)        // Сглаживание
        {

            int w1 = amp.width;
            int h1 = amp.height;
            int w2 = w1 / 2;
            int h2 = h1 / 2;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w2, h2);

            for (int i = 0; i < w1 - 1; i += 2)
                for (int j = 0; j < h1 - 1; j += 2)
                {
                    res_array.array[i / 2, j / 2] = amp.array[i, j];
                }
            return res_array;
        }

        //  Увеличение размера файла в 2 раза простым повторением
/*
        public static ZArrayDescriptor Filt_2_1(ZArrayDescriptor amp)        
        {

            int w1 = amp.width;
            int h1 = amp.height;
            int w2 = 2*w1;
            int h2 = 2*h1;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w2, h2);

            for (int i = 0; i < w2; i ++)
                for (int j = 0; j < h2; j ++)
                {
                    res_array.array[i, j] = amp.array[i/2, j/2];
                }
            return res_array;
        }
*/
        //  Увеличение размера файла в 2 раза с усреднением

        public static ZArrayDescriptor Filt_2_1_s(ZArrayDescriptor amp)
        {

            int w1 = amp.width;
            int h1 = amp.height;
            int w2 = 2 * w1;
            int h2 = 2 * h1;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w2, h2);

            for (int i = 0; i < w2-2; i++)
                for (int j = 0; j < h2-2; j++)
                {
                    
                    if (i % 2 == 0 || i % 2 == 0) res_array.array[i, j] = amp.array[i / 2, j / 2];
                    else
                    {
                        int ii = i / 2, jj = j / 2;
                        double a = (amp.array[ii , jj] + amp.array[ii , jj + 1 ] +
                                    amp.array[ii + 1 , jj ] + amp.array[ii + 1 , jj + 1 ]);
                        res_array.array[i , j ] = a / 4;
                    }
                }
            return res_array;
        }
        
        
        //  Взвешенный фильтр для голографии 9х9

        public static ZArrayDescriptor Filt_Hologramm(ZArrayDescriptor amp)   // Сглаживание
        {

            int w1 = amp.width;
            int h1 = amp.height;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);


            //double r1 = 0;

            for (int i = 1; i < w1-1; i++)
                for (int j = 1; j < h1 - 1; j++)
                {
                   double a =  (amp.array[i - 1, j - 1] + 2*amp.array[i - 1, j] + amp.array[i - 1, j + 1] +
                              2*amp.array[i, j - 1]     + 4*amp.array[i, j]     + 2*amp.array[i, j + 1] +
                                amp.array[i + 1, j - 1] + 2*amp.array[i + 1, j] + amp.array[i + 1, j + 1]);
                   res_array.array[i, j] = amp.array[i, j] - a/16;
                }
            return res_array;
        }

        public static ZArrayDescriptor Filt_Ramka(ZArrayDescriptor amp, int k)   // Сглаживание
        {

           

            int w1 = amp.width;
            int h1 = amp.height;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);

          
            double r1 = 0;

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                    res_array.array[i, j] = amp.array[i, j];

            for (int i = 0; i < w1; i++)    // По строкам
            {
               for (int j = 0; j < k; j++)     res_array.array[i, j] = r1;
               for (int j = h1-k; j < h1; j++) res_array.array[i, j] = r1;

            }

            for (int j = 0; j < h1; j++)   // По столбцам
            {
                for (int i = 0; i < k; i++)        res_array.array[i, j] = r1;
                for (int i = w1 - k; i < h1; i++) res_array.array[i, j] = r1;
                   
               
            }
           

            return res_array;
        }
        
        // -------------------------------------------------------------------------------------------- 
       public static ZArrayDescriptor  Filt_smothingSM(ZArrayDescriptor amp, int k_filt)   // Сглаживание
        {
            
            int k = k_filt;
            int k_cntr = 1;

            int w1 = amp.width;
            int h1 = amp.height;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1,h1);

            int max = w1; if (h1 > max) max = h1;

            double[] k_x  = new double[max];
            double[] k_x1 = new double[max];
            double r1 = 0;

            for (int i = 0; i < w1; i++)    // По строкам
            {
                for (int j = 0; j < h1; j++) { k_x[j] = amp.array[i,j]; }

                k_cntr = 1;
                for (int ik = k; ik > 0; ik /= 2)  // Шаг 5 2 1
                {
                    if (k_cntr == 1) 
                       { for (int j = ik; j < h1 - ik; j++)   k_x1[j] = (k_x[j - ik] + k_x[j]*2 + k_x[j + ik])/ 4 ; k_cntr = 2; }
                    else 
                       { for (int j = ik; j < h1 - ik; j++)   k_x[j] = (k_x1[j - ik] + k_x1[j]*2 + k_x1[j + ik])/4; k_cntr = 1; }
                }

                for (int j = 0; j < h1; j++) { if (k_cntr == 1) res_array.array[i, j] = k_x[j]; else res_array.array[i, j] = k_x1[j]; }

            }

            for (int j = 0; j < h1; j++)   // По столбцам
            {
                for (int i = 0; i < w1; i++) { k_x[i] = res_array.array[i, j]; }
                k_cntr = 1;
                for (int ik = k; ik > 0; ik /= 2)
                {
                    if (k_cntr == 1)
                         { for (int i = ik; i < w1 - ik; i++)   k_x1[i] = (k_x[i - ik] + k_x[i]*2 + k_x[i + ik])/4;      k_cntr = 2; }
                    else
                         { for (int i = ik; i < w1 - ik; i++)   k_x[i]  = (k_x1[i - ik] + k_x1[i]*2 + k_x1[i + ik])/4;   k_cntr = 1; }
                }

                for (int i = 0; i < w1; i++) { if (k_cntr == 1) res_array.array[i, j] = k_x[i]; else res_array.array[i, j] = k_x1[i]; }
                           
            }

            int k1= k+k / 2;
            for (int i = 0; i < w1; i++)                             // Рамка
            {
                for (int j = 0; j < k1; j++) { res_array.array[i, j] = res_array.array[i, k1]; }
                for (int j = h1 - k1; j < h1; j++) { res_array.array[i, j] = res_array.array[i, h1 - k1-1]; }
            }
            for (int j = 0; j < h1; j++)
            {
                for (int i = 0; i < k1; i++)     { res_array.array[i, j] = res_array.array[k1, j]; }
                for (int i = w1-k1; i < w1; i++) { res_array.array[i, j] = res_array.array[w1-k1-1, j]; }
            }

            return res_array;
        }
       public static ZArrayDescriptor Filt_smothing(ZArrayDescriptor amp, int k_filt)  // Удаление низких частот
       {

           int k = k_filt;
           int k_cntr = 1;

           int w1 = amp.width;
           int h1 = amp.height;
           ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);

           int max = w1; if (h1 > max) max = h1;

           double[] k_x = new double[max];
           double[] k_x1 = new double[max];
           double r1 = 0;

           for (int i = 0; i < w1; i++)    // По строкам
           {
               for (int j = 0; j < h1; j++) { k_x[j] = amp.array[i, j]; }
               k_cntr = 1;
               for (int ik = k; ik > 0; ik /= 2)
               {
                   if (k_cntr == 1)
                   { for (int j = ik; j < h1 - ik; j++)   k_x1[j] = (k_x[j - ik] + k_x[j] * 2 + k_x[j + ik]) / 4; k_cntr = 2; }
                   else
                   { for (int j = ik; j < h1 - ik; j++)   k_x[j] = (k_x1[j - ik] + k_x1[j] * 2 + k_x1[j + ik]) / 4; k_cntr = 1; }
               }
               for (int j = 0; j < h1; j++)
               {
                   if (k_cntr == 1) r1 = k_x[j]; else r1 = k_x1[j];
                   res_array.array[i, j] = r1;
               }

           }

           for (int j = 0; j < h1; j++)   // По столбцам
           {
               for (int i = 0; i < w1; i++) { k_x[i] = res_array.array[i, j]; }
               k_cntr = 1;
               for (int ik = k; ik > 0; ik /= 2)
               {
                   if (k_cntr == 1) { for (int i = ik; i < w1 - ik; i++)   k_x1[i] = (k_x[i - ik] + k_x[i] * 2 + k_x[i + ik]) / 4; k_cntr = 2; }
                   else { for (int i = ik; i < w1 - ik; i++)               k_x[i] = (k_x1[i - ik] + k_x1[i] * 2 + k_x1[i + ik]) / 4; k_cntr = 1; }
               }
               for (int i = 0; i < w1; i++)
               {
                   if (k_cntr == 1) r1 = k_x[i]; else r1 = k_x1[i];
                   res_array.array[i, j] = r1; ;
               }
           }
           for (int j = 0; j < h1; j++)
               for (int i = 0; i < w1; i++)
               { res_array.array[i, j] = amp.array[i, j] - res_array.array[i, j]; }


           return res_array;
       }

       // -------------------------------------------------------------------------------------------- Фильтрация 121 
        
        
        public static void Filt_121(PictureBox pictureBox01,  ProgressBar progressBar1, int k_filt)
        {            
            int r1 = 0;
            int k = k_filt;
            int k_cntr = 1;

            int w1 = pictureBox01.Image.Width;
            int h1 = pictureBox01.Image.Height;

            int max = w1; if (h1 > max) max = h1;

            int[] k_x = new int[max];
            int[] k_x1 = new int[max];

            //Bitmap bmp1 = new Bitmap(Form1.SelfRef.pictureBox01.Image, w1, h1);
            Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);
            Bitmap bmp2 = new Bitmap(w1, h1);

            Color c;
            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = w1;
            progressBar1.Value = 1;
            progressBar1.Step = 1;
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++) { c = bmp1.GetPixel(i, j); k_x[j] = (int)((double)(c.R + c.G + c.B) / 3); }
                k_cntr = 1;
                for (int ik = k; ik > 0; ik /= 2)
                {
                    if (k_cntr == 1) { for (int j = ik; j < h1 - ik; j++)   k_x1[j] = (k_x[j - ik] + (k_x[j] << 1) + k_x[j + ik]) >> 2; k_cntr = 2; }
                    else { for (int j = ik; j < h1 - ik; j++)   k_x[j] = (k_x1[j - ik] + (k_x1[j] << 1) + k_x1[j + ik]) >> 2; k_cntr = 1; }
                }
                for (int j = 0; j < h1; j++)
                {
                    if (k_cntr == 1) r1 = k_x[j]; else r1 = k_x1[j];
                    bmp2.SetPixel(i, j, Color.FromArgb(r1, r1, r1));
                }
                progressBar1.PerformStep();
            }
            progressBar1.Maximum = h1;
           progressBar1.Value = 1;
            for (int j = 0; j < h1; j++)
            {
                for (int i = 0; i < w1; i++) { c = bmp2.GetPixel(i, j); k_x[i] = (int)((double)(c.R + c.G + c.B) / 3); }
                k_cntr = 1;
                for (int ik = k; ik > 0; ik /= 2)
                {
                    if (k_cntr == 1) { for (int i = ik; i < w1 - ik; i++)   k_x1[i] = (k_x[i - ik] + (k_x[i] << 1) + k_x[i + ik]) >> 2; k_cntr = 2; }
                    else { for (int i = ik; i < w1 - ik; i++)   k_x[i] = (k_x1[i - ik] + (k_x1[i] << 1) + k_x1[i + ik]) >> 2; k_cntr = 1; }
                }
                for (int i = 0; i < w1; i++)
                {
                    if (k_cntr == 1) r1 = k_x[i]; else r1 = k_x1[i];
                    bmp1.SetPixel(i, j, Color.FromArgb(r1, r1, r1));
                }
                 progressBar1.PerformStep();
            }           
            pictureBox01.Size = new System.Drawing.Size(w1, h1);
            pictureBox01.Image = bmp1;
             progressBar1.Value = 1;
        }
 // -------------------------------------------------------------------------------------------------- Медианная фильтрация
        static double[] f_x = new double[100];

       private static double filt_median(int k)
        {

            double min = f_x[0], s;
             int i_min1, i_min2;
             int    k2=k/2;
             for (int i = 0; i < k; i++)
             {
                 min = f_x[i]; i_min1 = i; i_min2 = i;
                 for (int j = i; j < k;  j++) if (f_x[j] < min) { min = f_x[j]; i_min2 = j; }
                 if (i_min1 != i_min2) {  s = f_x[i_min1]; f_x[i_min1] = f_x[i_min2]; f_x[i_min2] = s; }
             }    
            if (k%2 != 0) s= f_x[k/2]; else s= (f_x[k2]+ f_x[k2-1])/2;
            return s;
         }

       public static ZArrayDescriptor Filt_Mediana(ZArrayDescriptor amp, int k_filt)
       {          
           //int s = 0;
           int k = k_filt;
           int k2 = k / 2;

           int w1 = amp.width;
           int h1 = amp.height;

           ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);

           //double c;
          

           int max = w1; if (h1 > max) max = h1;
           double[] k_x1 = new double[max];
           double[] k_x2 = new double[max];

           for (int i = 0; i < w1; i++)
           {
               for (int j = 0; j < h1; j++) { k_x1[j] =  amp.array[i, j]; }
               for (int j = 0; j < h1 - k; j++)
               {
                   for (int m = 0; m < k; m++) { f_x[m] = k_x1[j + m]; }
                   k_x2[j] = filt_median(k);
               }
               for (int j = 0; j < h1 - k; j++) { res_array.array[i, j + k2] = k_x2[j]; }
           }
          
           for (int j = 0; j < h1; j++)
           {
               for (int i = 0; i < w1; i++) {  k_x1[i] = res_array.array[i, j]; }
               for (int i = 0; i < w1 - k; i++)
               {
                   for (int m = 0; m < k; m++) { f_x[m] = k_x1[i + m]; }
                   k_x2[i] = filt_median(k);
               }
               for (int i = 0; i < w1 - k; i++) { res_array.array[i + k2, j] = k_x2[i];   }
           }
           return res_array;
       }

//------------------------------------------------------------------------------------------------------
       public static void Filt_Sobel(PictureBox pictureBox01, int k_filt)
       {
           //if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
           int k = k_filt;

           int w1 = pictureBox01.Image.Width;
           int h1 = pictureBox01.Image.Height;

           //int max = w1; if (h1 > max) max = h1;


           Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);
           Bitmap bmp2 = new Bitmap(w1, h1);

           Color c, c1, c2, c3;
           int r0, r1, r2, r3;

           int max = -32000, min = 32000;

           for (int i = 0; i < w1; i++)
           {
               for (int j = k; j < h1 - k; j++)
               {
                   c1 = bmp1.GetPixel(i, j - k); c2 = bmp1.GetPixel(i, j); c3 = bmp1.GetPixel(i, j + k);
                   r1 = (c1.R + c1.G + c1.B) / 3; r2 = (c2.R + c2.G + c2.B) / 3; r3 = (c3.R + c3.G + c3.B) / 3;
                   r0 = Math.Abs(r1 - 2 * r2 + r3) / 2;
                   if (r0 > max) max = r0; if (r0 < min) min = r0;
                   bmp2.SetPixel(i, j, Color.FromArgb(r0, 0, 0));
               }          
           }

           MessageBox.Show("Max =" + max + "Min =" + min);

           for (int j = 0; j < h1; j++)
           {
               for (int i = k; i < w1 - k; i++)
               {
                   c1 = bmp1.GetPixel(i - k, j); c2 = bmp1.GetPixel(i, j); c3 = bmp1.GetPixel(i + k, j);
                   r1 = (c1.R + c1.G + c1.B) / 3; r2 = (c2.R + c2.G + c2.B) / 3; r3 = (c3.R + c3.G + c3.B) / 3;
                   r0 = Math.Abs(r1 - 2 * r2 + r3) / 2;

                   c = bmp2.GetPixel(i, j);
                   if (r0 < c.R) r0 = c.R;
                   if (r0 > max) max = r0; if (r0 < min) min = r0;
                   //if (r0 > 255) r0 = 255;
                   bmp2.SetPixel(i, j, Color.FromArgb(r0, r0, r0));
               }
           }

           for (int j = 0; j < h1; j++)
           {
               for (int i = 0; i < w1; i++)
               {
                   c = bmp2.GetPixel(i, j);
                   r0 = c.R * 255 / max;
                   if (r0 > 100) r0 = 255;
                   bmp2.SetPixel(i, j, Color.FromArgb(r0, r0, r0));
               }
           }

           pictureBox01.Size = new System.Drawing.Size(w1, h1);
           pictureBox01.Image = bmp2;
           MessageBox.Show("Max =" + max + "Min =" + min);
       }

       public static void Filt_BW(PictureBox pictureBox01, int k_filt)
       {
           int k = k_filt;

           int w1 = pictureBox01.Image.Width;
           int h1 = pictureBox01.Image.Height;
           Bitmap bmp1 = new Bitmap(pictureBox01.Image, w1, h1);
           Bitmap bmp2 = new Bitmap(w1, h1);

           Color c1;
           int r, r1;

           for (int j = 0; j < h1; j++)
           {
               for (int i = 0; i < w1; i++)
               {
                   c1 = bmp1.GetPixel(i, j);
                   r1 = (c1.R + c1.G + c1.B) / 3;
                   if (r1 > k) r = 255; else r = 0;
                   bmp2.SetPixel(i, j, Color.FromArgb(r, r, r));
               }
           }
           pictureBox01.Image = bmp2;
       }

        public static ZArrayDescriptor Sum_Line(ZArrayDescriptor amp)
        {
           int w1 = amp.width;
           int h1 = amp.height;

           double h = w1;
           ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);             

            double[] array_line = new double[w1];

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                   array_line[i] += amp.array[i, j];

            //for (int i = 0; i < w1; i++) { array_line[i] = array_line[i] / h; }

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1/2; j++)
                    res_array.array[i, j] = array_line[i]/h1;

            for (int i = 0; i < w1; i++)
                for (int j = h1 / 2; j < h1; j++)
                    res_array.array[i, j] = amp.array[i, j];

            return res_array;
        }


    }
}
