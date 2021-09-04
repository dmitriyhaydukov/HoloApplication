using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using Cloo;
using Common;
using Infrastructure;
using OpenTK;
using Processing.Computing;

namespace Processing.Utils
{
    public static class ImageUtils
    {
        public static void PopulateMinMax(IImageHandler imageHandler)
        {
            const int channelsCount = 2;
            // Обрабатывается сразу два канала
            using (new Timer("Populate MinMax"))
            {
                var app = Singleton.Get<OpenClApplication>();
                var kernel = app.Program.CreateKernel("minMax" + channelsCount);

                const int workSize = 16;
                var bufferWidth = imageHandler.Width / workSize;
                var bufferHeight = imageHandler.Height / workSize;
                var bufferMin = new float[bufferHeight * bufferWidth * 2];
                var bufferMax = new float[bufferHeight * bufferWidth * 2];

                var computeBufferMin = new ComputeBuffer<float>(app.ComputeContext, ComputeMemoryFlags.WriteOnly, bufferMin);
                var computeBufferMax = new ComputeBuffer<float>(app.ComputeContext, ComputeMemoryFlags.WriteOnly, bufferMax);

                imageHandler.UploadToComputingDevice();
                app.Acquire(imageHandler);
                kernel.SetMemoryArgument(0, imageHandler.ComputeBuffer);
                kernel.SetMemoryArgument(1, computeBufferMin);
                kernel.SetMemoryArgument(2, computeBufferMax);
                kernel.SetValueArgument(3, workSize);
                kernel.SetValueArgument(4, imageHandler.Width);
                kernel.SetValueArgument(5, imageHandler.Height);
                kernel.SetValueArgument(6, bufferWidth);

                app.ExecuteInQueue(kernel, bufferWidth, bufferHeight);
                app.Queue.Finish();

                app.Queue.ReadFromBuffer(computeBufferMin, ref bufferMin, true, null);
                app.Queue.ReadFromBuffer(computeBufferMax, ref bufferMax, true, null);

                app.Release(imageHandler);

                var min = new float[channelsCount];
                var max = new float[channelsCount];
                float val;
                int j;

                for (var i = 0; i < bufferHeight * bufferWidth * 2; i += 2)
                {
                    for (j = 0; j < channelsCount; j++)
                    {
                        val = bufferMin[i + j];
                        if (min[j] > val)
                            min[j] = val;

                        val = bufferMax[i + j];
                        if (max[j] < val)
                            max[j] = val;
                    }
                }

                DebugLogger.Log($"MinMax: [{min[0]} - {max[0]}] ; [{min[1]} - {max[1]}]");

                imageHandler.Tags.SetOrAdd(ImageHandlerTagKeys.MinimumValueF, min[0]);
                imageHandler.Tags.SetOrAdd(ImageHandlerTagKeys.MaximumValueF, max[0]);
                if (imageHandler.GetChannelsCount() == 2)
                {
                    imageHandler.Tags.SetOrAdd(ImageHandlerTagKeys.MinimumValue2F, min[1]);
                    imageHandler.Tags.SetOrAdd(ImageHandlerTagKeys.MaximumValue2F, max[1]);
                }
            }
        }

        public static ImageHandler Transpose(IImageHandler input)
        {
            var output = ImageHandler.Create(input.GetTitle() + " transponed", input.Height, input.Width, input.Format, input.PixelFormat);
            output.UploadToComputingDevice();

            Transpose(input, output);

            return output;
        }

