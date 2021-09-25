using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Camera
{
    [Serializable()]
    public class ZArrayDescriptor
    {
        public double[,] array;
        public int width;
        public int height;

        public ZArrayDescriptor()
        {
            width  = 0;
            height = 0;
        }

        public ZArrayDescriptor(int width1, int height1)
        {
            width = width1;
            height = height1;
            array = new double[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    array[i, j] = 0.0;
                }
            }
        }

        public ZArrayDescriptor(double[,] array1)
        {
           width  = array1.GetLength(0);
           height = array1.GetLength(1);
           array = new double[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    array[i, j] = array1[i, j];
                }
            }
        }


        public ZArrayDescriptor(ZArrayDescriptor descriptorToCopy)
        {
            if (descriptorToCopy == null) { MessageBox.Show("zArrayPicture = NULL"); return; }

            array = new double[descriptorToCopy.width, descriptorToCopy.height];
            width = descriptorToCopy.width;
            height = descriptorToCopy.height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    array[i, j] = descriptorToCopy.array[i, j];
                }
            }
        }

        // Конструктор array из PictureBox
        // если изображение меньше чем  Nx,  Ny, то оно помещается в центре, а по краям 0
        // если изображение больше, то оно обрезается
        public ZArrayDescriptor(PictureBox pictureBox01, int Nx, int Ny)
          {
              if (pictureBox01.Image == null)
              {
                  return;
              }
         
            
              width  = Nx;
              height = Ny;
              array = new double[Nx, Ny];

              int NxP = pictureBox01.Image.Width;
              int NyP = pictureBox01.Image.Height;

            Bitmap bmp2 = new Bitmap(pictureBox01.Image, NxP, NyP);
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);

            // Color.FromArgb(c, c, c);
            // c1 = ImageProcessor.getPixel(i, j, data1);                       // c1 = bmp1.GetPixel(i, j);   
            // ImageProcessor.setPixel(data5, i, j, Color.FromArgb(r, r, r));   // bmp2.SetPixel(j, i, c1);
            // bmp5.UnlockBits(data5);   
                      
            //MessageBox.Show("Nx =" + Convert.ToString(Nx) + "Ny =" + Convert.ToString(Ny));
            //MessageBox.Show("width =" + Convert.ToString(width) + "height =" + Convert.ToString(height));
            int N2 = 0;
            if (Nx > NxP)
            {
                N2 = (Nx - NxP) / 2;
                for (int i = 0; i < N2; i++) for (int j = 0; j < Ny; j++) array[i, j] = 0.0;
                for (int i = N2; i < Nx - N2; i++) Stolb(i, N2, data2, Ny, NyP, ColorModeEnum.Uknown);
                for (int i = Nx - N2; i < Nx; i++) for (int j = 0; j < Ny; j++) array[i, j] = 0.0;
            }
            else
                for (int i = 0; i < Nx; i++) Stolb(i, 0, data2, Ny, NyP, ColorModeEnum.Uknown);
        
            bmp2.UnlockBits(data2);
            
          }

        public ZArrayDescriptor(Bitmap bitmap, ColorModeEnum colorMode)
        {
            BitmapData bitmapData = ImageProcessor.getBitmapData(bitmap);

            width = bitmap.Width;
            height = bitmap.Height;
            array = new double[width, height];

            int NxP = bitmap.Width;
            int NyP = bitmap.Height;

            //for (int i = 0; i < NxP; i++) Stolb(i, 0, bitmapData, NyP, NyP);

            int Nx = NxP;
            int Ny = NyP;

            int N2 = (Nx - NxP) / 2;
            for (int i = 0; i < N2; i++) for (int j = 0; j < Ny; j++) array[i, j] = 0.0;
            for (int i = N2; i < Nx - N2; i++) Stolb(i, N2, bitmapData, Ny, NyP, colorMode);
            for (int i = Nx - N2; i < Nx; i++) for (int j = 0; j < Ny; j++) array[i, j] = 0.0;

            bitmap.UnlockBits(bitmapData);
        }

        public void Stolb(int i, int N2x, BitmapData data2, int Ny, int NyP, ColorModeEnum colorMode)
        {
            
            int N2=0;
            if (Ny > NyP)
            {
                N2 = (Ny-NyP) / 2;
                for (int j = 0; j < N2; j++) array[i, j] = 0.0;
                for (int j = N2; j < Ny-N2; j++)
                {
                    Color c1 = ImageProcessor.getPixel(i-N2x, j-N2, data2);
                    array[i, j] = GetColorValue(c1, colorMode);
                }
                for (int j = Ny-N2; j < Ny; j++) array[i, j] = 0.0;
            }
            else
                for (int j = 0; j < Ny; j++)
                {
                    Color c1 = ImageProcessor.getPixel(i-N2x, j, data2);
                    array[i, j] = c1.R;
                }
        }
        // Конструктор array из  zArray
        // если изображение меньше чем  Nx,  Ny, то оно помещается в левый угол, если изображение больше, то оно обрезается
        // 
        public ZArrayDescriptor(ZArrayDescriptor zArrayPicture, int Nx, int Ny, int k)
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture = NULL"); return; }
            if (k != 1) { MessageBox.Show(" ZArrayDescriptor k != 1"); return; }

           
                 width  = Nx;
                 height = Ny;
                 array = new double[Nx, Ny];
                 int Nx_z = zArrayPicture.width;
                 int Ny_z = zArrayPicture.height;
                 Nx_z = Math.Min(Nx, Nx_z);
                 Ny_z = Math.Min(Ny, Ny_z);

                 for (int i = 0; i < Nx_z; i++) 
                    for (int j = 0; j < Ny_z; j++)
                      array[i, j] = zArrayPicture.array[i,j];
            
            
        }
        
        //--------------------------------------------------------------------------------------------------------------------
        // Конструктор array из  zArray
        // если изображение меньше чем  Nx,  Ny, то оно помещается в центре, а по краям 0
        // если изображение больше, то оно обрезается
        public ZArrayDescriptor(ZArrayDescriptor zArrayPicture, int Nx, int Ny)
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture = NULL"); return; }


            width = Nx;
            height = Ny;
            array = new double[Nx, Ny];

            int NxP = zArrayPicture.width;
            int NyP = zArrayPicture.height;

           // MessageBox.Show("NX= " + Convert.ToString(Nx) + "Ny= " + Convert.ToString(Nx));
           // MessageBox.Show("NxP= " + Convert.ToString(NxP) + "NyP= " + Convert.ToString(NyP));

            int N2 = 0;
            if (Nx > NxP)
            {
                N2 = (Nx - NxP) / 2;
                for (int i = 0; i < N2; i++) for (int j = 0; j < Ny; j++) array[i, j] = 0.0;
                for (int i = N2; i < Nx - N2; i++) Stolb_a(i, N2, zArrayPicture, Ny, NyP);
                for (int i = Nx - N2; i < Nx; i++) for (int j = 0; j < Ny; j++) array[i, j] = 0.0;
            }
            else
                for (int i = 0; i < Nx; i++) Stolb_a(i, 0, zArrayPicture, Ny, NyP);

        }

        public void Stolb_a(int i, int N2x, ZArrayDescriptor data2, int Ny, int NyP)
        {

            int N2 = 0;
            if (Ny > NyP)
            {
                N2 = (Ny - NyP) / 2;
                for (int j = 0; j < N2; j++) array[i, j] = 0.0;
                for (int j = N2; j < Ny - N2; j++) { array[i, j] = data2.array[i - N2x, j - N2]; }
                for (int j = Ny - N2; j < Ny; j++) array[i, j] = 0.0;
            }
            else
                for (int j = 0; j < Ny; j++)  { array[i, j] = data2.array[i - N2x, j];       }
        }

        //  То же для фазы 0- 127  -------------------------------------------------------------------------------------
        public ZArrayDescriptor(int Nx, int Ny, ZArrayDescriptor zArrayPicture)
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture = NULL"); return; }
           

            width  = Nx;
            height = Ny;
            array = new double[Nx, Ny];

            int NxP = zArrayPicture.width;
            int NyP = zArrayPicture.height;


            // MessageBox.Show("NX= " + Convert.ToString(Nx) + "Ny= " + Convert.ToString(Nx));
            // MessageBox.Show("NxP= " + Convert.ToString(NxP) + "NyP= " + Convert.ToString(NyP));

            if (Nx==NxP && Ny==NyP)
            {
                for (int i = 0; i < Nx; i++) for (int j = 0; j < Ny; j++) array[i, j] = zArrayPicture.array[i,j];
                return;
            }


            int N2 = 0;
            if (Nx > NxP)
            {
                N2 = (Nx - NxP) / 2;
                for (int i = 0; i < N2; i++) for (int j = 0; j < Ny; j++) array[i, j] = 0; // 127;
                for (int i = N2; i < Nx - N2; i++) Stolb_a0(i, N2, zArrayPicture, Ny, NyP);
                for (int i = Nx - N2; i < Nx; i++) for (int j = 0; j < Ny; j++) array[i, j] = 0; // 127;
            }
            else
                for (int i = 0; i < Nx; i++) Stolb_a0(i, 0, zArrayPicture, Ny, NyP);
        }

        public void Stolb_a0(int i, int N2x, ZArrayDescriptor data2, int Ny, int NyP)
        {

            int N2 = 0;
            if (Ny > NyP)
            {
                N2 = (Ny - NyP) / 2;
                for (int j = 0; j < N2; j++) array[i, j] = 0; // 127;
                for (int j = N2; j < Ny - N2; j++) { array[i, j] = data2.array[i - N2x, j - N2]; }
                for (int j = Ny - N2; j < Ny; j++) array[i, j] = 0; // 127;
            }
            else
                for (int j = 0; j < Ny; j++) { array[i, j] = data2.array[i - N2x, j]; }
        }

        //---------------------------------------------------------------------------------------------------------------------
        //
        //         Из ZArrayDescriptor.array в PictureBox
        //
        public void Double_Picture(PictureBox pictureBox01)
          {

              Bitmap bmp2 = new Bitmap(width, height);
              BitmapData data2 = ImageProcessor.getBitmapData(bmp2);

              // c1 = ImageProcessor.getPixel(i, j, data1);                       // c1 = bmp1.GetPixel(i, j);   
              // ImageProcessor.setPixel(data5, i, j, Color.FromArgb(r, r, r));   // bmp2.SetPixel(j, i, c1);
              // bmp5.UnlockBits(data5);   
              if (pictureBox01 == null)  {   MessageBox.Show("pictureBox01 == null");           return; }
              if (array == null)         {   MessageBox.Show("ZArrayDescriptor array == null"); return;    }

              double max = double.MinValue;
              double min = double.MaxValue;
              
              for (int j = 0; j < width; j++)
              {
                  for (int i = 0; i < height; i++)
                  {
                      min = Math.Min(min, array[i, j]);
                      max = Math.Max(max, array[i, j]);
                  }
              }
              //MessageBox.Show("max = " + Convert.ToString(max) + " min = " + Convert.ToString(min));

              if (Math.Abs(max-min) <0.0000001 )
              {
                 // MessageBox.Show("max = min");
                  int c = 0;
                  if (max < 255 && max > 0.0) c = Convert.ToInt32(max);
                  if (max > 255) c = 255;
                  if (max < 0) c = 0; 
                  for (int j = 0; j < width; j++)
                  {
                      for (int i = 0; i < height; i++)
                      {
                          Color c1 = Color.FromArgb(c, c, c);
                          ImageProcessor.setPixel(data2, j, i, c1);
                      }

                  }
                  pictureBox01.Image = bmp2;
                  bmp2.UnlockBits(data2);
                  return;
            
              }
              if (max != min)
              {
                  double mxmn = 255.0 / (max - min);
                  for (int j = 0; j < width; j++)
                  {
                      for (int i = 0; i < height; i++)
                      {
                          int c = Convert.ToInt32((array[j, i] - min) * mxmn);
                          Color c1 = Color.FromArgb(c, c, c);
                          ImageProcessor.setPixel(data2, j, i, c1);
                      }

                  }
                  pictureBox01.Image = bmp2;
                  bmp2.UnlockBits(data2);
                  return;
              }
             

          }

        private double GetColorValue(Color color, ColorModeEnum colorMode)
        {
            double resValue = 0;
            switch (colorMode)
            {
                case ColorModeEnum.Uknown:
                    {
                        resValue = GetGrayScaleValue(color);
                        break;
                    }
                case ColorModeEnum.Gray:
                    {
                        resValue = GetGrayScaleValue(color);
                        break;
                    }
                case ColorModeEnum.Red:
                    {
                        resValue = color.R;
                        break;
                    }
                case ColorModeEnum.Green:
                    {
                        resValue = color.G;
                        break;
                    }
                case ColorModeEnum.Blue:
                    {
                        resValue = color.B;
                        break;
                    }
            }

            return resValue;
        }

        private double GetGrayScaleValue(Color color)
        {
            /*
            return
                (
                    Convert.ToDouble(color.R) + 
                    Convert.ToDouble(color.B) + 
                    Convert.ToDouble(color.G)
                ) / 3.0;
            */

            return
                (
                    color.R +
                    color.B +
                    color.G
                ) / 3.0;
        }

    }
}
