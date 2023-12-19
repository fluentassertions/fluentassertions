using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NullableNumericAssertionSpecs
{
    public class BePositive
    {
        [Fact]
        public void NaN_is_never_a_positive_float()
        {
            // Arrange
            float? value = float.NaN;

            // Act
            Action act = () => value.Should().BePositive();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*but found NaN*");
        }

        [Fact]
        public void NaN_is_never_a_positive_double()
        {
            // Arrange
            double? value = double.NaN;

            // Act
            Action act = () => value.Should().BePositive();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*but found NaN*");
        }
    }
}
