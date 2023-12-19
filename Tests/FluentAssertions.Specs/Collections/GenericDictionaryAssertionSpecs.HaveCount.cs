using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    public class HaveCount
    {
        [Fact]
        public void Should_succeed_when_asserting_dictionary_has_a_count_that_equals_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act / Assert
            dictionary.Should().HaveCount(3);
        }

        [Fact]
        public void Should_fail_when_asserting_dictionary_has_a_count_that_is_different_from_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().HaveCount(4);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_dictionary_has_a_count_that_is_different_from_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action action = () => dictionary.Should().HaveCount(4, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected dictionary to contain 4 item(s) because we want to test the failure message, but found 3: {[1] = \"One\", [2] = \"Two\", [3] = \"Three\"}.");
        }

        [Fact]
        public void When_dictionary_has_a_count_larger_than_the_minimum_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act / Assert
            dictionary.Should().HaveCount(c => c >= 3);
        }

        [Fact]
        public void When_dictionary_has_a_count_that_not_matches_the_predicate_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().HaveCount(c => c >= 4, "a minimum of 4 is required");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to have a count (c >= 4) because a minimum of 4 is required, but count is 3: {[1] = \"One\", [2] = \"Two\", [3] = \"Three\"}.");
        }

        [Fact]
        public void When_dictionary_count_is_matched_against_a_null_predicate_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().HaveCount(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare collection count against a <null> predicate.*");
        }

        [Fact]
        public void When_dictionary_count_is_matched_and_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().HaveCount(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain 1 item(s) because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_dictionary_count_is_matched_against_a_predicate_and_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().HaveCount(c => c < 3, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain (c < 3) items because we want to test the behaviour with a null subject, but found <null>.");
        }
    }

    public class NotHaveCount
    {
        [Fact]
        public void Should_succeed_when_asserting_dictionary_has_a_count_different_from_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act / Assert
            dictionary.Should().NotHaveCount(2);
        }

        [Fact]
        public void Should_fail_when_asserting_dictionary_has_a_count_that_equals_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().NotHaveCount(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_dictionary_has_a_count_that_equals_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action action = () => dictionary.Should().NotHaveCount(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*not contain*3*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_dictionary_count_is_same_than_and_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().NotHaveCount(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*not contain*1*we want to test the behaviour with a null subject*found <null>*");
        }
    }
}
