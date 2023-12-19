using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    public class ContainKeys
    {
        [Fact]
        public void Should_succeed_when_asserting_dictionary_contains_multiple_keys_from_the_dictionary()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            dictionary.Should().ContainKeys(2, 1);
        }

        [Fact]
        public void When_a_dictionary_does_not_contain_a_list_of_keys_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainKeys(new[] { 2, 3 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1] = \"One\", [2] = \"Two\"} to contain keys {2, 3} because we do, but could not find {3}.");
        }

        [Fact]
        public void Null_dictionaries_do_not_contain_any_keys()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                dictionary.Should().ContainKeys(new[] { 2, 3 }, "because {0}", "we do");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain keys {2, 3} because we do, but found <null>.");
        }

        [Fact]
        public void
            When_the_contents_of_a_dictionary_are_checked_against_an_empty_list_of_keys_it_should_throw_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainKeys();

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify key containment against an empty sequence*");
        }
    }

    public class NotContainKeys
    {
        [Fact]
        public void When_dictionary_does_not_contain_multiple_keys_from_the_dictionary_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKeys(3, 4);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_a_dictionary_contains_a_list_of_keys_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKeys(new[] { 2, 3 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1] = \"One\", [2] = \"Two\"} to not contain keys {2, 3} because we do, but found {2}.");
        }

        [Fact]
        public void When_a_dictionary_contains_exactly_one_of_the_keys_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKeys(new[] { 2 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1] = \"One\", [2] = \"Two\"} to not contain key 2 because we do.");
        }

        [Fact]
        public void Null_dictionaries_do_not_contain_any_keys()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                dictionary.Should().NotContainKeys(new[] { 2 }, "because {0}", "we do");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to not contain keys {2} because we do, but found <null>.");
        }

        [Fact]
        public void
            When_the_noncontents_of_a_dictionary_are_checked_against_an_empty_list_of_keys_it_should_throw_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKeys();

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify key containment against an empty sequence*");
        }

        [Fact]
        public void
            When_a_dictionary_checks_a_list_of_keys_not_to_be_present_it_will_honor_the_case_sensitive_equality_comparer_of_the_dictionary()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["ONE"] = "One",
                ["TWO"] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKeys("One", "Two");

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void
            When_a_dictionary_checks_a_list_of_keys_not_to_be_present_it_will_honor_the_case_insensitive_equality_comparer_of_the_dictionary()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["ONE"] = "One",
                ["TWO"] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKeys("One", "Two");

            // Assert
            act.Should().Throw<XunitException>();
        }
    }
}
