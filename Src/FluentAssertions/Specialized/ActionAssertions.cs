using System;
using System.Diagnostics;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Specialized;

/// <summary>
/// Contains a number of methods to assert that an <see cref="Action"/> yields the expected result.
/// </summary>
[DebuggerNonUserCode]
public class ActionAssertions : DelegateAssertions<Action, ActionAssertions>
{
    public ActionAssertions(Action subject, IExtractExceptions extractor)
        : base(subject, extractor)
    {
    }

    public ActionAssertions(Action subject, IExtractExceptions extractor, IClock clock)
        : base(subject, extractor, clock)
    {
    }

    /// <summary>
    /// Asserts that the current <see cref="Delegate" /> does not throw any exception.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<ActionAssertions> NotThrow(string because = "", params object[] becauseArgs)
    {
        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw{reason}, but found <null>.");

        if (success)
        {
            FailIfSubjectIsAsyncVoid();
            Exception exception = InvokeSubjectWithInterception();
            return NotThrowInternal(exception, because, becauseArgs);
        }

        return new AndConstraint<ActionAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Delegate"/> stops throwing any exception
    /// after a specified amount of time.
    /// </summary>
    /// <remarks>
    /// The delegate is invoked. If it raises an exception,
    /// the invocation is repeated until it either stops raising any exceptions
    /// or the specified wait time is exceeded.
    /// </remarks>
    /// <param name="waitTime">
    /// The time after which the delegate should have stopped throwing any exception.
    /// </param>
    /// <param name="pollInterval">
    /// The time between subsequent invocations of the delegate.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="waitTime"/> or <paramref name="pollInterval"/> are negative.</exception>
    public AndConstraint<ActionAssertions> NotThrowAfter(TimeSpan waitTime, TimeSpan pollInterval, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNegative(waitTime);
        Guard.ThrowIfArgumentIsNegative(pollInterval);

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw after {0}{reason}, but found <null>.", waitTime);

        if (success)
        {
            FailIfSubjectIsAsyncVoid();

            TimeSpan? invocationEndTime = null;
            Exception exception = null;
            ITimer timer = Clock.StartTimer();

            while (invocationEndTime is null || invocationEndTime < waitTime)
            {
                exception = InvokeSubjectWithInterception();

                if (exception is null)
                {
                    break;
                }

                Clock.Delay(pollInterval);
                invocationEndTime = timer.Elapsed;
            }

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(exception is null)
                .FailWith("Did not expect any exceptions after {0}{reason}, but found {1}.", waitTime, exception);
        }

        return new AndConstraint<ActionAssertions>(this);
    }

    protected override void InvokeSubject()
    {
        Subject();
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "action";
}
