using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    /// <summary>
    /// Summary description for CustomAssertionSpecs
    /// </summary>
    public class NumericAssertionSpecs
    {
        #region Positive / Negative

        [Fact]
        public void When_a_positive_value_is_positive_it_should_not_throw()
        {
            // Arrange
            float value = 1F;

            // Act
            Action act = () => value.Should().BePositive();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_negative_value_is_positive_it_should_throw()
        {
            // Arrange
            double value = -1D;

            // Act
            Action act = () => value.Should().BePositive();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_zero_value_is_positive_it_should_throw()
        {
            // Arrange
            int value = 0;

            // Act
            Action act = () => value.Should().BePositive();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_negative_value_is_positive_it_should_throw_with_descriptive_message()
        {
            // Arrange
            int value = -1;

            // Act
            Action act = () => value.Should().BePositive("because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be positive because we want to test the failure message, but found -1.");
        }

        [Fact]
        public void When_a_negative_value_is_negative_it_should_not_throw()
        {
            // Arrange
            int value = -1;

            // Act
            Action act = () => value.Should().BeNegative();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_positive_value_is_negative_it_should_throw()
        {
            // Arrange
            int value = 1;

            // Act
            Action act = () => value.Should().BeNegative();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_zero_value_is_negative_it_should_throw()
        {
            // Arrange
            int value = 0;

            // Act
            Action act = () => value.Should().BeNegative();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_positive_value_is_negative_it_should_throw_with_descriptive_message()
        {
            // Arrange
            int value = 1;

            // Act
            Action act = () => value.Should().BeNegative("because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be negative because we want to test the failure message, but found 1.");
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_negative_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().BeNegative();

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_positive_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().BePositive();

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        #endregion

        #region Be / NotBe

        [Fact]
        public void When_a_value_is_equal_to_same_value_it_should_not_throw()
        {
            // Arrange
            int value = 1;
            int sameValue = 1;

            // Act
            Action act = () => value.Should().Be(sameValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_value_is_equal_to_different_value_it_should_throw()
        {
            // Arrange
            int value = 1;
            int differentValue = 2;

            // Act
            Action act = () => value.Should().Be(differentValue);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_value_is_equal_to_different_value_it_should_throw_with_descriptive_message()
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
        public void When_a_nullable_value_is_equal_it_should_not_throw()
        {
            // Arrange
            int value = 2;
            int? nullableValue = 2;

            // Act
            Action act = () => value.Should().Be(nullableValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_value_is_null_but_the_subject_isnt_it_should_throw()
        {
            // Arrange
            int value = 2;
            int? nullableValue = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.Should().Be(nullableValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected*<null>, but found 2.");
        }

        [Fact]
        public void When_a_nullable_value_has_value_but_the_subject_is_null_should_throw()
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

        [Fact]
        public void When_a_value_is_not_equal_to_a_different_value_it_should_not_throw()
        {
            // Arrange
            int value = 1;
            int differentValue = 2;

            // Act
            Action act = () => value.Should().NotBe(differentValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_value_is_not_equal_to_the_same_value_it_should_throw()
        {
            // Arrange
            int value = 1;
            int sameValue = 1;

            // Act
            Action act = () => value.Should().NotBe(sameValue);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_value_is_not_equal_to_the_same_value_it_should_throw_with_descriptive_message()
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

        [Fact]
        public void When_a_nullable_numeric_null_value_not_equals_null_it_should_throw()
        {
            // Arrange
            int? nullableIntegerA = null;
            int? nullableIntegerB = null;

            // Act
            Action act = () => nullableIntegerA.Should().NotBe(nullableIntegerB);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_nullable_numeric_value_not_equals_null_it_should_succeed()
        {
            // Arrange
            int? nullableIntegerA = 1;
            int? nullableIntegerB = null;

            // Act / Assert
            nullableIntegerA.Should().NotBe(nullableIntegerB);
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_not_equals_nullable_value_it_should_succeed()
        {
            // Arrange
            int? nullableIntegerA = null;
            int? nullableIntegerB = 1;

            // Act / Assert
            nullableIntegerA.Should().NotBe(nullableIntegerB);
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_not_equals_value_it_should_succeed()
        {
            // Arrange
            int? nullableIntegerA = null;
            int nullableIntegerB = 1;

            // Act / Assert
            nullableIntegerA.Should().NotBe(nullableIntegerB);
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_equals_null_it_should_succeed()
        {
            // Arrange
            int? nullableIntegerA = null;
            int? nullableIntegerB = null;

            // Act / Assert
            nullableIntegerA.Should().Be(nullableIntegerB);
        }

        [Fact]
        public void When_a_nullable_numeric_value_equals_null_it_should_throw()
        {
            // Arrange
            int? nullableIntegerA = 1;
            int? nullableIntegerB = null;

            // Act
            Action act = () => nullableIntegerA.Should().Be(nullableIntegerB);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_equals_nullable_value_it_should_throw()
        {
            // Arrange
            int? nullableIntegerA = null;
            int? nullableIntegerB = 1;

            // Act
            Action act = () => nullableIntegerA.Should().Be(nullableIntegerB);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_equals_value_it_should_throw()
        {
            // Arrange
            int? nullableIntegerA = null;
            int nullableIntegerB = 1;

            // Act
            Action act = () => nullableIntegerA.Should().Be(nullableIntegerB);

            // Assert
            act.Should().Throw<XunitException>();
        }

        #endregion

        #region Greater Than (Or Equal To)

        [Fact]
        public void When_a_value_is_greater_than_smaller_value_it_should_not_throw()
        {
            // Arrange
            int value = 2;
            int smallerValue = 1;

            // Act
            Action act = () => value.Should().BeGreaterThan(smallerValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_value_is_greater_than_greater_value_it_should_throw()
        {
            // Arrange
            int value = 2;
            int greaterValue = 3;

            // Act
            Action act = () => value.Should().BeGreaterThan(greaterValue);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_value_is_greater_than_same_value_it_should_throw()
        {
            // Arrange
            int value = 2;
            int sameValue = 2;

            // Act
            Action act = () => value.Should().BeGreaterThan(sameValue);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_value_is_greater_than_greater_value_it_should_throw_with_descriptive_message()
        {
            // Arrange
            int value = 2;
            int greaterValue = 3;

            // Act
            Action act = () => value.Should().BeGreaterThan(greaterValue, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be greater than 3 because we want to test the failure message, but found 2.");
        }

        [Fact]
        public void When_a_value_is_greater_or_equal_to_smaller_value_it_should_not_throw()
        {
            // Arrange
            int value = 2;
            int smallerValue = 1;

            // Act
            Action act = () => value.Should().BeGreaterOrEqualTo(smallerValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_value_is_greater_or_equal_to_same_value_it_should_not_throw()
        {
            // Arrange
            int value = 2;
            int sameValue = 2;

            // Act
            Action act = () => value.Should().BeGreaterOrEqualTo(sameValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_value_is_greater_or_equal_to_greater_value_it_should_throw()
        {
            // Arrange
            int value = 2;
            int greaterValue = 3;

            // Act
            Action act = () => value.Should().BeGreaterOrEqualTo(greaterValue);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_value_is_greater_or_equal_to_greater_value_it_should_throw_with_descriptive_message()
        {
            // Arrange
            int value = 2;
            int greaterValue = 3;

            // Act
            Action act =
                () => value.Should().BeGreaterOrEqualTo(greaterValue, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be greater or equal to 3 because we want to test the failure message, but found 2.");
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_greater_than_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().BeGreaterThan(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_greater_or_equal_to_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().BeGreaterOrEqualTo(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        #endregion

        #region Less Than (Or Equal To)

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
        public void When_a_value_is_less_or_equal_to_greater_value_it_should_not_throw()
        {
            // Arrange
            int value = 1;
            int greaterValue = 2;

            // Act
            Action act = () => value.Should().BeLessOrEqualTo(greaterValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_value_is_less_or_equal_to_same_value_it_should_not_throw()
        {
            // Arrange
            int value = 2;
            int sameValue = 2;

            // Act
            Action act = () => value.Should().BeLessOrEqualTo(sameValue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_value_is_less_or_equal_to_smaller_value_it_should_throw()
        {
            // Arrange
            int value = 2;
            int smallerValue = 1;

            // Act
            Action act = () => value.Should().BeLessOrEqualTo(smallerValue);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_value_is_less_or_equal_to_smaller_value_it_should_throw_with_descriptive_message()
        {
            // Arrange
            int value = 2;
            int smallerValue = 1;

            // Act
            Action act = () => value.Should().BeLessOrEqualTo(smallerValue, "because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be less or equal to 1 because we want to test the failure message, but found 2.");
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

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_less_or_equal_to_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().BeLessOrEqualTo(0);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        #endregion

        #region In Range

        [Fact]
        public void When_a_value_is_outside_a_range_it_should_throw()
        {
            // Arrange
            float value = 3.99F;

            // Act
            Action act = () => value.Should().BeInRange(4, 5, "because that's the valid range");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be between*4* and*5* because that\'s the valid range, but found*3.99*");
        }

        [Fact]
        public void When_a_value_is_inside_a_range_it_should_not_throw()
        {
            // Arrange
            int value = 4;

            // Act
            Action act = () => value.Should().BeInRange(3, 5);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_in_range_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().BeInRange(0, 1);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        #endregion

        #region Not In Range

        [Fact]
        public void When_a_value_is_inside_an_unexpected_range_it_should_throw()
        {
            // Arrange
            float value = 4.99F;

            // Act
            Action act = () => value.Should().NotBeInRange(4, 5, "because that's the invalid range");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to not be between*4* and*5* because that\'s the invalid range, but found*4.99*");
        }

        [Fact]
        public void When_a_value_is_outside_an_unexpected_range_it_should_not_throw()
        {
            // Arrange
            float value = 3.99F;

            // Act
            Action act = () => value.Should().NotBeInRange(4, 5);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_numeric_null_value_is_not_not_in_range_to_it_should_throw()
        {
            // Arrange
            int? value = null;

            // Act
            Action act = () => value.Should().NotBeInRange(0, 1);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*null*");
        }

        #endregion

        #region Be One Of

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
                .WithMessage(string.Format("Expected value to be one of {{4, 5}}, but found {0}.", value));
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
                .WithMessage(
                    string.Format("Expected value to be one of {{4, 5}} because those are the valid values, but found {0}.", value));
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

        #endregion

        #region Bytes

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

        #endregion

        #region Nullable Bytes

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

        #endregion

        #region Floating Point

        #region float

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
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.Should().Be(3.5F);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be *3.5* but found <null>.");
        }

        [Fact]
        public void When_float_is_not_approximating_a_range_it_should_throw()
        {
            // Arrange
            float value = 3.1415927F;

            // Act
            Action act = () => value.Should().BeApproximately(3.14F, 0.001F, "rockets will crash otherwise");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to approximate *3.14* +/- *0.001* because rockets will crash otherwise, but *3.1415927* differed by *0.001592*");
        }

        [Fact]
        public void When_float_is_indeed_approximating_a_value_it_should_not_throw()
        {
            // Arrange
            float value = 3.1415927F;

            // Act
            Action act = () => value.Should().BeApproximately(3.14F, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(9F)]
        [InlineData(11F)]
        [Theory]
        public void When_float_is_approximating_a_value_on_boundaries_it_should_not_throw(float value)
        {
            // Act
            Action act = () => value.Should().BeApproximately(10F, 1F);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(9F)]
        [InlineData(11F)]
        [Theory]
        public void When_float_is_not_approximating_a_value_on_boundaries_it_should_throw(float value)
        {
            // Act
            Action act = () => value.Should().BeApproximately(10F, 0.9F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_approximating_a_float_towards_nan_it_should_not_throw()
        {
            // Arrange
            float value = float.NaN;

            // Act
            Action act = () => value.Should().BeApproximately(3.14F, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_approximating_positive_infinity_float_towards_positive_infinity_it_should_not_throw()
        {
            // Arrange
            float value = float.PositiveInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(float.PositiveInfinity, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_approximating_negative_infinity_float_towards_negative_infinity_it_should_not_throw()
        {
            // Arrange
            float value = float.NegativeInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(float.NegativeInfinity, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_float_is_not_approximating_positive_infinity_it_should_throw()
        {
            // Arrange
            float value = float.PositiveInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(float.MaxValue, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_float_is_not_approximating_negative_infinity_it_should_throw()
        {
            // Arrange
            float value = float.NegativeInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(float.MinValue, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_nullable_float_has_no_value_it_should_throw()
        {
            // Arrange
            float? value = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.Should().BeApproximately(3.14F, 0.001F);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to approximate*3.14* +/-*0.001*, but it was <null>.");
        }

        [Fact]
        public void When_float_is_approximating_a_range_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            float value = 3.1415927F;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14F, 0.1F, "rockets will crash otherwise");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to not approximate *3.14* +/- *0.1* because rockets will crash otherwise, but *3.1415927* only differed by *0.001592*");
        }

        [Fact]
        public void When_float_is_not_approximating_a_value_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            float value = 3.1415927F;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14F, 0.001F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_approximating_a_float_towards_nan_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            float value = float.NaN;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14F, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_not_approximating_a_float_towards_positive_infinity_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            float value = float.PositiveInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(float.MaxValue, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_not_approximating_a_float_towards_negative_infinity_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            float value = float.NegativeInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(float.MinValue, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_approximating_positive_infinity_float_towards_positive_infinity_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            float value = float.PositiveInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(float.PositiveInfinity, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_not_approximating_negative_infinity_float_towards_negative_infinity_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            float value = float.NegativeInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(float.NegativeInfinity, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [InlineData(9F)]
        [InlineData(11F)]
        [Theory]
        public void When_float_is_not_approximating_a_value_on_boundaries_it_should_not_throw(float value)
        {
            // Act
            Action act = () => value.Should().NotBeApproximately(10F, 0.9F);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(9F)]
        [InlineData(11F)]
        [Theory]
        public void When_float_is_approximating_a_value_on_boundaries_it_should_throw(float value)
        {
            // Act
            Action act = () => value.Should().NotBeApproximately(10F, 1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_nullable_float_has_no_value_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            float? value = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.Should().NotBeApproximately(3.14F, 0.001F);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region double

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
                    "Expected value to be 3.4 because we want to test the error message, but found 3.5.");
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
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.Should().Be(3.5);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be 3.5, but found <null>.");
        }

        [Fact]
        public void When_double_is_not_approximating_a_range_it_should_throw()
        {
            // Arrange
            double value = 3.1415927;

            // Act
            Action act = () => value.Should().BeApproximately(3.14, 0.001, "rockets will crash otherwise");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to approximate 3.14 +/- 0.001 because rockets will crash otherwise, but 3.1415927 differed by 0.001592*");
        }

        [Fact]
        public void When_double_is_indeed_approximating_a_value_it_should_not_throw()
        {
            // Arrange
            double value = 3.1415927;

            // Act
            Action act = () => value.Should().BeApproximately(3.14, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_approximating_a_double_towards_nan_it_should_not_throw()
        {
            // Arrange
            double value = double.NaN;

            // Act
            Action act = () => value.Should().BeApproximately(3.14F, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_approximating_positive_infinity_double_towards_positive_infinity_it_should_not_throw()
        {
            // Arrange
            double value = double.PositiveInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(double.PositiveInfinity, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_approximating_negative_infinity_double_towards_negative_infinity_it_should_not_throw()
        {
            // Arrange
            double value = double.NegativeInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(double.NegativeInfinity, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_double_is_not_approximating_positive_infinity_it_should_throw()
        {
            // Arrange
            double value = double.PositiveInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(double.MaxValue, 0.1);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_double_is_not_approximating_negative_infinity_it_should_throw()
        {
            // Arrange
            double value = double.NegativeInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(double.MinValue, 0.1);

            // Assert
            act.Should().Throw<XunitException>();
        }


        [InlineData(9D)]
        [InlineData(11D)]
        [Theory]
        public void When_double_is_approximating_a_value_on_boundaries_it_should_not_throw(double value)
        {
            // Act
            Action act = () => value.Should().BeApproximately(10D, 1D);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(9D)]
        [InlineData(11D)]
        [Theory]
        public void When_double_is_not_approximating_a_value_on_boundaries_it_should_throw(double value)
        {
            // Act
            Action act = () => value.Should().BeApproximately(10D, 0.9D);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_double_is_approximating_a_range_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            double value = 3.1415927;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14, 0.1, "rockets will crash otherwise");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to not approximate *3.14* +/- *0.1* because rockets will crash otherwise, but *3.1415927* only differed by *0.001592*");
        }

        [Fact]
        public void When_double_is_not_approximating_a_value_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            double value = 3.1415927;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14, 0.001);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_approximating_a_double_towards_nan_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            double value = double.NaN;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14, 0.1);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_not_approximating_a_double_towards_positive_infinity_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            double value = double.PositiveInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(double.MaxValue, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_not_approximating_a_double_towards_negative_infinity_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            double value = double.NegativeInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(double.MinValue, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_approximating_positive_infinity_double_towards_positive_infinity_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            double value = double.PositiveInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(double.PositiveInfinity, 0.1);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_not_approximating_negative_infinity_double_towards_negative_infinity_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            double value = double.NegativeInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(double.NegativeInfinity, 0.1);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_nullable_double_has_no_value_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            double? value = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.Should().NotBeApproximately(3.14, 0.001);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(9D)]
        [InlineData(11D)]
        [Theory]
        public void When_double_is_not_approximating_a_value_on_boundaries_it_should_not_throw(double value)
        {
            // Act
            Action act = () => value.Should().NotBeApproximately(10D, 0.9D);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(9D)]
        [InlineData(11D)]
        [Theory]
        public void When_double_is_approximating_a_value_on_boundaries_it_should_throw(double value)
        {
            // Act
            Action act = () => value.Should().NotBeApproximately(10D, 1D);

            // Assert
            act.Should().Throw<XunitException>();
        }

        #endregion

        #region decimal

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
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.Should().Be(someValue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be*3.5*, but found <null>.");
        }

        [Fact]
        public void When_decimal_is_not_approximating_a_range_it_should_throw()
        {
            // Arrange
            decimal value = 3.5011m;

            // Act
            Action act = () => value.Should().BeApproximately(3.5m, 0.001m, "rockets will crash otherwise");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to approximate*3.5* +/-*0.001* because rockets will crash otherwise, but *3.5011* differed by*0.0011*");
        }

        [Fact]
        public void When_decimal_is_indeed_approximating_a_value_it_should_not_throw()
        {
            // Arrange
            decimal value = 3.5011m;

            // Act
            Action act = () => value.Should().BeApproximately(3.5m, 0.01m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_decimal_is_approximating_a_value_on_lower_boundary_it_should_not_throw()
        {
            // Act
            decimal value = 9m;

            // Act
            Action act = () => value.Should().BeApproximately(10m, 1m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_decimal_is_approximating_a_value_on_upper_boundary_it_should_not_throw()
        {
            // Act
            decimal value = 11m;

            // Act
            Action act = () => value.Should().BeApproximately(10m, 1m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_decimal_is_not_approximating_a_value_on_lower_boundary_it_should_throw()
        {
            // Act
            decimal value = 9m;

            // Act
            Action act = () => value.Should().BeApproximately(10m, 0.9m);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_decimal_is_not_approximating_a_value_on_upper_boundary_it_should_throw()
        {
            // Act
            decimal value = 11m;

            // Act
            Action act = () => value.Should().BeApproximately(10m, 0.9m);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_decimal_is_approximating_a_range_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            decimal value = 3.5011m;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.5m, 0.1m, "rockets will crash otherwise");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to not approximate *3.5* +/- *0.1* because rockets will crash otherwise, but *3.5011* only differed by *0.0011*");
        }

        [Fact]
        public void When_decimal_is_not_approximating_a_value_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            decimal value = 3.5011m;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.5m, 0.001m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_decimal_has_no_value_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            decimal? value = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.Should().NotBeApproximately(3.5m, 0.001m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_decimal_is_not_approximating_a_value_on_lower_boundary_it_should_not_throw()
        {
            // Act
            decimal value = 9m;

            // Act
            Action act = () => value.Should().NotBeApproximately(10m, 0.9m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_decimal_is_not_approximating_a_value_on_upper_boundary_it_should_not_throw()
        {
            // Act
            decimal value = 11m;

            // Act
            Action act = () => value.Should().NotBeApproximately(10m, 0.9m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_decimal_is_approximating_a_value_on_lower_boundary_it_should_throw()
        {
            // Act
            decimal value = 9m;

            // Act
            Action act = () => value.Should().NotBeApproximately(10m, 1m);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_decimal_is_approximating_a_value_on_upper_boundary_it_should_throw()
        {
            // Act
            decimal value = 11m;

            // Act
            Action act = () => value.Should().NotBeApproximately(10m, 1m);

            // Assert
            act.Should().Throw<XunitException>();
        }

        #endregion

        #endregion

        #region CloseTo

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
        public void When_a_sbyte_value_is_close_to_expected_value_it_should_succeed(sbyte actual, sbyte nearbyValue, byte delta)
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
        public void When_a_sbyte_value_is_not_close_to_expected_value_it_should_fail(sbyte actual, sbyte nearbyValue, byte delta)
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
        public void When_a_short_value_is_close_to_expected_value_it_should_succeed(short actual, short nearbyValue, ushort delta)
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
        public void When_a_short_value_is_not_close_to_expected_value_it_should_fail(short actual, short nearbyValue, ushort delta)
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
        [InlineData(long.MinValue, long.MinValue, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MinValue, long.MinValue, ulong.MaxValue / 2)]
        [InlineData(long.MinValue, long.MinValue, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MinValue, long.MinValue, ulong.MaxValue)]
        [InlineData(long.MinValue, long.MinValue + 1, 1)]
        [InlineData(long.MinValue, long.MinValue + 1, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MinValue, long.MinValue + 1, ulong.MaxValue / 2)]
        [InlineData(long.MinValue, long.MinValue + 1, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MinValue, long.MinValue + 1, ulong.MaxValue)]
        [InlineData(long.MinValue, -1, long.MaxValue)]
        [InlineData(long.MinValue + 1, long.MinValue, 1)]
        [InlineData(long.MinValue + 1, long.MinValue, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MinValue + 1, long.MinValue, ulong.MaxValue / 2)]
        [InlineData(long.MinValue + 1, long.MinValue, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MinValue + 1, long.MinValue, ulong.MaxValue)]
        [InlineData(long.MinValue + 1, 0, ulong.MaxValue / 2)]
        [InlineData(long.MinValue + 1, 0, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MinValue + 1, 0, ulong.MaxValue)]
        [InlineData(long.MinValue, long.MaxValue, ulong.MaxValue)]
        [InlineData(-1, long.MinValue, ulong.MaxValue / 2)]
        [InlineData(-1, long.MinValue, ulong.MaxValue / 2 + 1)]
        [InlineData(-1, long.MinValue, ulong.MaxValue)]
        [InlineData(-1, 0, 1)]
        [InlineData(-1, 0, ulong.MaxValue / 2 - 1)]
        [InlineData(-1, 0, ulong.MaxValue / 2)]
        [InlineData(-1, 0, ulong.MaxValue / 2 + 1)]
        [InlineData(-1, 0, ulong.MaxValue)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, -1, 1)]
        [InlineData(0, -1, ulong.MaxValue / 2 - 1)]
        [InlineData(0, -1, ulong.MaxValue / 2)]
        [InlineData(0, -1, ulong.MaxValue / 2 + 1)]
        [InlineData(0, -1, ulong.MaxValue)]
        [InlineData(0, 1, 1)]
        [InlineData(0, 1, ulong.MaxValue / 2 - 1)]
        [InlineData(0, 1, ulong.MaxValue / 2)]
        [InlineData(0, 1, ulong.MaxValue / 2 + 1)]
        [InlineData(0, 1, ulong.MaxValue)]
        [InlineData(0, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(0, long.MaxValue, ulong.MaxValue / 2 + 1)]
        [InlineData(0, long.MaxValue, ulong.MaxValue)]
        [InlineData(0, long.MinValue + 1, ulong.MaxValue / 2)]
        [InlineData(0, long.MinValue + 1, ulong.MaxValue / 2 + 1)]
        [InlineData(0, long.MinValue + 1, ulong.MaxValue)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 0, ulong.MaxValue / 2 - 1)]
        [InlineData(1, 0, ulong.MaxValue / 2)]
        [InlineData(1, 0, ulong.MaxValue / 2 + 1)]
        [InlineData(1, 0, ulong.MaxValue)]
        [InlineData(1, long.MaxValue, ulong.MaxValue / 2 - 1)]
        [InlineData(1, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(1, long.MaxValue, ulong.MaxValue / 2 + 1)]
        [InlineData(1, long.MaxValue, ulong.MaxValue)]
        [InlineData(long.MaxValue - 1, long.MaxValue, 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue - 1, long.MaxValue, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, ulong.MaxValue)]
        [InlineData(long.MaxValue, 0, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, 0, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MaxValue, 0, ulong.MaxValue)]
        [InlineData(long.MaxValue, 1, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MaxValue, 1, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, 1, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MaxValue, 1, ulong.MaxValue)]
        [InlineData(long.MaxValue, long.MaxValue, 0)]
        [InlineData(long.MaxValue, long.MaxValue, 1)]
        [InlineData(long.MaxValue, long.MaxValue, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MaxValue, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, long.MaxValue, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MaxValue, long.MaxValue, ulong.MaxValue)]
        [InlineData(long.MaxValue, long.MaxValue - 1, 1)]
        [InlineData(long.MaxValue, long.MaxValue - 1, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MaxValue, long.MaxValue - 1, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, long.MaxValue - 1, ulong.MaxValue / 2 + 1)]
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
        [InlineData(long.MinValue + 1, 0, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MinValue, long.MaxValue, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MinValue, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(-1, 0, 0)]
        [InlineData(-1, 1, 1)]
        [InlineData(-1, long.MaxValue, long.MaxValue)]
        [InlineData(-1, long.MinValue, ulong.MaxValue / 2 - 1)]
        [InlineData(0, long.MinValue, long.MaxValue)]
        [InlineData(0, long.MinValue + 1, ulong.MaxValue / 2 - 1)]
        [InlineData(0, long.MaxValue, ulong.MaxValue / 2 - 1)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 0, 0)]
        [InlineData(1, long.MinValue, long.MaxValue)]
        [InlineData(long.MaxValue, long.MinValue, 1)]
        [InlineData(long.MaxValue, -1, long.MaxValue)]
        [InlineData(long.MaxValue, 0, ulong.MaxValue / 2 - 1)]
        [Theory]
        public void When_a_long_value_is_not_close_to_expected_value_it_should_fail(long actual, long nearbyValue, ulong delta)
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
        public void When_an_ushort_value_is_close_to_expected_value_it_should_succeed(ushort actual, ushort nearbyValue, ushort delta)
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
        public void When_an_ushort_value_is_not_close_to_expected_value_it_should_fail(ushort actual, ushort nearbyValue, ushort delta)
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
        public void When_an_uint_value_is_not_close_to_expected_value_it_should_fail(uint actual, uint nearbyValue, uint delta)
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
        public void When_an_ulong_value_is_close_to_expected_value_it_should_succeed(ulong actual, ulong nearbyValue, ulong delta)
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
        public void When_an_ulong_value_is_not_close_to_expected_value_it_should_fail(ulong actual, ulong nearbyValue, ulong delta)
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

        #endregion

        #region NotBeCloseTo
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
        public void When_a_sbyte_value_is_not_close_to_expected_value_it_should_succeed(sbyte actual, sbyte distantValue, byte delta)
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
        public void When_a_short_value_is_not_close_to_expected_value_it_should_succeed(short actual, short distantValue, ushort delta)
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
        public void When_a_short_value_is_close_to_expected_value_it_should_fail(short actual, short distantValue, ushort delta)
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
        public void When_an_int_value_is_not_close_to_expected_value_it_should_succeed(int actual, int distantValue, uint delta)
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
        [InlineData(long.MinValue + 1, 0, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MinValue, long.MaxValue, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MinValue, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(-1, 0, 0)]
        [InlineData(-1, 1, 1)]
        [InlineData(-1, long.MaxValue, long.MaxValue)]
        [InlineData(-1, long.MinValue, ulong.MaxValue / 2 - 1)]
        [InlineData(0, long.MinValue, long.MaxValue)]
        [InlineData(0, long.MinValue + 1, ulong.MaxValue / 2 - 1)]
        [InlineData(0, long.MaxValue, ulong.MaxValue / 2 - 1)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 0, 0)]
        [InlineData(1, long.MinValue, long.MaxValue)]
        [InlineData(long.MaxValue, long.MinValue, 1)]
        [InlineData(long.MaxValue, -1, long.MaxValue)]
        [InlineData(long.MaxValue, 0, ulong.MaxValue / 2 - 1)]
        [Theory]
        public void When_a_long_value_is_not_close_to_expected_value_it_should_succeed(long actual, long distantValue, ulong delta)
        {
            // Act
            Action act = () => actual.Should().NotBeCloseTo(distantValue, delta);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(long.MinValue, long.MinValue, 0)]
        [InlineData(long.MinValue, long.MinValue, 1)]
        [InlineData(long.MinValue, long.MinValue, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MinValue, long.MinValue, ulong.MaxValue / 2)]
        [InlineData(long.MinValue, long.MinValue, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MinValue, long.MinValue, ulong.MaxValue)]
        [InlineData(long.MinValue, long.MinValue + 1, 1)]
        [InlineData(long.MinValue, long.MinValue + 1, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MinValue, long.MinValue + 1, ulong.MaxValue / 2)]
        [InlineData(long.MinValue, long.MinValue + 1, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MinValue, long.MinValue + 1, ulong.MaxValue)]
        [InlineData(long.MinValue, -1, long.MaxValue)]
        [InlineData(long.MinValue + 1, long.MinValue, 1)]
        [InlineData(long.MinValue + 1, long.MinValue, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MinValue + 1, long.MinValue, ulong.MaxValue / 2)]
        [InlineData(long.MinValue + 1, long.MinValue, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MinValue + 1, long.MinValue, ulong.MaxValue)]
        [InlineData(long.MinValue + 1, 0, ulong.MaxValue / 2)]
        [InlineData(long.MinValue + 1, 0, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MinValue + 1, 0, ulong.MaxValue)]
        [InlineData(long.MinValue, long.MaxValue, ulong.MaxValue)]
        [InlineData(-1, long.MinValue, ulong.MaxValue / 2)]
        [InlineData(-1, long.MinValue, ulong.MaxValue / 2 + 1)]
        [InlineData(-1, long.MinValue, ulong.MaxValue)]
        [InlineData(-1, 0, 1)]
        [InlineData(-1, 0, ulong.MaxValue / 2 - 1)]
        [InlineData(-1, 0, ulong.MaxValue / 2)]
        [InlineData(-1, 0, ulong.MaxValue / 2 + 1)]
        [InlineData(-1, 0, ulong.MaxValue)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, -1, 1)]
        [InlineData(0, -1, ulong.MaxValue / 2 - 1)]
        [InlineData(0, -1, ulong.MaxValue / 2)]
        [InlineData(0, -1, ulong.MaxValue / 2 + 1)]
        [InlineData(0, -1, ulong.MaxValue)]
        [InlineData(0, 1, 1)]
        [InlineData(0, 1, ulong.MaxValue / 2 - 1)]
        [InlineData(0, 1, ulong.MaxValue / 2)]
        [InlineData(0, 1, ulong.MaxValue / 2 + 1)]
        [InlineData(0, 1, ulong.MaxValue)]
        [InlineData(0, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(0, long.MaxValue, ulong.MaxValue / 2 + 1)]
        [InlineData(0, long.MaxValue, ulong.MaxValue)]
        [InlineData(0, long.MinValue + 1, ulong.MaxValue / 2)]
        [InlineData(0, long.MinValue + 1, ulong.MaxValue / 2 + 1)]
        [InlineData(0, long.MinValue + 1, ulong.MaxValue)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 0, ulong.MaxValue / 2 - 1)]
        [InlineData(1, 0, ulong.MaxValue / 2)]
        [InlineData(1, 0, ulong.MaxValue / 2 + 1)]
        [InlineData(1, 0, ulong.MaxValue)]
        [InlineData(1, long.MaxValue, ulong.MaxValue / 2 - 1)]
        [InlineData(1, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(1, long.MaxValue, ulong.MaxValue / 2 + 1)]
        [InlineData(1, long.MaxValue, ulong.MaxValue)]
        [InlineData(long.MaxValue - 1, long.MaxValue, 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue - 1, long.MaxValue, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, ulong.MaxValue)]
        [InlineData(long.MaxValue, 0, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, 0, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MaxValue, 0, ulong.MaxValue)]
        [InlineData(long.MaxValue, 1, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MaxValue, 1, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, 1, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MaxValue, 1, ulong.MaxValue)]
        [InlineData(long.MaxValue, long.MaxValue, 0)]
        [InlineData(long.MaxValue, long.MaxValue, 1)]
        [InlineData(long.MaxValue, long.MaxValue, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MaxValue, long.MaxValue, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, long.MaxValue, ulong.MaxValue / 2 + 1)]
        [InlineData(long.MaxValue, long.MaxValue, ulong.MaxValue)]
        [InlineData(long.MaxValue, long.MaxValue - 1, 1)]
        [InlineData(long.MaxValue, long.MaxValue - 1, ulong.MaxValue / 2 - 1)]
        [InlineData(long.MaxValue, long.MaxValue - 1, ulong.MaxValue / 2)]
        [InlineData(long.MaxValue, long.MaxValue - 1, ulong.MaxValue / 2 + 1)]
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
        public void When_a_byte_value_is_not_close_to_expected_value_it_should_succeed(byte actual, byte distantValue, byte delta)
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
        public void When_an_ushort_value_is_not_close_to_expected_value_it_should_succeed(ushort actual, ushort distantValue, ushort delta)
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
        public void When_an_ushort_value_is_close_to_expected_value_it_should_fail(ushort actual, ushort distantValue, ushort delta)
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
        public void When_an_uint_value_is_not_close_to_expected_value_it_should_succeed(uint actual, uint distantValue, uint delta)
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
        public void When_an_ulong_value_is_not_close_to_expected_value_it_should_succeed(ulong actual, ulong distantValue, ulong delta)
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
        public void When_an_ulong_value_is_close_to_expected_value_it_should_fail(ulong actual, ulong distantValue, ulong delta)
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

        #endregion

        #region Match


        [Fact]
        public void When_value_satisfies_predicate_it_should_not_throw()
        {
            // Arrange
            int value = 1;

            // Act / Assert
            value.Should().Match(o => o > 0);
        }

        [Fact]
        public void When_value_does_not_match_the_predicate_it_should_throw()
        {
            // Arrange
            int value = 1;

            // Act
            Action act = () => value.Should().Match(o => o == 0, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected value to match (o == 0) because we want to test the failure message, but found 1.");
        }

        [Fact]
        public void When_value_is_matched_against_a_null_it_should_throw()
        {
            // Arrange
            int value = 1;

            // Act
            Action act = () => value.Should().Match(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("predicate");
        }

        #endregion

        [Fact]
        public void When_chaining_constraints_with_and_should_not_throw()
        {
            // Arrange
            int value = 2;
            int greaterValue = 3;
            int smallerValue = 1;

            // Act
            Action action = () => value.Should()
                .BePositive()
                .And
                .BeGreaterThan(smallerValue)
                .And
                .BeLessThan(greaterValue);

            // Assert
            action.Should().NotThrow();
        }
    }
}
