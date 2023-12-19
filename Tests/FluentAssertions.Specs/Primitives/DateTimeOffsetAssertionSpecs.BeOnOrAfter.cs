using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class BeOnOrAfter
    {
        [Fact]
        public void When_asserting_subject_datetimeoffset_is_on_or_after_earlier_expected_datetimeoffset_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2016, 06, 04), TimeSpan.Zero);
            DateTimeOffset expectation = new(new DateTime(2016, 06, 03), TimeSpan.Zero);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_is_on_or_after_the_same_expected_datetimeoffset_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2016, 06, 04), TimeSpan.Zero);
            DateTimeOffset expectation = new(new DateTime(2016, 06, 04), TimeSpan.Zero);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_is_on_or_after_later_expected_datetimeoffset_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2016, 06, 04), TimeSpan.Zero);
            DateTimeOffset expectation = new(new DateTime(2016, 06, 05), TimeSpan.Zero);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or after <2016-06-05 +0h>, but it was <2016-06-04 +0h>.");
        }
    }

    public class NotBeOnOrAfter
    {
        [Fact]
        public void When_asserting_subject_datetimeoffset_is_not_on_or_after_earlier_expected_datetimeoffset_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2016, 06, 04), TimeSpan.Zero);
            DateTimeOffset expectation = new(new DateTime(2016, 06, 03), TimeSpan.Zero);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-03 +0h>, but it was <2016-06-04 +0h>.");
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_is_not_on_or_after_the_same_expected_datetimeoffset_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2016, 06, 04), TimeSpan.Zero);
            DateTimeOffset expectation = new(new DateTime(2016, 06, 04), TimeSpan.Zero);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-04 +0h>, but it was <2016-06-04 +0h>.");
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_is_not_on_or_after_later_expected_datetimeoffset_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2016, 06, 04), TimeSpan.Zero);
            DateTimeOffset expectation = new(new DateTime(2016, 06, 05), TimeSpan.Zero);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }
    }
}
