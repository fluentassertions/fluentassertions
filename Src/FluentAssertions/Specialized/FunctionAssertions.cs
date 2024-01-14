using System;
using System.Diagnostics;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized;

/// <summary>
/// Contains a number of methods to assert that a synchronous function yields the expected result.
/// </summary>
[DebuggerNonUserCode]
public class FunctionAssertions<T> : DelegateAssertions<Func<T>, FunctionAssertions<T>>
{
    private readonly Assertion assertion;

    public FunctionAssertions(Func<T> subject, IExtractExceptions extractor, Assertion assertion)
        : base(subject, extractor, assertion)
    {
        this.assertion = assertion;
    }

    public FunctionAssertions(Func<T> subject, IExtractExceptions extractor, Assertion assertion, IClock clock)
        : base(subject, extractor, assertion, clock)
    {
        this.assertion = assertion;
    }

    protected override void InvokeSubject()
    {
        Subject();
    }

    protected override string Identifier => "function";

    /// <summary>
    /// Asserts that the current <see cref="Func{T}"/> does not throw any exception.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<FunctionAssertions<T>, T> NotThrow(string because = "", params object[] becauseArgs)
    {
        assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw{reason}, but found <null>.");

        T result = default;

        if (assertion.Succeeded)
        {
            try
            {
                result = Subject();
            }
            catch (Exception exception)
            {
                assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect any exception{reason}, but found {0}.", exception);

                result = default;
            }
        }

        return new AndWhichConstraint<FunctionAssertions<T>, T>(this, result);
    }

    /// <summary>
    /// Asserts that the current <see cref="Func{T}"/> stops throwing any exception
    /// after a specified amount of time.
    /// </summary>
    /// <remarks>
    /// The <see cref="Func{T}"/> is invoked. If it raises an exception,
    /// the invocation is repeated until it either stops raising any exceptions
    /// or the specified wait time is exceeded.
    /// </remarks>
    /// <param name="waitTime">
    /// The time after which the <see cref="Func{T}"/> should have stopped throwing any exception.
    /// </param>
    /// <param name="pollInterval">
    /// The time between subsequent invocations of the <see cref="Func{T}"/>.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="waitTime"/> or <paramref name="pollInterval"/> are negative.</exception>
    public AndWhichConstraint<FunctionAssertions<T>, T> NotThrowAfter(TimeSpan waitTime, TimeSpan pollInterval,
        string because = "", params object[] becauseArgs)
    {
        assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw any exceptions after {0}{reason}, but found <null>.", waitTime);

        T result = default;

        if (assertion.Succeeded)
        {
            result = NotThrowAfter(Subject, Clock, waitTime, pollInterval, because, becauseArgs);
        }

        return new AndWhichConstraint<FunctionAssertions<T>, T>(this, result);
    }

    internal TResult NotThrowAfter<TResult>(Func<TResult> subject, IClock clock, TimeSpan waitTime, TimeSpan pollInterval,
        string because, object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNegative(waitTime);
        Guard.ThrowIfArgumentIsNegative(pollInterval);

        TimeSpan? invocationEndTime = null;
        Exception exception = null;
        ITimer timer = clock.StartTimer();

        while (invocationEndTime is null || invocationEndTime < waitTime)
        {
            try
            {
                return subject();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            clock.Delay(pollInterval);
            invocationEndTime = timer.Elapsed;
        }

        assertion
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect any exceptions after {0}{reason}, but found {1}.", waitTime, exception);

        return default;
    }
}
