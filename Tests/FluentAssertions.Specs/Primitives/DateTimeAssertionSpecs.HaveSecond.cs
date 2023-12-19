using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class HaveSecond
    {
        [Fact]
        public void When_asserting_subject_datetime_should_have_seconds_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00);
            int expectation = 0;

            // Act
            Action act = () => subject.Should().HaveSecond(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_should_have_seconds_with_different_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00);
            int expectation = 1;

            // Act
            Action act = () => subject.Should().HaveSecond(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the seconds part of subject to be 1, but found 0.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_have_second_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveSecond(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the seconds part of subject to be 22, but found a <null> DateTime.");
        }
    }

    public class NotHaveSecond
    {
        [Fact]
        public void When_asserting_subject_datetime_should_not_have_seconds_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00);
            int expectation = 0;

            // Act
            Action act = () => subject.Should().NotHaveSecond(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the seconds part of subject to be 0, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetime_should_not_have_seconds_with_different_value_it_should_succeed()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00);
            int expectation = 1;

            // Act
            Action act = () => subject.Should().NotHaveSecond(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetime_should_not_have_second_should_throw()
        {
            // Arrange
            DateTime? subject = null;
            int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveSecond(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the seconds part of subject to be 22, but found a <null> DateTime.");
        }
    }
}
