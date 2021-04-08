using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HoloCommon.MemoryManagement;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter number 1:");
            string str1 = Console.ReadLine();

            Console.WriteLine("Enter number 2:");
            string str2 = Console.ReadLine();

            double num1 = double.Parse(str1);
            double num2 = double.Parse(str2);

            double res = num1 + num2;

            MemoryWriter.Write(res);
        }
    }
}