        public static void Transpose(IImageHandler input, IImageHandler output)
        {
            if (output.Height != input.Width || output.Width != input.Height)
                throw new InvalidOperationException("Не подходящие размеры изображения вывода.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("transpose_img", input.Width, input.Height, input, output);
        }

        public static void CyclicShift(IImageHandler image)
        {
            using (new Timer("Cyclic Shift"))
            {
                var app = Singleton.Get<OpenClApplication>();
                app.Acquire(image);
                var w = image.Width / 2;
                var h = image.Height / 2;
                app.ExecuteKernel("cyclicShift_img", w, h, image, image);
                app.Release(image);
            }
        }
        
        public static Bitmap BitmapFromArray(float[] array, int width, int height)
        {
            var min = array[0];
            var max = array[0];
            float cur;

            for (var i = 0; i < array.Length; i++)
            {
                cur = array[i];
                if (min > cur)
                    min = cur;
                if (max < cur)
                    max = cur;
            }

            var k = 255 / (max - min);

            var bitmap = new Bitmap(width, height);
            var bdata = bitmap.LockBits(Rectangle.FromLTRB(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* p0 = (byte*) bdata.Scan0;
                byte val;

                for (var i = 0; i < array.Length; i++)
                {
                    val = (byte)((array[i] + min) * k);
                    p0[0] = val;
                    p0[1] = val;
                    p0[2] = val;
                    p0 += 3;
                }
            }

            bitmap.UnlockBits(bdata);

            return bitmap;
        }

        public static Bitmap ExtractSelection(Bitmap bmp, ImageSelection selection)
        {
            // TODO: use bmp pixel format
            var extraction = new Bitmap(selection.Width, selection.Height, PixelFormat.Format24bppRgb);

            var srcBitmapData = bmp.LockBits(Rectangle.FromLTRB(0,0,bmp.Width,bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            var dstBitmapData = extraction.LockBits(Rectangle.FromLTRB(0, 0, selection.Width, selection.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            var length = selection.Width * selection.Height * 3;

            unsafe
            {
                var srcPointer = (byte*) srcBitmapData.Scan0;
                var dstPointer = (byte*) dstBitmapData.Scan0;
                int x;
                int srcCoord, dstCoord;
                for (int y = 0; y < selection.Height; y++)
                {
                    for (x = 0; x < selection.Width; x++)
                    {
                        dstCoord = x + y * selection.Width;
                        srcCoord = x + selection.X0 + (y + selection.Y0) * bmp.Width;
                        dstPointer[dstCoord * 3] = srcPointer[srcCoord * 3];
                        dstPointer[dstCoord * 3 + 1] = srcPointer[srcCoord * 3 + 1];
                        dstPointer[dstCoord * 3 + 2] = srcPointer[srcCoord * 3 + 2];
                    }
                }
//                for (int i = 0; i < length; i++)
//                {
//                    dstPointer[0] = srcPointer[0];
//                    dstPointer++;
//                    srcPointer++;
//                }
            }

            bmp.UnlockBits(srcBitmapData);
            extraction.UnlockBits(dstBitmapData);

            return extraction;
        }

        public static byte[] BitmapToByteArray(Bitmap bmp)
        {
            var bitmapData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
            var data = new byte[bmp.Width * bmp.Height * (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4)];
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, data, 0, data.Length);
            bmp.UnlockBits(bitmapData);

            return data;
        }

        public static byte[] BitmapAsGreyscaleToByteArray(Bitmap bmp, out float min, out float max)
        {
            var bitmapData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
            var data = new byte[bmp.Width * bmp.Height * sizeof(float)];
           
            min = float.MaxValue;
            max = float.MinValue;
            unsafe
            {
                fixed (byte* pd = data)
                {
                    var p0 = (byte*)bitmapData.Scan0;
                    var fpd = (float*)pd;
                    float v;
                    var dataStep = bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4;
                    var dataLength = bmp.Width * bmp.Height * dataStep;


                    for (var i = 0; i < dataLength; i += dataStep)
                    {
                        v = (p0[i] * 0.33f + p0[i + 1] * 0.33f + p0[i + 2] * 0.33f) / 255f;

                        if (min > v)
                            min = v;

                        if (max < v)
                            max = v;

                        *fpd++ = v;
                    }
                }
            }

            bmp.UnlockBits(bitmapData);

            return data;
        }
    }
}
