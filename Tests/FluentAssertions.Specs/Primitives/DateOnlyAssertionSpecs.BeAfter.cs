#if NET6_0_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateOnlyAssertionSpecs
{
    public class BeAfter
    {
        [Fact]
        public void When_asserting_subject_dateonly_is_after_earlier_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 03);

            // Act/Assert
            subject.Should().BeAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_after_earlier_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_after_later_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 05);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-05>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_after_later_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 05);

            // Act/Assert
            subject.Should().NotBeAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_after_the_same_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_after_the_same_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 04);

            // Act/Assert
            subject.Should().NotBeAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_on_or_after_earlier_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 03);

            // Act/Assert
            subject.Should().BeOnOrAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_on_or_after_earlier_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_on_or_after_the_same_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 04);

            // Act/Assert
            subject.Should().BeOnOrAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_on_or_after_the_same_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_on_or_after_later_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 05);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or after <2016-06-05>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_on_or_after_later_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 05);

            // Act/Assert
            subject.Should().NotBeOnOrAfter(expectation);
        }
    }
}

#endif
