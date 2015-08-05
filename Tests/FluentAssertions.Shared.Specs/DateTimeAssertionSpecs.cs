using System;
using FluentAssertions.Primitives;

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class DateTimeAssertionSpecs
    {
        private DateTime Today { get; set; }
        private DateTime Yesterday { get; set; }
        private DateTime Tomorrow { get; set; }

        [TestInitialize]
        public void InitializeTest()
        {
            Today = DateTime.Today;
            Yesterday = Today.AddDays(-1);
            Tomorrow = Today.AddDays(1);
        }

        #region (Not) Have Value

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_datetime_value_with_a_value_to_have_a_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime? nullableDateTime = Today;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => nullableDateTime.Should().HaveValue();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_fail_when_asserting_nullable_datetime_value_without_a_value_to_have_a_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime? nullableDateTime = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => nullableDateTime.Should().HaveValue();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_datetime_value_without_a_value_to_be_null()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime? nullableDateTime = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableDateTime.Should().NotHaveValue();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_fail_when_asserting_nullable_datetime_value_with_a_value_to_be_null()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime? nullableDateTime = Today;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableDateTime.Should().NotHaveValue();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>();
        }

        #endregion

        #region Be / NotBe

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_value_is_equal_to_the_same_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime dateTime = Today;
            DateTime sameDateTime = Today;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dateTime.Should().Be(sameDateTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }
        
        [TestMethod]
        public void When_both_values_are_at_their_minimum_then_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime dateTime = DateTime.MinValue;
            DateTime sameDateTime = DateTime.MinValue;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dateTime.Should().Be(sameDateTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }
        
        [TestMethod]
        public void When_both_values_are_at_their_maximum_then_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime dateTime = DateTime.MaxValue;
            DateTime sameDateTime = DateTime.MaxValue;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dateTime.Should().Be(sameDateTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_value_is_equal_to_the_different_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dateTime = new DateTime(2012, 03, 10);
            var otherDateTime = new DateTime(2012, 03, 11);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dateTime.Should().Be(otherDateTime, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected date and time to be <2012-03-11>*failure message, but found <2012-03-10>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_value_is_not_equal_to_a_different_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime dateTime = Today;
            DateTime otherDateTime = Tomorrow;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dateTime.Should().NotBe(otherDateTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_value_is_not_equal_to_the_same_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dateTime = DateTime.SpecifyKind(10.March(2012).At(10, 00), DateTimeKind.Local);
            var sameDateTime = DateTime.SpecifyKind(10.March(2012).At(10, 00), DateTimeKind.Utc);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => dateTime.Should().NotBe(sameDateTime, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect date and time to be <2012-03-10 10:00:00> because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_numeric_value_equals_the_same_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime? nullableDateTimeA = Today;
            DateTime? nullableDateTimeB = Today;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableDateTimeA.Should().Be(nullableDateTimeB);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }
        
        [TestMethod]
        public void When_a_nullable_date_time_is_equal_to_a_normal_date_time_but_the_kinds_differ_it_should_still_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime? nullableDateTime = new DateTime(2014, 4, 20, 9, 11, 0, DateTimeKind.Unspecified);
            DateTime normalDateTime = new DateTime(2014, 4, 20, 9, 11, 0, DateTimeKind.Utc);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableDateTime.Should().Be(normalDateTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }
        
        [TestMethod]
        public void When_two_date_times_are_equal_but_the_kinds_differ_it_should_still_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime dateTimeA = new DateTime(2014, 4, 20, 9, 11, 0, DateTimeKind.Unspecified);
            DateTime dateTimeB = new DateTime(2014, 4, 20, 9, 11, 0, DateTimeKind.Utc);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                dateTimeA.Should().Be(dateTimeB);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_numeric_null_value_equals_null()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime? nullableDateTimeA = null;
            DateTime? nullableDateTimeB = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableDateTimeA.Should().Be(nullableDateTimeB);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_fail_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime? nullableDateTimeA = Today;
            DateTime? nullableDateTimeB = Today.AddDays(2);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableDateTimeA.Should().Be(nullableDateTimeB);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_null_value_is_equal_to_another_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime? nullableDateTime = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableDateTime.Should().Be(Today, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format("Expected <{0}> because we want to test the failure message, but found <null>.",
                    Today.ToString("yyyy-MM-dd")));
        }

        #endregion

        #region Be Close To

        [TestMethod]
        public void When_datetime_is_less_then_but_close_to_another_value_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime time = DateTime.SpecifyKind(Today.At(12, 15, 30, 980), DateTimeKind.Unspecified);
            DateTime nearbyTime = DateTime.SpecifyKind(Today.At(12, 15, 31), DateTimeKind.Utc);

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
        public void When_datetime_is_greater_then_but_close_to_another_value_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime time = Today.At(12, 15, 31, 020);
            DateTime nearbyTime = Today.At(12, 15, 31);

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
        public void When_datetime_is_less_then_and_not_close_to_another_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime time = 13.March(2012).At(12, 15, 30, 979);
            DateTime nearbyTime = 13.March(2012).At(12, 15, 31);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().BeCloseTo(nearbyTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected date and time to be within 20 ms from <2012-03-13 12:15:31>, but found <2012-03-13 12:15:30.979>.");
        }

        [TestMethod]
        public void When_datetime_is_greater_then_and_not_close_to_another_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime time = 13.March(2012).At(12, 15, 31, 021);
            DateTime nearbyTime = 13.March(2012).At(12, 15, 31);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().BeCloseTo(nearbyTime);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected date and time to be within 20 ms from <2012-03-13 12:15:31>, but found <2012-03-13 12:15:31.021>.");
        }

        [TestMethod]
        public void When_datetime_is_within_specified_number_of_milliseconds_from_another_value_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime time = Today.At(12, 15, 31, 035);
            DateTime nearbyTime = Today.At(12, 15, 31);

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
        public void When_a_null_date_time_is_asserted_to_be_close_to_another_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime? time = null;
            DateTime nearbyTime = Today.At(12, 15, 31);

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

        [TestMethod]
        public void When_a_date_time_is_close_to_the_minimum_date_time_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime time = DateTime.MinValue.Add(50.Milliseconds());
            DateTime nearbyTime = DateTime.MinValue;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().BeCloseTo(nearbyTime, 100);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_date_time_is_close_to_the_maximum_date_time_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime time = DateTime.MaxValue.Add(-50.Milliseconds());
            DateTime nearbyTime = DateTime.MaxValue;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => time.Should().BeCloseTo(nearbyTime, 100);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        [TestMethod]
        public void When_a_point_of_time_occurs_before_another_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime earlierDate = DateTime.SpecifyKind(Today, DateTimeKind.Unspecified);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime laterDate = DateTime.SpecifyKind(Today.AddMinutes(5), DateTimeKind.Utc);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            earlierDate.Should().BeBefore(laterDate);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_is_before_earlier_datetime()
        {
            Action act = () => Today.Should().BeBefore(Yesterday);
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_is_before_earlier_datetime()
        {
            DateTimeAssertions assertions = Today.Should();
            assertions.Invoking(x => x.BeBefore(Yesterday, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format(
                    "Expected a date and time before <{0}> because we want to test the failure message, but found <{1}>.",
                    Yesterday.ToString("yyyy-MM-dd"), Today.ToString("yyyy-MM-dd")));
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_is_on_or_before_later_datetime()
        {
            Today.Should().BeOnOrBefore(Tomorrow);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_is_on_or_before_the_same_datetime()
        {
            Today.Should().BeOnOrBefore(Today);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_is_on_or_before_earlier_datetime()
        {
            Action act = () => Today.Should().BeOnOrBefore(Yesterday);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_is_on_or_before_earlier_datetime()
        {
            DateTimeAssertions assertions = Today.Should();
            assertions.Invoking(x => x.BeOnOrBefore(Yesterday, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format(
                    "Expected a date and time on or before <{0}> because we want to test the failure message, but found <{1}>.",
                    Yesterday.ToString("yyyy-MM-dd"), Today.ToString("yyyy-MM-dd")));
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_is_after_earlier_datetime()
        {
            Today.Should().BeAfter(Yesterday);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_is_after_later_datetime()
        {
            Action act = () => Today.Should().BeAfter(Tomorrow);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_is_after_later_datetime()
        {
            DateTimeAssertions assertions = Today.Should();
            assertions.Invoking(x => x.BeAfter(Tomorrow, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format(
                    "Expected a date and time after <{0}> because we want to test the failure message, but found <{1}>.",
                    Tomorrow.ToString("yyyy-MM-dd"), Today.ToString("yyyy-MM-dd")));
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_is_on_or_after_earlier_datetime()
        {
            Today.Should().BeOnOrAfter(Yesterday);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_is_on_or_after_the_same_datetime()
        {
            Today.Should().BeOnOrAfter(Today);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_is_on_or_after_later_datetime()
        {
            Action act = () => Today.Should().BeOnOrAfter(Tomorrow);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_is_on_or_after_later_datetime()
        {
            DateTimeAssertions assertions = Today.Should();
            assertions.Invoking(x => x.BeOnOrAfter(Tomorrow, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format(
                    "Expected a date and time on or after <{0}> because we want to test the failure message, but found <{1}>.",
                    Tomorrow.ToString("yyyy-MM-dd"), Today.ToString("yyyy-MM-dd")));
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_a_year_with_the_same_value()
        {
            new DateTime(2009, 12, 31).Should().HaveYear(2009);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_has_a_year_with_a_different_value()
        {
            Action act = () => new DateTime(2009, 12, 31).Should().HaveYear(2008);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_a_year_with_a_different_value()
        {
            DateTimeAssertions assertions = new DateTime(2009, 12, 31).Should();
            assertions.Invoking(x => x.HaveYear(2008, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected year 2008 because we want to test the failure message, but found 2009.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_a_month_with_the_same_value()
        {
            new DateTime(2009, 12, 31).Should().HaveMonth(12);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_has_a_month_with_a_different_value()
        {
            Action act = () => new DateTime(2009, 12, 31).Should().HaveMonth(11);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_a_month_with_a_different_value()
        {
            DateTimeAssertions assertions = new DateTime(2009, 12, 31).Should();
            assertions.Invoking(x => x.HaveMonth(11, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected month 11 because we want to test the failure message, but found 12.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_a_day_with_the_same_value()
        {
            new DateTime(2009, 12, 31).Should().HaveDay(31);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_has_a_day_with_a_different_value()
        {
            Action act = () => new DateTime(2009, 12, 31).Should().HaveDay(30);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_a_day_with_a_different_value()
        {
            DateTimeAssertions assertions = new DateTime(2009, 12, 31).Should();
            assertions.Invoking(x => x.HaveDay(30, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected day 30 because we want to test the failure message, but found 31.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_an_hour_with_the_same_value()
        {
            new DateTime(2009, 12, 31, 23, 59, 00).Should().HaveHour(23);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_has_an_hour_with_different_value()
        {
            Action act = () => new DateTime(2009, 12, 31, 23, 59, 00).Should().HaveHour(22);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_an_hour_with_different_value()
        {
            DateTimeAssertions assertions = new DateTime(2009, 12, 31, 23, 59, 00).Should();
            assertions.Invoking(x => x.HaveHour(22, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected hour 22 because we want to test the failure message, but found 23.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_minutes_with_the_same_value()
        {
            new DateTime(2009, 12, 31, 23, 59, 00).Should().HaveMinute(59);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_has_minutes_with_different_value()
        {
            Action act = () => new DateTime(2009, 12, 31, 23, 59, 00).Should().HaveMinute(58);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_minutes_with_different_value()
        {
            DateTimeAssertions assertions = new DateTime(2009, 12, 31, 23, 59, 00).Should();
            assertions.Invoking(x => x.HaveMinute(58, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected minute 58 because we want to test the failure message, but found 59.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_seconds_with_the_same_value()
        {
            new DateTime(2009, 12, 31, 23, 59, 00).Should().HaveSecond(0);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_has_seconds_with_different_value()
        {
            Action act = () => new DateTime(2009, 12, 31, 23, 59, 00).Should().HaveSecond(1);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_seconds_with_different_value()
        {
            DateTimeAssertions assertions = new DateTime(2009, 12, 31, 23, 59, 00).Should();
            assertions.Invoking(x => x.HaveSecond(1, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected second 1 because we want to test the failure message, but found 0.");
        }

        #region Timespan Comparison

        [TestMethod]
        public void When_date_is_not_more_than_the_required_one_day_before_another_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var target = new DateTime(2009, 10, 2);
            DateTime subject = target.AddDays(-1);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeMoreThan(TimeSpan.FromDays(1)).Before(target, "we like {0}", "that");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected date and/or time <2009-10-01> to be more than 1d before <2009-10-02> because we like that, but it differs 1d.");
        }

        [TestMethod]
        public void When_date_is_more_than_the_required_one_day_before_another_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var target = new DateTime(2009, 10, 2);
            DateTime subject = target.AddHours(-25);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().BeMoreThan(TimeSpan.FromDays(1)).Before(target);
        }

        [TestMethod]
        public void When_date_is_not_at_least_one_day_before_another_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var target = new DateTime(2009, 10, 2);
            DateTime subject = target.AddHours(-23);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeAtLeast(TimeSpan.FromDays(1)).Before(target, "we like {0}", "that");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected date and/or time <2009-10-01 01:00:00> to be at least 1d before <2009-10-02> because we like that, but it differs 23h.");
        }

        [TestMethod]
        public void When_date_is_at_least_one_day_before_another_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var target = new DateTime(2009, 10, 2);
            DateTime subject = target.AddHours(-24);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().BeAtLeast(TimeSpan.FromDays(1)).Before(target);
        }

        [TestMethod]
        public void When_time_is_not_at_exactly_20_minutes_before_another_time_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime target = DateTime.Parse("0001/1/1 12:55");
            DateTime subject = DateTime.Parse("0001/1/1 12:36");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => subject.Should().BeExactly(TimeSpan.FromMinutes(20)).Before(target, "{0} minutes is enough", 20);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected date and/or time <12:36:00> to be exactly 20m before <12:55:00> because 20 minutes is enough, but it differs 19m.");
        }

        [TestMethod]
        public void When_time_is_exactly_90_seconds_before_another_time_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime target = DateTime.Parse("0001/1/1 12:55:00");
            DateTime subject = DateTime.Parse("0001/1/1 12:53:30");

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().BeExactly(TimeSpan.FromSeconds(90)).Before(target);
        }

        [TestMethod]
        public void When_date_is_not_within_50_hours_before_another_date_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var target = new DateTime(2010, 4, 10, 12, 0, 0);
            DateTime subject = target.AddHours(-50).AddSeconds(-1);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => subject.Should().BeWithin(TimeSpan.FromHours(50)).Before(target, "{0} hours is enough", 50);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected date and/or time <2010-04-08 09:59:59> to be within 2d and 2h before <2010-04-10 12:00:00> because 50 hours is enough, but it differs 2d, 2h and 1s.");
        }

        [TestMethod]
        public void When_date_is_exactly_within_1d_before_another_date_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var target = new DateTime(2010, 4, 10);
            DateTime subject = target.AddDays(-1);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().BeWithin(TimeSpan.FromHours(24)).Before(target);
        }

        [TestMethod]
        public void When_date_is_within_1d_before_another_date_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var target = new DateTime(2010, 4, 10);
            DateTime subject = target.AddHours(-23);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().BeWithin(TimeSpan.FromHours(24)).Before(target);
        }

        [TestMethod]
        public void When_a_utc_date_is_within_0s_before_itself_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var date = DateTime.UtcNow; // local timezone differs from UTC

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().BeWithin(TimeSpan.Zero).Before(date);
        }        
        
        [TestMethod]
        public void When_a_utc_date_is_within_0s_after_itself_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var date = DateTime.UtcNow; // local timezone differs from UTC

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            date.Should().BeWithin(TimeSpan.Zero).After(date);
        }

        [TestMethod]
        public void When_time_is_not_less_than_30s_after_another_time_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var target = new DateTime(1, 1, 1, 12, 0, 30);
            DateTime subject = target.AddSeconds(30);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => subject.Should().BeLessThan(TimeSpan.FromSeconds(30)).After(target, "{0}s is the max", 30);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected date and/or time <12:01:00> to be less than 30s after <12:00:30> because 30s is the max, but it differs 30s.");
        }

        [TestMethod]
        public void When_time_is_less_than_30s_after_another_time_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var target = new DateTime(1, 1, 1, 12, 0, 30);
            DateTime subject = target.AddSeconds(20);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().BeLessThan(TimeSpan.FromSeconds(30)).After(target);
        }

        #endregion

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime yesterday = Today.AddDays(-1);
            DateTime? nullableDateTime = Today;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableDateTime.Should()
                    .HaveValue()
                    .And
                    .BeAfter(yesterday);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }
    }
}