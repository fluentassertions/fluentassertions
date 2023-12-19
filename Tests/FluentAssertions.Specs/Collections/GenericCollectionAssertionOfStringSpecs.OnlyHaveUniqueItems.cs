using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class OnlyHaveUniqueItems
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_with_unique_items_contains_only_unique_items()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three", "four" };

            // Act / Assert
            collection.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void When_a_collection_contains_duplicate_items_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three", "three" };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems("{0} don't like {1}", "we", "duplicates");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we don't like duplicates, but item \"three\" is not unique.");
        }

        [Fact]
        public void When_a_collection_contains_multiple_duplicate_items_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "two", "three", "three" };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems("{0} don't like {1}", "we", "duplicates");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we don't like duplicates, but items {\"two\", \"three\"} are not unique.");
        }

        [Fact]
        public void When_asserting_collection_to_only_have_unique_items_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act =
                () => collection.Should().OnlyHaveUniqueItems("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we want to test the behaviour with a null subject, but found <null>.");
        }
    }
}
