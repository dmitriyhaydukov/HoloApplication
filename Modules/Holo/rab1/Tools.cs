using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace rab1
{
    class AbstractTool
    {
        virtual public void PressMouse(int x, int y) { }
        virtual public void DragMouse(int x, int y) { }
        virtual public void ClickMouse(int x, int y) { }
    }

    class AbstrPaintingTool:AbstractTool
    {
        protected Graphics graphics;
    }

    class GBrush: AbstrPaintingTool
    {
        public Pen pen = new Pen(Color.Black);
        private int prevX, prevY;
        public GBrush(Graphics gr)
        {
            this.graphics = gr;
        }
        public GBrush(Image img)
        {
            this.graphics = Graphics.FromImage(img);
        }
        public void SetImage(Image img)
        {
            this.graphics = Graphics.FromImage(img);
        }

        public override void PressMouse(int x, int y)
        {
            this.graphics.DrawLine(this.pen, x, y, x, y);
            this.prevX = x;
            this.prevY = y;
        }
        public override void DragMouse(int x, int y)
        {
            this.graphics.DrawLine(this.pen, prevX, prevY, x, y);
            this.prevY = y;
            this.prevX = x;
        }

    }

    class GPipette:AbstractTool
    {
        private Color bindedColor = new Color();
        private Bitmap bmp;

        public GPipette(Bitmap bitmap, ref Color bindColor)
        {
            this.bmp = bitmap;
            this.bindedColor = bindColor;
        }
        public GPipette(Bitmap bitmap)
        {
            this.bmp = bitmap;
        }

        public void BindColor(ref Color colorPtr)
        {
            this.bindedColor = colorPtr;
        }
        public void SetBitmap(Bitmap bitmap)
        {
            this.bmp = bitmap;
        }
        public override void ClickMouse(int x, int y)
        {
            this.bindedColor = this.bmp.GetPixel(x, y);
        }
    }
}
