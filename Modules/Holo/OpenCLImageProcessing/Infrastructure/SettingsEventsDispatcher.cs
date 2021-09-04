using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Infrastructure
{
    public interface ISettingsEventsDispatcher
    {
        Type SettingsType { get; }
    }
    public class SettingsEventsDispatcher<TIn>: ISettingsEventsDispatcher
    {
        public delegate bool SettingsRequested(TIn settings);

        public event SettingsRequested OnSettingsRequested;

        public Type SettingsType => typeof(TIn);

        public bool RequestSettings(TIn settings)
        {
            if (OnSettingsRequested != null)
            {
                OnSettingsRequested.Invoke(settings);
                return true;
            }

            return false;
        }
    }

    public static class SettingsEventsDispatcherProvider
    {
        private static List<ISettingsEventsDispatcher> dispatchers = new List<ISettingsEventsDispatcher>();

        public static SettingsEventsDispatcher<T> Get<T>()
        {
            return (SettingsEventsDispatcher<T>) dispatchers.FirstOrDefault(d => d.SettingsType == typeof(T)) ??
                   new SettingsEventsDispatcher<T>();
        }
    }
}
