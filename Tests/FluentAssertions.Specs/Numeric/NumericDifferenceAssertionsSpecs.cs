using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Numeric
{
    public class NumericDifferenceAssertionsSpecs
    {
        [Theory]
        [InlineData(10, 19)]
        [InlineData(19, 9)]
        public void When_an_int_difference_is_not_in_difference_threshold_range_it_should_not_add_difference_message(int value, int expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected} because we want to test the failure message, but found {value}.");
        }

        [Theory]
        [InlineData(50, 20)]
        [InlineData(20, 50)]
        [InlineData(123, -123)]
        [InlineData(-123, 123)]
        public void When_an_int_difference_is_in_difference_threshold_range_it_should_add_difference_message(int value, int expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected} because we want to test the failure message, but found {value} (difference of {value - expected}).");
        }

        [Theory]
        [InlineData(10, 19)]
        [InlineData(19, 9)]
        public void When_a_nullable_int_difference_is_not_in_difference_threshold_range_it_should_not_add_difference_message(int? value, int expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected} because we want to test the failure message, but found {value}.");
        }

        [Fact]
        public void When_a_int_is_compared_to_null_it_should_not_add_difference_message()
        {
            // Arrange
            int? value = null;
            const int expected = 12;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected} because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void When_a_null_is_compared_to_value_it_should_not_add_difference_message()
        {
            // Arrange
            const int value = 12;
            int? nullableValue = null;

            // Act
            Action act = () =>
                value.Should().Be(nullableValue, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be <null> because we want to test the failure message, but found 12.");
        }

        [Theory]
        [InlineData(50, 20)]
        [InlineData(20, 50)]
        [InlineData(123, -123)]
        [InlineData(-123, 123)]
        public void When_a_nullable_int_difference_is_in_difference_threshold_range_it_should_add_difference_message(int? value, int expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected} because we want to test the failure message, but found {value} (difference of {value - expected}).");
        }

        [Theory]
        [InlineData(10, 19)]
        [InlineData(19, 9)]
        public void When_a_long_difference_is_not_in_difference_threshold_range_it_should_not_add_difference_message(long value, long expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}L because we want to test the failure message, but found {value}L.");
        }

        [Theory]
        [InlineData(50, 20)]
        [InlineData(20, 50)]
        public void When_a_long_difference_is_in_difference_threshold_range_it_should_add_difference_message(long value, long expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}L because we want to test the failure message, but found {value}L (difference of {value - expected}).");
        }

        [Theory]
        [InlineData(10, 19)]
        [InlineData(19, 9)]
        public void When_a_nullable_long_difference_is_not_in_difference_threshold_range_it_should_not_add_difference_message(long? value, long expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}L because we want to test the failure message, but found {value}L.");
        }

        [Theory]
        [InlineData(50, 20)]
        [InlineData(20, 50)]
        public void When_a_nullable_long_difference_is_in_difference_threshold_range_it_should_add_difference_message(long? value, long expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}L because we want to test the failure message, but found {value}L (difference of {value - expected}).");
        }

        [Theory]
        [InlineData(10, 19)]
        [InlineData(19, 9)]
        public void When_a_short_difference_is_not_in_difference_threshold_range_it_should_not_add_difference_message(short value, short expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}s because we want to test the failure message, but found {value}s.");
        }

        [Theory]
        [InlineData(50, 20)]
        [InlineData(20, 50)]
        public void When_a_short_difference_is_in_difference_threshold_range_it_should_add_difference_message(short value, short expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}s because we want to test the failure message, but found {value}s (difference of {value - expected}).");
        }

        [Fact]
        public void When_a_nullable_short_difference_is_not_in_difference_threshold_range_it_should_not_add_difference_message()
        {
            // Arrange
            short? value = 2;
            const short expected = 1;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}s because we want to test the failure message, but found {value}s.");
        }

        [Fact]
        public void When_a_nullable_short_difference_is_in_difference_threshold_range_it_should_add_difference_message()
        {
            // Arrange
            short? value = 15;
            const short expected = 2;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}s because we want to test the failure message, but found {value}s (difference of {value - expected}).");
        }

        [Fact]
        public void When_a_ulong_difference_is_not_in_difference_threshold_range_it_should_not_add_difference_message()
        {
            // Arrange
            const ulong value = 19;
            const ulong expected = 10;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}UL because we want to test the failure message, but found {value}UL.");
        }

        [Theory]
        [InlineData(50, 20)]
        [InlineData(20, 50)]
        public void When_a_ulong_difference_is_in_difference_threshold_range_it_should_add_difference_message(ulong value, ulong expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}UL because we want to test the failure message, but found {value}UL (difference of {value - expected}).");
        }

        [Fact]
        public void When_a_nullable_ulong_difference_is_not_in_difference_threshold_range_it_should_not_add_difference_message()
        {
            // Arrange
            ulong? value = 15;
            const ulong expected = 12;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}UL because we want to test the failure message, but found {value}UL.");
        }

        [Fact]
        public void When_a_nullable_ulong_difference_is_in_difference_threshold_range_it_should_add_difference_message()
        {
            // Arrange
            ulong? value = 50;
            const ulong expected = 20;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}UL because we want to test the failure message, but found {value}UL (difference of {value - expected}).");
        }

        [Fact]
        public void When_a_nullable_ushort_difference_is_in_difference_threshold_range_it_should_add_difference_message()
        {
            // Arrange
            ushort? value = 15;
            const ushort expected = 2;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}us because we want to test the failure message, but found {value}us (difference of {value - expected}).");
        }

        [Fact]
        public void When_a_double_difference_is_in_difference_threshold_range_it_should_add_difference_message()
        {
            // Arrange
            const double value = 1.5;
            const double expected = 1;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be 1.0 because we want to test the failure message, but found {value} (difference of {value - expected}).");
        }

        [Fact]
        public void When_a_nullable_double_difference_is_in_difference_threshold_range_it_should_add_difference_message()
        {
            // Arrange
            double? value = 1.5;
            const double expected = 1;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be 1.0 because we want to test the failure message, but found {value} (difference of {value - expected}).");
        }

        [Fact]
        public void When_a_float_difference_is_in_difference_threshold_range_it_should_add_difference_message()
        {
            // Arrange
            const float value = 1.5F;
            const float expected = 1;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be 1F because we want to test the failure message, but found 1.5F (difference of {value - expected}).");
        }

        [Fact]
        public void When_a_nullable_float_difference_is_in_difference_threshold_range_it_should_add_difference_message()
        {
            // Arrange
            float? value = 1.5F;
            const float expected = 1;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be 1F because we want to test the failure message, but found 1.5F (difference of {value - expected}).");
        }

        [Fact]
        public void When_a_decimal_difference_is_in_difference_threshold_range_it_should_add_difference_message()
        {
            // Arrange
            const decimal value = 1.5m;
            const decimal expected = 1;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be 1m because we want to test the failure message, but found 1.5m (difference of {value - expected}).");
        }

        [Fact]
        public void When_a_nullable_decimal_difference_is_in_difference_threshold_range_it_should_add_difference_message()
        {
            // Arrange
            decimal? value = 1.5m;
            const decimal expected = 1;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be 1m because we want to test the failure message, but found 1.5m (difference of {value - expected}).");
        }

        [Fact]
        public void When_a_sbyte_difference_is_in_difference_threshold_range_it_should_add_difference_message()
        {
            // Arrange
            const sbyte value = 1;
            const sbyte expected = 3;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}y because we want to test the failure message, but found {value}y (difference of {value - expected}).");
        }

        [Fact]
        public void When_a_nullable_sbyte_difference_is_in_difference_threshold_range_it_should_add_difference_message()
        {
            // Arrange
            sbyte? value = 1;
            const sbyte expected = 3;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"Expected value to be {expected}y because we want to test the failure message, but found {value}y (difference of {value - expected}).");
        }
    }
}
