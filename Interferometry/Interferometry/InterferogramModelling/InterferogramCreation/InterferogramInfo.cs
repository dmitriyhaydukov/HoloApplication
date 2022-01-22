using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interferometry.InterferogramCreation {
    //Параметры интерференционной картины
    public class InterferogramInfo {
        int width;                  //Ширина интерферограммы
        int height;                 //Высота интерферограммы

        double meanIntensity;       //Средняя интенсивность
        double intensityModulation; //Модуляция интенсивности
        double minIntensity;        //Минимальное значение интенсивности
        double maxIntensity;        //Максимаьная интенсивность

        double noisePercent;        //Процент шума интенсивности
        double maxNoise;            //Максимальное значение шума
        
        double? maxRange;
        int? moduleValue;

        //--------------------------------------------------------------------------------
        public InterferogramInfo(
            int width,
            int height,
            double percentNoise,
            double minIntensity,
            double? maxRange,
            int? moduleValue
        ) {
            this.width = width;
            this.height = height;
            this.noisePercent = percentNoise;

            this.minIntensity = minIntensity;
            this.maxIntensity = 255;
            this.meanIntensity = (this.maxIntensity - this.minIntensity) / 2.0;
            this.intensityModulation = 1;
            this.maxNoise = this.maxIntensity / 100 * this.noisePercent;

            this.maxRange = maxRange;
            this.moduleValue = moduleValue;
        }
        //--------------------------------------------------------------------------------
        //Ширина
        public int Width {
            get {
                return this.width;
            }
        }
        //--------------------------------------------------------------------------------
        //Высота
        public int Height {
            get {
                return this.height; 
            }
        }
        //--------------------------------------------------------------------------------
        //Максимальная интенсивность
        public double MaxIntensity {
            get {
                return this.maxIntensity;
            }
        }
        //--------------------------------------------------------------------------------
        //Максимальная интенсивность
        public double MinIntensity
        {
            get
            {
                return this.minIntensity;
            }
        }
        //--------------------------------------------------------------------------------
        //Средняя интенсивность
        public double MeanIntensity {
            get {
                return this.meanIntensity;
            }
        }
        //--------------------------------------------------------------------------------
        //Модуляция интенсивности
        public double IntensityModulation {
            get {
                return this.intensityModulation;
            }
        }
        //--------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------
        //Максимальный шум
        public double MaxNoise {
            get {
                return this.maxNoise;
            }
        }
        //--------------------------------------------------------------------------------
        public double? MaxRange
        {
            get
            {
                return this.maxRange;
            }
        }
        //--------------------------------------------------------------------------------
        public int? ModuleValue
        {
            get
            {
                return this.moduleValue;
            }
        }
        //--------------------------------------------------------------------------------
    }
}
