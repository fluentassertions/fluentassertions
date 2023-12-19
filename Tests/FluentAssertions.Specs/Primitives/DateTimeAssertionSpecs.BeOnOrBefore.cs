using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class BeOnOrBefore
    {
        [Fact]
        public void When_asserting_subject_datetime_is_on_or_before_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 05);

            // Act
            Action act = () => subject.Should().BeOnOrBefore(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_on_or_before_the_same_date_as_the_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeOnOrBefore(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_on_or_before_earlier_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 03);

            // Act
            Action act = () => subject.Should().BeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or before <2016-06-03>, but found <2016-06-04>.");
        }
    }

    public class NotBeOnOrBefore
    {
        [Fact]
        public void When_asserting_subject_datetime_is_on_or_before_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 05);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-05>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_on_or_before_the_same_date_as_the_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_on_or_before_earlier_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().NotThrow();
        }
    }
}
