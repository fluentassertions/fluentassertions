using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized
{
    public class GenericAsyncFunctionAssertions<TResult> : AsyncFunctionAssertions<Task<TResult>, GenericAsyncFunctionAssertions<TResult>>
    {
        public GenericAsyncFunctionAssertions(Func<Task<TResult>> subject, IExtractExceptions extractor)
            : this(subject, extractor, new Clock())
        {
        }

        public GenericAsyncFunctionAssertions(Func<Task<TResult>> subject, IExtractExceptions extractor, IClock clock)
            : base(subject, extractor, clock)
        {
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public new async Task<AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>> CompleteWithinAsync(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to complete within {0}{reason}, but found <null>.", timeSpan);

            ITimer timer = Clock.StartTimer();
            Task<TResult> task = Subject.Invoke();
            TimeSpan remainingTime = timeSpan - timer.Elapsed;

            bool success = Execute.Assertion
                .ForCondition(remainingTime >= TimeSpan.Zero)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);

            if (success)
            {
                using var timeoutCancellationTokenSource = new CancellationTokenSource();
                Task completedTask =
                    await Task.WhenAny(task, Clock.DelayAsync(remainingTime, timeoutCancellationTokenSource.Token));

                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    await completedTask;
                }

                Execute.Assertion
                    .ForCondition(completedTask == task)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);
            }

            return new AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>(this, task.Result);
        }

        /// <summary>
        /// Asserts that the current <see cref="Task{T}"/> does not throw any exception.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public new async Task<AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>> NotThrowAsync(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} not to throw{reason}, but found <null>.");

            try
            {
                TResult result = await Subject.Invoke();
                return new AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>(this, result);
            }
            catch (Exception exception)
            {
                NotThrowInternal(exception, because, becauseArgs);
                return new AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>(this, default(TResult));
            }
        }

        /// <summary>
        /// Asserts that the current <see cref="Task{T}"/> stops throwing any exception
        /// after a specified amount of time.
        /// </summary>
        /// <remarks>
        /// The <see cref="Task{T}"/> is invoked. If it raises an exception,
        /// the invocation is repeated until it either stops raising any exceptions
        /// or the specified wait time is exceeded.
        /// </remarks>
        /// <param name="waitTime">
        /// The time after which the <see cref="Task{T}"/> should have stopped throwing any exception.
        /// </param>
        /// <param name="pollInterval">
        /// The time between subsequent invocations of the <see cref="Task{T}"/>.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if waitTime or pollInterval are negative.</exception>
        public new Task<AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>> NotThrowAfterAsync(TimeSpan waitTime, TimeSpan pollInterval, string because = "", params object[] becauseArgs)
        {
            if (waitTime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(waitTime), $"The value of {nameof(waitTime)} must be non-negative.");
            }

            if (pollInterval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(pollInterval),
                    $"The value of {nameof(pollInterval)} must be non-negative.");
            }

            Execute.Assertion
                .ForCondition(Subject is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} not to throw any exceptions after {0}{reason}, but found <null>.", waitTime);

            return AssertionTaskAsync();

            async Task<AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>> AssertionTaskAsync()
            {
                TimeSpan? invocationEndTime = null;
                Exception exception = null;
                ITimer timer = Clock.StartTimer();

                while (invocationEndTime is null || invocationEndTime < waitTime)
                {
                    try
                    {
                        TResult result = await Subject.Invoke();
                        return new AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>(this, result);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                        await Clock.DelayAsync(pollInterval, CancellationToken.None);
                        invocationEndTime = timer.Elapsed;
                    }
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect any exceptions after {0}{reason}, but found {1}.", waitTime, exception);

                return new AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>(this, default(TResult));
            }
        }
    }
}
