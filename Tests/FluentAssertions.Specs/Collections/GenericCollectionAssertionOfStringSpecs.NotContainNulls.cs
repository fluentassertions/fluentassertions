using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class NotContainNulls
    {
        [Fact]
        public void When_asserting_collection_to_not_contain_nulls_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().NotContainNulls("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s because we want to test the behaviour with a null subject, but collection is <null>.");
        }

        [Fact]
        public void When_collection_contains_multiple_nulls_that_are_unexpected_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "", null, "", null };

            // Act
            Action act = () => collection.Should().NotContainNulls("because they are {0}", "evil");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s*because they are evil*{1, 3}*");
        }

        [Fact]
        public void When_collection_contains_nulls_that_are_unexpected_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "", null };

            // Act
            Action act = () => collection.Should().NotContainNulls("because they are {0}", "evil");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s because they are evil, but found one at index 1.");
        }

        [Fact]
        public void When_collection_does_not_contain_nulls_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should().NotContainNulls();
        }
    }
}
