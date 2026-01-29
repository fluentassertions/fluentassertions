using System;
using System.Collections.Generic;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

[System.Diagnostics.StackTraceHidden]
internal static class EnumerableEquivalencyValidatorExtensions
{
    public static Continuation AssertEitherCollectionIsNotEmpty<T>(this AssertionChain assertionChain,
        ICollection<object> subject,
        ICollection<T> expectation)
    {
        return assertionChain
            .WithExpectation("Expected {context:subject} to be a collection with {0} item(s){reason}", expectation.Count,
                chain => chain
                    .ForCondition(subject.Count > 0 || expectation.Count == 0)
                    .FailWith(", but found an empty collection.")
                    .Then
                    .ForCondition(subject.Count == 0 || expectation.Count > 0)
                    .FailWith($", but {{0}}{Environment.NewLine}contains {{1}} item(s).",
                        subject,
                        subject.Count));
    }

    public static Continuation AssertCollectionHasEnoughItems<T>(this AssertionChain assertionChain, ICollection<object> subject,
        ICollection<T> expectation)
    {
        return assertionChain
            .WithExpectation("Expected {context:subject} to be a collection with {0} item(s){reason}", expectation.Count,
                chain => chain
                    .ForCondition(subject.Count >= expectation.Count)
                    .FailWith($", but {{0}}{Environment.NewLine}contains {{1}} item(s) less than{Environment.NewLine}{{2}}.",
                        subject,
                        expectation.Count - subject.Count,
                        expectation));
    }

    public static Continuation AssertCollectionHasNotTooManyItems<T>(this AssertionChain assertionChain,
        ICollection<object> subject,
        ICollection<T> expectation)
    {
        return assertionChain
            .WithExpectation("Expected {context:subject} to be a collection with {0} item(s){reason}", expectation.Count,
                chain => chain
                    .ForCondition(subject.Count <= expectation.Count)
                    .FailWith($", but {{0}}{Environment.NewLine}contains {{1}} item(s) more than{Environment.NewLine}{{2}}.",
                        subject,
                        subject.Count - expectation.Count,
                        expectation));
    }
}

