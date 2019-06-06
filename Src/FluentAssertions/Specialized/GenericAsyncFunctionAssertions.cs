using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized
{
    public class GenericAsyncFunctionAssertions<TResult> : AsyncFunctionAssertions
    {
        private readonly Func<Task<TResult>> subject;
        private readonly IClock clock;

        public GenericAsyncFunctionAssertions(Func<Task<TResult>> subject, IExtractExceptions extractor) : this(subject, extractor, new Clock())
        {
        }

        public GenericAsyncFunctionAssertions(Func<Task<TResult>> subject, IExtractExceptions extractor, IClock clock) : base(
            subject, extractor, clock)
        {
            this.subject = subject;
            this.clock = clock;
        }

        /// <summary>
        /// Asserts that the current <see cref="Task{T}"/> will complete within specified time.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, Task<TResult>> CompleteWithin(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            Task<TResult> task = subject();
            bool completed = clock.Wait(task, timeSpan);

            Execute.Assertion
                .ForCondition(completed)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);

            return new AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, Task<TResult>>(this, task);
        }

        /// <summary>
        /// Asserts that the current <see cref="Task{T}"/> will complete within the specified time.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public async Task<AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, Task<TResult>>> CompleteWithinAsync(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                Task<TResult> task = subject();

                Task completedTask =
                    await Task.WhenAny(task, clock.DelayAsync(timeSpan, timeoutCancellationTokenSource.Token));

                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    await completedTask;
                }

                Execute.Assertion
                    .ForCondition(completedTask == task)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);

                return new AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, Task<TResult>>(this, task);
            }
        }
    }
}
