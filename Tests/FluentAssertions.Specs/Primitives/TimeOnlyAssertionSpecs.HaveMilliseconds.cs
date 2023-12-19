#if NET6_0_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class TimeOnlyAssertionSpecs
{
    public class HaveMilliseconds
    {
        [Fact]
        public void When_asserting_subject_timeonly_should_have_milliseconds_with_the_same_value_it_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(14, 12, 31, 123);
            const int expectation = 123;

            // Act/Assert
            subject.Should().HaveMilliseconds(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_milliseconds_with_the_same_value_it_should_throw()
        {
            // Arrange
            TimeOnly subject = new(14, 12, 31, 445);
            const int expectation = 445;

            // Act
            Action act = () => subject.Should().NotHaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 445, but it was.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_have_milliseconds_with_a_different_value_it_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31, 555);
            const int expectation = 12;

            // Act
            Action act = () => subject.Should().HaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 12, but found 555.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_milliseconds_with_a_different_value_it_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31, 445);
            const int expectation = 31;

            // Act/Assert
            subject.Should().NotHaveMilliseconds(expectation);
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_have_milliseconds_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the milliseconds part of subject to be 22, but found a <null> TimeOnly.");
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_not_have_milliseconds_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveMilliseconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the milliseconds part of subject to be 22, but found a <null> TimeOnly.");
        }
    }
}

#endif
