using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class DictionaryExtensions
    {
        public static void SetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue value)
        {
            if (self.ContainsKey(key))
                self[key] = value;
            else
                self.Add(key, value);
        }
    }
}
