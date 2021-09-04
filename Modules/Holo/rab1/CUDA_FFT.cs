using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Diagnostics;
using System.Windows.Forms;

//using ExtraLibrary.Mathematics.Matrices;
//using ExtraLibrary.Arraying.ArrayCreation;

using System.Runtime.InteropServices;
using ClassLibrary;
//using rab1;

namespace rab1
{
    //[Serializable()]
    public class CUDA_FFT
    {

        [DllImport("CudaCalculationFFT.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public extern static System.Int32 FFT(ComplexNumber* inputMatrix, ComplexNumber* outputMatrix, int width, int height);

        //----------------------------------------------------------------------------------------------
        //           Cuda calculation FFT
        //----------------------------------------------------------------------------------------------
        public static unsafe ComplexNumber[] CudaFFT(ComplexNumber[] input, int width, int height)
        {
            ComplexNumber[] output = new ComplexNumber[input.Length];
            fixed (ComplexNumber* pi = input, po = output)
            {
            
                FFT(pi, po, height, width);
           
            }

            return output;
        }

        //----------------------------------------------------------------------------------------------
        //           Cuda  FFT из главного окна
        //----------------------------------------------------------------------------------------------
        public static void Fur_CUDA(ZArrayDescriptor zArrayPicture)                                            // Прямое преобразование Фурье (CUDA)
        {
            //MessageBox.Show("Прямое преобразование Фурье с произвольным числом точек");


            int nx = zArrayPicture.width;
            int ny = zArrayPicture.height;

            System.Diagnostics.Stopwatch sw = new Stopwatch();

            System.Diagnostics.Stopwatch sw1 = new Stopwatch();
            System.Diagnostics.Stopwatch sw3 = new Stopwatch();
            System.Diagnostics.Stopwatch sw4 = new Stopwatch();

            sw.Start();

            ComplexNumber[] Cud_array = ComplexMatrix.ToArrayByRows(zArrayPicture);            // Из двухмерного в одномерный ->  Массив комплексных (Struct)чисел [nx*ny]          


            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            MessageBox.Show("CUDA FFT Из двухмерного в одномерный  Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);

            sw1.Start();

            ComplexNumber[] Cud_array1 = CUDA_FFT.CudaFFT(Cud_array, nx, ny);

            sw1.Stop();
            ts = sw1.Elapsed;
            MessageBox.Show("CUDA FFT  Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);

            sw3.Start();
            int k=0;
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    zArrayPicture.array[i, j] = Complex.FromPolarCoordinates(Cud_array1[k].Real, Cud_array1[k].Imaginary).Magnitude;
                    k++;
                }
                    //zArrayPicture.array[i, j] = C_array1[i, j].Magnitude;
            //zArrayPicture = ComplexMatrix.ToArrayByRows_column(Cud_array, nx, ny);

            sw3.Stop();
            ts = sw3.Elapsed;
            MessageBox.Show("CUDA FFT  zArrayPicture.array  Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);
           // Cud_array1 = null;
        }
        //----------------------------------------------------------------------------------------------
        //           Cuda  FFT из главного окна из k1 в k2
        //----------------------------------------------------------------------------------------------
        public static ZComplexDescriptor Fur_CUDA1( ZComplexDescriptor zarray)                                            // Прямое преобразование Фурье (CUDA)
        {            
            int nx = zarray.width;
            int ny = zarray.height;
            ZComplexDescriptor resultArray = new ZComplexDescriptor(nx, ny);

            ComplexNumber[] Cud_array = ComplexMatrix.ToArrayByRows(zarray);            // Из двухмерного в одномерный ->  Массив комплексных (Struct)чисел [nx*ny]                  
            ComplexNumber[] Cud_array1 = CUDA_FFT.CudaFFT(Cud_array, nx, ny);
               
            int k = 0;
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {  
                    resultArray.array[i, j] = Complex.FromPolarCoordinates(Cud_array1[k].Real, Cud_array1[k].Imaginary);  // Преобразование из одномерного в [ , ]        
                    k++;
                }
            return resultArray;

        }

        //----------------------------------------------------------------------------------------------
        //           Cuda  Френель из k1 в k2
        //----------------------------------------------------------------------------------------------
        public static ZComplexDescriptor Fr_CUDA(ZComplexDescriptor zarray, double lambda, double d, double dx) 
        {
            int nx = zarray.width;
            int ny = zarray.height;
            ZComplexDescriptor resultArray = new ZComplexDescriptor(nx, ny);

            Complex[] Array_с2x = FurieN.fexp2(lambda, d, nx, dx);                       // Умножение на экспоненту
            Complex[] Array_с2y = FurieN.fexp2(lambda, d, ny, dx);  

            for (int j = 0; j < ny; j++)
                for (int i = 0; i < nx; i++) resultArray.array[i, j] = zarray.array[i, j] * Array_с2x[i] * Array_с2y[j];
            
            //Array_с2 = FurieN.fexp2(lambda, d, ny, dx); 
            //for (int i = 0; i < nx; i++)
            //    for (int j = 0; j < ny; j++) resultArray.array[i, j] = resultArray.array[i, j] * Array_с2[j];

            ComplexNumber[] Cud_array = ComplexMatrix.ToArrayByRows(resultArray);       // Из двухмерного в одномерный ->  Массив комплексных (Struct)чисел [nx*ny]                  
            ComplexNumber[] Cud_array1 = CUDA_FFT.CudaFFT(Cud_array, nx, ny);           // БПФ

            int k = 0;
            for (int i = 0; i < nx; i++)                                                // Из одномерного в двухмерный
                for (int j = 0; j < ny; j++)
                {
                    resultArray.array[i, j] = Complex.FromPolarCoordinates(Cud_array1[k].Real, Cud_array1[k].Imaginary);  // Преобразование из одномерного в [ , ]        
                    k++;
                }


            double[] phase_y = FurieN.fexp1(lambda, d, ny, dx);                           // Умножение на экспоненту
            double[] phase_x = FurieN.fexp1(lambda, d, nx, dx);  
            
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                { resultArray.array[i, j] = Complex.FromPolarCoordinates(resultArray.array[i, j].Magnitude, resultArray.array[i, j].Phase + phase_y[j] + phase_x[i]); }
            
          //  phase = FurieN.fexp1(lambda, d, nx, dx);       
          //  for (int j = 0; j < ny; j++)
          //      for (int i = 0; i < nx; i++)
          //      { resultArray.array[i, j] = Complex.FromPolarCoordinates(resultArray.array[i, j].Magnitude, resultArray.array[i, j].Phase + phase[i]); }

            return resultArray;

        }

        //----------------------------------------------------------------------------------------------
    }

    public class ComplexMatrix
    {

        private int rowCount;           //Количество строк
        private int columnCount;        //Количество столбцов

        private Complex[,] dataArray;  //Массив данных   

        //-----------------------------------------------------------------------------------------
        //Индексатор
        public Complex this[int row, int column]
        {
            get
            {
                return this.dataArray[row, column];
            }
            set
            {
                this.dataArray[row, column] = value;
            }
        }
        //-----------------------------------------------------------------------------------------
        // Конструктор
/*
        public ComplexMatrix(int row, int col)
        {
            this.rowCount = row;
            this.columnCount = col;

         
        }

*/
        public ComplexMatrix(Complex[,] array)
        {
            this.rowCount = array.GetLength(0);
            this.columnCount = array.GetLength(1);

            this.dataArray = new Complex[this.rowCount, this.columnCount];
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    this.dataArray[row, column] = array[row, column];
                }
            }
        }

        public ComplexMatrix(ZArrayDescriptor ampl)
        {
            this.rowCount = ampl.width;
            this.columnCount = ampl.height;

            this.dataArray = new Complex[this.rowCount, this.columnCount];
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    this.dataArray[row, column] = Complex.FromPolarCoordinates(ampl.array[row, column], 0); 
                }
            }
        }

