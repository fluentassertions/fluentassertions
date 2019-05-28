using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Common;

namespace FluentAssertions.Specs
{
    /// <summary>
    /// Implementation of <see cref="ITimer"/> for testing purposes only.
    /// </summary>
    internal class TestingTimer : ITimer
    {
        private TaskCompletionSource<bool> Signal { get; } = new TaskCompletionSource<bool>();

        Task ITimer.DelayAsync(TimeSpan delay, CancellationToken cancellationToken)
        {
            return Signal.Task;
        }

        bool ITimer.Wait(Task task, TimeSpan timeout)
        {
            Signal.Task.GetAwaiter().GetResult();
            return Signal.Task.Result;
        }

        public void CompletesBeforeTimeout()
        {
            this.Signal.SetResult(true);
        }

        public void RunsIntoTimeout()
        {
            this.Signal.SetResult(false);
        }
    }
}
