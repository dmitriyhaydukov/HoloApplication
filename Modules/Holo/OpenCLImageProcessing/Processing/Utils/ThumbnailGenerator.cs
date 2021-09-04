using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Cloo;
using Common;
using Infrastructure;
using Processing.Computing;

namespace Processing.Utils
{
    public static class ThumbnailGenerator
    {
        public const int ThumbnailWidth = 64;
        public const int ThumbnailHeight = 64;
        public static Bitmap Generate(Bitmap image)
        {
            var aspectRatio = image.Width / (float) image.Height;

            var xp = ThumbnailWidth;
            var yp = ThumbnailHeight;

            if (image.Width > ThumbnailWidth && image.Height > ThumbnailHeight)
            {
                if (xp / (float)yp > aspectRatio)
                    xp = (int) (yp * aspectRatio);
                else
                    yp = (int) (xp / aspectRatio);
            }
            else
            {
                xp = image.Width;
                yp = image.Height;
            }

            var thumbnail = new Bitmap(ThumbnailWidth, ThumbnailHeight);
            var graphics = Graphics.FromImage(thumbnail);
            graphics.FillRectangle(Brushes.White, 0, 0, ThumbnailWidth, ThumbnailHeight);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(image, new Rectangle((ThumbnailWidth - xp) / 2, (ThumbnailHeight - yp) / 2, xp, yp));
            graphics.Dispose();

            return thumbnail;
        }

        public static Bitmap Generate()
        {
            var thumbnail = new Bitmap(ThumbnailWidth, ThumbnailHeight);
            var graphics = Graphics.FromImage(thumbnail);
            graphics.FillRectangle(Brushes.Black, 0, 0, ThumbnailWidth, ThumbnailHeight);
            graphics.Dispose();
            return thumbnail;
        }

        public static Bitmap Generate(IImageHandler imageHandler)
        {
            if (imageHandler.OpenGlTextureId.HasValue)
            {
                using (new Timer("Get Reduced Image"))
                {
                    var app = Singleton.Get<OpenClApplication>();

                    int workSize = (int)Math.Floor(Math.Min((float)imageHandler.Width / ThumbnailWidth, (float)imageHandler.Height / ThumbnailHeight));
                    var bufferWidth = imageHandler.Width / workSize;
                    var bufferHeight = imageHandler.Height / workSize;
                    var buffer = new float[bufferHeight * bufferWidth];

                    var computeBuffer = new ComputeBuffer<float>(app.ComputeContext, ComputeMemoryFlags.WriteOnly, buffer);

                    app.Acquire(imageHandler);
                    app.ExecuteKernel("reduce", bufferWidth, bufferHeight, imageHandler, computeBuffer, workSize, imageHandler.Width, imageHandler.Height, bufferWidth);

                    app.Queue.ReadFromBuffer(computeBuffer, ref buffer, true, null);

                    app.Release(imageHandler);

                    return Generate(ImageUtils.BitmapFromArray(buffer, bufferWidth, bufferHeight));
                }
            }

            return Generate(imageHandler.ToBitmap());
        }
    }
}
