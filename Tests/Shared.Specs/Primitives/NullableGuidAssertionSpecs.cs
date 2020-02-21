using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class NullableGuidAssertionSpecs
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_guid_value_with_a_value_to_have_a_value()
        {
            // Arrange
            Guid? nullableGuid = Guid.NewGuid();

            // Act / Assert
            nullableGuid.Should().HaveValue();
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_guid_value_with_a_value_to_not_be_null()
        {
            // Arrange
            Guid? nullableGuid = Guid.NewGuid();

            // Act / Assert
            nullableGuid.Should().NotBeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_guid_value_without_a_value_to_have_a_value()
        {
            // Arrange
            Guid? nullableGuid = null;

            // Act
            Action act = () => nullableGuid.Should().HaveValue();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_guid_value_without_a_value_to_not_be_null()
        {
            // Arrange
            Guid? nullableGuid = null;

            // Act
            Action act = () => nullableGuid.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_guid_value_without_a_value_to_have_a_value()
        {
            // Arrange
            Guid? nullableGuid = null;

            // Act
            Action act = () => nullableGuid.Should().HaveValue("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_guid_value_without_a_value_to_not_be_null()
        {
            // Arrange
            Guid? nullableGuid = null;

            // Act
            Action act = () => nullableGuid.Should().NotBeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_guid_value_without_a_value_to_not_have_a_value()
        {
            // Arrange
            Guid? nullableGuid = null;

            // Act / Assert
            nullableGuid.Should().NotHaveValue();
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_guid_value_without_a_value_to_be_null()
        {
            // Arrange
            Guid? nullableGuid = null;

            // Act / Assert
            nullableGuid.Should().BeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_guid_value_with_a_value_to_not_have_a_value()
        {
            // Arrange
            Guid? nullableGuid = Guid.NewGuid();

            // Act
            Action act = () => nullableGuid.Should().NotHaveValue();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_guid_value_with_a_value_to_be_null()
        {
            // Arrange
            Guid? nullableGuid = Guid.NewGuid();

            // Act
            Action act = () => nullableGuid.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_when_guid_is_null_while_asserting_guid_equals_another_guid()
        {
            // Arrange
            Guid? guid = null;
            var someGuid = new Guid("55555555-ffff-eeee-dddd-444444444444");

            // Act
            Action act = () =>
                guid.Should().Be(someGuid, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected Guid to be {55555555-ffff-eeee-dddd-444444444444} because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_guid_null_equals_null()
        {
            // Arrange
            Guid? nullGuid = null;
            Guid? otherNullGuid = null;

            // Act
            Action act = () =>
                nullGuid.Should().Be(otherNullGuid);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_guid_value_with_a_value_to_not_have_a_value()
        {
            // Arrange
            Guid? nullableGuid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");

            // Act
            Action act = () => nullableGuid.Should().NotHaveValue("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found {11111111-aaaa-bbbb-cccc-999999999999}.");
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_guid_value_with_a_value_to_be_null()
        {
            // Arrange
            Guid? nullableGuid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");

            // Act
            Action act = () => nullableGuid.Should().BeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found {11111111-aaaa-bbbb-cccc-999999999999}.");
        }

        [Fact]
        public void Should_fail_when_asserting_null_equals_some_guid()
        {
            // Arrange
            Guid? nullableGuid = null;
            var someGuid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");

            // Act
            Action act = () =>
                nullableGuid.Should().Be(someGuid, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected nullableGuid to be {11111111-aaaa-bbbb-cccc-999999999999} because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            Guid? nullableGuid = Guid.NewGuid();

            // Act / Assert
            nullableGuid.Should()
                .HaveValue()
                .And
                .NotBeEmpty();
        }
    }
}
