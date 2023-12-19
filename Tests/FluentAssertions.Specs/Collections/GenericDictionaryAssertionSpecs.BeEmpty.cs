using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    public class BeEmpty
    {
        [Fact]
        public void Should_succeed_when_asserting_dictionary_without_items_is_empty()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            // Act / Assert
            dictionary.Should().BeEmpty();
        }

        [Fact]
        public void Should_fail_when_asserting_dictionary_with_items_is_empty()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One"
            };

            // Act
            Action act = () => dictionary.Should().BeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_dictionary_with_items_is_empty()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One"
            };

            // Act
            Action act = () => dictionary.Should().BeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected dictionary to be empty because we want to test the failure message, but found {[1] = \"One\"}.");
        }

        [Fact]
        public void When_asserting_dictionary_to_be_empty_but_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().BeEmpty("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }
    }

    public class NotBeEmpty
    {
        [Fact]
        public void When_asserting_dictionary_with_items_is_not_empty_it_should_succeed()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One"
            };

            // Act / Assert
            dictionary.Should().NotBeEmpty();
        }

#if !NET5_0_OR_GREATER
        [Fact]
        public void When_asserting_dictionary_with_items_is_not_empty_it_should_enumerate_the_dictionary_only_once()
        {
            // Arrange
            var trackingDictionary = new TrackingTestDictionary(new KeyValuePair<int, string>(1, "One"));

            // Act
            trackingDictionary.Should().NotBeEmpty();

            // Assert
            trackingDictionary.Enumerator.LoopCount.Should().Be(1);
        }

#endif

        [Fact]
        public void When_asserting_dictionary_without_items_is_not_empty_it_should_fail()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            // Act
            Action act = () => dictionary.Should().NotBeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_dictionary_without_items_is_not_empty_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            // Act
            Action act = () => dictionary.Should().NotBeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected dictionary not to be empty because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_dictionary_to_be_not_empty_but_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().NotBeEmpty("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary not to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }
    }
}
