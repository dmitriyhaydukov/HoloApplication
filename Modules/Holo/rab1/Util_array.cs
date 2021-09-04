using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using ClassLibrary;

namespace rab1
{
    class Util_array
    {

        //
        //         Из PictureBox в  ZArrayDescriptor.array 
        //
        public static ZArrayDescriptor getArrayFromImage(PictureBox pictureBox)
        {
            if (pictureBox == null) { MessageBox.Show("pictureBox == null"); return null; }


            int w1 = pictureBox.Image.Width;
            int h1 = pictureBox.Image.Height;
            Bitmap bmp1 = new Bitmap(pictureBox.Image, w1, h1);
            BitmapData data = ImageProcessor.getBitmapData(bmp1);


            ZArrayDescriptor result = new ZArrayDescriptor();
            result.array = new double[w1, h1];
            result.width = w1;
            result.height = h1;
   
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    Color c = ImageProcessor.getPixel( i, j, data);
                    result.array[i, j] = (c.R + c.G + c.B) / 3;
                    //result.array[i, j] = ImageProcessor.getPixel_blue(j, i, data);

                }
            }

            bmp1.UnlockBits(data);

            return result;
        }
  /*      public static ZArrayDescriptor getArrayFromImage(PictureBox pictureBox)
        {
            if (pictureBox == null) { MessageBox.Show("pictureBox == null"); return null; }


            int w1 = pictureBox.Image.Width;
            int h1 = pictureBox.Image.Height;
            Bitmap bmp1 = new Bitmap(pictureBox.Image, w1, h1);
            //BitmapData data = ImageProcessor.getBitmapData(bmp2);


            ZArrayDescriptor result = new ZArrayDescriptor();
            result.array = new double[w1, h1];
            result.width = w1;
            result.height = h1;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    //Color currentColor = ImageProcessor.getPixel(i, j, data);
                    Color c = bmp1.GetPixel(i, j);
                    result.array[i, j] = (c.R + c.G + c.B) / 3;
                }
            }

            //bmp2.UnlockBits(data);

            return result;
        }
*/
        public static ZArrayDescriptor getArrayFromImage(Image image)
        {
           
            int w1 = image.Width;
            int h1 = image.Height;
            Bitmap bmp1 = new Bitmap(image, w1, h1);
            //BitmapData data = ImageProcessor.getBitmapData(bmp2);


            ZArrayDescriptor result = new ZArrayDescriptor(w1, h1);
           

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    
                    Color c = bmp1.GetPixel(i, j);
                    result.array[i, j] = (c.R + c.G + c.B) / 3;
                }
            }

            //bmp2.UnlockBits(data);

            return result;
        }




    }
}
