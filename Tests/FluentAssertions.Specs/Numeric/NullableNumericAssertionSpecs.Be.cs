using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NullableNumericAssertionSpecs
{
    public class Be
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_equals_an_equal_value()
        {
            // Arrange
            int? nullableIntegerA = 1;
            int? nullableIntegerB = 1;

            // Act / Assert
            nullableIntegerA.Should().Be(nullableIntegerB);
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_null_value_equals_null()
        {
            // Arrange
            int? nullableIntegerA = null;
            int? nullableIntegerB = null;

            // Act / Assert
            nullableIntegerA.Should().Be(nullableIntegerB);
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            // Arrange
            int? nullableIntegerA = 1;
            int? nullableIntegerB = 2;

            // Act
            Action act = () => nullableIntegerA.Should().Be(nullableIntegerB);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            // Arrange
            int? nullableIntegerA = 1;
            int? nullableIntegerB = 2;

            // Act
            Action act = () =>
                nullableIntegerA.Should().Be(nullableIntegerB, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*2 because we want to test the failure message, but found 1.");
        }

        [Fact]
        public void Nan_is_never_equal_to_a_normal_float()
        {
            // Arrange
            float? value = float.NaN;

            // Act
            Action act = () => value.Should().Be(3.4F);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be *3.4F, but found NaN*");
        }

        [Fact]
        public void NaN_can_be_compared_to_NaN_when_its_a_float()
        {
            // Arrange
            float? value = float.NaN;

            // Act
            value.Should().Be(float.NaN);
        }

        [Fact]
        public void Nan_is_never_equal_to_a_normal_double()
        {
            // Arrange
            double? value = double.NaN;

            // Act
            Action act = () => value.Should().Be(3.4D);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be *3.4, but found NaN*");
        }

        [Fact]
        public void NaN_can_be_compared_to_NaN_when_its_a_double()
        {
            // Arrange
            double? value = double.NaN;

            // Act
            value.Should().Be(double.NaN);
        }
    }
}
