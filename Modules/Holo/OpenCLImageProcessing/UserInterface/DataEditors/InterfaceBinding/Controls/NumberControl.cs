using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public partial class NumberControl : UserControl
    {
        private float _max;
        private float _min;
        private float _value;
        private float _step;

        public string Title
        {
            get => lblTitle.Text;
            set => lblTitle.Text = value;
        }

        public float Maximum
        {
            get => _max;
            set => _max = value;
        }

        public float Minimum
        {
            get => _min;
            set => _min = value;
        }

        public float Step
        {
            get => _step;
            set
            {
                _step = value;
                UpdateLabels();
            }
        }

        public float Value
        {
            get => _value; 
            set
            {
                _value = value;
                UpdateLabels();
                ValueChanged?.Invoke();
            }
        }

        public event Action ValueChanged;

        public NumberControl()
        {
            InitializeComponent();

            txtCurrent.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                    e.Handled = true;

                // only allow one decimal point
                if (e.KeyChar == '.' && txtCurrent.Text.IndexOf('.') > -1)
                    e.Handled = true;
            };

            txtCurrent.TextChanged += (sender, args) =>
            {
                if (txtCurrent.Focused && float.TryParse(txtCurrent.Text, out float value))
                {
                    Value = value;
                }
            };
        }
        
        private void UpdateLabels()
        {
            if (!txtCurrent.Focused)
                txtCurrent.Text = Value.ToString();
        }

        private void SliderControl_Paint(object sender, PaintEventArgs e)
        {
            var pen = new Pen(Color.LightGray, 1.0f)
            {
                DashStyle = DashStyle.Dash
            };
            e.Graphics.DrawLine(pen, 0, 1, Width, 1);
        }

        private void SliderControl_Load(object sender, EventArgs e)
        {

        }

        private void AddStep(float multiplier)
        {
            Value += _step * multiplier;
            UpdateLabels();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddStep(1);
        }

        private void btnAdd10_Click(object sender, EventArgs e)
        {
            AddStep(10);
        }

        private void btnSubstract_Click(object sender, EventArgs e)
        {
            AddStep(-1);
        }

        private void btnSubstract10_Click(object sender, EventArgs e)
        {
            AddStep(-10);
        }
    }
}