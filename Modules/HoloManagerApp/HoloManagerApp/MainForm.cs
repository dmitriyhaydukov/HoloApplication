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

using Interferometry.InterferogramDecoding;

namespace HoloManagerApp
{
    public partial class MainForm : Form
    {
        private const int PICTURE_TAKEN_DELAY = 2000;

        private const int M1 = 213;
        private const int M2 = 167;

        private const int MAX_RANGE_VALUE = 800;

        private const double GAP_DIFFERENCE_VALUE = 60;

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
                    "{0} {1} {2} {3}",
                    phaseShift.ToString(CultureInfo.InvariantCulture),
                    maxRange.ToString(CultureInfo.InvariantCulture),
                    M1.ToString(CultureInfo.InvariantCulture),
                    1.ToString()
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
                            ColorDescriptor = new ColorDescriptor(0, 125, 0),
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

            double intersectionLineThreshold = 100;
            double prevValue = 0;

            int maxLinesCount1 = 4;
            int maxLinesCount2 = 4;

            List<List<ChartPoint>> intersectionLinePointsList1 = new List<List<ChartPoint>>();
            List<List<ChartPoint>> intersectionLinePointsList2 = new List<List<ChartPoint>>();

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

                if (Math.Abs(prevValue - v) > intersectionLineThreshold && intersectionLinePointsList1.Count < maxLinesCount1)
                {
                    List<ChartPoint> list = new List<ChartPoint>();
                    for (int j = 0; j < points2.Count; j++)
                    {
                        list.Add(new ChartPoint(j, p.Y));
                    }
                    intersectionLinePointsList1.Add(list);
                }

                prevValue = v;
            }

            List<ChartPoint> points4 = new List<ChartPoint>();
            for (int k = 0; k < points2.Count; k++)
            {
                ChartPoint p = points2[k];
                double v = p.Y % M2;
                ChartPoint newPoint = new ChartPoint(p.X, v);
                points4.Add(newPoint);

                if (Math.Abs(prevValue - v) > intersectionLineThreshold && intersectionLinePointsList2.Count < maxLinesCount2)
                {
                    List<ChartPoint> list = new List<ChartPoint>();
                    for (int j = 0; j < points2.Count; j++)
                    {
                        list.Add(new ChartPoint(j, p.Y));
                    }
                    intersectionLinePointsList2.Add(list);
                }

                prevValue = v;
            }

            Chart chart2 = new Chart()
            {
                SeriesCollection = new List<ChartSeries>()
                    {
                        new ChartSeries()
                        {
                            Name = "Graph",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(0, 0, 255),
                            Points = points2
                        },
                        new ChartSeries()
                        {
                            Name = "M1",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(0, 125, 0),
                            Points = points3
                        },
                        new ChartSeries()
                        {
                            Name = "M2",
                            Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                            ColorDescriptor = new ColorDescriptor(125, 0, 0),
                            Points = points4
                        }
                    }
            };

            for (int i = 0; i < intersectionLinePointsList1.Count; i++)
            {
                List<ChartPoint> list = intersectionLinePointsList1[i];
                chart2.SeriesCollection.Add(new ChartSeries()
                {
                    Name = "Intersection 1",
                    Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                    ColorDescriptor = new ColorDescriptor(0, 255, 0),
                    Points = list
                });
            }

