using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class BeInRange
    {
        [Fact]
        public void When_a_value_is_outside_a_range_it_should_throw()
        {
            // Arrange
            float value = 3.99F;

            // Act
            Action act = () => value.Should().BeInRange(4, 5, "because that's the valid range");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be between*4* and*5* because that\'s the valid range, but found*3.99*");
        }

        [Fact]
        public void When_a_value_is_inside_a_range_it_should_not_throw()
        {
            // Arrange
            int value = 4;

            // Act
            Action act = () => value.Should().BeInRange(3, 5);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_in_range_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().BeInRange(0, 1);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        [Fact]
        public void NaN_is_never_in_range_of_two_floats()
        {
            // Arrange
            float value = float.NaN;

            // Act
            Action act = () => value.Should().BeInRange(4, 5);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be between*4* and*5*, but found*NaN*");
        }

        [Theory]
        [InlineData(float.NaN, 5F)]
        [InlineData(5F, float.NaN)]
        public void A_float_can_never_be_in_a_range_containing_NaN(float minimumValue, float maximumValue)
        {
            // Arrange
            float value = 4.5F;

            // Act
            Action act = () => value.Should().BeInRange(minimumValue, maximumValue);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage(
                    "*NaN*");
        }

        [Fact]
        public void A_NaN_is_never_in_range_of_two_doubles()
        {
            // Arrange
            double value = double.NaN;

            // Act
            Action act = () => value.Should().BeInRange(4, 5);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be between*4* and*5*, but found*NaN*");
        }

        [Theory]
        [InlineData(double.NaN, 5)]
        [InlineData(5, double.NaN)]
        public void A_double_can_never_be_in_a_range_containing_NaN(double minimumValue, double maximumValue)
        {
            // Arrange
            double value = 4.5D;

            // Act
            Action act = () => value.Should().BeInRange(minimumValue, maximumValue);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage(
                    "*NaN*");
        }
    }

    public class NotBeInRange
    {
        [Fact]
        public void When_a_value_is_inside_an_unexpected_range_it_should_throw()
        {
            // Arrange
            float value = 4.99F;

            // Act
            Action act = () => value.Should().NotBeInRange(4, 5, "because that's the invalid range");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to not be between*4* and*5* because that\'s the invalid range, but found*4.99*");
        }

        [Fact]
        public void When_a_value_is_outside_an_unexpected_range_it_should_not_throw()
        {
            // Arrange
            float value = 3.99F;

            // Act
            Action act = () => value.Should().NotBeInRange(4, 5);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_not_in_range_to_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().NotBeInRange(0, 1);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        [Fact]
        public void NaN_is_never_inside_any_range_of_floats()
        {
            // Arrange
            float value = float.NaN;

            // Act / Assert
            value.Should().NotBeInRange(4, 5);
        }

        [Theory]
        [InlineData(float.NaN, 1F)]
        [InlineData(1F, float.NaN)]
        public void Cannot_use_NaN_in_a_range_of_floats(float minimumValue, float maximumValue)
        {
            // Arrange
            float value = 4.5F;

            // Act
            Action act = () => value.Should().NotBeInRange(minimumValue, maximumValue);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void NaN_is_never_inside_any_range_of_doubles()
        {
            // Arrange
            double value = double.NaN;

            // Act / Assert
            value.Should().NotBeInRange(4, 5);
        }

        [Theory]
        [InlineData(double.NaN, 1D)]
        [InlineData(1D, double.NaN)]
        public void Cannot_use_NaN_in_a_range_of_doubles(double minimumValue, double maximumValue)
        {
            // Arrange
            double value = 4.5D;

            // Act
            Action act = () => value.Should().NotBeInRange(minimumValue, maximumValue);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("*NaN*");
        }
    }
}
