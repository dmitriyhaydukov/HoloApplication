using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rab1
{
    public class GammaCorrection
    {
        private static int quantization = 16;
        private static Dictionary<int, int> gammaCorrectionMap = new Dictionary<int, int>()
        {
            { 0,    0   },
            { 1,    7  },
            { 2,    11  },
            { 3,    16  },
            { 4,    21  },
            { 5,    34  },
            { 6,    46  },
            { 7,    50  },
            { 8,    70 },
            { 9,    90 },
            { 10,   120 },
            { 11,   151 },
            { 12,   160 },
            { 13,   180 },
            { 14,   210 },
            { 15,   190 },
            { 16,   220 }
        };

        public static double GetCorrectedValue(double value)
        {
            return gammaCorrectionMap[Convert.ToInt32(value) / quantization];  
        }
    }
}
