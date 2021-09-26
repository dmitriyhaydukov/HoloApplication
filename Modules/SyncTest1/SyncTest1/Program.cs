using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using HoloCommon.Synchronization;

namespace SyncTest1
{
    class Program
    {        
        static void Main(string[] args)
        {
            SynchronizationManager.RunActionOnSignal(Process, Events.Camera.TAKE_PICTURE);           
        }

        private static void Process()
        {
            Console.WriteLine("picture taken");
            SynchronizationManager.SetSignal(Events.Camera.PICTURE_TAKEN);
        }
    }
}
