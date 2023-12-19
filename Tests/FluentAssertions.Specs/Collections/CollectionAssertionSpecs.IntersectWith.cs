using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The [Not]IntersectWith specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class IntersectWith
    {
        [Fact]
        public void When_asserting_the_items_in_an_two_intersecting_collections_intersect_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };
            var otherCollection = new[] { 3, 4, 5 };

            // Act / Assert
            collection.Should().IntersectWith(otherCollection);
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_non_intersecting_collections_intersect_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };
            var otherCollection = new[] { 4, 5 };

            // Act
            Action action = () => collection.Should().IntersectWith(otherCollection, "they should share items");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to intersect with {4, 5} because they should share items," +
                    " but {1, 2, 3} does not contain any shared items.");
        }

        [Fact]
        public void When_collection_is_null_then_intersect_with_should_fail()
        {
            // Arrange
            IEnumerable<int> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().IntersectWith(new[] { 4, 5 }, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection to intersect with {4, 5} *failure message*, but found <null>.");
        }
    }

    public class NotIntersectWith
    {
        [Fact]
        public void When_asserting_the_items_in_an_two_non_intersecting_collections_do_not_intersect_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };
            var otherCollection = new[] { 4, 5 };

            // Act / Assert
            collection.Should().NotIntersectWith(otherCollection);
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_intersecting_collections_do_not_intersect_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };
            var otherCollection = new[] { 2, 3, 4 };

            // Act
            Action action = () => collection.Should().NotIntersectWith(otherCollection, "they should not share items");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to intersect with {2, 3, 4} because they should not share items," +
                    " but found the following shared items {2, 3}.");
        }

        [Fact]
        public void When_asserting_collection_to_not_intersect_with_same_collection_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };
            var otherCollection = collection;

            // Act
            Action act = () => collection.Should().NotIntersectWith(otherCollection,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect*to intersect with*because we want to test the behaviour with same objects*but they both reference the same object.");
        }

        [Fact]
        public void When_collection_is_null_then_not_intersect_with_should_fail()
        {
            // Arrange
            IEnumerable<int> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotIntersectWith(new[] { 4, 5 }, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to intersect with {4, 5} *failure message*, but found <null>.");
        }
    }
}
