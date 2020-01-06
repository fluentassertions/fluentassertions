using System;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class NullableSimpleTimeSpanAssertionSpecs
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_TimeSpan_value_with_a_value_to_have_a_value()
        {
            // Arrange
            TimeSpan? nullableTimeSpan = new TimeSpan();

            // Act / Assert
            nullableTimeSpan.Should().HaveValue();
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_TimeSpan_value_with_a_value_to_not_be_null()
        {
            // Arrange
            TimeSpan? nullableTimeSpan = new TimeSpan();

            // Act / Assert
            nullableTimeSpan.Should().NotBeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_TimeSpan_value_without_a_value_to_not_be_null()
        {
            // Arrange
            TimeSpan? nullableTimeSpan = null;

            // Act
            Action act = () => nullableTimeSpan.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_TimeSpan_value_without_a_value_to_have_a_value()
        {
            // Arrange
            TimeSpan? nullableTimeSpan = null;

            // Act
            Action act = () => nullableTimeSpan.Should().HaveValue("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_TimeSpan_value_without_a_value_to_not_be_null()
        {
            // Arrange
            TimeSpan? nullableTimeSpan = null;

            // Act
            Action act = () => nullableTimeSpan.Should().NotBeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_TimeSpan_value_without_a_value_to_not_have_a_value()
        {
            // Arrange
            TimeSpan? nullableTimeSpan = null;

            // Act / Assert
            nullableTimeSpan.Should().NotHaveValue();
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_TimeSpan_value_without_a_value_to_be_null()
        {
            // Arrange
            TimeSpan? nullableTimeSpan = null;

            // Act / Assert
            nullableTimeSpan.Should().BeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_TimeSpan_value_with_a_value_to_not_have_a_value()
        {
            // Arrange
            TimeSpan? nullableTimeSpan = new TimeSpan();

            // Act
            Action act = () => nullableTimeSpan.Should().NotHaveValue();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_TimeSpan_value_with_a_value_to_be_null()
        {
            // Arrange
            TimeSpan? nullableTimeSpan = new TimeSpan();

            // Act
            Action act = () => nullableTimeSpan.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_TimeSpan_value_with_a_value_to_not_have_a_value()
        {
            // Arrange
            TimeSpan? nullableTimeSpan = 1.Seconds();

            // Act
            Action act = () => nullableTimeSpan.Should().NotHaveValue("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found 1s.");
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_TimeSpan_value_with_a_value_to_be_null()
        {
            // Arrange
            TimeSpan? nullableTimeSpan = 1.Seconds();

            // Act
            Action act = () => nullableTimeSpan.Should().BeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found 1s.");
        }

        [Fact]
        public void When_asserting_a_nullable_TimeSpan_is_equal_to_a_different_nullable_TimeSpan_it_should_should_throw_appropriately()
        {
            // Arrange
            TimeSpan? nullableTimeSpanA = 1.Seconds();
            TimeSpan? nullableTimeSpanB = 2.Seconds();

            // Act
            Action action = () => nullableTimeSpanA.Should().Be(nullableTimeSpanB);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_nullable_TimeSpan_is_equal_to_another_a_nullable_TimeSpan_but_it_is_null_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            TimeSpan? nullableTimeSpanA = null;
            TimeSpan? nullableTimeSpanB = 1.Seconds();

            // Act
            Action action =
                () =>
                    nullableTimeSpanA.Should()
                        .Be(nullableTimeSpanB, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected 1s because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void When_asserting_a_nullable_TimeSpan_is_equal_to_another_a_nullable_TimeSpan_and_both_are_null_it_should_succeed()
        {
            // Arrange
            TimeSpan? nullableTimeSpanA = null;
            TimeSpan? nullableTimeSpanB = null;

            // Act
            Action action = () => nullableTimeSpanA.Should().Be(nullableTimeSpanB);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nullable_TimeSpan_is_equal_to_the_same_nullable_TimeSpan_it_should_succeed()
        {
            // Arrange
            TimeSpan? nullableTimeSpanA = new TimeSpan();
            TimeSpan? nullableTimeSpanB = new TimeSpan();

            // Act
            Action action = () => nullableTimeSpanA.Should().Be(nullableTimeSpanB);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            TimeSpan? nullableTimeSpan = new TimeSpan();

            // Act / Assert
            nullableTimeSpan.Should()
                .HaveValue()
                .And
                .Be(new TimeSpan());
        }
    }
}
