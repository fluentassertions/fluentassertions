using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Specialized;

/// <summary>
/// Contains a number of methods to assert that a method yields the expected result.
/// </summary>
[DebuggerNonUserCode]
public abstract class DelegateAssertionsBase<TDelegate, TAssertions>
    : ReferenceTypeAssertions<TDelegate, DelegateAssertionsBase<TDelegate, TAssertions>>
    where TDelegate : Delegate
    where TAssertions : DelegateAssertionsBase<TDelegate, TAssertions>
{
    private readonly Assertion assertion;

    private protected IExtractExceptions Extractor { get; }

    private protected DelegateAssertionsBase(TDelegate @delegate, IExtractExceptions extractor, Assertion assertion,
        IClock clock)
        : base(@delegate, assertion)
    {
        this.assertion = assertion;
        Extractor = extractor ?? throw new ArgumentNullException(nameof(extractor));
        Clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    private protected IClock Clock { get; }

    protected ExceptionAssertions<TException> ThrowInternal<TException>(
        Exception exception, string because, object[] becauseArgs)
        where TException : Exception
    {
        TException[] expectedExceptions = Extractor.OfType<TException>(exception).ToArray();

        assertion
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected a <{0}> to be thrown{reason}, ", typeof(TException))
            .ForCondition(exception is not null)
            .FailWith("but no exception was thrown.")
            .Then
            .ForCondition(expectedExceptions.Length > 0)
            .FailWith("but found <{0}>:" + Environment.NewLine + "{1}.",
                exception?.GetType(),
                exception);

        return new ExceptionAssertions<TException>(expectedExceptions, assertion);
    }

    protected AndConstraint<TAssertions> NotThrowInternal(Exception exception, string because, object[] becauseArgs)
    {
        assertion
            .ForCondition(exception is null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect any exception{reason}, but found {0}.", exception);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    protected AndConstraint<TAssertions> NotThrowInternal<TException>(Exception exception, string because, object[] becauseArgs)
        where TException : Exception
    {
        IEnumerable<TException> exceptions = Extractor.OfType<TException>(exception);

        assertion
            .ForCondition(!exceptions.Any())
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {0}{reason}, but found {1}.", typeof(TException), exception);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }
}
