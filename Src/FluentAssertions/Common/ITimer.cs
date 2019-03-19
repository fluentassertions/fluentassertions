using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Represents an abstract timer that is used to make some of this library's timing dependent functionality better testable.
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// Creates a task that will complete after a time delay.
        /// </summary>
        /// <param name="delay">The time span to wait before completing the returned task</param>
        /// <param name="timeoutCancellationTokenSource"></param>
        /// <returns>A task that represents the time delay.</returns>
        /// <seealso cref="Task.Delay(TimeSpan)"/>
        Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken);

        /// <summary>
        /// Waits for the task for a specified time.
        /// </summary>
        /// <param name="task">The task to be waited for.</param>
        /// <param name="timeout">The time span to wait.</param>
        /// <returns><c>true</c> if the task completes before specified timeout.</returns>
        bool Wait(Task task, TimeSpan timeout);
    }
}
