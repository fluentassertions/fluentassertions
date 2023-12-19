using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    public class HaveCountLessThanOrEqualTo
    {
        [Fact]
        public void Should_succeed_when_asserting_dictionary_has_a_count_less_than_or_equal_to_less_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act / Assert
            dictionary.Should().HaveCountLessThanOrEqualTo(3);
        }

        [Fact]
        public void Should_fail_when_asserting_dictionary_has_a_count_less_than_or_equal_to_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().HaveCountLessThanOrEqualTo(2);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_dictionary_has_a_count_less_than_or_equal_to_the_number_of_items_it_should_fail_with_descriptive_message_()
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
                dictionary.Should().HaveCountLessThanOrEqualTo(2, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected dictionary to contain at most 2 item(s) because we want to test the failure message, but found 3: {[1] = \"One\", [2] = \"Two\", [3] = \"Three\"}.");
        }

        [Fact]
        public void When_dictionary_count_is_less_than_or_equal_to_and_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () =>
                dictionary.Should().HaveCountLessThanOrEqualTo(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*at most*1*we want to test the behaviour with a null subject*found <null>*");
        }
    }
}
