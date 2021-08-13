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

        public void Dispose()
        {
            if (stopwatch.IsRunning)
            {
                // We want to keep the elapsed time available after the timer is disposed, so disposing
                // just stops it.
                stopwatch.Stop();
            }
        }
    }
}
