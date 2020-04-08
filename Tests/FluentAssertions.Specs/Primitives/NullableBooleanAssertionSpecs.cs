using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class NullableBooleanAssertionSpecs
    {
        [Fact]
        public void When_asserting_nullable_boolean_value_with_a_value_to_have_a_value_it_should_succeed()
        {
            // Arrange
            bool? nullableBoolean = true;

            // Act / Assert
            nullableBoolean.Should().HaveValue();
        }

        [Fact]
        public void When_asserting_nullable_boolean_value_with_a_value_to_not_be_null_it_should_succeed()
        {
            // Arrange
            bool? nullableBoolean = true;

            // Act / Assert
            nullableBoolean.Should().NotBeNull();
        }

        [Fact]
        public void When_asserting_nullable_boolean_value_without_a_value_to_have_a_value_it_should_fail()
        {
            // Arrange
            bool? nullableBoolean = null;

            // Act
            Action act = () => nullableBoolean.Should().HaveValue();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_nullable_boolean_value_without_a_value_to_not_be_null_it_should_fail()
        {
            // Arrange
            bool? nullableBoolean = null;

            // Act
            Action act = () => nullableBoolean.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_nullable_boolean_value_without_a_value_to_have_a_value_it_should_fail_with_descriptive_message()
        {
            // Arrange
            bool? nullableBoolean = null;

            // Act
            Action act = () => nullableBoolean.Should().HaveValue("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_nullable_boolean_value_without_a_value_to_not_be_null_it_should_fail_with_descriptive_message()
        {
            // Arrange
            bool? nullableBoolean = null;

            // Act
            Action act = () => nullableBoolean.Should().NotBeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_nullable_boolean_value_without_a_value_to_not_have_a_value_it_should_succeed()
        {
            // Arrange
            bool? nullableBoolean = null;

            // Act / Assert
            nullableBoolean.Should().NotHaveValue();
        }

        [Fact]
        public void When_asserting_nullable_boolean_value_without_a_value_to_be_null_it_should_succeed()
        {
            // Arrange
            bool? nullableBoolean = null;

            // Act / Assert
            nullableBoolean.Should().BeNull();
        }

        [Fact]
        public void When_asserting_nullable_boolean_value_with_a_value_to_not_have_a_value_it_should_fail()
        {
            // Arrange
            bool? nullableBoolean = true;

            // Act
            Action act = () => nullableBoolean.Should().NotHaveValue();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_nullable_boolean_value_with_a_value_to_be_null_it_should_fail()
        {
            // Arrange
            bool? nullableBoolean = true;

            // Act
            Action act = () => nullableBoolean.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_nullable_boolean_value_with_a_value_to_be_not_have_a_value_it_should_fail_with_descriptive_message()
        {
            // Arrange
            bool? nullableBoolean = true;

            // Act
            Action act = () => nullableBoolean.Should().NotHaveValue("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found True.");
        }

        [Fact]
        public void When_asserting_nullable_boolean_value_with_a_value_to_be_null_it_should_fail_with_descriptive_message()
        {
            // Arrange
            bool? nullableBoolean = true;

            // Act
            Action act = () => nullableBoolean.Should().BeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found True.");
        }

        [Fact]
        public void When_asserting_boolean_null_value_is_false_it_should_fail()
        {
            // Arrange
            bool? nullableBoolean = null;

            // Act
            Action action = () =>
                nullableBoolean.Should().BeFalse("we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected nullableBoolean to be false because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void When_asserting_boolean_null_value_is_true_it_sShould_fail()
        {
            // Arrange
            bool? nullableBoolean = null;

            // Act
            Action action = () =>
                nullableBoolean.Should().BeTrue("we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected nullableBoolean to be true because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void When_asserting_boolean_null_value_to_be_equal_to_different_nullable_boolean_should_fail()
        {
            // Arrange
            bool? nullableBoolean = null;
            bool? differentNullableBoolean = false;

            // Act
            Action action = () =>
                nullableBoolean.Should().Be(differentNullableBoolean, "we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected False because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void When_asserting_boolean_null_value_to_be_equal_to_null_it_should_succeed()
        {
            // Arrange
            bool? nullableBoolean = null;
            bool? otherNullableBoolean = null;

            // Act
            Action action = () =>
                nullableBoolean.Should().Be(otherNullableBoolean);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_true_is_not_false_it_should_succeed()
        {
            // Arrange
            bool? trueBoolean = true;

            // Act
            Action action = () =>
                trueBoolean.Should().NotBeFalse();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_null_is_not_false_it_should_succeed()
        {
            // Arrange
            bool? nullValue = null;

            // Act
            Action action = () =>
                nullValue.Should().NotBeFalse();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_false_is_not_false_it_should_fail_with_descriptive_message()
        {
            // Arrange
            bool? falseBoolean = false;

            // Act
            Action action = () =>
                falseBoolean.Should().NotBeFalse("we want to test the failure message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*falseBoolean*not*False*because we want to test the failure message, but found False.");
        }

        [Fact]
        public void When_asserting_false_is_not_true_it_should_succeed()
        {
            // Arrange
            bool? trueBoolean = false;

            // Act
            Action action = () =>
                trueBoolean.Should().NotBeTrue();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_null_is_not_true_it_should_succeed()
        {
            // Arrange
            bool? nullValue = null;

            // Act
            Action action = () =>
                nullValue.Should().NotBeTrue();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_true_is_not_true_it_should_fail_with_descriptive_message()
        {
            // Arrange
            bool? falseBoolean = true;

            // Act
            Action action = () =>
                falseBoolean.Should().NotBeTrue("we want to test the failure message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*falseBoolean*not*True*because we want to test the failure message, but found True.");
        }

        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            bool? nullableBoolean = true;

            // Act / Assert
            nullableBoolean.Should()
                .HaveValue()
                .And
                .BeTrue();
        }
    }
}
