using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NullableNumericAssertionSpecs
{
    public class BeInRange
    {
        [Theory]
        [InlineData(float.NaN, 5F)]
        [InlineData(5F, float.NaN)]
        public void A_float_can_never_be_in_a_range_containing_NaN(float minimumValue, float maximumValue)
        {
            // Arrange
            float? value = 4.5F;

            // Act
            Action act = () => value.Should().BeInRange(minimumValue, maximumValue);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage(
                    "*NaN*");
        }

        [Fact]
        public void NaN_is_never_in_range_of_two_floats()
        {
            // Arrange
            float? value = float.NaN;

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
            double? value = 4.5;

            // Act
            Action act = () => value.Should().BeInRange(minimumValue, maximumValue);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage(
                    "*NaN*");
        }

        [Fact]
        public void NaN_is_never_in_range_of_two_doubles()
        {
            // Arrange
            double? value = double.NaN;

            // Act
            Action act = () => value.Should().BeInRange(4, 5);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be between*4* and*5*, but found*NaN*");
        }
    }

    public class NotBeInRange
    {
        [Theory]
        [InlineData(float.NaN, 1F)]
        [InlineData(1F, float.NaN)]
        public void Cannot_use_NaN_in_a_range_of_floats(float minimumValue, float maximumValue)
        {
            // Arrange
            float? value = 4.5F;

            // Act
            Action act = () => value.Should().NotBeInRange(minimumValue, maximumValue);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void NaN_is_never_inside_any_range_of_floats()
        {
            // Arrange
            float? value = float.NaN;

            // Act / Assert
            value.Should().NotBeInRange(4, 5);
        }

        [Theory]
        [InlineData(double.NaN, 1D)]
        [InlineData(1D, double.NaN)]
        public void Cannot_use_NaN_in_a_range_of_doubles(double minimumValue, double maximumValue)
        {
            // Arrange
            double? value = 4.5D;

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
            double? value = double.NaN;

            // Act / Assert
            value.Should().NotBeInRange(4, 5);
        }
    }
}
