using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The BeOneOf specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class BeOneOf
    {
        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            string value = "abc";

            // Act
            Action action = () => value.Should().BeOneOf("def", "xyz");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {\"def\", \"xyz\"}, but found \"abc\".");
        }

        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw_with_descriptive_message()
        {
            // Arrange
            string value = "abc";

            // Act
            Action action = () => value.Should().BeOneOf(new[] { "def", "xyz" }, "because those are the valid values");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be one of {\"def\", \"xyz\"} because those are the valid values, but found \"abc\".");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed()
        {
            // Arrange
            string value = "abc";

            // Act
            Action action = () => value.Should().BeOneOf("abc", "def", "xyz");

            // Assert
            action.Should().NotThrow();
        }
    }
}
