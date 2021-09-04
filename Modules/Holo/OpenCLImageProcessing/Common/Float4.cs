using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public struct Float4
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Float4(float[] vector)
        {
            X = vector[0];
            Y = vector[1];
            Z = vector[2];
            W = vector[3];
        }
    }
}
