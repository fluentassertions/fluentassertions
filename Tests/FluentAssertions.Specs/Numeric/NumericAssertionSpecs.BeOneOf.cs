using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class BeOneOf
    {
        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            int value = 3;

            // Act
            Action act = () => value.Should().BeOneOf(4, 5);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {4, 5}, but found 3.");
        }

        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw_with_descriptive_message()
        {
            // Arrange
            int value = 3;

            // Act
            Action act = () => value.Should().BeOneOf(new[] { 4, 5 }, "because those are the valid values");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {4, 5} because those are the valid values, but found 3.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed()
        {
            // Arrange
            int value = 4;

            // Act
            Action act = () => value.Should().BeOneOf(4, 5);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_one_of_to_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().BeOneOf(0, 1);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        [Fact]
        public void Two_floats_that_are_NaN_can_be_compared()
        {
            // Arrange
            float value = float.NaN;

            // Act / Assert
            value.Should().BeOneOf(float.NaN, 4.5F);
        }

        [Fact]
        public void Floats_are_never_equal_to_NaN()
        {
            // Arrange
            float value = float.NaN;

            // Act
            Action act = () => value.Should().BeOneOf(1.5F, 4.5F);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected*1.5F*found*NaN*");
        }

        [Fact]
        public void Two_doubles_that_are_NaN_can_be_compared()
        {
            // Arrange
            double value = double.NaN;

            // Act / Assert
            value.Should().BeOneOf(double.NaN, 4.5F);
        }

        [Fact]
        public void Doubles_are_never_equal_to_NaN()
        {
            // Arrange
            double value = double.NaN;

            // Act
            Action act = () => value.Should().BeOneOf(1.5D, 4.5D);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected*1.5*found NaN*");
        }
    }
}
