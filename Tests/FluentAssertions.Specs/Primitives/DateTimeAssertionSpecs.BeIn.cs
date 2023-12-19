using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class BeIn
    {
        [Fact]
        public void When_asserting_subject_datetime_represents_its_own_kind_it_should_succeed()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, DateTimeKind.Local);

            // Act
            Action act = () => subject.Should().BeIn(DateTimeKind.Local);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_represents_a_different_kind_it_should_throw()
        {
            // Arrange
            DateTime subject = new(2009, 12, 31, 23, 59, 00, DateTimeKind.Local);

            // Act
            Action act = () => subject.Should().BeIn(DateTimeKind.Utc);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be in Utc, but found Local.");
        }

        [Fact]
        public void When_asserting_subject_null_datetime_represents_a_specific_kind_it_should_throw()
        {
            // Arrange
            DateTime? subject = null;

            // Act
            Action act = () => subject.Should().BeIn(DateTimeKind.Utc);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be in Utc, but found a <null> DateTime.");
        }
    }
}
