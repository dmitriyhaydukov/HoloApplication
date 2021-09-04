using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloo;
using Infrastructure;
using OpenTK;
using Processing.Utils;

namespace Processing.Computing
{
    public class Freshnel
    {
        private OpenClApplication App => Singleton.Get<OpenClApplication>();
        private Fourier _fourier;
        private IImageHandler _oldInput;
        private ComputeBuffer<Vector2> _freshnelInnerMultipliersX;
        private ComputeBuffer<Vector2> _freshnelInnerMultipliersY;
        private int _width;
        private int _height;
        private float _wavelength;
        private float _distance;
        private float _objectSize;

        public Freshnel(int width, int height)
        {
            _width = width;
            _height = height;

            _freshnelInnerMultipliersX = new ComputeBuffer<Vector2>(App.ComputeContext, ComputeMemoryFlags.None, _width);
            _freshnelInnerMultipliersY = new ComputeBuffer<Vector2>(App.ComputeContext, ComputeMemoryFlags.None, _height);
        }

        public void Compute(IImageHandler input, IImageHandler output, float wavelength, float distance, float objectSize, bool cyclicShift)
        {
            if (input.Width != _width || input.Height != _height)
                throw new InvalidOperationException("Неожиданные размеры изображения.");

            if (wavelength != _wavelength || distance != _distance || objectSize != _objectSize)
            {
                _wavelength = wavelength;
                _distance = distance;
                _objectSize = objectSize;

                App.ExecuteKernel("freshnelGenerateInnerMultipliers", _width, 1, _freshnelInnerMultipliersX, _wavelength / 1000f, _distance * 1000f, (float) _width, _objectSize * 1000f);
                App.ExecuteKernel("freshnelGenerateInnerMultipliers", _height, 1, _freshnelInnerMultipliersY, _wavelength / 1000f, _distance * 1000f, (float)_height, _objectSize * 1000f);
            }

            App.ExecuteKernel("freshnelMultiplyInner", _width, _height, input, output, _freshnelInnerMultipliersX, _freshnelInnerMultipliersY);

            Fourier.Transform(output);

            App.ExecuteKernel("freshnelMultiplyInner", _width, _height, output, output, _freshnelInnerMultipliersX, _freshnelInnerMultipliersY);

            if (cyclicShift)
                ImageUtils.CyclicShift(output);
        }

        #region public static

        private static Dictionary<string, Freshnel> _cachedProcessors = new Dictionary<string, Freshnel>();

        /// <summary>
        /// Преобразование Френеля
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="wavelength">Длина волны, нм</param>
        /// <param name="distance">Расстояние, мм</param>
        /// <param name="objectSize">Размер объекта, мм</param>
        /// <param name="cyclicShift"></param>
        public static void Transform(IImageHandler input, IImageHandler output, float wavelength, float distance, float objectSize, bool cyclicShift)
        {
            var key = $"{input.Width}_{input.Height}";

            if (!_cachedProcessors.TryGetValue(key, out Freshnel processor))
                _cachedProcessors[key] = processor = new Freshnel(input.Width, input.Height);

            processor.Compute(input, output, wavelength, distance, objectSize, cyclicShift);
        }

        #endregion
    }
}
