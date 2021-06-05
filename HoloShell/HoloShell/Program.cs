using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;

using HoloCommon.Modules;
using HoloCommon.ProcessManagement;
using HoloCommon.MemoryManagement;

using HoloCommon.Models.Charting;
using HoloCommon.Serialization.Charting;

namespace HoloShell
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
            //Test();
        }

        static void Run()
        {
            Console.WriteLine("***Holo shell***");

            ModulesReader modulesReader = new ModulesReader();
            ModulesList modulesList = modulesReader.ReadModules(ShellConfig.ModulesDescriptorPath);

            using (MemoryMappedFile mmf = MemoryBaseProcessor.CreateMMF())
            {
                for (int k = 0; k < modulesList.ModuleItems.Count; k++)
                {
                    ModuleItem moduleItem = modulesList.ModuleItems[k];

                    string path = moduleItem.Path;
                    string arguments = moduleItem.Arguments;
                    bool waitForExit = moduleItem.WaitForExit;

                    ProcessManager.RunProcess(path, arguments, waitForExit);
                }
            }

            Console.ReadLine();
        }

        static void Test()
        {
            TestCharting();
        }

        static void TestCharting()
        {
            using (MemoryMappedFile mmf = MemoryBaseProcessor.CreateMMF())
            {
                Chart chart = new Chart();
                ChartSeries chartSeries = new ChartSeries()
                {
                    Points = new List<ChartPoint>()
                    {
                        new ChartPoint(1, 1),
                        new ChartPoint(2, 6),
                        new ChartPoint(3, 3),
                        new ChartPoint(4, 7),
                        new ChartPoint(5, 2),
                        new ChartPoint(6, 5)
                    }
                };

                chart.SeriesCollection = new List<ChartSeries>()
                {
                    chartSeries
                };

                ChartSerialization cs = new ChartSerialization();
                MemoryWriter.Write<Chart>(chart, cs);

                chart = MemoryReader.Read<Chart>(cs);
            }
        }
    }
}