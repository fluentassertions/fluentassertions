using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NullableNumericAssertionSpecs
{
    public class BeNull
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_without_a_value_to_be_null()
        {
            // Arrange
            int? nullableInteger = null;

            // Act / Assert
            nullableInteger.Should().BeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_with_a_value_to_be_null()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act
            Action act = () => nullableInteger.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_with_a_value_to_be_null()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act
            Action act = () => nullableInteger.Should().BeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found 1.");
        }
    }

    public class NotBeNull
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_with_value_to_not_be_null()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act / Assert
            nullableInteger.Should().NotBeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_without_a_value_to_not_be_null()
        {
            // Arrange
            int? nullableInteger = null;

            // Act
            Action act = () => nullableInteger.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_without_a_value_to_not_be_null()
        {
            // Arrange
            int? nullableInteger = null;

            // Act
            Action act = () => nullableInteger.Should().NotBeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }
    }
}
