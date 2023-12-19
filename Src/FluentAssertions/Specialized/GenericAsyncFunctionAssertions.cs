using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Specialized;

/// <summary>
/// Contains a number of methods to assert that an asynchronous method yields the expected result.
/// </summary>
/// <typeparam name="TResult">The type returned in the <see cref="Task{T}"/>.</typeparam>
public class GenericAsyncFunctionAssertions<TResult>
    : AsyncFunctionAssertions<Task<TResult>, GenericAsyncFunctionAssertions<TResult>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericAsyncFunctionAssertions{TResult}"/> class.
    /// </summary>
    public GenericAsyncFunctionAssertions(Func<Task<TResult>> subject, IExtractExceptions extractor)
        : this(subject, extractor, new Clock())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericAsyncFunctionAssertions{TResult}"/> class with custom <see cref="IClock"/>.
    /// </summary>
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
    public async Task<AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>> CompleteWithinAsync(
        TimeSpan timeSpan, string because = "", params object[] becauseArgs)
    {
        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to complete within {0}{reason}, but found <null>.", timeSpan);

        if (success)
        {
            (Task<TResult> task, TimeSpan remainingTime) = InvokeWithTimer(timeSpan);

            success = Execute.Assertion
                .ForCondition(remainingTime >= TimeSpan.Zero)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);

            if (success)
            {
                bool completesWithinTimeout = await CompletesWithinTimeoutAsync(task, remainingTime);

                success = Execute.Assertion
                    .ForCondition(completesWithinTimeout)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);
            }

#pragma warning disable CA1849 // Call async methods when in an async method
            TResult result = success ? task.Result : default;
#pragma warning restore CA1849 // Call async methods when in an async method
            return new AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>(this, result);
        }

        return new AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>(this, default(TResult));
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
    public async Task<AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>> NotThrowAsync(
        string because = "", params object[] becauseArgs)
    {
        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw{reason}, but found <null>.");

        if (success)
        {
            try
            {
                TResult result = await Subject!.Invoke();
                return new AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>(this, result);
            }
            catch (Exception exception)
            {
                _ = NotThrowInternal(exception, because, becauseArgs);
            }
        }

        return new AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>(this, default(TResult));
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
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="waitTime"/> or <paramref name="pollInterval"/> are negative.</exception>
    public Task<AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>> NotThrowAfterAsync(
        TimeSpan waitTime, TimeSpan pollInterval, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNegative(waitTime);
        Guard.ThrowIfArgumentIsNegative(pollInterval);

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw any exceptions after {0}{reason}, but found <null>.", waitTime);

        if (success)
        {
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

        return Task.FromResult(new AndWhichConstraint<GenericAsyncFunctionAssertions<TResult>, TResult>(this, default(TResult)));
    }
}
