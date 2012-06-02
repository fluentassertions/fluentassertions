using System;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class NullableBooleanAssertionSpecs
    {
        [TestMethod]
        public void Should_succeed_when_asserting_nullable_boolean_value_with_a_value_to_have_a_value()
        {
            bool? nullableBoolean = true;
            nullableBoolean.Should().HaveValue();
        }

        [TestMethod]
        public void Should_fail_when_asserting_nullable_boolean_value_without_a_value_to_have_a_value()
        {
            bool? nullableBoolean = null;
            Action act = () => nullableBoolean.Should().HaveValue();
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_boolean_value_without_a_value_to_have_a_value()
        {
            bool? nullableBoolean = null;
            var assertions = nullableBoolean.Should();
            assertions.Invoking(x => x.HaveValue("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_boolean_value_without_a_value_to_be_null()
        {
            bool? nullableBoolean = null;
            nullableBoolean.Should().NotHaveValue();
        }

        [TestMethod]
        public void Should_fail_when_asserting_nullable_boolean_value_with_a_value_to_be_null()
        {
            bool? nullableBoolean = true;
            Action act = () => nullableBoolean.Should().NotHaveValue();
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_boolean_value_with_a_value_to_be_null()
        {
            bool? nullableBoolean = true;
            var assertions = nullableBoolean.Should();
            assertions.Invoking(x => x.NotHaveValue("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found True.");
        }

        [TestMethod]
        public void Should_fail_when_asserting_boolean_null_value_is_false()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            bool? nullableBoolean = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableBoolean.Should().BeFalse("we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected False because we want to test the failure message, but found <null>.");
        }

        [TestMethod]
        public void Should_fail_when_asserting_boolean_null_value_is_true()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            bool? nullableBoolean = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableBoolean.Should().BeTrue("we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected True because we want to test the failure message, but found <null>.");
        }

        [TestMethod]
        public void Should_fail_when_asserting_boolean_null_value_to_be_equal_to_different_nullable_boolean()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            bool? nullableBoolean = null;
            bool? differentNullableBoolean = false;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableBoolean.Should().Be(differentNullableBoolean, "we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected False because we want to test the failure message, but found <null>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_boolean_null_value_to_be_equal_to_null()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            bool? nullableBoolean = null;
            bool? otherNullableBoolean = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableBoolean.Should().Be(otherNullableBoolean);

            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            bool? nullableBoolean = true;
            nullableBoolean.Should()
                .HaveValue()
                .And
                .BeTrue();
        }
    }
}