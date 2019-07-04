using System;
using System.Collections.Generic;
using FluentAssertions.Execution;
using FluentAssertions.Localization;

namespace FluentAssertions.Equivalency
{
    internal static class EnumerableEquivalencyValidatorExtensions
    {
        public static Continuation AssertEitherCollectionIsNotEmpty<T>(this IAssertionScope scope, ICollection<object> subject, ICollection<T> expectation)
        {
            return scope
                .ForCondition((subject.Count > 0) || (expectation.Count == 0))
                .FailWith(Resources.Collection_CommaButFoundEmptyCollection)
                .Then
                .ForCondition((subject.Count == 0) || (expectation.Count > 0))
                .FailWith(Resources.Collection_CommaButX0X2ContainsX1ItemsFormat,
                    subject,
                    subject.Count,
                    Environment.NewLine);
        }

        public static Continuation AssertCollectionHasEnoughItems<T>(this IAssertionScope scope, ICollection<object> subject, ICollection<T> expectation)
        {
            return scope
                .ForCondition(subject.Count >= expectation.Count)
                .FailWith(Resources.Collection_CommaButX0X3ContainsX1ItemsLessThanWX2Format,
                    subject,
                    expectation.Count - subject.Count,
                    expectation,
                    Environment.NewLine);
        }

        public static Continuation AssertCollectionHasNotTooManyItems<T>(this IAssertionScope scope, ICollection<object> subject, ICollection<T> expectation)
        {
            return scope
                .ForCondition(subject.Count <= expectation.Count)
                .FailWith(Resources.Collection_CommaButX0X3ContainsX1ItemsMoreThanWX2Format,
                    subject,
                    subject.Count - expectation.Count,
                    expectation,
                    Environment.NewLine);
        }
    }
}
