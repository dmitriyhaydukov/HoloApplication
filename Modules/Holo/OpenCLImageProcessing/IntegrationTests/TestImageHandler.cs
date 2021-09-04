using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processing;

namespace IntegrationTests
{
    public class TestImageHandler : ImageHandler
    {
        public TestImageHandler(int[] data)
        {
            Data = new byte[data.Length * sizeof(int)];

            unsafe
            {
                fixed (byte* pd = Data)
                {
                    var ipd = (int*) pd;

                    for (var i = 0; i < data.Length; i++)
                    {
                        *ipd++ = data[i];
                    }
                }
            }
        }
    }
}
