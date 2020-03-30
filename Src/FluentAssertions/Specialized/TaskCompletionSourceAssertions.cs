using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized
{
    public class TaskCompletionSourceAssertions<T>
    {
        private readonly TaskCompletionSource<T> subject;

        public TaskCompletionSourceAssertions(TaskCompletionSource<T> tcs)
            : this(tcs, new Clock())
        {
        }

        public TaskCompletionSourceAssertions(TaskCompletionSource<T> tcs, IClock clock)
        {
            subject = tcs;
            Clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        private protected IClock Clock { get; }

        /// <summary>
        /// Asserts that the <see cref="Task"/> of the current <see cref="TaskCompletionSource{T}"/> will complete within the specified time.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public Task<AndWhichConstraint<TaskCompletionSourceAssertions<T>, T>> CompleteWithinAsync(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            return AssertAsync(timeSpan, because, becauseArgs,
                "Expected {context} to complete within {0}{reason}, but found <null>.",
                "Expected {context:task} to complete within {0}{reason}.",
                new object[] { timeSpan });
        }

        private async Task<AndWhichConstraint<TaskCompletionSourceAssertions<T>, T>> AssertAsync(
            TimeSpan timeSpan,
            string because,
            object[] becauseArgs,
            string failMessage1,
            string failMessage2,
            object[] failArgs)
        {
            Execute.Assertion
                .ForCondition(subject is object)
                .BecauseOf(because, becauseArgs)
                .FailWith(failMessage1, failArgs);

            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            Task completedTask;
            using (NoSynchronizationContextScope.Enter())
            {
                completedTask = await Task.WhenAny(
                    subject.Task,
                    Clock.DelayAsync(timeSpan, timeoutCancellationTokenSource.Token));
            }

            if (completedTask == subject.Task)
            {
                timeoutCancellationTokenSource.Cancel();
                await completedTask;
            }

            Execute.Assertion
                .ForCondition(completedTask == subject.Task)
                .BecauseOf(because, becauseArgs)
                .FailWith(failMessage2, failArgs);

            return new AndWhichConstraint<TaskCompletionSourceAssertions<T>, T>(this, subject.Task.Result);
        }
    }
}
