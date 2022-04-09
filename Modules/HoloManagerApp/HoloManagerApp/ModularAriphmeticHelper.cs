using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using HoloCommon.Models.Charting;

using ExtraLibrary.Geometry2D;

namespace HoloManagerApp
{
    public class ModularArithmeticHelper
    {
        private int m1;
        private int m2;

        private int M1;
        private int M2;

        private int N1;
        private int N2;

        private int[] array;

        public ModularArithmeticHelper(int m1, int m2)
        {
            this.m1 = m1;
            this.m2 = m2;

            this.M1 = m2;
            this.M2 = m1;

            this.N1 = CalculateN(M1, m1);
            this.N2 = CalculateN(M2, m2);

            this.array = new int[m2];

            for (int j = 0; j <= m2 - 1; j++)
            {
                this.array[j] = ((N2 * j) % m2) * m1;
            }
        }

        public int CalculateValue(int b1, int b2)
        {
            int j = b2 - b1;
            if (j < 0)
            {
                j = j + this.m2;
            }

            int res = this.array[j] + b1;

            return res;
        }

        public static List<Point2D> BuildTable
        (
            int m1, 
            int m2, 
            int? range,
            bool readDiagonalsFromFile,
            List<ChartPoint> points,
            out Dictionary<int, List<Point2D>> notDiagonalPointsDictionary,
            out List<Point2D> unwrappedPoints
        )
        {
            int M1 = m2;
            int M2 = m1;

            int N1 = CalculateN(M1, m1);
            int N2 = CalculateN(M2, m2);

            Dictionary<int, Point2D> pointsDictionary = new Dictionary<int, Point2D>();
            notDiagonalPointsDictionary = new Dictionary<int, List<Point2D>>();

            unwrappedPoints = new List<Point2D>();

            int diagonalRange = 2;

            int[] diagonalNumbersByb2 = new int[m2];
            for (int i = 0; i < m2; i++) // i = b2
            {
                int diagonalNumber = ((M2 * N2 * i) % (m1 * m2)) / m1;
                diagonalNumbersByb2[i] = diagonalNumber;
            }

            int[] diagonalNumbersByb1 = new int[m1];
            for (int i = 0; i < m1; i++) // i = b1
            {
                int diagonalNumber = ((M1 * N1 * i) % (m1 * m2)) / m1;
                diagonalNumbersByb1[i] = diagonalNumber;
            }

            int[] diagonalNumbersAugmented = new int[m1 + m2 - 1];
                        
            for (int k = diagonalNumbersByb1.Length - 1, j = 0; k >= 0; k--, j++)
            {
                diagonalNumbersAugmented[j] = diagonalNumbersByb1[k];
            }

            for (int k = 0, j = m1 - 1; k < diagonalNumbersByb2.Length; k++, j++)
            {
                diagonalNumbersAugmented[j] = diagonalNumbersByb2[k];
            }

            for (int i = 0; i < diagonalNumbersAugmented.Length; i++)
            {
                if (diagonalNumbersAugmented[i] > diagonalRange)
                {
                    diagonalNumbersAugmented[i] = int.MaxValue;
                }
            }

            int[] resDiagonalNumbersAugmented = new int[diagonalNumbersAugmented.Length];
            for (int k = 0; k < resDiagonalNumbersAugmented.Length; k++)    //copy array
            {
                resDiagonalNumbersAugmented[k] = diagonalNumbersAugmented[k];
            }
            
            for (int i = 0; i < diagonalNumbersAugmented.Length; i++)
            {
                int value = diagonalNumbersAugmented[i];
                if (value != int.MaxValue)
                {
                    int leftIndex = i > 0 ? i - 1 : 0;
                    while(
                        (leftIndex > 0) && 
                        (diagonalNumbersAugmented[leftIndex] == int.MaxValue)
                    )
                    {
                        leftIndex -= 1;
                    }
                    //int resLeftIndex = leftIndex != 0 ? (i + leftIndex) / 2 : leftIndex;
                    int resLeftIndex = (i + leftIndex) / 2;

                    int rightIndex = 
                        i < diagonalNumbersAugmented.Length - 1 ?
                        i + 1 : 
                        diagonalNumbersAugmented.Length - 1;
                    
                    while (
                        (rightIndex < diagonalNumbersAugmented.Length - 1) &&
                        (diagonalNumbersAugmented[rightIndex] == int.MaxValue)
                    )
                    {
                        rightIndex += 1;
                    }

                    //int resRightIndex = rightIndex != (diagonalNumbersAugmented.Length - 1) ? (i + rightIndex) / 2 : rightIndex;
                    int resRightIndex = (i + rightIndex) / 2;

                    for (int j = resLeftIndex; j <= resRightIndex; j++)
                    {
                        resDiagonalNumbersAugmented[j] = value;
                    }
                }
            }

            string fileContent =
                //string.Join(" ", diagonalNumbersAugmented) + '\n' +
                string.Join(" ", resDiagonalNumbersAugmented);

            File.WriteAllText(@"D:\Images\!!\diagonals.txt", fileContent);

            if (readDiagonalsFromFile)
            {
                string filePath = @"D:\Images\!!\diagonalsManual1.txt";
                string diagonalsString = File.ReadAllText(filePath);
                string[] parts = diagonalsString.Split(' ', '\n');

                resDiagonalNumbersAugmented = new int[parts.Length];
                for (int j = 0; j < parts.Length; j++)
                {
                    resDiagonalNumbersAugmented[j] = int.Parse(parts[j]);
                }
            }

            for (int b1 = 0; b1 < m1; b1++)
            {
                for (int b2 = 0; b2 < m2; b2++)
                {        
                    int value = (M1 * N1 * b1 + M2 * N2 * b2) % (m1 * m2);
                    if (range != null)
                    {
                        if (value < range)
                        {
                            Point2D point = new Point2D(b1, b2);
                            pointsDictionary.Add(value, point);
                        }
                        else
                        {
                            int index = b2 + m1 - 1 - b1;
                            int diagonalNum = resDiagonalNumbersAugmented[index];

                            if (diagonalNum != int.MaxValue)
                            {
                                Point2D point = new Point2D(b1, b2);

                                List<Point2D> dicPoints = null;
                                if (notDiagonalPointsDictionary.TryGetValue(diagonalNum, out dicPoints))
                                {
                                    dicPoints.Add(point);
                                }
                                else
                                {
                                    notDiagonalPointsDictionary.Add(diagonalNum, new List<Point2D>() { point });
                                }
                            }
                        }
                    }
                }
            }

            pointsDictionary = pointsDictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            List<Point2D> pointsList = pointsDictionary.Select(x => x.Value).ToList();
                        
            //Coefficients
            Dictionary<int, int> coefficientsDictionary = new Dictionary<int, int>();
            int[] coefficientsArray = new int[resDiagonalNumbersAugmented.Length];
            for (int j = 0; j < coefficientsArray.Length; j++)
            {
                coefficientsArray[j] = 0;
            }

            for (int k = 1; k < resDiagonalNumbersAugmented.Length; k++)
            {
                int prevDiagNum = resDiagonalNumbersAugmented[k - 1];
                int diagNum = resDiagonalNumbersAugmented[k];
                if (diagNum != int.MaxValue && diagNum != prevDiagNum)
                {
                    if (coefficientsDictionary.ContainsKey(diagNum))
                    {
                        coefficientsDictionary[diagNum] = coefficientsDictionary[diagNum] + 1;
                    }
                    else
                    {
                        coefficientsDictionary.Add(diagNum, 0);
                    }
                }
                else
                {
                    coefficientsArray[k] = coefficientsDictionary.ContainsKey(diagNum) ? coefficientsDictionary[diagNum] : 0;
                }
            }

            string coefContent = string.Join(" ", coefficientsArray);
            File.WriteAllText(@"D:\Images\!!\coefficients.txt", coefContent);
            
            for (int j = 0; j < points.Count; j++)
            {
                ChartPoint point = points[j];

                int b1 = Convert.ToInt32(Math.Floor(point.X));
                int b2 = Convert.ToInt32(Math.Floor(point.Y));

                int index = b2 + m1 - 1 - b1;
                int diagonalNum = resDiagonalNumbersAugmented[index];
                int coef = coefficientsArray[index];

                if (diagonalNum != int.MaxValue)
                {
                    int x = b1 + diagonalNum * m1;
                    int y = b2 + (diagonalNum + coef) * m2;
                    Point2D p = new Point2D(x, y);
                    unwrappedPoints.Add(p);
                }
            }

            return pointsList;
        }

        private static int CalculateN(int M, int m)
        {
            int n = 1;
            int value = (M * n) % m;
            while (value != 1)
            {
                n++;
                value = (M * n) % m;
            }
            return n;
        }
    }
}

