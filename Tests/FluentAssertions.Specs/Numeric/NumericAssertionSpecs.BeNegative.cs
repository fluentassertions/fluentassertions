using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class BeNegative
    {
        [Fact]
        public void When_a_negative_value_is_negative_it_should_not_throw()
        {
            // Arrange
            int value = -1;

            // Act
            Action act = () => value.Should().BeNegative();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_positive_value_is_negative_it_should_throw()
        {
            // Arrange
            int value = 1;

            // Act
            Action act = () => value.Should().BeNegative();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_zero_value_is_negative_it_should_throw()
        {
            // Arrange
            int value = 0;

            // Act
            Action act = () => value.Should().BeNegative();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_positive_value_is_negative_it_should_throw_with_descriptive_message()
        {
            // Arrange
            int value = 1;

            // Act
            Action act = () => value.Should().BeNegative("because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be negative because we want to test the failure message, but found 1.");
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_negative_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().BeNegative();

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        [Fact]
        public void NaN_is_never_a_negative_float()
        {
            // Arrange
            float value = float.NaN;

            // Act
            Action act = () => value.Should().BeNegative();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*but found NaN*");
        }

        [Fact]
        public void NaN_is_never_a_negative_double()
        {
            // Arrange
            double value = double.NaN;

            // Act
            Action act = () => value.Should().BeNegative();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*but found NaN*");
        }
    }
}
