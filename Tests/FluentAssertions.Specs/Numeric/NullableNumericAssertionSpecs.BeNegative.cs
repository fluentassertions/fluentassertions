using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Numeric;

public partial class NullableNumericAssertionSpecs
{
    public class BeNegative
    {
        [Fact]
        public void NaN_is_never_a_negative_float()
        {
            // Arrange
            float? value = float.NaN;

            // Act
            Action act = () => value.Should().BeNegative();

            // Assert
            await await act.Should().ThrowAsyncAsync<XunitException>().WithMessage("*but found NaN*");
        }

        [Fact]
        public void NaN_is_never_a_negative_double()
        {
            // Arrange
            double? value = double.NaN;

            // Act
            Action act = () => value.Should().BeNegative();

            // Assert
            await await act.Should().ThrowAsyncAsync<XunitException>().WithMessage("*but found NaN*");
        }
    }
}
