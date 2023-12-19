using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    public class HaveCountGreaterThan
    {
        [Fact]
        public void Should_succeed_when_asserting_dictionary_has_a_count_greater_than_less_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act / Assert
            dictionary.Should().HaveCountGreaterThan(2);
        }

        [Fact]
        public void Should_fail_when_asserting_dictionary_has_a_count_greater_than_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().HaveCountGreaterThan(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_dictionary_has_a_count_greater_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action action = () =>
                dictionary.Should().HaveCountGreaterThan(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected dictionary to contain more than 3 item(s) because we want to test the failure message, but found 3: {[1] = \"One\", [2] = \"Two\", [3] = \"Three\"}.");
        }

        [Fact]
        public void When_dictionary_count_is_greater_than_and_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().HaveCountGreaterThan(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*more than*1*we want to test the behaviour with a null subject*found <null>*");
        }
    }
}
