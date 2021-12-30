using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized
{
#pragma warning disable CS0659 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
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
        public async Task<AndWhichConstraint<TaskCompletionSourceAssertions<T>, T>> CompleteWithinAsync(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(subject is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to complete within {0}{reason}, but found <null>.", timeSpan);

            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            Task completedTask = await Task.WhenAny(
                subject.Task,
                Clock.DelayAsync(timeSpan, timeoutCancellationTokenSource.Token));

            if (completedTask == subject.Task)
            {
                timeoutCancellationTokenSource.Cancel();
                await completedTask;
            }

            Execute.Assertion
                .ForCondition(completedTask == subject.Task)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);
            return new AndWhichConstraint<TaskCompletionSourceAssertions<T>, T>(this, subject.Task.Result);
        }

        /// <summary>
        /// Asserts that the <see cref="Task"/> of the current <see cref="TaskCompletionSource{T}"/> will not complete within the specified time.
        /// </summary>
        /// <param name="timeSpan">The time span to wait for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public async Task NotCompleteWithinAsync(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(subject is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to not complete within {0}{reason}, but found <null>.", timeSpan);

            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            Task completedTask = await Task.WhenAny(
                subject.Task,
                Clock.DelayAsync(timeSpan, timeoutCancellationTokenSource.Token));

            if (completedTask == subject.Task)
            {
                timeoutCancellationTokenSource.Cancel();
                await completedTask;
            }

            Execute.Assertion
                .ForCondition(completedTask != subject.Task)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to not complete within {0}{reason}.", timeSpan);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            throw new NotSupportedException("Calling Equals on Assertion classes is not supported.");
    }
}
