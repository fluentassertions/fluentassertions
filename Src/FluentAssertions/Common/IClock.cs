using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Represents an abstract timer that is used to make some of this library's timing dependent functionality better testable.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Will block the current thread until a time delay has passed.
        /// </summary>
        /// <param name="timeToDelay">The time span to wait before completing the returned task</param>
        void Delay(TimeSpan timeToDelay);

        /// <summary>
        /// Creates a task that will complete after a time delay.
        /// </summary>
        /// <param name="delay">The time span to wait before completing the returned task</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task that represents the time delay.</returns>
        /// <seealso cref="Task.Delay(TimeSpan)"/>
        Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a timer to measure the time to complete some arbitrary executions.
        /// </summary>
        ITimer StartTimer();
    }
}
