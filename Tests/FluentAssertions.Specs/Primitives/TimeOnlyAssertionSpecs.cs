using System;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

#if NET6_0_OR_GREATER
namespace FluentAssertions.Specs.Primitives;

public class TimeOnlyAssertionSpecs
{
    [Fact]
    public void Should_succeed_when_asserting_nullable_timeonly_value_with_value_to_have_a_value()
    {
        // Arrange
        TimeOnly? timeOnly = new(15, 06, 04);

        // Act/Assert
        timeOnly.Should().HaveValue();
    }

    [Fact]
    public void Should_succeed_when_asserting_nullable_timeonly_value_with_value_to_not_be_null()
    {
        // Arrange
        TimeOnly? timeOnly = new(15, 06, 04);

        // Act/Assert
        timeOnly.Should().NotBeNull();
    }

    [Fact]
    public void Should_succeed_when_asserting_nullable_timeonly_value_with_null_to_be_null()
    {
        // Arrange
        TimeOnly? timeOnly = null;

        // Act/Assert
        timeOnly.Should().BeNull();
    }

    public class Be
    {
        [Fact]
        public void Should_succeed_when_asserting_timeonly_value_is_equal_to_the_same_value()
        {
            // Arrange
            TimeOnly timeOnly = new(15, 06, 04, 146);
            TimeOnly sameTimeOnly = new(15, 06, 04, 146);

            // Act/Assert
            timeOnly.Should().Be(sameTimeOnly);
        }

        [Fact]
        public void When_timeonly_value_is_equal_to_the_same_nullable_value_be_should_succeed()
        {
            // Arrange
            TimeOnly timeOnly = new(15, 06, 04, 146);
            TimeOnly? sameTimeOnly = new(15, 06, 04, 146);

            // Act/Assert
            timeOnly.Should().Be(sameTimeOnly);
        }

        [Fact]
        public void When_both_values_are_at_their_minimum_then_it_should_succeed()
        {
            // Arrange
            TimeOnly timeOnly = TimeOnly.MinValue;
            TimeOnly sameTimeOnly = TimeOnly.MinValue;

            // Act/Assert
            timeOnly.Should().Be(sameTimeOnly);
        }

        [Fact]
        public void When_both_values_are_at_their_maximum_then_it_should_succeed()
        {
            // Arrange
            TimeOnly timeOnly = TimeOnly.MaxValue;
            TimeOnly sameTimeOnly = TimeOnly.MaxValue;

            // Act/Assert
            timeOnly.Should().Be(sameTimeOnly);
        }