            for (int i = 0; i < intersectionLinePointsList2.Count; i++)
            {
                List<ChartPoint> list = intersectionLinePointsList2[i];
                chart2.SeriesCollection.Add(new ChartSeries()
                {
                    Name = "Intersection 2",
                    Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                    ColorDescriptor = new ColorDescriptor(255, 0, 0),
                    Points = list
                });
            }

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
                    "{0} {1} {2} {3}",
                    phaseShift.ToString(CultureInfo.InvariantCulture),
                    maxRange.ToString(CultureInfo.InvariantCulture),
                    M2.ToString(CultureInfo.InvariantCulture),
                    1.ToString()
                );

            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\InterferogramCreatorConsoleApp\InterferogramCreatorConsoleApp\bin\Debug\InterferogramCreatorConsoleApp.exe", arguments, false);
        }

        private void btnBuildRealTable_Click(object sender, EventArgs e)
        {
            //string imagePath1 = @"D:\Images\20220312-Cropped\Image1.png";
            //string imagePath2 = @"D:\Images\20220312-Cropped\Image2.png";

            //string imagePath1 = @"D:\Images\20220521\5-Cropped\Image01.png";
            //string imagePath2 = @"D:\Images\20220521\5-Cropped\Image02.png";

            //string imagePath1 = @"D:\Images\20220526-Cropped-Filtered4\Image1.png";
            //string imagePath2 = @"D:\Images\20220526-Cropped-Filtered4\Image2.png";

            string imagePath1 = @"D:\Images\20221202\Cropped\3.png";
            string imagePath2 = @"D:\Images\20221202\Cropped\7.png";

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
                ColorDescriptor = new ColorDescriptor(0, 125, 0),
                Points = chartOriginalPoints1
            });
            chartOriginal.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Original2",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(255, 0, 0),
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
                ColorDescriptor = new ColorDescriptor(0, 125, 0),
                Points = chartTransformPoints1
            });
            chartTransform.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Transform2",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(255, 0, 0),
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
                ColorDescriptor = new ColorDescriptor(0, 255, 0),
                Points = chartPointsIdeal
            });

            /*
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
            */

            MemoryWriter.Write<Chart>(chart, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);
                       
            Chart chartUnwrapped = new Chart() { SeriesCollection = new List<ChartSeries>() };
            chartUnwrapped.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Unwrapped",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Bubble,
                ColorDescriptor = new ColorDescriptor(0, 0, 255),
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
                ColorDescriptor = new ColorDescriptor(0, 0, 255),
                Points = resCorrectedPoints.Select(x => new ChartPoint(x.X, x.Y)).ToList()
            });

            /*
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
            */

            MemoryWriter.Write<Chart>(chartCorrected, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);

            Thread.Sleep(2000);
                                 
            Chart chartFiltered = new Chart() { SeriesCollection = new List<ChartSeries>() };
            chartFiltered.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Filtered",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(0, 0, 0),
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

        private void btnTakePicturesWithPhaseShifts_Click(object sender, EventArgs e)
        {
            bool isM1 = true;
            double[] phaseShifts = new double[] { 0, 45, 90, 135 };

            int phaseShiftIndex = 0;
            double phaseShift = phaseShifts[phaseShiftIndex];

            Action takePictureAction = () =>
            {
                Thread.Sleep(PICTURE_TAKEN_DELAY);
                SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Camera.TAKE_PICTURE);
            };

            Action pictureTakenAction = () =>
            {
                if (phaseShiftIndex < phaseShifts.Length)
                {
                    phaseShift = phaseShifts[phaseShiftIndex];
                    if (isM1)
                    {
                        CreateInterferogramForSeriesM1(phaseShift);
                    }
                    else
                    {
                        CreateInterferogramForSeriesM2(phaseShift);
                    }
                    phaseShiftIndex++;
                }
                else
                {
                    if (!isM1)
                    {
                        return;
                    }

                    isM1 = false;
                    phaseShiftIndex = 0;
                    SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_SAVED);
                }
            };

            Thread thread1 = SynchronizationManager.RunActionOnSignal(pictureTakenAction, HoloCommon.Synchronization.Events.Image.IMAGE_SAVED);
            Thread thread2 = SynchronizationManager.RunActionOnSignal(takePictureAction, HoloCommon.Synchronization.Events.Image.IMAGE_UPDATED);

            SynchronizationManager.SetSignal(HoloCommon.Synchronization.Events.Image.IMAGE_SAVED);
        }

        private void CreateInterferogramForSeriesM1(double phaseShift)
        {
            int maxRange = MAX_RANGE_VALUE;
            string arguments =
            string.Format(
                "{0} {1} {2}",
                phaseShift.ToString(CultureInfo.InvariantCulture),
                maxRange.ToString(CultureInfo.InvariantCulture),
                M1.ToString(CultureInfo.InvariantCulture)
            );

            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\InterferogramCreatorConsoleApp\InterferogramCreatorConsoleApp\bin\Debug\InterferogramCreatorConsoleApp.exe", arguments, false);
        }

        private void CreateInterferogramForSeriesM2(double phaseShift)
        {
            int maxRange = MAX_RANGE_VALUE;
            string arguments =
            string.Format(
                "{0} {1} {2}",
                phaseShift.ToString(CultureInfo.InvariantCulture),
                maxRange.ToString(CultureInfo.InvariantCulture),
                M2.ToString(CultureInfo.InvariantCulture)
            );

            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\InterferogramCreatorConsoleApp\InterferogramCreatorConsoleApp\bin\Debug\InterferogramCreatorConsoleApp.exe", arguments, false);
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {            
            string shift1_imagePath1 = @"D:\Images\20221202\Cropped\1.png";
            string shift1_imagePath2 = @"D:\Images\20221202\Cropped\5.png";

            string shift2_imagePath1 = @"D:\Images\20221202\Cropped\2.png";
            string shift2_imagePath2 = @"D:\Images\20221202\Cropped\6.png";

            string shift3_imagePath1 = @"D:\Images\20221202\Cropped\3.png";
            string shift3_imagePath2 = @"D:\Images\20221202\Cropped\7.png";

            string shift4_imagePath1 = @"D:\Images\20221202\Cropped\4.png";
            string shift4_imagePath2 = @"D:\Images\20221202\Cropped\8.png";

            int row = 50;


            WriteableBitmap bitmap_shift1_image1 = WriteableBitmapCreator.CreateWriteableBitmapFromFile(shift1_imagePath1);
            WriteableBitmap bitmap_shift1_image2 = WriteableBitmapCreator.CreateWriteableBitmapFromFile(shift1_imagePath2);

            WriteableBitmap bitmap_shift2_image1 = WriteableBitmapCreator.CreateWriteableBitmapFromFile(shift2_imagePath1);
            WriteableBitmap bitmap_shift2_image2 = WriteableBitmapCreator.CreateWriteableBitmapFromFile(shift2_imagePath2);

            WriteableBitmap bitmap_shift3_image1 = WriteableBitmapCreator.CreateWriteableBitmapFromFile(shift3_imagePath1);
            WriteableBitmap bitmap_shift3_image2 = WriteableBitmapCreator.CreateWriteableBitmapFromFile(shift3_imagePath2);

            WriteableBitmap bitmap_shift4_image1 = WriteableBitmapCreator.CreateWriteableBitmapFromFile(shift4_imagePath1);
            WriteableBitmap bitmap_shift4_image2 = WriteableBitmapCreator.CreateWriteableBitmapFromFile(shift4_imagePath2);

            //bitmap wrappers
            WriteableBitmapWrapper wrapper_shift1_image1 = WriteableBitmapWrapper.Create(bitmap_shift1_image1);
            WriteableBitmapWrapper wrapper_shift1_image2 = WriteableBitmapWrapper.Create(bitmap_shift1_image2);

            WriteableBitmapWrapper wrapper_shift2_image1 = WriteableBitmapWrapper.Create(bitmap_shift2_image1);
            WriteableBitmapWrapper wrapper_shift2_image2 = WriteableBitmapWrapper.Create(bitmap_shift2_image2);

            WriteableBitmapWrapper wrapper_shift3_image1 = WriteableBitmapWrapper.Create(bitmap_shift3_image1);
            WriteableBitmapWrapper wrapper_shift3_image2 = WriteableBitmapWrapper.Create(bitmap_shift3_image2);

            WriteableBitmapWrapper wrapper_shift4_image1 = WriteableBitmapWrapper.Create(bitmap_shift4_image1);
            WriteableBitmapWrapper wrapper_shift4_image2 = WriteableBitmapWrapper.Create(bitmap_shift4_image2);

            //row values
            double[] shift1_rowValues1 = wrapper_shift1_image1.GetGrayScaleMatrix().GetRow(row);
            double[] shift1_rowValues2 = wrapper_shift1_image2.GetGrayScaleMatrix().GetRow(row);

            double[] shift2_rowValues1 = wrapper_shift2_image1.GetGrayScaleMatrix().GetRow(row);
            double[] shift2_rowValues2 = wrapper_shift2_image2.GetGrayScaleMatrix().GetRow(row);

            double[] shift3_rowValues1 = wrapper_shift3_image1.GetGrayScaleMatrix().GetRow(row);
            double[] shift3_rowValues2 = wrapper_shift3_image2.GetGrayScaleMatrix().GetRow(row);

            double[] shift4_rowValues1 = wrapper_shift4_image1.GetGrayScaleMatrix().GetRow(row);
            double[] shift4_rowValues2 = wrapper_shift4_image2.GetGrayScaleMatrix().GetRow(row);

            double shift1_min1 = shift1_rowValues1.Min();
            double shift1_min2 = shift1_rowValues2.Min();

            double shift2_min1 = shift2_rowValues1.Min();
            double shift2_min2 = shift2_rowValues2.Min();

            double shift3_min1 = shift3_rowValues1.Min();
            double shift3_min2 = shift3_rowValues2.Min();

            double shift4_min1 = shift4_rowValues1.Min();
            double shift4_min2 = shift4_rowValues2.Min();


            double shift1_max1 = shift1_rowValues1.Max();
            double shift1_max2 = shift1_rowValues2.Max();

            double shift2_max1 = shift2_rowValues1.Max();
            double shift2_max2 = shift2_rowValues2.Max();

            double shift3_max1 = shift3_rowValues1.Max();
            double shift3_max2 = shift3_rowValues2.Max();

            double shift4_max1 = shift4_rowValues1.Max();
            double shift4_max2 = shift4_rowValues2.Max();
            

            double shift1_min = Math.Min(shift1_min1, shift1_min2);
            double shift1_max = Math.Max(shift1_max1, shift1_max2);

            double shift2_min = Math.Min(shift2_min1, shift2_min2);
            double shift2_max = Math.Max(shift2_max1, shift2_max2);

            double shift3_min = Math.Min(shift3_min1, shift3_min2);
            double shift3_max = Math.Max(shift3_max1, shift3_max2);

            double shift4_min = Math.Min(shift4_min1, shift4_min2);
            double shift4_max = Math.Max(shift4_max1, shift4_max2);
            
            Interval<double> shift1_startInterval1 = new Interval<double>(shift1_min1, shift1_max1);
            Interval<double> shift1_startInterval2 = new Interval<double>(shift1_min2, shift1_max2);

            Interval<double> shift2_startInterval1 = new Interval<double>(shift2_min1, shift2_max1);
            Interval<double> shift2_startInterval2 = new Interval<double>(shift2_min2, shift2_max2);

            Interval<double> shift3_startInterval1 = new Interval<double>(shift3_min1, shift3_max1);
            Interval<double> shift3_startInterval2 = new Interval<double>(shift3_min2, shift3_max2);

            Interval<double> shift4_startInterval1 = new Interval<double>(shift4_min1, shift4_max1);
            Interval<double> shift4_startInterval2 = new Interval<double>(shift4_min2, shift4_max2);


            Interval<double> intervalM1 = new Interval<double>(0, M1);
            Interval<double> intervalM2 = new Interval<double>(0, M2);

            RealIntervalTransform shift1_intervalTransformM1 = new RealIntervalTransform(shift1_startInterval1, intervalM1);
            RealIntervalTransform shift1_intervalTransformM2 = new RealIntervalTransform(shift1_startInterval2, intervalM2);

            RealIntervalTransform shift2_intervalTransformM1 = new RealIntervalTransform(shift2_startInterval1, intervalM1);
            RealIntervalTransform shift2_intervalTransformM2 = new RealIntervalTransform(shift2_startInterval2, intervalM2);

            RealIntervalTransform shift3_intervalTransformM1 = new RealIntervalTransform(shift3_startInterval1, intervalM1);
            RealIntervalTransform shift3_intervalTransformM2 = new RealIntervalTransform(shift3_startInterval2, intervalM2);

            RealIntervalTransform shift4_intervalTransformM1 = new RealIntervalTransform(shift4_startInterval1, intervalM1);
            RealIntervalTransform shift4_intervalTransformM2 = new RealIntervalTransform(shift4_startInterval2, intervalM2);
            
            double[] shift1_values1 = shift1_rowValues1.Select(x => shift1_intervalTransformM1.TransformToFinishIntervalValue(x)).ToArray();
            double[] shift1_values2 = shift1_rowValues2.Select(x => shift1_intervalTransformM2.TransformToFinishIntervalValue(x)).ToArray();

            double[] shift2_values1 = shift2_rowValues1.Select(x => shift2_intervalTransformM1.TransformToFinishIntervalValue(x)).ToArray();
            double[] shift2_values2 = shift2_rowValues2.Select(x => shift2_intervalTransformM2.TransformToFinishIntervalValue(x)).ToArray();

            double[] shift3_values1 = shift3_rowValues1.Select(x => shift3_intervalTransformM1.TransformToFinishIntervalValue(x)).ToArray();
            double[] shift3_values2 = shift3_rowValues2.Select(x => shift3_intervalTransformM2.TransformToFinishIntervalValue(x)).ToArray();

            double[] shift4_values1 = shift4_rowValues1.Select(x => shift4_intervalTransformM1.TransformToFinishIntervalValue(x)).ToArray();
            double[] shift4_values2 = shift4_rowValues2.Select(x => shift4_intervalTransformM2.TransformToFinishIntervalValue(x)).ToArray();

            List<ChartPoint> shift1_chartPoints = new List<ChartPoint>();
            for (int k = 0; k < shift1_values1.Length; k++)
            {
                ChartPoint p = new ChartPoint(shift1_values1[k], shift1_values2[k]);
                shift1_chartPoints.Add(p);
            }

            List<ChartPoint> shift2_chartPoints = new List<ChartPoint>();
            for (int k = 0; k < shift2_values1.Length; k++)
            {
                ChartPoint p = new ChartPoint(shift2_values1[k], shift2_values2[k]);
                shift2_chartPoints.Add(p);
            }

            List<ChartPoint> shift3_chartPoints = new List<ChartPoint>();
            for (int k = 0; k < shift3_values1.Length; k++)
            {
                ChartPoint p = new ChartPoint(shift3_values1[k], shift3_values2[k]);
                shift3_chartPoints.Add(p);
            }

            List<ChartPoint> shift4_chartPoints = new List<ChartPoint>();
            for (int k = 0; k < shift4_values1.Length; k++)
            {
                ChartPoint p = new ChartPoint(shift4_values1[k], shift4_values2[k]);
                shift4_chartPoints.Add(p);
            }

            Dictionary<int, List<Point2D>> notDiagonalPointsDictionary = null;
            List<Point2D> unwrappedPoints = null;

            List<Point2D> shift1_resCorrectedPoints = null;
            List<Point2D> shift2_resCorrectedPoints = null;
            List<Point2D> shift3_resCorrectedPoints = null;
            List<Point2D> shift4_resCorrectedPoints = null;

            List<Point2D> specialPoints = null;
            List<Point2D> specialPointsCorrected = null;

            List<Point2D> specialPoints2 = null;
            List<Point2D> specialPointsCorrected2 = null;

            bool readDiagonalsFromFile = true;

            List<Point2D> filteredPoints = null;

            List<Point2D> shift1_pointsIdeal = ModularArithmeticHelper.BuildTable
                (
                    M1, M2, MAX_RANGE_VALUE, readDiagonalsFromFile, shift1_chartPoints,
                    out notDiagonalPointsDictionary,
                    out unwrappedPoints,
                    out shift1_resCorrectedPoints,
                    out specialPoints,
                    out specialPointsCorrected,
                    out specialPoints2,
                    out specialPointsCorrected2
                );

            RealMatrix shift1Matrix = new RealMatrix(1, shift1_resCorrectedPoints.Count);
            for (int j = 0; j < shift1_resCorrectedPoints.Count; j++)
            {
                shift1Matrix[0, j] = shift1_resCorrectedPoints[j].Y;
            }
            
            List<int> indecesWithNoValue = new List<int>();
            double? prevValue = null;
            for (int j = 0; j < shift1_resCorrectedPoints.Count; j++)
            {
                double value = shift1_resCorrectedPoints[j].Y;
                if (prevValue.HasValue && Math.Abs((double)(value - prevValue)) > GAP_DIFFERENCE_VALUE)
                {
                    indecesWithNoValue.Add(j);   
                }
                else
                {
                    prevValue = value;
                }
            }
            
            foreach(int ind in indecesWithNoValue)
            {
                shift1_resCorrectedPoints[ind] = null;
            }

            List<Tuple<int, int>> gaps = new List<Tuple<int, int>>();
                        
            int startIndex = 0;
            int endIndex = 0;

            for (int j = 0; j < shift1_resCorrectedPoints.Count; j++)
            {
                Point2D point = shift1_resCorrectedPoints[j];
                if (point == null && j > endIndex)
                {
                    startIndex = j;
                    int k = startIndex;
                    while (point == null)
                    {
                        k++;
                        point = shift1_resCorrectedPoints[k];
                    }
                    endIndex = k;
                    gaps.Add(new Tuple<int, int>(startIndex, endIndex));
                }
            }







            List<Point2D> shift2_pointsIdeal = ModularArithmeticHelper.BuildTable
                (
                    M1, M2, MAX_RANGE_VALUE, readDiagonalsFromFile, shift2_chartPoints,
                    out notDiagonalPointsDictionary,
                    out unwrappedPoints,
                    out shift2_resCorrectedPoints,
                    out specialPoints,
                    out specialPointsCorrected,
                    out specialPoints2,
                    out specialPointsCorrected2
                );

            List<Point2D> shift3_pointsIdeal = ModularArithmeticHelper.BuildTable
                (
                    M1, M2, MAX_RANGE_VALUE, readDiagonalsFromFile, shift3_chartPoints,
                    out notDiagonalPointsDictionary,
                    out unwrappedPoints,
                    out shift3_resCorrectedPoints,
                    out specialPoints,
                    out specialPointsCorrected,
                    out specialPoints2,
                    out specialPointsCorrected2
                );

            List<Point2D> shift4_pointsIdeal = ModularArithmeticHelper.BuildTable
                (
                    M1, M2, MAX_RANGE_VALUE, readDiagonalsFromFile, shift4_chartPoints,
                    out notDiagonalPointsDictionary,
                    out unwrappedPoints,
                    out shift4_resCorrectedPoints,
                    out specialPoints,
                    out specialPointsCorrected,
                    out specialPoints2,
                    out specialPointsCorrected2
                );

            const int SLEEP = 5000;
                        
            Chart shift1_chartCorrected = new Chart() { SeriesCollection = new List<ChartSeries>() };
            shift1_chartCorrected.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Corrected 1",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Bubble,
                ColorDescriptor = new ColorDescriptor(0, 0, 255),
                Points = shift1_resCorrectedPoints.Select(x => x != null ? new ChartPoint(x.X, x.Y) : null).ToList()
            });
            /*
            shift1_chartCorrected.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Original",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(0, 255, 0),
                Points = originalRowValues.Select((x, i) => new ChartPoint(i, x)).ToList()
            });
            */

            MemoryWriter.Write<Chart>(shift1_chartCorrected, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);
            Thread.Sleep(SLEEP);
            
            /*
            Chart shift2_chartCorrected = new Chart() { SeriesCollection = new List<ChartSeries>() };
            shift2_chartCorrected.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Corrected 2",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(0, 0, 255),
                Points = shift2_resCorrectedPoints.Select(x => new ChartPoint(x.X, x.Y)).ToList()
            });
            MemoryWriter.Write<Chart>(shift2_chartCorrected, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);
            Thread.Sleep(SLEEP);

            Chart shift3_chartCorrected = new Chart() { SeriesCollection = new List<ChartSeries>() };
            shift3_chartCorrected.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Corrected 3",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(0, 0, 255),
                Points = shift3_resCorrectedPoints.Select(x => new ChartPoint(x.X, x.Y)).ToList()
            });
            MemoryWriter.Write<Chart>(shift3_chartCorrected, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);
            Thread.Sleep(SLEEP);

            Chart shift4_chartCorrected = new Chart() { SeriesCollection = new List<ChartSeries>() };
            shift4_chartCorrected.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Corrected 4",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(0, 0, 255),
                Points = shift4_resCorrectedPoints.Select(x => new ChartPoint(x.X, x.Y)).ToList()
            });
            MemoryWriter.Write<Chart>(shift4_chartCorrected, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);
            Thread.Sleep(SLEEP);
            
                                 
            double[] shifts = new double[]
            {
                Math.PI * 45.0 / 180.0,
                Math.PI * 90.0 / 180.0,
                Math.PI * 135.0 / 180.0,
                Math.PI
            };

            GenericInterferogramDecoder decoder = new GenericInterferogramDecoder();

            double[] phaseArray = new double[shift1_resCorrectedPoints.Count];

            for (int k = 0; k < shift1_resCorrectedPoints.Count; k++)
            {
                double intensity1 = shift1_resCorrectedPoints[k].Y;
                double intensity2 = shift2_resCorrectedPoints[k].Y;
                double intensity3 = shift3_resCorrectedPoints[k].Y;
                double intensity4 = shift4_resCorrectedPoints[k].Y;

                double[] intensities = new double[]
                {
                    intensity1,
                    intensity2,
                    intensity3,
                    intensity4
                };

                double phase = decoder.Decode(intensities, shifts);
                phaseArray[k] = phase;
            }

            Chart phaseChart = new Chart() { SeriesCollection = new List<ChartSeries>() };
            phaseChart.SeriesCollection.Add(new ChartSeries()
            {
                Name = "Phase",
                Type = HoloCommon.Enumeration.Charting.ChartSeriesType.Linear,
                ColorDescriptor = new ColorDescriptor(255, 0, 0),
                Points = phaseArray.Select((x, j) => new ChartPoint(j, x)).ToList()
            });
            MemoryWriter.Write<Chart>(phaseChart, new ChartSerialization());
            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\ChartApp\ChartApp\bin\Release\ChartApp.exe", null, false, false);
            Thread.Sleep(SLEEP);
            */
        }

        private void btnSinusOriginal_Click(object sender, EventArgs e)
        {
            int maxRange = MAX_RANGE_VALUE;

            double phaseShift = int.Parse(txtPhaseShift.Text);
            string arguments =
                string.Format(
                    "{0} {1} {2} {3}",
                    phaseShift.ToString(CultureInfo.InvariantCulture),
                    maxRange.ToString(CultureInfo.InvariantCulture),
                    M1.ToString(CultureInfo.InvariantCulture),
                    0.ToString()
                );

            ProcessManager.RunProcess(@"D:\Projects\HoloApplication\Modules\InterferogramCreatorConsoleApp\InterferogramCreatorConsoleApp\bin\Debug\InterferogramCreatorConsoleApp.exe", arguments, false);
        }
    }
}