using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions.Common;

#if WINRT || WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class SimpleTimeSpanAssertionSpecs
    {
        [TestMethod]
        public void When_asserting_positive_value_to_be_positive_it_should_succeed()
        {
            1.Seconds().Should().BePositive();
        }

        [TestMethod]
        public void When_asserting_negative_value_to_be_positive_it_should_fail()
        {
            Action act = () => 1.Seconds().Negate().Should().BePositive();
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void When_asserting_negative_value_to_be_positive_it_should_fail_with_descriptive_message()
        {
            var assertions = 1.Seconds().Negate().Should();
            assertions.Invoking(x => x.BePositive("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected positive value because we want to test the failure message, but found -1s");
        }

        [TestMethod]
        public void When_asserting_negative_value_to_be_negative_it_should_succeed()
        {
            1.Seconds().Negate().Should().BeNegative();
        }

        [TestMethod]
        public void When_asserting_positive_value_to_be_negative_it_should_fail()
        {
            Action act = () => 1.Seconds().Should().BeNegative();
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void When_asserting_positive_value_to_be_negative_it_should_fail_with_descriptive_message()
        {
            var assertions = 1.Seconds().Should();
            assertions.Invoking(x => x.BeNegative("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected negative value because we want to test the failure message, but found 1s");
        }

        [TestMethod]
        public void When_asserting_value_to_be_equal_to_same_value_it_should_succeed()
        {
            1.Seconds().Should().Be(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void When_asserting_value_to_be_equal_to_different_value_it_should_fail()
        {
            Action act = () => 1.Seconds().Should().Be(2.Seconds());
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void When_asserting_value_to_be_equal_to_different_value_it_should_fail_with_descriptive_message()
        {
            var assertions = 1.Seconds().Should();
            assertions.Invoking(x => x.Be(2.Seconds(), "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected 2s because we want to test the failure message, but found 1s.");
        }

        [TestMethod]
        public void When_asserting_value_to_be_not_equal_to_different_value_it_should_succeed()
        {
            1.Seconds().Should().NotBe(2.Seconds());
        }

        [TestMethod]
        public void When_asserting_value_to_be_not_equal_to_the_same_value_it_should_fail()
        {
            var oneSecond = 1.Seconds();
            Action act = () => oneSecond.Should().NotBe(oneSecond);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void When_asserting_value_to_be_not_equal_to_the_same_value_it_should_fail_with_descriptive_message()
        {
            var oneSecond = 1.Seconds();
            var assertions = oneSecond.Should();
            assertions.Invoking(x => x.NotBe(oneSecond, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Did not expect 1s because we want to test the failure message.");
        }

        [TestMethod]
        public void When_asserting_value_to_be_greater_than_smaller_value_it_should_succeed()
        {
            2.Seconds().Should().BeGreaterThan(1.Seconds());
        }

        [TestMethod]
        public void When_asserting_value_to_be_greater_than_greater_value_it_should_fail()
        {
            Action act = () => 1.Seconds().Should().BeGreaterThan(2.Seconds());
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void When_asserting_value_to_be_greater_than_same_value_it_should_fail()
        {
            var twoSeconds = 2.Seconds();
            Action act = () => twoSeconds.Should().BeGreaterThan(twoSeconds);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void When_asserting_value_to_be_greater_than_greater_value_it_should_fail_with_descriptive_message()
        {
            var assertions = 1.Seconds().Should();
            assertions.Invoking(x => x.BeGreaterThan(2.Seconds(), "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected a value greater than 2s because we want to test the failure message, but found 1s.");
        }

        [TestMethod]
        public void When_asserting_value_to_be_greater_or_equal_to_smaller_value_it_should_succeed()
        {
            2.Seconds().Should().BeGreaterOrEqualTo(1.Seconds());
        }

        [TestMethod]
        public void When_asserting_value_to_be_greater_or_equal_to_same_value_it_should_succeed()
        {
            var twoSeconds = 2.Seconds();
            twoSeconds.Should().BeGreaterOrEqualTo(twoSeconds);
        }

        [TestMethod]
        public void When_asserting_value_to_be_greater_or_equal_to_greater_value_it_should_fail()
        {
            Action act = () => 1.Seconds().Should().BeGreaterOrEqualTo(2.Seconds());
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void When_asserting_value_to_be_greater_or_equal_to_greater_value_it_should_fail_with_descriptive_message()
        {
            var assertions = 1.Seconds().Should();
            assertions.Invoking(x => x.BeGreaterOrEqualTo(2.Seconds(), "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected a value greater or equal to 2s because we want to test the failure message, but found 1s.");
        }

        [TestMethod]
        public void When_asserting_value_to_be_less_than_greater_value_it_should_succeed()
        {
            1.Seconds().Should().BeLessThan(2.Seconds());
        }

        [TestMethod]
        public void When_asserting_value_to_be_less_than_smaller_value_it_should_fail()
        {
            Action act = () => 2.Seconds().Should().BeLessThan(1.Seconds());
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void When_asserting_value_to_be_less_than_same_value_it_should_fail()
        {
            var twoSeconds = 2.Seconds();
            Action act = () => twoSeconds.Should().BeLessThan(twoSeconds);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void When_asserting_value_to_be_less_than_smaller_value_it_should_fail_with_descriptive_message()
        {
            var assertions = 2.Seconds().Should();
            assertions.Invoking(x => x.BeLessThan(1.Seconds(), "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected a value less than 1s because we want to test the failure message, but found 2s.");
        }

        [TestMethod]
        public void When_asserting_value_to_be_less_or_equal_to_greater_value_it_should_succeed()
        {
            1.Seconds().Should().BeLessOrEqualTo(2.Seconds());
        }

        [TestMethod]
        public void When_asserting_value_to_be_less_or_equal_to_same_value_it_should_succeed()
        {
            var twoSeconds = 2.Seconds();
            twoSeconds.Should().BeLessOrEqualTo(twoSeconds);
        }

        [TestMethod]
        public void When_asserting_value_to_be_less_or_equal_to_smaller_value_it_should_fail()
        {
            Action act = () => 2.Seconds().Should().BeLessOrEqualTo(1.Seconds());
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void When_asserting_value_to_be_less_or_equal_to_smaller_value_it_should_fail_with_descriptive_message()
        {
            var assertions = 2.Seconds().Should();
            assertions.Invoking(x => x.BeLessOrEqualTo(1.Seconds(), "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(@"Expected a value less or equal to 1s because we want to test the failure message, but found 2s.");
        }

        #region Be Close To

        [TestMethod]
        public void When_time_is_less_then_but_close_to_another_value_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var time = new TimeSpan(1, 12, 15, 30, 980);
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().BeCloseTo(nearbyTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_time_is_greater_then_but_close_to_another_value_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var time = new TimeSpan(1, 12, 15, 31, 020);
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().BeCloseTo(nearbyTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_time_is_less_then_and_not_close_to_another_value_it_should_throw_with_descriptive_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var time = new TimeSpan(1, 12, 15, 30, 979);
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().BeCloseTo(nearbyTime, 20, "we want to test the error message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected time to be within 20 ms from 1d, 12h, 15m and 31s because we want to test the error message, but found 1d, 12h, 15m and 30.979s.");
        }

        [TestMethod]
        public void When_time_is_greater_then_and_not_close_to_another_value_it_should_throw_with_descriptive_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var time = new TimeSpan(1, 12, 15, 31, 021);
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().BeCloseTo(nearbyTime, 20, "we want to test the error message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected time to be within 20 ms from 1d, 12h, 15m and 31s because we want to test the error message, but found 1d, 12h, 15m and 31.021s.");
        }

        [TestMethod]
        public void When_time_is_within_specified_number_of_milliseconds_from_another_value_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var time = new TimeSpan(1, 12, 15, 31, 035);
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().BeCloseTo(nearbyTime, 35);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_null_time_is_asserted_to_be_close_to_another_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            TimeSpan? time = null;
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().BeCloseTo(nearbyTime, 35);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected*, but found <null>.");
        }

        #endregion
    }
}
