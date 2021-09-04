using System;
using System.Linq;

namespace Common
{
    public static class ArrayExtensions
    {
        public static int Width<T>(this T[,] self)
        {
            return self.GetLength(1);
        }

        public static int Height<T>(this T[,] self)
        {
            return self.GetLength(0);
        }

        // really?
        public static float[] OrtogonalVector(this float[] vector)
        {
            var n = vector.Length;
            var result = new float[vector.Length];

            result[0] = vector[1] - vector[n - 1];
            result[n - 1] = vector[0] - vector[n - 2];

            for (int i = 1; i < n - 1; i++)
            {
                result[i] = vector[i + 1] - vector[i - 1];
            }

            return result;
        }

        // TODO: найти как это нормально называется
        /// <summary>
        /// Сумма поэлементных произведений
        /// </summary>
        /// <param name="vactor"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static float VectorMul(this float[] vector, float[] vector2)
        {
            if (vector.Length != vector2.Length)
                throw new InvalidOperationException("Длины векторов должны быть одинаковыми.");

            var result = 0f;

            for (var i = 0; i < vector.Length; i++)
            {
                result += vector[i] * vector2[i];
            }

            return result;
        }

        /// <summary>
        /// Возвращает одномерную копию двумерного массива
        /// a2d[i, j] = a1d[j * width + i]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T[] Flatten<T>(this T[,] self)
        {
            var size = self.GetLength(0) * self.GetLength(1);
            var flatten = new T[size];
            System.Buffer.BlockCopy(self, 0, flatten, 0, size);
            return flatten;
        }

        public static void GetMinMax(this float[] self, out float min, out float max)
        {
            min = self[0];
            max = self[0];
            var cur = 0f;

            for (var i = 0; i < self.Length; i++)
            {
                cur = self[i];

                if (min > cur)
                    min = cur;
                if (max < cur)
                    max = cur;
            }
        }
    }
}
