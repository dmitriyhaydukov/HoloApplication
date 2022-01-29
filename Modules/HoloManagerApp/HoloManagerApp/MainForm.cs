﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.IO;

using HoloCommon.MemoryManagement;
using HoloCommon.ProcessManagement;
using HoloCommon.Synchronization;
using HoloCommon.Serialization.Charting;
using HoloCommon.Models.General;
using HoloCommon.Models.Charting;

using ExtraLibrary.ImageProcessing;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Transformation;
using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Geometry2D;

namespace HoloManagerApp
{
    public partial class MainForm : Form
    {
        private const int PICTURE_TAKEN_DELAY = 2000;

        private const int M1 = 213;
        private const int M2 = 167;

        private const int MAX_RANGE_VALUE = 800;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnCorrectedGraph_Click(object sender, EventArgs e)
        {
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\CorrectedGraph\CorrectedGraph\bin\Debug\CorrectedGraph.exe", null, false);
        }

        private void btnCreateInterferogram_Click(object sender, EventArgs e)
        {
            double phaseShift = 0;
            string arguments = 
                string.Format(
                    "{0} {1} {2}",
                    phaseShift.ToString(CultureInfo.InvariantCulture),
                    MAX_RANGE_VALUE.ToString(CultureInfo.InvariantCulture),
                    M1.ToString(CultureInfo.InvariantCulture)
                );

            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\InterferogramCreatorConsoleApp\InterferogramCreatorConsoleApp\bin\Debug\InterferogramCreatorConsoleApp.exe", arguments, false);
        }

        private void btnTakePicture_Click(object sender, EventArgs e)
        {
            SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Camera.TAKE_PICTURE);
        }       
        private void btnTakeSeries_Click(object sender, EventArgs e)
        {
            double phaseShift = 0;
            double phaseShiftStep = GetPhaseShiftStep();

            Action takePictureAction = () =>
            {
                Thread.Sleep(PICTURE_TAKEN_DELAY);
                SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Camera.TAKE_PICTURE);
            };

            Action pictureTakenAction = () =>
            {
                if (phaseShift < 2 * Math.PI)
                {
                    CreateInterferogram(phaseShift);
                    phaseShift += phaseShiftStep;
                }
            };

            Thread thread1 = SynchronizationManager.RunActionOnSignal(pictureTakenAction, HoloCommon.Synchronization.Events.Image.IMAGE_SAVED);
            Thread thread2 = SynchronizationManager.RunActionOnSignal(takePictureAction, HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);

            SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);
        }
        

        /*
        private void btnTakeSeries_Click(object sender, EventArgs e)
        {
            double phaseShift = 0;
            double phaseShiftStep = GetPhaseShiftStep();

            Action action = () =>
            {
                Thread.Sleep(PICTURE_TAKEN_DELAY);
                if (phaseShift < 2 * Math.PI)
                {
                    CreateInterferogram(phaseShift);
                    phaseShift += phaseShiftStep;
                }
            };
            
            Thread thread2 = SynchronizationManager.RunActionOnSignal(action, HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);

            SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);
        }
        */

        private void CreateInterferogram(double phaseShift)
        {
            string arguments = phaseShift.ToString(CultureInfo.InvariantCulture);
                
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\InterferogramCreatorConsoleApp\InterferogramCreatorConsoleApp\bin\Debug\InterferogramCreatorConsoleApp.exe", arguments, false, false);
        }

        private double GetPhaseShiftStep()
        {
            return  2 * Math.PI / 220;
        }

        private void btnGraphFromImages_Click(object sender, EventArgs e)
        {
            string directoryPath = @"D:\Images\!";

            int x = 2690;
            int y = 1990;

            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);
                IEnumerable<string> sortedFiles = files.OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f)));

                int n = 0;

                int minIntensity = 255;
                int maxIntensity = 0;
                List<ChartPoint> points = new List<ChartPoint>();

                foreach(string file in sortedFiles)
                {
                    using (Bitmap bitmap = new Bitmap(file))
                    {
                        System.Drawing.Color color = bitmap.GetPixel(x, y);
                        int intensity = ColorWrapper.GetGrayIntensity(color);
                        if (intensity < minIntensity)
                        {
                            minIntensity = intensity;
                        }
                        if (intensity > maxIntensity)
                        {
                            maxIntensity = intensity;
                        }

                        ChartPoint chartPoint = new ChartPoint(n, intensity);
                        points.Add(chartPoint);
                        n++;
                    }
                }

                Chart chart1 = new Chart()
                {
                    SeriesCollection = new List<ChartSeries>()
                    {
                        new ChartSeries()
                        {
                            Name = "Grpah",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(255, 0, 0),
                            Points = points
                        }
                    }
                };

                MemoryWriter.Write<Chart>(chart1, new ChartSerialization());
                ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

                Thread.Sleep(2000);

                Interval<double> startInterval = new Interval<double>(minIntensity, maxIntensity);
                Interval<double> finishInterval = new Interval<double>(-1, 1);
                RealIntervalTransform transform = new RealIntervalTransform(startInterval, finishInterval);
                List<ChartPoint> points2 = new List<ChartPoint>();
                foreach(ChartPoint p in points)
                {
                    double newValue = transform.TransformToFinishIntervalValue(p.Y);
                    ChartPoint point = new ChartPoint(p.X, newValue);
                    points2.Add(point);
                }

                Chart chart2 = new Chart()
                {
                    SeriesCollection = new List<ChartSeries>()
                    {
                        new ChartSeries()
                        {
                            Name = "Grpah2",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(0, 255, 0),
                            Points = points2
                        }
                    }
                };

                MemoryWriter.Write<Chart>(chart2, new ChartSerialization());
                ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

                Thread.Sleep(2000);

                List<ChartPoint> points3 = new List<ChartPoint>();
                foreach (ChartPoint p in points2)
                {
                    double newValue = Math.Acos(p.Y);
                    ChartPoint point = new ChartPoint(p.X, newValue);
                    points3.Add(point);
                }

                Chart chart3 = new Chart()
                {
                    SeriesCollection = new List<ChartSeries>()
                    {
                        new ChartSeries()
                        {
                            Name = "Grpah3",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(0, 0, 0),
                            Points = points3
                        }
                    }
                };

                MemoryWriter.Write<Chart>(chart3, new ChartSerialization());
                ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

                Thread.Sleep(2000);
            }
        }

        private void btnIntensityIncrease_Click(object sender, EventArgs e)
        {
            int count = 1000;
            double step = GetStep();

            List<double> valuesList = new List<double>();

            double x = 0;
            for (int k = 0; k < count; k++)
            {
                double value = Math.Cos(x);
                valuesList.Add(value);
                x += step;
            }

            List<ChartPoint> points = new List<ChartPoint>();
            for (int k = 0; k < count; k++)
            {
                ChartPoint p = new ChartPoint(k, valuesList[k]);
                points.Add(p);
            }

            Chart chart1 = new Chart()
            {
                SeriesCollection = new List<ChartSeries>()
                    {
                        new ChartSeries()
                        {
                            Name = "Graph",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(255, 0, 0),
                            Points = points
                        }
                    }
            };

            MemoryWriter.Write<Chart>(chart1, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);

            Interval<double> startInterval = new Interval<double>(-1, 1);
            Interval<double> finishInterval = new Interval<double>(0, MAX_RANGE_VALUE);
            RealIntervalTransform transform = new RealIntervalTransform(startInterval, finishInterval);
            List<ChartPoint> points2 = new List<ChartPoint>();
            for (int k = 0; k < count; k++)
            {
                double newValue = transform.TransformToFinishIntervalValue(valuesList[k]);
                ChartPoint p = new ChartPoint(k, newValue);
                points2.Add(p);
            }
            
            List<ChartPoint> points3 = new List<ChartPoint>();
            for(int k = 0; k < points2.Count; k++)
            {
                ChartPoint p = points2[k];
                double v = p.Y % M1;
                ChartPoint newPoint = new ChartPoint(p.X, v);
                points3.Add(newPoint);
            }

            List<ChartPoint> points4 = new List<ChartPoint>();
            for (int k = 0; k < points2.Count; k++)
            {
                ChartPoint p = points2[k];
                double v = p.Y % M2;
                ChartPoint newPoint = new ChartPoint(p.X, v);
                points4.Add(newPoint);
            }

            Chart chart2 = new Chart()
            {
                SeriesCollection = new List<ChartSeries>()
                    {
                        new ChartSeries()
                        {
                            Name = "Graph",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(0, 0, 0),
                            Points = points2
                        },
                        new ChartSeries()
                        {
                            Name = "M1",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(0, 255, 0),
                            Points = points3
                        },
                        new ChartSeries()
                        {
                            Name = "M2",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(255, 0, 0),
                            Points = points4
                        }
                    }
            };

            MemoryWriter.Write<Chart>(chart2, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);
            
            List<ChartPoint> points5 = new List<ChartPoint>();
            
            for (int k = 0; k < points3.Count; k++)
            {
                ChartPoint m1Point = points3[k];
                ChartPoint m2Point = points4[k];

                ChartPoint point = new ChartPoint(m1Point.Y, m2Point.Y);
                //ChartPoint point = new ChartPoint(m1Point.Y - M1, M2 - m2Point.Y);
                points5.Add(point);
            }

            List<Point2D> pointsDiagonal = ModularArithmeticHelper.BuildTable(M1, M2, MAX_RANGE_VALUE);

            List<ChartPoint> points6 = new List<ChartPoint>();
            for (int k = 0; k < pointsDiagonal.Count; k++)
            {
                Point2D p = pointsDiagonal[k];
                ChartPoint p1 = new ChartPoint(p.X, p.Y);
                //ChartPoint p1 = new ChartPoint(p.X - M1, M2 - p.Y);
                points6.Add(p1);
            }

            Chart chart5 = new Chart()
            {
                //InvertAxisX = true,
                //InvertAxisY = true,
                SeriesCollection = new List<ChartSeries>()
                    {
                        new ChartSeries()
                        {
                            Name = "Diagonal",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(0, 255, 0),
                            Points = points6
                        },

                        new ChartSeries()
                        {
                            Name = "Points",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Bubble,
                            ColorDescriptor = new ColorDescriptor(0, 0, 0),
                            Points = points5
                        }
                    }
            };

            MemoryWriter.Write<Chart>(chart5, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);
        }
        
        private double GetStep()
        {
            return 0.01;
        }

        private void btnBuildTable_Click(object sender, EventArgs e)
        {
            List<Point2D> points = ModularArithmeticHelper.BuildTable(M1, M2, MAX_RANGE_VALUE);

            List<ChartPoint> chartPoints = new List<ChartPoint>();
            for (int k = 0; k < points.Count; k++)
            {
                Point2D p = points[k];
                ChartPoint p1 = new ChartPoint(p.X, p.Y);
                chartPoints.Add(p1);
            }

            Chart chart1 = new Chart()
            {
                SeriesCollection = new List<ChartSeries>()
                    {
                        new ChartSeries()
                        {
                            Name = "Graph diagonals",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(255, 0, 0),
                            Points = chartPoints
                        }
                    }
            };

            MemoryWriter.Write<Chart>(chart1, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);
        }

        private void btnCreateInterferogram2_Click(object sender, EventArgs e)
        {
            double phaseShift = 0;
            string arguments =
                string.Format(
                    "{0} {1} {2}",
                    phaseShift.ToString(CultureInfo.InvariantCulture),
                    MAX_RANGE_VALUE.ToString(CultureInfo.InvariantCulture),
                    M2.ToString(CultureInfo.InvariantCulture)
                );

            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\InterferogramCreatorConsoleApp\InterferogramCreatorConsoleApp\bin\Debug\InterferogramCreatorConsoleApp.exe", arguments, false);
        }

        private void btnBuildRealTable_Click(object sender, EventArgs e)
        {
            string imagePath1 = @"D:\Images\20220122-Croped\Image1.png";
            string imagePath2 = @"D:\Images\20220122-Croped\Image2.png";

            int row = 150;

            WriteableBitmap bitmap1 = WriteableBitmapCreator.CreateWriteableBitmapFromFile(imagePath1);
            WriteableBitmap bitmap2 = WriteableBitmapCreator.CreateWriteableBitmapFromFile(imagePath2);

            WriteableBitmapWrapper wrapper1 = WriteableBitmapWrapper.Create(bitmap1);
            WriteableBitmapWrapper wrapper2 = WriteableBitmapWrapper.Create(bitmap2);
                        
            double[] rowValues1 = wrapper1.GetGrayScaleMatrix().GetRow(row);
            double[] rowValues2 = wrapper2.GetGrayScaleMatrix().GetRow(row);

            double min1 = rowValues1.Min();
            double min2 = rowValues2.Min();
            
            double max1 = rowValues1.Max();
            double max2 = rowValues2.Max();

            double min = Math.Min(min1, min2);
            double max = Math.Max(max1, max2);

            //Interval<double> startInterval = new Interval<double>(min, max);
            Interval<double> startInterval1 = new Interval<double>(min1, max1);
            Interval<double> startInterval2 = new Interval<double>(min2, max2);

            Interval<double> intervalM1 = new Interval<double>(0, M1);
            Interval<double> intervalM2 = new Interval<double>(0, M2);

            RealIntervalTransform intervalTransformM1 = new RealIntervalTransform(startInterval1, intervalM1);
            RealIntervalTransform intervalTransformM2 = new RealIntervalTransform(startInterval2, intervalM2);

            double[] values1 = rowValues1.Select(x => intervalTransformM1.TransformToFinishIntervalValue(x)).ToArray();
            double[] values2 = rowValues2.Select(x => intervalTransformM2.TransformToFinishIntervalValue(x)).ToArray();

            List<ChartPoint> chartPoints = new List<ChartPoint>();
            for (int k = 0; k < values1.Length; k++)
            {
                ChartPoint p1 = new ChartPoint(values1[k], values2[k]);
                chartPoints.Add(p1);
            }

            List<Point2D> pointsIdeal = ModularArithmeticHelper.BuildTable(M1, M2, MAX_RANGE_VALUE);

            List<ChartPoint> chartPointsIdeal = new List<ChartPoint>();
            for (int k = 0; k < pointsIdeal.Count; k++)
            {
                Point2D p = pointsIdeal[k];
                ChartPoint p1 = new ChartPoint(p.X, p.Y);
                chartPointsIdeal.Add(p1);
            }

            Chart chart = new Chart()
            {
                SeriesCollection = new List<ChartSeries>()
                    {
                        new ChartSeries()
                        {
                            Name = "Diagonals",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(0, 125, 0),
                            Points = chartPointsIdeal
                        },
                        new ChartSeries()
                        {
                            Name = "Distribution",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Bubble,
                            ColorDescriptor = new ColorDescriptor(255, 0, 0),
                            Points = chartPoints
                        }
                    }
            };

            MemoryWriter.Write<Chart>(chart, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);
        }
    }
}