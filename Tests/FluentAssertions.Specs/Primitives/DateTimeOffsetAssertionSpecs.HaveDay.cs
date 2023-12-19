using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class HaveDay
    {
        [Fact]
        public void When_asserting_subject_datetimeoffset_should_have_day_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31), TimeSpan.Zero);
            int expectation = 31;

            // Act
            Action act = () => subject.Should().HaveDay(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_should_have_day_with_a_different_value_it_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31), TimeSpan.Zero);
            int expectation = 30;

            // Act
            Action act = () => subject.Should().HaveDay(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the day part of subject to be 30, but it was 31.");
        }

        [Fact]
        public void When_asserting_subject_null_datetimeoffset_should_have_day_should_throw()
        {
            // Arrange
            DateTimeOffset? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveDay(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the day part of subject to be 22, but found a <null> DateTimeOffset.");
        }
    }

    public class NotHaveDay
    {
        [Fact]
        public void When_asserting_subject_datetimeoffset_should_not_have_day_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31), TimeSpan.Zero);
            int expectation = 31;

            // Act
            Action act = () => subject.Should().NotHaveDay(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the day part of subject to be 31, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_should_not_have_day_with_a_different_value_it_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31), TimeSpan.Zero);
            int expectation = 30;

            // Act
            Action act = () => subject.Should().NotHaveDay(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetimeoffset_should_not_have_day_should_throw()
        {
            // Arrange
            DateTimeOffset? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveDay(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the day part of subject to be 22, but found a <null> DateTimeOffset.");
        }
    }
}
