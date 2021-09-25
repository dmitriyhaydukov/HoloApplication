using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncTest1
{
    class Program
    {
        private const string SYNC_EVENT_NAME = "HoloSyncEvent";

        static void Main(string[] args)
        {
            ThreadStart threadStart = Program.Process;
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        private static void Process()
        {
            EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.ManualReset, SYNC_EVENT_NAME);
            while(handle.WaitOne())
            {
                Console.WriteLine("holo sync event");
                handle.Reset();
            }
        }
    }
}
