using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class BooleanAssertionSpecs
    {
        [Fact]
        public void Should_succeed_when_asserting_boolean_value_true_is_true()
        {
            // Arrange
            bool boolean = true;

            // Act / Assert
            boolean.Should().BeTrue();
        }

        [Fact]
        public void Should_fail_when_asserting_boolean_value_false_is_true()
        {
            // Arrange
            bool boolean = false;

            // Act
            Action action = () => boolean.Should().BeTrue();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_false_is_true()
        {
            // Act
            Action action = () =>
                false.Should().BeTrue("because we want to test the failure {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Expected boolean to be true because we want to test the failure message, but found False.");
        }

        [Fact]
        public void Should_succeed_when_asserting_boolean_value_false_is_false()
        {
            // Act
            Action action = () =>
                false.Should().BeFalse();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_boolean_value_true_is_false()
        {
            // Act
            Action action = () =>
                true.Should().BeFalse();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_true_is_false()
        {
            // Act
            Action action = () =>
                true.Should().BeFalse("we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected boolean to be false because we want to test the failure message, but found True.");
        }

        [Fact]
        public void Should_succeed_when_asserting_boolean_value_to_be_equal_to_the_same_value()
        {
            // Act
            Action action = () =>
                false.Should().Be(false);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_boolean_value_to_be_equal_to_a_different_value()
        {
            // Act
            Action action = () =>
                false.Should().Be(true);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_to_be_equal_to_a_different_value()
        {
            // Act
            Action action = () =>
                false.Should().Be(true, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*Expected*boolean*True*because we want to test the failure message, but found False.*");
        }

        [Fact]
        public void Should_succeed_when_asserting_boolean_value_not_to_be_equal_to_the_same_value()
        {
            // Act
            Action action = () =>
                true.Should().NotBe(false);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_boolean_value_not_to_be_equal_to_a_different_value()
        {
            // Act
            Action action = () =>
                true.Should().NotBe(true);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_not_to_be_equal_to_a_different_value()
        {
            // Act
            Action action = () =>
                true.Should().NotBe(true, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*Expected*boolean*True*because we want to test the failure message, but found True.*");
        }
    }
}