        [Fact]
        public void Should_fail_when_asserting_timeonly_value_is_equal_to_the_different_value()
        {
            // Arrange
            var timeOnly = new TimeOnly(15, 03, 10);
            var otherTimeOnly = new TimeOnly(15, 03, 11);

            // Act
            Action act = () => timeOnly.Should().Be(otherTimeOnly, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected timeOnly to be <15:03:11.000>*failure message, but found <15:03:10.000>.");
        }

        [Fact]
        public void Should_fail_when_asserting_timeonly_value_is_equal_to_the_different_value_by_milliseconds()
        {
            // Arrange
            var timeOnly = new TimeOnly(15, 03, 10, 556);
            var otherTimeOnly = new TimeOnly(15, 03, 10, 175);

            // Act
            Action act = () => timeOnly.Should().Be(otherTimeOnly, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected timeOnly to be <15:03:10.175>*failure message, but found <15:03:10.556>.");
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_equals_the_same_value()
        {
            // Arrange
            TimeOnly? nullableTimeOnlyA = new TimeOnly(15, 06, 04, 123);
            TimeOnly? nullableTimeOnlyB = new TimeOnly(15, 06, 04, 123);

            // Act/Assert
            nullableTimeOnlyA.Should().Be(nullableTimeOnlyB);
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_null_value_equals_null()
        {
            // Arrange
            TimeOnly? nullableTimeOnlyA = null;
            TimeOnly? nullableTimeOnlyB = null;

            // Act/Assert
            nullableTimeOnlyA.Should().Be(nullableTimeOnlyB);
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            // Arrange
            TimeOnly? nullableTimeOnlyA = new TimeOnly(15, 06, 04);
            TimeOnly? nullableTimeOnlyB = new TimeOnly(15, 06, 06);

            // Act
            Action action = () =>
                nullableTimeOnlyA.Should().Be(nullableTimeOnlyB);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_timeonly_null_value_is_equal_to_another_value()
        {
            // Arrange
            TimeOnly? nullableTimeOnly = null;

            // Act
            Action action = () =>
                nullableTimeOnly.Should().Be(new TimeOnly(15, 06, 04), "because we want to test the failure {0}",
                    "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected nullableTimeOnly to be <15:06:04.000> because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void Should_succeed_when_asserting_timeonly_value_is_not_equal_to_a_different_value()
        {
            // Arrange
            TimeOnly timeOnly = new(15, 06, 04);
            TimeOnly otherTimeOnly = new(15, 06, 05);

            // Act/Assert
            timeOnly.Should().NotBe(otherTimeOnly);
        }
    }

    public class BeCloseTo
    {
        [Fact]
        public void When_time_is_close_to_a_negative_precision_it_should_throw()
        {
            // Arrange
            var time = TimeOnly.FromDateTime(DateTime.UtcNow);
            var actual = new TimeOnly(time.Ticks - 1);

            // Act
            Action act = () => actual.Should().BeCloseTo(time, -1.Ticks());

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("precision")
                .WithMessage("*must be non-negative*");
        }

        [Fact]
        public void When_a_time_is_close_to_a_later_time_by_one_tick_it_should_succeed()
        {
            // Arrange
            var time = TimeOnly.FromDateTime(DateTime.UtcNow);
            var actual = new TimeOnly(time.Ticks - 1);

            // Act / Assert
            actual.Should().BeCloseTo(time, TimeSpan.FromTicks(1));
        }

        [Fact]
        public void When_a_time_is_close_to_an_earlier_time_by_one_tick_it_should_succeed()
        {
            // Arrange
            var time = TimeOnly.FromDateTime(DateTime.UtcNow);
            var actual = new TimeOnly(time.Ticks + 1);

            // Act / Assert
            actual.Should().BeCloseTo(time, TimeSpan.FromTicks(1));
        }

        [Fact]
        public void When_subject_time_is_close_to_the_minimum_time_it_should_succeed()
        {
            // Arrange
            TimeOnly time = TimeOnly.MinValue.Add(50.Milliseconds());
            TimeOnly nearbyTime = TimeOnly.MinValue;

            // Act / Assert
            time.Should().BeCloseTo(nearbyTime, 100.Milliseconds());
        }

        [Fact]
        public void When_subject_time_is_close_to_the_maximum_time_it_should_succeed()
        {
            // Arrange
            TimeOnly time = TimeOnly.MaxValue.Add(-50.Milliseconds());
            TimeOnly nearbyTime = TimeOnly.MaxValue;

            // Act / Assert
            time.Should().BeCloseTo(nearbyTime, 100.Milliseconds());
        }

        [Fact]
        public void When_subject_time_is_close_to_another_value_that_is_later_by_more_than_20ms_it_should_throw()
        {
            // Arrange
            TimeOnly time = new(12, 15, 30, 979);
            TimeOnly nearbyTime = new(12, 15, 31);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 20.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected time to be within 20ms from <12:15:31.000>, but <12:15:30.979> was off by 21ms.");
        }

        [Fact]
        public void When_subject_time_is_close_to_another_value_that_is_earlier_by_more_than_20ms_it_should_throw()
        {
            // Arrange
            TimeOnly time = new(12, 15, 31, 021);
            TimeOnly nearbyTime = new(12, 15, 31);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 20.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected time to be within 20ms from <12:15:31.000>, but <12:15:31.021> was off by 21ms.");
        }

        [Fact]
        public void When_subject_time_is_close_to_an_earlier_time_by_35ms_it_should_succeed()
        {
            // Arrange
            TimeOnly time = new(12, 15, 31, 035);
            TimeOnly nearbyTime = new(12, 15, 31);

            // Act / Assert
            time.Should().BeCloseTo(nearbyTime, 35.Milliseconds());
        }

        [Fact]
        public void A_time_is_close_to_a_later_time_when_passing_midnight()
        {
            // Arrange
            TimeOnly time = new(23, 59, 0);
            TimeOnly nearbyTime = new(0, 1, 0);

            // Act / Assert
            time.Should().BeCloseTo(nearbyTime, 2.Minutes());
        }

        [Fact]
        public void A_time_is_close_to_an_earlier_time_when_passing_midnight()
        {
            // Arrange
            TimeOnly time = new(0, 1, 0);
            TimeOnly nearbyTime = new(23, 59, 0);

            // Act / Assert
            time.Should().BeCloseTo(nearbyTime, 2.Minutes());
        }

        [Fact]
        public void A_time_outside_of_the_precision_to_a_later_time_when_passing_midnight_fails()
        {
            // Arrange
            TimeOnly time = new(23, 58, 59);
            TimeOnly nearbyTime = new(0, 1, 0);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 2.Minutes());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * to be within 2m from <00:01:00.000>*, but <23:58:59.000> was off by 2m and 1s*");
        }

        [Fact]
        public void A_time_outside_of_the_precision_to_an_earlier_time_when_passing_midnight_fails()
        {
            // Arrange
            TimeOnly time = new(0, 1, 0);
            TimeOnly nearbyTime = new(23, 58, 59);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 2.Minutes());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * to be within 2m from <23:58:59.000>*, but <00:01:00.000> was off by 2m and 1s*");
        }

        [Fact]
        public void When_subject_nulltime_is_close_to_another_it_should_throw()
        {
            // Arrange
            TimeOnly? time = null;
            TimeOnly nearbyTime = new(12, 15, 31);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, 35.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*, but found <null>.");
        }

        [Fact]
        public void A_null_time_inside_an_assertion_scope_fails()
        {
            // Arrange
            TimeOnly? time = null;
            TimeOnly nearbyTime = new(12, 15, 31);

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                time.Should().BeCloseTo(nearbyTime, 35.Milliseconds());
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*, but found <null>.");
        }
    }

    public class NotBeCloseTo
    {
        [Fact]
        public void A_null_time_is_never_unclose_to_an_other_time()
        {
            // Arrange
            TimeOnly? time = null;
            TimeOnly nearbyTime = new(12, 15, 31);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 35.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect*, but found <null>.");
        }

        [Fact]
        public void A_null_time_inside_an_assertion_scope_is_never_unclose_to_an_other_time()
        {
            // Arrange
            TimeOnly? time = null;
            TimeOnly nearbyTime = new(12, 15, 31);

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                time.Should().NotBeCloseTo(nearbyTime, 35.Milliseconds());
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect*, but found <null>.");
        }

        [Fact]
        public void When_time_is_not_close_to_a_negative_precision_it_should_throw()
        {
            // Arrange
            var time = TimeOnly.FromDateTime(DateTime.UtcNow);
            var actual = new TimeOnly(time.Ticks - 1);

            // Act
            Action act = () => actual.Should().NotBeCloseTo(time, -1.Ticks());

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("precision")
                .WithMessage("*must be non-negative*");
        }

        [Fact]
        public void When_a_time_is_close_to_a_later_time_by_one_tick_it_should_fail()
        {
            // Arrange
            var time = TimeOnly.FromDateTime(DateTime.UtcNow);
            var actual = new TimeOnly(time.Ticks - 1);

            // Act
            Action act = () => actual.Should().NotBeCloseTo(time, TimeSpan.FromTicks(1));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_time_is_close_to_an_earlier_time_by_one_tick_it_should_fail()
        {
            // Arrange
            var time = TimeOnly.FromDateTime(DateTime.UtcNow);
            var actual = new TimeOnly(time.Ticks + 1);

            // Act
            Action act = () => actual.Should().NotBeCloseTo(time, TimeSpan.FromTicks(1));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_time_is_close_to_a_min_value_by_one_tick_it_should_fail()
        {
            // Arrange
            var time = TimeOnly.MinValue;
            var actual = new TimeOnly(time.Ticks + 1);

            // Act
            Action act = () => actual.Should().NotBeCloseTo(time, TimeSpan.FromTicks(1));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_time_is_close_to_a_max_value_by_one_tick_it_should_fail()
        {
            // Arrange
            var time = TimeOnly.MaxValue;
            var actual = new TimeOnly(time.Ticks - 1);

            // Act
            Action act = () => actual.Should().NotBeCloseTo(time, TimeSpan.FromTicks(1));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_subject_time_is_not_close_to_an_earlier_time_it_should_throw()
        {
            // Arrange
            TimeOnly time = new(12, 15, 31, 020);
            TimeOnly nearbyTime = new(12, 15, 31);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 20.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect time to be within 20ms from <12:15:31.000>, but it was <12:15:31.020>.");
        }

        [Fact]
        public void When_asserting_subject_time_is_not_close_to_an_earlier_time_by_a_20ms_timespan_it_should_throw()
        {
            // Arrange
            TimeOnly time = new(12, 15, 31, 020);
            TimeOnly nearbyTime = new(12, 15, 31);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(20));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect time to be within 20ms from <12:15:31.000>, but it was <12:15:31.020>.");
        }

        [Fact]
        public void When_asserting_subject_time_is_not_close_to_another_value_that_is_later_by_more_than_20ms_it_should_succeed()
        {
            // Arrange
            TimeOnly time = new(12, 15, 30, 979);
            TimeOnly nearbyTime = new(12, 15, 31);

            // Act / Assert
            time.Should().NotBeCloseTo(nearbyTime, 20.Milliseconds());
        }

        [Fact]
        public void
            When_asserting_subject_time_is_not_close_to_another_value_that_is_earlier_by_more_than_20ms_it_should_succeed()
        {
            // Arrange
            TimeOnly time = new(12, 15, 31, 021);
            TimeOnly nearbyTime = new(12, 15, 31);

            // Act / Assert
            time.Should().NotBeCloseTo(nearbyTime, 20.Milliseconds());
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_close_to_an_earlier_datetime_by_35ms_it_should_throw()
        {
            // Arrange
            TimeOnly time = new(12, 15, 31, 035);
            TimeOnly nearbyTime = new(12, 15, 31);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 35.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect time to be within 35ms from <12:15:31.000>, but it was <12:15:31.035>.");
        }

        [Fact]
        public void When_asserting_subject_time_is_not_close_to_the_minimum_time_it_should_throw()
        {
            // Arrange
            TimeOnly time = TimeOnly.MinValue.Add(50.Milliseconds());
            TimeOnly nearbyTime = TimeOnly.MinValue;

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect time to be within 100ms from <00:00:00.000>, but it was <00:00:00.050>.");
        }

        [Fact]
        public void When_asserting_subject_time_is_not_close_to_the_maximum_time_it_should_throw()
        {
            // Arrange
            TimeOnly time = TimeOnly.MaxValue.Add(-50.Milliseconds());
            TimeOnly nearbyTime = TimeOnly.MaxValue;

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect time to be within 100ms from <23:59:59.999>, but it was <23:59:59.949>.");
        }

        [Fact]
        public void A_time_is_not_close_to_a_later_time_when_passing_midnight()
        {
            // Arrange
            TimeOnly time = new(23, 58, 0);
            TimeOnly nearbyTime = new(0, 1, 0);

            // Act / Assert
            time.Should().NotBeCloseTo(nearbyTime, 2.Minutes());
        }

        [Fact]
        public void A_time_is_not_close_to_an_earlier_time_when_passing_midnight()
        {
            // Arrange
            TimeOnly time = new(0, 2, 0);
            TimeOnly nearbyTime = new(23, 59, 0);

            // Act / Assert
            time.Should().NotBeCloseTo(nearbyTime, 2.Minutes());
        }

        [Fact]
        public void A_time_inside_of_the_precision_to_a_later_time_when_passing_midnight_fails()
        {
            // Arrange
            TimeOnly time = new(23, 59, 0);
            TimeOnly nearbyTime = new(0, 1, 0);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 2.Minutes());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect * to be within 2m from <00:01:00.000>*, but it was <23:59:00.000>*");
        }

        [Fact]
        public void A_time_inside_of_the_precision_to_an_earlier_time_when_passing_midnight_fails()
        {
            // Arrange
            TimeOnly time = new(0, 1, 0);
            TimeOnly nearbyTime = new(23, 59, 0);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 2.Minutes());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect * to be within 2m from <23:59:00.000>*, but it was <00:01:00.000>*");
        }
    }

    public class BeBefore
    {
        [Fact]
        public void When_asserting_subject_is_not_before_earlier_expected_timeonly_it_should_succeed()
        {
            // Arrange
            TimeOnly expected = new(15, 06, 03);
            TimeOnly subject = new(15, 06, 04);

            // Act/Assert
            subject.Should().NotBeBefore(expected);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_before_the_same_timeonly_it_should_throw()
        {
            // Arrange
            TimeOnly expected = new(15, 06, 04);
            TimeOnly subject = new(15, 06, 04);

            // Act
            Action act = () => subject.Should().BeBefore(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <15:06:04.000>, but found <15:06:04.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_before_the_same_timeonly_it_should_succeed()
        {
            // Arrange
            TimeOnly expected = new(15, 06, 04);
            TimeOnly subject = new(15, 06, 04);

            // Act/Assert
            subject.Should().NotBeBefore(expected);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_on_or_before_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04, 175);
            TimeOnly expectation = new(15, 06, 05, 23);

            // Act/Assert
            subject.Should().BeOnOrBefore(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_on_or_before_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04, 150);
            TimeOnly expectation = new(15, 06, 05, 340);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <15:06:05.340>, but found <15:06:04.150>.");
        }

        [Fact]
        public void
            When_asserting_subject_timeonly_is_on_or_before_the_same_time_as_the_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 04);

            // Act/Assert
            subject.Should().BeOnOrBefore(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_on_or_before_the_same_time_as_the_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04, 123);
            TimeOnly expectation = new(15, 06, 04, 123);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <15:06:04.123>, but found <15:06:04.123>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_on_or_before_earlier_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 07);
            TimeOnly expectation = new(15, 06);

            // Act
            Action act = () => subject.Should().BeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or before <15:06:00.000>, but found <15:07:00.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_on_or_before_earlier_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 03);

            // Act/Assert
            subject.Should().NotBeOnOrBefore(expectation);
        }
    }

    public class BeAfter
    {
        [Fact]
        public void When_asserting_subject_timeonly_is_after_earlier_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04, 123);
            TimeOnly expectation = new(15, 06, 03, 45);

            // Act/Assert
            subject.Should().BeAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_after_earlier_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or before <15:06:03.000>, but found <15:06:04.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_after_later_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 05);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <15:06:05.000>, but found <15:06:04.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_after_later_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 05);

            // Act/Assert
            subject.Should().NotBeAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_after_the_same_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04, 145);
            TimeOnly expectation = new(15, 06, 04, 145);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <15:06:04.145>, but found <15:06:04.145>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_after_the_same_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04, 123);
            TimeOnly expectation = new(15, 06, 04, 123);

