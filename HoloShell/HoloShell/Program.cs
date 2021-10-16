using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;

using HoloCommon.Modules;
using HoloCommon.ProcessManagement;
using HoloCommon.MemoryManagement;

namespace HoloShell
{
    class Program
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(SetConsoleCtrlEventHandler handler, bool add);

        private delegate bool SetConsoleCtrlEventHandler(ExitType signal);

        private static List<Process> processList = new List<Process>();

        static void Main(string[] args)
        {
            Run();
        }

        static void Run()
        {
            Console.WriteLine("***Holo shell***");

            SetConsoleCtrlHandler(ConsoleCtrlHandler, true);

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

                    Process process = ProcessManager.RunProcess(path, arguments, waitForExit);
                    processList.Add(process);
                }

                Console.ReadLine();
            }           
        }

        private static bool ConsoleCtrlHandler(ExitType signal)
        {
            switch (signal)
            {
                case ExitType.CTRL_BREAK_EVENT:
                case ExitType.CTRL_C_EVENT:
                case ExitType.CTRL_LOGOFF_EVENT:
                case ExitType.CTRL_SHUTDOWN_EVENT:
                case ExitType.CTRL_CLOSE_EVENT:

                    KillProcesses();
                    Environment.Exit(0);
                    return false;

                default:
                    return false;
            }
        }

        private static void KillProcesses()
        {
            foreach(Process process in processList)
            {
                ProcessManager.KillProcess(process);
            }
        }
    }
}