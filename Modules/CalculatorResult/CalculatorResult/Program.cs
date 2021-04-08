using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HoloCommon.MemoryManagement;

namespace CalculatorResult
{
    class Program
    {
        static void Main(string[] args)
        {
            double res = MemoryReader.ReadDouble();
            Console.WriteLine(string.Format("Calculator result: {0}", res));
            Console.ReadKey();
        }
    }
}
