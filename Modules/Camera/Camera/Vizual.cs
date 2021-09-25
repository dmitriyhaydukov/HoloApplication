using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;


namespace Camera
{
    class Vizual
    {



        //---------------------------------------------------------------------------------------------------------------------
        //
        //         Из ZArrayDescriptor.array в PictureBox
        //
        public static void Vizual_Picture(ZArrayDescriptor zArrayDescriptor, PictureBox pictureBox01)
        {         

            // c1 = ImageProcessor.getPixel(i, j, data1);                       // c1 = bmp1.GetPixel(i, j);   
            // ImageProcessor.setPixel(data5, i, j, Color.FromArgb(r, r, r));   // bmp2.SetPixel(j, i, c1);
            // bmp5.UnlockBits(data5);   
           
            if (pictureBox01     == null) { MessageBox.Show("Vizual_Picture: pictureBox01 == null"); return; }
            if (zArrayDescriptor == null) { MessageBox.Show("Vizual_Picture: ZArrayDescriptor array == null"); return; }

            int width = zArrayDescriptor.width;
            int height = zArrayDescriptor.height;

            if (width == 0 || height == 0) { MessageBox.Show("Vizual_Picture: width == 0 || height == 0"); return; }

            Bitmap     bmp2  = new Bitmap(width, height);
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);

            double max = SumClass.getMax(zArrayDescriptor);
            double min = SumClass.getMin(zArrayDescriptor);

            //MessageBox.Show("max = " + Convert.ToString(max) + " min = " + Convert.ToString(min));

            if (Math.Abs(max - min) < 0.0000001)
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
                        int c;
                        try
                        {
                            c = Convert.ToInt32((zArrayDescriptor.array[j, i] - min) * mxmn);
                        }
                        catch (System.OverflowException )
                        {
                            c = 0;
                        }

