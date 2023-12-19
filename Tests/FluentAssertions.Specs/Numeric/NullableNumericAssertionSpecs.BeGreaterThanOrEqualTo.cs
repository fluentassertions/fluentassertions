using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NullableNumericAssertionSpecs
{
    public class BeGreaterThanOrEqualTo
    {
        [Fact]
        public void A_float_can_never_be_greater_than_or_equal_to_NaN()
        {
            // Arrange
            float? value = 3.4F;

            // Act
            Action act = () => value.Should().BeGreaterThanOrEqualTo(float.NaN);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void NaN_is_never_greater_than_or_equal_to_another_float()
        {
            // Arrange
            float? value = float.NaN;

            // Act
            Action act = () => value.Should().BeGreaterThanOrEqualTo(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void A_double_can_never_be_greater_than_or_equal_to_NaN()
        {
            // Arrange
            double? value = 3.4;

            // Act
            Action act = () => value.Should().BeGreaterThanOrEqualTo(double.NaN);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void NaN_is_never_greater_than_or_equal_to_another_double()
        {
            // Arrange
            double? value = double.NaN;

            // Act
            Action act = () => value.Should().BeGreaterThanOrEqualTo(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*NaN*");
        }
    }
}
