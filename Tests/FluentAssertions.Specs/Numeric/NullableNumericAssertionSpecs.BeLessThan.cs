using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NullableNumericAssertionSpecs
{
    public class BeLessThan
    {
        [Fact]
        public void A_float_can_never_be_less_than_NaN()
        {
            // Arrange
            float? value = 3.4F;

            // Act
            Action act = () => value.Should().BeLessThan(float.NaN);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void NaN_is_never_less_than_another_float()
        {
            // Arrange
            float? value = float.NaN;

            // Act
            Action act = () => value.Should().BeLessThan(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void A_double_can_never_be_less_than_NaN()
        {
            // Arrange
            double? value = 3.4F;

            // Act
            Action act = () => value.Should().BeLessThan(double.NaN);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void NaN_is_never_less_than_another_double()
        {
            // Arrange
            double? value = double.NaN;

            // Act
            Action act = () => value.Should().BeLessThan(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*NaN*");
        }
    }
}
