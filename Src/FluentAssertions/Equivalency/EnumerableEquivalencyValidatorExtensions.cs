using System;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    internal static class EnumerableEquivalencyValidatorExtensions
    {
        public static Continuation AssertCollectionsHaveSameCount<T>(object[] subject, T[] expectation)
        {
            return AssertionScope.Current
                .WithExpectation("Expected {context:subject} to be a collection with {0} item(s){reason}", expectation.Length)
                .AssertEitherCollectionIsNotEmpty(subject, expectation)
                .Then
                .AssertCollectionHasEnoughItems(subject, expectation)
                .Then
                .AssertCollectionHasNotTooManyItems(subject, expectation);
        }

        public static Continuation AssertEitherCollectionIsNotEmpty<T>(this AssertionScope scope, object[] subject, T[] expectation)
        {
            return scope
                .ForCondition((subject.Length > 0) || (expectation.Length == 0))
                .FailWith(", but found an empty collection.")
                .Then
                .ForCondition((subject.Length == 0) || (expectation.Length > 0))
                .FailWith(", but {0}{2}contains {1} item(s).",
                    subject,
                    subject.Length,
                    Environment.NewLine);
        }

        public static Continuation AssertCollectionHasEnoughItems<T>(this AssertionScope scope, object[] subject, T[] expectation)
        {
            return scope
                .ForCondition(subject.Length >= expectation.Length)
                .FailWith(", but {0}{3}contains {1} item(s) less than{3}{2}.",
                    subject,
                    expectation.Length - subject.Length,
                    expectation,
                    Environment.NewLine);
        }

        public static Continuation AssertCollectionHasNotTooManyItems<T>(this AssertionScope scope, object[] subject, T[] expectation)
        {
            return scope
                .ForCondition(subject.Length <= expectation.Length)
                .FailWith(", but {0}{3}contains {1} item(s) more than{3}{2}.",
                    subject,
                    subject.Length - expectation.Length,
                    expectation,
                    Environment.NewLine);
        }
    }
}
