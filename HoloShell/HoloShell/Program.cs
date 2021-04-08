using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;

using HoloShell.Modules;
using HoloShell.ProcessManagement;
using HoloCommon.MemoryManagement;

namespace HoloShell
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***Holo shell***");

            ModulesReader modulesReader = new ModulesReader();
            ModulesList modulesList = modulesReader.ReadModules(ShellConfig.ModulesDescriptorPath);

            using (MemoryMappedFile mmf = MemoryBaseProcessor.CreateMMF())
            {
                for(int k = 0; k < modulesList.ModuleItems.Count; k++)
                {
                    ModuleItem moduleItem = modulesList.ModuleItems[k];
                    
                    string path = moduleItem.Path;
                    string arguments = moduleItem.Arguments;

                    ProcessManager.RunProcessAndWaitForExit(path, arguments);
                }
            }
        }
    }
}