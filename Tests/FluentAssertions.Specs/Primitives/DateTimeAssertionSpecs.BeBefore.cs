using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class BeBefore
    {
        [Fact]
        public void When_asserting_a_point_of_time_is_before_a_later_point_it_should_succeed()
        {
            // Arrange
            DateTime earlierDate = DateTime.SpecifyKind(new DateTime(2016, 06, 04), DateTimeKind.Unspecified);
            DateTime laterDate = DateTime.SpecifyKind(new DateTime(2016, 06, 04, 0, 5, 0), DateTimeKind.Utc);

            // Act
            Action act = () => earlierDate.Should().BeBefore(laterDate);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_is_before_earlier_expected_datetime_it_should_throw()
        {
            // Arrange
            DateTime expected = new(2016, 06, 03);
            DateTime subject = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeBefore(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_before_the_same_datetime_it_should_throw()
        {
            // Arrange
            DateTime expected = new(2016, 06, 04);
            DateTime subject = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeBefore(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-04>, but found <2016-06-04>.");
        }
    }

    public class NotBeBefore
    {
        [Fact]
        public void When_asserting_a_point_of_time_is_not_before_another_it_should_throw()
        {
            // Arrange
            DateTime earlierDate = DateTime.SpecifyKind(new DateTime(2016, 06, 04), DateTimeKind.Unspecified);
            DateTime laterDate = DateTime.SpecifyKind(new DateTime(2016, 06, 04, 0, 5, 0), DateTimeKind.Utc);

            // Act
            Action act = () => earlierDate.Should().NotBeBefore(laterDate);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected earlierDate to be on or after <2016-06-04 00:05:00>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_is_not_before_earlier_expected_datetime_it_should_succeed()
        {
            // Arrange
            DateTime expected = new(2016, 06, 03);
            DateTime subject = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeBefore(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_before_the_same_datetime_it_should_succeed()
        {
            // Arrange
            DateTime expected = new(2016, 06, 04);
            DateTime subject = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeBefore(expected);

            // Assert
            act.Should().NotThrow();
        }
    }
}
