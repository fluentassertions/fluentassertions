#if NET6_0_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class TimeOnlyAssertionSpecs
{
    public class BeBefore
    {
        [Fact]
        public void When_asserting_subject_is_not_before_earlier_expected_timeonly_it_should_succeed()
        {
            // Arrange
            TimeOnly expected = new(15, 06, 03);
            TimeOnly subject = new(15, 06, 04);

            // Act/Assert
            subject.Should().NotBeBefore(expected);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_before_the_same_timeonly_it_should_throw()
        {
            // Arrange
            TimeOnly expected = new(15, 06, 04);
            TimeOnly subject = new(15, 06, 04);

            // Act
            Action act = () => subject.Should().BeBefore(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <15:06:04.000>, but found <15:06:04.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_before_the_same_timeonly_it_should_succeed()
        {
            // Arrange
            TimeOnly expected = new(15, 06, 04);
            TimeOnly subject = new(15, 06, 04);

            // Act/Assert
            subject.Should().NotBeBefore(expected);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_on_or_before_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04, 175);
            TimeOnly expectation = new(15, 06, 05, 23);

            // Act/Assert
            subject.Should().BeOnOrBefore(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_on_or_before_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04, 150);
            TimeOnly expectation = new(15, 06, 05, 340);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <15:06:05.340>, but found <15:06:04.150>.");
        }

        [Fact]
        public void
            When_asserting_subject_timeonly_is_on_or_before_the_same_time_as_the_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 04);

            // Act/Assert
            subject.Should().BeOnOrBefore(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_on_or_before_the_same_time_as_the_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04, 123);
            TimeOnly expectation = new(15, 06, 04, 123);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <15:06:04.123>, but found <15:06:04.123>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_on_or_before_earlier_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 07);
            TimeOnly expectation = new(15, 06);

            // Act
            Action act = () => subject.Should().BeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or before <15:06:00.000>, but found <15:07:00.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_on_or_before_earlier_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 03);

            // Act/Assert
            subject.Should().NotBeOnOrBefore(expectation);
        }
    }
}

#endif
