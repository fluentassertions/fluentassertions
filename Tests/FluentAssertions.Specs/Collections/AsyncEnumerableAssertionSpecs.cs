using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <summary>
/// Collection assertion specs.
/// </summary>
public partial class AsyncEnumerableAssertionSpecs
{
    public class Chainings
    {
        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            int[] collection = [1, 2, 3];

            // Act / Assert
            collection.ToAsyncEnumerable()
                .Should()
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
            (int, string)[] collection =
            [
                (1, "a"),
                (2, "b"),
                (2, "c"),
                (3, "a")
            ];

            // Act
            Action action = () => collection.ToAsyncEnumerable()
                .Should()
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
            (int, string)[] tuples =
            [
                (1, "a"),
                (2, "b"),
                (2, "c"),
                (3, "a")
            ];

            var collection = tuples.ToAsyncEnumerable();

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
            (int, string)[] collection =
            [
                (1, "a"),
                (2, "B"),
                (2, "b"),
                (3, "a")
            ];

            // Act
            Action action = () => collection.ToAsyncEnumerable()
                .Should()
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
            (int, string, double)[] collection =
            [
                (1, "a", 1.1),
                (2, "b", 1.2),
                (2, "c", 1.3),
                (3, "a", 1.1)
            ];

            // Act
            Action action = () => collection.ToAsyncEnumerable()
                .Should()
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
            (int, string)[] collection =
            [
                (3, "a"),
                (2, "c"),
                (2, "b"),
                (1, "a")
            ];

            // Act
            Action action = () => collection.ToAsyncEnumerable()
                .Should()
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
            (int, string)[] tuples =
            [
                (3, "a"),
                (2, "c"),
                (2, "b"),
                (1, "a")
            ];

            var collection = tuples.ToAsyncEnumerable();

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
            (int, string)[] collection =
            [
                (3, "a"),
                (2, "b"),
                (2, "B"),
                (1, "a")
            ];

            // Act
            Action action = () => collection.ToAsyncEnumerable()
                .Should()
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
            (int, string, double)[] collection =
            [
                (3, "a", 1.1),
                (2, "c", 1.3),
                (2, "b", 1.2),
                (1, "a", 1.1)
            ];

            // Act
            Action action = () => collection.ToAsyncEnumerable()
                .Should()
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

internal class CountingAsyncEnumerable<TElement> : IAsyncEnumerable<TElement>
{
    private readonly IAsyncEnumerable<TElement> backingSet;

    public CountingAsyncEnumerable(IEnumerable<TElement> backingSet)
    {
        this.backingSet = backingSet.ToAsyncEnumerable();
        GetEnumeratorCallCount = 0;
    }

    public int GetEnumeratorCallCount { get; private set; }

    public IAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        GetEnumeratorCallCount++;
        return backingSet.GetAsyncEnumerator(cancellationToken);
    }
}
