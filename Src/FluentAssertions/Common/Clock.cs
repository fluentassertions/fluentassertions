using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Default implementation for <see cref="IClock"/> for production use.
    /// </summary>
    internal class Clock : IClock
    {
        public void Delay(TimeSpan timeToDelay) => Task.Delay(timeToDelay).GetAwaiter().GetResult();

        public Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken)
        {
            return Task.Delay(delay, cancellationToken);
        }

        public bool Wait(Task task, TimeSpan timeout)
        {
            using (NoSynchronizationContextScope.Enter())
            {
                return task.Wait(timeout);
            }
        }

        public ITimer StartTimer() => new StopwatchTimer();
    }

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
