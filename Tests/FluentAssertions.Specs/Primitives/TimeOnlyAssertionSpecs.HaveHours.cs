#if NET6_0_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class TimeOnlyAssertionSpecs
{
    public class HaveHours
    {
        [Fact]
        public void When_asserting_subject_timeonly_should_have_hours_with_the_same_value_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31);
            const int expectation = 15;

            // Act/Assert
            subject.Should().HaveHours(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_hours_with_the_same_value_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31);
            const int expectation = 15;

            // Act
            Action act = () => subject.Should().NotHaveHours(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the hours part of subject to be 15, but it was.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_have_hours_with_a_different_value_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 12, 31);
            const int expectation = 14;

            // Act
            Action act = () => subject.Should().HaveHours(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the hours part of subject to be 14, but found 15.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_should_not_have_hours_with_a_different_value_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(21, 12, 31);
            const int expectation = 23;

            // Act/Assert
            subject.Should().NotHaveHours(expectation);
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_have_hours_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 21;

            // Act
            Action act = () => subject.Should().HaveHours(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the hours part of subject to be 21, but found <null>.");
        }

        [Fact]
        public void When_asserting_subject_null_timeonly_should_not_have_hours_should_throw()
        {
            // Arrange
            TimeOnly? subject = null;
            const int expectation = 19;

            // Act
            Action act = () => subject.Should().NotHaveHours(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the hours part of subject to be 19, but found a <null> TimeOnly.");
        }
    }
}

#endif
