using System.Diagnostics;

namespace HoloShell.ProcessManagement
{
    public class ProcessManager
    {
        public static void RunProcessAndWaitForExit(string path, string arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = false;
            process.Start();
            process.WaitForExit();
        }
    }
}