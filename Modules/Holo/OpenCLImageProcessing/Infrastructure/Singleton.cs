using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public static class Singleton
    {
        private static readonly List<object> _singletones = new List<object>();

        public static T Get<T>()
        {
            if (!_singletones.OfType<T>().Any())
            {
                if (!typeof(T).IsAbstract)
                    _singletones.Add(Activator.CreateInstance<T>());
                else
                    throw new InvalidOperationException($"Can't find or create object of type {typeof(T).Name}");
            }

            return _singletones.OfType<T>().Single();
        }

        public static void Register<T>(T instance)
        {
            if (_singletones.OfType<T>().Any())
                throw new InvalidOperationException($"Instance of type {typeof(T).Name} is already registered.");
            
            _singletones.Add(instance);
        }
    }
}