using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processing.Computing
{
    public interface IOpenClSourcesProvider
    {
        string GetProgram(string name, bool throwErrorIfNotExists = true);
        void SaveProgram(string program, string name);
    }
}
