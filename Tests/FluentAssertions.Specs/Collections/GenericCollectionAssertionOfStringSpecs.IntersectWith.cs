using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class IntersectWith
    {
        [Fact]
        public void When_asserting_the_items_in_an_two_intersecting_collections_intersect_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new[] { "three", "four", "five" };

            // Act / Assert
            collection.Should().IntersectWith(otherCollection);
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_non_intersecting_collections_intersect_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new[] { "four", "five" };

            // Act
            Action action = () => collection.Should().IntersectWith(otherCollection, "they should share items");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to intersect with {\"four\", \"five\"} because they should share items," +
                    " but {\"one\", \"two\", \"three\"} does not contain any shared items.");
        }
    }

    public class NotIntersectWith
    {
        [Fact]
        public void When_asserting_collection_to_not_intersect_with_same_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = collection;

            // Act
            Action act = () => collection.Should().NotIntersectWith(otherCollection,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*to intersect with*because we want to test the behaviour with same objects*but they both reference the same object.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_intersecting_collections_do_not_intersect_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new[] { "two", "three", "four" };

            // Act
            Action action = () => collection.Should().NotIntersectWith(otherCollection, "they should not share items");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Did not expect collection to intersect with {\"two\", \"three\", \"four\"} because they should not share items," +
                    " but found the following shared items {\"two\", \"three\"}.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_non_intersecting_collections_do_not_intersect_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new[] { "four", "five" };

            // Act / Assert
            collection.Should().NotIntersectWith(otherCollection);
        }
    }
}
