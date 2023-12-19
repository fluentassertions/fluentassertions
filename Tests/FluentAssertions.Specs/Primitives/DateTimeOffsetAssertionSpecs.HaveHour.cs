using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class HaveHour
    {
        [Fact]
        public void When_asserting_subject_datetimeoffset_should_have_hour_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31, 23, 59, 00), TimeSpan.Zero);
            int expectation = 23;

            // Act
            Action act = () => subject.Should().HaveHour(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_should_have_hour_with_different_value_it_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31, 23, 59, 00), TimeSpan.Zero);
            int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveHour(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the hour part of subject to be 22, but it was 23.");
        }

        [Fact]
        public void When_asserting_subject_null_datetimeoffset_should_have_hour_should_throw()
        {
            // Arrange
            DateTimeOffset? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveHour(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the hour part of subject to be 22, but found a <null> DateTimeOffset.");
        }
    }

    public class NotHaveHour
    {
        [Fact]
        public void When_asserting_subject_datetimeoffset_should_not_have_hour_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31, 23, 59, 00), TimeSpan.Zero);
            int expectation = 23;

            // Act
            Action act = () => subject.Should().NotHaveHour(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the hour part of subject to be 23, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_should_not_have_hour_with_different_value_it_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31, 23, 59, 00), TimeSpan.Zero);
            int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveHour(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetimeoffset_should_not_have_hour_should_throw()
        {
            // Arrange
            DateTimeOffset? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveHour(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the hour part of subject to be 22, but found a <null> DateTimeOffset.");
        }
    }
}
