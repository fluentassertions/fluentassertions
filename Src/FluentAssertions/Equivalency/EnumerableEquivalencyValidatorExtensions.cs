using System;
using System.Collections.Generic;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    internal static class EnumerableEquivalencyValidatorExtensions
    {
        public static Continuation AssertEitherCollectionIsNotEmpty<T>(this IAssertionScope scope, ICollection<object> subject, ICollection<T> expectation)
        {
            return scope
                .ForCondition((subject.Count > 0) || (expectation.Count == 0))
                .FailWith(", but found an empty collection.")
                .Then
                .ForCondition((subject.Count == 0) || (expectation.Count > 0))
                .FailWith(", but {0}{2}contains {1} item(s).",
                    subject,
                    subject.Count,
                    Environment.NewLine);
        }

        public static Continuation AssertCollectionHasEnoughItems<T>(this IAssertionScope scope, ICollection<object> subject, ICollection<T> expectation)
        {
            return scope
                .ForCondition(subject.Count >= expectation.Count)
                .FailWith(", but {0}{3}contains {1} item(s) less than{3}{2}.",
                    subject,
                    expectation.Count - subject.Count,
                    expectation,
                    Environment.NewLine);
        }

        public static Continuation AssertCollectionHasNotTooManyItems<T>(this IAssertionScope scope, ICollection<object> subject, ICollection<T> expectation)
        {
            return scope
                .ForCondition(subject.Count <= expectation.Count)
                .FailWith(", but {0}{3}contains {1} item(s) more than{3}{2}.",
                    subject,
                    subject.Count - expectation.Count,
                    expectation,
                    Environment.NewLine);
        }
    }
}
