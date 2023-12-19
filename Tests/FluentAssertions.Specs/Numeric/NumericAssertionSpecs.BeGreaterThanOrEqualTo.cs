using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class BeGreaterThanOrEqualTo
    {
        [Fact]
        public void When_a_value_is_greater_than_or_equal_to_smaller_value_it_should_not_throw()
        {
            // Arrange
            int value = 2;
            int smallerValue = 1;

            // Act
            Action act = () => value.Should().BeGreaterThanOrEqualTo(smallerValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_value_is_greater_than_or_equal_to_same_value_it_should_not_throw()
        {
            // Arrange
            int value = 2;
            int sameValue = 2;

            // Act
            Action act = () => value.Should().BeGreaterThanOrEqualTo(sameValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_value_is_greater_than_or_equal_to_greater_value_it_should_throw()
        {
            // Arrange
            int value = 2;
            int greaterValue = 3;

            // Act
            Action act = () => value.Should().BeGreaterThanOrEqualTo(greaterValue);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_value_is_greater_than_or_equal_to_greater_value_it_should_throw_with_descriptive_message()
        {
            // Arrange
            int value = 2;
            int greaterValue = 3;

            // Act
            Action act =
                () => value.Should()
                    .BeGreaterThanOrEqualTo(greaterValue, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be greater than or equal to 3 because we want to test the failure message, but found 2.");
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_greater_than_or_equal_to_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().BeGreaterThanOrEqualTo(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        [Fact]
        public void NaN_is_never_greater_than_or_equal_to_another_float()
        {
            // Act
            Action act = () => float.NaN.Should().BeGreaterThanOrEqualTo(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void A_float_cannot_be_greater_than_or_equal_to_NaN()
        {
            // Act
            Action act = () => 3.4F.Should().BeGreaterThanOrEqualTo(float.NaN);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void NaN_is_never_greater_or_equal_to_another_double()
        {
            // Act
            Action act = () => double.NaN.Should().BeGreaterThanOrEqualTo(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void A_double_can_never_be_greater_or_equal_to_NaN()
        {
            // Act
            Action act = () => 3.4D.Should().BeGreaterThanOrEqualTo(double.NaN);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void Chaining_after_one_assertion()
        {
            // Arrange
            int value = 2;
            int smallerValue = 1;

            // Act / Assert
            value.Should().BeGreaterThanOrEqualTo(smallerValue).And.Be(2);
        }
    }
}
