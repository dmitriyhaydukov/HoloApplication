using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Processing.Utils
{
    public class ArrayDrawer
    {
        public Font ArrayTitleFont { get; set; }
        public Brush ArrayTitleFontBrush { get; set; }
        public int ArrayTitleMarginBottom { get; set; } = 10;
        
        public int PlotPaddingLeft { get; set; } = 10;
        public int PlotMarginBottom { get; set; } = 15;

        public int ArrayItemWidth { get; set; } = 20;
        public int ArrayItemPadding { get; set; } = 5;
        public Brush ArrayItemBrush { get; set; }

        public int PlotAxisMarginTop { get; set; } = 2;
        public Font PlotAxisFont { get; set; }
        public Brush PlotAxisBrush { get; set; }
        public Pen PlotAxisPen { get; set; }

        public Brush ArrayValueBackgroundBrush { get; set; }
        public Brush ArrayValueFontBrush { get; set; }
        public Font ArrayValueFont { get; set; }
        public int ArrayValueContainerPadding { get; set; } = 2;

        public ArrayDrawer()
        {
            ArrayTitleFont = new Font(FontFamily.GenericSerif, 20, FontStyle.Regular, GraphicsUnit.Pixel);
            ArrayTitleFontBrush = Brushes.DarkBlue;

            PlotAxisFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Regular, GraphicsUnit.Pixel);
            PlotAxisPen = new Pen(Color.Black);
            PlotAxisPen.EndCap = LineCap.ArrowAnchor;
            ArrayItemBrush = Brushes.Blue;
            PlotAxisBrush = Brushes.Gray;

            ArrayValueBackgroundBrush = Brushes.AliceBlue;
            ArrayValueFontBrush = Brushes.Black;
            ArrayValueFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        public Size MeasurePlotSize(int plotHeight, int arrayLength)
        {
            var bitmap = new Bitmap(100, 100);
            var g = Graphics.FromImage(bitmap);
            // TODO: g.MeasureString("100", PlotAxisFont).Height == 0 ??
            float height = plotHeight + g.MeasureString("TITLE", ArrayTitleFont).Height + PlotAxisMarginTop + g.MeasureString("100", PlotAxisFont).Height + PlotMarginBottom;
            float width = PlotPaddingLeft + arrayLength * (ArrayItemWidth + ArrayItemPadding * 2);

            return new Size((int)width, (int)height);
        }

        public Point Draw(Bitmap bmp, Vector2[] array, string title, int plotHeight, int top = 0, int left = 0, Graphics g = null)
        {
            if (g == null)
            {
                g = Graphics.FromImage(bmp);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            }
            int cy = top;
            int cx = left + PlotPaddingLeft;

            var titleSize = g.MeasureString(title, ArrayTitleFont);
            g.DrawString(title, ArrayTitleFont, ArrayTitleFontBrush, cx, cy);
            cy += (int)titleSize.Height + ArrayTitleMarginBottom;

            cy += plotHeight;
            var xArray = array.Select(v => v.X).ToArray();
            xArray.GetMinMax(out float min, out float max);
            var k = plotHeight / (max - min);
            var plotWidth = array.Length * (ArrayItemWidth + ArrayItemPadding * 2);
            var cur = 0f;
            float ccx, ccy;
            SizeF size;
           
            for (int i = 0, ix = cx + ArrayItemPadding; i < array.Length; i++)
            {
                cur = (xArray[i] - min) * k;
                g.FillRectangle(ArrayItemBrush, ix, cy - cur, ArrayItemWidth, cur);

                size = g.MeasureString(xArray[i].ToString("0.##"), ArrayValueFont);
                ccx = ix;
                ccy = cy - cur - size.Height;
               // g.FillRectangle(ArrayValueBackgroundBrush, ccx - ArrayValueContainerPadding, ccy - ArrayValueContainerPadding, size.Width + ArrayValueContainerPadding * 2, size.Height + ArrayValueContainerPadding * 2);
                g.DrawString(xArray[i].ToString("0.##"), ArrayValueFont, ArrayValueFontBrush, ccx, ccy);

                g.DrawString(i.ToString(), PlotAxisFont, PlotAxisBrush, ix, cy + PlotAxisMarginTop);
                ix += ArrayItemWidth + ArrayItemPadding * 2;
            }

            g.DrawLine(PlotAxisPen, cx, cy, cx + plotWidth, cy);

            cy += (int)g.MeasureString("1", PlotAxisFont).Height + PlotAxisMarginTop + PlotMarginBottom;

            return new Point(cx + plotWidth, cy);
        }
    }
}
