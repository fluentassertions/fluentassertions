using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class HaveYear
    {
        [Fact]
        public void When_asserting_subject_datetimeoffset_should_have_year_with_the_same_value_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 06, 04), TimeSpan.Zero);
            int expectation = 2009;

            // Act
            Action act = () => subject.Should().HaveYear(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_should_have_year_with_a_different_value_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 06, 04), TimeSpan.Zero);
            int expectation = 2008;

            // Act
            Action act = () => subject.Should().HaveYear(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the year part of subject to be 2008, but it was 2009.");
        }

        [Fact]
        public void When_asserting_subject_null_datetimeoffset_should_have_year_should_throw()
        {
            // Arrange
            DateTimeOffset? subject = null;
            int expectation = 2008;

            // Act
            Action act = () => subject.Should().HaveYear(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the year part of subject to be 2008, but found a <null> DateTimeOffset.");
        }
    }

    public class NotHaveYear
    {
        [Fact]
        public void When_asserting_subject_datetimeoffset_should_not_have_year_with_the_same_value_should_throw()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 06, 04), TimeSpan.Zero);
            int expectation = 2009;

            // Act
            Action act = () => subject.Should().NotHaveYear(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the year part of subject to be 2009, but it was.");
        }

        [Fact]
        public void When_asserting_subject_datetimeoffset_should_not_have_year_with_a_different_value_should_succeed()
        {
            // Arrange
            DateTimeOffset subject = new(new DateTime(2009, 06, 04), TimeSpan.Zero);
            int expectation = 2008;

            // Act
            Action act = () => subject.Should().NotHaveYear(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_null_datetimeoffset_should_not_have_year_should_throw()
        {
            // Arrange
            DateTimeOffset? subject = null;
            int expectation = 2008;

            // Act
            Action act = () => subject.Should().NotHaveYear(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the year part of subject to be 2008, but found a <null> DateTimeOffset.");
        }
    }
}
