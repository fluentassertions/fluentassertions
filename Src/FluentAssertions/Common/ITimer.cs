using System;
using System.Threading.Tasks;

namespace FluentAssertions.Common
{
    /// <summary>
    /// The abstraction for timer operations.
    /// </summary>
    /// <remarks>
    /// This interface is intended for internal use only.
    /// </remarks>
    public interface ITimer
    {
        /// <summary>
        /// Creates a task that will complete after a time delay.
        /// </summary>
        /// <param name="delay">The time span to wait before completing the returned task</param>
        /// <returns>A task that represents the time delay.</returns>
        /// <seealso cref="Task.Delay(TimeSpan)"/>
        Task DelayAsync(TimeSpan delay);

        /// <summary>
        /// Waits for the task for specified time.
        /// </summary>
        /// <param name="task">The task to be waited for.</param>
        /// <param name="timeout">The time span to wait.</param>
        /// <returns><c>true</c> if the task completes before specified timeout.</returns>
        bool Wait(Task task, TimeSpan timeout);
    }
}
