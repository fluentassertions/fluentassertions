using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    
    public class SimpleTimeSpanAssertionSpecs
    {
        [Fact]
        public void When_asserting_positive_value_to_be_positive_it_should_succeed()
        {
            1.Seconds().Should().BePositive();
        }

        [Fact]
        public void When_asserting_negative_value_to_be_positive_it_should_fail()
        {
            Action act = () => 1.Seconds().Negate().Should().BePositive();
            act.ShouldThrow<XunitException>();

        }

        [Fact]
        public void When_asserting_negative_value_to_be_positive_it_should_fail_with_descriptive_message()
        {
            var assertions = 1.Seconds().Negate().Should();
            assertions.Invoking(x => x.BePositive("because we want to test the failure {0}", "message"))
                .ShouldThrow<XunitException>()
                .WithMessage("Expected positive value because we want to test the failure message, but found -1s");
        }

        [Fact]
        public void When_asserting_negative_value_to_be_negative_it_should_succeed()
        {
            1.Seconds().Negate().Should().BeNegative();
        }

        [Fact]
        public void When_asserting_positive_value_to_be_negative_it_should_fail()
        {
            Action act = () => 1.Seconds().Should().BeNegative();
            act.ShouldThrow<XunitException>();

        }

        [Fact]
        public void When_asserting_positive_value_to_be_negative_it_should_fail_with_descriptive_message()
        {
            var assertions = 1.Seconds().Should();
            assertions.Invoking(x => x.BeNegative("because we want to test the failure {0}", "message"))
                .ShouldThrow<XunitException>()
                .WithMessage("Expected negative value because we want to test the failure message, but found 1s");
        }

        [Fact]
        public void When_asserting_value_to_be_equal_to_same_value_it_should_succeed()
        {
            1.Seconds().Should().Be(TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void When_asserting_value_to_be_equal_to_different_value_it_should_fail()
        {
            Action act = () => 1.Seconds().Should().Be(2.Seconds());
            act.ShouldThrow<XunitException>();

        }

        [Fact]
        public void When_asserting_value_to_be_equal_to_different_value_it_should_fail_with_descriptive_message()
        {
            var assertions = 1.Seconds().Should();
            assertions.Invoking(x => x.Be(2.Seconds(), "because we want to test the failure {0}", "message"))
                .ShouldThrow<XunitException>()
                .WithMessage(@"Expected 2s because we want to test the failure message, but found 1s.");
        }

        [Fact]
        public void When_asserting_value_to_be_not_equal_to_different_value_it_should_succeed()
        {
            1.Seconds().Should().NotBe(2.Seconds());
        }

        [Fact]
        public void When_asserting_value_to_be_not_equal_to_the_same_value_it_should_fail()
        {
            var oneSecond = 1.Seconds();
            Action act = () => oneSecond.Should().NotBe(oneSecond);
            act.ShouldThrow<XunitException>();

        }

        [Fact]
        public void When_asserting_value_to_be_not_equal_to_the_same_value_it_should_fail_with_descriptive_message()
        {
            var oneSecond = 1.Seconds();
            var assertions = oneSecond.Should();
            assertions.Invoking(x => x.NotBe(oneSecond, "because we want to test the failure {0}", "message"))
                .ShouldThrow<XunitException>()
                .WithMessage(@"Did not expect 1s because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_value_to_be_greater_than_smaller_value_it_should_succeed()
        {
            2.Seconds().Should().BeGreaterThan(1.Seconds());
        }

        [Fact]
        public void When_asserting_value_to_be_greater_than_greater_value_it_should_fail()
        {
            Action act = () => 1.Seconds().Should().BeGreaterThan(2.Seconds());
            act.ShouldThrow<XunitException>();

        }

        [Fact]
        public void When_asserting_value_to_be_greater_than_same_value_it_should_fail()
        {
            var twoSeconds = 2.Seconds();
            Action act = () => twoSeconds.Should().BeGreaterThan(twoSeconds);
            act.ShouldThrow<XunitException>();

        }

        [Fact]
        public void When_asserting_value_to_be_greater_than_greater_value_it_should_fail_with_descriptive_message()
        {
            var assertions = 1.Seconds().Should();
            assertions.Invoking(x => x.BeGreaterThan(2.Seconds(), "because we want to test the failure {0}", "message"))
                .ShouldThrow<XunitException>()
                .WithMessage(@"Expected a value greater than 2s because we want to test the failure message, but found 1s.");
        }

        [Fact]
        public void When_asserting_value_to_be_greater_or_equal_to_smaller_value_it_should_succeed()
        {
            2.Seconds().Should().BeGreaterOrEqualTo(1.Seconds());
        }

        [Fact]
        public void When_asserting_value_to_be_greater_or_equal_to_same_value_it_should_succeed()
        {
            var twoSeconds = 2.Seconds();
            twoSeconds.Should().BeGreaterOrEqualTo(twoSeconds);
        }

        [Fact]
        public void When_asserting_value_to_be_greater_or_equal_to_greater_value_it_should_fail()
        {
            Action act = () => 1.Seconds().Should().BeGreaterOrEqualTo(2.Seconds());
            act.ShouldThrow<XunitException>();

        }

        [Fact]
        public void When_asserting_value_to_be_greater_or_equal_to_greater_value_it_should_fail_with_descriptive_message()
        {
            var assertions = 1.Seconds().Should();
            assertions.Invoking(x => x.BeGreaterOrEqualTo(2.Seconds(), "because we want to test the failure {0}", "message"))
                .ShouldThrow<XunitException>()
                .WithMessage(@"Expected a value greater or equal to 2s because we want to test the failure message, but found 1s.");
        }

        [Fact]
        public void When_asserting_value_to_be_less_than_greater_value_it_should_succeed()
        {
            1.Seconds().Should().BeLessThan(2.Seconds());
        }

        [Fact]
        public void When_asserting_value_to_be_less_than_smaller_value_it_should_fail()
        {
            Action act = () => 2.Seconds().Should().BeLessThan(1.Seconds());
            act.ShouldThrow<XunitException>();

        }

        [Fact]
        public void When_asserting_value_to_be_less_than_same_value_it_should_fail()
        {
            var twoSeconds = 2.Seconds();
            Action act = () => twoSeconds.Should().BeLessThan(twoSeconds);
            act.ShouldThrow<XunitException>();

        }

        [Fact]
        public void When_asserting_value_to_be_less_than_smaller_value_it_should_fail_with_descriptive_message()
        {
            var assertions = 2.Seconds().Should();
            assertions.Invoking(x => x.BeLessThan(1.Seconds(), "because we want to test the failure {0}", "message"))
                .ShouldThrow<XunitException>()
                .WithMessage(@"Expected a value less than 1s because we want to test the failure message, but found 2s.");
        }

        [Fact]
        public void When_asserting_value_to_be_less_or_equal_to_greater_value_it_should_succeed()
        {
            1.Seconds().Should().BeLessOrEqualTo(2.Seconds());
        }

        [Fact]
        public void When_asserting_value_to_be_less_or_equal_to_same_value_it_should_succeed()
        {
            var twoSeconds = 2.Seconds();
            twoSeconds.Should().BeLessOrEqualTo(twoSeconds);
        }

        [Fact]
        public void When_asserting_value_to_be_less_or_equal_to_smaller_value_it_should_fail()
        {
            Action act = () => 2.Seconds().Should().BeLessOrEqualTo(1.Seconds());
            act.ShouldThrow<XunitException>();

        }

        [Fact]
        public void When_asserting_value_to_be_less_or_equal_to_smaller_value_it_should_fail_with_descriptive_message()
        {
            var assertions = 2.Seconds().Should();
            assertions.Invoking(x => x.BeLessOrEqualTo(1.Seconds(), "because we want to test the failure {0}", "message"))
                .ShouldThrow<XunitException>()
                .WithMessage(@"Expected a value less or equal to 1s because we want to test the failure message, but found 2s.");
        }

        #region Be Close To

        [Fact]
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

        [Fact]
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

        [Fact]
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
            act.ShouldThrow<XunitException>()
                .WithMessage(
                    "Expected time to be within 0.020s from 1d, 12h, 15m and 31s because we want to test the error message, but found 1d, 12h, 15m and 30.979s.");
        }

        [Fact]
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
            act.ShouldThrow<XunitException>()
                .WithMessage(
                    "Expected time to be within 0.020s from 1d, 12h, 15m and 31s because we want to test the error message, but found 1d, 12h, 15m and 31.021s.");
        }

        [Fact]
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

        [Fact]
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
            act.ShouldThrow<XunitException>()
                .WithMessage("Expected*, but found <null>.");
        }

        [Fact]
        public void When_time_away_from_another_value_it_should_throw_with_descriptive_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var time = new TimeSpan(1, 12, 15, 30, 979);
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().BeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(20), "we want to test the error message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage(
                    "Expected time to be within 0.020s from 1d, 12h, 15m and 31s because we want to test the error message, but found 1d, 12h, 15m and 30.979s.");
        }

        #endregion

        #region Not Be Close To

        [Fact]
        public void When_asserting_subject_time_is_not_close_to_a_later_time_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var time = new TimeSpan(1, 12, 15, 30, 980);
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().NotBeCloseTo(nearbyTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage("Expected time to not be within 0.020s from 1d, 12h, 15m and 31s, but found 1d, 12h, 15m and 30.980s.");
        }

        [Fact]
        public void When_asserting_subject_time_is_not_close_to_an_earlier_time_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var time = new TimeSpan(1, 12, 15, 31, 020);
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().NotBeCloseTo(nearbyTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage("Expected time to not be within 0.020s from 1d, 12h, 15m and 31s, but found 1d, 12h, 15m and 31.020s.");
        }

        [Fact]
        public void When_asserting_subject_time_is_not_close_to_an_earlier_time_by_a_20ms_timespan_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var time = new TimeSpan(1, 12, 15, 31, 020);
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(20));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage("Expected time to not be within 0.020s from 1d, 12h, 15m and 31s, but found 1d, 12h, 15m and 31.020s.");
        }

        [Fact]
        public void When_asserting_subject_time_is_not_close_to_another_value_that_is_later_by_more_than_20ms_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var time = new TimeSpan(1, 12, 15, 30, 979);
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().NotBeCloseTo(nearbyTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_subject_time_is_not_close_to_another_value_that_is_earlier_by_more_than_20ms_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var time = new TimeSpan(1, 12, 15, 31, 021);
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().NotBeCloseTo(nearbyTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_subject_time_is_not_close_to_an_ealier_time_by_35ms_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var time = new TimeSpan(1, 12, 15, 31, 035);
            var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 35);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage("Expected time to not be within 0.035s from 1d, 12h, 15m and 31s, but found 1d, 12h, 15m and 31.035s.");
        }

        [Fact]
        public void When_asserting_subject_null_time_is_not_close_to_another_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            TimeSpan? time = null;
            TimeSpan nearbyTime = TimeSpan.FromHours(1);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 35);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage("Expected*, but found <null>.");
        }

        #endregion
    }
}
