using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class BeCloseTo
    {
        [InlineData(sbyte.MinValue, sbyte.MinValue, 0)]
        [InlineData(sbyte.MinValue, sbyte.MinValue, 1)]
        [InlineData(sbyte.MinValue, sbyte.MinValue, sbyte.MaxValue)]
        [InlineData(sbyte.MinValue, sbyte.MinValue + 1, 1)]
        [InlineData(sbyte.MinValue, sbyte.MinValue + 1, sbyte.MaxValue)]
        [InlineData(sbyte.MinValue, -1, sbyte.MaxValue)]
        [InlineData(sbyte.MinValue + 1, sbyte.MinValue, 1)]
        [InlineData(sbyte.MinValue + 1, sbyte.MinValue, sbyte.MaxValue)]
        [InlineData(sbyte.MinValue + 1, 0, sbyte.MaxValue)]
        [InlineData(-1, sbyte.MinValue, sbyte.MaxValue)]
        [InlineData(-1, 0, 1)]
        [InlineData(-1, 0, sbyte.MaxValue)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, -1, 1)]
        [InlineData(0, -1, sbyte.MaxValue)]
        [InlineData(0, 1, 1)]
        [InlineData(0, 1, sbyte.MaxValue)]
        [InlineData(0, sbyte.MaxValue, sbyte.MaxValue)]
        [InlineData(0, sbyte.MinValue + 1, sbyte.MaxValue)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 0, sbyte.MaxValue)]
        [InlineData(1, sbyte.MaxValue, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue - 1, sbyte.MaxValue, 1)]
        [InlineData(sbyte.MaxValue - 1, sbyte.MaxValue, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue, 0, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue, 1, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue, 0)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue, 1)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue - 1, 1)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue - 1, sbyte.MaxValue)]
        [Theory]
        public void When_a_sbyte_value_is_close_to_expected_value_it_should_succeed(sbyte actual, sbyte nearbyValue,
            byte delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(sbyte.MinValue, sbyte.MaxValue, 1)]
        [InlineData(sbyte.MinValue, 0, sbyte.MaxValue)]
        [InlineData(sbyte.MinValue, 1, sbyte.MaxValue)]
        [InlineData(-1, 0, 0)]
        [InlineData(-1, 1, 1)]
        [InlineData(-1, sbyte.MaxValue, sbyte.MaxValue)]
        [InlineData(0, sbyte.MinValue, sbyte.MaxValue)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 0, 0)]
        [InlineData(1, sbyte.MinValue, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue, sbyte.MinValue, 1)]
        [InlineData(sbyte.MaxValue, -1, sbyte.MaxValue)]
        [Theory]
        public void When_a_sbyte_value_is_not_close_to_expected_value_it_should_fail(sbyte actual, sbyte nearbyValue,
            byte delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_sbyte_value_is_not_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            sbyte actual = 1;
            sbyte nearbyValue = 4;
            byte delta = 2;

            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*4*but found*1*");
        }

        [Fact]
        public void When_a_sbyte_value_is_returned_from_BeCloseTo_it_should_chain()
        {
            // Arrange
            sbyte actual = sbyte.MaxValue;

            // Act
            Action act = () => actual.Should().BeCloseTo(actual, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(short.MinValue, short.MinValue, 0)]
        [InlineData(short.MinValue, short.MinValue, 1)]
        [InlineData(short.MinValue, short.MinValue, short.MaxValue)]
        [InlineData(short.MinValue, short.MinValue + 1, 1)]
        [InlineData(short.MinValue, short.MinValue + 1, short.MaxValue)]
        [InlineData(short.MinValue, -1, short.MaxValue)]
        [InlineData(short.MinValue + 1, short.MinValue, 1)]
        [InlineData(short.MinValue + 1, short.MinValue, short.MaxValue)]
        [InlineData(short.MinValue + 1, 0, short.MaxValue)]
        [InlineData(-1, short.MinValue, short.MaxValue)]
        [InlineData(-1, 0, 1)]
        [InlineData(-1, 0, short.MaxValue)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, -1, 1)]
        [InlineData(0, -1, short.MaxValue)]
        [InlineData(0, 1, 1)]
        [InlineData(0, 1, short.MaxValue)]
        [InlineData(0, short.MaxValue, short.MaxValue)]
        [InlineData(0, short.MinValue + 1, short.MaxValue)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 0, short.MaxValue)]
        [InlineData(1, short.MaxValue, short.MaxValue)]
        [InlineData(short.MaxValue - 1, short.MaxValue, 1)]
        [InlineData(short.MaxValue - 1, short.MaxValue, short.MaxValue)]
        [InlineData(short.MaxValue, 0, short.MaxValue)]
        [InlineData(short.MaxValue, 1, short.MaxValue)]
        [InlineData(short.MaxValue, short.MaxValue, 0)]
        [InlineData(short.MaxValue, short.MaxValue, 1)]
        [InlineData(short.MaxValue, short.MaxValue, short.MaxValue)]
        [InlineData(short.MaxValue, short.MaxValue - 1, 1)]
        [InlineData(short.MaxValue, short.MaxValue - 1, short.MaxValue)]
        [Theory]
        public void When_a_short_value_is_close_to_expected_value_it_should_succeed(short actual, short nearbyValue,
            ushort delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(short.MinValue, short.MaxValue, 1)]
        [InlineData(short.MinValue, 0, short.MaxValue)]
        [InlineData(short.MinValue, 1, short.MaxValue)]
        [InlineData(-1, 0, 0)]
        [InlineData(-1, 1, 1)]
        [InlineData(-1, short.MaxValue, short.MaxValue)]
        [InlineData(0, short.MinValue, short.MaxValue)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 0, 0)]
        [InlineData(1, short.MinValue, short.MaxValue)]
        [InlineData(short.MaxValue, short.MinValue, 1)]
        [InlineData(short.MaxValue, -1, short.MaxValue)]
        [Theory]
        public void When_a_short_value_is_not_close_to_expected_value_it_should_fail(short actual, short nearbyValue,
            ushort delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_short_value_is_not_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            short actual = 1;
            short nearbyValue = 4;
            ushort delta = 2;

            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*4*but found*1*");
        }

        [Fact]
        public void When_a_short_value_is_returned_from_BeCloseTo_it_should_chain()
        {
            // Arrange
            short actual = short.MaxValue;

            // Act
            Action act = () => actual.Should().BeCloseTo(actual, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(int.MinValue, int.MinValue, 0)]
        [InlineData(int.MinValue, int.MinValue, 1)]
        [InlineData(int.MinValue, int.MinValue, int.MaxValue)]
        [InlineData(int.MinValue, int.MinValue + 1, 1)]
        [InlineData(int.MinValue, int.MinValue + 1, int.MaxValue)]
        [InlineData(int.MinValue, -1, int.MaxValue)]
        [InlineData(int.MinValue + 1, int.MinValue, 1)]
        [InlineData(int.MinValue + 1, int.MinValue, int.MaxValue)]
        [InlineData(int.MinValue + 1, 0, int.MaxValue)]
        [InlineData(-1, int.MinValue, int.MaxValue)]
        [InlineData(-1, 0, 1)]
        [InlineData(-1, 0, int.MaxValue)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, -1, 1)]
        [InlineData(0, -1, int.MaxValue)]
        [InlineData(0, 1, 1)]
        [InlineData(0, 1, int.MaxValue)]
        [InlineData(0, int.MaxValue, int.MaxValue)]
        [InlineData(0, int.MinValue + 1, int.MaxValue)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 0, int.MaxValue)]
        [InlineData(1, int.MaxValue, int.MaxValue)]
        [InlineData(int.MaxValue - 1, int.MaxValue, 1)]
        [InlineData(int.MaxValue - 1, int.MaxValue, int.MaxValue)]
        [InlineData(int.MaxValue, 0, int.MaxValue)]
        [InlineData(int.MaxValue, 1, int.MaxValue)]
        [InlineData(int.MaxValue, int.MaxValue, 0)]
        [InlineData(int.MaxValue, int.MaxValue, 1)]
        [InlineData(int.MaxValue, int.MaxValue, int.MaxValue)]
        [InlineData(int.MaxValue, int.MaxValue - 1, 1)]
        [InlineData(int.MaxValue, int.MaxValue - 1, int.MaxValue)]
        [Theory]
        public void When_an_int_value_is_close_to_expected_value_it_should_succeed(int actual, int nearbyValue, uint delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(int.MinValue, int.MaxValue, 1)]
        [InlineData(int.MinValue, 0, int.MaxValue)]
        [InlineData(int.MinValue, 1, int.MaxValue)]
        [InlineData(-1, 0, 0)]
        [InlineData(-1, 1, 1)]
        [InlineData(-1, int.MaxValue, int.MaxValue)]
        [InlineData(0, int.MinValue, int.MaxValue)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 0, 0)]
        [InlineData(1, int.MinValue, int.MaxValue)]
        [InlineData(int.MaxValue, int.MinValue, 1)]
        [InlineData(int.MaxValue, -1, int.MaxValue)]
        [Theory]
        public void When_an_int_value_is_not_close_to_expected_value_it_should_fail(int actual, int nearbyValue, uint delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_an_int_value_is_not_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            int actual = 1;
            int nearbyValue = 4;
            uint delta = 2;

            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*4*but found*1*");
        }

        [Fact]
        public void When_an_int_value_is_returned_from_BeCloseTo_it_should_chain()
        {
            // Arrange
            int actual = int.MaxValue;

            // Act
            Action act = () => actual.Should().BeCloseTo(actual, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(long.MinValue, long.MinValue, 0)]
        [InlineData(long.MinValue, long.MinValue, 1)]
        [InlineData(long.MinValue, long.MinValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MinValue, long.MinValue, ulong.MaxValue / 2)]
        [InlineData(long.MinValue, long.MinValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MinValue, long.MinValue, ulong.MaxValue)]
        [InlineData(long.MinValue, long.MinValue + 1, 1)]
        [InlineData(long.MinValue, long.MinValue + 1, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MinValue, long.MinValue + 1, ulong.MaxValue / 2)]
        [InlineData(long.MinValue, long.MinValue + 1, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MinValue, long.MinValue + 1, ulong.MaxValue)]
        [InlineData(long.MinValue, -1, long.MaxValue)]
        [InlineData(long.MinValue + 1, long.MinValue, 1)]
        [InlineData(long.MinValue + 1, long.MinValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MinValue + 1, long.MinValue, ulong.MaxValue / 2)]
        [InlineData(long.MinValue + 1, long.MinValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MinValue + 1, long.MinValue, ulong.MaxValue)]
        [InlineData(long.MinValue + 1, 0, ulong.MaxValue / 2)]
        [InlineData(long.MinValue + 1, 0, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MinValue + 1, 0, ulong.MaxValue)]
        [InlineData(long.MinValue, long.MaxValue, ulong.MaxValue)]
        [InlineData(-1, long.MinValue, ulong.MaxValue / 2)]
        [InlineData(-1, long.MinValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(-1, long.MinValue, ulong.MaxValue)]
        [InlineData(-1, 0, 1)]
        [InlineData(-1, 0, (ulong.MaxValue / 2) - 1)]
        [InlineData(-1, 0, ulong.MaxValue / 2)]
        [InlineData(-1, 0, (ulong.MaxValue / 2) + 1)]
        [InlineData(-1, 0, ulong.MaxValue)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, -1, 1)]
        [InlineData(0, -1, (ulong.MaxValue / 2) - 1)]
        [InlineData(0, -1, ulong.MaxValue / 2)]
        [InlineData(0, -1, (ulong.MaxValue / 2) + 1)]
        [InlineData(0, -1, ulong.MaxValue)]
        [InlineData(0, 1, 1)]
        [InlineData(0, 1, (ulong.MaxValue / 2) - 1)]
        [InlineData(0, 1, ulong.MaxValue / 2)]
        [InlineData(0, 1, (ulong.MaxValue / 2) + 1)]
        [InlineData(0, 1, ulong.MaxValue)]
        [InlineData(0, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(0, long.MaxValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(0, long.MaxValue, ulong.MaxValue)]
        [InlineData(0, long.MinValue + 1, ulong.MaxValue / 2)]
        [InlineData(0, long.MinValue + 1, (ulong.MaxValue / 2) + 1)]
        [InlineData(0, long.MinValue + 1, ulong.MaxValue)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 0, (ulong.MaxValue / 2) - 1)]
        [InlineData(1, 0, ulong.MaxValue / 2)]
        [InlineData(1, 0, (ulong.MaxValue / 2) + 1)]
        [InlineData(1, 0, ulong.MaxValue)]
        [InlineData(1, long.MaxValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(1, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(1, long.MaxValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(1, long.MaxValue, ulong.MaxValue)]
        [InlineData(long.MaxValue - 1, long.MaxValue, 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue - 1, long.MaxValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, ulong.MaxValue)]
        [InlineData(long.MaxValue, 0, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, 0, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MaxValue, 0, ulong.MaxValue)]
        [InlineData(long.MaxValue, 1, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MaxValue, 1, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, 1, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MaxValue, 1, ulong.MaxValue)]
        [InlineData(long.MaxValue, long.MaxValue, 0)]
        [InlineData(long.MaxValue, long.MaxValue, 1)]
        [InlineData(long.MaxValue, long.MaxValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MaxValue, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, long.MaxValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MaxValue, long.MaxValue, ulong.MaxValue)]
        [InlineData(long.MaxValue, long.MaxValue - 1, 1)]
        [InlineData(long.MaxValue, long.MaxValue - 1, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MaxValue, long.MaxValue - 1, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, long.MaxValue - 1, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MaxValue, long.MaxValue - 1, ulong.MaxValue)]
        [Theory]
        public void When_a_long_value_is_close_to_expected_value_it_should_succeed(long actual, long nearbyValue, ulong delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(long.MinValue, long.MaxValue, 1)]
        [InlineData(long.MinValue, 0, long.MaxValue)]
        [InlineData(long.MinValue, 1, long.MaxValue)]
        [InlineData(long.MinValue + 1, 0, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MinValue, long.MaxValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MinValue, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(-1, 0, 0)]
        [InlineData(-1, 1, 1)]
        [InlineData(-1, long.MaxValue, long.MaxValue)]
        [InlineData(-1, long.MinValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(0, long.MinValue, long.MaxValue)]
        [InlineData(0, long.MinValue + 1, (ulong.MaxValue / 2) - 1)]
        [InlineData(0, long.MaxValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 0, 0)]
        [InlineData(1, long.MinValue, long.MaxValue)]
        [InlineData(long.MaxValue, long.MinValue, 1)]
        [InlineData(long.MaxValue, -1, long.MaxValue)]
        [InlineData(long.MaxValue, 0, (ulong.MaxValue / 2) - 1)]
        [Theory]
        public void When_a_long_value_is_not_close_to_expected_value_it_should_fail(long actual, long nearbyValue,
            ulong delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_long_value_is_not_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            long actual = 1;
            long nearbyValue = 4;
            ulong delta = 2;

            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*4*but found*1*");
        }

        [Fact]
        public void When_a_long_value_is_returned_from_BeCloseTo_it_should_chain()
        {
            // Arrange
            long actual = long.MaxValue;

            // Act
            Action act = () => actual.Should().BeCloseTo(actual, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(1, byte.MaxValue, byte.MaxValue)]
        [InlineData(byte.MinValue, byte.MinValue + 1, byte.MaxValue)]
        [InlineData(byte.MinValue + 1, 0, byte.MaxValue)]
        [InlineData(byte.MinValue + 1, byte.MinValue, 1)]
        [InlineData(byte.MinValue + 1, byte.MinValue, byte.MaxValue)]
        [InlineData(byte.MaxValue - 1, byte.MaxValue, 1)]
        [InlineData(byte.MaxValue - 1, byte.MaxValue, byte.MaxValue)]
        [InlineData(byte.MaxValue, 0, byte.MaxValue)]
        [InlineData(byte.MaxValue, 1, byte.MaxValue)]
        [InlineData(byte.MaxValue, byte.MaxValue - 1, 1)]
        [InlineData(byte.MaxValue, byte.MaxValue - 1, byte.MaxValue)]
        [InlineData(byte.MaxValue, byte.MaxValue, 0)]
        [InlineData(byte.MaxValue, byte.MaxValue, 1)]
        [Theory]
        public void When_a_byte_value_is_close_to_expected_value_it_should_succeed(byte actual, byte nearbyValue, byte delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(byte.MinValue, byte.MaxValue, 1)]
        [InlineData(byte.MaxValue, byte.MinValue, 1)]
        [Theory]
        public void When_a_byte_value_is_not_close_to_expected_value_it_should_fail(byte actual, byte nearbyValue, byte delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_byte_value_is_not_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            byte actual = 1;
            byte nearbyValue = 4;
            byte delta = 2;

            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*4*but found*1*");
        }

        [Fact]
        public void When_a_byte_value_is_returned_from_BeCloseTo_it_should_chain()
        {
            // Arrange
            byte actual = byte.MaxValue;

            // Act
            Action act = () => actual.Should().BeCloseTo(actual, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(1, ushort.MaxValue, ushort.MaxValue)]
        [InlineData(ushort.MinValue, ushort.MinValue + 1, ushort.MaxValue)]
        [InlineData(ushort.MinValue + 1, 0, ushort.MaxValue)]
        [InlineData(ushort.MinValue + 1, ushort.MinValue, 1)]
        [InlineData(ushort.MinValue + 1, ushort.MinValue, ushort.MaxValue)]
        [InlineData(ushort.MaxValue - 1, ushort.MaxValue, 1)]
        [InlineData(ushort.MaxValue - 1, ushort.MaxValue, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, 0, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, 1, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, ushort.MaxValue - 1, 1)]
        [InlineData(ushort.MaxValue, ushort.MaxValue - 1, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, ushort.MaxValue, 0)]
        [InlineData(ushort.MaxValue, ushort.MaxValue, 1)]
        [Theory]
        public void When_an_ushort_value_is_close_to_expected_value_it_should_succeed(ushort actual, ushort nearbyValue,
            ushort delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(ushort.MinValue, ushort.MaxValue, 1)]
        [InlineData(ushort.MaxValue, ushort.MinValue, 1)]
        [Theory]
        public void When_an_ushort_value_is_not_close_to_expected_value_it_should_fail(ushort actual, ushort nearbyValue,
            ushort delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_an_ushort_value_is_not_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            ushort actual = 1;
            ushort nearbyValue = 4;
            ushort delta = 2;

            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*4*but found*1*");
        }

        [Fact]
        public void When_an_ushort_value_is_returned_from_BeCloseTo_it_should_chain()
        {
            // Arrange
            ushort actual = ushort.MaxValue;

            // Act
            Action act = () => actual.Should().BeCloseTo(actual, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(1, uint.MaxValue, uint.MaxValue)]
        [InlineData(uint.MinValue, uint.MinValue + 1, uint.MaxValue)]
        [InlineData(uint.MinValue + 1, 0, uint.MaxValue)]
        [InlineData(uint.MinValue + 1, uint.MinValue, 1)]
        [InlineData(uint.MinValue + 1, uint.MinValue, uint.MaxValue)]
        [InlineData(uint.MaxValue - 1, uint.MaxValue, 1)]
        [InlineData(uint.MaxValue - 1, uint.MaxValue, uint.MaxValue)]
        [InlineData(uint.MaxValue, 0, uint.MaxValue)]
        [InlineData(uint.MaxValue, 1, uint.MaxValue)]
        [InlineData(uint.MaxValue, uint.MaxValue - 1, 1)]
        [InlineData(uint.MaxValue, uint.MaxValue - 1, uint.MaxValue)]
        [InlineData(uint.MaxValue, uint.MaxValue, 0)]
        [InlineData(uint.MaxValue, uint.MaxValue, 1)]
        [Theory]
        public void When_an_uint_value_is_close_to_expected_value_it_should_succeed(uint actual, uint nearbyValue, uint delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(uint.MinValue, uint.MaxValue, 1)]
        [InlineData(uint.MaxValue, uint.MinValue, 1)]
        [Theory]
        public void When_an_uint_value_is_not_close_to_expected_value_it_should_fail(uint actual, uint nearbyValue,
            uint delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_an_uint_value_is_not_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            uint actual = 1;
            uint nearbyValue = 4;
            uint delta = 2;

            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*4*but found*1*");
        }

        [Fact]
        public void When_an_uint_value_is_returned_from_BeCloseTo_it_should_chain()
        {
            // Arrange
            uint actual = uint.MaxValue;

            // Act
            Action act = () => actual.Should().BeCloseTo(actual, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(1, ulong.MaxValue, ulong.MaxValue)]
        [InlineData(ulong.MinValue, ulong.MinValue + 1, ulong.MaxValue)]
        [InlineData(ulong.MinValue + 1, 0, ulong.MaxValue)]
        [InlineData(ulong.MinValue + 1, ulong.MinValue, 1)]
        [InlineData(ulong.MinValue + 1, ulong.MinValue, ulong.MaxValue)]
        [InlineData(ulong.MaxValue - 1, ulong.MaxValue, 1)]
        [InlineData(ulong.MaxValue - 1, ulong.MaxValue, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, 0, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, 1, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, ulong.MaxValue - 1, 1)]
        [InlineData(ulong.MaxValue, ulong.MaxValue - 1, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, ulong.MaxValue, 0)]
        [InlineData(ulong.MaxValue, ulong.MaxValue, 1)]
        [Theory]
        public void When_an_ulong_value_is_close_to_expected_value_it_should_succeed(ulong actual, ulong nearbyValue,
            ulong delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(ulong.MinValue, ulong.MaxValue, 1)]
        [InlineData(ulong.MaxValue, ulong.MinValue, 1)]
        [Theory]
        public void When_an_ulong_value_is_not_close_to_expected_value_it_should_fail(ulong actual, ulong nearbyValue,
            ulong delta)
        {
            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_an_ulong_value_is_not_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            ulong actual = 1;
            ulong nearbyValue = 4;
            ulong delta = 2;

            // Act
            Action act = () => actual.Should().BeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*4*but found*1*");
        }

        [Fact]
        public void When_an_ulong_value_is_returned_from_BeCloseTo_it_should_chain()
        {
            // Arrange
            ulong actual = ulong.MaxValue;

            // Act
            Action act = () => actual.Should().BeCloseTo(actual, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }
    }

    public class NotBeCloseTo
    {
        [InlineData(sbyte.MinValue, sbyte.MaxValue, 1)]
        [InlineData(sbyte.MinValue, 0, sbyte.MaxValue)]
        [InlineData(sbyte.MinValue, 1, sbyte.MaxValue)]
        [InlineData(-1, 0, 0)]
        [InlineData(-1, 1, 1)]
        [InlineData(-1, sbyte.MaxValue, sbyte.MaxValue)]
        [InlineData(0, sbyte.MinValue, sbyte.MaxValue)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 0, 0)]
        [InlineData(1, sbyte.MinValue, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue, sbyte.MinValue, 1)]
        [InlineData(sbyte.MaxValue, -1, sbyte.MaxValue)]
        [Theory]
        public void When_a_sbyte_value_is_not_close_to_expected_value_it_should_succeed(sbyte actual, sbyte distantValue,
            byte delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(sbyte.MinValue, sbyte.MinValue, 0)]
        [InlineData(sbyte.MinValue, sbyte.MinValue, 1)]
        [InlineData(sbyte.MinValue, sbyte.MinValue, sbyte.MaxValue)]
        [InlineData(sbyte.MinValue, sbyte.MinValue + 1, 1)]
        [InlineData(sbyte.MinValue, sbyte.MinValue + 1, sbyte.MaxValue)]
        [InlineData(sbyte.MinValue, -1, sbyte.MaxValue)]
        [InlineData(sbyte.MinValue + 1, sbyte.MinValue, 1)]
        [InlineData(sbyte.MinValue + 1, sbyte.MinValue, sbyte.MaxValue)]
        [InlineData(sbyte.MinValue + 1, 0, sbyte.MaxValue)]
        [InlineData(-1, sbyte.MinValue, sbyte.MaxValue)]
        [InlineData(-1, 0, 1)]
        [InlineData(-1, 0, sbyte.MaxValue)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, -1, 1)]
        [InlineData(0, -1, sbyte.MaxValue)]
        [InlineData(0, 1, 1)]
        [InlineData(0, 1, sbyte.MaxValue)]
        [InlineData(0, sbyte.MaxValue, sbyte.MaxValue)]
        [InlineData(0, sbyte.MinValue + 1, sbyte.MaxValue)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 0, sbyte.MaxValue)]
        [InlineData(1, sbyte.MaxValue, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue - 1, sbyte.MaxValue, 1)]
        [InlineData(sbyte.MaxValue - 1, sbyte.MaxValue, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue, 0, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue, 1, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue, 0)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue, 1)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue - 1, 1)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue - 1, sbyte.MaxValue)]
        [Theory]
        public void When_a_sbyte_value_is_close_to_expected_value_it_should_fail(sbyte actual, sbyte distantValue, byte delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_sbyte_value_is_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            sbyte actual = 1;
            sbyte nearbyValue = 3;
            byte delta = 2;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*3*but found*1*");
        }

        [Fact]
        public void When_a_sbyte_value_is_returned_from_NotBeCloseTo_it_should_chain()
        {
            // Arrange
            sbyte actual = sbyte.MaxValue;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(0, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(short.MinValue, short.MaxValue, 1)]
        [InlineData(short.MinValue, 0, short.MaxValue)]
        [InlineData(short.MinValue, 1, short.MaxValue)]
        [InlineData(-1, 0, 0)]
        [InlineData(-1, 1, 1)]
        [InlineData(-1, short.MaxValue, short.MaxValue)]
        [InlineData(0, short.MinValue, short.MaxValue)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 0, 0)]
        [InlineData(1, short.MinValue, short.MaxValue)]
        [InlineData(short.MaxValue, short.MinValue, 1)]
        [InlineData(short.MaxValue, -1, short.MaxValue)]
        [Theory]
        public void When_a_short_value_is_not_close_to_expected_value_it_should_succeed(short actual, short distantValue,
            ushort delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(short.MinValue, short.MinValue, 0)]
        [InlineData(short.MinValue, short.MinValue, 1)]
        [InlineData(short.MinValue, short.MinValue, short.MaxValue)]
        [InlineData(short.MinValue, short.MinValue + 1, 1)]
        [InlineData(short.MinValue, short.MinValue + 1, short.MaxValue)]
        [InlineData(short.MinValue, -1, short.MaxValue)]
        [InlineData(short.MinValue + 1, short.MinValue, 1)]
        [InlineData(short.MinValue + 1, short.MinValue, short.MaxValue)]
        [InlineData(short.MinValue + 1, 0, short.MaxValue)]
        [InlineData(-1, short.MinValue, short.MaxValue)]
        [InlineData(-1, 0, 1)]
        [InlineData(-1, 0, short.MaxValue)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, -1, 1)]
        [InlineData(0, -1, short.MaxValue)]
        [InlineData(0, 1, 1)]
        [InlineData(0, 1, short.MaxValue)]
        [InlineData(0, short.MaxValue, short.MaxValue)]
        [InlineData(0, short.MinValue + 1, short.MaxValue)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 0, short.MaxValue)]
        [InlineData(1, short.MaxValue, short.MaxValue)]
        [InlineData(short.MaxValue - 1, short.MaxValue, 1)]
        [InlineData(short.MaxValue - 1, short.MaxValue, short.MaxValue)]
        [InlineData(short.MaxValue, 0, short.MaxValue)]
        [InlineData(short.MaxValue, 1, short.MaxValue)]
        [InlineData(short.MaxValue, short.MaxValue, 0)]
        [InlineData(short.MaxValue, short.MaxValue, 1)]
        [InlineData(short.MaxValue, short.MaxValue, short.MaxValue)]
        [InlineData(short.MaxValue, short.MaxValue - 1, 1)]
        [InlineData(short.MaxValue, short.MaxValue - 1, short.MaxValue)]
        [Theory]
        public void When_a_short_value_is_close_to_expected_value_it_should_fail(short actual, short distantValue,
            ushort delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_short_value_is_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            short actual = 1;
            short nearbyValue = 3;
            ushort delta = 2;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*3*but found*1*");
        }

        [Fact]
        public void When_a_short_value_is_returned_from_NotBeCloseTo_it_should_chain()
        {
            // Arrange
            short actual = short.MaxValue;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(0, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(int.MinValue, int.MaxValue, 1)]
        [InlineData(int.MinValue, 0, int.MaxValue)]
        [InlineData(int.MinValue, 1, int.MaxValue)]
        [InlineData(-1, 0, 0)]
        [InlineData(-1, 1, 1)]
        [InlineData(-1, int.MaxValue, int.MaxValue)]
        [InlineData(0, int.MinValue, int.MaxValue)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 0, 0)]
        [InlineData(1, int.MinValue, int.MaxValue)]
        [InlineData(int.MaxValue, int.MinValue, 1)]
        [InlineData(int.MaxValue, -1, int.MaxValue)]
        [Theory]
        public void When_an_int_value_is_not_close_to_expected_value_it_should_succeed(int actual, int distantValue,
            uint delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(int.MinValue, int.MinValue, 0)]
        [InlineData(int.MinValue, int.MinValue, 1)]
        [InlineData(int.MinValue, int.MinValue, int.MaxValue)]
        [InlineData(int.MinValue, int.MinValue + 1, 1)]
        [InlineData(int.MinValue, int.MinValue + 1, int.MaxValue)]
        [InlineData(int.MinValue, -1, int.MaxValue)]
        [InlineData(int.MinValue + 1, int.MinValue, 1)]
        [InlineData(int.MinValue + 1, int.MinValue, int.MaxValue)]
        [InlineData(int.MinValue + 1, 0, int.MaxValue)]
        [InlineData(-1, int.MinValue, int.MaxValue)]
        [InlineData(-1, 0, 1)]
        [InlineData(-1, 0, int.MaxValue)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, -1, 1)]
        [InlineData(0, -1, int.MaxValue)]
        [InlineData(0, 1, 1)]
        [InlineData(0, 1, int.MaxValue)]
        [InlineData(0, int.MaxValue, int.MaxValue)]
        [InlineData(0, int.MinValue + 1, int.MaxValue)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 0, int.MaxValue)]
        [InlineData(1, int.MaxValue, int.MaxValue)]
        [InlineData(int.MaxValue - 1, int.MaxValue, 1)]
        [InlineData(int.MaxValue - 1, int.MaxValue, int.MaxValue)]
        [InlineData(int.MaxValue, 0, int.MaxValue)]
        [InlineData(int.MaxValue, 1, int.MaxValue)]
        [InlineData(int.MaxValue, int.MaxValue, 0)]
        [InlineData(int.MaxValue, int.MaxValue, 1)]
        [InlineData(int.MaxValue, int.MaxValue, int.MaxValue)]
        [InlineData(int.MaxValue, int.MaxValue - 1, 1)]
        [InlineData(int.MaxValue, int.MaxValue - 1, int.MaxValue)]
        [Theory]
        public void When_an_int_value_is_close_to_expected_value_it_should_fail(int actual, int distantValue, uint delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_an_int_value_is_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            int actual = 1;
            int nearbyValue = 3;
            uint delta = 2;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*3*but found*1*");
        }

        [Fact]
        public void When_an_int_value_is_returned_from_NotBeCloseTo_it_should_chain()
        {
            // Arrange
            int actual = int.MaxValue;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(0, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(long.MinValue, long.MaxValue, 1)]
        [InlineData(long.MinValue, 0, long.MaxValue)]
        [InlineData(long.MinValue, 1, long.MaxValue)]
        [InlineData(long.MinValue + 1, 0, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MinValue, long.MaxValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MinValue, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(-1, 0, 0)]
        [InlineData(-1, 1, 1)]
        [InlineData(-1, long.MaxValue, long.MaxValue)]
        [InlineData(-1, long.MinValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(0, long.MinValue, long.MaxValue)]
        [InlineData(0, long.MinValue + 1, (ulong.MaxValue / 2) - 1)]
        [InlineData(0, long.MaxValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 0, 0)]
        [InlineData(1, long.MinValue, long.MaxValue)]
        [InlineData(long.MaxValue, long.MinValue, 1)]
        [InlineData(long.MaxValue, -1, long.MaxValue)]
        [InlineData(long.MaxValue, 0, (ulong.MaxValue / 2) - 1)]
        [Theory]
        public void When_a_long_value_is_not_close_to_expected_value_it_should_succeed(long actual, long distantValue,
            ulong delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(long.MinValue, long.MinValue, 0)]
        [InlineData(long.MinValue, long.MinValue, 1)]
        [InlineData(long.MinValue, long.MinValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MinValue, long.MinValue, ulong.MaxValue / 2)]
        [InlineData(long.MinValue, long.MinValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MinValue, long.MinValue, ulong.MaxValue)]
        [InlineData(long.MinValue, long.MinValue + 1, 1)]
        [InlineData(long.MinValue, long.MinValue + 1, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MinValue, long.MinValue + 1, ulong.MaxValue / 2)]
        [InlineData(long.MinValue, long.MinValue + 1, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MinValue, long.MinValue + 1, ulong.MaxValue)]
        [InlineData(long.MinValue, -1, long.MaxValue)]
        [InlineData(long.MinValue + 1, long.MinValue, 1)]
        [InlineData(long.MinValue + 1, long.MinValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MinValue + 1, long.MinValue, ulong.MaxValue / 2)]
        [InlineData(long.MinValue + 1, long.MinValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MinValue + 1, long.MinValue, ulong.MaxValue)]
        [InlineData(long.MinValue + 1, 0, ulong.MaxValue / 2)]
        [InlineData(long.MinValue + 1, 0, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MinValue + 1, 0, ulong.MaxValue)]
        [InlineData(long.MinValue, long.MaxValue, ulong.MaxValue)]
        [InlineData(-1, long.MinValue, ulong.MaxValue / 2)]
        [InlineData(-1, long.MinValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(-1, long.MinValue, ulong.MaxValue)]
        [InlineData(-1, 0, 1)]
        [InlineData(-1, 0, (ulong.MaxValue / 2) - 1)]
        [InlineData(-1, 0, ulong.MaxValue / 2)]
        [InlineData(-1, 0, (ulong.MaxValue / 2) + 1)]
        [InlineData(-1, 0, ulong.MaxValue)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, -1, 1)]
        [InlineData(0, -1, (ulong.MaxValue / 2) - 1)]
        [InlineData(0, -1, ulong.MaxValue / 2)]
        [InlineData(0, -1, (ulong.MaxValue / 2) + 1)]
        [InlineData(0, -1, ulong.MaxValue)]
        [InlineData(0, 1, 1)]
        [InlineData(0, 1, (ulong.MaxValue / 2) - 1)]
        [InlineData(0, 1, ulong.MaxValue / 2)]
        [InlineData(0, 1, (ulong.MaxValue / 2) + 1)]
        [InlineData(0, 1, ulong.MaxValue)]
        [InlineData(0, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(0, long.MaxValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(0, long.MaxValue, ulong.MaxValue)]
        [InlineData(0, long.MinValue + 1, ulong.MaxValue / 2)]
        [InlineData(0, long.MinValue + 1, (ulong.MaxValue / 2) + 1)]
        [InlineData(0, long.MinValue + 1, ulong.MaxValue)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 0, (ulong.MaxValue / 2) - 1)]
        [InlineData(1, 0, ulong.MaxValue / 2)]
        [InlineData(1, 0, (ulong.MaxValue / 2) + 1)]
        [InlineData(1, 0, ulong.MaxValue)]
        [InlineData(1, long.MaxValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(1, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(1, long.MaxValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(1, long.MaxValue, ulong.MaxValue)]
        [InlineData(long.MaxValue - 1, long.MaxValue, 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue - 1, long.MaxValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, ulong.MaxValue)]
        [InlineData(long.MaxValue, 0, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, 0, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MaxValue, 0, ulong.MaxValue)]
        [InlineData(long.MaxValue, 1, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MaxValue, 1, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, 1, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MaxValue, 1, ulong.MaxValue)]
        [InlineData(long.MaxValue, long.MaxValue, 0)]
        [InlineData(long.MaxValue, long.MaxValue, 1)]
        [InlineData(long.MaxValue, long.MaxValue, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MaxValue, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, long.MaxValue, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MaxValue, long.MaxValue, ulong.MaxValue)]
        [InlineData(long.MaxValue, long.MaxValue - 1, 1)]
        [InlineData(long.MaxValue, long.MaxValue - 1, (ulong.MaxValue / 2) - 1)]
        [InlineData(long.MaxValue, long.MaxValue - 1, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, long.MaxValue - 1, (ulong.MaxValue / 2) + 1)]
        [InlineData(long.MaxValue, long.MaxValue - 1, ulong.MaxValue)]
        [Theory]
        public void When_a_long_value_is_close_to_expected_value_it_should_fail(long actual, long distantValue, ulong delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_long_value_is_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            long actual = 1;
            long nearbyValue = 3;
            ulong delta = 2;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*3*but found*1*");
        }

        [Fact]
        public void When_a_long_value_is_returned_from_NotBeCloseTo_it_should_chain()
        {
            // Arrange
            long actual = long.MaxValue;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(0, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(byte.MinValue, byte.MaxValue, 1)]
        [InlineData(byte.MaxValue, byte.MinValue, 1)]
        [Theory]
        public void When_a_byte_value_is_not_close_to_expected_value_it_should_succeed(byte actual, byte distantValue,
            byte delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(1, byte.MaxValue, byte.MaxValue)]
        [InlineData(byte.MinValue, byte.MinValue + 1, byte.MaxValue)]
        [InlineData(byte.MinValue + 1, 0, byte.MaxValue)]
        [InlineData(byte.MinValue + 1, byte.MinValue, 1)]
        [InlineData(byte.MinValue + 1, byte.MinValue, byte.MaxValue)]
        [InlineData(byte.MaxValue - 1, byte.MaxValue, 1)]
        [InlineData(byte.MaxValue - 1, byte.MaxValue, byte.MaxValue)]
        [InlineData(byte.MaxValue, 0, byte.MaxValue)]
        [InlineData(byte.MaxValue, 1, byte.MaxValue)]
        [InlineData(byte.MaxValue, byte.MaxValue - 1, 1)]
        [InlineData(byte.MaxValue, byte.MaxValue - 1, byte.MaxValue)]
        [InlineData(byte.MaxValue, byte.MaxValue, 0)]
        [InlineData(byte.MaxValue, byte.MaxValue, 1)]
        [Theory]
        public void When_a_byte_value_is_close_to_expected_value_it_should_fail(byte actual, byte distantValue, byte delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_byte_value_is_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            byte actual = 1;
            byte nearbyValue = 3;
            byte delta = 2;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*3*but found*1*");
        }

        [Fact]
        public void When_a_byte_value_is_returned_from_NotBeCloseTo_it_should_chain()
        {
            // Arrange
            byte actual = byte.MaxValue;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(0, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(ushort.MinValue, ushort.MaxValue, 1)]
        [InlineData(ushort.MaxValue, ushort.MinValue, 1)]
        [Theory]
        public void When_an_ushort_value_is_not_close_to_expected_value_it_should_succeed(ushort actual, ushort distantValue,
            ushort delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(1, ushort.MaxValue, ushort.MaxValue)]
        [InlineData(ushort.MinValue, ushort.MinValue + 1, ushort.MaxValue)]
        [InlineData(ushort.MinValue + 1, 0, ushort.MaxValue)]
        [InlineData(ushort.MinValue + 1, ushort.MinValue, 1)]
        [InlineData(ushort.MinValue + 1, ushort.MinValue, ushort.MaxValue)]
        [InlineData(ushort.MaxValue - 1, ushort.MaxValue, 1)]
        [InlineData(ushort.MaxValue - 1, ushort.MaxValue, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, 0, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, 1, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, ushort.MaxValue - 1, 1)]
        [InlineData(ushort.MaxValue, ushort.MaxValue - 1, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, ushort.MaxValue, 0)]
        [InlineData(ushort.MaxValue, ushort.MaxValue, 1)]
        [Theory]
        public void When_an_ushort_value_is_close_to_expected_value_it_should_fail(ushort actual, ushort distantValue,
            ushort delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_an_ushort_value_is_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            ushort actual = 1;
            ushort nearbyValue = 3;
            ushort delta = 2;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*3*but found*1*");
        }

        [Fact]
        public void When_an_ushort_value_is_returned_from_NotBeCloseTo_it_should_chain()
        {
            // Arrange
            ushort actual = ushort.MaxValue;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(0, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(uint.MinValue, uint.MaxValue, 1)]
        [InlineData(uint.MaxValue, uint.MinValue, 1)]
        [Theory]
        public void When_an_uint_value_is_not_close_to_expected_value_it_should_succeed(uint actual, uint distantValue,
            uint delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(1, uint.MaxValue, uint.MaxValue)]
        [InlineData(uint.MinValue, uint.MinValue + 1, uint.MaxValue)]
        [InlineData(uint.MinValue + 1, 0, uint.MaxValue)]
        [InlineData(uint.MinValue + 1, uint.MinValue, 1)]
        [InlineData(uint.MinValue + 1, uint.MinValue, uint.MaxValue)]
        [InlineData(uint.MaxValue - 1, uint.MaxValue, 1)]
        [InlineData(uint.MaxValue - 1, uint.MaxValue, uint.MaxValue)]
        [InlineData(uint.MaxValue, 0, uint.MaxValue)]
        [InlineData(uint.MaxValue, 1, uint.MaxValue)]
        [InlineData(uint.MaxValue, uint.MaxValue - 1, 1)]
        [InlineData(uint.MaxValue, uint.MaxValue - 1, uint.MaxValue)]
        [InlineData(uint.MaxValue, uint.MaxValue, 0)]
        [InlineData(uint.MaxValue, uint.MaxValue, 1)]
        [Theory]
        public void When_an_uint_value_is_close_to_expected_value_it_should_fail(uint actual, uint distantValue, uint delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_an_uint_value_is_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            uint actual = 1;
            uint nearbyValue = 3;
            uint delta = 2;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*3*but found*1*");
        }

        [Fact]
        public void When_an_uint_value_is_returned_from_NotBeCloseTo_it_should_chain()
        {
            // Arrange
            uint actual = uint.MaxValue;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(0, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(ulong.MinValue, ulong.MaxValue, 1)]
        [InlineData(ulong.MaxValue, ulong.MinValue, 1)]
        [Theory]
        public void When_an_ulong_value_is_not_close_to_expected_value_it_should_succeed(ulong actual, ulong distantValue,
            ulong delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(1, ulong.MaxValue, ulong.MaxValue)]
        [InlineData(ulong.MinValue, ulong.MinValue + 1, ulong.MaxValue)]
        [InlineData(ulong.MinValue + 1, 0, ulong.MaxValue)]
        [InlineData(ulong.MinValue + 1, ulong.MinValue, 1)]
        [InlineData(ulong.MinValue + 1, ulong.MinValue, ulong.MaxValue)]
        [InlineData(ulong.MaxValue - 1, ulong.MaxValue, 1)]
        [InlineData(ulong.MaxValue - 1, ulong.MaxValue, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, 0, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, 1, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, ulong.MaxValue - 1, 1)]
        [InlineData(ulong.MaxValue, ulong.MaxValue - 1, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, ulong.MaxValue, 0)]
        [InlineData(ulong.MaxValue, ulong.MaxValue, 1)]
        [Theory]
        public void When_an_ulong_value_is_close_to_expected_value_it_should_fail(ulong actual, ulong distantValue,
            ulong delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_an_ulong_value_is_close_to_expected_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            ulong actual = 1;
            ulong nearbyValue = 3;
            ulong delta = 2;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(nearbyValue, delta);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*be within*2*from*3*but found*1*");
        }

        [Fact]
        public void When_an_ulong_value_is_returned_from_NotBeCloseTo_it_should_chain()
        {
            // Arrange
            ulong actual = ulong.MaxValue;

            // Act
            Action act = () => actual.Should().NotBeCloseTo(0, 0)
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }
    }
}
