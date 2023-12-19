using System;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class BeMoreThan
    {
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
                "Expected subject <2009-10-01> to be more than 1d before <2009-10-02> because we like that, but it is behind by 1d.");
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
        public void When_asserting_subject_be_more_than_10_seconds_after_target_but_subject_is_before_target_it_should_throw()
        {
            // Arrange
            var expectation = 1.January(0001).At(0, 0, 30);
            var subject = 1.January(0001).At(0, 0, 15);

            // Act
            Action action = () => subject.Should().BeMoreThan(10.Seconds()).After(expectation);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject <00:00:15> to be more than 10s after <00:00:30>, but it is behind by 15s.");
        }

        [Fact]
        public void When_asserting_subject_be_more_than_10_seconds_before_target_but_subject_is_after_target_it_should_throw()
        {
            // Arrange
            var expectation = 1.January(0001).At(0, 0, 30);
            var subject = 1.January(0001).At(0, 0, 45);

            // Act
            Action action = () => subject.Should().BeMoreThan(10.Seconds()).Before(expectation);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject <00:00:45> to be more than 10s before <00:00:30>, but it is ahead by 15s.");
        }
    }
}
