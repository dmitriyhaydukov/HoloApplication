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
    public class TriangleInterferogramCreator : InterferogramCreator {
        //----------------------------------------------------------------------------------------------
        int period;    //Период в пикселях
        //----------------------------------------------------------------------------------------------
        private int previousValueY;
        private int previousValueX;

        Interval<double> startInterval;
        Interval<double> finishInterval;
        RealIntervalTransform transform = null;

        Interval<double> finalStartInterval;
        Interval<double> finalifnishInterval;
        RealIntervalTransform finalTransform = null;

        int halfPeriod;
        double coefficient;
        //----------------------------------------------------------------------------------------------
        //Конструктор
        public TriangleInterferogramCreator(
            InterferogramInfo InterferogramInfo,
            int period
        ) :
            base( InterferogramInfo ) {
            this.period = period;
            this.halfPeriod = this.period / 2;

            this.coefficient = this.interferogramInfo.ModuleValue.Value / halfPeriod;

            if (interferogramInfo.MaxRange.HasValue && interferogramInfo.ModuleValue.HasValue)
            {
                this.startInterval = new Interval<double>(0, interferogramInfo.ModuleValue.Value);
                this.finishInterval = new Interval<double>(interferogramInfo.FinalMinIntensity.Value, 200);

                this.transform = new RealIntervalTransform(startInterval, finishInterval);
            }
        }
        //----------------------------------------------------------------------------------------------
        //Сформировать картину
        public override RealMatrix CreateInterferogram( double shift ) {
            RealMatrix interferogram = new RealMatrix
                ( this.interferogramInfo.Height, this.interferogramInfo.Width );
            for ( int x = 0; x < interferogram.ColumnCount; x++ ) {
                for ( int y = 0; y < interferogram.RowCount; y++ ) {
                    double intensity = this.CalculateIntensity( x, y, this.period, shift );
                    interferogram[ y, x] = intensity;
                }
            }
            return interferogram;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //Вычисление интенсивности в точке
        private double CalculateIntensity(
            double x,
            double y,
            double period,
            double shift   //сдвиг
        ) {
            double noise =
                ( this.randomNumberGenerator.GetNextDouble() - 0.5 ) * 2 *
                this.interferogramInfo.MaxNoise;

            double remainder = x % this.period;

            double intensity =
                remainder < this.halfPeriod ?
                this.coefficient * x + noise :
                this.interferogramInfo.ModuleValue.Value - this.coefficient * x + noise;

            intensity = intensity % interferogramInfo.ModuleValue.Value;
            Console.WriteLine(intensity);

            if (this.transform != null)
            {
                intensity = transform.TransformToFinishIntervalValue(intensity);
                
                if (this.finalTransform != null)
                {
                    intensity = this.finalTransform.TransformToFinishIntervalValue(intensity);
                }
            }

            return intensity;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
    }
}
