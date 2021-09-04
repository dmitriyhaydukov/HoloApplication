using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Common.Controls.SliderElements
{
    internal class SliderPin
    {
        public class PinStyle
        {
            public int BodyWidth { get; set; } = 12;
            public int BodyHeight { get; set; } = 12;
            public int ArrowWidth { get; set; } = 6;
            public int ArrowHeight { get; set; } = 3;
            public Pen BodyPen { get; set; } = new Pen(Color.Gray, 1);
            public Brush BodyBrush { get; set; } = Brushes.DeepSkyBlue;
            public Pen ArrowPen { get; set; } = new Pen(Color.Gray, 1);
            public Brush ArrowBrush { get; set; } = Brushes.LightBlue;
        }

        public static void Draw(Graphics graphics, PinStyle style, int cx, int cy)
        {
            var bodyRect = new Rectangle(cx - style.BodyWidth / 2, cy - style.BodyHeight / 2, style.BodyWidth, style.BodyHeight);
            //graphics.FillRectangle(style.BodyBrush, bodyRect);
            //graphics.DrawRectangle(style.BodyPen, bodyRect);
            graphics.FillEllipse(style.BodyBrush, bodyRect);
            graphics.DrawEllipse(style.BodyPen, bodyRect);

            var arrowPoints = new[]
            {
                new Point(cx - style.ArrowWidth / 2, cy + style.BodyHeight / 2),
                new Point(cx, cy + style.BodyHeight / 2 + style.ArrowHeight),
                new Point(cx + style.ArrowWidth / 2, cy + style.BodyHeight / 2)
            };
            graphics.FillPolygon(style.ArrowBrush, arrowPoints);
            graphics.DrawLines(style.ArrowPen, arrowPoints);
        }

        public static bool CheckCoord(PinStyle style, int cx, int cy, int mx, int my)
        {
            return mx < cx + style.BodyWidth / 2 && mx > cx - style.BodyWidth / 2 &&
                   my < cy + style.BodyHeight / 2 && my > cy - style.BodyHeight / 2;
        }
    }
}
