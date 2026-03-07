using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class HaveMillisecond
    {
        [Fact]
        public void Succeeds_for_datetimeoffset_with_the_same_milliseconds()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31, 23, 59, 00, 123), TimeSpan.Zero);

            // Act / Assert
            subject.Should().HaveMillisecond(123);
        }

        [Fact]
        public void Fails_for_datetimeoffset_with_different_milliseconds()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31, 23, 59, 00, 123), TimeSpan.Zero);

            // Act
            Action act = () => subject.Should().HaveMillisecond(124);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 124, but it was 123.");
        }

        [Fact]
        public void Fails_for_null_datetimeoffset()
        {
            // Arrange
            DateTimeOffset? subject = null;

            // Act
            Action act = () => subject.Should().HaveMillisecond(22);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 22, but found a <null> DateTimeOffset.");
        }
    }

    public class NotHaveMillisecond
    {
        [Fact]
        public void Fails_for_datetimeoffset_with_the_same_milliseconds()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31, 23, 59, 00, 123), TimeSpan.Zero);

            // Act
            Action act = () => subject.Should().NotHaveMillisecond(123);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 123, but it was.");
        }

        [Fact]
        public void Succeeds_for_datetimeoffset_with_different_milliseconds()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31, 23, 59, 00, 123), TimeSpan.Zero);

            // Act / Assert
            subject.Should().NotHaveMillisecond(124);
        }

        [Fact]
        public void Fails_for_null_datetimeoffset()
        {
            // Arrange
            DateTimeOffset? subject = null;

            // Act
            Action act = () => subject.Should().NotHaveMillisecond(22);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 22, but found a <null> DateTimeOffset.");
        }
    }
}
