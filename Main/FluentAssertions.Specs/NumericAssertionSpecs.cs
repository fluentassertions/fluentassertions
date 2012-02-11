using System;

using FluentAssertions.Assertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Specs
{
    /// <summary>
    /// Summary description for CustomAssertionSpecs
    /// </summary>
    [TestClass]
    public class NumericAssertionSpecs
    {
        #region Positive / Negative

        [TestMethod]
        public void Should_succeed_when_asserting_positive_value_to_be_positive()
        {
            (1).Should().BePositive();
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void Should_fail_when_asserting_negative_value_to_be_positive()
        {
            (-1).Should().BePositive();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_negative_value_to_be_positive()
        {
            var assertions = (-1).Should();
            assertions.Invoking(x => x.BePositive("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected positive value because we want to test the failure message, but found -1");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_negative_value_to_be_negative()
        {
            (-1).Should().BeNegative();
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void Should_fail_when_asserting_positive_value_to_be_negative()
        {
            1.Should().BeNegative();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_positive_value_to_be_negative()
        {
            var assertions = (1).Should();
            assertions.Invoking(x => x.BeNegative("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected negative value because we want to test the failure message, but found 1");
        }

        #endregion

        #region Be / NotBe

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_equal_to_same_value()
        {
            1.Should().Be(1);
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void Should_fail_when_asserting_value_to_be_equal_to_different_value()
        {
            1.Should().Be(2);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_equal_to_different_value()
        {
            var assertions = 1.Should();
            assertions.Invoking(x => x.Be(2, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected 2 because we want to test the failure message, but found 1.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_not_equal_to_different_value()
        {
            1.Should().NotBe(2);
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void Should_fail_when_asserting_value_to_be_not_equal_to_the_same_value()
        {
            1.Should().NotBe(1);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_not_equal_to_the_same_value()
        {
            var assertions = 1.Should();
            assertions.Invoking(x => x.NotBe(1, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Did not expect 1 because we want to test the failure message.");
        }

        #endregion

        #region Greater Than (Or Equal To)

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_greater_than_smaller_value()
        {
            2.Should().BeGreaterThan(1);
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void Should_fail_when_asserting_value_to_be_greater_than_greater_value()
        {
            2.Should().BeGreaterThan(3);
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void Should_fail_when_asserting_value_to_be_greater_than_same_value()
        {
            2.Should().BeGreaterThan(2);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_greater_than_greater_value()
        {
            var assertions = 2.Should();
            assertions.Invoking(x => x.BeGreaterThan(3, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected a value greater than 3 because we want to test the failure message, but found 2.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_greater_or_equal_to_smaller_value()
        {
            2.Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_greater_or_equal_to_same_value()
        {
            2.Should().BeGreaterOrEqualTo(2);
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void Should_fail_when_asserting_value_to_be_greater_or_equal_to_greater_value()
        {
            2.Should().BeGreaterOrEqualTo(3);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_greater_or_equal_to_greater_value()
        {
            var assertions = 2.Should();
            assertions.Invoking(x => x.BeGreaterOrEqualTo(3, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected a value greater or equal to 3 because we want to test the failure message, but found 2.");
        }

        #endregion

        #region Less Than (Or Equal To)

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_less_than_greater_value()
        {
            1.Should().BeLessThan(2);
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void Should_fail_when_asserting_value_to_be_less_than_smaller_value()
        {
            2.Should().BeLessThan(1);
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void Should_fail_when_asserting_value_to_be_less_than_same_value()
        {
            2.Should().BeLessThan(2);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_less_than_smaller_value()
        {
            var assertions = 2.Should();
            assertions.Invoking(x => x.BeLessThan(1, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected a value less than 1 because we want to test the failure message, but found 2.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_less_or_equal_to_greater_value()
        {
            1.Should().BeLessOrEqualTo(2);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_less_or_equal_to_same_value()
        {
            2.Should().BeLessOrEqualTo(2);
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void Should_fail_when_asserting_value_to_be_less_or_equal_to_smaller_value()
        {
            2.Should().BeLessOrEqualTo(1);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_less_or_equal_to_smaller_value()
        {
            var assertions = 2.Should();
            assertions.Invoking(x => x.BeLessOrEqualTo(1, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected a value less or equal to 1 because we want to test the failure message, but found 2.");
        }

        #endregion

        #region In Range

        [TestMethod]
        public void When_a_value_is_outside_a_range_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            float value = 3.99F;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeInRange(4, 5, "because that's the valid range");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(string.Format(
                "Expected value {0} to be between {1} and {2} because that's the valid range, but it was not.", value, 4, 5));
        }
        
        [TestMethod]
        public void When_a_value_is_inside_a_range_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            int value = 4;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeInRange(3, 5);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region Bytes

        [TestMethod]
        public void When_asserting_a_byte_value_it_should_treat_is_any_numeric_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            byte value = 2;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().Be(2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }
        
        [TestMethod]
        public void When_asserting_a_short_value_it_should_treat_is_any_numeric_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Int16 value = 2;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().Be(2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region Floating Point

        [TestMethod]
        public void When_asserting_that_a_float_value_is_equal_to_a_different_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            float value = 3.5F;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().Be(3.4F, "we want to test the error message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format("Expected value {0} to be exactly {1} because we want to test the error message.",
                    value, 3.4F));
        }

        [TestMethod]
        public void When_asserting_that_a_float_value_is_equal_to_the_same_value_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            float value = 3.5F;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().Be(3.5F);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_that_a_null_float_value_is_equal_to_some_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            float? value = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().Be(3.5F);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(string.Format(
                "Expected value to be exactly {0}, but it was <null>.", 3.5));
        }

        [TestMethod]
        public void When_float_is_not_approximating_a_range_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            float value = 3.1415927F;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(3.14F, 0.001F, "rockets will crash otherwise");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            float difference = Math.Abs(value - 3.14F);
            
            act.ShouldThrow<AssertFailedException>().WithMessage(string.Format(
                "Expected value {0} to approximate {1} +/- {2} because rockets will crash otherwise, but it differed by {3}.",
                value, 3.14F, 0.001F, difference));
        }

        [TestMethod]
        public void When_float_is_indeed_approximating_a_value_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            float value = 3.1415927F;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(3.14F, 0.1F);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_nullable_float_has_no_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            float? value = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(3.14F, 0.001F);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(string.Format(
                "Expected value to approximate {0} +/- {1}, but it was <null>.", 3.14, 0.001));
        }

        [TestMethod]
        public void When_asserting_that_a_double_value_is_equal_to_a_different_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            double value = 3.5;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().Be(3.4, "we want to test the error message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format("Expected value {0} to be exactly {1} because we want to test the error message.",
                    value, 3.4));
        }

        [TestMethod]
        public void When_asserting_that_a_double_value_is_equal_to_the_same_value_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            double value = 3.5;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().Be(3.5);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_that_a_null_double_value_is_equal_to_some_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            double? value = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().Be(3.5);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(string.Format(
                "Expected value to be exactly {0}, but it was <null>.", 3.5));
        }
        
        [TestMethod]
        public void When_double_is_not_approximating_a_range_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            double value = 3.1415927;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(3.14, 0.001, "rockets will crash otherwise");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            double difference = Math.Abs(value - 3.14);
            
            act.ShouldThrow<AssertFailedException>().WithMessage(string.Format(
                "Expected value {0} to approximate {1} +/- {2} because rockets will crash otherwise, but it differed by {3}.",
                value, 3.14, 0.001, difference));
        }

        [TestMethod]
        public void When_double_is_indeed_approximating_a_value_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            double value = 3.1415927;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => value.Should().BeApproximately(3.14, 0.1);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_nullable_double_has_no_value_it_should_throw()
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
            act.ShouldThrow<AssertFailedException>().WithMessage(string.Format(
                "Expected value to approximate {0} +/- {1}, but it was <null>.", 3.14, 0.001));
        }

        #endregion

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            2.Should()
                .BePositive()
                .And
                .BeGreaterThan(1)
                .And
                .BeLessThan(3);
        }
    }
}