using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.Utils
{
    public class NamingUtil
    {
        private static Dictionary<string, int> _titlesIndices = new Dictionary<string, int>();
        public static string IndexedTitle(string title, int startIndex = 1)
        {
            if (!_titlesIndices.ContainsKey(title))
                _titlesIndices.Add(title, startIndex);

            var index = _titlesIndices[title];
            _titlesIndices[title] = index + 1;

            return title + index;
        }
    }
}
