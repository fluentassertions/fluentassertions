using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Default implementation for <see cref="ITimer"/> for production use.
    /// </summary>
    internal class TaskTimer : ITimer
    {
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
    }
}