        public ComplexMatrix(ZComplexDescriptor ampl)
        {
            this.rowCount = ampl.width;
            this.columnCount = ampl.height;

            this.dataArray = new Complex[this.rowCount, this.columnCount];
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    this.dataArray[row, column] = Complex.FromPolarCoordinates(ampl.array[row, column].Magnitude , ampl.array[row, column].Phase );
                }
            }
        }

        //------------------------------------------------------------------------------------------
       
        public ComplexNumber[] ToArrayByRows()                       // Прямое  преобразование массива в FFT
        {

            
            int count = this.rowCount * this.columnCount;
            ComplexNumber[] array = new ComplexNumber[count];
            int k = 0;
            for (int row = 0; row < this.rowCount; row++)
            {
                for (int col = 0; col < this.columnCount; col++)
                {
                    Complex c = this.dataArray[row, col];
                    array[k] = new ComplexNumber(c.Real, c.Imaginary);
                    k++;
                }
            }       

            return array;
        }


        public static ComplexNumber[] ToArrayByRows(ZArrayDescriptor ampl)   // Прямое преобразование массива в FFT из ZArrayDescriptor
        {
            //Stopwatch sw = new Stopwatch();
            // sw.Start();
            int row = ampl.width;
            int col = ampl.height;

            int count = row * col;
            ComplexNumber[] array = new ComplexNumber[count];
            int k = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {   
                    array[k] = new ComplexNumber(ampl.array[i, j], 0);
                    k++;
                }
            }

            // sw.Stop();
            // Console.WriteLine("Copy matrix to one dimaensional array: {0}", sw.Elapsed);

            return array;
        }


        public static ComplexNumber[] ToArrayByRows(ZComplexDescriptor ampl)   // Прямое преобразование массива в FFT из ZComplexDescriptor
        {
            
            int row = ampl.width;
            int col = ampl.height;
            //ComplexMatrix(row, col);

            int count = row * col;
            ComplexNumber[] array = new ComplexNumber[count];
            int k = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    array[k] = new ComplexNumber(ampl.array[i, j].Real  , ampl.array[i, j].Imaginary );
                    k++;
                }
            }


            return array;
        }

        //-----------------------------------------------------------------------------------------
       
        public static ZArrayDescriptor ToArrayByRows_column(ComplexNumber[] array, int nx, int ny)           // Обратное преобразование массива
        {
            //int row = ampl.width;
            //int col = ampl.height;
            int row = nx;
            int col = ny;
            ZArrayDescriptor ampl = new ZArrayDescriptor(nx, ny);

            // this.dataArray = new Complex[this.rowCount, this.columnCount];

            int k = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    ampl.array[i, j] = array[k].Real;
                    k++;
                }
            }
            return ampl;
        }

        public ComplexMatrix(ComplexNumber[] array, int width, int height)
        {
            this.rowCount = width;
            this.columnCount = height;

            this.dataArray = new Complex[this.rowCount, this.columnCount];

            int k = 0;
            for (int row = 0; row < this.rowCount; row++)
            {
                for (int col = 0; col < this.columnCount; col++)
                {
                    this.dataArray[row, col] = new Complex(array[k].Real, array[k].Imaginary);
                    k++;
                }
            }
        }

    }
    //----------------------------------------------------------------------------------------------
    public struct ComplexNumber
    {
        public double Real;
        public double Imaginary;

        public ComplexNumber(double r, double i)
        {
            this.Real = r;
            this.Imaginary = i;
        }
    }

}
