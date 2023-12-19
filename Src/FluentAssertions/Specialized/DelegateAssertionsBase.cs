using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Primitives;

namespace FluentAssertionsAsync.Specialized;

/// <summary>
/// Contains a number of methods to assert that a method yields the expected result.
/// </summary>
[DebuggerNonUserCode]
public abstract class DelegateAssertionsBase<TDelegate, TAssertions>
    : ReferenceTypeAssertions<TDelegate, DelegateAssertionsBase<TDelegate, TAssertions>>
    where TDelegate : Delegate
    where TAssertions : DelegateAssertionsBase<TDelegate, TAssertions>
{
    private protected IExtractExceptions Extractor { get; }

    private protected DelegateAssertionsBase(TDelegate @delegate, IExtractExceptions extractor, IClock clock)
        : base(@delegate)
    {
        Extractor = extractor ?? throw new ArgumentNullException(nameof(extractor));
        Clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    private protected IClock Clock { get; }

    protected ExceptionAssertions<TException> ThrowInternal<TException>(
        Exception exception, string because, object[] becauseArgs)
        where TException : Exception
    {
        TException[] expectedExceptions = Extractor.OfType<TException>(exception).ToArray();

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected a <{0}> to be thrown{reason}, ", typeof(TException))
            .ForCondition(exception is not null)
            .FailWith("but no exception was thrown.")
            .Then
            .ForCondition(expectedExceptions.Length > 0)
            .FailWith("but found <{0}>:" + Environment.NewLine + "{1}.",
                exception?.GetType(),
                exception)
            .Then
            .ClearExpectation();

        return new ExceptionAssertions<TException>(expectedExceptions);
    }

    protected AndConstraint<TAssertions> NotThrowInternal(Exception exception, string because, object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(exception is null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect any exception{reason}, but found {0}.", exception);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    protected AndConstraint<TAssertions> NotThrowInternal<TException>(Exception exception, string because, object[] becauseArgs)
        where TException : Exception
    {
        IEnumerable<TException> exceptions = Extractor.OfType<TException>(exception);

        Execute.Assertion
            .ForCondition(!exceptions.Any())
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {0}{reason}, but found {1}.", typeof(TException), exception);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }
}
