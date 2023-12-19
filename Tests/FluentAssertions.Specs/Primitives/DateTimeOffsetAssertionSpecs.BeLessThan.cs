using System;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class BeLessThan
    {
        [Fact]
        public void When_time_is_not_less_than_30s_after_another_time_it_should_throw()
        {
            // Arrange
            var target = 1.January(1).At(12, 0, 30).WithOffset(1.Hours());
            DateTimeOffset subject = target + 30.Seconds();

            // Act
            Action act =
                () => subject.Should().BeLessThan(TimeSpan.FromSeconds(30)).After(target, "{0}s is the max", 30);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject <12:01:00 +1h> to be less than 30s after <12:00:30 +1h> because 30s is the max, but it is ahead by 30s.");
        }

        [Fact]
        public void When_time_is_less_than_30s_after_another_time_it_should_not_throw()
        {
            // Arrange
            var target = new DateTimeOffset(1.January(1).At(12, 0, 30));
            DateTimeOffset subject = target + 20.Seconds();

            // Act / Assert
            subject.Should().BeLessThan(TimeSpan.FromSeconds(30)).After(target);
        }

        [Fact]
        public void When_asserting_subject_be_less_than_10_seconds_after_target_but_subject_is_before_target_it_should_throw()
        {
            // Arrange
            var expectation = 1.January(0001).At(0, 0, 30).WithOffset(0.Hours());
            var subject = 1.January(0001).At(0, 0, 25).WithOffset(0.Hours());

            // Act
            Action action = () => subject.Should().BeLessThan(10.Seconds()).After(expectation);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject <00:00:25 +0h> to be less than 10s after <00:00:30 +0h>, but it is behind by 5s.");
        }

        [Fact]
        public void When_asserting_subject_be_less_than_10_seconds_before_target_but_subject_is_after_target_it_should_throw()
        {
            // Arrange
            var expectation = 1.January(0001).At(0, 0, 30).WithOffset(0.Hours());
            var subject = 1.January(0001).At(0, 0, 45).WithOffset(0.Hours());

            // Act
            Action action = () => subject.Should().BeLessThan(10.Seconds()).Before(expectation);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected subject <00:00:45 +0h> to be less than 10s before <00:00:30 +0h>, but it is ahead by 15s.");
        }

        [Fact]
        public void Should_throw_a_helpful_error_when_accidentally_using_equals_with_a_range()
        {
            // Arrange
            DateTimeOffset someDateTimeOffset = new(2022, 9, 25, 13, 48, 42, 0, TimeSpan.Zero);

            // Act
            var action = () => someDateTimeOffset.Should().BeLessThan(0.Seconds()).Equals(null);

            // Assert
            action.Should().Throw<NotSupportedException>()
                .WithMessage("Equals is not part of Fluent Assertions. Did you mean Before() or After() instead?");
        }
    }
}
