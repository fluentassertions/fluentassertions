#if NET6_0_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateOnlyAssertionSpecs
{
    public class Be
    {
        [Fact]
        public void Should_succeed_when_asserting_dateonly_value_is_equal_to_the_same_value()
        {
            // Arrange
            DateOnly dateOnly = new(2016, 06, 04);
            DateOnly sameDateOnly = new(2016, 06, 04);

            // Act/Assert
            dateOnly.Should().Be(sameDateOnly);
        }

        [Fact]
        public void When_dateonly_value_is_equal_to_the_same_nullable_value_be_should_succeed()
        {
            // Arrange
            DateOnly dateOnly = new(2016, 06, 04);
            DateOnly? sameDateOnly = new(2016, 06, 04);

            // Act/Assert
            dateOnly.Should().Be(sameDateOnly);
        }

        [Fact]
        public void When_both_values_are_at_their_minimum_then_it_should_succeed()
        {
            // Arrange
            DateOnly dateOnly = DateOnly.MinValue;
            DateOnly sameDateOnly = DateOnly.MinValue;

            // Act/Assert
            dateOnly.Should().Be(sameDateOnly);
        }

        [Fact]
        public void When_both_values_are_at_their_maximum_then_it_should_succeed()
        {
            // Arrange
            DateOnly dateOnly = DateOnly.MaxValue;
            DateOnly sameDateOnly = DateOnly.MaxValue;

            // Act/Assert
            dateOnly.Should().Be(sameDateOnly);
        }

        [Fact]
        public void Should_fail_when_asserting_dateonly_value_is_equal_to_the_different_value()
        {
            // Arrange
            var dateOnly = new DateOnly(2012, 03, 10);
            var otherDateOnly = new DateOnly(2012, 03, 11);

            // Act
            Action act = () => dateOnly.Should().Be(otherDateOnly, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected dateOnly to be <2012-03-11>*failure message, but found <2012-03-10>.");
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_equals_the_same_value()
        {
            // Arrange
            DateOnly? nullableDateOnlyA = new DateOnly(2016, 06, 04);
            DateOnly? nullableDateOnlyB = new DateOnly(2016, 06, 04);

            // Act/Assert
            nullableDateOnlyA.Should().Be(nullableDateOnlyB);
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_null_value_equals_null()
        {
            // Arrange
            DateOnly? nullableDateOnlyA = null;
            DateOnly? nullableDateOnlyB = null;

            // Act/Assert
            nullableDateOnlyA.Should().Be(nullableDateOnlyB);
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            // Arrange
            DateOnly? nullableDateOnlyA = new DateOnly(2016, 06, 04);
            DateOnly? nullableDateOnlyB = new DateOnly(2016, 06, 06);

            // Act
            Action action = () =>
                nullableDateOnlyA.Should().Be(nullableDateOnlyB);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_dateonly_null_value_is_equal_to_another_value()
        {
            // Arrange
            DateOnly? nullableDateOnly = null;

            // Act
            Action action = () =>
                nullableDateOnly.Should().Be(new DateOnly(2016, 06, 04), "because we want to test the failure {0}",
                    "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected nullableDateOnly to be <2016-06-04> because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void Should_succeed_when_asserting_dateonly_value_is_not_equal_to_a_different_value()
        {
            // Arrange
            DateOnly dateOnly = new(2016, 06, 04);
            DateOnly otherDateOnly = new(2016, 06, 05);

            // Act/Assert
            dateOnly.Should().NotBe(otherDateOnly);
        }
    }

    public class NotBe
    {
        [Fact]
        public void Different_dateonly_values_are_valid()
        {
            // Arrange
            DateOnly date = new(2020, 06, 04);
            DateOnly otherDate = new(2020, 06, 05);

            // Act & Assert
            date.Should().NotBe(otherDate);
        }

        [Fact]
        public void Different_dateonly_values_with_different_nullability_are_valid()
        {
            // Arrange
            DateOnly date = new(2020, 06, 04);
            DateOnly? otherDate = new(2020, 07, 05);

            // Act & Assert
            date.Should().NotBe(otherDate);
        }

        [Fact]
        public void Same_dateonly_values_are_invalid()
        {
            // Arrange
            DateOnly date = new(2020, 06, 04);
            DateOnly sameDate = new(2020, 06, 04);

            // Act
            Action act =
                () => date.Should().NotBe(sameDate, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected date not to be <2020-06-04> because we want to test the failure message, but it is.");
        }

        [Fact]
        public void Same_dateonly_values_with_different_nullability_are_invalid()
        {
            // Arrange
            DateOnly date = new(2020, 06, 04);
            DateOnly? sameDate = new(2020, 06, 04);

            // Act
            Action act =
                () => date.Should().NotBe(sameDate, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected date not to be <2020-06-04> because we want to test the failure message, but it is.");
        }
    }
}

#endif
