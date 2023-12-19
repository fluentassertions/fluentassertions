using System;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class BeWithin
    {
        [Fact]
        public void When_date_is_not_within_50_hours_before_another_date_it_should_throw()
        {
            // Arrange
            var target = 10.April(2010).At(12, 0).WithOffset(0.Hours());
            DateTimeOffset subject = target - 50.Hours() - 1.Seconds();

            // Act
            Action act =
                () => subject.Should().BeWithin(TimeSpan.FromHours(50)).Before(target, "{0} hours is enough", 50);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject <2010-04-08 09:59:59 +0h> to be within 2d and 2h before <2010-04-10 12:00:00 +0h> because 50 hours is enough, but it is behind by 2d, 2h and 1s.");
        }

        [Fact]
        public void When_date_is_exactly_within_1d_before_another_date_it_should_not_throw()
        {
            // Arrange
            var target = new DateTimeOffset(10.April(2010));
            DateTimeOffset subject = target - 1.Days();

            // Act / Assert
            subject.Should().BeWithin(TimeSpan.FromHours(24)).Before(target);
        }

        [Fact]
        public void When_date_is_within_1d_before_another_date_it_should_not_throw()
        {
            // Arrange
            var target = new DateTimeOffset(10.April(2010));
            DateTimeOffset subject = target - 23.Hours();

            // Act / Assert
            subject.Should().BeWithin(TimeSpan.FromHours(24)).Before(target);
        }

        [Fact]
        public void When_a_utc_date_is_within_0s_before_itself_it_should_not_throw()
        {
            // Arrange
            var date = DateTimeOffset.UtcNow; // local timezone differs from +0h

            // Act / Assert
            date.Should().BeWithin(TimeSpan.Zero).Before(date);
        }

        [Fact]
        public void When_a_utc_date_is_within_0s_after_itself_it_should_not_throw()
        {
            // Arrange
            var date = DateTimeOffset.UtcNow; // local timezone differs from +0h

            // Act / Assert
            date.Should().BeWithin(TimeSpan.Zero).After(date);
        }

        [Theory]
        [InlineData(30, 20)] // edge case
        [InlineData(30, 25)]
        public void When_asserting_subject_be_within_10_seconds_after_target_but_subject_is_before_target_it_should_throw(
            int targetSeconds, int subjectSeconds)
        {
            // Arrange
            var expectation = 1.January(0001).At(0, 0, targetSeconds).WithOffset(0.Hours());
            var subject = 1.January(0001).At(0, 0, subjectSeconds).WithOffset(0.Hours());

            // Act
            Action action = () => subject.Should().BeWithin(10.Seconds()).After(expectation);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    $"Expected subject <00:00:{subjectSeconds} +0h> to be within 10s after <00:00:30 +0h>, but it is behind by {Math.Abs(subjectSeconds - targetSeconds)}s.");
        }

        [Theory]
        [InlineData(30, 40)] // edge case
        [InlineData(30, 35)]
        public void When_asserting_subject_be_within_10_seconds_before_target_but_subject_is_after_target_it_should_throw(
            int targetSeconds, int subjectSeconds)
        {
            // Arrange
            var expectation = 1.January(0001).At(0, 0, targetSeconds).WithOffset(0.Hours());
            var subject = 1.January(0001).At(0, 0, subjectSeconds).WithOffset(0.Hours());

            // Act
            Action action = () => subject.Should().BeWithin(10.Seconds()).Before(expectation);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    $"Expected subject <00:00:{subjectSeconds} +0h> to be within 10s before <00:00:30 +0h>, but it is ahead by {Math.Abs(subjectSeconds - targetSeconds)}s.");
        }
    }
}
