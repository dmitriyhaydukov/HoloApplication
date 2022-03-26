using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                            int r = 20;

                            if (Math.Abs(b1 - b2) < r)
                            {
                                Point2D point = new Point2D(b1, b2);
                                notDiagonalPoints.Add(point);
                            }
                            else
                            {
                                
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