            // Act/Assert
            subject.Should().NotBeAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_on_or_after_earlier_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 07);
            TimeOnly expectation = new(15, 06);

            // Act/Assert
            subject.Should().BeOnOrAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_on_or_after_earlier_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <15:06:03.000>, but found <15:06:04.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_on_or_after_the_same_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 04);

            // Act/Assert
            subject.Should().BeOnOrAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_on_or_after_the_same_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06);
            TimeOnly expectation = new(15, 06);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <15:06:00.000>, but found <15:06:00.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_on_or_after_later_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 05);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or after <15:06:05.000>, but found <15:06:04.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_on_or_after_later_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 05);

            // Act/Assert
            subject.Should().NotBeOnOrAfter(expectation);
        }
    }

    public class HaveHours
    {
        [Fact]
        public void When_asserting_subject_timeonly_should_have_hours_with_the_same_value_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31);
            const int expectation = 15;

            // Act/Assert
            subject.Should().HaveHours(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_hours_with_the_same_value_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31);
            const int expectation = 15;

            // Act
            Action act = () => subject.Should().NotHaveHours(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the hours part of subject to be 15, but it was.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_have_hours_with_a_different_value_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31);
            const int expectation = 14;

            // Act
            Action act = () => subject.Should().HaveHours(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the hours part of subject to be 14, but found 15.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_hours_with_a_different_value_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(21, 12, 31);
            const int expectation = 23;

            // Act/Assert
            subject.Should().NotHaveHours(expectation);
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_have_hours_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 21;

            // Act
            Action act = () => subject.Should().HaveHours(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the hours part of subject to be 21, but found <null>.");
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_not_have_hours_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 19;

            // Act
            Action act = () => subject.Should().NotHaveHours(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the hours part of subject to be 19, but found a <null> TimeOnly.");
        }
    }

    public class HaveMinutes
    {
        [Fact]
        public void When_asserting_subject_timeonly_should_have_minutes_with_the_same_value_it_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(21, 12, 31);
            const int expectation = 12;

            // Act/Assert
            subject.Should().HaveMinutes(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_minutes_with_the_same_value_it_should_throw()
        {
            // Arrange
            TimeOnly subject = new(21, 12, 31);
            const int expectation = 12;

            // Act
            Action act = () => subject.Should().NotHaveMinutes(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the minutes part of subject to be 12, but it was.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_have_a_minute_with_a_different_value_it_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31);
            const int expectation = 11;

            // Act
            Action act = () => subject.Should().HaveMinutes(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the minutes part of subject to be 11, but found 12.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_a_minute_with_a_different_value_it_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31);
            const int expectation = 11;

            // Act/Assert
            subject.Should().NotHaveMinutes(expectation);
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_have_minutes_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 12;

            // Act
            Action act = () => subject.Should().HaveMinutes(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the minutes part of subject to be 12, but found a <null> TimeOnly.");
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_not_have_minutes_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 12;

            // Act
            Action act = () => subject.Should().NotHaveMinutes(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the minutes part of subject to be 12, but found a <null> TimeOnly.");
        }
    }

    public class HaveSeconds
    {
        [Fact]
        public void When_asserting_subject_timeonly_should_have_seconds_with_the_same_value_it_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(14, 12, 31);
            const int expectation = 31;

            // Act/Assert
            subject.Should().HaveSeconds(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_seconds_with_the_same_value_it_should_throw()
        {
            // Arrange
            TimeOnly subject = new(14, 12, 31);
            const int expectation = 31;

            // Act
            Action act = () => subject.Should().NotHaveSeconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the seconds part of subject to be 31, but it was.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_have_seconds_with_a_different_value_it_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31);
            const int expectation = 30;

            // Act
            Action act = () => subject.Should().HaveSeconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the seconds part of subject to be 30, but found 31.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_seconds_with_a_different_value_it_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31);
            const int expectation = 30;

            // Act/Assert
            subject.Should().NotHaveSeconds(expectation);
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_have_seconds_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveSeconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the seconds part of subject to be 22, but found a <null> TimeOnly.");
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_not_have_seconds_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveSeconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the seconds part of subject to be 22, but found a <null> TimeOnly.");
        }
    }

    public class HaveMilliseconds
    {
        [Fact]
        public void When_asserting_subject_timeonly_should_have_milliseconds_with_the_same_value_it_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(14, 12, 31, 123);
            const int expectation = 123;

            // Act/Assert
            subject.Should().HaveMilliseconds(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_milliseconds_with_the_same_value_it_should_throw()
        {
            // Arrange
            TimeOnly subject = new(14, 12, 31, 445);
            const int expectation = 445;

            // Act
            Action act = () => subject.Should().NotHaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 445, but it was.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_have_milliseconds_with_a_different_value_it_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31, 555);
            const int expectation = 12;

            // Act
            Action act = () => subject.Should().HaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 12, but found 555.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_milliseconds_with_a_different_value_it_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31, 445);
            const int expectation = 31;

            // Act/Assert
            subject.Should().NotHaveMilliseconds(expectation);
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_have_milliseconds_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 22, but found a <null> TimeOnly.");
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_not_have_milliseconds_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 22, but found a <null> TimeOnly.");
        }
    }

    public class BeOneOf
    {
        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            TimeOnly value = new(15, 12, 20);

            // Act
            Action action = () => value.Should().BeOneOf(value.AddHours(1), value.AddMinutes(-1));

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {<16:12:20.000>, <15:11:20.000>}, but found <15:12:20.000>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed()
        {
            // Arrange
            TimeOnly value = new(15, 12, 30);

            // Act/Assert
            value.Should().BeOneOf(new TimeOnly(4, 1, 30), new TimeOnly(15, 12, 30));
        }

        [Fact]
        public void When_a_null_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            TimeOnly? value = null;

            // Act
            Action action = () => value.Should().BeOneOf(new TimeOnly(15, 1, 30), new TimeOnly(5, 4, 10, 123));

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {<15:01:30.000>, <05:04:10.123>}, but found <null>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed_when_timeonly_is_null()
        {
            // Arrange
            TimeOnly? value = null;

            // Act/Assert
            value.Should().BeOneOf(new TimeOnly(15, 1, 30), null);
        }
    }

    public class NotBe
    {
        [Fact]
        public void Different_timeonly_values_are_valid()
        {
            // Arrange
            TimeOnly time = new(19, 06, 04);
            TimeOnly otherTime = new(20, 06, 05);

            // Act & Assert
            time.Should().NotBe(otherTime);
        }

        [Fact]
        public void Different_timeonly_values_with_different_nullability_are_valid()
        {
            // Arrange
            TimeOnly time = new(19, 06, 04);
            TimeOnly? otherTime = new(19, 07, 05);

            // Act & Assert
            time.Should().NotBe(otherTime);
        }

        [Fact]
        public void Same_timeonly_values_are_invalid()
        {
            // Arrange
            TimeOnly time = new(19, 06, 04);
            TimeOnly sameTime = new(19, 06, 04);

            // Act
            Action act =
                () => time.Should().NotBe(sameTime, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected time not to be <19:06:04.000> because we want to test the failure message, but it is.");
        }

        [Fact]
        public void Same_timeonly_values_with_different_nullability_are_invalid()
        {
            // Arrange
            TimeOnly time = new(19, 06, 04);
            TimeOnly? sameTime = new(19, 06, 04);

            // Act
            Action act =
                () => time.Should().NotBe(sameTime, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected time not to be <19:06:04.000> because we want to test the failure message, but it is.");
        }
    }

    public class AndChaining
    {
        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            TimeOnly earlierTimeOnly = new(15, 06, 03);
            TimeOnly? nullableTimeOnly = new(15, 06, 04);

            // Act/Assert
            nullableTimeOnly.Should()
                .HaveValue()
                .And
                .BeAfter(earlierTimeOnly);
        }
    }

    public class Miscellaneous
    {
        [Fact]
        public void Should_throw_a_helpful_error_when_accidentally_using_equals()
        {
            // Arrange
            TimeOnly someTimeOnly = new(21, 1);

            // Act
            var act = () => someTimeOnly.Should().Equals(null);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
        }
    }
}

#endif
