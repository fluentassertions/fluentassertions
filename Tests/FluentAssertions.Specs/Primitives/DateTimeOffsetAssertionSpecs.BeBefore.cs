using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class BeBefore
    {
        [Fact]
        public void When_asserting_a_point_of_time_is_before_a_later_point_it_should_succeed()
        {
            // Arrange
            DateTimeOffset earlierDate = new(new DateTime(2016, 06, 04), TimeSpan.Zero);
            DateTimeOffset laterDate = new(new DateTime(2016, 06, 04, 0, 5, 0), TimeSpan.Zero);

            // Act
            Action act = () => earlierDate.Should().BeBefore(laterDate);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_is_before_earlier_expected_datetimeoffset_it_should_throw()
        {
            // Arrange
            DateTimeOffset expected = new(new DateTime(2016, 06, 03), TimeSpan.Zero);
            DateTimeOffset subject = new(new DateTime(2016, 06, 04), TimeSpan.Zero);

            // Act
            Action act = () => subject.Should().BeBefore(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-03 +0h>, but it was <2016-06-04 +0h>.");
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_is_before_the_same_datetimeoffset_it_should_throw()
        {
            // Arrange
            DateTimeOffset expected = new(new DateTime(2016, 06, 04), TimeSpan.Zero);
            DateTimeOffset subject = new(new DateTime(2016, 06, 04), TimeSpan.Zero);

            // Act
            Action act = () => subject.Should().BeBefore(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-04 +0h>, but it was <2016-06-04 +0h>.");
        }
    }

    public class NotBeBefore
    {
        [Fact]
        public void When_asserting_a_point_of_time_is_not_before_another_it_should_throw()
        {
            // Arrange
            DateTimeOffset earlierDate = new(new DateTime(2016, 06, 04), TimeSpan.Zero);
            DateTimeOffset laterDate = new(new DateTime(2016, 06, 04, 0, 5, 0), TimeSpan.Zero);

            // Act
            Action act = () => earlierDate.Should().NotBeBefore(laterDate);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected earlierDate to be on or after <2016-06-04 00:05:00 +0h>, but it was <2016-06-04 +0h>.");
        }

        [Fact]
        public void When_asserting_subject_is_not_before_earlier_expected_datetimeoffset_it_should_succeed()
        {
            // Arrange
            DateTimeOffset expected = new(new DateTime(2016, 06, 03), TimeSpan.Zero);
            DateTimeOffset subject = new(new DateTime(2016, 06, 04), TimeSpan.Zero);

            // Act
            Action act = () => subject.Should().NotBeBefore(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_is_not_before_the_same_datetimeoffset_it_should_succeed()
        {
            // Arrange
            DateTimeOffset expected = new(new DateTime(2016, 06, 04), TimeSpan.Zero);
            DateTimeOffset subject = new(new DateTime(2016, 06, 04), TimeSpan.Zero);

            // Act
            Action act = () => subject.Should().NotBeBefore(expected);

            // Assert
            act.Should().NotThrow();
        }
    }
}
