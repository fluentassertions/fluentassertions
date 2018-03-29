using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Execution
{
    internal static class GivenSelectorExtensions
    {
        public static ContinuationOfGiven<IEnumerable<T>> AssertCollectionIsNotNull<T>(
            this GivenSelector<IEnumerable<T>> givenSelector)
        {
            return givenSelector
                .ForCondition(items => !ReferenceEquals(items, null))
                .FailWith("but found collection is <null>.");
        }

        public static ContinuationOfGiven<ICollection<T>> AssertEitherCollectionIsNotEmpty<T>(
            this GivenSelector<ICollection<T>> givenSelector, int length)
        {
            return givenSelector
                .ForCondition(items => (items.Any() || (length == 0)))
                .FailWith("but found empty collection.")
                .Then
                .ForCondition(items => (!items.Any() || (length > 0)))
                .FailWith("but found {0}.", items => items);
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
                .FailWith("but {0} contains {1} item(s) less.", items => items, items => length - items.Count);
        }

        public static ContinuationOfGiven<ICollection<T>> AssertCollectionHasNotTooManyItems<T>(this GivenSelector<ICollection<T>> givenSelector,
            int length)
        {
            return givenSelector
                .ForCondition(items => items.Count <= length)
                .FailWith("but {0} contains {1} item(s) too many.", items => items, items => items.Count - length);
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
                .FailWith("but {0} differs at index {1}.", diff => diff.Items, diff => diff.Index);
        }
    }
}
