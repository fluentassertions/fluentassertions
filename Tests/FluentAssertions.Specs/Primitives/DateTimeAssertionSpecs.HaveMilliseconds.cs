using System;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class HaveMilliseconds
    {
        [Fact]
        public void Same_value_succeeds()
        {
            // Arrange
            DateTime subject = 31.December(2009).At(23, 59, 59, 999);
            int expectation = 999;

            // Act & Assert
            subject.Should().HaveMilliseconds(expectation);
        }

        [Fact]
        public void Different_value_throws()
        {
            // Arrange
            DateTime subject = 31.December(2009).At(23, 59, 59, 999);
            int expectation = 1;

            // Act
            Action act = () => subject.Should().HaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 1, but found 999.");
        }

        [Fact]
        public void Different_value_throws_with_reason()
        {
            // Arrange
            DateTime subject = 31.December(2009).At(23, 59, 59, 999);
            int expectation = 1;

            // Act
            Action act = () => subject.Should().HaveMilliseconds(expectation, "because we want to test the failure case with {0} and {1}", expectation, subject.Millisecond);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 1 because we want to test the failure case with 1 and 999, but found 999.");
        }

        [Fact]
        public void Null_subject_throws()
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
        public void Same_value_throws()
        {
            // Arrange
            DateTime subject = 31.December(2009).At(23, 59, 59, 999);
            int expectation = 999;

            // Act
            Action act = () => subject.Should().NotHaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 999, but it was.");
        }

        [Fact]
        public void Same_value_throws_with_reason()
        {
            // Arrange
            DateTime subject = 31.December(2009).At(23, 59, 59, 999);
            int expectation = 999;

            // Act
            Action act = () => subject.Should().NotHaveMilliseconds(expectation, "because we want to test the failure case with {0} and {1}", expectation, subject.Millisecond);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 999 because we want to test the failure case with 999 and 999, but it was.");
        }

        [Fact]
        public void Different_value_succeeds()
        {
            // Arrange
            DateTime subject = 31.December(2009).At(23, 59, 59, 999);
            int expectation = 1;

            // Act & Assert
            subject.Should().NotHaveMilliseconds(expectation);
        }

        [Fact]
        public void Null_subject_throws()
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
