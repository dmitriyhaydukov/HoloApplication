using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using HoloCommon.Synchronization;

namespace SyncTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            SynchronizationManager.RunActionOnSignal(PictureTakenHandler, Events.Camera.PICTURE_TAKEN);

            string command = null;
            while(!String.Equals(command, "q", StringComparison.OrdinalIgnoreCase))
            {
                command = Console.ReadLine();
                if (command == "sync")
                {
                    SynchronizationManager.SetSignal(Events.Camera.TAKE_PICTURE);
                }
            }
        }

        static void PictureTakenHandler()
        {
            Console.WriteLine("picture taken handler");
        }
    }
}
