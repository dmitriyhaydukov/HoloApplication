using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Processing
{
    public static class Vector2Extensions
    {
        public static bool InRadius(this Vector2 self, Vector2 other, float radius)
        {
            return (self - other).Length < radius;
        }

        /// <summary>
        /// Скалярное произведение
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static float DotProduct(this Vector2 self, Vector2 other)
        {
            return self.X * other.X + self.Y * other.Y;
        }

        public static float DistanceTo(this Vector2 self, Vector2 other)
        {
            return (self - other).Length;
        }

        public static float DistanceToSegment(this Vector2 point, Vector2 p0, Vector2 p1)
        {
            var v = p1 - p0;
            var w = point - p0;
            var c1 = w.DotProduct(v);
            var c2 = v.DotProduct(v);

            if (c1 < 0)
                return point.DistanceTo(p0);
            if (c2 <= c1)
                return point.DistanceTo(p1);

            var b = c1 / c2;
            var pb = p0 + v * b;

            return point.DistanceTo(pb);
        }
    }
}
