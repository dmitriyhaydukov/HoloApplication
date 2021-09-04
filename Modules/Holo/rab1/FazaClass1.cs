using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
//using rab1;
using System.Numerics;
using System.Threading;
using ClassLibrary;
using rab1.Forms;

public delegate void ImageProcessed(Bitmap resultBitmap);
//public delegate void ImageProcessedForOpenGL(List<Point3D> points);

namespace rab1
{
    public class FazaClass
    {
         public static event ImageProcessed imageProcessed;
        //public static event ImageProcessedForOpenGL imageProcessedForOpenGL;

        public static ZArrayDescriptor ATAN_N(ZArrayDescriptor[] zArrayDescriptor, double[] fz)
        {
            

            int n_sdv = fz.Length;                                             // Число фазовых сдвигов
            for (int i = 0; i < n_sdv; i++) if (zArrayDescriptor[i] == null) { MessageBox.Show("FazaCalass  zArrayDescriptor == NULL"); return null; }
           
            int w1 = zArrayDescriptor[0].width;
            int h1 = zArrayDescriptor[0].height;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(w1, h1);        // Массив для фаз

            double[] i_sdv = new double[n_sdv];
            double[] v_sdv = new double[n_sdv];                                  // Вектор коэффициентов
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];
           
            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fz[i]); k_cos[i] = Math.Cos(fz[i]); }  //  Сдвиги фаз (4 сдвига - 0, 90, 180, 270  градусов)

            int[] ims1 = new int[h1];
            int[] ims2 = new int[h1];
            int[] ims3 = new int[h1];

            double pi2= Math.PI*2;
            for (int i = 0; i < w1; i++)
            {
                
                for (int j = 0; j < h1; j++)
                {
                  
                    // ------                                     Формула расшифровки
                    for (int k = 0; k < n_sdv; k++) { i_sdv[k] = zArrayDescriptor[k].array[i, j]; }
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int k = 1; k < n_sdv - 1; k++) v_sdv[k] = i_sdv[k + 1] - i_sdv[k - 1];
                    double fz1 = 0;
                    double fz2 = 0;
                    for (int k = 0; k < n_sdv; k++) { fz1 += v_sdv[k] * k_sin[k]; fz2 += v_sdv[k] * k_cos[k]; }

                    double faza = Math.Atan2(-fz1, fz2);
                    if (faza < 0) faza = faza + pi2;
                    cmpl.array[i, j] = faza; 
                }
            }
            return cmpl;
        }
        /// <summary>
        /// Фигура Лиссажу regComplex 1,2,3,4  => zArrayPicture
        /// </summary>
        /// <param name="zArrayDescriptor"></param>
        /// <param name="fz"></param>
        /// <param name="regComplex"></param>
        /// <returns></returns>
        
        public static ZArrayDescriptor ATAN_Gr(ZArrayDescriptor[] zArrayDescriptor, double[] fz, int regComplex, int n_begin, int n_end)   
        {

            int n_sdv = fz.Length;                                             // Число фазовых сдвигов
            for (int i = 0; i < n_sdv; i++) if (zArrayDescriptor[regComplex * 4 + i] == null) { MessageBox.Show("FazaCalass.ATAN_Gr zArrayDescriptor == NULL"); return null; }


            int w1 = zArrayDescriptor[regComplex * 4].width;
            int h1 = zArrayDescriptor[regComplex * 4].height;

            double[] i_sdv = new double[n_sdv];
            double[] v_sdv = new double[n_sdv];                                  // Вектор коэффициентов
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];

            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fz[i]); k_cos[i] = Math.Cos(fz[i]); }  //  Сдвиги фаз (4 сдвига - 0, 90, 180, 270  градусов)

            int[] ims1 = new int[h1];
            int[] ims2 = new int[h1];
            int[] ims3 = new int[h1];

           // double pi2 = Math.PI * 2;
           
            double max_fz1 = double.MinValue; ;
            double max_fz2 = double.MinValue; ;
            double min_fz1 = double.MaxValue;
            double min_fz2 = double.MaxValue;

            for (int i = 0; i < w1; i++)
            {
                for (int j = n_begin; j < n_end; j++)
                {
                    // ------                                     Формула расшифровки для числителя и знаменателя
                    for (int k = 0; k < n_sdv; k++) { i_sdv[k] = zArrayDescriptor[regComplex*4 + k].array[i, j]; }
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int k = 1; k < n_sdv - 1; k++) v_sdv[k] = i_sdv[k + 1] - i_sdv[k - 1];
                    double fz1 = 0;
                    double fz2 = 0;
                    for (int k = 0; k < n_sdv; k++) { fz1 += v_sdv[k] * k_sin[k]; fz2 += v_sdv[k] * k_cos[k]; }
                    fz1 = -fz1;
                    if (fz1 > max_fz1) max_fz1 = fz1; if (fz1 < min_fz1) min_fz1 = fz1;
                    if (fz2 > max_fz2) max_fz2 = fz2; if (fz2 < min_fz2) min_fz2 = fz2;
                }
            }
            //MessageBox.Show("min  fz1= " + min_fz1 + " max fz1= " + max_fz1 + "min  fz2= " + min_fz2 + " max fz2= " + max_fz2);
            int nn = 1024;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nn, nn);        // Массив для фаз
            nn = 256;
            for (int i = 0; i < w1; i++)
            {
                for (int j = n_begin; j < n_end; j++)
                {

                    // ------                                     Формула расшифровки
                    for (int k = 0; k < n_sdv; k++) { i_sdv[k] = zArrayDescriptor[regComplex * 4 + k].array[i, j]; }
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int k = 1; k < n_sdv - 1; k++) v_sdv[k] = i_sdv[k + 1] - i_sdv[k - 1];
                    double fz1 = 0;
                    double fz2 = 0;
                    for (int k = 0; k < n_sdv; k++) { fz1 += v_sdv[k] * k_sin[k]; fz2 += v_sdv[k] * k_cos[k]; }
                    fz1 = -fz1;
                    int x = (int)  ((fz1 - min_fz1) * nn / (max_fz1 - min_fz1));
                    int y = (int)  ((fz2 - min_fz2) * nn / (max_fz2 - min_fz2));
                    cmpl.array[x+10, y+10] += 25;
                   
                    //cmpl.array[x+20, y+20] = 250;
                   
                }
            }

            return cmpl;
        }
        // Фигура Лиссажу regComplex 1,2,3,4  по строке N_Line => ZArrayDescriptor
        public static ZArrayDescriptor ATAN_Gr_N(ZArrayDescriptor[] zArrayDescriptor, double[] fz, int regComplex, int N_Line)  
        {

            int n_sdv = fz.Length;                                             // Число фазовых сдвигов
            for (int i = 0; i < n_sdv; i++) if (zArrayDescriptor[regComplex * 4 + i] == null) { MessageBox.Show("FazaCalass.ATAN_Gr zArrayDescriptor == NULL"); return null; }


            int w1 = zArrayDescriptor[regComplex * 4].width;
            int h1 = zArrayDescriptor[regComplex * 4].height;
            //ZArrayDescriptor cmpl = new ZArrayDescriptor(w1, h1);        // Массив для фаз

            double[] i_sdv = new double[n_sdv];
            double[] v_sdv = new double[n_sdv];                                  // Вектор коэффициентов
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];

            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fz[i]); k_cos[i] = Math.Cos(fz[i]); }  //  Сдвиги фаз (4 сдвига - 0, 90, 180, 270  градусов)

            int[] ims1 = new int[h1];
            int[] ims2 = new int[h1];
            int[] ims3 = new int[h1];

            // double pi2 = Math.PI * 2;

            double max_fz1 = double.MinValue; ;
            double max_fz2 = double.MinValue; ;
            double min_fz1 = double.MaxValue;
            double min_fz2 = double.MaxValue;

         
                for (int j = 0; j < w1; j++)
                {

                    // ------                                     Формула расшифровки
                    for (int k = 0; k < n_sdv; k++) { i_sdv[k] = zArrayDescriptor[regComplex * 4 + k].array[j, N_Line]; }
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int k = 1; k < n_sdv - 1; k++) v_sdv[k] = i_sdv[k + 1] - i_sdv[k - 1];
                    double fz1 = 0;
                    double fz2 = 0;
                    for (int k = 0; k < n_sdv; k++) { fz1 += v_sdv[k] * k_sin[k]; fz2 += v_sdv[k] * k_cos[k]; }
                    fz1 = -fz1;
                    if (fz1 > max_fz1) max_fz1 = fz1;
                    if (fz2 > max_fz2) max_fz2 = fz2;
                    if (fz1 < min_fz1) min_fz1 = fz1;
                    if (fz2 < min_fz2) min_fz2 = fz2;
                }
           
            //MessageBox.Show("min  fz1= " + min_fz1 + " max fz1= " + max_fz1 + "min  fz2= " + min_fz2 + " max fz2= " + max_fz2);
            int nn = 1024;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nn, nn);        // Массив для фаз
            nn = 256;                                                    // 512x512
           
                for (int j = 0; j < w1; j++)
                {

                    // ------                                     Формула расшифровки
                    for (int k = 0; k < n_sdv; k++) { i_sdv[k] = zArrayDescriptor[regComplex * 4 + k].array[j, N_Line]; }
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int k = 1; k < n_sdv - 1; k++) v_sdv[k] = i_sdv[k + 1] - i_sdv[k - 1];
                    double fz1 = 0;
                    double fz2 = 0;
                    for (int k = 0; k < n_sdv; k++) { fz1 += v_sdv[k] * k_sin[k]; fz2 += v_sdv[k] * k_cos[k]; }
                    fz1 = -fz1;
                    int x = (int)((fz1 - min_fz1) * (nn-1) / (max_fz1 - min_fz1));
                    int y = (int)((fz2 - min_fz2) * (nn-1) / (max_fz2 - min_fz2));
                    //cmpl.array[x + 10, y + 10] += 25;

                     cmpl.array[x+20, y+20] = 250;

                }
    

            return cmpl;
        }

        //Lissagu(Form1.zArrayDescriptor, N_line, kk1, kk2);

        // Фигура Лиссажу k1, k2  по строке N_Line => ZArrayDescriptor 
        public static ZArrayDescriptor Lissagu(ZArrayDescriptor[] zArrayDescriptor, int regComplex,  int k1, int k2, int n_begin, int n_end, 
                                               int StepI, double  fzrad1, double fzrad2 )
        {

            //MessageBox.Show("k1 " + k1 + "k2 " + k2 + "regComplex " + regComplex);
            for (int i = 0; i < 4; i++) if (zArrayDescriptor[regComplex * 4 + i] == null)
                          { MessageBox.Show("FazaCalass.Lissagu zArrayDescriptor == NULL"); return null; }


            int w1 = zArrayDescriptor[regComplex * 4].width;
            int h1 = zArrayDescriptor[regComplex * 4].height;
           
            double max_x = double.MinValue; ;
            double max_y = double.MinValue; ;
            double min_y = double.MaxValue;
            double min_x = double.MaxValue;

            for (int i = 0; i < w1; i++)
               for (int j = n_begin; j < n_end; j++)
                {

                  double x = zArrayDescriptor[regComplex * 4 + k1].array[i, j]; // * Math.Cos(fzrad1);
                    double y = zArrayDescriptor[regComplex * 4 + k2].array[i, j]; // * Math.Cos(Math.Abs(fzrad2 - fzrad1)); 

                  if (x > max_x) max_x = x;    if (x < min_x) min_x = x;
                  if (y > max_y) max_y = y;    if (y < min_y) min_y = y;

                }
            if ((max_x - min_x) == 0) { MessageBox.Show("(max_x - min_x) == 0"); return null; }
            if ((max_y - min_y) == 0) { MessageBox.Show("(max_y - min_y) == 0"); return null; }
            int nn = 1024;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nn, nn);        // Массив для фаз
            nn = 256;                                                         
        
            for (int i = 0; i < w1; i++)
                for (int j = n_begin; j < n_end; j++)
            {
        
                double xx = zArrayDescriptor[regComplex * 4 + k1].array[i, j];
                double yy = zArrayDescriptor[regComplex * 4 + k2].array[i, j] * Math.Sin(Math.Abs(fzrad2-fzrad1));
                int x = (int) ( (xx - min_x) * (nn-1) / (max_x - min_x)  );
                int y = (int) ( (yy - min_y) * (nn-1) / (max_y - min_y)  );
                //if (x < 0 || x > 255) { MessageBox.Show(" x " + x); continue; }
                //if (y < 0 || y > 255) { MessageBox.Show(" y " + y); continue; }
                cmpl.array[x + 10 , y + 10 ] += StepI;   if (cmpl.array[x + 10, y + 10] > 250) cmpl.array[x + 10, y + 10] = 250;
            }

            return cmpl;
        }

        public static ZArrayDescriptor Lissagu3D(ZArrayDescriptor[] zArrayDescriptor, int regComplex, int k1, int k2, int k3, int n_begin, int n_end)
        {

            //MessageBox.Show("k1 " + k1 + "k2 " + k2 + "k3 " + k3 + "regComplex " + regComplex);
            for (int i = 0; i < 4; i++) if (zArrayDescriptor[regComplex * 4 + i] == null)
                { MessageBox.Show("FazaCalass.Lissagu zArrayDescriptor == NULL"); return null; }


            int w1 = zArrayDescriptor[regComplex * 4].width;
            int h1 = zArrayDescriptor[regComplex * 4].height;

            double min_x = double.MaxValue; double max_x = double.MinValue;
            double min_y = double.MaxValue; double max_y = double.MinValue;         
            double min_z = double.MaxValue; double max_z = double.MaxValue;

            for (int i = 0; i < w1; i++)
              for (int j = n_begin; j <n_end; j++)
                {

                double x = zArrayDescriptor[regComplex * 4 + k1].array[i, j];
                double y = zArrayDescriptor[regComplex * 4 + k2].array[i, j];
                double z = zArrayDescriptor[regComplex * 4 + k3].array[i, j];

                if (x > max_x) max_x = x; if (x < min_x) min_x = x;
                if (y > max_y) max_y = y; if (y < min_y) min_y = y;
                if (z > max_y) max_z = z; if (z < min_z) min_z = z;

                }

            int nn = 256;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nn+20, nn+20);        // Массив для фаз
           

            for (int i = 0; i < w1; i++)
                for (int j = n_begin; j < n_end; j++)
                {

                double xx = zArrayDescriptor[regComplex * 4 + k1].array[i, j];
                double yy = zArrayDescriptor[regComplex * 4 + k2].array[i, j];
                double zz = zArrayDescriptor[regComplex * 4 + k3].array[i, j];
                int x = (int)((xx - min_x) * (nn - 1) / (max_x - min_x));     if (x < 0 || x > 255) { MessageBox.Show(" x " + x); continue; }
                int y = (int)((yy - min_y) * (nn - 1) / (max_y - min_y));     if (y < 0 || y > 255) { MessageBox.Show(" y " + y); continue; }

                int z = (int)((zz - min_z) * (nn - 1 - 100) / (max_z - min_z));
                if (z != 0) z = z+100;
                cmpl.array[x+10 , y+10 ] = z;


            } 


            return cmpl;
        }



      

