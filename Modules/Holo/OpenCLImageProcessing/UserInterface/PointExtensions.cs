using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace UserInterface
{
    public enum CoordinateResolvingDirection
    {
        ViewToImage,
        ImageToView
    }

    public enum CoordinateResolvingOrigin
    {
        View,
        Image
    }

    public static class ViewImageGeometryUtil
    {
        private static void ResolveCoordinates_ImageToView(float ix, float iy, float dx, float dy, out float vx,
            out float vy, float zoom, CoordinateResolvingOrigin origin)
        {
            vx = ix * zoom;
            vy = iy * zoom;

            if (origin == CoordinateResolvingOrigin.Image)
            {
                dx = dx * zoom;
                dy = dy * zoom;
            }

            vx += dx;
            vy += dy;
        }

        private static void ResolveCoordinates_ViewToImage(float vx, float vy, float dx, float dy, out float ix,
            out float iy, float zoom, CoordinateResolvingOrigin origin)
        {
            ix = vx / zoom;
            iy = vy / zoom;

            if (origin == CoordinateResolvingOrigin.View)
            {
                dx = dx / zoom;
                dy = dy / zoom;
            }

            ix += dx;
            iy += dy;
        }

        /// <summary>
        /// Разрешаем координаты изображения с учетом зума и сдвига относительно отображения.
        /// Сдвиг считается в координатах для origin
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="zoom"></param>
        /// <param name="direction"></param>
        /// <param name="origin"></param>
        public static void ResolveCoordinates(float x, float y, float dx, float dy, out float rx, out float ry,
            float zoom, CoordinateResolvingDirection direction,
            CoordinateResolvingOrigin origin = CoordinateResolvingOrigin.Image)
        {
            if (direction == CoordinateResolvingDirection.ImageToView)
                ResolveCoordinates_ImageToView(x, y, dx, dy, out rx, out ry, zoom, origin);
            else
                ResolveCoordinates_ViewToImage(x, y, dx, dy, out rx, out ry, zoom, origin);
        }

        public static PointF ResolveToView(this PointF self, float zoom, float dx, float dy,
            CoordinateResolvingOrigin origin = CoordinateResolvingOrigin.Image)
        {
            float x, y;
            ResolveCoordinates(self.X, self.Y, dx, dy, out x, out y, zoom, CoordinateResolvingDirection.ImageToView,
                origin);
            return new PointF(x, y);
        }

        public static PointF ResolveToImage(this PointF self, float zoom, float dx, float dy,
            CoordinateResolvingOrigin origin = CoordinateResolvingOrigin.Image)
        {
            float x, y;
            ResolveCoordinates(self.X, self.Y, dx, dy, out x, out y, zoom, CoordinateResolvingDirection.ViewToImage,
                origin);
            return new PointF(x, y);
        }
    }
}
