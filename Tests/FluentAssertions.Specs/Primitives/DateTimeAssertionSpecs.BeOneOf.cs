using System;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class BeOneOf
    {
        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            DateTime value = new(2016, 12, 30, 23, 58, 57);

            // Act
            Action action = () => value.Should().BeOneOf(value + 1.Days(), value + 1.Milliseconds());

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be one of {<2016-12-31 23:58:57>, <2016-12-30 23:58:57.001>}, but found <2016-12-30 23:58:57>.");
        }

        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw_with_descriptive_message()
        {
            // Arrange
            DateTime value = new(2016, 12, 30, 23, 58, 57);

            // Act
            Action action = () =>
                value.Should().BeOneOf(new[] { value + 1.Days(), value + 1.Milliseconds() }, "because it's true");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be one of {<2016-12-31 23:58:57>, <2016-12-30 23:58:57.001>} because it's true, but found <2016-12-30 23:58:57>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed()
        {
            // Arrange
            DateTime value = new(2016, 12, 30, 23, 58, 57);

            // Act
            Action action = () => value.Should().BeOneOf(new DateTime(2216, 1, 30, 0, 5, 7),
                new DateTime(2016, 12, 30, 23, 58, 57), new DateTime(2012, 3, 3));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_a_null_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            DateTime? value = null;

            // Act
            Action action = () => value.Should().BeOneOf(new DateTime(2216, 1, 30, 0, 5, 7), new DateTime(1116, 4, 10, 2, 45, 7));

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {<2216-01-30 00:05:07>, <1116-04-10 02:45:07>}, but found <null>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed_when_datetime_is_null()
        {
            // Arrange
            DateTime? value = null;

            // Act
            Action action = () => value.Should().BeOneOf(new DateTime(2216, 1, 30, 0, 5, 7), null);

            // Assert
            action.Should().NotThrow();
        }
    }
}
