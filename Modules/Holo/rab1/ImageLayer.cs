using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace rab1
{
    class ImageLayer
    {
        public Bitmap Bmp
        {
            get;private set;
        }
        public bool IsVisible
        {
            set;get;
        }

        public ImageLayer(int w,int h)
        {
            this.Bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            this.IsVisible = true;
        }
        public ImageLayer(Bitmap bmpToCopy)
        {
            this.Bmp = new Bitmap(bmpToCopy);
            this.IsVisible = true;
        }
        public void Rotate(float degrees)
        {
            Graphics.FromImage(this.Bmp).RotateTransform(degrees);
        }
        public void Flip(RotateFlipType type)
        {
            this.Bmp.RotateFlip(type);
        }
        public void ResizeScaled(int newW, int newH)
        {
            this.Bmp = new Bitmap(this.Bmp, newW, newH);
        }
        public void ResizeUnscaled(bool centre, int newW, int newH)
        {
            var bmp = new Bitmap(newW, newH);
            var gr = Graphics.FromImage(bmp);
            if (centre)
            {
                var leftSlack = Math.Max(0, newW - this.Bmp.Width) / 2;
                var topSlack = Math.Max(0, newH - this.Bmp.Height) / 2;
                gr.DrawImageUnscaled(this.Bmp, leftSlack, topSlack);
            }
            else
                gr.DrawImageUnscaled(this.Bmp, 0, 0);
            this.Bmp = bmp;
        }
        public void TransmissionRGB(int[] transmR, int[] transmG, int[] transmB)
        {
            uint[] uPixels; int[] pixels;
            var data = this.Bmp.LockBits(new Rectangle(0, 0, this.Bmp.Width, this.Bmp.Height),
                                         System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                         System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            pixels = new int[data.Width * data.Height];
            Marshal.Copy(data.Scan0, pixels, 0, data.Width * data.Height);
            uPixels = pixels.Select(num => (UInt32)num).ToArray();
            for (int i = 0, stp = pixels.Length; i < stp; i++)
            {
                var pixel = uPixels[i];
                var a = (int)((pixel >> 24));
                var r = (int)((pixel << 8) >> 24);
                var g = (int)((pixel << 16) >> 24);
                var b = (int)((pixel << 24) >> 24);
                pixels[i] = a << 24 + (transmR[r] << 16) +
                                (transmG[g] << 8) + (transmB[b]);
            }

            //try
            {
                Marshal.Release(data.Scan0);
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(pixels, 0);
                data.Scan0 = ptr;
            }

            //finally
            {
                this.Bmp.UnlockBits(data);
            }
        }
    }
}
