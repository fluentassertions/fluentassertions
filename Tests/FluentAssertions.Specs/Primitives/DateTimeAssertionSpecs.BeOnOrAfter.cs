using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class BeOnOrAfter
    {
        [Fact]
        public void When_asserting_subject_datetime_is_on_or_after_earlier_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 03);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_on_or_after_the_same_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_on_or_after_later_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 05);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or after <2016-06-05>, but found <2016-06-04>.");
        }
    }

    public class NotBeOnOrAfter
    {
        [Fact]
        public void When_asserting_subject_datetime_is_not_on_or_after_earlier_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_on_or_after_the_same_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_on_or_after_later_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 05);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }
    }
}
