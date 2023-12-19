using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class HaveElementAt
    {
        [Fact]
        public void When_asserting_collection_has_element_at_specific_index_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().HaveElementAt(1, "one",
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to have element at index 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_collection_does_not_have_an_element_at_the_specific_index_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().HaveElementAt(4, "three", "we put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected \"three\" at index 4 because we put it there, but found no element.");
        }

        [Fact]
        public void When_collection_does_not_have_the_expected_element_at_specific_index_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().HaveElementAt(1, "three", "we put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected \"three\" at index 1 because we put it there, but found \"two\".");
        }

        [Fact]
        public void When_collection_has_expected_element_at_specific_index_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should().HaveElementAt(1, "two");
        }
    }
}
