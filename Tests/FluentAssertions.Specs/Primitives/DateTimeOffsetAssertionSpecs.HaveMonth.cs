using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class HaveMonth
    {
        [Fact]
        public void When_asserting_subject_datetimeoffset_should_have_month_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31), TimeSpan.Zero);
            int expectation = 12;

            // Act
            Action act = () => subject.Should().HaveMonth(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_should_have_a_month_with_a_different_value_it_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31), TimeSpan.Zero);
            int expectation = 11;

            // Act
            Action act = () => subject.Should().HaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the month part of subject to be 11, but it was 12.");
        }

        [Fact]
        public void When_asserting_subject_null_datetimeoffset_should_have_month_should_throw()
        {
            // Arrange
            DateTimeOffset? subject = null;
            int expectation = 12;

            // Act
            Action act = () => subject.Should().HaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the month part of subject to be 12, but found a <null> DateTimeOffset.");
        }
    }

    public class NotHaveMonth
    {
        [Fact]
        public void When_asserting_subject_datetimeoffset_should_not_have_month_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31), TimeSpan.Zero);
            int expectation = 12;

            // Act
            Action act = () => subject.Should().NotHaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the month part of subject to be 12, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_should_not_have_a_month_with_a_different_value_it_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 12, 31), TimeSpan.Zero);
            int expectation = 11;

            // Act
            Action act = () => subject.Should().NotHaveMonth(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetimeoffset_should_not_have_month_should_throw()
        {
            // Arrange
            DateTimeOffset? subject = null;
            int expectation = 12;

            // Act
            Action act = () => subject.Should().NotHaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the month part of subject to be 12, but found a <null> DateTimeOffset.");
        }
    }
}
