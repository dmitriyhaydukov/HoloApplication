using System.Collections.Generic;
using System;

namespace rab1
{
    class Polynomial
    {
        private List<double> coefficients = new List<double>();

        public double AugmentCoefficient(int pos, double val)
        {
            if (pos < 0)
                return Double.NaN;
            var len = this.coefficients.Count;
            while (pos >= len)
            {
                this.coefficients.Add(0d);
            }
            this.coefficients[pos] += val;
            return this.coefficients[pos];
        }
        public void SetCoefficient(int pos, double val)
        {
            if (pos < 0)
                return;
            var len = this.coefficients.Count;
            while (pos >= len)
            {
                this.coefficients.Add(0d);
            }
            this.coefficients[pos] = val;
        }
        public double Calculate(double x)
        {
            double ret = 0d;
            var pow = 0;
            foreach (var coef in this.coefficients)
            {
                ret += coef * Math.Pow(x, pow);
                pow++;
            }
            return ret;
        }
        public double[] Calculate(double[] xValues)
        {
            var len = xValues.Length;
            //int count = this.coefficients.Count;
            int pow;
            var ret = new double[len];
            for (int i = 0; i < len; i++)
            {
                pow = 0;
                foreach (var coef in this.coefficients)
                {
                    ret[i] += coef * Math.Pow(xValues[i], pow);
                    pow++;
                }
            }
            return ret;
        }
        public double[] Calculate(double[] xValues, double ceil, double floor)
        {
            var len = xValues.Length;
            int pow;
            double val;
            var ret = new double[len];
            for (int i = 0; i < len; i++)
            {
                pow = 0;
                val = 0d;
                foreach (var coef in this.coefficients)
                {
                    val += coef * Math.Pow(xValues[i], pow);
                    pow++;
                }
                ret[i] = Math.Max(floor, Math.Min(ceil, val));
            }
            return ret;
        }
        public Polynomial(double[] coefficients)
        {
            if (coefficients.Length != 0)
                this.coefficients = new List<double>(coefficients);
            else
                this.coefficients = new List<double> { 0d };
        }
        public Polynomial(double coefficient0, double coefficient1)
        {
            this.coefficients = new List<double> { coefficient0, coefficient1 };
        }
        public override string ToString()
        {
            string ret = "";
            if (this.coefficients.Count == 0)
                return "0";
            this.coefficients.Reverse();
            try
            {
                var pow = this.coefficients.Count;
                foreach (var coef in this.coefficients)
                {
                    pow--;
                    ret += " + " + coef.ToString() + " x^" + pow.ToString();
                }
            }
            finally
            {
                this.coefficients.Reverse();
            }
            //потому что первым стоит ненужный " + "
            return ret.Substring(3);//base.ToString();
        }
        public int ShrinkToSignificant()
        {
            var empty = 0;
            this.coefficients.Reverse();
            try
            {
                var unlocker = true;
                foreach (var coeff in this.coefficients)
                {
                    if (unlocker)
                    {
                        if (coeff == 0d)
                            empty++;
                        else
                            unlocker = false;
                    }
                }
            }
            finally
            {
                this.coefficients.Reverse();
            }
            var count = this.coefficients.Count;
            this.coefficients.RemoveRange(Math.Max(1, count - empty), Math.Min(count - 1, empty));
            return empty;
        }

        public static Polynomial operator +(Polynomial pol, double x)
        {
            var arr = pol.coefficients.ToArray();
            arr[0] += x;
            return new Polynomial(arr);
        }
        public static Polynomial operator -(Polynomial pol, double x)
        {
            var arr = pol.coefficients.ToArray();
            arr[0] -= x;
            return new Polynomial(arr);
        }
        public static Polynomial operator *(Polynomial pol, double x)
        {
            var arr = pol.coefficients.ToArray();
            for (int i = 0, stp = arr.Length; i < stp; i++)
            {
                arr[i] *= x;
            }
            return new Polynomial(arr);
        }
        public static Polynomial operator /(Polynomial pol, double x)
        {
            var arr = pol.coefficients.ToArray();
            for (int i = 0, stp = arr.Length; i < stp; i++)
            {
                arr[i] /= x;
            }
            return new Polynomial(arr);
        }
        public static Polynomial operator +(Polynomial pol1, Polynomial pol2)
        {
            Polynomial bigger, smaller;
            if (pol2.coefficients.Count > pol1.coefficients.Count)
            {
                bigger = pol2;
                smaller = pol1;
            }
            else
            {
                bigger = pol1;
                smaller = pol2;
            }
            var arr = bigger.coefficients.ToArray();
            var i = 0;
            foreach (var coeff in smaller.coefficients)
            {
                arr[i] += coeff;
                i++;
            }
            return new Polynomial(arr);
        }
        public static Polynomial operator -(Polynomial pol1, Polynomial pol2)
        {
            var count = Math.Max(pol2.coefficients.Count, pol1.coefficients.Count);
            var arr = new double[count];
            var i = 0;
            foreach (var coeff in pol1.coefficients)
            {
                arr[i] = coeff;
                i++;
            }
            i = 0;
            foreach (var coeff in pol2.coefficients)
            {
                arr[i] -= coeff;
                i++;
            }
            return new Polynomial(arr);
        }
        public static Polynomial operator *(Polynomial pol1, Polynomial pol2)
        {
            var arr = new double[(pol2.coefficients.Count + pol1.coefficients.Count - 1)];
            int i = 0, j = 0;
            foreach (var coeff1 in pol1.coefficients)
            {
                j = 0;
                foreach (var coeff2 in pol2.coefficients)
                {
                    arr[i + j] += coeff1 * coeff2;
                    j++;
                }
                i++;
            }
            var ret = new Polynomial(arr);
            //ret.ShrinkToSignificant();
            return ret;
        }
        public static Polynomial operator -(Polynomial pol)
        {
            return pol * (-1d);
        }
    }

    class Interpolation
    {
        public static Polynomial LagrangePolynomial(int size, double[] xValues, double[] yValues)
        {
            if (xValues.Length != yValues.Length || xValues.Length != size)
                throw new Exception("Еhe lengths of arrays do not match the declared size.");
            var ret = new Polynomial(0d, 0d);
            for (int i = 0; i < size; i++)
            {
                var basicsPol = new Polynomial(1d, 0d);
                for (int j = 0; j < size; j++)
                {
                    if (j != i)
                        basicsPol *= (new Polynomial(-xValues[j], 1d)) / (xValues[i] - xValues[j]);
                }
                ret += basicsPol * yValues[i];
            }
            ret.ShrinkToSignificant();
            return ret;
        }
    }
}
