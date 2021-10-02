using System.Diagnostics;

namespace HoloCommon.ProcessManagement
{
    public class ProcessManager
    {
        public static void RunProcess(string path, string arguments, bool waitForExit)
        {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.RedirectStandardOutput = false;
            process.Start();

            if (waitForExit)
            {
                process.WaitForExit();
            }
        }
    }
}