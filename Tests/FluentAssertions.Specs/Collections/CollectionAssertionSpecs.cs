using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <summary>
/// Collection assertion specs.
/// </summary>
public partial class CollectionAssertionSpecs
{
    public class Chainings
    {
        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should()
                .HaveCount(3)
                .And
                .HaveElementAt(1, 2)
                .And
                .NotContain(4);
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_subsequent_ascending_assertion_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (1, "a"),
                (2, "b"),
                (2, "c"),
                (3, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInAscendingOrder(x => x.Item1)
                .And
                .ThenBeInAscendingOrder(x => x.Item2);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_not_ordered_according_to_the_subsequent_ascending_assertion_it_should_fail()
        {
            // Arrange
            var collection = new[]
            {
                (1, "a"),
                (2, "b"),
                (2, "c"),
                (3, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInAscendingOrder(x => x.Item1)
                .And
                .BeInAscendingOrder(x => x.Item2);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection * to be ordered \"by Item2\"*");
        }

        [Fact]
        public void
            When_the_collection_is_ordered_according_to_the_subsequent_ascending_assertion_with_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (1, "a"),
                (2, "B"),
                (2, "b"),
                (3, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInAscendingOrder(x => x.Item1)
                .And
                .ThenBeInAscendingOrder(x => x.Item2, StringComparer.InvariantCultureIgnoreCase);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_multiple_subsequent_ascending_assertions_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (1, "a", 1.1),
                (2, "b", 1.2),
                (2, "c", 1.3),
                (3, "a", 1.1)
            };

            // Act
            Action action = () => collection.Should()
                .BeInAscendingOrder(x => x.Item1)
                .And
                .ThenBeInAscendingOrder(x => x.Item2)
                .And
                .ThenBeInAscendingOrder(x => x.Item3);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_subsequent_descending_assertion_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (3, "a"),
                (2, "c"),
                (2, "b"),
                (1, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInDescendingOrder(x => x.Item1)
                .And
                .ThenBeInDescendingOrder(x => x.Item2);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_not_ordered_according_to_the_subsequent_descending_assertion_it_should_fail()
        {
            // Arrange
            var collection = new[]
            {
                (3, "a"),
                (2, "c"),
                (2, "b"),
                (1, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInDescendingOrder(x => x.Item1)
                .And
                .BeInDescendingOrder(x => x.Item2);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection * to be ordered \"by Item2\"*");
        }

        [Fact]
        public void
            When_the_collection_is_ordered_according_to_the_subsequent_descending_assertion_with_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (3, "a"),
                (2, "b"),
                (2, "B"),
                (1, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInDescendingOrder(x => x.Item1)
                .And
                .ThenBeInDescendingOrder(x => x.Item2, StringComparer.InvariantCultureIgnoreCase);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_multiple_subsequent_descending_assertions_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (3, "a", 1.1),
                (2, "c", 1.3),
                (2, "b", 1.2),
                (1, "a", 1.1)
            };

            // Act
            Action action = () => collection.Should()
                .BeInDescendingOrder(x => x.Item1)
                .And
                .ThenBeInDescendingOrder(x => x.Item2)
                .And
                .ThenBeInDescendingOrder(x => x.Item3);

            // Assert
            action.Should().NotThrow();
        }
    }

    private class ByLastCharacterComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return x.Last().CompareTo(y.Last());
        }
    }
}

internal class CountingGenericEnumerable<TElement> : IEnumerable<TElement>
{
    private readonly IEnumerable<TElement> backingSet;

    public CountingGenericEnumerable(IEnumerable<TElement> backingSet)
    {
        this.backingSet = backingSet;
        GetEnumeratorCallCount = 0;
    }

    public int GetEnumeratorCallCount { get; private set; }

    public IEnumerator<TElement> GetEnumerator()
    {
        GetEnumeratorCallCount++;
        return backingSet.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

internal class CountingGenericCollection<TElement> : ICollection<TElement>
{
    private readonly ICollection<TElement> backingSet;

    public CountingGenericCollection(ICollection<TElement> backingSet)
    {
        this.backingSet = backingSet;
    }

    public int GetEnumeratorCallCount { get; private set; }

    public IEnumerator<TElement> GetEnumerator()
    {
        GetEnumeratorCallCount++;
        return backingSet.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(TElement item) { throw new NotImplementedException(); }

    public void Clear() { throw new NotImplementedException(); }

    public bool Contains(TElement item) { throw new NotImplementedException(); }

    public void CopyTo(TElement[] array, int arrayIndex) { throw new NotImplementedException(); }

    public bool Remove(TElement item) { throw new NotImplementedException(); }

    public int GetCountCallCount { get; private set; }

    public int Count
    {
        get
        {
            GetCountCallCount++;
            return backingSet.Count;
        }
    }

    public bool IsReadOnly { get; private set; }
}

internal sealed class TrackingTestEnumerable : IEnumerable<int>
{
    private readonly int[] values;

    public TrackingTestEnumerable(params int[] values)
    {
        this.values = values;
        Enumerator = new TrackingEnumerator(this.values);
    }

    public TrackingEnumerator Enumerator { get; }

    public IEnumerator<int> GetEnumerator()
    {
        Enumerator.IncreaseEnumerationCount();
        Enumerator.Reset();
        return Enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal sealed class TrackingEnumerator : IEnumerator<int>
{
    private readonly int[] values;
    private int index;

    public TrackingEnumerator(int[] values)
    {
        index = -1;

        this.values = values;
    }

    public int LoopCount { get; private set; }

    public void IncreaseEnumerationCount()
    {
        LoopCount++;
    }

    public bool MoveNext()
    {
        index++;
        return index < values.Length;
    }

    public void Reset()
    {
        index = -1;
    }

    public void Dispose() { }

    object IEnumerator.Current => Current;

    public int Current => values[index];
}

internal class OneTimeEnumerable<T> : IEnumerable<T>
{
    private readonly T[] items;
    private int enumerations;

    public OneTimeEnumerable(params T[] items) => this.items = items;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator()
    {
        if (enumerations++ > 0)
        {
            throw new InvalidOperationException("OneTimeEnumerable can be enumerated one time only");
        }

        return items.AsEnumerable().GetEnumerator();
    }
}

internal class SomeClass
{
    public string Text { get; set; }

    public int Number { get; set; }
}
