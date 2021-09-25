using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncTest2
{
    class Program
    {
        private const string SYNC_EVENT_NAME = "HoloSyncEvent";

        static void Main(string[] args)
        {
            EventWaitHandle handle = EventWaitHandle.OpenExisting(SYNC_EVENT_NAME);

            string command = null;
            while(!String.Equals(command, "q", StringComparison.OrdinalIgnoreCase))
            {
                command = Console.ReadLine();
                if (command == "sync")
                {
                    handle.Set();
                }
            }
        }
    }
}
