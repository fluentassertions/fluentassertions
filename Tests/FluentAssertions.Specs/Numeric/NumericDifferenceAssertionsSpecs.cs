using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Numeric
{
    public class NumericDifferenceAssertionsSpecs
    {
        public class Be
        {
            [Theory]
            [InlineData(8, 5)]
            [InlineData(1, 9)]
            public void When_an_int_difference_between_small_numbers_it_should_not_add_difference_message(int value, int expected)
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
            public void When_an_int_difference_between_large_numbers_it_should_add_difference_message(int value, int expected)
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
            [InlineData(8, 5)]
            [InlineData(1, 9)]
            public void When_a_nullable_int_difference_between_small_numbers_it_should_not_add_difference_message(int? value, int expected)
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
            public void When_a_nullable_int_difference_between_large_numbers_it_should_add_difference_message(int? value, int expected)
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
            [InlineData(8, 5)]
            [InlineData(1, 9)]
            public void When_a_long_difference_between_small_numbers_it_should_not_add_difference_message(long value, long expected)
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
            public void When_a_long_difference_between_large_numbers_it_should_add_difference_message(long value, long expected)
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
            [InlineData(8, 5)]
            [InlineData(1, 9)]
            public void When_a_nullable_long_difference_between_small_numbers_it_should_not_add_difference_message(long? value, long expected)
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
            public void When_a_nullable_long_difference_between_large_numbers_it_should_add_difference_message(long? value, long expected)
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
            [InlineData(8, 5)]
            [InlineData(1, 9)]
            public void When_a_short_difference_between_small_numbers_it_should_not_add_difference_message(short value, short expected)
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
            public void When_a_short_difference_between_large_numbers_it_should_add_difference_message(short value, short expected)
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
            public void When_a_nullable_short_difference_between_small_numbers_it_should_not_add_difference_message()
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
            public void When_a_nullable_short_difference_between_large_numbers_it_should_add_difference_message()
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
            public void When_a_ulong_difference_between_small_numbers_it_should_not_add_difference_message()
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
                    .WithMessage($"Expected value to be {expected}UL because we want to test the failure message, but found {value}UL.");
            }

            [Theory]
            [InlineData(50, 20)]
            [InlineData(20, 50)]
            public void When_a_ulong_difference_between_large_numbers_it_should_add_difference_message(ulong value, ulong expected)
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
            public void When_a_nullable_ulong_difference_between_small_numbers_it_should_not_add_difference_message()
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
                    .WithMessage($"Expected value to be {expected}UL because we want to test the failure message, but found {value}UL.");
            }

            [Fact]
            public void When_a_nullable_ulong_difference_between_large_numbers_it_should_add_difference_message()
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
            public void When_a_nullable_ushort_difference_between_large_numbers_it_should_add_difference_message()
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
                    .WithMessage($"Expected value to be {expected}us because we want to test the failure message, but found {value}us (difference of {value - expected}).");
            }

            [Fact]
            public void When_a_double_difference_between_large_numbers_it_should_add_difference_message()
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
            public void When_a_nullable_double_difference_between_large_numbers_it_should_add_difference_message()
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
            public void When_a_float_difference_between_large_numbers_it_should_add_difference_message()
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
            public void When_a_nullable_float_difference_between_large_numbers_it_should_add_difference_message()
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
            public void When_a_decimal_difference_between_large_numbers_it_should_add_difference_message()
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
            public void When_a_nullable_decimal_difference_between_large_numbers_it_should_add_difference_message()
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
            public void When_a_sbyte_difference_between_large_numbers_it_should_add_difference_message()
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
            public void When_a_nullable_sbyte_difference_between_large_numbers_it_should_add_difference_message()
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

        public class BeLessThan
        {
            [Fact]
            public void When_an_int_value_is_equal_to_expected_value_it_should_not_add_difference_message()
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
            public void When_an_int_value_is_less_than_expected_value_with_small_numbers_it_should_not_add_difference_message()
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
            public void When_an_int_value_is_less_than_expected_value_with_large_numbers_it_should_add_difference_message()
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
                    .WithMessage("Expected value to be less than 22 because we want to test the failure message, but found 52 (difference of 30).");
            }

            [Fact]
            public void When_a_float_value_is_equal_to_expected_value_it_should_not_add_difference_message()
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
                    .WithMessage("Expected value to be greater than 2.3F because we want to test the failure message, but found 2.3F.");
            }

            [Fact]
            public void When_a_ushort_value_is_equal_to_expected_value_it_should_not_add_difference_message()
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
                    .WithMessage("Expected value to be greater than 11us because we want to test the failure message, but found 11us.");
            }

            [Fact]
            public void When_a_sbyte_value_is_equal_to_expected_value_it_should_not_add_difference_message()
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
        }

        public class BeLessThanOrEqualTo
        {
            [Fact]
            public void When_an_int_value_is_less_than_expected_value_with_small_numbers_it_should_not_add_difference_message()
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
                    .WithMessage("Expected value to be less than or equal to 2 because we want to test the failure message, but found 4.");
            }

            [Fact]
            public void When_an_int_value_is_less_than_expected_value_with_large_numbers_it_should_add_difference_message()
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
                    .WithMessage("Expected value to be less than or equal to 22 because we want to test the failure message, but found 52 (difference of 30).");
            }
        }

        public class BeGreaterThan
        {
            [Fact]
            public void When_an_int_value_is_equal_to_expected_value_it_should_not_add_difference_message()
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
            public void When_an_int_value_is_greater_than_expected_value_with_small_numbers_it_should_not_add_difference_message()
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
            public void When_an_int_value_is_greater_than_expected_value_with_large_numbers_it_should_add_difference_message()
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
                    .WithMessage("Expected value to be greater than 52 because we want to test the failure message, but found 22 (difference of -30).");
            }

            [Fact]
            public void When_a_double_value_is_equal_to_expected_value_it_should_not_add_difference_message()
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
        }

        public class BeGreaterThanOrEqualTo
        {
            [Fact]
            public void When_an_int_value_is_greater_than_expected_value_with_small_numbers_it_should_not_add_difference_message()
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
                    .WithMessage("Expected value to be greater than or equal to 4 because we want to test the failure message, but found 2.");
            }

            [Fact]
            public void When_an_int_value_is_greater_than_expected_value_with_large_numbers_it_should_add_difference_message()
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
                    .WithMessage("Expected value to be greater than or equal to 52 because we want to test the failure message, but found 22 (difference of -30).");
            }
        }
    }
}
