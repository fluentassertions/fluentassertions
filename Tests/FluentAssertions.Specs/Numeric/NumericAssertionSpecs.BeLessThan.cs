using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class BeLessThan
    {
        [Fact]
        public void When_a_value_is_less_than_greater_value_it_should_not_throw()
        {
            // Arrange
            int value = 1;
            int greaterValue = 2;

            // Act
            Action act = () => value.Should().BeLessThan(greaterValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_value_is_less_than_smaller_value_it_should_throw()
        {
            // Arrange
            int value = 2;
            int smallerValue = 1;

            // Act
            Action act = () => value.Should().BeLessThan(smallerValue);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_value_is_less_than_same_value_it_should_throw()
        {
            // Arrange
            int value = 2;
            int sameValue = 2;

            // Act
            Action act = () => value.Should().BeLessThan(sameValue);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_value_is_less_than_smaller_value_it_should_throw_with_descriptive_message()
        {
            // Arrange
            int value = 2;
            int smallerValue = 1;

            // Act
            Action act = () => value.Should().BeLessThan(smallerValue, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be less than 1 because we want to test the failure message, but found 2.");
        }

        [Fact]
        public void NaN_is_never_less_than_another_float()
        {
            // Act
            Action act = () => float.NaN.Should().BeLessThan(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void A_float_can_never_be_less_than_NaN()
        {
            // Act
            Action act = () => 3.4F.Should().BeLessThan(float.NaN);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void NaN_is_never_less_than_another_double()
        {
            // Act
            Action act = () => double.NaN.Should().BeLessThan(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void A_double_can_never_be_less_than_NaN()
        {
            // Act
            Action act = () => 3.4D.Should().BeLessThan(double.NaN);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("*NaN*");
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_less_than_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().BeLessThan(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        [Theory]
        [InlineData(5, -1)]
        [InlineData(10, 5)]
        [InlineData(10, -1)]
        public void To_test_the_remaining_paths_for_difference_on_int(int subject, int expectation)
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
        public void To_test_the_remaining_paths_for_difference_on_long(long subject, long expectation)
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
