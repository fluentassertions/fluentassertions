using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class HaveMillisecond
    {
        [Fact]
        public void Milliseconds_are_asserted_when_values_match()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, 123);

            // Act / Assert
            subject.Should().HaveMillisecond(123);
        }

        [Fact]
        public void Should_fail_when_asserting_different_millisecond_value()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, 123);

            // Act
            Action act = () => subject.Should().HaveMillisecond(124);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 124, but found 123.");
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_null_datetime()
        {
            // Arrange
            DateTime? subject = null;

            // Act
            Action act = () => subject.Should().HaveMillisecond(22);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 22, but found a <null> DateTime.");
        }
    }

    public class NotHaveMillisecond
    {
        [Fact]
        public void Should_fail_when_asserting_same_millisecond_value()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, 123);

            // Act
            Action act = () => subject.Should().NotHaveMillisecond(123);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 123, but it was.");
        }

        [Fact]
        public void Milliseconds_are_not_asserted_when_values_differ()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, 123);

            // Act / Assert
            subject.Should().NotHaveMillisecond(124);
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_null_datetime()
        {
            // Arrange
            DateTime? subject = null;

            // Act
            Action act = () => subject.Should().NotHaveMillisecond(22);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 22, but found a <null> DateTime.");
        }
    }
}
