using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    public class ContainValues
    {
        [Fact]
        public void When_dictionary_contains_multiple_values_from_the_dictionary_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainValues("Two", "One");

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_a_dictionary_does_not_contain_a_number_of_values_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainValues(new[] { "Two", "Three" }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1] = \"One\", [2] = \"Two\"} to contain value {\"Two\", \"Three\"} because we do, but could not find {\"Three\"}.");
        }

        [Fact]
        public void
            When_the_contents_of_a_dictionary_are_checked_against_an_empty_list_of_values_it_should_throw_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainValues();

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify value containment against an empty sequence*");
        }
    }

    public class NotContainValues
    {
        [Fact]
        public void When_dictionary_does_not_contain_multiple_values_that_is_not_in_the_dictionary_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainValues("Three", "Four");

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_a_dictionary_contains_a_exactly_one_of_the_values_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainValues(new[] { "Two" }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1] = \"One\", [2] = \"Two\"} to not contain value \"Two\" because we do.");
        }

        [Fact]
        public void When_a_dictionary_contains_a_number_of_values_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainValues(new[] { "Two", "Three" }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1] = \"One\", [2] = \"Two\"} to not contain value {\"Two\", \"Three\"} because we do, but found {\"Two\"}.");
        }

        [Fact]
        public void
            When_the_noncontents_of_a_dictionary_are_checked_against_an_empty_list_of_values_it_should_throw_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainValues();

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify value containment with an empty sequence*");
        }

        [Fact]
        public void Null_dictionaries_do_not_contain_any_values()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                dictionary.Should().NotContainValues(new[] { "Two", "Three" }, "because {0}", "we do");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to not contain values {\"Two\", \"Three\"} because we do, but found <null>.");
        }
    }
}
