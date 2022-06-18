using System;
using System.Linq;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Arraying.ArrayOperation;

namespace HoloManagerApp
{
    //Смарт фильтр
    public class SmartEdgeByRowsGrayScaleFilter
    {
        //------------------------------------------------------------------------------------------------
        //Функция фильтрации
        private double GetFilteredValue(
            double[] array,
            double threshold
        )
        {
            double filteredValue;
            int n = array.Length;

            double min = array.Min();
            double max = array.Max();
            
            bool isGap = (max - min) > threshold;
                        
            if (isGap)
            {
                filteredValue = max;
            }
            else
            {
                filteredValue = array[n / 2];
            }

            return filteredValue;
        }

        /*
        private double GetFilteredValue(
            double[] array,
            double threshold
        )
        {
            double filteredValue;
            int n = array.Length;
            int medianIndex = n / 2;
            int halfSize = n / 2;

            double[] array1 = new double[halfSize];
            double[] array2 = new double[halfSize];

            Array.Copy(array, 0, array1, 0, halfSize);
            Array.Copy(array, medianIndex, array2, 0, halfSize);

            bool isArray2Gap = true;

            double min1 = array1.Min();
            double max1 = array1.Max();
            double max2 = array2.Max();

            bool isGap = (max2 - min1) > threshold;

            for(int i = 0; i < array2.Length; i++)
            {
                if (array2[i] <= max1)
                {
                    isArray2Gap = false;
                }
            }  

            if (isGap && isArray2Gap)
            {
                filteredValue = max2;
            }
            else
            {
                filteredValue = array[halfSize];
            }

            return filteredValue;
        }
        */
        //------------------------------------------------------------------------------------------------
        //Фильтрация строк изображения
        private RealMatrix FiltrateGrayScaleImageRows(
            RealMatrix grayScaleIntensityMatrix,
            int windowSize,
            double threshold
        )
        {
            int width = grayScaleIntensityMatrix.ColumnCount;
            int height = grayScaleIntensityMatrix.RowCount;

            int step = windowSize / 2;

            RealMatrix newMatrix = new RealMatrix(height, width);

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {

                    int columnLeft = column - step;
                    if (columnLeft < 0)
                    {
                        columnLeft = column;
                    }
                    int columnRight = column + step;
                    if (width <= columnRight)
                    {
                        columnRight = column;
                    }

                    double[] originalValues = grayScaleIntensityMatrix.GetRowValues(row, columnLeft, columnRight);

                    double newValue = this.GetFilteredValue(originalValues, threshold);
                    newMatrix[row, column] = newValue;
                }
            }
            return newMatrix;
        }
        //------------------------------------------------------------------------------------------------
        //Фильтрация изображения
        public RealMatrix ExecuteFiltration(
            RealMatrix grayScaleIntensityMatrix, int windowSize, double threshold
        )
        {
            RealMatrix filteredRowsMatrix = this.FiltrateGrayScaleImageRows
                (grayScaleIntensityMatrix, windowSize, threshold);

            RealMatrix resultMatrix = filteredRowsMatrix;
            return resultMatrix;
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
    }
}
