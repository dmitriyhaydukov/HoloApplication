using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloo;
using Infrastructure;
using OpenTK;
using Processing;
using Processing.Computing;

namespace IntegrationTests.KernelTests
{
    public abstract class KernelTestBase : TestBase
    {
        public OpenClApplication OpenClApplication => Singleton.Get<OpenClApplication>();

        public IImageHandler CreateImageFromIntArray(int[] input)
        {
            return new TestImageHandler(input);
        }

        public int[] ReadIntArrayFromBuffer(ComputeBuffer<Vector2> buffer)
        {
            var tmpBuffer = new Vector2[buffer.Size];
            var output = new int[buffer.Size];
            OpenClApplication.Queue.ReadFromBuffer(buffer, ref tmpBuffer, true, null);

            for (var i = 0; i < output.Length; i++)
            {
                output[i] = (int)tmpBuffer[i].X;
            }

            return output;
        }
    }
}