using System;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class BeLessThan
    {
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
                "Expected subject <12:01:00> to be less than 30s after <12:00:30> because 30s is the max, but it is ahead by 30s.");
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

        [Fact]
        public void When_asserting_subject_be_less_than_10_seconds_after_target_but_subject_is_before_target_it_should_throw()
        {
            // Arrange
            var expectation = 1.January(0001).At(0, 0, 30);
            var subject = 1.January(0001).At(0, 0, 25);

            // Act
            Action action = () => subject.Should().BeLessThan(10.Seconds()).After(expectation);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject <00:00:25> to be less than 10s after <00:00:30>, but it is behind by 5s.");
        }

        [Fact]
        public void When_asserting_subject_be_less_than_10_seconds_before_target_but_subject_is_after_target_it_should_throw()
        {
            // Arrange
            var expectation = 1.January(0001).At(0, 0, 30);
            var subject = 1.January(0001).At(0, 0, 45);

            // Act
            Action action = () => subject.Should().BeLessThan(10.Seconds()).Before(expectation);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject <00:00:45> to be less than 10s before <00:00:30>, but it is ahead by 15s.");
        }
    }
}
