using System;
using System.Diagnostics;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Specialized;

/// <summary>
/// Contains a number of methods to assert that a synchronous function yields the expected result.
/// </summary>
[DebuggerNonUserCode]
public class FunctionAssertions<T> : DelegateAssertions<Func<T>, FunctionAssertions<T>>
{
    public FunctionAssertions(Func<T> subject, IExtractExceptions extractor)
        : base(subject, extractor)
    {
    }

    public FunctionAssertions(Func<T> subject, IExtractExceptions extractor, IClock clock)
        : base(subject, extractor, clock)
    {
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
        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw{reason}, but found <null>.");

        T result = default;

        if (success)
        {
            result = FunctionAssertionHelpers.NotThrow(Subject, because, becauseArgs);
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
        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw any exceptions after {0}{reason}, but found <null>.", waitTime);

        T result = default;

        if (success)
        {
            result = FunctionAssertionHelpers.NotThrowAfter(Subject, Clock, waitTime, pollInterval, because, becauseArgs);
        }

        return new AndWhichConstraint<FunctionAssertions<T>, T>(this, result);
    }
}
