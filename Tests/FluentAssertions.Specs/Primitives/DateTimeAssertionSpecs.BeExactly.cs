using System;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class BeExactly
    {
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
                "Expected subject <12:36:00> to be exactly 20m before <12:55:00> because 20 minutes is enough, but it is behind by 19m.");
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
        public void When_asserting_subject_be_exactly_10_seconds_after_target_but_subject_is_before_target_it_should_throw()
        {
            // Arrange
            var expectation = 1.January(0001).At(0, 0, 30);
            var subject = 1.January(0001).At(0, 0, 20);

            // Ac
            Action action = () => subject.Should().BeExactly(10.Seconds()).After(expectation);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject <00:00:20> to be exactly 10s after <00:00:30>, but it is behind by 10s.");
        }

        [Fact]
        public void When_asserting_subject_be_exactly_10_seconds_before_target_but_subject_is_after_target_it_should_throw()
        {
            // Arrange
            var expectation = 1.January(0001).At(0, 0, 30);
            var subject = 1.January(0001).At(0, 0, 40);

            // Act
            Action action = () => subject.Should().BeExactly(10.Seconds()).Before(expectation);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject <00:00:40> to be exactly 10s before <00:00:30>, but it is ahead by 10s.");
        }
    }
}
