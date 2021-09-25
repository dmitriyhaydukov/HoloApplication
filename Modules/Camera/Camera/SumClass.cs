using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using ClassLibrary;


namespace Camera
{
    public  class SumClass
    {

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Метод для получения максимума из массива
        /// </summary>
        public static double getMax(ZArrayDescriptor newDescriptor)
        {
            if (newDescriptor == null)
            {
                return 0;
            }

            double max = double.MinValue; 

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {
                   // max = Math.Max(max, newDescriptor.array[i, j]);
                    if (max < newDescriptor.array[i, j]) max = newDescriptor.array[i, j];
                }
            }

            return max;
        }
        public static double getMax(ZComplexDescriptor newDescriptor)
        {
            if (newDescriptor == null) { return 0; }

            double max = double.MinValue;

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {
                    // max = Math.Max(max, newDescriptor.array[i, j]);
                    double a = newDescriptor.array[i, j].Magnitude;
                    if (max < a) max = a;
                }
            }

            return max;
        }
        public static double getAverage(ZArrayDescriptor newDescriptor)
        {
            if (newDescriptor == null) { return 0; }


            double a = 0;

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {

                    a = a + newDescriptor.array[i, j];
                }
            }
            a = a / (newDescriptor.width * newDescriptor.height);
            return a;
        }
        public static double getAverage(ZComplexDescriptor newDescriptor)
        {
            if (newDescriptor == null) { return 0; }


            double a = 0;

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {

                    a = a + newDescriptor.array[i, j].Magnitude;
                }
            }
            a = a / (newDescriptor.width * newDescriptor.height);
            return a;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Метод для получения минимума из массива
        /// </summary>
        public static double getMin(ZArrayDescriptor newDescriptor)
        {
            if (newDescriptor == null)   {  return 0;   }

            double min = double.MaxValue;

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {
                    //min = Math.Min(min, newDescriptor.array[i, j]);
                    if (min > newDescriptor.array[i, j]) min = newDescriptor.array[i, j];
                }
            }

            return min;
        }

        public static double getMin(ZComplexDescriptor newDescriptor)
        {
            if (newDescriptor == null) { return 0; }

            double min = double.MaxValue;

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {
                    //min = Math.Min(min, newDescriptor.array[i, j]);
                    double a = newDescriptor.array[i, j].Magnitude;
                    if (min > a) min = a;
                }
            }

            return min;
        }
        public static ZArrayDescriptor Inversia(ZArrayDescriptor amp)
        {
            int w1 = amp.width;
            int h1 = amp.height;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);
            double max = getMax(amp);
            //double min = getMin(amp);

            for (int i = 0; i < w1; i++)    // По строкам
                for (int j = 0; j < h1; j++)
                {
                    res_array.array[i, j] = (max - amp.array[i, j]);

                }

            return res_array;
        }

        public static ZArrayDescriptor Sum_zArray(ZArrayDescriptor a, ZArrayDescriptor b)
        {
            int w1 = a.width;
            int h1 = a.height;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);
            
            //double min = getMin(amp);

            for (int i = 0; i < w1; i++)    // По строкам
                for (int j = 0; j < h1; j++)
                {
                    res_array.array[i, j] = a.array[i, j]+ b.array[i, j];
                }

            return res_array;
        }

      

        /// <summary>
        /// Суммирование строк в массиве от Y1 до Y2
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ZArrayDescriptor Sum_zArrayY(ZArrayDescriptor zArray, int y1, int y2)
        {
            if (zArray == null) { MessageBox.Show(" Sum_zArrayY: zArrayDescriptor == null"); return null; }
            int w1 = zArray.width;
            int h1 = zArray.height;
            if (y2 < y1)   { MessageBox.Show(" y2 < y1"); return null; }
            if (y2 > h1)   { MessageBox.Show(" y2 > h1"); return null; }
            if (y1 < 0)    { MessageBox.Show(" y1 < 0");  return null;  }
           
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);

            double[] array_line = new double[w1];

            for (int i = 0; i < w1; i++)
                for (int j = y1; j < y2; j++)
                    array_line[i] += zArray.array[i, j];

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                    res_array.array[i, j] = array_line[i] / (y2 - y1);

            return res_array;
        }

        /// <summary>
        /// Суммирование строк в массиве от 0 до h1
        /// </summary>
        /// <param name="zArray"></param>
        /// <returns></returns>
        public static ZArrayDescriptor Sum_zArrayY_ALL(ZArrayDescriptor zArray)
        {
            if (zArray == null) { MessageBox.Show(" Sum_zArrayY: zArrayDescriptor == null"); return null; }
            int w1 = zArray.width;
            int h1 = zArray.height;
            
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);

            double[] array_line = new double[w1];

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                    array_line[i] += zArray.array[i, j];

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                    res_array.array[i, j] = array_line[i] / h1;

            return res_array;
        }












        public static ZArrayDescriptor Sub_zArray(ZArrayDescriptor a, ZArrayDescriptor b)
        {
            int w1 = a.width;
            int h1 = a.height;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);

            //double min = getMin(amp);

            for (int i = 0; i < w1; i++)    // По строкам
                for (int j = 0; j < h1; j++)
                {
                    res_array.array[i, j] = a.array[i, j] - b.array[i, j];
                }

            return res_array;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///          Приведение изображения к диапазону
        /// </summary>     
        public static void Range_Picture(PictureBox pictureBox01, ZArrayDescriptor zArrayPicture, double min, double max)
        {

            // c1 = ImageProcessor.getPixel(i, j, data1);                       // c1 = bmp1.GetPixel(i, j);   
            // ImageProcessor.setPixel(data5, i, j, Color.FromArgb(r, r, r));   // bmp2.SetPixel(j, i, c1);
            // bmp5.UnlockBits(data5);   
            if (pictureBox01  == null)  { MessageBox.Show("SumClass pictureBox01 == null");           return; }
            if (zArrayPicture == null)  { MessageBox.Show("SumClass ZArrayDescriptor array == null"); return; }
           
            int width = zArrayPicture.width;
            int height = zArrayPicture.height;
            Bitmap bmp2 = new Bitmap(width, height);
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);
           
            if (max == min)
            {   
                for (int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        int c = 0;
                        if (max < 255 && max > 0.0) c = Convert.ToInt32(max);
                        if (max > 255) c = 255;
                        if (max < 0) c = 0;
                        Color c1 = Color.FromArgb(c, c, c);
                        ImageProcessor.setPixel(data2, j, i, c1);
                    }

                }

            }
            if (max != min)
            {
                double mxmn = 255.0 / (max - min);
                for (int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        double fc = zArrayPicture.array[j, i];
                        if (fc > max) fc = max;
                        if (fc < min) fc = min;
                        int c;
                        try { c = Convert.ToInt32((fc - min) * mxmn); } catch (DivideByZeroException) { c = 0; }
                        Color c1 = Color.FromArgb(c, c, c);
                        ImageProcessor.setPixel(data2, j, i, c1);
                    }

                }
            }
            pictureBox01.Image = bmp2;
            bmp2.UnlockBits(data2);

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///          Приведение изображения к диапазону без ZArrayDescriptor (перегруженный метод)
        /// </summary>     
        public static void Range_Picture(PictureBox pictureBox01,  double min, double max)
        {

            // c1 = ImageProcessor.getPixel(i, j, data1);                       // c1 = bmp1.GetPixel(i, j);   
            // ImageProcessor.setPixel(data5, i, j, Color.FromArgb(r, r, r));   // bmp2.SetPixel(j, i, c1);
            // bmp5.UnlockBits(data5);   
            if (pictureBox01 == null) { MessageBox.Show("SumClass pictureBox01 == null"); return; }


            int width = pictureBox01.Image.Width;
            int height = pictureBox01.Image.Height;

            Bitmap     bmp2  = new Bitmap(width, height);
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);

            Bitmap     bmp1  = new Bitmap(pictureBox01.Image, width, height);
            BitmapData data1 = ImageProcessor.getBitmapData(bmp1);


            if (max == min)
            {
                for (int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        int c = 0;
                        if (max < 255 && max > 0.0) c = Convert.ToInt32(max);
                        if (max > 255) c = 255;
                        if (max < 0) c = 0;
                        Color c1 = Color.FromArgb(c, c, c);
                        ImageProcessor.setPixel(data2, j, i, c1);
                    }

                }

            }
            if (max != min)
            {
                double mxmn = 255.0 / (max - min);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        //double fc = zArrayPicture.array[j, i];
                        Color c2 = ImageProcessor.getPixel(i, j, data1);
                        double fc = c2.G;
                        if (fc > max) fc = max;
                        if (fc < min) fc = min;
                        int c = Convert.ToInt32((fc - min) * mxmn);
                        Color c1 = Color.FromArgb(c, c, c);
                        ImageProcessor.setPixel(data2, i, j, c1);
                    }

                }
            }
            pictureBox01.Image = bmp2;
            bmp2.UnlockBits(data2);

        }
        /// <summary>
        /// Прведение к диапазону
        /// Если zArrayPicture распределен от min1 до max1 => диапазон расширяется до  min max
        /// 
        /// </summary>
        /// <param name="zArrayPicture"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ZArrayDescriptor Range_Array(ZArrayDescriptor zArrayPicture, double min, double max)
        {
            if (zArrayPicture == null) { MessageBox.Show("SumClass ZArrayPicture == null"); return null; }
            int width  = zArrayPicture.width;
            int height = zArrayPicture.height;
            ZArrayDescriptor rezult = new ZArrayDescriptor(width, height);

            if (max == min) return rezult;
 
            double max1 = max-min;
         
            //MessageBox.Show("max1 = " + max1);

            for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                       double fc = zArrayPicture.array[i, j];
                       if (fc > max) { rezult.array[i, j] = max; continue; }
                       if (fc < min) { rezult.array[i, j] = min; continue; }
                       rezult.array[i, j] = max1 * (fc - min) / (max - min) + min;   
                    }
                }           
            return rezult;
        }

        public static ZArrayDescriptor Range_Array1(ZArrayDescriptor zArrayPicture, double fmax)
        {
            if (zArrayPicture == null) { MessageBox.Show("SumClass ZArrayPicture == null"); return null; }
            int width = zArrayPicture.width;
            int height = zArrayPicture.height;
            ZArrayDescriptor rezult = new ZArrayDescriptor(width, height);

            double min = getMin(zArrayPicture);
            double max = getMax(zArrayPicture);

            if (max == min) return rezult;

            double max1 = max - min;

            //MessageBox.Show("fmax1 = " + fmax);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    double fc = zArrayPicture.array[i, j];
                    
                    rezult.array[i, j] = fmax * (fc - min) / max1;
                }
            }
            return rezult;
        }

        //
        //                         Устранение фазовой неоднозначности
        //
        public static ZArrayDescriptor Shift_Picture(ZArrayDescriptor zArrayPicture, double o_gr, double o_mn)
        {
            if (zArrayPicture == null) { MessageBox.Show("SumClass ZArrayDescriptor array == null"); return null; }
            int width = zArrayPicture.width;
            int height = zArrayPicture.height;
            ZArrayDescriptor z_array = new  ZArrayDescriptor(width, height);
            double max = getMax(zArrayPicture);
            //double min = getMin(zArrayPicture);
            double min = o_mn;
                for (int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        double c = zArrayPicture.array[i,j];
                        if (c > o_gr ) c = min-(max-c);
                        z_array.array[i, j] = c;
                    }

                }

            
           return z_array;

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  
        /// </summary>        
        
        
        
        public static void Sum_Color(Image[] img, int k1, int k2, int k3)
        {
            int w1 = img[k1-1].Width;
            int h1 = img[k1-1].Height;
            Bitmap bmp1 = new Bitmap(img[k1-1], w1, h1);
            

            int w2 = img[k2 - 1].Width;
            int h2 = img[k2 - 1].Height;
            Bitmap bmp2 = new Bitmap(img[k2 - 1], w2, h2);
           

            w1 = (int)Math.Min(w1, w2);
            h1 = (int)Math.Min(h1, h2);
           
            Bitmap bmp3 = new Bitmap(img[k3 - 1], w1, h1);

            Color c1,c2;
            int r1, r2, rs , rg, rb;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c1 = bmp1.GetPixel(i, j);
                    c2 = bmp2.GetPixel(i, j);
                    r1 = c1.R; r2 = c2.R; rs = r1 + r2; if (rs > 255) rs = 255;
                    r1 = c1.G; r2 = c2.G; rg = r1 + r2; if (rg > 255) rg = 255;
                    r1 = c1.B; r2 = c2.B; rb = r1 + r2; if (rb > 255) rb = 255;
                    bmp3.SetPixel(i, j, Color.FromArgb(rs, rg, rb));
                }
            }
            img[k3 - 1] = bmp3;
           
        }


        public static void Sub_Color(Image[] img, int k1, int k2, int k3, double N1, double N2)
        {
            int w1 = img[k1 - 1].Width;
            int h1 = img[k1 - 1].Height;
            Bitmap bmp1 = new Bitmap(img[k1 - 1], w1, h1);


            int w2 = img[k2 - 1].Width;
            int h2 = img[k2 - 1].Height;
            Bitmap bmp2 = new Bitmap(img[k2 - 1], w2, h2);


            w1 = (int)Math.Min(w1, w2);
            h1 = (int)Math.Min(h1, h2);

            Bitmap bmp3 = new Bitmap(img[k3 - 1], w1, h1);

            Color c1, c2;
            int r1, r2, rs;
            int max=-32000, min=32000;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c1 = bmp1.GetPixel(i, j);  r1 = (int) (c1.R * N1);
                    c2 = bmp2.GetPixel(i, j);  r2 = (int) (c2.R * N2);
                    rs = (r1 - r2);
                    min = Math.Min(rs, min);
                    max = Math.Max(rs, max);
                }
            }
// ---------------------------------------------------------------------------------------------------
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c1 = bmp1.GetPixel(i, j); r1 = (int) (c1.R * N1);
                    c2 = bmp2.GetPixel(i, j); r2 = (int) (c2.R * N2);
                    rs = (r1 - r2);
                    rs = (rs - min) * 255 / (max - min);
                    bmp3.SetPixel(i, j, Color.FromArgb(rs, rs, rs));
                }
            }
            img[k3 - 1] = bmp3;

        }
    }
}
