using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentAssertionsAsync.Common;

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

    public ITimer StartTimer() => new StopwatchTimer();
}
