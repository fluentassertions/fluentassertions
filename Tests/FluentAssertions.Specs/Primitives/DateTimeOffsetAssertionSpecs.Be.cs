using System;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class Be
    {
        [Fact]
        public void Should_succeed_when_asserting_datetimeoffset_value_is_equal_to_the_same_value()
        {
            // Arrange
            DateTimeOffset dateTime = new DateTime(2016, 06, 04).ToDateTimeOffset();
            DateTimeOffset sameDateTime = new DateTime(2016, 06, 04).ToDateTimeOffset();

            // Act
            Action act = () => dateTime.Should().Be(sameDateTime);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_datetimeoffset_value_is_equal_to_the_same_nullable_value_be_should_succeed()
        {
            // Arrange
            DateTimeOffset dateTime = 4.June(2016).ToDateTimeOffset();
            DateTimeOffset? sameDateTime = 4.June(2016).ToDateTimeOffset();

            // Act
            Action act = () => dateTime.Should().Be(sameDateTime);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_both_values_are_at_their_minimum_then_it_should_succeed()
        {
            // Arrange
            DateTimeOffset dateTime = DateTimeOffset.MinValue;
            DateTimeOffset sameDateTime = DateTimeOffset.MinValue;

            // Act
            Action act = () => dateTime.Should().Be(sameDateTime);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_both_values_are_at_their_maximum_then_it_should_succeed()
        {
            // Arrange
            DateTimeOffset dateTime = DateTimeOffset.MaxValue;
            DateTimeOffset sameDateTime = DateTimeOffset.MaxValue;

            // Act
            Action act = () => dateTime.Should().Be(sameDateTime);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_datetimeoffset_value_is_equal_to_the_different_value()
        {
            // Arrange
            var dateTime = 10.March(2012).WithOffset(1.Hours());
            var otherDateTime = 11.March(2012).WithOffset(1.Hours());

            // Act
            Action act = () => dateTime.Should().Be(otherDateTime, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dateTime to represent the same point in time as <2012-03-11 +1h>*failure message, but <2012-03-10 +1h> does not.");
        }

        [Fact]
        public void When_datetimeoffset_value_is_equal_to_the_different_nullable_value_be_should_failed()
        {
            // Arrange
            DateTimeOffset dateTime = 10.March(2012).WithOffset(1.Hours());
            DateTimeOffset? otherDateTime = 11.March(2012).WithOffset(1.Hours());

            // Act
            Action act = () => dateTime.Should().Be(otherDateTime, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dateTime to represent the same point in time as <2012-03-11 +1h>*failure message, but <2012-03-10 +1h> does not.");
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_datetimeoffset_value_equals_the_same_value()
        {
            // Arrange
            DateTimeOffset? nullableDateTimeA = new DateTime(2016, 06, 04).ToDateTimeOffset();
            DateTimeOffset? nullableDateTimeB = new DateTime(2016, 06, 04).ToDateTimeOffset();

            // Act
            Action action = () =>
                nullableDateTimeA.Should().Be(nullableDateTimeB);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_datetimeoffset_null_value_equals_null()
        {
            // Arrange
            DateTimeOffset? nullableDateTimeA = null;
            DateTimeOffset? nullableDateTimeB = null;

            // Act / Assert
            nullableDateTimeA.Should().Be(nullableDateTimeB);
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetimeoffset_value_equals_a_different_value()
        {
            // Arrange
            DateTimeOffset? nullableDateTimeA = new DateTime(2016, 06, 04).ToDateTimeOffset();
            DateTimeOffset? nullableDateTimeB = new DateTime(2016, 06, 06).ToDateTimeOffset();

            // Act
            Action action = () =>
                nullableDateTimeA.Should().Be(nullableDateTimeB);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_datetimeoffset_null_value_is_equal_to_another_value()
        {
            // Arrange
            DateTimeOffset? nullableDateTime = null;
            DateTimeOffset expectation = 27.March(2016).ToDateTimeOffset(1.Hours());

            // Act
            Action action = () =>
                nullableDateTime.Should().Be(expectation, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected nullableDateTime to represent the same point in time as <2016-03-27 +1h> because we want to test the failure message, but found a <null> DateTimeOffset.");
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_non_null_value_is_equal_to_null_value()
        {
            // Arrange
            DateTimeOffset? nullableDateTime = 27.March(2016).ToDateTimeOffset(1.Hours());
            DateTimeOffset? expectation = null;

            // Act
            Action action = () =>
                nullableDateTime.Should().Be(expectation, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected nullableDateTime to be <null> because we want to test the failure message, but it was <2016-03-27 +1h>.");
        }

        [Fact]
        public void
            When_asserting_different_date_time_offsets_representing_the_same_world_time_it_should_succeed()
        {
            // Arrange
            var specificDate = 1.May(2008).At(6, 32);

            var dateWithFiveHourOffset = new DateTimeOffset(specificDate - 5.Hours(), -5.Hours());

            var dateWithSixHourOffset = new DateTimeOffset(specificDate - 6.Hours(), -6.Hours());

            // Act / Assert
            dateWithFiveHourOffset.Should().Be(dateWithSixHourOffset);
        }
    }

    public class NotBe
    {
        [Fact]
        public void Should_succeed_when_asserting_datetimeoffset_value_is_not_equal_to_a_different_value()
        {
            // Arrange
            DateTimeOffset dateTime = new DateTime(2016, 06, 04).ToDateTimeOffset();
            DateTimeOffset otherDateTime = new DateTime(2016, 06, 05).ToDateTimeOffset();

            // Act
            Action act = () => dateTime.Should().NotBe(otherDateTime);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_datetimeoffset_value_is_not_equal_to_a_nullable_different_value_notbe_should_succeed()
        {
            // Arrange
            DateTimeOffset dateTime = 4.June(2016).ToDateTimeOffset();
            DateTimeOffset? otherDateTime = 5.June(2016).ToDateTimeOffset();

            // Act
            Action act = () => dateTime.Should().NotBe(otherDateTime);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_datetimeoffset_value_is_not_equal_to_the_same_value()
        {
            // Arrange
            var dateTime = new DateTimeOffset(10.March(2012), 1.Hours());
            var sameDateTime = new DateTimeOffset(10.March(2012), 1.Hours());

            // Act
            Action act =
                () => dateTime.Should().NotBe(sameDateTime, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect dateTime to represent the same point in time as <2012-03-10 +1h> because we want to test the failure message, but it did.");
        }

        [Fact]
        public void When_datetimeoffset_value_is_not_equal_to_the_same_nullable_value_notbe_should_failed()
        {
            // Arrange
            DateTimeOffset dateTime = new(10.March(2012), 1.Hours());
            DateTimeOffset? sameDateTime = new DateTimeOffset(10.March(2012), 1.Hours());

            // Act
            Action act =
                () => dateTime.Should().NotBe(sameDateTime, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect dateTime to represent the same point in time as <2012-03-10 +1h> because we want to test the failure message, but it did.");
        }

        [Fact]
        public void
            When_asserting_different_date_time_offsets_representing_different_world_times_it_should_not_succeed()
        {
            // Arrange
            var specificDate = 1.May(2008).At(6, 32);

            var dateWithZeroHourOffset = new DateTimeOffset(specificDate, TimeSpan.Zero);
            var dateWithOneHourOffset = new DateTimeOffset(specificDate, 1.Hours());

            // Act / Assert
            dateWithZeroHourOffset.Should().NotBe(dateWithOneHourOffset);
        }
    }
}
