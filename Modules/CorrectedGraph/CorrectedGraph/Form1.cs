using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HoloCommon.MemoryManagement;
using HoloCommon.ProcessManagement;
using HoloCommon.Models.General;
using HoloCommon.Models.Charting;
using HoloCommon.Serialization.Charting;

namespace CorrectedGraph
{
    public partial class Form1 : Form
    {
        double[] cl = { 35, 50, 58, 65, 72, 78, 85, 94, 100, 108, 118, 132, 149, 168, 192, 255 };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            double[] values = Clin(cl, 256, 4096);

            List<ChartPoint> chartPoints = new List<ChartPoint>();
            for (int k = 0; k < values.Length; k++)
            {
                ChartPoint point = new ChartPoint(k, values[k]);
                chartPoints.Add(point);
            }

            Chart chart = new Chart()
            {
                SeriesCollection = new List<ChartSeries>()
                {
                    new ChartSeries()
                    {
                        Name = "Graphic",
                        ColorDescriptor = new ColorDescriptor(255, 0, 0),
                        Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                        Points = chartPoints
                    }
                }
            };

            MemoryWriter.Write<Chart>(chart, new ChartSerialization());
            ProcessManager.RunProcess(@"d:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false);
        }

        private double[] Clin(double[] cl, int kv, int nx)
        {
            int nx1 = cl.GetLength(0);
            double[] cl1 = new double[nx1];
            for (int i = 0; i < nx1; i++) cl1[i] = cl[i];

            int nx2 = nx / 16;

            double[] am = new double[nx];
            int nsd = 0;
            switch (kv)
            {
                case 16: for (int i = 0; i < nx; i++) { am[i] = cl1[i / nx2]; } break; 
                case 32:
                    cl1 = MasX2(cl1);
                    nx2 = nx / 32;
                    nsd = nx / 32;
                    break;  // 4096-128
                case 64:
                    cl1 = MasX2(cl1); cl1 = MasX2(cl1);
                    nx2 = nx / 64;
                    nsd = nx / 32 + nx / 64;
                    break;
                case 128:
                    cl1 = MasX2(cl1); cl1 = MasX2(cl1); cl1 = MasX2(cl1);
                    nx2 = nx / 128;
                    nsd = nx / 32 + nx / 64 + nx / 128;
                    break;
                case 256:
                    cl1 = MasX2(cl1); cl1 = MasX2(cl1); cl1 = MasX2(cl1); cl1 = MasX2(cl1);
                    nx2 = nx / 256;
                    nsd = nx / 32 + nx / 64 + nx / 128 + nx / 256;
                    break;
                default: return null;
            }
            for (int i = 0; i < nx; i++) { am[i] = cl1[i / nx2]; }
            am = Strerch(am, nsd);
            return am;
        }

        /// <summary>
        /// Растяжение массива 0 - n-nx -> 0 - n
        /// </summary>
        /// <param name="am"></param>
        /// <returns></returns>
        private double[] Strerch(double[] am, int nx)
        {
            int n = am.Length; double[] am2 = new double[n];
            int n1 = n - nx;

            for (int i = 0; i < n; i++)
            {
                int i1 = (n1 - 1) * i / n;
                am2[i] = am[i1];
            }
            return am2;
        }

        private double[] MasX2(double[] am)
        {
            int nx = am.Length;
            int nx2 = nx * 2;
            double[] am2 = new double[nx2];

            for (int i = 0; i < nx2 - 1; i++)
            {
                if (i % 2 == 0) am2[i] = am[i / 2];
                if (i % 2 != 0) am2[i] = (am[i / 2] + am[i / 2 + 1]) / 2;
            }

            return am2;
        }
    }
}
