#if NET6_0_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class TimeOnlyAssertionSpecs
{
    public class BeAfter
    {
        [Fact]
        public void When_asserting_subject_timeonly_is_after_earlier_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04, 123);
            TimeOnly expectation = new(15, 06, 03, 45);

            // Act/Assert
            subject.Should().BeAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_after_earlier_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or before <15:06:03.000>, but found <15:06:04.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_after_later_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 05);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <15:06:05.000>, but found <15:06:04.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_after_later_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 05);

            // Act/Assert
            subject.Should().NotBeAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_after_the_same_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04, 145);
            TimeOnly expectation = new(15, 06, 04, 145);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <15:06:04.145>, but found <15:06:04.145>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_after_the_same_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04, 123);
            TimeOnly expectation = new(15, 06, 04, 123);

            // Act/Assert
            subject.Should().NotBeAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_on_or_after_earlier_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 07);
            TimeOnly expectation = new(15, 06);

            // Act/Assert
            subject.Should().BeOnOrAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_on_or_after_earlier_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <15:06:03.000>, but found <15:06:04.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_on_or_after_the_same_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 04);

            // Act/Assert
            subject.Should().BeOnOrAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_on_or_after_the_same_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06);
            TimeOnly expectation = new(15, 06);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <15:06:00.000>, but found <15:06:00.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_on_or_after_later_expected_timeonly_should_throw()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 05);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or after <15:06:05.000>, but found <15:06:04.000>.");
        }

        [Fact]
        public void When_asserting_subject_timeonly_is_not_on_or_after_later_expected_timeonly_should_succeed()
        {
            // Arrange
            TimeOnly subject = new(15, 06, 04);
            TimeOnly expectation = new(15, 06, 05);

            // Act/Assert
            subject.Should().NotBeOnOrAfter(expectation);
        }
    }
}

#endif
