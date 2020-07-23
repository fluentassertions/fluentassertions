using System;
using System.Diagnostics;

namespace FluentAssertions.Common
{
    internal class StopwatchTimer : ITimer
    {
        private readonly Stopwatch stopwatch;

        public StopwatchTimer()
        {
            stopwatch = Stopwatch.StartNew();
        }

        public TimeSpan Elapsed => stopwatch.Elapsed;
    }
}
