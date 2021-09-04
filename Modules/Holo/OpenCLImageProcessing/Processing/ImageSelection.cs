using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenTK;

namespace Processing
{
    /// <summary>
    /// Прямоугольная область выделения на изображении
    /// </summary>
    public class ImageSelection
    {
        public string Name;
        public int X0;
        public int X1;
        public int Y0;
        public int Y1;

        public int Width
        {
            get => X1 - X0;
            set => X1 = X0 + value;
        }

        public int Height
        {
            get => Y1 - Y0;
            set => Y1 = Y0 + value;
        }

        public override string ToString()
        {
            return Name;
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(X0, Y0, Width, Height);
        }

        /// <summary>
        /// Перемещает выделение в новое место, сохраняя размеры
        /// </summary>
        /// <param name="pos"></param>
        public void MoveTo(Vector2 pos)
        {
            var w = Width;
            var h = Height;
            X0 = (int)pos.X;
            Y0 = (int)pos.Y;
            Width = w;
            Height = h;
        }

        public void MoveBy(Vector2 delta)
        {
            X0 += (int)delta.X;
            Y0 += (int)delta.Y;
            X1 += (int)delta.X;
            Y1 += (int)delta.Y;
        }

        public string CoordsToString()
        {
            return $"{X0}, {Y0}: {Width} x {Height}";
        }
    }
}
