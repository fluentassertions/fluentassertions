#if NET6_0_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateOnlyAssertionSpecs
{
    public class BeBefore
    {
        [Fact]
        public void When_asserting_subject_is_not_before_earlier_expected_dateonly_it_should_succeed()
        {
            // Arrange
            DateOnly expected = new(2016, 06, 03);
            DateOnly subject = new(2016, 06, 04);

            // Act/Assert
            subject.Should().NotBeBefore(expected);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_before_the_same_dateonly_it_should_throw()
        {
            // Arrange
            DateOnly expected = new(2016, 06, 04);
            DateOnly subject = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeBefore(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_before_the_same_dateonly_it_should_succeed()
        {
            // Arrange
            DateOnly expected = new(2016, 06, 04);
            DateOnly subject = new(2016, 06, 04);

            // Act/Assert
            subject.Should().NotBeBefore(expected);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_on_or_before_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 05);

            // Act/Assert
            subject.Should().BeOnOrBefore(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_on_or_before_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 05);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-05>, but found <2016-06-04>.");
        }

        [Fact]
        public void
            When_asserting_subject_dateonly_is_on_or_before_the_same_date_as_the_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 04);

            // Act/Assert
            subject.Should().BeOnOrBefore(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_on_or_before_the_same_date_as_the_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_on_or_before_earlier_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 03);

            // Act
            Action act = () => subject.Should().BeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_on_or_before_earlier_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 03);

            // Act/Assert
            subject.Should().NotBeOnOrBefore(expectation);
        }
    }
}

#endif
