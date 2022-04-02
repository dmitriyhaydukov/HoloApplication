﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

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

        public static List<Point2D> BuildTable(int m1, int m2, int? range, out List<Point2D> notDiagonalPoints)
        {
            int M1 = m2;
            int M2 = m1;

            int N1 = CalculateN(M1, m1);
            int N2 = CalculateN(M2, m2);

            Dictionary<int, Point2D> pointsDictionary = new Dictionary<int, Point2D>();
            notDiagonalPoints = new List<Point2D>();

            int diagonalRange = 10;

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
                    diagonalNumbersAugmented[i] = 0;
                }
            }

            string fileContent =
                string.Join(" ", diagonalNumbersByb2) + '\n' +
                string.Join(" ", diagonalNumbersByb1) + '\n' +
                string.Join(" ", diagonalNumbersAugmented);

            File.WriteAllText(@"D:\Images\!!\diagonals.txt", fileContent);

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
                            //int index = b2 - b1 >= 0 ? b2 - b1 : b2 - b1 + m2;
                            if (diagonalNumbersAugmented[b2 + m1 - 1 - b1] == 0)
                            {
                                notDiagonalPoints.Add(new Point2D(b1, b2));
                            }
                            
                        }
                    }
                }
            }

            pointsDictionary = pointsDictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            List<Point2D> pointsList = pointsDictionary.Select(x => x.Value).ToList();

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

