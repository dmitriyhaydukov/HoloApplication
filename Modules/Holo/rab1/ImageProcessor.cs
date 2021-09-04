using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace rab1
{
    class ImageProcessor
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        unsafe public static BitmapData getBitmapData(Bitmap someBitmap)
        {
            BitmapData data1 = someBitmap.LockBits(new Rectangle(0, 0, someBitmap.Width, someBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride1 = data1.Stride;
            byte* ptr1 = (byte*)data1.Scan0;

            return data1;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        unsafe public static Color getPixel(int x, int y, BitmapData data1)
        {
            int stride1 = data1.Stride;
            byte* ptr1 = (byte*)data1.Scan0;
            Color result = Color.Empty;
            result = Color.FromArgb(ptr1[(x * 3) + y * stride1 + 2], ptr1[(x * 3) + y * stride1 + 1], ptr1[(x * 3) + y * stride1]);
            return result;
        }

        unsafe public static int getPixel_blue(int x, int y, BitmapData data1)
        {
            int stride1 = data1.Stride;
            byte* ptr1 = (byte*)data1.Scan0;
            int a = ptr1[(x * 3) + y * stride1];
            //Color result = Color.Empty;
            //result = Color.FromArgb(ptr1[(x * 3) + y * stride1 + 2], ptr1[(x * 3) + y * stride1 + 1], ptr1[(x * 3) + y * stride1]);
            return a;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        unsafe public static void setPixel(BitmapData data, int x, int y, Color fillColor)
        {
            int stride = data.Stride;
            byte* ptr = (byte*)data.Scan0;

            ptr[(x * 3) + y * stride] = fillColor.B;
            ptr[(x * 3) + y * stride + 1] = fillColor.G;
            ptr[(x * 3) + y * stride + 2] = fillColor.R;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        unsafe public static void floodImage(int x, int y, Color fillColor, Image someImage)
        {
            floodImage(x, y, fillColor, someImage, false, fillColor);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        unsafe public static void floodImage(int x, int y, Color fillColor, Image someImage, bool squareStep, Color borderColor)
        {
            List<Point> listOfPoints = new List<Point>();
            listOfPoints.Add(new Point(x, y));

            BitmapData imageData = ImageProcessor.getBitmapData((Bitmap)(someImage));
            ImageProcessor.setPixel(imageData, x, y, fillColor);

            for (; ; )
            {
                if (listOfPoints.Count == 0)
                {
                    break;
                }

                List<Point> pointsToDelete = new List<Point>();
                List<Point> pointsToAdd = new List<Point>();

                foreach (Point currentPoint in listOfPoints)
                {
                    if (squareStep == false)
                    {
                        drawPixel(currentPoint.X, currentPoint.Y, fillColor, pointsToAdd, imageData, someImage, borderColor);
                    }
                    else
                    {
                        drawPixelSquareStep(currentPoint.X, currentPoint.Y, fillColor, pointsToAdd, imageData, someImage, borderColor);
                    }

                    pointsToDelete.Add(currentPoint);
                }

                List<Point> result = listOfPoints.Except(pointsToDelete).ToList();
                listOfPoints = result;

                listOfPoints.AddRange(pointsToAdd);

                pointsToDelete = null;
                pointsToAdd = null;
                result = null;
            }

            ((Bitmap)(someImage)).UnlockBits(imageData);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        unsafe private static void drawPixel(int x, int y, Color fillColor, List<Point> listOfPoints, BitmapData imageData, Image someImage)
        {
            drawPixel(x, y, fillColor, listOfPoints, imageData, someImage, fillColor);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        unsafe private static void drawPixel(int x, int y, Color fillColor, List<Point> listOfPoints, BitmapData imageData, Image someImage, Color borderColor)
        {
            Color pixelcolor;
            if (x + 1 < someImage.Size.Width)
            {
                pixelcolor = ImageProcessor.getPixel(x + 1, y, imageData);
                if (pixelcolor.ToArgb() != borderColor.ToArgb())
                {
                    listOfPoints.Add(new Point(x + 1, y));
                    ImageProcessor.setPixel(imageData, x + 1, y, fillColor);
                }
            }

            if ((x + 1 < someImage.Size.Width) && (y + 1 < someImage.Size.Height))
            {
                pixelcolor = ImageProcessor.getPixel(x + 1, y + 1, imageData);

                if ((pixelcolor.ToArgb() != borderColor.ToArgb()))
                {
                    listOfPoints.Add(new Point(x + 1, y + 1));
                    ImageProcessor.setPixel(imageData, x + 1, y + 1, fillColor);
                }
            }

            if (y + 1 < someImage.Size.Height)
            {
                pixelcolor = ImageProcessor.getPixel(x, y + 1, imageData);

                if (pixelcolor.ToArgb() != borderColor.ToArgb())
                {
                    listOfPoints.Add(new Point(x, y + 1));
                    ImageProcessor.setPixel(imageData, x, y + 1, fillColor);
                }
            }


            if (x - 1 >= 0)
            {
                pixelcolor = ImageProcessor.getPixel(x - 1, y, imageData);

                if (pixelcolor.ToArgb() != borderColor.ToArgb())
                {
                    listOfPoints.Add(new Point(x - 1, y));
                    ImageProcessor.setPixel(imageData, x - 1, y, fillColor);
                }
            }

            if (y - 1 >= 0)
            {
                pixelcolor = ImageProcessor.getPixel(x, y - 1, imageData);

                if (pixelcolor.ToArgb() != borderColor.ToArgb())
                {
                    listOfPoints.Add(new Point(x, y - 1));
                    ImageProcessor.setPixel(imageData, x, y - 1, fillColor);
                }
            }

            if ((y - 1 >= 0) && (x - 1 >= 0))
            {
                pixelcolor = ImageProcessor.getPixel(x - 1, y - 1, imageData);

                if (pixelcolor.ToArgb() != borderColor.ToArgb())
                {
                    listOfPoints.Add(new Point(x - 1, y - 1));
                    ImageProcessor.setPixel(imageData, x - 1, y - 1, fillColor);
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        unsafe private static void drawPixelSquareStep(int x, int y, Color fillColor, List<Point> listOfPoints, BitmapData imageData, Image someImage)
        {
            drawPixelSquareStep(x, y, fillColor, listOfPoints, imageData, someImage, fillColor);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        unsafe private static void drawPixelSquareStep(int x, int y, Color fillColor, List<Point> listOfPoints, BitmapData imageData, Image someImage, Color borderColor)
        {
            Color pixelcolor;
            if (x + 1 < someImage.Size.Width)
            {
                pixelcolor = ImageProcessor.getPixel(x + 1, y, imageData);
                if ((pixelcolor.ToArgb() != borderColor.ToArgb()) && (pixelcolor.ToArgb() != fillColor.ToArgb()))
                {
                    listOfPoints.Add(new Point(x + 1, y));
                    ImageProcessor.setPixel(imageData, x + 1, y, fillColor);
                }
            }

            if (y + 1 < someImage.Size.Height)
            {
                pixelcolor = ImageProcessor.getPixel(x, y + 1, imageData);

                if ((pixelcolor.ToArgb() != borderColor.ToArgb()) && (pixelcolor.ToArgb() != fillColor.ToArgb()))
                {
                    listOfPoints.Add(new Point(x, y + 1));
                    ImageProcessor.setPixel(imageData, x, y + 1, fillColor);
                }
            }

            if (x - 1 >= 0)
            {
                pixelcolor = ImageProcessor.getPixel(x - 1, y, imageData);

                if ((pixelcolor.ToArgb() != borderColor.ToArgb()) && (pixelcolor.ToArgb() != fillColor.ToArgb()))
                {
                    listOfPoints.Add(new Point(x - 1, y));
                    ImageProcessor.setPixel(imageData, x - 1, y, fillColor);
                }
            }

            if (y - 1 >= 0)
            {
                pixelcolor = ImageProcessor.getPixel(x, y - 1, imageData);

                if ((pixelcolor.ToArgb() != borderColor.ToArgb()) && (pixelcolor.ToArgb() != fillColor.ToArgb()))
                {
                    listOfPoints.Add(new Point(x, y - 1));
                    ImageProcessor.setPixel(imageData, x, y - 1, fillColor);
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Bitmap ResizeBitmap(Bitmap sourceBMP, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
                g.DrawImage(sourceBMP, 0, 0, width, height);
            return result;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
