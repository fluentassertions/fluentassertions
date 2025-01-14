using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Numeric;

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

        [Theory]
        [InlineData(5, -1)]
        [InlineData(10, 5)]
        [InlineData(10, -1)]
        public void To_test_the_remaining_paths_for_difference_on_nullable_int(int? subject, int expectation)
        {
            // Arrange
            // Act
            Action act = () => subject.Should().BeLessThan(expectation);

            // Assert
            act
                .Should().Throw<XunitException>()
                .Which.Message.Should().NotMatch("*(difference of 0)*");
        }

        [Theory]
        [InlineData(5L, -1L)]
        [InlineData(10L, 5L)]
        [InlineData(10L, -1L)]
        public void To_test_the_remaining_paths_for_difference_on_nullable_long(long? subject, long expectation)
        {
            // Arrange
            // Act
            Action act = () => subject.Should().BeLessThan(expectation);

            // Assert
            act
                .Should().Throw<XunitException>()
                .Which.Message.Should().NotMatch("*(difference of 0)*");
        }
    }
}
