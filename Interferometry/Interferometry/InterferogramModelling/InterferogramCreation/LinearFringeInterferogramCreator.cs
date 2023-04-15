using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Mathematics.Transformation;
using ExtraLibrary.Randomness;

namespace Interferometry.InterferogramCreation {
    //Формирователь картин линейных интерференционных полос
    public class LinearFringeInterferogramCreator : InterferogramCreator {

        //private static double[] clin = { 35, 50, 58, 65, 72, 78, 85, 94, 100, 108, 118, 132, 149, 168, 192, 255 };  // Клин для исправления нелинейности

        //private static double[] clin = { 70, 75, 80, 84, 88, 92, 96, 99, 104, 108, 118, 132, 149, 168, 192, 255 };  // Клин для исправления нелинейности
        //private static double[] clin = { 59, 65, 80, 88, 95, 100, 108, 112, 118, 125, 132, 149, 168, 185, 192, 255 };
        private static double[] clin = { 35, 60, 65, 72, 79, 84, 90, 94, 100, 108, 118, 132, 149, 168, 192, 255 };


        private static double[] interpolatedClin = null;

        //----------------------------------------------------------------------------------------------
        int fringeCount;    //Количество интерференционных полос
        //----------------------------------------------------------------------------------------------
        private int previousValueY;
        private int previousValueX;

        Interval<double> startInterval;
        Interval<double> finishInterval;
        RealIntervalTransform transform = null;

        Interval<double> finalStartInterval;
        Interval<double> finalifnishInterval;
        RealIntervalTransform finalTransform = null;

