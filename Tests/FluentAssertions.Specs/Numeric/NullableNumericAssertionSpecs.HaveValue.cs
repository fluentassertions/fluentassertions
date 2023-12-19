using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NullableNumericAssertionSpecs
{
    public class HaveValue
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_with_value_to_have_a_value()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act / Assert
            nullableInteger.Should().HaveValue();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_without_a_value_to_have_a_value()
        {
            // Arrange
            int? nullableInteger = null;

            // Act
            Action act = () => nullableInteger.Should().HaveValue();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_without_a_value_to_have_a_value()
        {
            // Arrange
            int? nullableInteger = null;

            // Act
            Action act = () => nullableInteger.Should().HaveValue("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }
    }

    public class NotHaveValue
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_without_a_value_to_not_have_a_value()
        {
            // Arrange
            int? nullableInteger = null;

            // Act / Assert
            nullableInteger.Should().NotHaveValue();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_with_a_value_to_not_have_a_value()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act
            Action act = () => nullableInteger.Should().NotHaveValue();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_nullable_value_with_unexpected_value_is_found_it_should_throw_with_message()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act
            Action action = () => nullableInteger.Should().NotHaveValue("it was {0} expected", "not");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Did not expect a value because it was not expected, but found 1.");
        }
    }
}
