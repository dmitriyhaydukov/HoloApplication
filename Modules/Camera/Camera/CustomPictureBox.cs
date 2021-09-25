using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Camera
{
    class CustomPictureBox : PictureBox
    {
        public float OffsetX { get; set; }

        protected override void OnPaint(PaintEventArgs pe) 
        {
            pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            pe.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            pe.Graphics.SmoothingMode = SmoothingMode.None;

            if (this.Image != null)
            {
                pe.Graphics.TranslateTransform(OffsetX, 0);
            }

            base.OnPaint(pe);
        }
    }
}
