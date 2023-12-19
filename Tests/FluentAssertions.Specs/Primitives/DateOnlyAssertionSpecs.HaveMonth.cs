#if NET6_0_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateOnlyAssertionSpecs
{
    public class HaveMonth
    {
        [Fact]
        public void When_asserting_subject_dateonly_should_have_month_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 12;

            // Act/Assert
            subject.Should().HaveMonth(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_should_not_have_month_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 12;

            // Act
            Action act = () => subject.Should().NotHaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the month part of subject to be 12, but it was.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_should_have_a_month_with_a_different_value_it_should_throw()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 11;

            // Act
            Action act = () => subject.Should().HaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the month part of subject to be 11, but found 12.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_should_not_have_a_month_with_a_different_value_it_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 11;

            // Act/Assert
            subject.Should().NotHaveMonth(expectation);
        }

        [Fact]
        public void When_asserting_subject_null_dateonly_should_have_month_should_throw()
        {
            // Arrange
            DateOnly? subject = null;
            const int expectation = 12;

            // Act
            Action act = () => subject.Should().HaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the month part of subject to be 12, but found a <null> DateOnly.");
        }

        [Fact]
        public void When_asserting_subject_null_dateonly_should_not_have_month_should_throw()
        {
            // Arrange
            DateOnly? subject = null;
            const int expectation = 12;

            // Act
            Action act = () => subject.Should().NotHaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the month part of subject to be 12, but found a <null> DateOnly.");
        }
    }
}

#endif
