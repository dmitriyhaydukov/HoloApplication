using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Processing.DataProcessors;

namespace Processing.Computing
{
    public class ImageProcessing
    {
        public static void Shift(IImageHandler input, IImageHandler output, int dx, int dy, bool cyclic)
        {
            if (!input.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("shift", input.Width, input.Height, input.ComputeBuffer, output.ComputeBuffer, dx, dy, cyclic ? 1 : 0);
        }

        /// <summary>
        /// Попиксельное деление комплексных изображения
        /// </summary>
        /// <param name="numerator">Делимое</param>
        /// <param name="denumerator">Делитель</param>
        public static void Divide(IImageHandler numerator, IImageHandler denumerator, IImageHandler output)
        {
            if (!numerator.SizeEquals(denumerator) || !numerator.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("divide", output.Width, output.Height, numerator.ComputeBuffer, denumerator.ComputeBuffer, output.ComputeBuffer);
        }

        public static void Sum(IImageHandler image1, IImageHandler image2, IImageHandler output)
        {
            if (!image1.SizeEquals(image2) || !image1.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("sum", output.Width, output.Height, image1.ComputeBuffer, image2.ComputeBuffer, output.ComputeBuffer);
        }

        public static void Divide(IImageHandler image, float num, IImageHandler output)
        {
            if (!image.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("divideByNum", output.Width, output.Height, image.ComputeBuffer, output.ComputeBuffer, num);
        }

        public static void Combine(IImageHandler amplitudeImage, IImageHandler phaseImage, IImageHandler output)
        {
            if (!amplitudeImage.SizeEquals(phaseImage) || !amplitudeImage.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("combineAmplitudeAndPhase", output.Width, output.Height, amplitudeImage.ComputeBuffer, phaseImage.ComputeBuffer, output.ComputeBuffer);
        }

        public static void Interference(IImageHandler image1, IImageHandler image2, IImageHandler output)
        {
            if (!image1.SizeEquals(image1) || !output.SizeEquals(image1))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("interference", output.Width, output.Height, image1.ComputeBuffer, image2.ComputeBuffer, output.ComputeBuffer);
        }
    }
}
