using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HoloCommon.Synchronization
{
    public class SynchronizationManager
    {
        public static void SetSignal(string name)
        {
            EventWaitHandle eventWaitHandle = CreateEventWaitHandle(name);
            eventWaitHandle.Set();
        }

        public static void ResetSignal(string name)
        {
            EventWaitHandle eventWaitHandle = CreateEventWaitHandle(name);
            eventWaitHandle.Reset();
        }

        //Returns runned thread in which action is executed
        public static Thread RunActionOnSignal(Action action, string signalName)
        {
            ThreadStart threadStart = () => 
            {
                EventWaitHandle eventWaitHandle = CreateEventWaitHandle(signalName);
                while (eventWaitHandle.WaitOne())
                {
                    action();
                    eventWaitHandle.Reset();
                }
            };

            Thread thread = new Thread(threadStart);
            thread.Start();

            return thread;
        }

        private static EventWaitHandle CreateEventWaitHandle(string name)
        {
            EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, name);
            return eventWaitHandle;
        }
    }
}
