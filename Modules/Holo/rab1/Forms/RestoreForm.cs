using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
//using HayLibrary.Mathematics.Matrices;
using System.IO;

public delegate void ImageRestored(Bitmap newBitmap, double ratio);
public delegate void PheseMapBuilded(Bitmap newBitmap, double ratio);


namespace rab1.Forms
{
    public partial class RestoreForm : Form
    {
       // public event ImageRestored imageRestored;
        //public event PheseMapBuilded phaseMapBuilded;
        public Bitmap imageToEdit;

        private Color colorToFill = Color.Black;
        private Dictionary<int, List<Point>> numberAndPointCorrespondence = new Dictionary<int, List<Point>>();
        private Dictionary<int, Color> numberAndColorCorrespondence = new Dictionary<int, Color>();
        private List<Point> newList;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public RestoreForm()
        {
            InitializeComponent();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void RestoreForm_Shown(object sender, EventArgs e)
        {
            if (imageToEdit != null)
            {
                phaseMapImage.Image = imageToEdit;
            }

            Bitmap newBitmap = new Bitmap(phaseMapImage.Image.Width, phaseMapImage.Image.Height);
            BitmapData imageData = ImageProcessor.getBitmapData(newBitmap);
            BitmapData imageData2 = ImageProcessor.getBitmapData((Bitmap)phaseMapImage.Image);

            for (int x = 0; x < phaseMapImage.Image.Width; x++)
            {
                for (int y = 0; y < phaseMapImage.Image.Height; y++)
                {
                    Color currentColor = ImageProcessor.getPixel(x, y, imageData2);

                    if (currentColor.ToArgb() == Color.Red.ToArgb())
                    {
                        ImageProcessor.setPixel(imageData, x, y, Color.Red);
                    }
                }
            }

            ((Bitmap)phaseMapImage.Image).UnlockBits(imageData2);
            (newBitmap).UnlockBits(imageData);

            rightImage.Image = newBitmap;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void phaseMapImage_Click(object sender, EventArgs e)
        {
            if (phaseMapImage.Image == null)
            {
                MessageBox.Show("Сначала загрузите изображение");
                return;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void rightImage_MouseDown(object sender, MouseEventArgs e)
        {
            Color pointColor = ((Bitmap)rightImage.Image).GetPixel(e.X, e.Y);
            if (pointColor.ToArgb() == Color.Red.ToArgb())
            {
                return;
            }

            colorToFill = generateNewColor();

            int currentLineNumber = 0;
            bool found = false;
            foreach (int key in numberAndPointCorrespondence.Keys)
            {
                List<Point> currentList = numberAndPointCorrespondence[key];

                foreach (Point currentPoint in currentList)
                {
                    if ((e.X == currentPoint.X) && (e.Y == currentPoint.Y))
                    {
                        currentLineNumber = key;
                        found = true;
                        break;
                    }
                }

                if (found == true)
                {
                    break;
                }
            }

            
            






            ImageProcessor.floodImage(e.X, e.Y, colorToFill, rightImage.Image, true, Color.Red);
            rightImage.Invalidate();
            rightImage.Update();


            newList = new List<Point>();

            BitmapData imageData = ImageProcessor.getBitmapData((Bitmap)rightImage.Image);

            for (int x = 0; x < phaseMapImage.Image.Width; x++)
            {
                for (int y = 0; y < phaseMapImage.Image.Height; y++)
                {
                    Color currentColor = ImageProcessor.getPixel(x, y, imageData);

                    if (currentColor.ToArgb() == colorToFill.ToArgb())
                    {
                        newList.Add(new Point(x, y));
                    }
                }
            }

            ((Bitmap)rightImage.Image).UnlockBits(imageData);


            if (found == true)
            {
                List<Point> result = numberAndPointCorrespondence[currentLineNumber].Except(newList).ToList();
                numberAndPointCorrespondence[currentLineNumber] = result;
            }

            ChooseForm chooseForm = new ChooseForm();
            chooseForm.oldValue = currentLineNumber;
            chooseForm.userChoosedNumber += newNumber;
            chooseForm.ShowDialog();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void newNumber(int newNumber)
        {
            if(numberAndPointCorrespondence.ContainsKey(newNumber))
            {
                List<Point> existedList = numberAndPointCorrespondence[newNumber];
                existedList.AddRange(newList);

                BitmapData imageData = ImageProcessor.getBitmapData((Bitmap)rightImage.Image);

                foreach(Point currentPoint in existedList)
                {
                    ImageProcessor.setPixel(imageData, currentPoint.X, currentPoint.Y, colorToFill);
                }

                ((Bitmap)rightImage.Image).UnlockBits(imageData);

                rightImage.Invalidate();
                rightImage.Update();
            }
            else
            {
                numberAndPointCorrespondence.Add(newNumber, newList);
            }

            if (numberAndColorCorrespondence.ContainsKey(newNumber))
            {
                numberAndColorCorrespondence[newNumber] = colorToFill;
            }
            else
            {
                numberAndColorCorrespondence.Add(newNumber, colorToFill);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private Color generateNewColor()
        {
            Random random = new Random();
            bool win = true;

            for (; ; )
            {
                Color newColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

                if (newColor.ToArgb() == Color.Red.ToArgb())
                {
                    continue;
                }

                BitmapData imageData = ImageProcessor.getBitmapData((Bitmap)rightImage.Image);

                for (int x = 0; x < rightImage.Image.Width; x++)
                {
                    for (int y = 0; y < rightImage.Image.Height; y++)
                    {
                        Color currentColor = ImageProcessor.getPixel(x, y, imageData);

                        if (currentColor.ToArgb() == newColor.ToArgb())
                        {
                            win = false;
                            break;
                        }
                    }
                }

                ((Bitmap)rightImage.Image).UnlockBits(imageData);

                if (win == true)
                {
                    return newColor;
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void buildButton_Click(object sender, EventArgs e)
        {
/*
            Bitmap areasImage = (Bitmap)rightImage.Image;
            Bitmap intensityImage = (Bitmap)phaseMapImage.Image;

            BitmapData areasImageData = ImageProcessor.getBitmapData(areasImage);
            BitmapData intensityImageData = ImageProcessor.getBitmapData(intensityImage);

            List<Point3D> pointsList = new List<Point3D>();
            int maxColorIntensity = 0;
            int minColorIntensity = 100000;

            //var somePlane = getPlaneParams(pointsList);
            var somePlane = new Pi_Class1.Plane();
            somePlane.a = 0.000023888348386256014;
            somePlane.b = -0.00000030605791592613019;
            somePlane.c = -0.0000047370353095202745;
            somePlane.d = 0.00018888266329119749;

            int[][] pointsForFile = new int[areasImage.Width][];

            for (int i = 0; i < areasImage.Width; i++)
            {
                pointsForFile[i] = new int[areasImage.Height];
            }

            for (int x = 0; x < areasImage.Width; x++)
            {
                for (int y = 0; y < areasImage.Height; y++)
                {
                    Color currentColor = ImageProcessor.getPixel(x, y, areasImageData);

                    int currentLineNumber = 0;
                    foreach (int key in numberAndColorCorrespondence.Keys)
                    {
                        if (numberAndColorCorrespondence[key].ToArgb() == currentColor.ToArgb())
                        {
                            currentLineNumber = key;
                            break;
                        }
                    }

                    Color currentIntensity = ImageProcessor.getPixel(x, y, intensityImageData);
                    double additionalZcomponent = 255 * currentLineNumber;

                    double currentColorIntencity = currentIntensity.R;

                    currentColorIntencity = 255 - currentColorIntencity + additionalZcomponent;

                    if (maxColorIntensity < currentColorIntencity)
                    {
                        maxColorIntensity = (int)currentColorIntencity;
                    }

                    if ((minColorIntensity > currentColorIntencity) && (currentColorIntencity != 0))
                    {
                        minColorIntensity = (int)currentColorIntencity;
                    }

                    if ((int)(currentColorIntencity) != 0)
                    {
                        Point3D newPoint = new Point3D(y, x, (int)(currentColorIntencity));
                        pointsList.Add(newPoint);

                        double planeZ = ((somePlane.a * newPoint.x) + (somePlane.b * newPoint.y) + somePlane.d) / (somePlane.c);

                        pointsForFile[x][y] = (int)Math.Abs(Math.Abs(newPoint.z) - Math.Abs(planeZ));
                    }
                    else
                    {
                        pointsForFile[x][y] = 500000;
                    }
                }
            }

            writeToFile(pointsForFile, areasImage.Width, areasImage.Height);


            //OpenGLForm newForm = new OpenGLForm();
            List<Point3D> result = new List<Point3D>();

            foreach (Point3D currentPoint in pointsList)
            {
                double planeZ = ((somePlane.a * currentPoint.x) + (somePlane.b * currentPoint.y) + somePlane.d) / (somePlane.c);
               
                //newForm.addPoint(new Point3D(currentPoint.x, currentPoint.y, -(int)planeZ));

                //newForm.addPoint(currentPoint);

                newForm.addPoint(new Point3D(currentPoint.x, currentPoint.y, (int)Math.Abs(Math.Abs(currentPoint.z) - Math.Abs(planeZ)), Color.RoyalBlue));

                //result.Add(new Point3D(currentPoint.x, currentPoint.y, (int)Math.Abs(Math.Abs(currentPoint.z) - Math.Abs(planeZ))));
            }

            newForm.Show();

            ((Bitmap)rightImage.Image).UnlockBits(areasImageData);
            ((Bitmap)phaseMapImage.Image).UnlockBits(intensityImageData);


            /*Bitmap resultBitmap = new Bitmap(phaseMapImage.Image.Width, phaseMapImage.Image.Height);

            BitmapData imageData = ImageProcessor.getBitmapData(resultBitmap);

            foreach (Point3D currentPoint in pointsList)
            {
                if (maxColorIntensity < currentPoint.z)
                {
                    maxColorIntensity = currentPoint.z;
                }

                if ((minColorIntensity > currentPoint.z) && (currentPoint.z != 0))
                {
                    minColorIntensity = currentPoint.z;
                }
            }

            int abs = maxColorIntensity - minColorIntensity;
            double ratio = abs / 255.0;

            foreach (Point3D currentPoint in pointsList)
            {
                var scaledValue = (int)(Math.Abs(currentPoint.z) / ratio) - 1;
                ImageProcessor.setPixel(imageData, currentPoint.y, currentPoint.x, Color.FromArgb(scaledValue, scaledValue, scaledValue));
            }

            resultBitmap.UnlockBits(imageData);
            phaseMapBuilded(resultBitmap, ratio);




            resultBitmap = new Bitmap(phaseMapImage.Image.Width, phaseMapImage.Image.Height);
            imageData = ImageProcessor.getBitmapData(resultBitmap);

            foreach (Point3D currentPoint in result)
            {
                if (maxColorIntensity < currentPoint.z)
                {
                    maxColorIntensity = currentPoint.z;
                }

                if ((minColorIntensity > currentPoint.z) && (currentPoint.z != 0))
                {
                    minColorIntensity = currentPoint.z;
                }
            }

            abs = maxColorIntensity - minColorIntensity;
            ratio = abs / 255.0;
            foreach (Point3D currentPoint in result)
            {
                int scaledValue = (int) (Math.Abs(currentPoint.z) / ratio);
                ImageProcessor.setPixel(imageData, currentPoint.y, currentPoint.x, Color.FromArgb(scaledValue, scaledValue, scaledValue));
            }

            resultBitmap.UnlockBits(imageData);
            imageRestored(resultBitmap, ratio);
         */
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void RestoreForm_SizeChanged(object sender, EventArgs e)
        {
            relayout();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void relayout()
        {
            panel1.Size = new Size(this.Size.Width/2, this.Size.Height - 100);

            this.panel2.Location = new Point(this.panel1.Location.X + this.panel1.Size.Width + 10, this.panel2.Location.Y);
            this.panel2.Size = new Size(this.Size.Width / 2 - 50, this.Size.Height - 100);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*
        public Pi_Class1.Plane getPlaneParams(List<Point3D> pointsOfPlane)
        {
            /*int a = 1;
            int b = 2;
            int c = -3;
            int d = 1;
            pointsOfPlane = new List<Point3D>();

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    var newPoint = new Point3D(x, y, -(x*a + y*b + d)/c);
                    pointsOfPlane.Add(newPoint);
                }
            }
           
            var result = new Pi_Class1.Plane();

            var initMatrix = new double[pointsOfPlane.Count(), 4];

            for (int j = 0; j < pointsOfPlane.Count(); j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        initMatrix[j, i] = pointsOfPlane[j].x;            
                    }
                    else if (i == 1)
                    {
                        initMatrix[j, i] = pointsOfPlane[j].y;  
                    }
                    else if (i == 2)
                    {
                        initMatrix[j, i] = pointsOfPlane[j].z;
                    }
                    else if (i == 3)
                    {
                        initMatrix[j, i] = 1;
                    }
                }
            }

            var initialMatrix = new RealMatrix(initMatrix);
            var transposedMatrix = initialMatrix.GetTransposedMatrix();
            var multMatrix = transposedMatrix * initialMatrix;

            double deter = multMatrix.GetDeterminant();
            //RealMatrix conjMatrix = multMatrix.GetAlgebraicalComplement()

            var complementData = new double[4, 4];
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    complementData[j, i] = multMatrix.GetAlgebraicalComplement(j, i);
                }
            }

            var complementMatrix = new RealMatrix(complementData);

            RealMatrix transposedMatrix2 = complementMatrix.GetTransposedMatrix();
            RealMatrix obrMatrix = transposedMatrix2 * (1.0 / deter);

            var onesVector = new double[4, 1];
            onesVector[0, 0] = 1;
            onesVector[1, 0] = 1;
            onesVector[2, 0] = 1;
            onesVector[3, 0] = 1;

            var onesMatrix = new RealMatrix(onesVector);
            RealMatrix resultMatrix = obrMatrix * onesMatrix;

            double[,] resultData = resultMatrix.GetDataArray();
            result.a = resultData[0, 0];
            result.b = resultData[1, 0];
            result.c = resultData[2, 0];
            result.d = resultData[3, 0];

            /*result.a = result.a/result.d;
            result.b = result.b / result.d;
            result.c = result.c / result.d;
            result.d = result.d / result.d;*/



            /*List<Point3D> pointsOfPlane2 = new List<Point3D>();

            for (double x = 0; x < 4; x++)
            {
                for (double y = 0; y < 4; y++)
                {
                    var newPoint = new Point3D((int) x, (int) y, (int) ((x * result.a + y * result.b + result.d) / result.c));
                    pointsOfPlane2.Add(newPoint);
                }
            }
           

            return result;
        }
         */
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void writeToFile(int[][] pointsForWriting, int width, int height) //выгрузка
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog(); //создали диалог
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) //если нажата ОК
            {
                String fileName = saveFileDialog1.FileName; //взяли имя из диалога

                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate); //создаём поток
                System.IO.BinaryWriter w = new System.IO.BinaryWriter(fs); //создаём записывальщик

                /////////////// От чего до чего Шаг /////////////////////
                int StartX = 0;    // Начальный X
                int FinishX = width; // Конечный X
                int StartY = 0;    // Начальный Y
                int FinishY = height; // Конечный Y
                int Step = 1;    // Шаг
                /////////////////////////////////////////////////////////

                w.Write((int)(FinishX - StartX) / Step);  //запись количества значений по X
                w.Write((int)(FinishY - StartY) / Step);  //запись количества значений по Y

                
                for (int y = StartY; y < FinishY; y += Step) //внешний цикл по Y
                {
                    for (int x = StartX; x < FinishX; x += Step) //внутренний цикл по X //получается построчная запись
                    {
                        w.Write(pointsForWriting[x][y]); //запиcываем посчитанное значение
                    }
                }
                w.Close(); //закрываем записывальщик
                fs.Close(); //закрываем поток
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}