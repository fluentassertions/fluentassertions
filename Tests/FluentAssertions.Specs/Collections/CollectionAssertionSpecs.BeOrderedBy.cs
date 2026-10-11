using System;
using System.Collections.Generic;
using FluentAssertions.Collections;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <content>
/// The BeOrderedBy with CollectionSortOrder direction specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class BeOrderedBy
    {
        [Fact]
        public void When_collection_is_ordered_ascending_and_direction_is_ascending_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Name = "a", Age = 1 },
                new { Name = "b", Age = 2 },
                new { Name = "c", Age = 3 },
            };

            // Act
            Action act = () => collection.Should().BeOrderedBy(x => x.Age, CollectionSortOrder.Ascending);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_is_ordered_descending_and_direction_is_descending_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Name = "c", Age = 3 },
                new { Name = "b", Age = 2 },
                new { Name = "a", Age = 1 },
            };

            // Act
            Action act = () => collection.Should().BeOrderedBy(x => x.Age, CollectionSortOrder.Descending);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_is_not_ordered_ascending_and_direction_is_ascending_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Name = "b", Age = 2 },
                new { Name = "a", Age = 1 },
            };

            // Act
            Action act = () => collection.Should().BeOrderedBy(x => x.Age, CollectionSortOrder.Ascending);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*to be ordered*");
        }

        [Fact]
        public void When_collection_is_not_ordered_descending_and_direction_is_descending_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Name = "a", Age = 1 },
                new { Name = "b", Age = 2 },
            };

            // Act
            Action act = () => collection.Should().BeOrderedBy(x => x.Age, CollectionSortOrder.Descending);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*to be ordered*");
        }

        [Fact]
        public void When_comparer_is_null_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Name = "a", Age = 1 },
            };

            // Act
            Action act = () => collection.Should().BeOrderedBy(x => x.Age, null, CollectionSortOrder.Ascending);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert collection ordering without specifying a comparer*");
        }

        [Fact]
        public void When_direction_is_not_defined_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Name = "a", Age = 1 },
            };

            // Act
            Action act = () => collection.Should().BeOrderedBy(x => x.Age, (CollectionSortOrder)42);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void When_direction_is_not_defined_on_comparer_overload_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Name = "a", Age = 1 },
            };

            // Act
            Action act = () => collection.Should().BeOrderedBy(x => x.Age, Comparer<int>.Default, (CollectionSortOrder)42);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
