using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertionsAsync.Common;
#if NET8_0_OR_GREATER
using ITimer = FluentAssertionsAsync.Common.ITimer;
#endif

namespace FluentAssertionsAsync.Specs;

/// <summary>
/// Implementation of <see cref="IClock"/> for testing purposes only.
/// </summary>
/// <remarks>
/// It allows you to control the "current" date and time for test purposes.
/// </remarks>
internal class FakeClock : IClock
{
    private readonly TaskCompletionSource<bool> delayTask = new();

    private TimeSpan elapsedTime = TimeSpan.Zero;

    Task IClock.DelayAsync(TimeSpan delay, CancellationToken cancellationToken)
    {
        elapsedTime += delay;
        return delayTask.Task;
    }

    public ITimer StartTimer() => new TestTimer(() => elapsedTime);

    /// <summary>
    /// Advances the internal clock.
    /// </summary>
    public void Delay(TimeSpan timeToDelay)
    {
        elapsedTime += timeToDelay;
    }

    /// <summary>
    /// Simulates the completion of the pending delay task.
    /// </summary>
    public void Complete()
    {
        // the value is not relevant
        delayTask.SetResult(true);
    }

    /// <summary>
    /// Simulates the completion of the pending delay task after the internal clock has been advanced.
    /// </summary>
    public void CompleteAfter(TimeSpan timeSpan)
    {
        Delay(timeSpan);

        // the value is not relevant
        delayTask.SetResult(true);
    }
}
