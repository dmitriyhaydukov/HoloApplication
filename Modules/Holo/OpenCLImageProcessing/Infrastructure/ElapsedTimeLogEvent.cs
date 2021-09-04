using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Infrastructure
{
    /// <summary>
    /// Создает два сообщения
    /// [eventMessage] started
    /// [eventMessage] finished in [elapsedMilliseconds] ms
    /// </summary>
    public class ElapsedTimeLogEvent
    {
        private string _eventMessage;
        private Stopwatch _stopwatch;
        public ElapsedTimeLogEvent(string eventMessage)
        {
            _eventMessage = eventMessage;
            _stopwatch = new Stopwatch();
            LoggerProvider.Get().Log(eventMessage + " started", Logger.LogType.Performance);
            _stopwatch.Start();
        }

        ~ElapsedTimeLogEvent()
        {
            _stopwatch.Stop();
            LoggerProvider.Get().Log($"{_eventMessage} finished in {_stopwatch.ElapsedMilliseconds} ms", Logger.LogType.Performance);
            _stopwatch = null;
            _eventMessage = null;
        }
    }
}