                        Color c1 = Color.FromArgb(c, c, c);
                        ImageProcessor.setPixel(data2, j, i, c1);
                    }

                }
                pictureBox01.Image = bmp2;
                bmp2.UnlockBits(data2);
                return;
            }
        }

        public static void Vizual_PictureAsRaw(ZArrayDescriptor zArrayDescriptor, PictureBox pictureBox)
        {
            int width = zArrayDescriptor.width;
            int height = zArrayDescriptor.height;

            if (width == 0 || height == 0) { MessageBox.Show("Vizual_Picture: width == 0 || height == 0"); return; }

            Bitmap bmp2 = new Bitmap(width, height);
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);
                       
            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    int c;
                    
                    c = Convert.ToInt32(zArrayDescriptor.array[j, i]);
                                        
                    Color c1 = Color.FromArgb(c, c, c);
                    ImageProcessor.setPixel(data2, j, i, c1);
                }
            }

            pictureBox.Image = bmp2;
            bmp2.UnlockBits(data2);
        }

        // Рисование прямоугольника на форме  pictureBox01

        public static void Vizual_Rect_Ris(ZArrayDescriptor zArrayDescriptor, PictureBox pictureBox01, int x0, int y0, int x1, int y1)
        {

            if (pictureBox01 == null) { MessageBox.Show("pictureBox01 == null"); return; }
            if (zArrayDescriptor == null) { MessageBox.Show("ZArrayDescriptor array == null"); return; }

            int width = zArrayDescriptor.width;
            int height = zArrayDescriptor.height;

            if (width == 0 || height == 0) { MessageBox.Show("width == 0 || height == 0"); return; }

            Bitmap bmp2 = new Bitmap(width, height);
   
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);

            double max = SumClass.getMax(zArrayDescriptor);
            double min = SumClass.getMin(zArrayDescriptor);

            //MessageBox.Show("max = " + Convert.ToString(max) + " min = " + Convert.ToString(min));

            if (Math.Abs(max - min) < 0.0000001)
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
                        int c = Convert.ToInt32((zArrayDescriptor.array[j, i] - min) * mxmn);
                        Color c1 = Color.FromArgb(c, c, c);
                        ImageProcessor.setPixel(data2, j, i, c1);
                    }

                }

            }
            DrawRect(data2, x0, y0, x1, y1);           // Рисование окружности красным цветом
            //Fill_Circle_Outside(zArrayDescriptor, data2, width, height);   // Заполнение цветом снаружи


            pictureBox01.Image = bmp2;
            bmp2.UnlockBits(data2);
            return;
        }


        // Рисование окружности на форме  pictureBox01

        public static void Vizual_Circle_Ris(ZArrayDescriptor zArrayDescriptor, PictureBox pictureBox01, int x0, int y0, int radius)
        {

            if (pictureBox01 == null) { MessageBox.Show("pictureBox01 == null"); return; }
            if (zArrayDescriptor == null) { MessageBox.Show("ZArrayDescriptor array == null"); return; }

            int width = zArrayDescriptor.width;
            int height = zArrayDescriptor.height;

            if (width == 0 || height == 0) { MessageBox.Show("width == 0 || height == 0"); return; }

            Bitmap bmp2 = new Bitmap(width, height);
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);

            double max = SumClass.getMax(zArrayDescriptor);
            double min = SumClass.getMin(zArrayDescriptor);

            //MessageBox.Show("max = " + Convert.ToString(max) + " min = " + Convert.ToString(min));

            if (Math.Abs(max - min) < 0.0000001)
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
                        int c = Convert.ToInt32((zArrayDescriptor.array[j, i] - min) * mxmn);
                        Color c1 = Color.FromArgb(c, c, c);
                        ImageProcessor.setPixel(data2, j, i, c1);
                    }

                }

            }
            Color c2 = Color.FromArgb(255, 128, 255);
            DrawCircle(data2, x0, y0, radius, c2);           // Рисование окружности красным цветом
            DrawCircle(data2, x0, y0, radius + 1, c2);
            
            //Fill_Circle_Outside(zArrayDescriptor, data2, width, height);   // Заполнение цветом снаружи


            pictureBox01.Image = bmp2;
            bmp2.UnlockBits(data2);
            return;
        }



        public static void Vizual_Circle(ZArrayDescriptor zArrayDescriptor, PictureBox pictureBox01, int x0, int y0, int radius)
        {

            // c1 = ImageProcessor.getPixel(i, j, data1);                       // c1 = bmp1.GetPixel(i, j);   
            // ImageProcessor.setPixel(data5, i, j, Color.FromArgb(r, r, r));   // bmp2.SetPixel(j, i, c1);
            // bmp5.UnlockBits(data5);   
            if (pictureBox01 == null) { MessageBox.Show("pictureBox01 == null"); return; }
            if (zArrayDescriptor == null) { MessageBox.Show("ZArrayDescriptor array == null"); return; }

            int width = zArrayDescriptor.width;
            int height = zArrayDescriptor.height;

            if (width == 0 || height == 0) { MessageBox.Show("width == 0 || height == 0"); return; }

            Bitmap bmp2 = new Bitmap(width, height);
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);

            double max = SumClass.getMax(zArrayDescriptor);
            double min = SumClass.getMin(zArrayDescriptor);

            //MessageBox.Show("max = " + Convert.ToString(max) + " min = " + Convert.ToString(min));

            if (Math.Abs(max - min) < 0.0000001)
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
                        int c = Convert.ToInt32((zArrayDescriptor.array[j, i] - min) * mxmn);
                        Color c1 = Color.FromArgb(c, c, c);
                        ImageProcessor.setPixel(data2, j, i, c1);
                    }

                }

                Color c2 = Color.FromArgb(255, 128, 255);
                DrawCircle(data2, x0, y0, radius, c2);           // Рисование окружности  цветом
               
                Fill_Circle_Outside(zArrayDescriptor, data2, width, height, c2);   // Заполнение цветом снаружи


                pictureBox01.Image = bmp2;
                bmp2.UnlockBits(data2);
                return;
            }
        }
        private static void DrawCircle(BitmapData data2, int x0, int y0, int radius, Color c1)
        {

            //if (pictureBox01 == null)     { MessageBox.Show("Visual:DrawCircle pictureBox01 == null"); return; }
           // if (zArrayDescriptor == null) { MessageBox.Show("Visual:DrawCircle ZArrayDescriptor array == null"); return; }
           // int width = zArrayDescriptor.width;
           // int height = zArrayDescriptor.height;
          //  if (width == 0 || height == 0) { MessageBox.Show("Visual:DrawCircle width == 0 || height == 0"); return; }
           // Bitmap bmp2 = new Bitmap(width, height);
           // BitmapData data2 = ImageProcessor.getBitmapData(bmp2);

            int x, y, d;
            x = 0; y = radius; d = 3 - (radius << 1);
            while (x < y)
            {
                DrawCircle_8(data2, x, y, x0, y0, c1);
                if (d < 0)
                    d += (x << 2) + 6;
                else
                {
                    d += ((x - y) << 2) + 10;
                    --y;
                }
                x++;
            }
            if (x == y) DrawCircle_8(data2, x, y, x0, y0, c1);
           // pictureBox01.Image = bmp2;
           // bmp2.UnlockBits(data2);
        }

        private static void DrawCircle_8(BitmapData data2, int x, int y, int x0, int y0, Color c1)
        {
            //Color c = ImageProcessor.getPixel(x, y, data2);
            //Color c1 = Color.FromArgb(255-c.R, 255 - c.B, 255 - c.G);
            //Color c1 = Color.FromArgb(255, 128 , 255 );
            ImageProcessor.setPixel(data2, x0 + x, y0 - y, c1);
            ImageProcessor.setPixel(data2, x0 + y, y0 - x, c1);
            ImageProcessor.setPixel(data2, x0 + y, y0 + x, c1);
            ImageProcessor.setPixel(data2, x0 + x, y0 + y, c1);
            ImageProcessor.setPixel(data2, x0 - x, y0 + y, c1);
            ImageProcessor.setPixel(data2, x0 - y, y0 + x, c1);
            ImageProcessor.setPixel(data2, x0 - y, y0 - x, c1);
            ImageProcessor.setPixel(data2, x0 - x, y0 - y, c1);
        }

        private static void DrawRect(BitmapData data2, int minx, int miny, int maxx, int maxy)
        {
            Color c1 = Color.FromArgb(255, 128, 255);
            for (int i = minx; i < maxx; i++) { ImageProcessor.setPixel(data2, i, miny, c1); ImageProcessor.setPixel(data2, i, miny + 1, c1);  }
            for (int i = minx; i < maxx; i++) { ImageProcessor.setPixel(data2, i, maxy, c1); ImageProcessor.setPixel(data2, i, maxy+1, c1);    }
            for (int j = miny; j < maxy; j++) { ImageProcessor.setPixel(data2, minx, j, c1); ImageProcessor.setPixel(data2, minx+1, j, c1);    }
            for (int j = miny; j < maxy; j++) { ImageProcessor.setPixel(data2, maxx, j, c1); ImageProcessor.setPixel(data2, maxx + 1, j, c1);  }

        }

        private static void Fill_Circle_Outside(ZArrayDescriptor zArrayDescriptor, BitmapData data2, int width, int height, Color c2)    // Заполнение цветом
        {

            Color c;
            Color red= c2;
            Color green = Color.FromArgb(0, 0, 0);
            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    c = ImageProcessor.getPixel(j, i, data2);
                    //if (c != red) ImageProcessor.setPixel(data2, j, i, green); else break;
                    if (c != red) zArrayDescriptor.array[j,i] = 0;  else break;
                }

            }
            for (int j = 0; j < width; j++)
            {
                for (int i = height-1; i >= 0  ; i--)
                {
                    c = ImageProcessor.getPixel(j, i, data2);
                    if (c == green) continue;
                    // if (c != red ) ImageProcessor.setPixel(data2, j, i, green); else break;
                    if (c != red) zArrayDescriptor.array[j, i] = 0; else break;
                }

            }


        }

        /*
                private void Visual_Complex(ZComplexDescriptor[] zComplex, int regComplex)                 // --------------------------------------------------Отображение Complex(int regComplex)
                {
                    if (zComplex[regComplex] == null) { MessageBox.Show("Complex_pictureBox:  zComplex[regComplex] == NULL"); return; }

                    PictureBox pictureB00, pictureB01, pictureB02, pictureB03;


                    int regCmplx = 0;
                    switch (regComplex)
                    {
                        case 0: regCmplx = 0; pictureB00  = pictureBox1; pictureB01 = pictureBox2; pictureB02 = pictureBox3; pictureB03 = pictureBox4; break;
                        case 1: regCmplx = 4; pictureB00  = pictureBox5; pictureB01 = pictureBox6; pictureB02 = pictureBox7; pictureB03 = pictureBox8; break;
                        case 2: regCmplx = 8; pictureB00  = pictureBox9; pictureB01 = pictureBox10; pictureB02 = pictureBox11; pictureB03 = pictureBox12; break;
                        default: regCmplx = 0; pictureB00 = pictureBox1; pictureB01 = pictureBox2; pictureB02 = pictureBox3; pictureB03 = pictureBox4; break;
                    }

                    int width = zComplex[regComplex].width;
                    int height = zComplex[regComplex].height;
                    double[,] Image_double = new double[width, height];

                    //MessageBox.Show("regComplex " + Convert.ToString(regComplex) + "width " + Convert.ToString(width) + "height " + Convert.ToString(height));

                    Image_double = Furie.Re(zComplex[regComplex].array);
                    zArrayDescriptor[regCmplx] = new ZArrayDescriptor(Image_double);
                    zArrayDescriptor[regCmplx].Double_Picture(pictureB00);   // Double_Picture  в ZArrayDescriptor

                    Image_double = Furie.Im(zComplex[regComplex].array);
                    zArrayDescriptor[regCmplx + 1] = new ZArrayDescriptor(Image_double);
                    zArrayDescriptor[regCmplx + 1].Double_Picture(pictureB01);

                    Image_double = Furie.Amplituda(zComplex[regComplex].array);
                    zArrayDescriptor[regCmplx + 2] = new ZArrayDescriptor(Image_double);
                    zArrayDescriptor[regCmplx + 2].Double_Picture(pictureB02);

                    Image_double = Furie.Faza(zComplex[regComplex].array);
                    zArrayDescriptor[regCmplx + 3] = new ZArrayDescriptor(Image_double);
                    zArrayDescriptor[regCmplx + 3].Double_Picture(pictureB03);

                }
        */


    }
}
