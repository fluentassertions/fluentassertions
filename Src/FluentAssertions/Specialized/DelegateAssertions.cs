using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized;

/// <summary>
/// Contains a number of methods to assert that a synchronous method yields the expected result.
/// </summary>
[DebuggerNonUserCode]
public abstract class DelegateAssertions<TDelegate, TAssertions> : DelegateAssertionsBase<TDelegate, TAssertions>
    where TDelegate : Delegate
    where TAssertions : DelegateAssertions<TDelegate, TAssertions>
{
    protected DelegateAssertions(TDelegate @delegate, IExtractExceptions extractor)
        : base(@delegate, extractor, new Clock())
    {
    }

    private protected DelegateAssertions(TDelegate @delegate, IExtractExceptions extractor, IClock clock)
        : base(@delegate, extractor, clock)
    {
    }

    /// <summary>
    /// Asserts that the current <see cref="Delegate" /> throws an exception of type <typeparamref name="TException"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public ExceptionAssertions<TException> Throw<TException>(string because = "", params object[] becauseArgs)
        where TException : Exception
    {
        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to throw {0}{reason}, but found <null>.", typeof(TException));

        if (success)
        {
            FailIfSubjectIsAsyncVoid();
            Exception exception = InvokeSubjectWithInterception();
            return ThrowInternal<TException>(exception, because, becauseArgs);
        }

        return new ExceptionAssertions<TException>(Array.Empty<TException>());
    }

    /// <summary>
    /// Asserts that the current <see cref="Delegate" /> does not throw an exception of type <typeparamref name="TException"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotThrow<TException>(string because = "", params object[] becauseArgs)
        where TException : Exception
    {
        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw {0}{reason}, but found <null>.", typeof(TException));

        if (success)
        {
            FailIfSubjectIsAsyncVoid();
            Exception exception = InvokeSubjectWithInterception();
            return NotThrowInternal<TException>(exception, because, becauseArgs);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    public AndConstraint<TAssertions> NotThrow(string because = "", params object[] becauseArgs)
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

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Delegate"/> throws an exception of the exact type <typeparamref name="TException"/> (and not a derived exception type).
    /// </summary>
    /// <typeparam name="TException">
    /// The type of the exception it should throw.
    /// </typeparam>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <returns>
    /// Returns an object that allows asserting additional members of the thrown exception.
    /// </returns>
    public ExceptionAssertions<TException> ThrowExactly<TException>(string because = "",
        params object[] becauseArgs)
        where TException : Exception
    {
        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to throw exactly {0}{reason}, but found <null>.", typeof(TException));

        if (success)
        {
            FailIfSubjectIsAsyncVoid();
            Exception exception = InvokeSubjectWithInterception();

            Type expectedType = typeof(TException);

            success = Execute.Assertion
                .ForCondition(exception is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0}{reason}, but no exception was thrown.", expectedType);

            if (success)
            {
                exception.Should().BeOfType(expectedType, because, becauseArgs);
            }

            return new ExceptionAssertions<TException>(new[] { exception as TException });
        }

        return new ExceptionAssertions<TException>(Array.Empty<TException>());
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
    public AndConstraint<TAssertions> NotThrowAfter(TimeSpan waitTime, TimeSpan pollInterval, string because = "",
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

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    protected abstract void InvokeSubject();

    private Exception InvokeSubjectWithInterception()
    {
        Exception actualException = null;

        try
        {
            // For the duration of this nested invocation, configure CallerIdentifier
            // to match the contents of the subject rather than our own call site.
            //
            //   Action action = () => subject.Should().BeSomething();
            //   action.Should().Throw<Exception>();
            //
            // If an assertion failure occurs, we want the message to talk about "subject"
            // not "action".
            using (CallerIdentifier.OnlyOneFluentAssertionScopeOnCallStack()
                       ? CallerIdentifier.OverrideStackSearchUsingCurrentScope()
                       : default)
            {
                InvokeSubject();
            }
        }
        catch (Exception exc)
        {
            actualException = exc;
        }

        return actualException;
    }

    private void FailIfSubjectIsAsyncVoid()
    {
        if (Subject.GetMethodInfo().IsDecoratedWithOrInherit<AsyncStateMachineAttribute>())
        {
            throw new InvalidOperationException(
                "Cannot use action assertions on an async void method. Assign the async method to a variable of type Func<Task> instead of Action so that it can be awaited.");
        }
    }
}
