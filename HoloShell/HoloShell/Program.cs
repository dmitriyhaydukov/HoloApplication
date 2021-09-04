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
    }
}