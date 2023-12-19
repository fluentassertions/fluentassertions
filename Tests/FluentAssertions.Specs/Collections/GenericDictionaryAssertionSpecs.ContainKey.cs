using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    public class ContainKey
    {
        [Fact]
        public void Should_succeed_when_asserting_dictionary_contains_a_key_from_the_dictionary()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            dictionary.Should().ContainKey(1);
        }

        [Fact]
        public void When_a_dictionary_has_custom_equality_comparer_the_contains_key_assertion_should_work_accordingly()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["One"] = "One",
                ["Two"] = "Two"
            };

            // Act

            // Assert
            dictionary.Should().ContainKey("One");
            dictionary.Should().ContainKey("ONE");
            dictionary.Should().ContainKey("one");
        }

        [Fact]
        public void When_a_dictionary_does_not_contain_single_key_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainKey(3, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1] = \"One\", [2] = \"Two\"} to contain key 3 because we do.");
        }

        [Fact]
        public void When_the_requested_key_exists_it_should_allow_continuation_with_the_value()
        {
            // Arrange
            var dictionary = new Dictionary<string, MyClass>
            {
                ["Key"] = new() { SomeProperty = 3 }
            };

            // Act
            Action act = () => dictionary.Should().ContainKey("Key").WhoseValue.Should().Be(4);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*4*3*.");
        }

        [Fact]
        public void When_an_assertion_fails_on_ContainKey_succeeding_message_should_be_included()
        {
            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                var values = new Dictionary<int, int>();
                values.Should().ContainKey(0);
                values.Should().ContainKey(1);
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*to contain key 0*Expected*to contain key 1*");
        }
    }

    public class NotContainKey
    {
        [Fact]
        public void When_dictionary_does_not_contain_a_key_that_is_not_in_the_dictionary_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKey(4);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_dictionary_contains_an_unexpected_key_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKey(1, "because we {0} like it", "don't");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1] = \"One\", [2] = \"Two\"} not to contain key 1 because we don't like it, but found it anyhow.");
        }

        [Fact]
        public void When_asserting_dictionary_does_not_contain_key_against_null_dictionary_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                dictionary.Should().NotContainKey(1, "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary not to contain key 1 because we want to test the behaviour with a null subject, but found <null>.");
        }
    }
}
