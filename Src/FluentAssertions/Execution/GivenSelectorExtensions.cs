using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using FluentAssertions.Common;

namespace FluentAssertions.Execution
{
    internal static class GivenSelectorExtensions
    {
        public static ContinuationOfGiven<IEnumerable<T>> AssertCollectionIsNotNull<T>(
            this GivenSelector<IEnumerable<T>> givenSelector)
        {
            return givenSelector
                .ForCondition(items => !(items is null))
                .FailWith("but found collection is <null>.");
        }

        public static ContinuationOfGiven<ICollection<T>> AssertEitherCollectionIsNotEmpty<T>(
            this GivenSelector<ICollection<T>> givenSelector, int length)
        {
            return givenSelector
                .ForCondition(items => ((items.Count > 0) || (length == 0)))
                .FailWith("but found empty collection.")
                .Then
                .ForCondition(items => ((items.Count == 0) || (length > 0)))
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

        public static ContinuationOfGiven<IEnumerable> AssertForEachEnumerablesHaveSameItems(
            this GivenSelector<object> givenSelector, IEnumerable expected, Func<object, IEnumerable, int> findIndex)
            => givenSelector
                .Given<IEnumerable>(actual => new ForEachEnumerableWithIndex(actual, findIndex(actual, expected)))
                .ForCondition(diff => diff.As<ForEachEnumerableWithIndex>().Index < 0)
                .FailWith("but {0} differs at index {1} when using 'foreach'.",
                    diff => diff,
                    diff => diff.As<ForEachEnumerableWithIndex>().Index);

        public static ContinuationOfGiven<ICollection<TActual>> AssertCollectionsHaveSameItems<TActual, TExpected>(this GivenSelector<ICollection<TActual>> givenSelector,
            ICollection<TExpected> expected, Func<ICollection<TActual>, ICollection<TExpected>, int> findIndex)
        {
            return givenSelector
                .Given<ICollection<TActual>>(actual => new CollectionWithIndex<TActual>(actual, findIndex(actual, expected)))
                .ForCondition(diff => diff.As<CollectionWithIndex<TActual>>().Index == -1)
                .FailWith("but {0} differs at index {1}.",
                    diff => diff.As<CollectionWithIndex<TActual>>().Items,
                    diff => diff.As<CollectionWithIndex<TActual>>().Index);
        }

        sealed class ForEachEnumerableWithIndex : IEnumerable
        {
            readonly object items;
            MethodInfo methodGetEnumerator;

            public int Index { get; }

            public ForEachEnumerableWithIndex(object items, int index)
            {
                this.items = items;
                Index = index;
            }

            public IEnumerator GetEnumerator() => new Enumerator(this);

            class Enumerator : IEnumerator, IDisposable
            {
                readonly object enumerator;
                readonly PropertyInfo propertyCurrent;
                readonly MethodInfo methodMoveNext;
                MethodInfo methodReset;
                MethodInfo methodDispose;
                bool methodResetInitialized;
                bool methodDisposeInitialized;
                object methodResetSync;
                object methodDisposeSync;

                public Enumerator(ForEachEnumerableWithIndex enumerable)
                {
                    LazyInitializer.EnsureInitialized(ref enumerable.methodGetEnumerator,
                        () => enumerable.items.GetType().GetPublicExplicitParameterlessMethod("GetEnumerator"));
                    enumerator = enumerable.methodGetEnumerator.Invoke(enumerable.items, new object[0]);

                    var enumeratorType = enumerator.GetType();
                    propertyCurrent = enumeratorType.GetPublicExplicitProperty("Current");
                    methodMoveNext = enumeratorType.GetPublicExplicitParameterlessMethod("MoveNext");
                }

                public object Current => propertyCurrent.GetValue(enumerator);

                public bool MoveNext() => (bool)methodMoveNext.Invoke(enumerator, new object[0]);

                public void Reset()
                {
                    LazyInitializer.EnsureInitialized(ref methodReset, ref methodResetInitialized, ref methodResetSync,
                        () => enumerator.GetType().GetPublicExplicitParameterlessMethod("Reset"));
                    methodReset?.Invoke(enumerator, new object[0]);
                }

                public void Dispose()
                {
                    LazyInitializer.EnsureInitialized(ref methodDispose, ref methodDisposeInitialized, ref methodDisposeSync,
                        () => enumerator.GetType().GetPublicExplicitParameterlessMethod("Dispose"));
                    methodDispose?.Invoke(enumerator, new object[0]);
                }
            }
        }

        private sealed class CollectionWithIndex<T> : ICollection<T>
        {
            public ICollection<T> Items { get; }

            public int Index { get; }

            public CollectionWithIndex(ICollection<T> items, int index)
            {
                Items = items;
                Index = index;
            }

            public int Count => Items.Count;

            public bool IsReadOnly => Items.IsReadOnly;

            public void Add(T item) => Items.Add(item);

            public void Clear() => Items.Clear();

            public bool Contains(T item) => Items.Contains(item);

            public void CopyTo(T[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

            public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

            public bool Remove(T item) => Items.Remove(item);

            IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
        }
    }
}
