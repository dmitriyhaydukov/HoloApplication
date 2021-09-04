using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Controls.SliderElements;

namespace Common.Controls
{
    public partial class Slider : UserControl
    {
        protected const int PaddingX = 10;
        protected const int PaddingY = 10;

        protected float _min = 0;
        protected float _max = 1;
        protected float _val = 0;
        protected float _step = 0.1f;

        protected int _slideLineStartX => PaddingX;
        protected int _sliderLineEndX => Width - PaddingX;
        protected int _valX => ConvertValueToXPos(_val);
        protected bool _moving = false;

        internal SliderPin.PinStyle _pinStyle = new SliderPin.PinStyle();
        internal SliderLine.LineStyle _lineStyle = new SliderLine.LineStyle();

        public float Min
        {
            get => _min;
            set
            {
                _min = value;
                Refresh();
            }
        }

        public float Max
        {
            get => _max;
            set
            {
                _max = value;
                Refresh();
            }
        }

        public float Value
        {
            get => _val;
            set
            {
                _val = value;

                if (_val > _max)
                    _val = _max;

                if (_val < _min)
                    _val = _min;

                Refresh();
            }
        }

        public float Step
        {
            get => _step;
            set
            {
                _step = value;
                Refresh();
            }
        }

        public event Action ValueChanged;

        public Slider()
        {
            InitializeComponent();
        }

        private void Slider_Paint(object sender, PaintEventArgs e)
        {
            if (_min == _max)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            var x0 = _slideLineStartX;
            var x1 = _sliderLineEndX;
            var xv = _valX;
            SliderLine.Draw(e.Graphics, _lineStyle, x0, x1, xv, PaddingY);
            SliderPin.Draw(e.Graphics, _pinStyle, xv, PaddingY);
        }

        private void Slider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && SliderPin.CheckCoord(_pinStyle, _valX, PaddingY, e.X, e.Y))
                _moving = true;
        }

        private void Slider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _moving)
            {
                _val = ConvertXPosToValue(e.X);
                ValueChanged?.Invoke();
                Refresh();
            }
            else if (e.Button == MouseButtons.None)
            {
                if (SliderPin.CheckCoord(_pinStyle, _valX, PaddingY, e.X, e.Y))
                    Cursor = Cursors.SizeWE;
                else
                    Cursor = Cursors.Default;
            }
        }

        private void Slider_MouseUp(object sender, MouseEventArgs e)
        {
            _moving = false;
        }

        protected float ConvertXPosToValue(int x)
        {
            var val = (_max - _min) / (_sliderLineEndX - _slideLineStartX) * (x - _slideLineStartX) + _min;

            if (val < _min)
                val = _min;

            if (val > _max)
                val = _max;

            if (Math.Abs(_step) > float.Epsilon)
                val = (float)Math.Ceiling(val / _step) * _step;

            return val;
        }

        protected int ConvertValueToXPos(float val)
        {
            return (int) ((_sliderLineEndX - _slideLineStartX) / (_max - _min) * (val - _min) + _slideLineStartX);
        }
    }
}