using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class NullableNumericAssertionSpecs
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_with_value_to_have_a_value()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act / Assert
            nullableInteger.Should().HaveValue();
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_with_value_to_not_be_null()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act / Assert
            nullableInteger.Should().NotBeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_without_a_value_to_have_a_value()
        {
            // Arrange
            int? nullableInteger = null;

            // Act
            Action act = () => nullableInteger.Should().HaveValue();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_without_a_value_to_not_be_null()
        {
            // Arrange
            int? nullableInteger = null;

            // Act
            Action act = () => nullableInteger.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_without_a_value_to_have_a_value()
        {
            // Arrange
            int? nullableInteger = null;

            // Act
            Action act = () => nullableInteger.Should().HaveValue("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_without_a_value_to_not_be_null()
        {
            // Arrange
            int? nullableInteger = null;

            // Act
            Action act = () => nullableInteger.Should().NotBeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_without_a_value_to_not_have_a_value()
        {
            // Arrange
            int? nullableInteger = null;

            // Act / Assert
            nullableInteger.Should().NotHaveValue();
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_without_a_value_to_be_null()
        {
            // Arrange
            int? nullableInteger = null;

            // Act / Assert
            nullableInteger.Should().BeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_with_a_value_to_not_have_a_value()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act
            Action act = () => nullableInteger.Should().NotHaveValue();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_with_a_value_to_be_null()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act
            Action act = () => nullableInteger.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_nullable_value_with_unexpected_value_is_found_it_should_throw_with_message()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act
            Action action = () => nullableInteger.Should().NotHaveValue("it was {0} expected", "not");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Did not expect a value because it was not expected, but found 1.");
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_with_a_value_to_be_null()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act
            Action act = () => nullableInteger.Should().BeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found 1.");
        }

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
            Action act = () => nullableIntegerA.Should().Be(nullableIntegerB, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*2 because we want to test the failure message, but found 1.");
        }

        #region Be Approximately

        [Fact]
        public void When_nullable_double_is_indeed_approximating_a_value_it_should_not_throw()
        {
            // Arrange
            double? value = 3.1415927;

            // Act
            Action act = () => value.Should().BeApproximately(3.14, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_double_is_indeed_approximating_a_nullable_value_it_should_not_throw()
        {
            // Arrange
            double? value = 3.1415927;
            double? expected = 3.142;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_double_is_null_approximating_a_nullable_null_value_it_should_not_throw()
        {
            // Arrange
            double? value = null;
            double? expected = null;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_double_with_value_is_not_approximating_a_non_null_nullable_value_it_should_throw()
        {
            // Arrange
            double? value = 13;
            double? expected = 12;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*12.0*0.1*13.0*");
        }

        [Fact]
        public void When_nullable_double_is_null_approximating_a_non_null_nullable_value_it_should_throw()
        {
            // Arrange
            double? value = null;
            double? expected = 12;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to approximate 12.0 +/- 0.1, but it was <null>.");
        }

        [Fact]
        public void When_nullable_double_is_not_null_approximating_a_null_value_it_should_throw()
        {
            // Arrange
            double? value = 12;
            double? expected = null;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to approximate <null> +/- 0.1, but it was 12.0.");
        }

        [Fact]
        public void When_nullable_double_has_no_value_it_should_throw()
        {
            // Arrange
            double? value = null;

            // Act
            Action act = () => value.Should().BeApproximately(3.14, 0.001);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to approximate 3.14 +/- 0.001, but it was <null>.");
        }

        [Fact]
        public void When_nullable_double_is_not_approximating_a_value_it_should_throw()
        {
            // Arrange
            double? value = 3.1415927F;

            // Act
            Action act = () => value.Should().BeApproximately(1.0, 0.1);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to approximate 1.0 +/- 0.1, but 3.14* differed by*");
        }

        [Fact]
        public void When_nullable_float_is_indeed_approximating_a_value_it_should_not_throw()
        {
            // Arrange
            float? value = 3.1415927F;

            // Act
            Action act = () => value.Should().BeApproximately(3.14F, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_float_is_indeed_approximating_a_nullable_value_it_should_not_throw()
        {
            // Arrange
            float? value = 3.1415927f;
            float? expected = 3.142f;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1f);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_float_is_null_approximating_a_nullable_null_value_it_should_not_throw()
        {
            // Arrange
            float? value = null;
            float? expected = null;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1f);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_float_with_value_is_not_approximating_a_non_null_nullable_value_it_should_throw()
        {
            // Arrange
            float? value = 13;
            float? expected = 12;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1f);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*12*0.1*13*");
        }

        [Fact]
        public void When_nullable_float_is_null_approximating_a_non_null_nullable_value_it_should_throw()
        {
            // Arrange
            float? value = null;
            float? expected = 12;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1f);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to approximate 12F +/- 0.1F, but it was <null>.");
        }

        [Fact]
        public void When_nullable_float_is_not_null_approximating_a_null_value_it_should_throw()
        {
            // Arrange
            float? value = 12;
            float? expected = null;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1f);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to approximate <null> +/- 0.1F, but it was 12F.");
        }

        [Fact]
        public void When_nullable_float_is_not_approximating_a_value_it_should_throw()
        {
            // Arrange
            float? value = 3.1415927F;

            // Act
            Action act = () => value.Should().BeApproximately(1.0F, 0.1F);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to approximate *1* +/- *0.1* but 3.14* differed by*");
        }

        [Fact]
        public void When_nullable_decimal_is_indeed_approximating_a_value_it_should_not_throw()
        {
            // Arrange
            decimal? value = 3.1415927m;

            // Act
            Action act = () => value.Should().BeApproximately(3.14m, 0.1m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_decimal_is_indeed_approximating_a_nullable_value_it_should_not_throw()
        {
            // Arrange
            decimal? value = 3.1415927m;
            decimal? expected = 3.142m;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_decimal_is_null_approximating_a_nullable_null_value_it_should_not_throw()
        {
            // Arrange
            decimal? value = null;
            decimal? expected = null;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_decimal_with_value_is_not_approximating_a_non_null_nullable_value_it_should_throw()
        {
            // Arrange
            decimal? value = 13;
            decimal? expected = 12;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1m);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*12*0.1*13*");
        }

        [Fact]
        public void When_nullable_decimal_is_null_approximating_a_non_null_nullable_value_it_should_throw()
        {
            // Arrange
            decimal? value = null;
            decimal? expected = 12;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1m);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to approximate 12M +/- 0.1M, but it was <null>.");
        }

        [Fact]
        public void When_nullable_decimal_is_not_null_approximating_a_null_value_it_should_throw()
        {
            // Arrange
            decimal? value = 12;
            decimal? expected = null;

            // Act
            Action act = () => value.Should().BeApproximately(expected, 0.1m);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to approximate <null> +/- 0.1M, but it was 12M.");
        }

        [Fact]
        public void When_nullable_decimal_has_no_value_it_should_throw()
        {
            // Arrange
            decimal? value = null;

            // Act
            Action act = () => value.Should().BeApproximately(3.14m, 0.001m);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected value to approximate*3.14* +/-*0.001*, but it was <null>.");
        }

        [Fact]
        public void When_nullable_decimal_is_not_approximating_a_value_it_should_throw()
        {
            // Arrange
            decimal? value = 3.1415927m;

            // Act
            Action act = () => value.Should().BeApproximately(1.0m, 0.1m);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to approximate*1.0* +/-*0.1*, but 3.14* differed by*");
        }

        #endregion

        #region Not Be Approximately

        [Fact]
        public void When_asserting_not_approximately_and_nullable_double_is_not_approximating_a_value_it_should_not_throw()
        {
            // Arrange
            double? value = 3.1415927;

            // Act
            Action act = () => value.Should().NotBeApproximately(1.0, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_double_has_no_value_it_should_throw()
        {
            // Arrange
            double? value = null;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14, 0.001);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_double_is_indeed_approximating_a_value_it_should_throw()
        {
            // Arrange
            double? value = 3.1415927;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14, 0.1);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to not approximate 3.14 +/- 0.1, but 3.14*only differed by*");
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_double_is_not_approximating_a_nullable_value_it_should_not_throw()
        {
            // Arrange
            double? value = 3.1415927;
            double? expected = 1.0;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_double_is_not_approximating_a_null_value_it_should_throw()
        {
            // Arrange
            double? value = 3.1415927;
            double? expected = null;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_null_double_is_not_approximating_a_nullable_double_value_it_should_throw()
        {
            // Arrange
            double? value = null;
            double? expected = 20.0;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_null_double_is_not_approximating_a_null_value_it_should_not_throw()
        {
            // Arrange
            double? value = null;
            double? expected = null;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*null*0.1*but*null*");
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_double_is_approximating_a_nullable_value_it_should_throw()
        {
            // Arrange
            double? value = 3.1415927;
            double? expected = 3.1;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_float_is_not_approximating_a_value_it_should_not_throw()
        {
            // Arrange
            float? value = 3.1415927F;

            // Act
            Action act = () => value.Should().NotBeApproximately(1.0F, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_float_has_no_value_it_should_throw()
        {
            // Arrange
            float? value = null;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14F, 0.001F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_float_is_indeed_approximating_a_value_it_should_throw()
        {
            // Arrange
            float? value = 3.1415927F;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14F, 0.1F);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to not approximate *3.14F* +/- *0.1F* but 3.14* only differed by*");
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_float_is_not_approximating_a_nullable_value_it_should_not_throw()
        {
            // Arrange
            float? value = 3.1415927F;
            float? expected = 1.0F;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_float_is_not_approximating_a_null_value_it_should_throw()
        {
            // Arrange
            float? value = 3.1415927F;
            float? expected = null;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_null_float_is_not_approximating_a_nullable_float_value_it_should_throw()
        {
            // Arrange
            float? value = null;
            float? expected = 20.0f;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_null_float_is_not_approximating_a_null_value_it_should_not_throw()
        {
            // Arrange
            float? value = null;
            float? expected = null;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1F);

            // Assert
            act.Should().Throw<XunitException>("Expected*<null>*+/-*0.1F*<null>*");
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_float_is_approximating_a_nullable_value_it_should_throw()
        {
            // Arrange
            float? value = 3.1415927F;
            float? expected = 3.1F;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_decimal_is_not_approximating_a_value_it_should_not_throw()
        {
            // Arrange
            decimal? value = 3.1415927m;

            // Act
            Action act = () => value.Should().NotBeApproximately(1.0m, 0.1m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_decimal_has_no_value_it_should_throw()
        {
            // Arrange
            decimal? value = null;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14m, 0.001m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_decimal_is_indeed_approximating_a_value_it_should_throw()
        {
            // Arrange
            decimal? value = 3.1415927m;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14m, 0.1m);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to not approximate*3.14* +/-*0.1*, but*3.14*only differed by*");
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_decimal_is_not_approximating_a_nullable_value_it_should_not_throw()
        {
            // Arrange
            decimal? value = 3.1415927m;
            decimal? expected = 1.0m;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_decimal_is_not_approximating_a_null_value_it_should_throw()
        {
            // Arrange
            decimal? value = 3.1415927m;
            decimal? expected = null;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_null_decimal_is_not_approximating_a_nullable_decimal_value_it_should_throw()
        {
            // Arrange
            decimal? value = null;
            decimal? expected = 20.0m;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_null_decimal_is_not_approximating_a_null_value_it_should_not_throw()
        {
            // Arrange
            decimal? value = null;
            decimal? expected = null;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1m);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*<null>*0.1M*<null>*");
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_decimal_is_approximating_a_nullable_value_it_should_throw()
        {
            // Arrange
            decimal? value = 3.1415927m;
            decimal? expected = 3.1m;

            // Act
            Action act = () => value.Should().NotBeApproximately(expected, 0.1m);

            // Assert
            act.Should().Throw<XunitException>();
        }

        #endregion

        #region Match


        [Fact]
        public void When_nullable_value_satisfies_predicate_it_should_not_throw()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act / Assert
            nullableInteger.Should().Match(o => o.HasValue);
        }

        [Fact]
        public void When_nullable_value_does_not_match_the_predicate_it_should_throw()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act
            Action act = () => nullableInteger.Should().Match(o => !o.HasValue, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected value to match Not(o.HasValue) because we want to test the failure message, but found 1.");
        }

        [Fact]
        public void When_nullable_value_is_matched_against_a_null_it_should_throw()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act
            Action act = () => nullableInteger.Should().Match(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("predicate");
        }

        #endregion

        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act / Assert
            nullableInteger.Should()
                .HaveValue()
                .And
                .BePositive();
        }
    }
}
