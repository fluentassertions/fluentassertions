using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class HaveMilliseconds
    {
        [Fact]
        public void Same_milliseconds_value_succeeds()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, 999);
            int expectation = 999;

            // Act
            Action act = () => subject.Should().HaveMilliseconds(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Different_milliseconds_value_throws()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, 999);
            int expectation = 1;

            // Act
            Action act = () => subject.Should().HaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 1, but found 999.");
        }

        [Fact]
        public void Null_datetime_throws()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 22, but found a <null> DateTime.");
        }
    }

    public class NotHaveMilliseconds
    {
        [Fact]
        public void Same_milliseconds_value_throws()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, 999);
            int expectation = 999;

            // Act
            Action act = () => subject.Should().NotHaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 999, but it was.");
        }

        [Fact]
        public void Different_milliseconds_value_succeeds()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, 999);
            int expectation = 1;

            // Act
            Action act = () => subject.Should().NotHaveMilliseconds(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Null_datetime_throws()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 22, but found a <null> DateTime.");
        }
    }
}
