using System;
using FluentAssertions.Primitives;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class NullableNumericAssertionSpecs
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_with_value_to_have_a_value()
        {
            int? nullableInteger = 1;
            nullableInteger.Should().HaveValue();
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_with_value_to_not_be_null()
        {
            int? nullableInteger = 1;
            nullableInteger.Should().NotBeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_without_a_value_to_have_a_value()
        {
            int? nullableInteger = null;
            Action act = () => nullableInteger.Should().HaveValue();
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_without_a_value_to_not_be_null()
        {
            int? nullableInteger = null;
            Action act = () => nullableInteger.Should().NotBeNull();
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_without_a_value_to_have_a_value()
        {
            int? nullableInteger = null;
            var assertions = nullableInteger.Should();
            assertions.Invoking(x => x.HaveValue("because we want to test the failure {0}", "message"))
                .Should().Throw<XunitException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_without_a_value_to_not_be_null()
        {
            int? nullableInteger = null;
            var assertions = nullableInteger.Should();
            assertions.Invoking(x => x.NotBeNull("because we want to test the failure {0}", "message"))
                .Should().Throw<XunitException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_without_a_value_to_not_have_a_value()
        {
            int? nullableInteger = null;
            nullableInteger.Should().NotHaveValue();
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_without_a_value_to_be_null()
        {
            int? nullableInteger = null;
            nullableInteger.Should().BeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_with_a_value_to_not_have_a_value()
        {
            int? nullableInteger = 1;

            Action act = () => nullableInteger.Should().NotHaveValue();
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_with_a_value_to_be_null()
        {
            int? nullableInteger = 1;

            Action act = () => nullableInteger.Should().BeNull();
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_nullable_value_with_unexpected_value_is_found_it_should_throw_with_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            int? nullableInteger = 1;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => nullableInteger.Should().NotHaveValue("it was {0} expected", "not");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action
                .Should().Throw<XunitException>()
                .WithMessage("Did not expect a value because it was not expected, but found 1.");
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_with_a_value_to_be_null()
        {
            int? nullableInteger = 1;
            var assertions = nullableInteger.Should();
            assertions.Invoking(x => x.BeNull("because we want to test the failure {0}", "message"))
                .Should().Throw<XunitException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found 1.");
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_equals_an_equal_value()
        {
            int? nullableIntegerA = 1;
            int? nullableIntegerB = 1;
            nullableIntegerA.Should().Be(nullableIntegerB);
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_null_value_equals_null()
        {
            int? nullableIntegerA = null;
            int? nullableIntegerB = null;
            nullableIntegerA.Should().Be(nullableIntegerB);
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            int? nullableIntegerA = 1;
            int? nullableIntegerB = 2;

            Action act = () => nullableIntegerA.Should().Be(nullableIntegerB);
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            int? nullableIntegerA = 1;
            int? nullableIntegerB = 2;
            var assertions = nullableIntegerA.Should();
            assertions.Invoking(x => x.Be(nullableIntegerB, "because we want to test the failure {0}", "message"))
                .Should().Throw<XunitException>()
                .WithMessage("Expected*2 because we want to test the failure message, but found 1.");
        }

        #region Be Approximately

        [Fact]
        public void When_nullable_double_is_indeed_approximating_a_value_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            double? value = 3.1415927;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(3.14, 0.1);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_double_has_no_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            double? value = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(3.14, 0.001);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to approximate 3.14 +/- 0.001, but it was <null>.");
        }

        [Fact]
        public void When_nullable_double_is_not_approximating_a_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            double? value = 3.1415927F;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(1.0, 0.1);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to approximate 1.0 +/- 0.1, but 3.14* differed by*");
        }

        [Fact]
        public void When_nullable_float_is_indeed_approximating_a_value_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            float? value = 3.1415927F;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(3.14F, 0.1F);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_float_is_not_approximating_a_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            float? value = 3.1415927F;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(1.0F, 0.1F);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to approximate *1* +/- *0.1* but 3.14* differed by*");
        }

        [Fact]
        public void When_nullable_decimal_is_indeed_approximating_a_value_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            decimal? value = 3.1415927m;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(3.14m, 0.1m);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_decimal_has_no_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            decimal? value = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(3.14m, 0.001m);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>().WithMessage("Expected value to approximate*3.14* +/-*0.001*, but it was <null>.");
        }

        [Fact]
        public void When_nullable_decimal_is_not_approximating_a_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            decimal? value = 3.1415927m;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(1.0m, 0.1m);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to approximate*1.0* +/-*0.1*, but 3.14* differed by*");
        }

        #endregion

        #region Not Be Approximately

        [Fact]
        public void When_asserting_not_approximately_and_nullable_double_is_not_approximating_a_value_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            double? value = 3.1415927;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().NotBeApproximately(1.0, 0.1);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_double_has_no_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            double? value = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().NotBeApproximately(3.14, 0.001);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to not approximate 3.14 +/- 0.001, but it was <null>.");
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_double_is_indeed_approximating_a_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            double? value = 3.1415927;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().NotBeApproximately(3.14, 0.1);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to not approximate 3.14 +/- 0.1, but 3.14*only differed by*");
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_float_is_not_approximating_a_value_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            float? value = 3.1415927F;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().NotBeApproximately(1.0F, 0.1F);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_float_has_no_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            float? value = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().NotBeApproximately(3.14F, 0.001F);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to not approximate 3.14F* +/- 0.001F*, but it was <null>.");
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_float_is_indeed_approximating_a_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            float? value = 3.1415927F;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().NotBeApproximately(3.14F, 0.1F);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to not approximate *3.14F* +/- *0.1F* but 3.14* only differed by*");
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_decimal_is_not_approximating_a_value_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            decimal? value = 3.1415927m;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().NotBeApproximately(1.0m, 0.1m);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_decimal_has_no_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            decimal? value = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().NotBeApproximately(3.14m, 0.001m);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to not approximate*3.14* +/-*0.001*, but it was <null>.");
        }

        [Fact]
        public void When_asserting_not_approximately_and_nullable_decimal_is_indeed_approximating_a_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            decimal? value = 3.1415927m;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().NotBeApproximately(3.14m, 0.1m);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to not approximate*3.14* +/-*0.1*, but*3.14*only differed by*");
        }

        #endregion

        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            int? nullableInteger = 1;
            nullableInteger.Should()
                .HaveValue()
                .And
                .BePositive();
        }
    }
}
