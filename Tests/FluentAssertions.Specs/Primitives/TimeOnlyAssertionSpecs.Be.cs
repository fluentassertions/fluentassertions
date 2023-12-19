#if NET6_0_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class TimeOnlyAssertionSpecs
{
    public class Be
    {
        [Fact]
        public void Should_succeed_when_asserting_timeonly_value_is_equal_to_the_same_value()
        {
            // Arrange
            TimeOnly timeOnly = new(15, 06, 04, 146);
            TimeOnly sameTimeOnly = new(15, 06, 04, 146);

            // Act/Assert
            timeOnly.Should().Be(sameTimeOnly);
        }

        [Fact]
        public void When_timeonly_value_is_equal_to_the_same_nullable_value_be_should_succeed()
        {
            // Arrange
            TimeOnly timeOnly = new(15, 06, 04, 146);
            TimeOnly? sameTimeOnly = new(15, 06, 04, 146);

            // Act/Assert
            timeOnly.Should().Be(sameTimeOnly);
        }

        [Fact]
        public void When_both_values_are_at_their_minimum_then_it_should_succeed()
        {
            // Arrange
            TimeOnly timeOnly = TimeOnly.MinValue;
            TimeOnly sameTimeOnly = TimeOnly.MinValue;

            // Act/Assert
            timeOnly.Should().Be(sameTimeOnly);
        }

        [Fact]
        public void When_both_values_are_at_their_maximum_then_it_should_succeed()
        {
            // Arrange
            TimeOnly timeOnly = TimeOnly.MaxValue;
            TimeOnly sameTimeOnly = TimeOnly.MaxValue;

            // Act/Assert
            timeOnly.Should().Be(sameTimeOnly);
        }

        [Fact]
        public void Should_fail_when_asserting_timeonly_value_is_equal_to_the_different_value()
        {
            // Arrange
            var timeOnly = new TimeOnly(15, 03, 10);
            var otherTimeOnly = new TimeOnly(15, 03, 11);

            // Act
            Action act = () => timeOnly.Should().Be(otherTimeOnly, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected timeOnly to be <15:03:11.000>*failure message, but found <15:03:10.000>.");
        }

        [Fact]
        public void Should_fail_when_asserting_timeonly_value_is_equal_to_the_different_value_by_milliseconds()
        {
            // Arrange
            var timeOnly = new TimeOnly(15, 03, 10, 556);
            var otherTimeOnly = new TimeOnly(15, 03, 10, 175);

            // Act
            Action act = () => timeOnly.Should().Be(otherTimeOnly, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected timeOnly to be <15:03:10.175>*failure message, but found <15:03:10.556>.");
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_equals_the_same_value()
        {
            // Arrange
            TimeOnly? nullableTimeOnlyA = new TimeOnly(15, 06, 04, 123);
            TimeOnly? nullableTimeOnlyB = new TimeOnly(15, 06, 04, 123);

            // Act/Assert
            nullableTimeOnlyA.Should().Be(nullableTimeOnlyB);
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_null_value_equals_null()
        {
            // Arrange
            TimeOnly? nullableTimeOnlyA = null;
            TimeOnly? nullableTimeOnlyB = null;

            // Act/Assert
            nullableTimeOnlyA.Should().Be(nullableTimeOnlyB);
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            // Arrange
            TimeOnly? nullableTimeOnlyA = new TimeOnly(15, 06, 04);
            TimeOnly? nullableTimeOnlyB = new TimeOnly(15, 06, 06);

            // Act
            Action action = () =>
                nullableTimeOnlyA.Should().Be(nullableTimeOnlyB);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_timeonly_null_value_is_equal_to_another_value()
        {
            // Arrange
            TimeOnly? nullableTimeOnly = null;

            // Act
            Action action = () =>
                nullableTimeOnly.Should().Be(new TimeOnly(15, 06, 04), "because we want to test the failure {0}",
                    "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected nullableTimeOnly to be <15:06:04.000> because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void Should_succeed_when_asserting_timeonly_value_is_not_equal_to_a_different_value()
        {
            // Arrange
            TimeOnly timeOnly = new(15, 06, 04);
            TimeOnly otherTimeOnly = new(15, 06, 05);

            // Act/Assert
            timeOnly.Should().NotBe(otherTimeOnly);
        }
    }

    public class NotBe
    {
        [Fact]
        public void Different_timeonly_values_are_valid()
        {
            // Arrange
            TimeOnly time = new(19, 06, 04);
            TimeOnly otherTime = new(20, 06, 05);

            // Act & Assert
            time.Should().NotBe(otherTime);
        }

        [Fact]
        public void Different_timeonly_values_with_different_nullability_are_valid()
        {
            // Arrange
            TimeOnly time = new(19, 06, 04);
            TimeOnly? otherTime = new(19, 07, 05);

            // Act & Assert
            time.Should().NotBe(otherTime);
        }

        [Fact]
        public void Same_timeonly_values_are_invalid()
        {
            // Arrange
            TimeOnly time = new(19, 06, 04);
            TimeOnly sameTime = new(19, 06, 04);

            // Act
            Action act =
                () => time.Should().NotBe(sameTime, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected time not to be <19:06:04.000> because we want to test the failure message, but it is.");
        }

        [Fact]
        public void Same_timeonly_values_with_different_nullability_are_invalid()
        {
            // Arrange
            TimeOnly time = new(19, 06, 04);
            TimeOnly? sameTime = new(19, 06, 04);

            // Act
            Action act =
                () => time.Should().NotBe(sameTime, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected time not to be <19:06:04.000> because we want to test the failure message, but it is.");
        }
    }
}

#endif
