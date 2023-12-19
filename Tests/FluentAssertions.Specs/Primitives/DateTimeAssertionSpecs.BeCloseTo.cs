using System;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class BeCloseTo
    {
        [Fact]
        public void When_asserting_that_time_is_close_to_a_negative_precision_it_should_throw()
        {
            // Arrange
            var dateTime = DateTime.UtcNow;
            var actual = new DateTime(dateTime.Ticks - 1);

            // Act
            Action act = () => actual.Should().BeCloseTo(dateTime, -1.Ticks());

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("precision")
                .WithMessage("*must be non-negative*");
        }

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
                    "Expected time to be within 20ms from <2012-03-13 12:15:31>, but <2012-03-13 12:15:30.979> was off by 21ms.");
        }

        [Fact]
        public void
            When_asserting_subject_datetime_is_close_to_another_value_that_is_later_by_more_than_a_20ms_timespan_it_should_throw()
        {
            // Arrange
            DateTime time = 13.March(2012).At(12, 15, 30, 979);
            DateTime nearbyTime = 13.March(2012).At(12, 15, 31);

            // Act
            Action act = () => time.Should().BeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(20));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected time to be within 20ms from <2012-03-13 12:15:31>, but <2012-03-13 12:15:30.979> was off by 21ms.");
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
                    "Expected time to be within 20ms from <2012-03-13 12:15:31>, but <2012-03-13 12:15:31.021> was off by 21ms.");
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
    }

    public class NotBeCloseTo
    {
        [Fact]
        public void When_asserting_that_time_is_not_close_to_a_negative_precision_it_should_throw()
        {
            // Arrange
            var dateTime = DateTime.UtcNow;
            var actual = new DateTime(dateTime.Ticks - 1);

            // Act
            Action act = () => actual.Should().NotBeCloseTo(dateTime, -1.Ticks());

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("precision")
                .WithMessage("*must be non-negative*");
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
        public void When_asserting_subject_datetime_is_not_close_to_a_later_datetime_it_should_throw()
        {
            // Arrange
            DateTime time = DateTime.SpecifyKind(new DateTime(2016, 06, 04).At(12, 15, 30, 980), DateTimeKind.Unspecified);
            DateTime nearbyTime = DateTime.SpecifyKind(new DateTime(2016, 06, 04).At(12, 15, 31), DateTimeKind.Utc);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 20.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Did not expect time to be within 20ms from <2016-06-04 12:15:31>, but it was <2016-06-04 12:15:30.980>.");
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
                .WithMessage(
                    "Did not expect time to be within 20ms from <2016-06-04 12:15:31>, but it was <2016-06-04 12:15:31.020>.");
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
                .WithMessage(
                    "Did not expect time to be within 20ms from <2016-06-04 12:15:31>, but it was <2016-06-04 12:15:31.020>.");
        }

        [Fact]
        public void
            When_asserting_subject_datetime_is_not_close_to_another_value_that_is_later_by_more_than_20ms_it_should_succeed()
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
        public void
            When_asserting_subject_datetime_is_not_close_to_another_value_that_is_earlier_by_more_than_20ms_it_should_succeed()
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
        public void When_asserting_subject_datetime_is_not_close_to_an_earlier_datetime_by_35ms_it_should_throw()
        {
            // Arrange
            DateTime time = new DateTime(2016, 06, 04).At(12, 15, 31, 035);
            DateTime nearbyTime = new DateTime(2016, 06, 04).At(12, 15, 31);

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 35.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Did not expect time to be within 35ms from <2016-06-04 12:15:31>, but it was <2016-06-04 12:15:31.035>.");
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
        public void When_asserting_subject_datetime_is_not_close_to_the_minimum_datetime_it_should_throw()
        {
            // Arrange
            DateTime time = DateTime.MinValue + 50.Milliseconds();
            DateTime nearbyTime = DateTime.MinValue;

            // Act
            Action act = () => time.Should().NotBeCloseTo(nearbyTime, 100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect time to be within 100ms from <0001-01-01 00:00:00.000>, but it was <00:00:00.050>.");
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
                .WithMessage(
                    "Did not expect time to be within 100ms from <9999-12-31 23:59:59.9999999>, but it was <9999-12-31 23:59:59.9499999>.");
        }
    }
}
