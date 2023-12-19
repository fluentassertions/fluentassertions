using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class BeEmpty
    {
        [Fact]
        public void Should_fail_when_asserting_collection_with_items_is_empty()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().BeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_without_items_is_empty()
        {
            // Arrange
            IEnumerable<string> collection = new string[0];

            // Act / Assert
            collection.Should().BeEmpty();
        }

        [Fact]
        public void When_asserting_collection_to_be_empty_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().BeEmpty("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_the_collection_is_not_empty_unexpectedly_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().BeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to be empty because we want to test the failure message, but found*one*two*three*");
        }
    }

    public class NotBeEmpty
    {
        [Fact]
        public void When_asserting_collection_to_be_not_empty_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().NotBeEmpty("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_with_items_is_not_empty_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should().NotBeEmpty();
        }

        [Fact]
        public void When_asserting_collection_without_items_is_not_empty_it_should_fail()
        {
            // Arrange
            IEnumerable<string> collection = new string[0];

            // Act
            Action act = () => collection.Should().NotBeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_collection_without_items_is_not_empty_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            IEnumerable<string> collection = new string[0];

            // Act
            Action act = () => collection.Should().NotBeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection not to be empty because we want to test the failure message.");
        }
    }
}
