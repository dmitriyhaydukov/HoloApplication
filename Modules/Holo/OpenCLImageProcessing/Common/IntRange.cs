using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class IntRange
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}
