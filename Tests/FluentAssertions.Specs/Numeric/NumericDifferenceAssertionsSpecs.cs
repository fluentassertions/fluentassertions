using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public class NumericDifferenceAssertionsSpecs
{
    public class Be
    {
        [Theory]
        [InlineData(8, 5)]
        [InlineData(1, 9)]
        public void The_difference_between_small_ints_is_not_included_in_the_message(int value, int expected)
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
        [InlineData(50, 20, 30)]
        [InlineData(20, 50, -30)]
        [InlineData(123, -123, 246)]
        [InlineData(-123, 123, -246)]
        public void The_difference_between_ints_is_included_in_the_message(int value, int expected, int expectedDifference)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    $"Expected value to be {expected} because we want to test the failure message, but found {value} (difference of {expectedDifference}).");
        }

        [Theory]
        [InlineData(8, 5)]
        [InlineData(1, 9)]
        public void The_difference_between_small_nullable_ints_is_not_included_in_the_message(int? value, int expected)
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
        public void The_difference_between_int_and_null_is_not_included_in_the_message()
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
                .WithMessage("Expected value to be 12 because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void The_difference_between_null_and_int_is_not_included_in_the_message()
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
        [InlineData(50, 20, 30)]
        [InlineData(20, 50, -30)]
        [InlineData(123, -123, 246)]
        [InlineData(-123, 123, -246)]
        public void The_difference_between_nullable_ints_is_included_in_the_message(int? value, int expected,
            int expectedDifference)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    $"Expected value to be {expected} because we want to test the failure message, but found {value} (difference of {expectedDifference}).");
        }

        [Fact]
        public void The_difference_between_nullable_uints_is_included_in_the_message()
        {
            // Arrange
            uint? value = 29;
            const uint expected = 19;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be 19u because we want to test the failure message, but found 29u (difference of 10).");
        }

        [Theory]
        [InlineData(8, 5)]
        [InlineData(1, 9)]
        public void The_difference_between_small_longs_is_not_included_in_the_message(long value, long expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    $"Expected value to be {expected}L because we want to test the failure message, but found {value}L.");
        }

        [Theory]
        [InlineData(50, 20, 30)]
        [InlineData(20, 50, -30)]
        public void The_difference_between_longs_is_included_in_the_message(long value, long expected, long expectedDifference)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    $"Expected value to be {expected}L because we want to test the failure message, but found {value}L (difference of {expectedDifference}).");
        }

        [Theory]
        [InlineData(8L, 5)]
        [InlineData(1L, 9)]
        public void The_difference_between_small_nullable_longs_is_not_included_in_the_message(long? value, long expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    $"Expected value to be {expected}L because we want to test the failure message, but found {value}L.");
        }

        [Theory]
        [InlineData(50L, 20, 30)]
        [InlineData(20L, 50, -30)]
        public void The_difference_between_nullable_longs_is_included_in_the_message(long? value, long expected,
            long expectedDifference)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    $"Expected value to be {expected}L because we want to test the failure message, but found {value}L (difference of {expectedDifference}).");
        }

        [Theory]
        [InlineData(8, 5)]
        [InlineData(1, 9)]
        public void The_difference_between_small_shorts_is_not_included_in_the_message(short value, short expected)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    $"Expected value to be {expected}s because we want to test the failure message, but found {value}s.");
        }

        [Theory]
        [InlineData(50, 20, 30)]
        [InlineData(20, 50, -30)]
        public void The_difference_between_shorts_is_included_in_the_message(short value, short expected,
            short expectedDifference)
        {
            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    $"Expected value to be {expected}s because we want to test the failure message, but found {value}s (difference of {expectedDifference}).");
        }

        [Fact]
        public void The_difference_between_small_nullable_shorts_is_not_included_in_the_message()
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
                .WithMessage("Expected value to be 1s because we want to test the failure message, but found 2s.");
        }

        [Fact]
        public void The_difference_between_nullable_shorts_is_included_in_the_message()
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
                .WithMessage(
                    "Expected value to be 2s because we want to test the failure message, but found 15s (difference of 13).");
        }

        [Fact]
        public void The_difference_between_small_ulongs_is_not_included_in_the_message()
        {
            // Arrange
            const ulong value = 9;
            const ulong expected = 4;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be 4UL because we want to test the failure message, but found 9UL.");
        }

        [Fact]
        public void The_difference_between_ulongs_is_included_in_the_message()
        {
            // Arrange
            const ulong value = 50;
            const ulong expected = 20;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be 20UL because we want to test the failure message, but found 50UL (difference of 30).");
        }

        [Fact]
        public void The_difference_between_small_nullable_ulongs_is_not_included_in_the_message()
        {
            // Arrange
            ulong? value = 7;
            const ulong expected = 4;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be 4UL because we want to test the failure message, but found 7UL.");
        }

        [Fact]
        public void The_difference_between_nullable_ulongs_is_included_in_the_message()
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
                .WithMessage(
                    "Expected value to be 20UL because we want to test the failure message, but found 50UL (difference of 30).");
        }

        [Fact]
        public void The_difference_between_ushorts_is_included_in_the_message()
        {
            // Arrange
            ushort? value = 11;
            const ushort expected = 2;

            // Act
            Action act = () =>
                value.Should().Be(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be 2us because we want to test the failure message, but found 11us (difference of 9).");
        }

        [Fact]
        public void The_difference_between_doubles_is_included_in_the_message()
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
                .WithMessage(
                    "Expected value to be 1.0 because we want to test the failure message, but found 1.5 (difference of 0.5).");
        }

        [Fact]
        public void The_difference_between_nullable_doubles_is_included_in_the_message()
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
                .WithMessage(
                    "Expected value to be 1.0 because we want to test the failure message, but found 1.5 (difference of 0.5).");
        }

        [Fact]
        public void The_difference_between_floats_is_included_in_the_message()
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
                .WithMessage(
                    "Expected value to be 1F because we want to test the failure message, but found 1.5F (difference of 0.5).");
        }

        [Fact]
        public void The_difference_between_nullable_floats_is_included_in_the_message()
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
                .WithMessage(
                    "Expected value to be 1F because we want to test the failure message, but found 1.5F (difference of 0.5).");
        }

        [Fact]
        public void The_difference_between_decimals_is_included_in_the_message()
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
                .WithMessage(
                    "Expected value to be 1m because we want to test the failure message, but found 1.5m (difference of 0.5).");
        }

        [Fact]
        public void The_difference_between_nullable_decimals_is_included_in_the_message()
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
                .WithMessage(
                    "Expected value to be 1m because we want to test the failure message, but found 1.5m (difference of 0.5).");
        }

        [Fact]
        public void The_difference_between_sbytes_is_included_in_the_message()
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
                .WithMessage(
                    "Expected value to be 3y because we want to test the failure message, but found 1y (difference of -2).");
        }

        [Fact]
        public void The_difference_between_nullable_sbytes_is_included_in_the_message()
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
                .WithMessage(
                    "Expected value to be 3y because we want to test the failure message, but found 1y (difference of -2).");
        }
    }

    public class BeLessThan
    {
        [Fact]
        public void The_difference_between_equal_ints_is_not_included_in_the_message()
        {
            // Arrange
            const int value = 15;
            const int expected = 15;

            // Act
            Action act = () =>
                value.Should().BeLessThan(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be less than 15 because we want to test the failure message, but found 15.");
        }

        [Fact]
        public void The_difference_between_small_ints_is_not_included_in_the_message()
        {
            // Arrange
            const int value = 4;
            const int expected = 2;

            // Act
            Action act = () =>
                value.Should().BeLessThan(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be less than 2 because we want to test the failure message, but found 4.");
        }

        [Fact]
        public void The_difference_between_ints_is_included_in_the_message()
        {
            // Arrange
            const int value = 52;
            const int expected = 22;

            // Act
            Action act = () =>
                value.Should().BeLessThan(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be less than 22 because we want to test the failure message, but found 52 (difference of 30).");
        }
    }

    public class BeLessThanOrEqualTo
    {
        [Fact]
        public void The_difference_between_small_ints_is_not_included_in_the_message()
        {
            // Arrange
            const int value = 4;
            const int expected = 2;

            // Act
            Action act = () =>
                value.Should().BeLessThanOrEqualTo(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be less than or equal to 2 because we want to test the failure message, but found 4.");
        }

        [Fact]
        public void The_difference_between_ints_is_included_in_the_message()
        {
            // Arrange
            const int value = 52;
            const int expected = 22;

            // Act
            Action act = () =>
                value.Should().BeLessThanOrEqualTo(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be less than or equal to 22 because we want to test the failure message, but found 52 (difference of 30).");
        }
    }

    public class BeGreaterThan
    {
        [Fact]
        public void The_difference_between_equal_ints_is_not_included_in_the_message()
        {
            // Arrange
            const int value = 15;
            const int expected = 15;

            // Act
            Action act = () =>
                value.Should().BeGreaterThan(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be greater than 15 because we want to test the failure message, but found 15.");
        }

        [Fact]
        public void The_difference_between_small_ints_is_not_included_in_the_message()
        {
            // Arrange
            const int value = 2;
            const int expected = 4;

            // Act
            Action act = () =>
                value.Should().BeGreaterThan(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be greater than 4 because we want to test the failure message, but found 2.");
        }

        [Fact]
        public void The_difference_between_ints_is_included_in_the_message()
        {
            // Arrange
            const int value = 22;
            const int expected = 52;

            // Act
            Action act = () =>
                value.Should().BeGreaterThan(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be greater than 52 because we want to test the failure message, but found 22 (difference of -30).");
        }

        [Fact]
        public void The_difference_between_equal_nullable_uints_is_not_included_in_the_message()
        {
            // Arrange
            uint? value = 15;
            const uint expected = 15;

            // Act
            Action act = () =>
                value.Should().BeGreaterThan(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be greater than 15u because we want to test the failure message, but found 15u.");
        }

        [Fact]
        public void The_difference_between_equal_doubles_is_not_included_in_the_message()
        {
            // Arrange
            const double value = 1.3;
            const double expected = 1.3;

            // Act
            Action act = () =>
                value.Should().BeGreaterThan(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be greater than 1.3 because we want to test the failure message, but found 1.3.");
        }

        [Fact]
        public void The_difference_between_equal_floats_is_not_included_in_the_message()
        {
            // Arrange
            const float value = 2.3F;
            const float expected = 2.3F;

            // Act
            Action act = () =>
                value.Should().BeGreaterThan(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be greater than 2.3F because we want to test the failure message, but found 2.3F.");
        }

        [Fact]
        public void The_difference_between_equal_ushorts_is_not_included_in_the_message()
        {
            // Arrange
            ushort? value = 11;
            const ushort expected = 11;

            // Act
            Action act = () =>
                value.Should().BeGreaterThan(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be greater than 11us because we want to test the failure message, but found 11us.");
        }

        [Fact]
        public void The_difference_between_equal_sbytes_is_not_included_in_the_message()
        {
            // Arrange
            const sbyte value = 3;
            const sbyte expected = 3;

            // Act
            Action act = () =>
                value.Should().BeGreaterThan(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be greater than 3y because we want to test the failure message, but found 3y.");
        }

        [Fact]
        public void The_difference_between_equal_nullable_ulongs_is_not_included_in_the_message()
        {
            // Arrange
            ulong? value = 15;
            const ulong expected = 15;

            // Act
            Action act = () =>
                value.Should().BeGreaterThan(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be greater than 15UL because we want to test the failure message, but found 15UL.");
        }
    }

    public class BeGreaterThanOrEqualTo
    {
        [Fact]
        public void The_difference_between_small_ints_is_not_included_in_the_message()
        {
            // Arrange
            const int value = 2;
            const int expected = 4;

            // Act
            Action act = () =>
                value.Should().BeGreaterThanOrEqualTo(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be greater than or equal to 4 because we want to test the failure message, but found 2.");
        }

        [Fact]
        public void The_difference_between_ints_is_included_in_the_message()
        {
            // Arrange
            const int value = 22;
            const int expected = 52;

            // Act
            Action act = () =>
                value.Should().BeGreaterThanOrEqualTo(expected, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be greater than or equal to 52 because we want to test the failure message, but found 22 (difference of -30).");
        }
    }

    public class Overflow
    {
        [Fact]
        public void The_difference_between_overflowed_ints_is_included_in_the_message()
        {
            // Act
            Action act = () =>
                int.MinValue.Should().Be(int.MaxValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected int.MinValue to be 2147483647*found -2147483648 (difference of -4294967295).");
        }

        [Fact]
        public void The_difference_between_overflowed_nullable_ints_is_included_in_the_message()
        {
            // Arrange
            int? minValue = int.MinValue;

            // Act
            Action act = () =>
                minValue.Should().Be(int.MaxValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected minValue to be 2147483647*found -2147483648 (difference of -4294967295).");
        }

        [Fact]
        public void The_difference_between_overflowed_uints_is_included_in_the_message()
        {
            // Act
            Action act = () =>
                uint.MinValue.Should().Be(uint.MaxValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected uint.MinValue to be 4294967295u*found 0u (difference of -4294967295).");
        }

        [Fact]
        public void The_difference_between_overflowed_nullable_uints_is_included_in_the_message()
        {
            // Arrange
            uint? minValue = uint.MinValue;

            // Act
            Action act = () =>
                minValue.Should().Be(uint.MaxValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected minValue to be 4294967295u*found 0u (difference of -4294967295).");
        }

        [Fact]
        public void The_difference_between_overflowed_longs_is_included_in_the_message()
        {
            // Act
            Action act = () =>
                long.MinValue.Should().Be(long.MaxValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected long.MinValue to be 9223372036854775807L*found -9223372036854775808L (difference of -18446744073709551615).");
        }

        [Fact]
        public void The_difference_between_overflowed_nullable_longs_is_included_in_the_message()
        {
            // Arrange
            long? minValue = long.MinValue;

            // Act
            Action act = () =>
                minValue.Should().Be(long.MaxValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected minValue to be 9223372036854775807L*found -9223372036854775808L (difference of -18446744073709551615).");
        }

        [Fact]
        public void The_difference_between_overflowed_ulongs_is_included_in_the_message()
        {
            // Act
            Action act = () =>
                ulong.MinValue.Should().Be(ulong.MaxValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected ulong.MinValue to be 18446744073709551615UL*found 0UL (difference of -18446744073709551615).");
        }

        [Fact]
        public void The_difference_between_overflowed_nullable_ulongs_is_included_in_the_message()
        {
            // Arrange
            ulong? minValue = ulong.MinValue;

            // Act
            Action act = () =>
                minValue.Should().Be(ulong.MaxValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected minValue to be 18446744073709551615UL*found 0UL (difference of -18446744073709551615).");
        }

        [Fact]
        public void The_difference_between_overflowed_decimals_is_not_included_in_the_message()
        {
            // Act
            Action act = () =>
                decimal.MinValue.Should().Be(decimal.MaxValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected decimal.MinValue to be 79228162514264337593543950335M*found -79228162514264337593543950335M.");
        }

        [Fact]
        public void The_difference_between_overflowed_nullable_decimals_is_not_included_in_the_message()
        {
            // Arrange
            decimal? minValue = decimal.MinValue;

            // Act
            Action act = () =>
                minValue.Should().Be(decimal.MaxValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected minValue to be 79228162514264337593543950335M*found -79228162514264337593543950335M.");
        }
    }
}
