using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace rab1
{
    class VisualPolynomial
    {
        SortedDictionary<double, double> points;
        private int leftBound;
        private int rightBound;
        private int topBound;
        private int bottomBound;
        private int numOfVals;
        private double[] vals;
        private double[] xPts;
        Bitmap bmp;
        Graphics gr;

        public VisualPolynomial(int left, int right, int bottom, int top, int count, double firstVal, double finalVal, Bitmap bitmap)
        {
            this.bmp = bitmap ?? throw new Exception("Bitmap does not exist!");
            this.leftBound = left;
            this.rightBound = right;
            this.topBound = top;
            this.bottomBound = bottom;
            this.vals = new double[count];
            this.numOfVals = count;
            this.gr = Graphics.FromImage(this.bmp);
            this.points = new SortedDictionary<double, double>();
            this.SetEdgeVal(firstVal, finalVal);
            this.SetxPts();
            this.CalcVals();
        }

        private void SetEdgeVal(double fst, double fnl)
        {
            this.points.Add(this.leftBound, fst);
            this.points.Add(this.rightBound, fnl);
        }
        private double SetxPts()
        {
            this.xPts = new double[this.numOfVals];
            double step = (this.rightBound - this.leftBound) / ((double)this.numOfVals-1);
            for (int i = 0; i < this.numOfVals; i++)
            {
                this.xPts[i] = i * step;
            }
            return step;
        }
        private void CalcVals()
        {
            this.vals = Interpolation.LagrangePolynomial(this.points.Count, this.points.Keys.ToArray(),
                        this.points.Values.ToArray()).Calculate(this.xPts, this.topBound, this.bottomBound);
        }
        
        public PointF AddPoint(int xPic, int yPic)
        {
            var coeffH = (double)this.numOfVals / this.bmp.Width;// ед/пикс
            var coeffV = (this.topBound - this.bottomBound) / (double)this.bmp.Height;//  ед/пикс
            double x = xPic * coeffH;
            double y = yPic * coeffV;

            if (this.points.ContainsKey(x))
                //Не будем добавлять узел и внешне об этом можно узнать
                return new PointF(float.NaN, float.NaN);

            this.points.Add(x, y);

            this.CalcVals();

            this.Draw();

            return new PointF((float)x, (float)y);
        }
        public bool DeletePoint(int xPic, int yPic)
        {
            var coeffH = (double)this.numOfVals / this.bmp.Width;// ед/пикс
            var coeffV = (this.topBound - this.bottomBound) / (double)this.bmp.Height;//  ед/пикс
            double x = xPic * coeffH;
            double y = yPic * coeffV;
            if (x == this.leftBound || x == this.rightBound)
                return false;
            if (this.points.ContainsKey(x)) 
                if (this.points[x] == y)
                {
                    //if(this.points[])
                    this.points.Remove(x);
                    this.CalcVals();
                    this.Draw();
                    return true;
                }
            return false;
        }
        public bool DeletePoint(double key)
        {
            if (!this.points.ContainsKey(key)) 
                return false;
            if (key == this.leftBound || key == this.rightBound)
                //нельзя удалять крайние узлы
                return false;
            this.points.Remove(key);
            this.CalcVals();
            this.Draw();
            return true;
        }
        public double ReplacePoint(double key, int xPicCur, int yPicCur)
        {
            var coeffH = (double)this.numOfVals / this.bmp.Width;// ед/пикс
            var coeffV = (this.topBound - this.bottomBound) / (double)this.bmp.Height;//  ед/пикс
            double xCur = xPicCur * coeffH;
            double yCur = yPicCur * coeffV;

            if (!this.points.ContainsKey(key)) // || this.points.ContainsKey(xCur))
                return double.NaN;

            //если самый левый/самый правый узел, то двигать можно только по У
            if (this.leftBound == key || this.rightBound == key)
            {
                this.points[key] = yCur;
                this.CalcVals();
                this.Draw();
                return key;
            }
            else
            {
                this.points.Remove(key);
                if (this.points.ContainsKey(xCur))
                    points[xCur] = yCur;
                else
                    this.points.Add(xCur, yCur);
                this.CalcVals();
                this.Draw();
                return xCur;
            }
        }
        public double ReplacePoint(int xPicPrev, int yPicPrev, int xPicCur, int yPicCur)
        {
            var coeffH = (double)this.numOfVals / this.bmp.Width;// ед/пикс
            var coeffV = (this.topBound - this.bottomBound) / (double)this.bmp.Height;//  ед/пикс
            double xPrev = xPicPrev * coeffH;
            double yPrev = yPicPrev * coeffV;
            double xCur = xPicCur * coeffH;
            double yCur = yPicCur * coeffV;

            if (!this.points.ContainsKey(xPrev))// || this.points.ContainsKey(xCur))  
                return double.NaN;
            if (this.points[xPrev] != yPrev)
                return double.NaN;
            //если самый левый/самый правый узел, то двигать можно только по У
            if (this.leftBound == xPrev || this.rightBound == xPrev) 
            {
                this.points[xPrev] = yCur;
                this.CalcVals();
                this.Draw();
                return xPrev;
            }
            else
            {
                this.points.Remove(xPrev);
                if (this.points.ContainsKey(xCur))
                    points[xCur] = yCur;
                else
                    this.points.Add(xCur, yCur);
                this.CalcVals();
                this.Draw();
                return xCur;
            }
        }
        public double IsTherePointNearly(int xPic, int yPic, double xRad, double yRad)
        {
            var coeffH = (double)this.numOfVals / this.bmp.Width;// ед/пикс
            var coeffV = (this.topBound - this.bottomBound) / (double)this.bmp.Height;//  ед/пикс
            double x = xPic * coeffH;
            double y = yPic * coeffV;

            var arrKeys = this.points.Keys.OrderBy(xVal => Math.Abs(xVal - x));
            foreach(var key in arrKeys)
            {
                if (Math.Abs(key - x) > xRad)
                    return double.NaN;
                if (Math.Abs(this.points[key] - y) <= yRad)
                    return key;
            }
            return double.NaN;
        }
        public void SetActive()
        {
            this.Draw();
        }
        public int[] GetIntArr()
        {
            return this.vals.Select(val => (int)Math.Round(val)).ToArray();
        }
        private void Draw()
        {
            var pointToLine = new PointF[this.numOfVals];
            var stepHor = this.bmp.Width / (float)this.numOfVals;// пикс/точек
            var stepVer = this.bmp.Height / (float)(this.topBound - this.bottomBound);
            for (int i = this.numOfVals - 1; i >= 0; i--) 
            {
                pointToLine[i] = new PointF(i * stepHor, (float)this.bmp.Height - (float)vals[i] * stepVer);
            }
            this.gr.Clear(Color.Transparent);//почистили
            this.gr.DrawLines(new Pen(Color.Black, 3f), pointToLine);//нарисили прямую
            var brush = Brushes.Red;
            int x, y;
            foreach(var point in this.points)//выделяем задающие точки
            {
                x = (int)Math.Round(point.Key * stepHor) - 4;
                y = (int)Math.Round(this.bmp.Height - point.Value * stepVer) - 4;
                this.gr.FillEllipse(brush, x, y, 8, 8);
            }
        }
        
        public void SetPointsValues(double[] values)
        {
            this.points.Clear();

            for (int k = 0; k < values.Length; k++)
            {
                double key = k * 16;
                if (!this.points.ContainsKey(key))
                {
                    this.points.Add(key, values[k]);
                }
            }
            
            this.CalcVals();

            this.Draw();
            
            //this.vals = values;
            //this.Draw();
        }
    }
}