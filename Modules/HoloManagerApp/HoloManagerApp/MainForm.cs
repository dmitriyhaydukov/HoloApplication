using System;
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
using ExtraLibrary.Arraying;
using ExtraLibrary.ImageProcessing;

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
            //int timesM1 = MAX_RANGE_VALUE / M1;
            //int maxRange = M1 * timesM1;
            int maxRange = MAX_RANGE_VALUE;

            double phaseShift = int.Parse(txtPhaseShift.Text);
            string arguments = 
                string.Format(
                    "{0} {1} {2}",
                    phaseShift.ToString(CultureInfo.InvariantCulture),
                    maxRange.ToString(CultureInfo.InvariantCulture),
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

            Dictionary<int, List<Point2D>> notDiagonalPointsDictionary = null;
            List<Point2D> unwrappedPoints = null;
            List<Point2D> resCorrectedPoints = null;
            List<Point2D> specialPoints = null;
            List<Point2D> specialPointsCorrected = null;

            List<Point2D> specialPoints2 = null;
            List<Point2D> specialPointsCorrected2 = null;

            List<ChartPoint> points6 = new List<ChartPoint>();

            List<Point2D> pointsDiagonal = ModularArithmeticHelper.BuildTable
                (
                    M1, M2, MAX_RANGE_VALUE, false, points6, 
                    out notDiagonalPointsDictionary, 
                    out unwrappedPoints, 
                    out resCorrectedPoints, 
                    out specialPoints,
                    out specialPointsCorrected,
                    out specialPoints2,
                    out specialPointsCorrected2
                );

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
            Dictionary<int, List<Point2D>> notDiagonalPointsDictionary = null;
            List<Point2D> unwrappedPoints = null;
            List<Point2D> resCorrectedPoints = null;
            List<Point2D> specialPoints = null;
            List<Point2D> specialPointsCorrected = null;

            List<Point2D> specialPoints2 = null;
            List<Point2D> specialPointsCorrected2 = null;

            List<ChartPoint> chartPoints = new List<ChartPoint>();

            List<Point2D> points = ModularArithmeticHelper.BuildTable
                (
                    M1, M2, MAX_RANGE_VALUE, false, chartPoints, 
                    out notDiagonalPointsDictionary, 
                    out unwrappedPoints, 
                    out resCorrectedPoints, 
                    out specialPoints,
                    out specialPointsCorrected,
                    out specialPoints2,
                    out specialPointsCorrected2
                );

            //List<ChartPoint> chartPoints = new List<ChartPoint>();
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
            //int timesM2 = MAX_RANGE_VALUE / M2;
            //int maxRange = M2 * timesM2;
            int maxRange = MAX_RANGE_VALUE;

            double phaseShift = int.Parse(txtPhaseShift.Text);
            string arguments =
                string.Format(
                    "{0} {1} {2}",
                    phaseShift.ToString(CultureInfo.InvariantCulture),
                    maxRange.ToString(CultureInfo.InvariantCulture),
                    M2.ToString(CultureInfo.InvariantCulture)
                );

            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\InterferogramCreatorConsoleApp\InterferogramCreatorConsoleApp\bin\Debug\InterferogramCreatorConsoleApp.exe", arguments, false);
        }

        private void btnBuildRealTable_Click(object sender, EventArgs e)
        {
            //string imagePath1 = @"D:\Images\20220312-Cropped\Image1.png";
            //string imagePath2 = @"D:\Images\20220312-Cropped\Image2.png";

            //string imagePath1 = @"D:\Images\20220521\5-Cropped\Image01.png";
            //string imagePath2 = @"D:\Images\20220521\5-Cropped\Image02.png";

            string imagePath1 = @"D:\Images\20220526-Cropped-Filtered4\Image1.png";
            string imagePath2 = @"D:\Images\20220526-Cropped-Filtered4\Image2.png";

            int row = 50;

            WriteableBitmap bitmap1 = WriteableBitmapCreator.CreateWriteableBitmapFromFile(imagePath1);
            WriteableBitmap bitmap2 = WriteableBitmapCreator.CreateWriteableBitmapFromFile(imagePath2);

            WriteableBitmapWrapper wrapper1 = WriteableBitmapWrapper.Create(bitmap1);
            WriteableBitmapWrapper wrapper2 = WriteableBitmapWrapper.Create(bitmap2);
                        
            double[] rowValues1 = wrapper1.GetGrayScaleMatrix().GetRow(row);
            double[] rowValues2 = wrapper2.GetGrayScaleMatrix().GetRow(row);
            
            /*
            //Filter row values
            RealMatrix matrix1 = new RealMatrix(1, rowValues1.Length);
            RealMatrix matrix2 = new RealMatrix(1, rowValues2.Length);

            for (int i = 0; i < rowValues1.Length; i++)
            {
                matrix1[0, i] = rowValues1[i]; 
            }

            for (int i = 0; i < rowValues2.Length; i++)
            {
                matrix2[0, i] = rowValues2[i];
            }

            int window = 11;
            double threshold = 30;
            SmartEdgeByRowsGrayScaleFilter smartFilter = new SmartEdgeByRowsGrayScaleFilter();
            matrix1 = smartFilter.ExecuteFiltration(matrix1, window, threshold);
            matrix2 = smartFilter.ExecuteFiltration(matrix2, window, threshold);

            rowValues1 = matrix1.GetRow(0);
            rowValues2 = matrix2.GetRow(0);
            */

            List<ChartPoint> chartOriginalPoints1 = new List<ChartPoint>();
            for (int k = 0; k < rowValues1.Length; k++)
            {
                ChartPoint p = new ChartPoint(k, rowValues1[k]);
                chartOriginalPoints1.Add(p);
            }

            List<ChartPoint> chartOriginalPoints2 = new List<ChartPoint>();
            for (int k = 0; k < rowValues2.Length; k++)
            {
                ChartPoint p = new ChartPoint(k, rowValues2[k]);
                chartOriginalPoints2.Add(p);
            }

            Chart chartOriginal = new Chart() { SeriesCollection = new List<ChartSeries>() };
            chartOriginal.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Original1",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(255, 0, 0),
                Points = chartOriginalPoints1
            });
            chartOriginal.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Original2",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(0, 0, 255),
                Points = chartOriginalPoints2
            });

            MemoryWriter.Write<Chart>(chartOriginal, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);

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


            List<ChartPoint> chartTransformPoints1 = new List<ChartPoint>();
            for (int k = 0; k < values1.Length; k++)
            {
                ChartPoint p = new ChartPoint(k, values1[k]);
                chartTransformPoints1.Add(p);
            }

            List<ChartPoint> chartTransformPoints2 = new List<ChartPoint>();
            for (int k = 0; k < values2.Length; k++)
            {
                ChartPoint p = new ChartPoint(k, values2[k]);
                chartTransformPoints2.Add(p);
            }

            Chart chartTransform = new Chart() { SeriesCollection = new List<ChartSeries>() };
            chartTransform.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Transform1",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(255, 0, 0),
                Points = chartTransformPoints1
            });
            chartTransform.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Transform2",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(0, 0, 255),
                Points = chartTransformPoints2
            });

            MemoryWriter.Write<Chart>(chartTransform, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);

            List<ChartPoint> chartPoints = new List<ChartPoint>();
            for (int k = 0; k < values1.Length; k++)
            {
                ChartPoint p1 = new ChartPoint(values1[k], values2[k]);
                chartPoints.Add(p1);
            }

            Dictionary<int, List<Point2D>> notDiagonalPointsDictionary = null;
            List<Point2D> unwrappedPoints = null;
            List<Point2D> resCorrectedPoints = null;
            List<Point2D> specialPoints = null;
            List<Point2D> specialPointsCorrected = null;

            List<Point2D> specialPoints2 = null;
            List<Point2D> specialPointsCorrected2 = null;

            bool readDiagonalsFromFile = true;

            List<Point2D> filteredPoints = null;

            List<Point2D> pointsIdeal = ModularArithmeticHelper.BuildTable
                (
                    M1, M2, MAX_RANGE_VALUE, readDiagonalsFromFile, chartPoints, 
                    out notDiagonalPointsDictionary, 
                    out unwrappedPoints, 
                    out resCorrectedPoints, 
                    out specialPoints,
                    out specialPointsCorrected,
                    out specialPoints2,
                    out specialPointsCorrected2
                );
            
            int height = 300;
            int width = resCorrectedPoints.Count();
            RealMatrix resMatrix = new RealMatrix(height, width);

            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < resCorrectedPoints.Count; k++)
                {
                    Point2D p = resCorrectedPoints[k];
                    resMatrix[j, k] = p.Y;
                }
            }

            int windowSize = 11;
            MedianByRowsGrayScaleFilter medianByRowsGrayScaleFilter = new MedianByRowsGrayScaleFilter();
            RealMatrix filteredMatrix = medianByRowsGrayScaleFilter.ExecuteFiltration(resMatrix, windowSize);

            //Create filtered points
            filteredPoints = new List<Point2D>();
            double[] filteredRowValues = filteredMatrix.GetRow(1);
            for (int k = 0; k < filteredRowValues.Length; k ++)
            {
                filteredPoints.Add(new Point2D(k, filteredRowValues[k]));
            }

            List<ChartPoint> chartPointsIdeal = new List<ChartPoint>();
            for (int k = 0; k < pointsIdeal.Count; k++)
            {
                Point2D p = pointsIdeal[k];
                ChartPoint p1 = new ChartPoint(p.X, p.Y);
                chartPointsIdeal.Add(p1);
            }

            Chart chart = new Chart() { SeriesCollection = new List<ChartSeries>() };

            System.Drawing.Color[] colors = GetVaryingColors(10).ToArray();

            foreach(KeyValuePair<int, List<Point2D>> kvp in notDiagonalPointsDictionary)
            {
                
                System.Drawing.Color color = colors[kvp.Key];

                ChartSeries chartSeries = new ChartSeries()
                {
                    Name = kvp.Key.ToString(),
                    Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Bubble,
                    ColorDescriptor = new ColorDescriptor(Convert.ToByte(color.R), Convert.ToByte(color.G), Convert.ToByte(color.B)),
                    Points = kvp.Value.Select(a => new ChartPoint(a.X, a.Y)).ToList()
                };

                chart.SeriesCollection.Add(chartSeries);
            }

            chart.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Distribution",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Bubble,
                ColorDescriptor = new ColorDescriptor(255, 0, 0),
                Points = chartPoints
            });

            chart.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Diagonals",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Bubble,
                ColorDescriptor = new ColorDescriptor(0, 125, 0),
                Points = chartPointsIdeal
            });

            chart.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Special points",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Bubble,
                ColorDescriptor = new ColorDescriptor(255, 215, 0),
                Points = specialPoints.Select(x => new ChartPoint(x.X, x.Y)).ToList()
            });

            chart.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Special points 2",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Bubble,
                ColorDescriptor = new ColorDescriptor(0, 0, 255),
                Points = specialPoints2.Select(x => new ChartPoint(x.X, x.Y)).ToList()
            });

            MemoryWriter.Write<Chart>(chart, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);
                       
            Chart chartUnwrapped = new Chart() { SeriesCollection = new List<ChartSeries>() };
            chartUnwrapped.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Unwrapped",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Bubble,
                ColorDescriptor = new ColorDescriptor(255, 0, 0),
                Points =  unwrappedPoints.Select(x => new ChartPoint(x.X, x.Y)).ToList()
            });

            MemoryWriter.Write<Chart>(chartUnwrapped, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);

            Chart chartCorrected = new Chart() { SeriesCollection = new List<ChartSeries>() };
            chartCorrected.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Corrected",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(255, 0, 0),
                Points = resCorrectedPoints.Select(x => new ChartPoint(x.X, x.Y)).ToList()
            });

            
            chartCorrected.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Special points",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Bubble,
                ColorDescriptor = new ColorDescriptor(255, 215, 0),
                Points = specialPointsCorrected.Select(x => new ChartPoint(x.X, x.Y)).ToList()
            });

            chartCorrected.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Special points 2",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Bubble,
                ColorDescriptor = new ColorDescriptor(0, 0, 255),
                Points = specialPointsCorrected2.Select(x => new ChartPoint(x.X, x.Y)).ToList()
            });

            MemoryWriter.Write<Chart>(chartCorrected, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);
                                 
            Chart chartFiltered = new Chart() { SeriesCollection = new List<ChartSeries>() };
            chartFiltered.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Filtered",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(255, 0, 0),
                Points = filteredPoints.Select(x => new ChartPoint(x.X, x.Y)).ToList()
            });

            MemoryWriter.Write<Chart>(chartFiltered, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);

            //double minValue = resCorrectedPoints.Select(x => x.Y).Min();
            //double maxValue = resCorrectedPoints.Select(x => x.Y).Max();

            //Interval<double> startInterval = new Interval<double>(minValue, maxValue);
            //Interval<double> finishInterval = new Interval<double>(0, 255);
            //RealIntervalTransform transform = new RealIntervalTransform(startInterval, finishInterval);

            /*
            int height = 400;
            int width = resCorrectedPoints.Count();
            RealMatrix resMatrix = new RealMatrix(height, width);

            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < resCorrectedPoints.Count; k++)
                {
                    Point2D p = resCorrectedPoints[k];
                    resMatrix[j, k] = p.Y;
                }
            }
            */

            /*
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < resCorrectedPoints.Count; k++)
                {
                    Point2D p = resCorrectedPoints[k];
                    resMatrix[j, k] = transform.TransformToFinishIntervalValue(p.Y);
                }
            }

            WriteableBitmap resBitmap = WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix(resMatrix, 72, 72);
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create(resBitmap);

            string fileName = @"D:\Images\20220526-Res\ResImage.png";
            wrapper.SaveToPngFile(fileName);
            */
        }

        IEnumerable<System.Drawing.Color> GetVaryingColors(int seedIndex)
        {
            List<System.Drawing.Color> colorsList = new List<System.Drawing.Color>();

            int maxValue = 1 << 24;
            int index = seedIndex % maxValue;

            for (int k = 0; k < 10; k++)
            {
                byte r = 0;
                byte g = 0;
                byte b = 0;

                for (int i = 0; i < 24; i++)
                {
                    if ((index & (1 << i)) != 0)
                    {
                        switch (i % 3)
                        {
                            case 0: r |= (byte)(1 << (23 - i) / 3); break;
                            case 1: g |= (byte)(1 << (23 - i) / 3); break;
                            case 2: b |= (byte)(1 << (23 - i) / 3); break;
                        }
                    }
                }

                colorsList.Add(System.Drawing.Color.FromArgb(0xFF, r, g, b));

                index = (index + 1) % maxValue;
            }

            return colorsList;
        }

        private void btnCreateTriangleImage_Click(object sender, EventArgs e)
        {
            int maxRange = MAX_RANGE_VALUE;

            double phaseShift = 0;
            string arguments =
                string.Format(
                    "{0} {1} {2}",
                    phaseShift.ToString(CultureInfo.InvariantCulture),
                    maxRange.ToString(CultureInfo.InvariantCulture),
                    M1.ToString(CultureInfo.InvariantCulture)
                );

            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\TriangleImageCreatorConsoleApp\TriangleImageCreatorConsoleApp\bin\Debug\TriangleImageCreatorConsoleApp.exe", arguments, false);
        }
    }
}