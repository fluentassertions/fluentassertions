using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class BePositive
    {
        [Fact]
        public void When_a_positive_value_is_positive_it_should_not_throw()
        {
            // Arrange
            float value = 1F;

            // Act
            Action act = () => value.Should().BePositive();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_negative_value_is_positive_it_should_throw()
        {
            // Arrange
            double value = -1D;

            // Act
            Action act = () => value.Should().BePositive();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_zero_value_is_positive_it_should_throw()
        {
            // Arrange
            int value = 0;

            // Act
            Action act = () => value.Should().BePositive();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void NaN_is_never_a_positive_float()
        {
            // Arrange
            float value = float.NaN;

            // Act
            Action act = () => value.Should().BePositive();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*but found NaN*");
        }

        [Fact]
        public void NaN_is_never_a_positive_double()
        {
            // Arrange
            double value = double.NaN;

            // Act
            Action act = () => value.Should().BePositive();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*but found NaN*");
        }

        [Fact]
        public void When_a_negative_value_is_positive_it_should_throw_with_descriptive_message()
        {
            // Arrange
            int value = -1;

            // Act
            Action act = () => value.Should().BePositive("because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be positive because we want to test the failure message, but found -1.");
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_positive_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().BePositive();

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }
    }
}
