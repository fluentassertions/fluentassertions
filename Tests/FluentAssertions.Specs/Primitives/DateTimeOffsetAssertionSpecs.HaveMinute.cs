using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class HaveMinute
    {
        [Fact]
        public void When_asserting_subject_datetimeoffset_should_have_minutes_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31, 23, 59, 00), TimeSpan.Zero);
            int expectation = 59;

            // Act
            Action act = () => subject.Should().HaveMinute(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_should_have_minutes_with_different_value_it_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31, 23, 59, 00), TimeSpan.Zero);
            int expectation = 58;

            // Act
            Action act = () => subject.Should().HaveMinute(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the minute part of subject to be 58, but it was 59.");
        }

        [Fact]
        public void When_asserting_subject_null_datetimeoffset_should_have_minute_should_throw()
        {
            // Arrange
            DateTimeOffset? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveMinute(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the minute part of subject to be 22, but found a <null> DateTimeOffset.");
        }
    }

    public class NotHaveMinute
    {
        [Fact]
        public void When_asserting_subject_datetimeoffset_should_not_have_minutes_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31, 23, 59, 00), TimeSpan.Zero);
            int expectation = 59;

            // Act
            Action act = () => subject.Should().NotHaveMinute(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the minute part of subject to be 59, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_should_not_have_minutes_with_different_value_it_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31, 23, 59, 00), TimeSpan.Zero);
            int expectation = 58;

            // Act
            Action act = () => subject.Should().NotHaveMinute(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetimeoffset_should_not_have_minute_should_throw()
        {
            // Arrange
            DateTimeOffset? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveMinute(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the minute part of subject to be 22, but found a <null> DateTimeOffset.");
        }
    }
}
