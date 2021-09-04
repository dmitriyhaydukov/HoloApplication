using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace Infrastructure
{
    public class Logger
    {
        public enum LogType
        {
            Info,
            Warning,
            Error,
            FatalError,
            Assert,
            Performance,
            Progress
        }

        private readonly ConcurrentBag<LogListener> _logListeners = new ConcurrentBag<LogListener>();

        public void AddListener(Action<LogMessageBase> listener)
        {
            _logListeners.Add(new LogListener(listener));
        }

        public void Log(string message, LogType logType = LogType.Info, object sender = null)
        {
            foreach (var listener in _logListeners)
            {
                listener.TriggerLogMessageEvent(new LogMessage
                {
                    Message = message,
                    Type = logType,
                    Sender = sender
                });
            }
        }

        protected class LogListener
        {
            private readonly Action<LogMessageBase> _onLogMessageEvent;
            private readonly SynchronizationContext _synchronizationContext;

            public LogListener(Action<LogMessageBase> listener)
            {
                _onLogMessageEvent = listener;
                _synchronizationContext = SynchronizationContext.Current;
            }

            public void TriggerLogMessageEvent(LogMessageBase message)
            {
                _synchronizationContext.Send(state => { _onLogMessageEvent.Invoke((LogMessageBase) state); }, message);
            }
        }

        public abstract class LogMessageBase
        {
            private readonly ConcurrentBag<LogListener> _messageUpdateListeners = new ConcurrentBag<LogListener>();
            public Guid Id { get; protected set; }
            public virtual string Message { get; set; }
            public virtual LogType Type { get; set; }
            public object Sender { get; set; }

            protected LogMessageBase()
            {
                Id = Guid.NewGuid();
            }

            public virtual void AddUpdateListener(Action<LogMessageBase> listener)
            {
                _messageUpdateListeners.Add(new LogListener(listener));
            }

            public virtual void Update()
            {
                foreach (var messageUpdateListener in _messageUpdateListeners)
                {
                    messageUpdateListener.TriggerLogMessageEvent(this);
                }
            }
        }
    }

    public static class LoggerProvider
    {
        private static Logger _logger;

        public static Logger Get()
        {
            return _logger ?? (_logger = new Logger());
        }
    }
}