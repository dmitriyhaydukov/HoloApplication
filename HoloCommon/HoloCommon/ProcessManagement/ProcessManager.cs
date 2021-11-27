using System;
using System.Diagnostics;
using System.Management;
using System.Collections.Generic;
using System.Linq;

namespace HoloCommon.ProcessManagement
{
    public class ProcessManager
    {
        public static Process RunProcess(string path, string arguments, bool waitForExit, bool useShellExecute = true)
        {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = arguments;

            process.StartInfo.UseShellExecute = useShellExecute;
            if (!useShellExecute)
            {
                process.StartInfo.CreateNoWindow = true;
            }

            process.StartInfo.RedirectStandardOutput = false;

            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;

            process.Start();

            if (waitForExit)
            {
                process.WaitForExit();
            }

            return process;
        }

        public static void KillProcess(Process process)
        {
            if(process != null)
            {
                try
                {
                    process.Kill();
                }
                catch(Exception)
                {
                    
                }
            }
        }

        private static void KillProcesses(IEnumerable<Process> processes)
        {
            foreach(Process p in processes)
            {
                KillProcess(p);
            }
        }

        private static void Process_Exited(object sender, EventArgs e)
        {
            Process process = sender as Process;
            if (process != null)
            {
                IEnumerable<Process> childProcesses = GetChildProcesses(process);
                KillProcesses(childProcesses);
            }
        }

        private static IEnumerable<Process> GetChildProcesses(Process process)
        {
            List<Process> children = new List<Process>();
            ManagementObjectSearcher mos = 
                new ManagementObjectSearcher
                (String.Format("SELECT * FROM Win32_Process WHERE ParentProcessID={0}", process.Id));

            foreach (ManagementObject mo in mos.Get())
            {
                children.Add(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])));
            }

            return children;
        }

        /*
        private static void KillAllProcessesSpawnedBy(UInt32 parentProcessId)
        {
            // NOTE: Process Ids are reused!
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
            (
                string.Format("SELECT * FROM Win32_Process WHERE ParentProcessId={0}", parentProcessId)               
            );

            ManagementObjectCollection collection = searcher.Get();
            if (collection.Count > 0)
            {
                foreach (var item in collection)
                {
                    UInt32 childProcessId = (UInt32)item["ProcessId"];
                    if ((int)childProcessId != Process.GetCurrentProcess().Id)
                    {
                        KillAllProcessesSpawnedBy(childProcessId);

                        Process childProcess = Process.GetProcessById((int)childProcessId);
                        childProcess.Kill();
                    }
                }
            }
        }
        */
    }
}