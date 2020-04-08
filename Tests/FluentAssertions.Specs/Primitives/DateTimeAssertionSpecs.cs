using System;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class DateTimeAssertionSpecs
    {
        #region (Not) Have Value

        [Fact]
        public void Should_succeed_when_asserting_nullable_datetime_value_with_a_value_to_have_a_value()
        {
            // Arrange
            DateTime? nullableDateTime = new DateTime(2016, 06, 04);

            // Act
            Action action = () => nullableDateTime.Should().HaveValue();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetime_value_without_a_value_to_have_a_value()
        {
            // Arrange
            DateTime? nullableDateTime = null;

            // Act
            Action action = () => nullableDateTime.Should().HaveValue();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_datetime_value_without_a_value_to_not_have_a_value()
        {
            // Arrange
            DateTime? nullableDateTime = null;

            // Act
            Action action = () =>
                nullableDateTime.Should().NotHaveValue();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetime_value_with_a_value_to_not_have_a_value()
        {
            // Arrange
            DateTime? nullableDateTime = new DateTime(2016, 06, 04);

            // Act
            Action action = () =>
                nullableDateTime.Should().NotHaveValue();

            // Assert
            action.Should().Throw<XunitException>();
        }

        #endregion

        #region (Not) Be Null

        [Fact]
        public void Should_succeed_when_asserting_nullable_datetime_value_with_a_value_to_not_be_null()
        {
            // Arrange
            DateTime? nullableDateTime = new DateTime(2016, 06, 04);

            // Act
            Action action = () => nullableDateTime.Should().NotBeNull();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetime_value_without_a_value_to_not_be_null()
        {
            // Arrange
            DateTime? nullableDateTime = null;

            // Act
            Action action = () => nullableDateTime.Should().NotBeNull();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_datetime_value_without_a_value_to_be_null()
        {
            // Arrange
            DateTime? nullableDateTime = null;

            // Act
            Action action = () =>
                nullableDateTime.Should().BeNull();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetime_value_with_a_value_to_be_null()
        {
            // Arrange
            DateTime? nullableDateTime = new DateTime(2016, 06, 04);

            // Act
            Action action = () =>
                nullableDateTime.Should().BeNull();

            // Assert
            action.Should().Throw<XunitException>();
        }

        #endregion

        #region (Not) Be

        [Fact]
        public void Should_succeed_when_asserting_datetime_value_is_equal_to_the_same_value()
        {
            // Arrange
            DateTime dateTime = new DateTime(2016, 06, 04);
            DateTime sameDateTime = new DateTime(2016, 06, 04);

            // Act
            Action act = () => dateTime.Should().Be(sameDateTime);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_both_values_are_at_their_minimum_then_it_should_succeed()
        {
            // Arrange
            DateTime dateTime = DateTime.MinValue;
            DateTime sameDateTime = DateTime.MinValue;

            // Act
            Action act = () => dateTime.Should().Be(sameDateTime);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_both_values_are_at_their_maximum_then_it_should_succeed()
        {
            // Arrange
            DateTime dateTime = DateTime.MaxValue;
            DateTime sameDateTime = DateTime.MaxValue;

            // Act
            Action act = () => dateTime.Should().Be(sameDateTime);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_datetime_value_is_equal_to_the_different_value()
        {
            // Arrange
            var dateTime = new DateTime(2012, 03, 10);
            var otherDateTime = new DateTime(2012, 03, 11);

            // Act
            Action act = () => dateTime.Should().Be(otherDateTime, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected dateTime to be <2012-03-11>*failure message, but found <2012-03-10>.");
        }

        [Fact]
        public void Should_succeed_when_asserting_datetime_value_is_not_equal_to_a_different_value()
        {
            // Arrange
            DateTime dateTime = new DateTime(2016, 06, 04);
            DateTime otherDateTime = new DateTime(2016, 06, 05);

            // Act
            Action act = () => dateTime.Should().NotBe(otherDateTime);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_datetime_value_is_not_equal_to_the_same_value()
        {
            // Arrange
            var dateTime = DateTime.SpecifyKind(10.March(2012).At(10, 00), DateTimeKind.Local);
            var sameDateTime = DateTime.SpecifyKind(10.March(2012).At(10, 00), DateTimeKind.Utc);

            // Act
            Action act =
                () => dateTime.Should().NotBe(sameDateTime, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected dateTime not to be <2012-03-10 10:00:00> because we want to test the failure message, but it is.");
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_equals_the_same_value()
        {
            // Arrange
            DateTime? nullableDateTimeA = new DateTime(2016, 06, 04);
            DateTime? nullableDateTimeB = new DateTime(2016, 06, 04);

            // Act
            Action action = () =>
                nullableDateTimeA.Should().Be(nullableDateTimeB);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_date_time_is_equal_to_a_normal_date_time_but_the_kinds_differ_it_should_still_succeed()
        {
            // Arrange
            DateTime? nullableDateTime = new DateTime(2014, 4, 20, 9, 11, 0, DateTimeKind.Unspecified);
            DateTime normalDateTime = new DateTime(2014, 4, 20, 9, 11, 0, DateTimeKind.Utc);

            // Act
            Action action = () =>
                nullableDateTime.Should().Be(normalDateTime);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_two_date_times_are_equal_but_the_kinds_differ_it_should_still_succeed()
        {
            // Arrange
            DateTime dateTimeA = new DateTime(2014, 4, 20, 9, 11, 0, DateTimeKind.Unspecified);
            DateTime dateTimeB = new DateTime(2014, 4, 20, 9, 11, 0, DateTimeKind.Utc);

            // Act
            Action action = () =>
                dateTimeA.Should().Be(dateTimeB);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_null_value_equals_null()
        {
            // Arrange
            DateTime? nullableDateTimeA = null;
            DateTime? nullableDateTimeB = null;

            // Act
            Action action = () =>
                nullableDateTimeA.Should().Be(nullableDateTimeB);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            // Arrange
            DateTime? nullableDateTimeA = new DateTime(2016, 06, 04);
            DateTime? nullableDateTimeB = new DateTime(2016, 06, 06);

            // Act
            Action action = () =>
                nullableDateTimeA.Should().Be(nullableDateTimeB);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_null_value_is_equal_to_another_value()
        {
            // Arrange
            DateTime? nullableDateTime = null;

            // Act
            Action action = () =>
                nullableDateTime.Should().Be(new DateTime(2016, 06, 04), "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected <2016-06-04> because we want to test the failure message, but found <null>.");
        }

        #endregion

        #region (Not) Be Close To

        [Fact]
        public void When_a_datetime_is_close_to_a_later_datetime_by_one_tick_it_should_succeed()
        {
            // Arrange
            var dateTime = DateTime.UtcNow;
            var actual = new DateTime(dateTime.Ticks - 1);

            // Act
            Action act = () => actual.Should().BeCloseTo(dateTime, TimeSpan.FromTicks(1));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_datetime_is_close_to_an_earlier_datetime_by_one_tick_it_should_succeed()
        {
            // Arrange
            var dateTime = DateTime.UtcNow;
            var actual = new DateTime(dateTime.Ticks + 1);

            // Act
            Action act = () => actual.Should().BeCloseTo(dateTime, TimeSpan.FromTicks(1));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_datetime_is_close_to_a_MinValue_by_one_tick_it_should_succeed()
        {
            // Arrange
            var dateTime = DateTime.MinValue;
            var actual = new DateTime(dateTime.Ticks + 1);

            // Act
            Action act = () => actual.Should().BeCloseTo(dateTime, TimeSpan.FromTicks(1));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_datetime_is_close_to_a_MaxValue_by_one_tick_it_should_succeed()
        {
            // Arrange
            var dateTime = DateTime.MaxValue;
            var actual = new DateTime(dateTime.Ticks - 1);

            // Act
            Action act = () => actual.Should().BeCloseTo(dateTime, TimeSpan.FromTicks(1));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_datetime_is_close_to_a_later_datetime_by_one_tick_it_should_fail()
        {
            // Arrange
            var dateTime = DateTime.UtcNow;
            var actual = new DateTime(dateTime.Ticks - 1);

            // Act
            Action act = () => actual.Should().NotBeCloseTo(dateTime, TimeSpan.FromTicks(1));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_datetime_is_close_to_an_earlier_datetime_by_one_tick_it_should_fail()
        {
            // Arrange
            var dateTime = DateTime.UtcNow;
            var actual = new DateTime(dateTime.Ticks + 1);

            // Act
            Action act = () => actual.Should().NotBeCloseTo(dateTime, TimeSpan.FromTicks(1));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_datetime_is_close_to_a_MinValue_by_one_tick_it_should_fail()
        {
            // Arrange
            var dateTime = DateTime.MinValue;
            var actual = new DateTime(dateTime.Ticks + 1);

            // Act
            Action act = () => actual.Should().NotBeCloseTo(dateTime, TimeSpan.FromTicks(1));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_datetime_is_close_to_a_MaxValue_by_one_tick_it_should_fail()
        {
            // Arrange
            var dateTime = DateTime.MaxValue;
            var actual = new DateTime(dateTime.Ticks - 1);

            // Act
            Action act = () => actual.Should().NotBeCloseTo(dateTime, TimeSpan.FromTicks(1));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_close_to_a_later_datetime_it_should_succeed()
        {
            // Arrange
            DateTime time = DateTime.SpecifyKind(new DateTime(2016, 06, 04).At(12, 15, 30, 980), DateTimeKind.Unspecified);
            DateTime nearbyTime = DateTime.SpecifyKind(new DateTime(2016, 06, 04).At(12, 15, 31), DateTimeKind.Utc);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 20.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_close_to_a_later_datetime_it_should_throw()
        {
            // Arrange
            DateTime time = DateTime.SpecifyKind(new DateTime(2016, 06, 04).At(12, 15, 30, 980), DateTimeKind.Unspecified);
            DateTime nearbyTime = DateTime.SpecifyKind(new DateTime(2016, 06, 04).At(12, 15, 31), DateTimeKind.Utc);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 20.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect time to be within 0.020s from <2016-06-04 12:15:31>, but it was <2016-06-04 12:15:30.980>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_close_to_an_earlier_datetime_it_should_succeed()
        {
            // Arrange
            DateTime time = new DateTime(2016, 06, 04).At(12, 15, 31, 020);
            DateTime nearbyTime = new DateTime(2016, 06, 04).At(12, 15, 31);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 20.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_close_to_an_earlier_datetime_it_should_throw()
        {
            // Arrange
            DateTime time = new DateTime(2016, 06, 04).At(12, 15, 31, 020);
            DateTime nearbyTime = new DateTime(2016, 06, 04).At(12, 15, 31);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 20.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect time to be within 0.020s from <2016-06-04 12:15:31>, but it was <2016-06-04 12:15:31.020>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_close_to_an_earlier_datetime_by_a_20ms_timespan_it_should_throw()
        {
            // Arrange
            DateTime time = new DateTime(2016, 06, 04).At(12, 15, 31, 020);
            DateTime nearbyTime = new DateTime(2016, 06, 04).At(12, 15, 31);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(20));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect time to be within 0.020s from <2016-06-04 12:15:31>, but it was <2016-06-04 12:15:31.020>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_close_to_another_value_that_is_later_by_more_than_20ms_it_should_throw()
        {
            // Arrange
            DateTime time = 13.March(2012).At(12, 15, 30, 979);
            DateTime nearbyTime = 13.March(2012).At(12, 15, 31);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 20.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected time to be within 0.020s from <2012-03-13 12:15:31>, but found <2012-03-13 12:15:30.979>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_close_to_another_value_that_is_later_by_more_than_a_20ms_timespan_it_should_throw()
        {
            // Arrange
            DateTime time = 13.March(2012).At(12, 15, 30, 979);
            DateTime nearbyTime = 13.March(2012).At(12, 15, 31);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(20));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected time to be within 0.020s from <2012-03-13 12:15:31>, but found <2012-03-13 12:15:30.979>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_close_to_another_value_that_is_later_by_more_than_20ms_it_should_succeed()
        {
            // Arrange
            DateTime time = 13.March(2012).At(12, 15, 30, 979);
            DateTime nearbyTime = 13.March(2012).At(12, 15, 31);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 20.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_close_to_another_value_that_is_earlier_by_more_than_20ms_it_should_throw()
        {
            // Arrange
            DateTime time = 13.March(2012).At(12, 15, 31, 021);
            DateTime nearbyTime = 13.March(2012).At(12, 15, 31);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 20.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected time to be within 0.020s from <2012-03-13 12:15:31>, but found <2012-03-13 12:15:31.021>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_close_to_another_value_that_is_earlier_by_more_than_20ms_it_should_succeed()
        {
            // Arrange
            DateTime time = 13.March(2012).At(12, 15, 31, 021);
            DateTime nearbyTime = 13.March(2012).At(12, 15, 31);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 20.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_close_to_an_earlier_datetime_by_35ms_it_should_succeed()
        {
            // Arrange
            DateTime time = new DateTime(2016, 06, 04).At(12, 15, 31, 035);
            DateTime nearbyTime = new DateTime(2016, 06, 04).At(12, 15, 31);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 35.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_close_to_an_earlier_datetime_by_35ms_it_should_throw()
        {
            // Arrange
            DateTime time = new DateTime(2016, 06, 04).At(12, 15, 31, 035);
            DateTime nearbyTime = new DateTime(2016, 06, 04).At(12, 15, 31);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 35.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect time to be within 0.035s from <2016-06-04 12:15:31>, but it was <2016-06-04 12:15:31.035>.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_is_close_to_another_it_should_throw()
        {
            // Arrange
            DateTime? time = null;
            DateTime nearbyTime = new DateTime(2016, 06, 04).At(12, 15, 31);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 35.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*, but found <null>.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_is_not_close_to_another_it_should_throw()
        {
            // Arrange
            DateTime? time = null;
            DateTime nearbyTime = new DateTime(2016, 06, 04).At(12, 15, 31);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 35.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect*, but it was <null>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_close_to_the_minimum_datetime_it_should_succeed()
        {
            // Arrange
            DateTime time = DateTime.MinValue + 50.Milliseconds();
            DateTime nearbyTime = DateTime.MinValue;

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 100.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_close_to_the_minimum_datetime_it_should_throw()
        {
            // Arrange
            DateTime time = DateTime.MinValue + 50.Milliseconds();
            DateTime nearbyTime = DateTime.MinValue;

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect time to be within 0.100s from <0001-01-01 00:00:00.000>, but it was <00:00:00.050>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_close_to_the_maximum_datetime_it_should_succeed()
        {
            // Arrange
            DateTime time = DateTime.MaxValue - 50.Milliseconds();
            DateTime nearbyTime = DateTime.MaxValue;

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 100.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_close_to_the_maximum_datetime_it_should_throw()
        {
            // Arrange
            DateTime time = DateTime.MaxValue - 50.Milliseconds();
            DateTime nearbyTime = DateTime.MaxValue;

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect time to be within 0.100s from <9999-12-31 23:59:59.9999999>, but it was <9999-12-31 23:59:59.9499999>.");
        }

        #endregion

        #region (Not) Be Before
        [Fact]
        public void When_asserting_a_point_of_time_is_before_a_later_point_it_should_succeed()
        {
            // Arrange
            DateTime earlierDate = DateTime.SpecifyKind(new DateTime(2016, 06, 04), DateTimeKind.Unspecified);
            DateTime laterDate = DateTime.SpecifyKind(new DateTime(2016, 06, 04, 0, 5, 0), DateTimeKind.Utc);

            // Act
            Action act = () => earlierDate.Should().BeBefore(laterDate);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_point_of_time_is_not_before_another_it_should_throw()
        {
            // Arrange
            DateTime earlierDate = DateTime.SpecifyKind(new DateTime(2016, 06, 04), DateTimeKind.Unspecified);
            DateTime laterDate = DateTime.SpecifyKind(new DateTime(2016, 06, 04, 0, 5, 0), DateTimeKind.Utc);

            // Act
            Action act = () => earlierDate.Should().NotBeBefore(laterDate);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected earlierDate to be on or after <2016-06-04 00:05:00>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_is_before_earlier_expected_datetime_it_should_throw()
        {
            // Arrange
            DateTime expected = new DateTime(2016, 06, 03);
            DateTime subject = new DateTime(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeBefore(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_is_not_before_earlier_expected_datetime_it_should_succeed()
        {
            // Arrange
            DateTime expected = new DateTime(2016, 06, 03);
            DateTime subject = new DateTime(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeBefore(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_before_the_same_datetime_it_should_throw()
        {
            // Arrange
            DateTime expected = new DateTime(2016, 06, 04);
            DateTime subject = new DateTime(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeBefore(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_before_the_same_datetime_it_should_succeed()
        {
            // Arrange
            DateTime expected = new DateTime(2016, 06, 04);
            DateTime subject = new DateTime(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeBefore(expected);

            // Assert
            act.Should().NotThrow();
        }
        #endregion

        #region (Not) Be On Or Before
        [Fact]
        public void When_asserting_subject_datetime_is_on_or_before_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 05);

            // Act
            Action act = () => subject.Should().BeOnOrBefore(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_on_or_before_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 05);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-05>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_on_or_before_the_same_date_as_the_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeOnOrBefore(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_on_or_before_the_same_date_as_the_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_on_or_before_earlier_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 03);

            // Act
            Action act = () => subject.Should().BeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_on_or_before_earlier_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().NotThrow();
        }
        #endregion

        #region (Not) Be After
        [Fact]
        public void When_asserting_subject_datetime_is_after_earlier_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 03);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_after_earlier_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_after_later_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 05);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-05>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_after_later_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 05);

            // Act
            Action act = () => subject.Should().NotBeAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_after_the_same_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_after_the_same_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }
        #endregion

        #region (Not) Be On Or After
        [Fact]
        public void When_asserting_subject_datetime_is_on_or_after_earlier_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 03);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_on_or_after_earlier_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_on_or_after_the_same_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_on_or_after_the_same_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_on_or_after_later_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 05);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or after <2016-06-05>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_on_or_after_later_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2016, 06, 04);
            DateTime expectation = new DateTime(2016, 06, 05);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }
        #endregion

        #region (Not) Have Year
        [Fact]
        public void When_asserting_subject_datetime_should_have_year_with_the_same_value_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31);
            int expectation = 2009;

            // Act
            Action act = () => subject.Should().HaveYear(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_year_with_the_same_value_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31);
            int expectation = 2009;

            // Act
            Action act = () => subject.Should().NotHaveYear(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the year part of subject to be 2009, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_have_year_with_a_different_value_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31);
            int expectation = 2008;

            // Act
            Action act = () => subject.Should().HaveYear(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the year part of subject to be 2008, but found 2009.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_year_with_a_different_value_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31);
            int expectation = 2008;

            // Act
            Action act = () => subject.Should().NotHaveYear(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_have_year_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 2008;

            // Act
            Action act = () => subject.Should().HaveYear(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the year part of subject to be 2008, but found <null>.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_not_have_year_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 2008;

            // Act
            Action act = () => subject.Should().NotHaveYear(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the year part of subject to be 2008, but found a <null> DateTime.");
        }
        #endregion

        #region (Not) Have Month
        [Fact]
        public void When_asserting_subject_datetime_should_have_month_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31);
            int expectation = 12;

            // Act
            Action act = () => subject.Should().HaveMonth(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_month_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31);
            int expectation = 12;

            // Act
            Action act = () => subject.Should().NotHaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the month part of subject to be 12, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_have_a_month_with_a_different_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31);
            int expectation = 11;

            // Act
            Action act = () => subject.Should().HaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the month part of subject to be 11, but found 12.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_a_month_with_a_different_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31);
            int expectation = 11;

            // Act
            Action act = () => subject.Should().NotHaveMonth(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_have_month_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 12;

            // Act
            Action act = () => subject.Should().HaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the month part of subject to be 12, but found a <null> DateTime.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_not_have_month_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 12;

            // Act
            Action act = () => subject.Should().NotHaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the month part of subject to be 12, but found a <null> DateTime.");
        }
        #endregion

        #region (Not) Have Day
        [Fact]
        public void When_asserting_subject_datetime_should_have_day_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31);
            int expectation = 31;

            // Act
            Action act = () => subject.Should().HaveDay(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_day_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31);
            int expectation = 31;

            // Act
            Action act = () => subject.Should().NotHaveDay(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the day part of subject to be 31, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_have_day_with_a_different_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31);
            int expectation = 30;

            // Act
            Action act = () => subject.Should().HaveDay(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the day part of subject to be 30, but found 31.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_day_with_a_different_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31);
            int expectation = 30;

            // Act
            Action act = () => subject.Should().NotHaveDay(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_have_day_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveDay(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the day part of subject to be 22, but found a <null> DateTime.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_not_have_day_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveDay(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the day part of subject to be 22, but found a <null> DateTime.");
        }
        #endregion

        #region (Not) Have Hour
        [Fact]
        public void When_asserting_subject_datetime_should_have_hour_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00);
            int expectation = 23;

            // Act
            Action act = () => subject.Should().HaveHour(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_hour_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00);
            int expectation = 23;

            // Act
            Action act = () => subject.Should().NotHaveHour(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the hour part of subject to be 23, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_have_hour_with_different_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00);
            int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveHour(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the hour part of subject to be 22, but found 23.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_hour_with_different_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00);
            int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveHour(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_have_hour_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveHour(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the hour part of subject to be 22, but found a <null> DateTime.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_not_have_hour_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveHour(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the hour part of subject to be 22, but found a <null> DateTime.");
        }
        #endregion

        #region (Not) Have Minute
        [Fact]
        public void When_asserting_subject_datetime_should_have_minutes_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00);
            int expectation = 59;

            // Act
            Action act = () => subject.Should().HaveMinute(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_minutes_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00);
            int expectation = 59;

            // Act
            Action act = () => subject.Should().NotHaveMinute(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the minute part of subject to be 59, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_have_minutes_with_different_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00);
            int expectation = 58;

            // Act
            Action act = () => subject.Should().HaveMinute(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the minute part of subject to be 58, but found 59.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_minutes_with_different_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00);
            int expectation = 58;

            // Act
            Action act = () => subject.Should().NotHaveMinute(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_have_minute_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveMinute(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the minute part of subject to be 22, but found a <null> DateTime.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_not_have_minute_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveMinute(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the minute part of subject to be 22, but found a <null> DateTime.");
        }
        #endregion

        #region (Not) Have Second
        [Fact]
        public void When_asserting_subject_datetime_should_have_seconds_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00);
            int expectation = 0;

            // Act
            Action act = () => subject.Should().HaveSecond(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_seconds_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00);
            int expectation = 0;

            // Act
            Action act = () => subject.Should().NotHaveSecond(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the seconds part of subject to be 0, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_have_seconds_with_different_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00);
            int expectation = 1;

            // Act
            Action act = () => subject.Should().HaveSecond(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the seconds part of subject to be 1, but found 0.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_seconds_with_different_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00);
            int expectation = 1;

            // Act
            Action act = () => subject.Should().NotHaveSecond(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_have_second_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveSecond(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the seconds part of subject to be 22, but found a <null> DateTime.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_not_have_second_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveSecond(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the seconds part of subject to be 22, but found a <null> DateTime.");
        }
        #endregion

        #region BeIn Utc/Local
        [Fact]
        public void When_asserting_subject_datetime_represents_its_own_kind_it_should_succeed()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00, DateTimeKind.Local);

            // Act
            Action act = () => subject.Should().BeIn(DateTimeKind.Local);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_represents_a_different_kind_it_should_throw()
        {
            // Arrange
            DateTime subject = new DateTime(2009, 12, 31, 23, 59, 00, DateTimeKind.Local);

            // Act
            Action act = () => subject.Should().BeIn(DateTimeKind.Utc);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be in Utc, but found Local.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_represents_a_specific_kind_it_should_throw()
        {
            // Arrange
            DateTime? subject = null;

            // Act
            Action act = () => subject.Should().BeIn(DateTimeKind.Utc);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be in Utc, but found a <null> DateTime.");
        }
        #endregion

        #region (Not) Be Same Date As
        [Fact]
        public void When_asserting_subject_datetime_should_be_same_date_as_another_with_the_same_date_it_should_succeed()
        {
            // Arrange
            var subject = new DateTime(2009, 12, 31, 4, 5, 6);

            // Act
            Action act = () => subject.Should().BeSameDateAs(new DateTime(2009, 12, 31));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_be_same_date_as_another_with_the_same_date_it_should_throw()
        {
            // Arrange
            var subject = new DateTime(2009, 12, 31, 4, 5, 6);

            // Act
            Action act = () => subject.Should().NotBeSameDateAs(new DateTime(2009, 12, 31));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the date part of subject to be <2009-12-31>, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_be_same_as_another_with_same_date_but_different_time_it_should_succeed()
        {
            // Arrange
            var subject = new DateTime(2009, 12, 31, 4, 5, 6);

            // Act
            Action act = () => subject.Should().BeSameDateAs(new DateTime(2009, 12, 31, 11, 15, 11));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_be_same_as_another_with_same_date_but_different_time_it_should_throw()
        {
            // Arrange
            var subject = new DateTime(2009, 12, 31, 4, 5, 6);

            // Act
            Action act = () => subject.Should().NotBeSameDateAs(new DateTime(2009, 12, 31, 11, 15, 11));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the date part of subject to be <2009-12-31>, but it was.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_to_be_same_date_as_another_datetime_it_should_throw()
        {
            // Arrange
            DateTime? subject = null;

            // Act
            Action act = () => subject.Should().BeSameDateAs(new DateTime(2009, 12, 31));

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected the date part of subject to be <2009-12-31>, but found a <null> DateTime.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_to_not_be_same_date_as_another_datetime_it_should_throw()
        {
            // Arrange
            DateTime? subject = null;

            // Act
            Action act = () => subject.Should().NotBeSameDateAs(new DateTime(2009, 12, 31));

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect the date part of subject to be <2009-12-31>, but found a <null> DateTime.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_have_same_date_as_another_but_it_doesnt_it_should_throw()
        {
            // Arrange
            var subject = new DateTime(2009, 12, 31);

            // Act
            Action act = () => subject.Should().BeSameDateAs(new DateTime(2009, 12, 30));

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected the date part of subject to be <2009-12-30>, but found <2009-12-31>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_same_date_as_another_but_it_doesnt_it_should_succeed()
        {
            // Arrange
            var subject = new DateTime(2009, 12, 31);

            // Act
            Action act = () => subject.Should().NotBeSameDateAs(new DateTime(2009, 12, 30));

            // Assert
            act.Should().NotThrow();
        }
        #endregion

        #region Timespan Comparison

        [Fact]
        public void When_date_is_not_more_than_the_required_one_day_before_another_it_should_throw()
        {
            // Arrange
            var target = new DateTime(2009, 10, 2);
            DateTime subject = target - 1.Days();

            // Act
            Action act = () => subject.Should().BeMoreThan(TimeSpan.FromDays(1)).Before(target, "we like {0}", "that");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected date and/or time <2009-10-01> to be more than 1d before <2009-10-02> because we like that, but it differs 1d.");
        }

        [Fact]
        public void When_date_is_more_than_the_required_one_day_before_another_it_should_not_throw()
        {
            // Arrange
            var target = new DateTime(2009, 10, 2);
            DateTime subject = target - 25.Hours();

            // Act / Assert
            subject.Should().BeMoreThan(TimeSpan.FromDays(1)).Before(target);
        }

        [Fact]
        public void When_date_is_not_at_least_one_day_before_another_it_should_throw()
        {
            // Arrange
            var target = new DateTime(2009, 10, 2);
            DateTime subject = target - 23.Hours();

            // Act
            Action act = () => subject.Should().BeAtLeast(TimeSpan.FromDays(1)).Before(target, "we like {0}", "that");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected date and/or time <2009-10-01 01:00:00> to be at least 1d before <2009-10-02> because we like that, but it differs 23h.");
        }

        [Fact]
        public void When_date_is_at_least_one_day_before_another_it_should_not_throw()
        {
            // Arrange
            var target = new DateTime(2009, 10, 2);
            DateTime subject = target - 24.Hours();

            // Act / Assert
            subject.Should().BeAtLeast(TimeSpan.FromDays(1)).Before(target);
        }

        [Fact]
        public void When_time_is_not_at_exactly_20_minutes_before_another_time_it_should_throw()
        {
            // Arrange
            DateTime target = 1.January(0001).At(12, 55);
            DateTime subject = 1.January(0001).At(12, 36);

            // Act
            Action act =
                () => subject.Should().BeExactly(TimeSpan.FromMinutes(20)).Before(target, "{0} minutes is enough", 20);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected date and/or time <12:36:00> to be exactly 20m before <12:55:00> because 20 minutes is enough, but it differs 19m.");
        }

        [Fact]
        public void When_time_is_exactly_90_seconds_before_another_time_it_should_not_throw()
        {
            // Arrange
            DateTime target = 1.January(0001).At(12, 55);
            DateTime subject = 1.January(0001).At(12, 53, 30);

            // Act / Assert
            subject.Should().BeExactly(TimeSpan.FromSeconds(90)).Before(target);
        }

        [Fact]
        public void When_date_is_not_within_50_hours_before_another_date_it_should_throw()
        {
            // Arrange
            var target = new DateTime(2010, 4, 10, 12, 0, 0);
            DateTime subject = target - 50.Hours() - 1.Seconds();

            // Act
            Action act =
                () => subject.Should().BeWithin(TimeSpan.FromHours(50)).Before(target, "{0} hours is enough", 50);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected date and/or time <2010-04-08 09:59:59> to be within 2d and 2h before <2010-04-10 12:00:00> because 50 hours is enough, but it differs 2d, 2h and 1s.");
        }

        [Fact]
        public void When_date_is_exactly_within_1d_before_another_date_it_should_not_throw()
        {
            // Arrange
            var target = new DateTime(2010, 4, 10);
            DateTime subject = target - 1.Days();

            // Act / Assert
            subject.Should().BeWithin(TimeSpan.FromHours(24)).Before(target);
        }

        [Fact]
        public void When_date_is_within_1d_before_another_date_it_should_not_throw()
        {
            // Arrange
            var target = new DateTime(2010, 4, 10);
            DateTime subject = target - 23.Hours();

            // Act / Assert
            subject.Should().BeWithin(TimeSpan.FromHours(24)).Before(target);
        }

        [Fact]
        public void When_a_utc_date_is_within_0s_before_itself_it_should_not_throw()
        {
            // Arrange
            var date = DateTime.UtcNow; // local timezone differs from UTC

            // Act / Assert
            date.Should().BeWithin(TimeSpan.Zero).Before(date);
        }

        [Fact]
        public void When_a_utc_date_is_within_0s_after_itself_it_should_not_throw()
        {
            // Arrange
            var date = DateTime.UtcNow; // local timezone differs from UTC

            // Act / Assert
            date.Should().BeWithin(TimeSpan.Zero).After(date);
        }

        [Fact]
        public void When_time_is_not_less_than_30s_after_another_time_it_should_throw()
        {
            // Arrange
            var target = new DateTime(1, 1, 1, 12, 0, 30);
            DateTime subject = target + 30.Seconds();

            // Act
            Action act =
                () => subject.Should().BeLessThan(TimeSpan.FromSeconds(30)).After(target, "{0}s is the max", 30);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected date and/or time <12:01:00> to be less than 30s after <12:00:30> because 30s is the max, but it differs 30s.");
        }

        [Fact]
        public void When_time_is_less_than_30s_after_another_time_it_should_not_throw()
        {
            // Arrange
            var target = new DateTime(1, 1, 1, 12, 0, 30);
            DateTime subject = target + 20.Seconds();

            // Act / Assert
            subject.Should().BeLessThan(TimeSpan.FromSeconds(30)).After(target);
        }

        #endregion

        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            DateTime earlierDateTime = new DateTime(2016, 06, 03);
            DateTime? nullableDateTime = new DateTime(2016, 06, 04);

            // Act
            Action action = () =>
                nullableDateTime.Should()
                    .HaveValue()
                    .And
                    .BeAfter(earlierDateTime);

            // Assert
            action.Should().NotThrow();
        }

        #region Be One Of

        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            DateTime value = new DateTime(2016, 12, 30, 23, 58, 57);

            // Act
            Action action = () => value.Should().BeOneOf(value + 1.Days(), value + 1.Milliseconds());

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {<2016-12-31 23:58:57>, <2016-12-30 23:58:57.001>}, but found <2016-12-30 23:58:57>.");
        }

        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw_with_descriptive_message()
        {
            // Arrange
            DateTime value = new DateTime(2016, 12, 30, 23, 58, 57);

            // Act
            Action action = () => value.Should().BeOneOf(new[] { value + 1.Days(), value + 1.Milliseconds() }, "because it's true");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {<2016-12-31 23:58:57>, <2016-12-30 23:58:57.001>} because it's true, but found <2016-12-30 23:58:57>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed()
        {
            // Arrange
            DateTime value = new DateTime(2016, 12, 30, 23, 58, 57);

            // Act
            Action action = () => value.Should().BeOneOf(new DateTime(2216, 1, 30, 0, 5, 7), new DateTime(2016, 12, 30, 23, 58, 57), new DateTime(2012, 3, 3));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_a_null_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            DateTime? value = null;

            // Act
            Action action = () => value.Should().BeOneOf(new DateTime(2216, 1, 30, 0, 5, 7), new DateTime(1116, 4, 10, 2, 45, 7));

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {<2216-01-30 00:05:07>, <1116-04-10 02:45:07>}, but found <null>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed_when_datetime_is_null()
        {
            // Arrange
            DateTime? value = null;

            // Act
            Action action = () => value.Should().BeOneOf(new DateTime(2216, 1, 30, 0, 5, 7), null);

            // Assert
            action.Should().NotThrow();
        }

        #endregion
    }
}