        //----------------------------------------------------------------------------------------------
        static LinearFringeInterferogramCreator()
        {
            interpolatedClin = InterpolateClin(clin);
        }
        //----------------------------------------------------------------------------------------------
        //Конструктор
        public LinearFringeInterferogramCreator(
            InterferogramInfo InterferogramInfo,
            int fringeCount
        ) :
            base( InterferogramInfo ) {
            this.fringeCount = fringeCount;

            if (interferogramInfo.MaxRange.HasValue && interferogramInfo.ModuleValue.HasValue && interferogramInfo.ByModule)
            {
                //this.startInterval = new Interval<double>(interferogramInfo.MinIntensity, interferogramInfo.MaxIntensity);
                //this.finishInterval = new Interval<double>(interferogramInfo.MinIntensity, interferogramInfo.MaxRange.Value);
                //this.finishInterval = new Interval<double>(0, interferogramInfo.MaxRange.Value);

                this.startInterval = new Interval<double>(0, interferogramInfo.ModuleValue.Value);
                this.finishInterval = new Interval<double>(interferogramInfo.FinalMinIntensity.Value, 255);

                this.transform = new RealIntervalTransform(startInterval, finishInterval);

                /*
                if (interferogramInfo.FinalMinIntensity.HasValue)
                {
                    this.finalStartInterval = new Interval<double>(0, interferogramInfo.ModuleValue.Value);
                    this.finalifnishInterval = new Interval<double>(interferogramInfo.FinalMinIntensity.Value, interferogramInfo.ModuleValue.Value);
                    this.finalifnishInterval = new Interval<double>(interferogramInfo.FinalMinIntensity.Value, interferogramInfo.MaxIntensity);
                    this.finalTransform = new RealIntervalTransform(finalStartInterval, finalifnishInterval);
                }
                */
            }
            else
            {
                this.startInterval = new Interval<double>(0, interferogramInfo.MaxIntensity);
                this.finishInterval = new Interval<double>(interferogramInfo.FinalMinIntensity.Value, 255);

                this.transform = new RealIntervalTransform(startInterval, finishInterval);
            }
        }
        //----------------------------------------------------------------------------------------------
        //Сформировать картину
        public override RealMatrix CreateInterferogram( double phaseShift ) {
            RealMatrix interferogram = new RealMatrix
                ( this.interferogramInfo.Height, this.interferogramInfo.Width );
            for ( int x = 0; x < interferogram.ColumnCount; x++ ) {
                for ( int y = 0; y < interferogram.RowCount; y++ ) {
                    double intensity = this.CalculateIntensity( x, y, phaseShift );
                    interferogram[ y, x] = intensity;
                }
            }
            return interferogram;
        }
        //----------------------------------------------------------------------------------------------
        public RealMatrix GetPhaseMatrix() {
            RealMatrix phaseMatrix = new RealMatrix( this.interferogramInfo.Height, this.interferogramInfo.Width );
            for ( int x = 0; x < phaseMatrix.ColumnCount; x++ ) {
                for ( int y = 0; y < phaseMatrix.RowCount; y++ ) {
                    double phase = this.CalculatePhase( x, y ); //% ( 2 * Math.PI );
                    phaseMatrix[ y, x ] = phase;
                }
            }
            return phaseMatrix;
        }
        //----------------------------------------------------------------------------------------------
        //Вычисление фазы в точке
        private double CalculatePhase( double x, double y ) {
            double phase;

            /*
            phase =
                x * 2 * Math.PI * ( ( double )this.fringeCount ) /
                ( ( double )this.interferogramInfo.Width ) % ( 2 * Math.PI );
            */

            double phase1 =
                ( x ) * 2 * Math.PI * ( ( double )this.fringeCount ) /
                ( ( double )this.interferogramInfo.Width ) % ( 2 * Math.PI );

            /*
            double phase2 =
                ( y ) * 2 * Math.PI * ( ( double )this.fringeCount ) /
                ( ( double )this.interferogramInfo.Width ) % ( 2 * Math.PI );
            */

            //double randomValue = Math.PI / 2 * ( this.randomNumberGenerator.GetNextDouble() - 0.5 );
            //double phaseOffset = this.randomNumberGenerator.GetNextInteger( 100 ) % ( 2 * Math.PI );
            
            //double phaseOffset = 0;
            //return ( phase1 + phase2 + phaseOffset ) % ( 2 * Math.PI );
            
            //return phase;

            return phase1;
        }
        //----------------------------------------------------------------------------------------------
        //Вычисление интенсивности в точке
        private double CalculateIntensity(
            double x,
            double y,
            double phaseShift   //Фазовый сдвиг
        ) {
            double phase = this.CalculatePhase( x, y ) + phaseShift;
            double noise =
                ( this.randomNumberGenerator.GetNextDouble() - 0.5 ) * 2 *
                this.interferogramInfo.MaxNoise;
            double intensity =
                this.interferogramInfo.MinIntensity +
                this.interferogramInfo.MeanIntensity +
                this.interferogramInfo.MeanIntensity *
                this.interferogramInfo.IntensityModulation *
                Math.Cos( phase ) +
                noise;

            if (this.interferogramInfo.ByModule)
            {
                intensity = intensity % interferogramInfo.ModuleValue.Value;
                if (this.transform != null)
                {
                    intensity = transform.TransformToFinishIntervalValue(intensity);
                    if (this.finalTransform != null)
                    {
                        intensity = this.finalTransform.TransformToFinishIntervalValue(intensity);
                    }
                }
            }
            else
            {
                intensity = transform.TransformToFinishIntervalValue(intensity);
            }

            intensity = CorrectValueByClin(intensity, interpolatedClin, 255);

            return intensity;
        }
        //----------------------------------------------------------------------------------------------
        public static double CorrectValueByClin(double idealValue, double[] clinArray, int idealCount)
        {
            if (clinArray == null) { return idealValue; }
            double clinArrayCount = 240;

            int value = Convert.ToInt32(idealValue * clinArrayCount / idealCount);  // от 0 до 240(255)
            double resValue = clinArray[value];
            return resValue;
        }
        //----------------------------------------------------------------------------------------------
        private static double[] InterpolateClin(double[] clin)
        {
            if (clin == null) { return null; }

            double[] resClin = clin;

            //2^4 = 16 (16 * 16 = 256)
            int iterationCount = 4;
            for (int i = 1; i <= iterationCount; i++)
            {
                resClin = InterpolateArrayByX2(resClin);
            }
            
            return resClin;
        }
        //----------------------------------------------------------------------------------------------
        private static double[] InterpolateArrayByX2(double[] originArray)
        {
            int resLength = originArray.Length * 2;
            double[] resArray = new double[resLength];

            for (int i = 0; i < resLength - 1; i++)
            {
                if (i % 2 == 0)
                {
                    resArray[i] = originArray[i / 2];
                }
                if (i % 2 != 0)
                {
                    double firstValue = originArray[i / 2];
                    double secondValue = originArray[i / 2 + 1];
                    resArray[i] = (firstValue + secondValue) / 2;
                }
            }

            return resArray;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
    }
}
