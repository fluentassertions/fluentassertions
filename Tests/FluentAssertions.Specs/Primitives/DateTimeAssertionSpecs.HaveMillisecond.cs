using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class HaveMillisecond
    {
        [Fact]
        public void When_asserting_subject_datetime_should_have_milliseconds_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, 999);
            int expectation = 999;

            // Act
            Action act = () => subject.Should().HaveMillisecond(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_should_have_milliseconds_with_different_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, 999);
            int expectation = 1;

            // Act
            Action act = () => subject.Should().HaveMillisecond(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 1, but found 999.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_have_millisecond_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveMillisecond(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 22, but found a <null> DateTime.");
        }
    }

    public class NotHaveMillisecond
    {
        [Fact]
        public void When_asserting_subject_datetime_should_not_have_milliseconds_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, 999);
            int expectation = 999;

            // Act
            Action act = () => subject.Should().NotHaveMillisecond(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 999, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_milliseconds_with_different_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, 999);
            int expectation = 1;

            // Act
            Action act = () => subject.Should().NotHaveMillisecond(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_not_have_millisecond_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveMillisecond(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 22, but found a <null> DateTime.");
        }
    }
}
