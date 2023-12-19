#if NET6_0_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class TimeOnlyAssertionSpecs
{
    public class HaveSeconds
    {
        [Fact]
        public void When_asserting_subject_timeonly_should_have_seconds_with_the_same_value_it_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(14, 12, 31);
            const int expectation = 31;

            // Act/Assert
            subject.Should().HaveSeconds(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_seconds_with_the_same_value_it_should_throw()
        {
            // Arrange
            TimeOnly subject = new(14, 12, 31);
            const int expectation = 31;

            // Act
            Action act = () => subject.Should().NotHaveSeconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the seconds part of subject to be 31, but it was.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_have_seconds_with_a_different_value_it_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31);
            const int expectation = 30;

            // Act
            Action act = () => subject.Should().HaveSeconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the seconds part of subject to be 30, but found 31.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_seconds_with_a_different_value_it_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31);
            const int expectation = 30;

            // Act/Assert
            subject.Should().NotHaveSeconds(expectation);
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_have_seconds_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveSeconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the seconds part of subject to be 22, but found a <null> TimeOnly.");
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_not_have_seconds_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveSeconds(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the seconds part of subject to be 22, but found a <null> TimeOnly.");
        }
    }
}

#endif
