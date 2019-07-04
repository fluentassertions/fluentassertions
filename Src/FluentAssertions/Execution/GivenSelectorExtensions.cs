using System;
using System.Collections.Generic;
using FluentAssertions.Common;
using FluentAssertions.Localization;

namespace FluentAssertions.Execution
{
    internal static class GivenSelectorExtensions
    {
        public static ContinuationOfGiven<IEnumerable<T>> AssertCollectionIsNotNull<T>(
            this GivenSelector<IEnumerable<T>> givenSelector)
        {
            return givenSelector
                .ForCondition(items => !(items is null))
                .FailWith(Resources.Collection_ButFoundCollectionIsNull);
        }

        public static ContinuationOfGiven<ICollection<T>> AssertEitherCollectionIsNotEmpty<T>(
            this GivenSelector<ICollection<T>> givenSelector, int length)
        {
            return givenSelector
                .ForCondition(items => ((items.Count > 0) || (length == 0)))
                .FailWith(Resources.Collection_ButFoundEmptyCollection)
                .Then
                .ForCondition(items => ((items.Count == 0) || (length > 0)))
                .FailWith(Resources.Common_ButFoundX0Format, items => items);
        }

        public static ContinuationOfGiven<ICollection<T>> AssertCollectionHasEnoughItems<T>(this GivenSelector<IEnumerable<T>> givenSelector,
            int length)
        {
            return givenSelector
                .Given(items => items.ConvertOrCastToCollection())
                .AssertCollectionHasEnoughItems(length);
        }

        public static ContinuationOfGiven<ICollection<T>> AssertCollectionHasEnoughItems<T>(this GivenSelector<ICollection<T>> givenSelector, int length)
        {
            return givenSelector
                .ForCondition(items => items.Count >= length)
                .FailWith(Resources.Collection_ButX0ContainsX1ItemsLessFormat, items => items, items => length - items.Count);
        }

        public static ContinuationOfGiven<ICollection<T>> AssertCollectionHasNotTooManyItems<T>(this GivenSelector<ICollection<T>> givenSelector,
            int length)
        {
            return givenSelector
                .ForCondition(items => items.Count <= length)
                .FailWith(Resources.Collection_ButX0ContainsX1ItemsTooManyFormat, items => items, items => items.Count - length);
        }

        public static ContinuationOfGiven<ICollection<T>> AssertCollectionsHaveSameCount<T>(this GivenSelector<ICollection<T>> givenSelector,
            int length)
        {
            return givenSelector
                .AssertEitherCollectionIsNotEmpty(length)
                .Then
                .AssertCollectionHasEnoughItems(length)
                .Then
                .AssertCollectionHasNotTooManyItems(length);
        }

        public static void AssertCollectionsHaveSameItems<TActual, TExpected>(this GivenSelector<ICollection<TActual>> givenSelector,
            ICollection<TExpected> expected, Func<ICollection<TActual>, ICollection<TExpected>, int> findIndex)
        {
            givenSelector
                .Given(actual => new { Items = actual, Index = findIndex(actual, expected) })
                .ForCondition(diff => diff.Index == -1)
                .FailWith(Resources.Collection_ButX0DiffersAtIndexX1Format, diff => diff.Items, diff => diff.Index);
        }
    }
}