/*
        public static ZArrayDescriptor ATAN_Gr9101112(ZArrayDescriptor[] zArrayDescriptor, double[] fz)   // Фигура Лиссажу 9,10,11,12  => zArrayPicture
        {
            int n_img = 8;

            int n_sdv = fz.Length;                                             // Число фазовых сдвигов
            for (int i = 0; i < n_sdv; i++)
                if (zArrayDescriptor[n_img + i] == null) { MessageBox.Show("FazaCalass  zArrayDescriptor == NULL"); return null; }


            int w1 = zArrayDescriptor[n_img].width;
            int h1 = zArrayDescriptor[n_img].height;
            //ZArrayDescriptor cmpl = new ZArrayDescriptor(w1, h1);        // Массив для фаз

            double[] i_sdv = new double[n_sdv];
            double[] v_sdv = new double[n_sdv];                                  // Вектор коэффициентов
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];

            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fz[i]); k_cos[i] = Math.Cos(fz[i]); }  //  Сдвиги фаз (4 сдвига - 0, 90, 180, 270  градусов)

            int[] ims1 = new int[h1];
            int[] ims2 = new int[h1];
            int[] ims3 = new int[h1];

            // double pi2 = Math.PI * 2;

            double max_fz1 = double.MinValue; ;
            double max_fz2 = double.MinValue; ;
            double min_fz1 = double.MaxValue;
            double min_fz2 = double.MaxValue;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    // ------                                     Формула расшифровки
                    for (int k = 0; k < n_sdv; k++) { i_sdv[k] = zArrayDescriptor[k + n_img].array[i, j]; }

                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int k = 1; k < n_sdv - 1; k++) v_sdv[k] = i_sdv[k + 1] - i_sdv[k - 1];
                    double fz1 = 0;
                    double fz2 = 0;
                    for (int k = 0; k < n_sdv; k++) { fz1 += v_sdv[k] * k_sin[k]; fz2 += v_sdv[k] * k_cos[k]; }
                    fz1 = -fz1;
                    if (fz1 > max_fz1) max_fz1 = fz1;
                    if (fz2 > max_fz2) max_fz2 = fz2;
                    if (fz1 < min_fz1) min_fz1 = fz1;
                    if (fz2 < min_fz2) min_fz2 = fz2;
                }
            }
            //MessageBox.Show("min  fz1= " + min_fz1 + " max fz1= " + max_fz1 + "min  fz2= " + min_fz2 + " max fz2= " + max_fz2);
            int nn1 = 256;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nn1, nn1);        // Массив для фаз
            int nn = nn1/2;
            // for (int i = 0; i < w1; i++)
            // {
            //      for (int j = 0; j < h1; j++)
            int t = 100;
            for (int i = w1/2-t; i < w1/2+t; i++)
            {
              for (int j = h1 / 2 - t; j < h1 / 2 + t; j++)
            {

                // ------                                     Формула расшифровки
                for (int k = 0; k < n_sdv; k++) { i_sdv[k] = zArrayDescriptor[k + n_img].array[i, j]; }
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int k = 1; k < n_sdv - 1; k++) v_sdv[k] = i_sdv[k + 1] - i_sdv[k - 1];
                    double fz1 = 0;
                    double fz2 = 0;
                    for (int k = 0; k < n_sdv; k++) { fz1 += v_sdv[k] * k_sin[k]; fz2 += v_sdv[k] * k_cos[k]; }
                    fz1 = -fz1;

                    int x = (int)((fz1 - min_fz1) * nn / (max_fz1 - min_fz1));
                    int y = (int)((fz2 - min_fz2) * nn / (max_fz2 - min_fz2));
                    cmpl.array[x + nn1/4, y + nn1 / 4] += 20;                              // В центр
                }
            }

         //   double min = SumClass.getMin(cmpl);
         //   double max = SumClass.getMax(cmpl);
         //   for (int i = 0; i < w1; i++)
         //   {
         //       for (int j = 0; j < h1; j++)
         //       {
         //           cmpl.array[i, j] = (cmpl.array[i,j] - min) * 255 / (max - min);
                   
         //       }
         //   }


                    return cmpl;
        }
*/


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*
        public static void Graph_ATAN(Image[] img, int x0_end, int x1_end, int y0_end, int y1_end, int n, double[] fzz, double Gamma)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            int w1 = img[0].Width;
            int h1 = img[0].Height;
           
            int n_sdv = n;                                                       // Число фазовых сдвигов

            int[] i_sdv = new int[4];
            int [] v_sdv = new int[4];
           
            Color c;

            Bitmap bmp1 = (Bitmap)img[0];
            Bitmap bmp2 = (Bitmap)img[1];
            Bitmap bmp3 = (Bitmap)img[2];
            Bitmap bmp4 = (Bitmap)img[3];


            BitmapData data1 = ImageProcessor.getBitmapData(bmp1);
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);
            BitmapData data3 = ImageProcessor.getBitmapData(bmp3);
            BitmapData data4 = ImageProcessor.getBitmapData(bmp4);

            int x0=0, x1=w1, y0=0, y1=h1;
            if (x0_end !=0 || x1_end !=0 || y0_end !=0 || y1_end !=0 ) 
            { 
                x0=x0_end; x1=x1_end;  y0=y0_end; y1=y1_end;
            }

            int r;
            double fz1, fz2;

            int[,] buffer1 = new int[x1, y1];
            int[,] buffer2 = new int[x1, y1];
            int[,] buffer3 = new int[x1, y1];
            int[,] buffer4 = new int[x1, y1];

            double[] k_sin = new double[4];
            double[] k_cos = new double[4];
            double[,] vvs = new double[4, 512];
            double[,] vvc = new double[4, 512];
            double pi = Math.PI;
            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fzz[i] * pi / 180); k_cos[i] = Math.Cos(fzz[i] * pi / 180); }  //  Сдвиги фаз 
            for (int ii = 0; ii < n_sdv; ii++) for (int i = 0; i < 512; i++) { vvs[ii, i] = i * k_sin[ii]; vvc[ii, i] = i * k_cos[ii]; } 
  
            double min1 = 35000, max1 = -35000;

            int w11 = 600;
            int h11 = 600;

            int all = x1 * y1 * 2 + w11 + h11;
            int done = 0;
            
            for (int i = x0; i < x1; i++)
            {
                done = i * x1;

                PopupProgressBar.setProgress(done, all);

                for (int j = y0; j < y1; j++)
                {
                    c = ImageProcessor.getPixel(i, j, data1);
                    r = (c.R + c.G + c.B) / 3;      
                    i_sdv[0] = (int)Math.Pow(r, Gamma);     
                    buffer1[i, j] = i_sdv[0];

                    c = ImageProcessor.getPixel(i, j, data2);
                    r = (c.R + c.G + c.B) / 3;   
                    i_sdv[1] = (int)Math.Pow(r, Gamma);  
                    buffer2[i, j] = i_sdv[1];

                    c = ImageProcessor.getPixel(i, j, data3);
                    r = (c.R + c.G + c.B) / 3;   
                    i_sdv[2] = (int)Math.Pow(r, Gamma); 
                    buffer3[i, j] = i_sdv[2];

                    c = ImageProcessor.getPixel(i, j, data4);
                    r = (c.R + c.G + c.B) / 3;   
                    i_sdv[3] = (int)Math.Pow(r, Gamma);   
                    buffer4[i, j] = i_sdv[3];
               
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1] + 255;
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2] +255;

                    for (int ii = 1; ii < n_sdv - 1; ii++)
                    {  
                        v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1] + 255; 
                    }

                    fz1 = fz2 = 0; 

                    for (int ii = 0; ii < n_sdv; ii++) 
                    { 
                       fz2 += vvs[ii, v_sdv[ii]];
                       fz1 += vvc[ii, v_sdv[ii]];
                    }

                    max1 = Math.Max(fz1, max1); 
                    max1 = Math.Max(fz2, max1);
                    min1 = Math.Min(fz1, min1); 
                    min1 = Math.Min(fz2, min1);                              
                }
            }
            
            int x, y;
            double fw = (int)w11;
            fw = fw / (max1-min1);
            Bitmap bmp5 = new Bitmap(w11, h11);
            BitmapData data5 = ImageProcessor.getBitmapData(bmp5);
            int[,] buffer5 = new int[w11, h11];

            
            for (int i = x0; i < x1; i++)
            {
                done += x1;
                PopupProgressBar.setProgress(done, all);

                for (int j = y0; j < y1; j++)
                {
                    i_sdv[0] = buffer1[i, j]; 
                    i_sdv[1] = buffer2[i, j]; 
                    i_sdv[2] = buffer3[i, j]; 
                    i_sdv[3] = buffer4[i, j];

                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1] + 255;
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2] + 255;
                    for (int ii = 1; ii < n_sdv - 1; ii++) { v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1] + 255; }


                    fz1 = fz2 = 0;
                    for (int ii = 0; ii < n_sdv; ii++)
                    {
                        fz2 += vvs[ii, v_sdv[ii]];
                        fz1 += vvc[ii, v_sdv[ii]];
                    }
                   
                    //-------------------------------------------------------------------------------------------

                    x = (int)((fz1-min1) * fw);
                    y = (int)((fz2-min1) * fw) ;

                    if ((x < w11 && x >= 0) && (y < h11 && y >= 0))
                    {
                        r = buffer5[x, y]; r++;
                        if (r > 255) { r = 255;  }
                        buffer5[x, y]=r;
                    }
                }
            }
            for (int i = 0; i < w11; i++)
            {
                done += w11;

                PopupProgressBar.setProgress(done, all);

                for (int j = 0; j < h11; j++)
                {
                    ImageProcessor.setPixel(data5, i, j, Color.FromArgb(buffer5[i, j], 0, 0));
                }
            }

            PopupProgressBar.setProgress(all, all);

            bmp1.UnlockBits(data1);
            bmp2.UnlockBits(data2);
            bmp3.UnlockBits(data3);
            bmp4.UnlockBits(data4);
            bmp5.UnlockBits(data5);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (imageProcessed != null)
            {
                imageProcessed(bmp5);
            }

            if (imageProcessedForOpenGL != null)
            {
                List<Point3D> newList = new List<Point3D>();

                for (int i = x0; i < x1; i++)
                {
                    for (int j = y0; j < y1; j++)
                    {
                        newList.Add(new Point3D(buffer1[i, j], buffer2[i, j], buffer3[i, j])); 
                    }                    
                }

                imageProcessedForOpenGL(newList);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    */

    }
}
