using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace rab1
{ 
    public enum GTool
    {
        Brush,
        Pipette,
    }


    public class Pain_t_Core
    {
        List<ImageLayer> layers = new List<ImageLayer>();
        public string Filename { get;  set; }
        int[] pixelsRchan,
               pixelsGchan,
               pixelsBchan,
               pixelsBrightness,
               pixelsAchan;
        int pixelsNum;
        Graphics graphics;
        Bitmap resultBmp;
        Bitmap baseLayer;
        Bitmap capLayer;
        PictureBox outPort;


        GTool tool = GTool.Brush;
        bool drag = false;
        AbstractTool curTool;
        GBrush gBrush;
        GPipette gPipette;
        private Color firstColor, secondColor;
        public Color FirstColor
        {
             set;get;
        }
        public Color SecondColor
        {
             set; get;
        }

        public Pain_t_Core(PictureBox pb)
        {
            this.outPort = pb;
            this.CreateTools(null);
        }

        //подготовка отрисовки
        public void Draw()
        {
            this.graphics.Clear(Color.Transparent);
            this.graphics.DrawImageUnscaled(this.baseLayer, 0, 0);
            this.graphics.DrawImageUnscaled(this.capLayer, 0, 0);
            this.outPort.Invalidate();
        }
        private void SplitActiveLayers()
        {
            var activeLayers = this.layers.Where(layer => layer.IsVisible);
            this.capLayer = activeLayers.Last().Bmp;
            this.RefreshTools();
            this.SetTool(this.tool);
            var w = this.resultBmp.Width;
            var h = this.resultBmp.Height;
            this.baseLayer = new Bitmap(w, h);
            var gr = Graphics.FromImage(this.baseLayer);
            foreach (var layer in activeLayers.Take(activeLayers.Count() - 1))
            {
                gr.DrawImageUnscaled(layer.Bmp, 0, 0);
            }
        }
        private BitmapData GetBitmapData(Bitmap bitmap)
        {
            return bitmap.LockBits(new Rectangle(0, 0, this.resultBmp.Width, this.resultBmp.Height),
                                         System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                         System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        public void FillPixChanelArrs()
        {
            int[] pixels;
            var data = this.GetBitmapData(this.resultBmp);
            try
            {
                pixels = new int[data.Width * data.Height];
                Marshal.Copy(data.Scan0, pixels, 0, data.Width * data.Height);
            }
            finally
            {
                this.resultBmp.UnlockBits(data);
            }
            var uPixels = pixels.Select(num => (UInt32)num).ToArray();
            this.pixelsNum = uPixels.Length;
            this.pixelsRchan = new int[pixelsNum];
            this.pixelsGchan = new int[pixelsNum];
            this.pixelsBchan = new int[pixelsNum];
            this.pixelsAchan = new int[pixelsNum];
            this.pixelsBrightness = new int[pixelsNum];
            int i = 0;
            foreach (var pixel in uPixels)
            {
                this.pixelsAchan[i] = (int)(pixel >> 24);
                var r = (int)((pixel << 8) >> 24);
                var g = (int)((pixel << 16) >> 24);
                var b = (int)((pixel << 24) >> 24);
                this.pixelsRchan[i] = r;
                this.pixelsGchan[i] = g;
                this.pixelsBchan[i] = b;
                var max = r > g ? r : g;
                if (max < b) max = b;
                var min = r < g ? r : g;
                if (min > b) min = b;
                this.pixelsBrightness[i] = (max + min) / 2;
                i++;
            }
        }

        //действия над слоями
        public int CopyLayer(int index)
        {
            this.layers.Add(new ImageLayer(this.layers[index].Bmp));
            this.SplitActiveLayers();
            this.Draw();
            return this.layers.Count - 1;
        }
        public int  AddLayer()
        {
            this.layers.Add(new ImageLayer(this.resultBmp.Width, this.resultBmp.Height));
            this.SplitActiveLayers();
            this.Draw();
            return this.layers.Count - 1;
        }
        public void DeleteLayer(int index)
        {
            this.layers.RemoveAt(index);
            this.SplitActiveLayers();
            this.Draw();
        }
        public void ExchangeLayers(int indexL1, int indexL2)
        {
            var tmp = this.layers[indexL1];
            this.layers[indexL1] = this.layers[indexL2];
            this.layers[indexL2] = tmp;
            this.SplitActiveLayers();
            this.Draw();
        }
        public void RotateLayer(int index, float degrees)
        {
            layers[index].Rotate(degrees);
            this.SplitActiveLayers();
            this.Draw();
        }
        public void FlipOrMirrorLayer(int index, RotateFlipType type)
        {
            this.layers[index].Flip(type);
            this.SplitActiveLayers();
            this.Draw();
        }
        public int FlattenLayers(IEnumerable<int> enumerable)
        {
            var newLayer = new Bitmap(this.resultBmp.Width, this.resultBmp.Height);
            var gr = Graphics.FromImage(newLayer);
            var zero = new Point(0, 0);
            foreach(var i in enumerable)
            {
                gr.DrawImage(this.layers[i].Bmp, zero);
                this.layers.RemoveAt(i);
            }
            this.layers.Add(new ImageLayer(newLayer));
            this.SplitActiveLayers();
            this.Draw();
            return this.layers.Count - 1;
        }
        public void SetLayerActivity(int index, bool active)
        {
            //var activeYet = this.layers[index].IsVisible;
            this.layers[index].IsVisible = active;
            this.SplitActiveLayers();
            this.Draw();
            //return (active)
        }

        //действия полотна
        public int GetLayersCount(bool checkForActivity)
        {
            if (checkForActivity)
                return this.layers.Where(layer => layer.IsVisible).Count();
            else
                return this.layers.Count;
        }
        public int RotateCanvas(float degrees)
        {
            var activeLayers = this.layers.Where(layer => layer.IsVisible);
            foreach(var layer in activeLayers)
            {
                layer.Rotate(degrees);
            }
            this.SplitActiveLayers();
            this.Draw();
            return activeLayers.Count();
        }
        public int FlipOrMirrorCanvas(RotateFlipType type)
        {
            var activeLayers = this.layers.Where(layer => layer.IsVisible);
            foreach (var layer in activeLayers)
            {
                layer.Flip(type);
            }
            this.resultBmp.RotateFlip(type);
            this.outPort.Invalidate();
            
            this.SplitActiveLayers();
            this.Draw();
            
            return activeLayers.Count();
        }
        public void ChangeResolutionUnscaled(bool centre, int wNew, int hNew)
        {
            this.resultBmp.Dispose();
            this.resultBmp = new Bitmap(wNew, hNew);
            this.graphics = Graphics.FromImage(this.resultBmp);
            this.outPort.Image = this.resultBmp;
            foreach (var layer in this.layers)
            {
                layer.ResizeUnscaled(centre, wNew, hNew);
            }
            this.SplitActiveLayers();
            this.Draw();
        }
        public void ChangeResolutionScaled(int wNew, int hNew)
        {
            this.resultBmp.Dispose();
            this.resultBmp = new Bitmap(wNew, hNew);
            this.graphics = Graphics.FromImage(this.resultBmp);
            this.outPort.Image = this.resultBmp;
            foreach (var layer in this.layers)
            {
                layer.ResizeScaled(wNew, hNew);
            }
            this.SplitActiveLayers();
            this.Draw();
        }
        //public void AddImage(string filename);//добавляет изображение как слой
        public bool OpenImage(string filename)
        {
            this.resultBmp = new Bitmap(filename);
            this.outPort.Image = this.resultBmp;
            this.graphics = Graphics.FromImage(this.resultBmp);
            this.layers.Clear();
            this.layers.Add(new ImageLayer(this.resultBmp));
            this.Filename = filename;
            this.SplitActiveLayers();
            this.Draw();
            return this.layers.Count != 0;
        }
        public bool CreateImage(int w, int h)
        {
            this.Filename = null;
            this.resultBmp = new Bitmap(w, h);
            this.outPort.Image = this.resultBmp;
            this.graphics = Graphics.FromImage(this.resultBmp);
            this.layers.Clear();
            this.layers.Add(new ImageLayer(this.resultBmp));
            this.SplitActiveLayers();
            this.Draw();
            return this.layers.Count != 0;
        }

        public bool SetImage(Bitmap bitmap)
        {
            this.resultBmp = bitmap;
            this.outPort.Image = this.resultBmp;
            this.graphics = Graphics.FromImage(this.resultBmp);
            this.layers.Clear();
            this.layers.Add(new ImageLayer(this.resultBmp));
            this.SplitActiveLayers();
            this.Draw();
            return this.layers.Count != 0;
        }

        public void SaveImage()
        {
            this.SplitActiveLayers();
            this.Draw();
            this.resultBmp.Save(this.Filename);
        }


        public void TryOnTransRGB(int[] transmR, int[] transmG, int[] transmB)
        {
            int[] pixels = new int[pixelsNum];
            for (int i = 0, stp = pixelsNum; i < stp; i++)  
            {
                pixels[i] = (this.pixelsAchan[i] << 24) +
                                (transmR[this.pixelsRchan[i]] << 16) +
                                (transmG[this.pixelsGchan[i]] << 8) +
                                (transmB[this.pixelsBchan[i]]);
            }
            
            var data = this.GetBitmapData(this.resultBmp);
            try
            {
                Marshal.Release(data.Scan0);
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(pixels, 0);
                data.Scan0 = ptr;
            }
            finally
            {
                this.resultBmp.UnlockBits(data);
            }
            this.outPort.Invalidate();
            
            //this.outPort.Refresh();
        }
        public void ApplyTransRGB(int[] transmR, int[] transmG, int[] transmB)
        {
            foreach(var layer in this.layers.Where(layer => layer.IsVisible))
            {
                layer.TransmissionRGB(transmR, transmG, transmB);
            }
            this.SplitActiveLayers();
            this.Draw();
        }
        //инструменты/рисование
        public void ClickMouse(int x,int y)
        {
            this.curTool.ClickMouse(x, y);
            this.Draw();
        }
        public void MoveMouse(int x, int y)
        {
            if (this.drag)
            {
                this.curTool.DragMouse(x, y);
                this.Draw();
            }
        }
        public void PressMouse(int x, int y)
        {
            this.drag = true;
            this.curTool.PressMouse(x, y);
            this.Draw();
        }
        public void ReleaseMouse()
        {
            this.drag = false;
        }
        public void SetTool(GTool type)
        {
            this.tool = type;
            switch (type)
            {
                case GTool.Brush:
                    {
                        this.curTool = this.gBrush;
                        break;
                    }
                case GTool.Pipette:
                    {
                        this.curTool = this.gPipette;
                        break;
                    }
                default:
                    {
                        this.curTool = this.gBrush;
                        break;
                    }
            }
        }

                //Взаимодействие с параметрами инструментов
        public int BrushWidth
        {
            set => this.gBrush.pen.Width = value;
            get => (int)this.gBrush.pen.Width;
        }
        public Color BrushColor
        {
            get => this.gBrush.pen.Color;
            set => this.gBrush.pen.Color = value;
        }

        public void BindPipetteBufColor(bool first)
        {
            if (first) 
                this.gPipette.BindColor(ref this.firstColor);
            else
                this.gPipette.BindColor(ref this.secondColor);
        }
        //Перестройка инструментов
        private void RefreshTools()
        {
            this.gBrush.SetImage(this.capLayer);
            this.gPipette.SetBitmap(this.resultBmp);
        }
        private void CreateTools(Graphics graphics)
        {
            this.firstColor = Color.FromArgb(0, 0, 0);
            this.secondColor = Color.FromArgb(255, 255, 255);
            this.gBrush = new GBrush(graphics);
            this.gBrush.pen.Color = this.firstColor;
            this.gBrush.pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round,
                                                     System.Drawing.Drawing2D.LineCap.Round,
                                                     System.Drawing.Drawing2D.DashCap.Flat);
            this.gPipette = new GPipette(new Bitmap(1, 1), ref this.firstColor);
        }

    }
}
