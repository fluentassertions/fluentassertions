using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class Be
    {
        [Fact]
        public void A_value_is_equal_to_the_same_value()
        {
            // Arrange
            int value = 1;
            int sameValue = 1;

            // Act
            value.Should().Be(sameValue);
        }

        [Fact]
        public void A_value_is_not_equal_to_another_value()
        {
            // Arrange
            int value = 1;
            int differentValue = 2;

            // Act
            Action act = () => value.Should().Be(differentValue, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be 2 because we want to test the failure message, but found 1.");
        }

        [Fact]
        public void A_value_is_equal_to_the_same_nullable_value()
        {
            // Arrange
            int value = 2;
            int? nullableValue = 2;

            // Act
            value.Should().Be(nullableValue);
        }

        [Fact]
        public void A_value_is_not_equal_to_null()
        {
            // Arrange
            int value = 2;
            int? nullableValue = null;

            // Act
            Action act = () => value.Should().Be(nullableValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected*<null>, but found 2.");
        }

        [Fact]
        public void Null_is_not_equal_to_another_nullable_value()
        {
            // Arrange
            int? value = 2;

            // Act
            Action action = () => ((int?)null).Should().Be(value);

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Expected*2, but found <null>.");
        }

        [InlineData(0, 0)]
        [InlineData(null, null)]
        [Theory]
        public void A_nullable_value_is_equal_to_the_same_nullable_value(int? subject, int? expected)
        {
            // Act / Assert
            subject.Should().Be(expected);
        }

        [InlineData(0, 1)]
        [InlineData(0, null)]
        [InlineData(null, 0)]
        [Theory]
        public void A_nullable_value_is_not_equal_to_another_nullable_value(int? subject, int? expected)
        {
            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Null_is_not_equal_to_another_value()
        {
            // Arrange
            int? subject = null;
            int expected = 1;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_that_a_float_value_is_equal_to_a_different_value_it_should_throw()
        {
            // Arrange
            float value = 3.5F;

            // Act
            Action act = () => value.Should().Be(3.4F, "we want to test the error message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be *3.4* because we want to test the error message, but found *3.5*");
        }

        [Fact]
        public void When_asserting_that_a_float_value_is_equal_to_the_same_value_it_should_not_throw()
        {
            // Arrange
            float value = 3.5F;

            // Act
            Action act = () => value.Should().Be(3.5F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_that_a_null_float_value_is_equal_to_some_value_it_should_throw()
        {
            // Arrange
            float? value = null;

            // Act
            Action act = () => value.Should().Be(3.5F);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be *3.5* but found <null>.");
        }

        [Fact]
        public void When_asserting_that_a_double_value_is_equal_to_a_different_value_it_should_throw()
        {
            // Arrange
            double value = 3.5;

            // Act
            Action act = () => value.Should().Be(3.4, "we want to test the error message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be 3.4 because we want to test the error message, but found 3.5*.");
        }

        [Fact]
        public void When_asserting_that_a_double_value_is_equal_to_the_same_value_it_should_not_throw()
        {
            // Arrange
            double value = 3.5;

            // Act
            Action act = () => value.Should().Be(3.5);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_that_a_null_double_value_is_equal_to_some_value_it_should_throw()
        {
            // Arrange
            double? value = null;

            // Act
            Action act = () => value.Should().Be(3.5);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be 3.5, but found <null>.");
        }

        [Fact]
        public void When_asserting_that_a_decimal_value_is_equal_to_a_different_value_it_should_throw()
        {
            // Arrange
            decimal value = 3.5m;

            // Act
            Action act = () => value.Should().Be(3.4m, "we want to test the error message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be*3.4* because we want to test the error message, but found*3.5*");
        }

        [Fact]
        public void When_asserting_that_a_decimal_value_is_equal_to_the_same_value_it_should_not_throw()
        {
            // Arrange
            decimal value = 3.5m;

            // Act
            Action act = () => value.Should().Be(3.5m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_that_a_null_decimal_value_is_equal_to_some_value_it_should_throw()
        {
            // Arrange
            decimal? value = null;
            decimal someValue = 3.5m;

            // Act
            Action act = () => value.Should().Be(someValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be*3.5*, but found <null>.");
        }

        [Fact]
        public void Nan_is_never_equal_to_a_normal_float()
        {
            // Arrange
            float value = float.NaN;

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
            float value = float.NaN;

            // Act
            value.Should().Be(float.NaN);
        }

        [Fact]
        public void Nan_is_never_equal_to_a_normal_double()
        {
            // Arrange
            double value = double.NaN;

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
            double value = double.NaN;

            // Act
            value.Should().Be(double.NaN);
        }
    }

    public class NotBe
    {
        [InlineData(1, 2)]
        [InlineData(null, 2)]
        [Theory]
        public void A_nullable_value_is_not_equal_to_another_value(int? subject, int unexpected)
        {
            // Act
            subject.Should().NotBe(unexpected);
        }

        [Fact]
        public void A_value_is_not_different_from_the_same_value()
        {
            // Arrange
            int value = 1;
            int sameValue = 1;

            // Act
            Action act = () => value.Should().NotBe(sameValue, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Did not expect value to be 1 because we want to test the failure message.");
        }

        [InlineData(null, null)]
        [InlineData(0, 0)]
        [Theory]
        public void A_nullable_value_is_not_different_from_the_same_value(int? subject, int? unexpected)
        {
            // Act
            Action act = () => subject.Should().NotBe(unexpected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [InlineData(0, 1)]
        [InlineData(0, null)]
        [InlineData(null, 0)]
        [Theory]
        public void A_nullable_value_is_different_from_another_value(int? subject, int? unexpected)
        {
            // Act / Assert
            subject.Should().NotBe(unexpected);
        }
    }

    public class Bytes
    {
        [Fact]
        public void When_asserting_a_byte_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            byte value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_sbyte_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            sbyte value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_short_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            short value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_ushort_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            ushort value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_uint_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            uint value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_long_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            long value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_ulong_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            ulong value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }
    }

    public class NullableBytes
    {
        [Fact]
        public void When_asserting_a_nullable_byte_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            byte? value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nullable_sbyte_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            sbyte? value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nullable_short_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            short? value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nullable_ushort_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            ushort? value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nullable_uint_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            uint? value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nullable_long_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            long? value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nullable_nullable_ulong_value_it_should_treat_is_any_numeric_value()
        {
            // Arrange
            ulong? value = 2;

            // Act
            Action act = () => value.Should().Be(2);

            // Assert
            act.Should().NotThrow();
        }
    }
}
