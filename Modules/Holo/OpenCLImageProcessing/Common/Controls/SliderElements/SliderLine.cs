using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Controls.SliderElements
{
    internal class SliderLine
    {
        public class LineStyle
        {
            public int OuterWidth { get; set; } = 6;
            public int InnerWidth { get; set; } = 4;
            public Brush InnerBrush { get; set; } = Brushes.LightGray;
            public Brush InnerSelectionBrush { get; set; } = Brushes.DeepSkyBlue;
            public Brush OuterBrush { get; set; } = Brushes.Gray;
        }

        public static void Draw(Graphics graphics, LineStyle style, int x0, int x1, int xv, int y)
        {
            var dx = (style.OuterWidth - style.InnerWidth) / 2;
            var outerPen = new Pen(style.OuterBrush, style.OuterWidth);
            graphics.DrawLine(outerPen, x0 - dx, y, x1 + dx, y);
            var innerPen = new Pen(style.InnerBrush, style.InnerWidth);
            var innerSelectionPen = new Pen(style.InnerSelectionBrush, style.InnerWidth);
            graphics.DrawLine(innerPen, xv, y, x1, y);
            graphics.DrawLine(innerSelectionPen, x0, y, xv, y);
        }
    }
}