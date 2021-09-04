using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Common
{
    public static class RectangleExtensions
    {
        /// <summary>
        /// Находится ли точка внутри прямоугольника
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool ContainsPoint(this RectangleF self, float x, float y)
        {
            return x >= self.X && x <= self.X + self.Width && y >= self.Y && y <= self.Y + self.Height;
        }

        public static bool ContainsPoint(this Rectangle self, int x, int y)
        {
            return x >= self.X && x <= self.X + self.Width && y >= self.Y && y <= self.Y + self.Height;
        }
    }
}
