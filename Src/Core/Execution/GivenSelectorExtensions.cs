using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Execution
{
    internal static class GivenSelectorExtensions
    {
        public static ContinuationOfGiven<IEnumerable<T>> AssertCollectionIsNotNull<T>(this GivenSelector<IEnumerable<T>> givenSelector)
        {
            return givenSelector
                .ForCondition(items => !ReferenceEquals(items, null))
                .FailWith("but found collection is <null>.");
        }

        public static ContinuationOfGiven<IEnumerable<T>> AssertCollectionIsNotEmpty<T>(this GivenSelector<IEnumerable<T>> givenSelector)
        {
            return givenSelector
               .ForCondition(items => items.Any())
               .FailWith("but found empty collection.");
        }

        public static ContinuationOfGiven<IEnumerable<T>> AssertCollectionIsNotNullOrEmpty<T>(this GivenSelector<IEnumerable<T>> givenSelector)
        {
            return givenSelector
               .AssertCollectionIsNotNull()
               .Then
               .AssertCollectionIsNotEmpty();
        }

        public static ContinuationOfGiven<T[]> AssertCollectionHasEnoughItems<T>(this GivenSelector<IEnumerable<T>> givenSelector, int length)
        {
            return givenSelector
                .Given(items => items.ToArray())
                .AssertCollectionHasEnoughItems(length);
        }

        public static ContinuationOfGiven<T[]> AssertCollectionHasEnoughItems<T>(this GivenSelector<T[]> givenSelector, int length)
        {
            return givenSelector
                .Given(items => items.ToArray())
                .ForCondition(items => items.Length >= length)
                .FailWith("but {0} contains {1} item(s) less.", items => items, items => length - items.Length);
        }

        public static ContinuationOfGiven<T[]> AssertCollectionHasNotTooManyItems<T>(this GivenSelector<IEnumerable<T>> givenSelector, int length)
        {
            return givenSelector
                .Given(items => items.ToArray())
                .AssertCollectionHasNotTooManyItems(length);
        }

        public static ContinuationOfGiven<T[]> AssertCollectionHasNotTooManyItems<T>(this GivenSelector<T[]> givenSelector, int length)
        {
            return givenSelector
                .Given(items => items.ToArray())
                .ForCondition(items => items.Length <= length)
                .FailWith("but {0} contains {1} item(s) too many.", items => items, items => items.Length - length);
        }

        public static ContinuationOfGiven<T[]> AssertCollectionsHaveSameCount<T>(this GivenSelector<IEnumerable<T>> givenSelector, int length)
        {
            return givenSelector
               .AssertCollectionIsNotEmpty()
               .Then
               .AssertCollectionHasEnoughItems(length)
               .Then
               .AssertCollectionHasNotTooManyItems(length);
        }

        public static void AssertCollectionsHaveSameItems<TActual, TExpected>(this GivenSelector<TActual[]> givenSelector, TExpected[] expected, Func<TActual[], TExpected[], int> findIndex)
        {
            givenSelector
                .Given(actual => new { Items = actual, Index = findIndex(actual, expected) })
                .ForCondition(diff => diff.Index == -1)
                .FailWith("but {0} differs at index {1}.", diff => diff.Items, diff => diff.Index);
        }
    }
}
