using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class NullableGuidAssertionSpecs
    {
        [TestMethod]
        public void Should_succeed_when_asserting_nullable_guid_value_with_a_value_to_have_a_value()
        {
            Guid? nullableGuid = Guid.NewGuid();
            nullableGuid.Should().HaveValue();
        }

        [TestMethod]
        public void Should_fail_when_asserting_nullable_guid_value_without_a_value_to_have_a_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Guid? nullableGuid = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => nullableGuid.Should().HaveValue();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_guid_value_without_a_value_to_have_a_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Guid? nullableGuid = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => nullableGuid.Should().HaveValue("because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_guid_value_without_a_value_to_be_null()
        {
            Guid? nullableGuid = null;
            nullableGuid.Should().NotHaveValue();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Should_fail_when_asserting_nullable_guid_value_with_a_value_to_be_null()
        {
            Guid? nullableGuid = Guid.NewGuid();
            nullableGuid.Should().NotHaveValue();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_guid_value_with_a_value_to_be_null()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Guid? nullableGuid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => nullableGuid.Should().NotHaveValue("because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found 11111111-aaaa-bbbb-cccc-999999999999.");
        }

        [TestMethod]
        public void Should_fail_when_asserting_null_equals_some_guid()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Guid? nullableGuid = null;
            var someGuid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => nullableGuid.Should().Be(someGuid, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected GUID to be 11111111-aaaa-bbbb-cccc-999999999999 because we want to test the failure message, but found <null>.");
        }

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            Guid? nullableGuid = Guid.NewGuid();
            nullableGuid.Should()
                .HaveValue()
                .And
                .NotBeEmpty();
        }
    }
}