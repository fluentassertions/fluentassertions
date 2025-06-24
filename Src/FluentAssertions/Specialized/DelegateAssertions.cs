using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
    private readonly AssertionChain assertionChain;

    protected DelegateAssertions(TDelegate @delegate, IExtractExceptions extractor, AssertionChain assertionChain)
        : base(@delegate, extractor, assertionChain, new Clock())
    {
        this.assertionChain = assertionChain;
    }

    private protected DelegateAssertions(TDelegate @delegate, IExtractExceptions extractor, AssertionChain assertionChain, IClock clock)
        : base(@delegate, extractor, assertionChain, clock)
    {
        this.assertionChain = assertionChain;
    }

    /// <summary>
    /// Asserts that the current <see cref="Delegate" /> throws any exception.
    /// </summary>
    /// <param name="because">
    /// (Optional)
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion is needed. If
    /// the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public ExceptionAssertions<Exception> Throw([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return Throw<Exception>(because, becauseArgs);
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
    public ExceptionAssertions<TException> Throw<TException>([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TException : Exception
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to throw {0}{reason}, but found <null>.", typeof(TException));

        if (assertionChain.Succeeded)
        {
            FailIfSubjectIsAsyncVoid();
            Exception exception = InvokeSubjectWithInterception();
            return ThrowInternal<TException>(exception, because, becauseArgs);
        }

        return new ExceptionAssertions<TException>([], assertionChain);
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
    public AndConstraint<TAssertions> NotThrow<TException>([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        where TException : Exception
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw {0}{reason}, but found <null>.", typeof(TException));

        if (assertionChain.Succeeded)
        {
            FailIfSubjectIsAsyncVoid();
            Exception exception = InvokeSubjectWithInterception();
            return NotThrowInternal<TException>(exception, because, becauseArgs);
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
    public ExceptionAssertions<TException> ThrowExactly<TException>([StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
        where TException : Exception
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to throw exactly {0}{reason}, but found <null>.", typeof(TException));

        if (assertionChain.Succeeded)
        {
            FailIfSubjectIsAsyncVoid();
            Exception exception = InvokeSubjectWithInterception();

            Type expectedType = typeof(TException);

            assertionChain
                .ForCondition(exception is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0}{reason}, but no exception was thrown.", expectedType);

            if (assertionChain.Succeeded)
            {
                exception.Should().BeOfType(expectedType, because, becauseArgs);
            }

            return new ExceptionAssertions<TException>([exception as TException], assertionChain);
        }

        return new ExceptionAssertions<TException>([], assertionChain);
    }

    protected abstract void InvokeSubject();

    private protected Exception InvokeSubjectWithInterception()
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

    private protected void FailIfSubjectIsAsyncVoid()
    {
        if (Subject.GetMethodInfo().IsDecoratedWithOrInherit<AsyncStateMachineAttribute>())
        {
            throw new InvalidOperationException(
                "Cannot use action assertions on an async void method. Assign the async method to a variable of type Func<Task> instead of Action so that it can be awaited.");
        }
    }
}
