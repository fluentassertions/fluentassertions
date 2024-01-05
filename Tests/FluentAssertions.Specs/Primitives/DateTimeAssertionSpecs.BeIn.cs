using System;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

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

    public class NotBeIn
    {
        [Fact]
        public void Date_is_not_in_kind()
        {
            // Arrange
            DateTime subject = 5.January(2024).AsLocal();

            // Act / Assert
            subject.Should().NotBeIn(DateTimeKind.Utc);
        }

        [Fact]
        public void Date_is_in_kind_but_should_not()
        {
            // Arrange
            DateTime subject = 5.January(2024).AsLocal();

            // Act
            Action act = () => subject.Should().NotBeIn(DateTimeKind.Local);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect subject to be in Local, but it was.");
        }

        [Fact]
        public void Date_is_null_on_kind_check()
        {
            // Arrange
            DateTime? subject = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                subject.Should().NotBeIn(DateTimeKind.Utc);
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not*<null>*");
        }
    }
}
