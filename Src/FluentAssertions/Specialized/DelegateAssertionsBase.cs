using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
    private readonly AssertionChain assertionChain;

    private protected IExtractExceptions Extractor { get; }

    private protected DelegateAssertionsBase(TDelegate @delegate, IExtractExceptions extractor, AssertionChain assertionChain,
        IClock clock)
        : base(@delegate, assertionChain)
    {
        Guard.ThrowIfArgumentIsNull(extractor);
        Guard.ThrowIfArgumentIsNull(clock);
        this.assertionChain = assertionChain;
        Extractor = extractor;
        Clock = clock;
    }

    private protected IClock Clock { get; }

    protected ExceptionAssertions<TException> ThrowInternal<TException>(
        Exception exception,
        [StringSyntax("CompositeFormat")] string because, object[] becauseArgs)
        where TException : Exception
    {
        TException[] expectedExceptions = Extractor.OfType<TException>(exception).ToArray();

        assertionChain
            .BecauseOf(because, becauseArgs)
            .WithExpectation("Expected a <{0}> to be thrown{reason}, ", typeof(TException), chain => chain
                .ForCondition(exception is not null)
                .FailWith("but no exception was thrown.")
                .Then
                .ForCondition(expectedExceptions.Length > 0)
                .FailWith("but found <{0}>:" + Environment.NewLine + "{1}.",
                    exception?.GetType(),
                    exception));

        return new ExceptionAssertions<TException>(expectedExceptions, assertionChain);
    }

    protected AndConstraint<TAssertions> NotThrowInternal(Exception exception, [StringSyntax("CompositeFormat")] string because,
        object[] becauseArgs)
    {
        assertionChain
            .ForCondition(exception is null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect any exception{reason}, but found {0}.", exception);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    protected AndConstraint<TAssertions> NotThrowInternal<TException>(Exception exception,
        [StringSyntax("CompositeFormat")] string because, object[] becauseArgs)
        where TException : Exception
    {
        IEnumerable<TException> exceptions = Extractor.OfType<TException>(exception);

        assertionChain
            .ForCondition(!exceptions.Any())
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {0}{reason}, but found {1}.", typeof(TException), exception);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }
}
