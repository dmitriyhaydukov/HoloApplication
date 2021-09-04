using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public struct FloatRange
    {
        public float Min;
        public float Max;

        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            return $"{Min} - {Max}";
        }
    }
}
