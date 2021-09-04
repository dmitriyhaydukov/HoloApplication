using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Common
{
    public class Timer : IDisposable
    {
        private string _blockName;
        private Stopwatch _stopwatch;

        public Timer(string blockName)
        {
            _stopwatch = new Stopwatch();
            _blockName = blockName;
            _stopwatch.Start();
            DebugLogger.StartBlock(_blockName);
        }

        public void Dispose()
        {
            DebugLogger.FinishBlock("Finished in " + _stopwatch.ElapsedMilliseconds + " ms");
            _stopwatch.Stop();
            _stopwatch = null;
        }
    }
}
