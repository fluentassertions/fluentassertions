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
    public class DateTimeOffsetAssertionSpecs
    {
        private DateTimeOffset Today { get; set; }
        private DateTimeOffset Yesterday { get; set; }
        private DateTimeOffset Tomorrow { get; set; }

        [TestInitialize]
        public void InitializeTest()
        {
            Today = DateTime.Today;
            Yesterday = Today.AddDays(-1);
            Tomorrow = Today.AddDays(1);
        }

        #region (Not) Have Value

        [TestMethod]
        public void When_nullable_datetime_value_with_a_value_to_have_a_value_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTimeOffset? nullableDateTime = Today;

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
            DateTimeOffset? nullableDateTime = null;

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
            DateTimeOffset? nullableDateTime = null;

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
            DateTimeOffset? nullableDateTime = Today;

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
            DateTimeOffset dateTime = Today;
            DateTimeOffset sameDateTime = Today;

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
            DateTimeOffset dateTime = DateTimeOffset.MinValue;
            DateTimeOffset sameDateTime = DateTimeOffset.MinValue;

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
            DateTimeOffset dateTime = DateTimeOffset.MaxValue;
            DateTimeOffset sameDateTime = DateTimeOffset.MaxValue;

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
            var dateTime = new DateTimeOffset(10.March(2012));
            var otherDateTime = new DateTimeOffset(11.March(2012));

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
            DateTimeOffset dateTime = Today;
            DateTimeOffset otherDateTime = Tomorrow;

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
        public void Should_succeed_when_asserting_nullable_numeric_value_equals_the_same_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTimeOffset? nullableDateTimeA = Today;
            DateTimeOffset? nullableDateTimeB = Today;

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
        public void Should_succeed_when_asserting_nullable_numeric_null_value_equals_null()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTimeOffset? nullableDateTimeA = null;
            DateTimeOffset? nullableDateTimeB = null;

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
            DateTimeOffset? nullableDateTimeA = Today;
            DateTimeOffset? nullableDateTimeB = Today.AddDays(2);

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
            DateTimeOffset? nullableDateTime = null;

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


        [TestMethod]
        public void
            When_asserting_different_date_time_offsets_representing_the_same_world_time_it_should_succeded()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var specificDate = 1.May(2008).At(6, 32);
             
            var dateWithFiveHourOffset = new DateTimeOffset(specificDate.Add(-5.Hours()), -5.Hours());

            var dateWithSixHourOffset = new DateTimeOffset(specificDate.Add(-6.Hours()), -6.Hours());

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            dateWithFiveHourOffset.Should().Be(dateWithSixHourOffset);
        }

        [TestMethod]
        public void
            When_asserting_different_date_time_offsets_representing_different_world_times_it_should_not_succeded()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var specificDate = 1.May(2008).At(6, 32);

            var dateWithFiveHourOffset = new DateTimeOffset(specificDate);
            var dateWithSixHourOffset = new DateTimeOffset(specificDate, 1.Hours());

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            dateWithFiveHourOffset.Should().NotBe(dateWithSixHourOffset);
        }

        #endregion

        #region Be Close To

        [TestMethod]
        public void When_datetime_is_greater_then_but_close_to_another_value_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTimeOffset time = Today.At(12, 15, 31, 020);
            DateTimeOffset nearbyTime = Today.At(12, 15, 31);

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
            DateTimeOffset time = 13.March(2012).At(12, 15, 30, 979);
            DateTimeOffset nearbyTime = 13.March(2012).At(12, 15, 31);

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
            DateTimeOffset time = 13.March(2012).At(12, 15, 31, 021);
            DateTimeOffset nearbyTime = 13.March(2012).At(12, 15, 31);

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
            DateTimeOffset time = Today.At(12, 15, 31, 035);
            DateTimeOffset nearbyTime = Today.At(12, 15, 31);

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
            DateTimeOffset? time = null;
            DateTimeOffset nearbyTime = Today.At(12, 15, 31);

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
            DateTimeOffset time = DateTimeOffset.MinValue.Add(50.Milliseconds());
            DateTimeOffset nearbyTime = DateTimeOffset.MinValue;

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
            DateTimeOffset time = DateTimeOffset.MaxValue.Add(-50.Milliseconds());
            DateTimeOffset nearbyTime = DateTimeOffset.MaxValue;

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
        public void Should_fail_when_asserting_datetime_is_before_earlier_datetime()
        {
            Action act = () => Today.Should().BeBefore(Yesterday);
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_is_before_earlier_datetime()
        {
            DateTimeOffsetAssertions assertions = Today.Should();
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
            DateTimeOffsetAssertions assertions = Today.Should();
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
            DateTimeOffsetAssertions assertions = Today.Should();
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
            DateTimeOffsetAssertions assertions = Today.Should();
            assertions.Invoking(x => x.BeOnOrAfter(Tomorrow, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format(
                    "Expected a date and time on or after <{0}> because we want to test the failure message, but found <{1}>.",
                    Tomorrow.ToString("yyyy-MM-dd"), Today.ToString("yyyy-MM-dd")));
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_a_year_with_the_same_value()
        {
            new DateTimeOffset(31.December(2009)).Should().HaveYear(2009);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_has_a_year_with_a_different_value()
        {
            Action act = () => new DateTimeOffset(31.December(2009)).Should().HaveYear(2008);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_a_year_with_a_different_value()
        {
            DateTimeOffsetAssertions assertions = new DateTimeOffset(31.December(2009)).Should();
            assertions.Invoking(x => x.HaveYear(2008, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected year 2008 because we want to test the failure message, but found 2009.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_a_month_with_the_same_value()
        {
            new DateTimeOffset(31.December(2009)).Should().HaveMonth(12);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_has_a_month_with_a_different_value()
        {
            Action act = () => new DateTimeOffset(31.December(2009)).Should().HaveMonth(11);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_a_month_with_a_different_value()
        {
            DateTimeOffsetAssertions assertions = new DateTimeOffset(31.December(2009)).Should();
            assertions.Invoking(x => x.HaveMonth(11, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected month 11 because we want to test the failure message, but found 12.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_a_day_with_the_same_value()
        {
            new DateTimeOffset(31.December(2009)).Should().HaveDay(31);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_has_a_day_with_a_different_value()
        {
            Action act = () => new DateTimeOffset(31.December(2009)).Should().HaveDay(30);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_a_day_with_a_different_value()
        {
            DateTimeOffsetAssertions assertions = new DateTimeOffset(31.December(2009)).Should();
            assertions.Invoking(x => x.HaveDay(30, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected day 30 because we want to test the failure message, but found 31.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_an_hour_with_the_same_value()
        {
            new DateTimeOffset(31.December(2009).At(23, 59, 00)).Should().HaveHour(23);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_has_an_hour_with_different_value()
        {
            Action act = () => new DateTimeOffset(31.December(2009).At(23, 59, 00)).Should().HaveHour(22);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_an_hour_with_different_value()
        {
            DateTimeOffsetAssertions assertions = new DateTimeOffset(31.December(2009).At(23, 59, 00)).Should();
            assertions.Invoking(x => x.HaveHour(22, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected hour 22 because we want to test the failure message, but found 23.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_minutes_with_the_same_value()
        {
            new DateTimeOffset(31.December(2009).At(23, 59, 00)).Should().HaveMinute(59);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_has_minutes_with_different_value()
        {
            Action act = () => new DateTimeOffset(31.December(2009).At(23, 59, 00)).Should().HaveMinute(58);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_minutes_with_different_value()
        {
            DateTimeOffsetAssertions assertions = new DateTimeOffset(31.December(2009).At(23, 59, 00)).Should();
            assertions.Invoking(x => x.HaveMinute(58, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected minute 58 because we want to test the failure message, but found 59.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_seconds_with_the_same_value()
        {
            new DateTimeOffset(31.December(2009).At(23, 59, 00)).Should().HaveSecond(0);
        }

        [TestMethod]
        public void Should_fail_when_asserting_datetime_has_seconds_with_different_value()
        {
            Action act = () => new DateTimeOffset(31.December(2009).At(23, 59, 00)).Should().HaveSecond(1);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_seconds_with_different_value()
        {
            DateTimeOffsetAssertions assertions = new DateTimeOffset(31.December(2009).At(23, 59, 00)).Should();
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
            var target = new DateTimeOffset(2.October(2009));
            DateTimeOffset subject = target.AddDays(-1);

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
            var target = new DateTimeOffset(2.October(2009));
            DateTimeOffset subject = target.AddHours(-25);

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
            var target = new DateTimeOffset(2.October(2009));
            DateTimeOffset subject = target.AddHours(-23);

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
            var target = new DateTimeOffset(2.October(2009));
            DateTimeOffset subject = target.AddHours(-24);

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
            DateTimeOffset target = DateTimeOffset.Parse("0001/1/1 12:55");
            DateTimeOffset subject = DateTimeOffset.Parse("0001/1/1 12:36");

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
            DateTimeOffset target = DateTimeOffset.Parse("0001/1/1 12:55:00");
            DateTimeOffset subject = DateTimeOffset.Parse("0001/1/1 12:53:30");

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
            var target = new DateTimeOffset(10.April(2010).At(12, 0));
            DateTimeOffset subject = target.AddHours(-50).AddSeconds(-1);

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
            var target = new DateTimeOffset(10.April(2010));
            DateTimeOffset subject = target.AddDays(-1);

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
            var target = new DateTimeOffset(10.April(2010));
            DateTimeOffset subject = target.AddHours(-23);

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
            var date = DateTimeOffset.UtcNow; // local timezone differs from UTC

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
            var date = DateTimeOffset.UtcNow; // local timezone differs from UTC

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
            var target = new DateTimeOffset(1.January(1).At(12,0,30));
            DateTimeOffset subject = target.AddSeconds(30);

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
            var target = new DateTimeOffset(1.January(1).At(12,0,30));
            DateTimeOffset subject = target.AddSeconds(20);

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
            DateTimeOffset yesterday = Today.AddDays(-1);
            DateTimeOffset? nullableDateTime = Today;

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