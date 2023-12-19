using System;
using System.Diagnostics;

namespace FluentAssertionsAsync.Common;

internal sealed class StopwatchTimer : ITimer
{
    private readonly Stopwatch stopwatch = Stopwatch.StartNew();

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
