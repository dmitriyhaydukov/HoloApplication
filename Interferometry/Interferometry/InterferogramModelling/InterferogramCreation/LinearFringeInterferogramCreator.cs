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
        //Конструктор
        public LinearFringeInterferogramCreator(
            InterferogramInfo InterferogramInfo,
            int fringeCount
        ) :
            base( InterferogramInfo ) {
            this.fringeCount = fringeCount;

            if (interferogramInfo.MaxRange.HasValue && interferogramInfo.ModuleValue.HasValue)
            {
                this.startInterval = new Interval<double>(interferogramInfo.MinIntensity, interferogramInfo.MaxIntensity);
                this.finishInterval = new Interval<double>(interferogramInfo.MinIntensity, interferogramInfo.MaxRange.Value);
                //this.finishInterval = new Interval<double>(0, interferogramInfo.MaxRange.Value);
                this.transform = new RealIntervalTransform(startInterval, finishInterval);

                if (interferogramInfo.FinalMinIntensity.HasValue)
                {
                    this.finalStartInterval = new Interval<double>(0, interferogramInfo.ModuleValue.Value);
                    this.finalifnishInterval = new Interval<double>(interferogramInfo.FinalMinIntensity.Value, interferogramInfo.ModuleValue.Value);
                    this.finalifnishInterval = new Interval<double>(interferogramInfo.FinalMinIntensity.Value, interferogramInfo.MaxIntensity);
                    this.finalTransform = new RealIntervalTransform(finalStartInterval, finalifnishInterval);
                }               
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
            intensity = this.GetCorrectedIntensity( intensity );

            if (this.transform != null)
            {
                intensity = transform.TransformToFinishIntervalValue(intensity);
                intensity = intensity % interferogramInfo.ModuleValue.Value;

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
