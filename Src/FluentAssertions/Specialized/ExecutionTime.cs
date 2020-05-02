using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FluentAssertions.Specialized
{
    public class ExecutionTime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionTime"/> class.
        /// </summary>
        /// <param name="action">The action of which the execution time must be asserted.</param>
        public ExecutionTime(Action action)
            : this(action, "the action")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionTime"/> class.
        /// </summary>
        /// <param name="action">The action of which the execution time must be asserted.</param>
        public ExecutionTime(Func<Task> action)
            : this(action, "the action")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionTime"/> class.
        /// </summary>
        /// <param name="action">The action of which the execution time must be asserted.</param>
        /// <param name="actionDescription">The description of the action to be asserted.</param>
        protected ExecutionTime(Action action, string actionDescription)
        {
            ActionDescription = actionDescription;
            stopwatch = new Stopwatch();
            IsRunning = true;
            Task = Task.Run(() =>
            {
                // move stopwatch as close to action start as possible
                // so that we have to get correct time readings
                try
                {
                    stopwatch.Start();
                    action();
                }
                catch (Exception exception)
                {
                    Exception = exception;
                }
                finally
                {
                    // ensures that we stop the stopwatch even on exceptions
                    stopwatch.Stop();
                    IsRunning = false;
                }
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionTime"/> class.
        /// </summary>
        /// <param name="action">The action of which the execution time must be asserted.</param>
        /// <param name="actionDescription">The description of the action to be asserted.</param>
        /// <remarks>
        /// This constructor is almost exact copy of the one accepting <see cref="Action"/>.
        /// The original constructor shall stay in place in order to keep backward-compatibility
        /// and to avoid unnecessary wrapping action in <see cref="Task"/>.
        /// </remarks>
        protected ExecutionTime(Func<Task> action, string actionDescription)
        {
            ActionDescription = actionDescription;
            stopwatch = new Stopwatch();
            IsRunning = true;
            Task = Task.Run(async () =>
            {
                // move stopwatch as close to action start as possible
                // so that we have to get correct time readings
                try
                {
                    stopwatch.Start();
                    await action();
                }
                catch (Exception exception)
                {
                    Exception = exception;
                }
                finally
                {
                    // ensures that we stop the stopwatch even on exceptions
                    stopwatch.Stop();
                    IsRunning = false;
                }
            });
        }

        internal TimeSpan ElapsedTime => stopwatch.Elapsed;

        internal bool IsRunning { get; private set; }

        internal string ActionDescription { get; }

        internal Task Task { get; }

        internal Exception Exception { get; private set; }

        private readonly Stopwatch stopwatch;
    }
}
