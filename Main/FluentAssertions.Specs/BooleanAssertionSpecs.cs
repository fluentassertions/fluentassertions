using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class BooleanAssertionSpecs
    {
        [TestMethod]
        public void Should_succeed_when_asserting_boolean_value_true_is_true()
        {
            true.Should().BeTrue();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Should_fail_when_asserting_boolean_value_false_is_true()
        {
            false.Should().BeTrue();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_false_is_true()
        {
            var assertions = false.Should();
            assertions.Invoking(x => x.BeTrue("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected True because we want to test the failure message, but found False.");
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
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected False because we want to test the failure message, but found <null>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_boolean_value_false_is_false()
        {
            false.Should().BeFalse();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Should_fail_when_asserting_boolean_value_true_is_false()
        {
            true.Should().BeFalse();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_true_is_false()
        {
            var assertions = true.Should();
            assertions.Invoking(x => x.BeFalse("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected False because we want to test the failure message, but found True.");
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
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected True because we want to test the failure message, but found <null>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_boolean_value_to_be_equal_to_the_same_value()
        {
            false.Should().Be(false);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Should_fail_when_asserting_boolean_value_to_be_equal_to_a_different_value()
        {
            false.Should().Be(true);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_to_be_equal_to_a_different_value()
        {
            var assertions = false.Should();
            assertions.Invoking(x => x.Be(true, "because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected True because we want to test the failure message, but found False.");
        }

        [TestMethod]
        public void Should_fail_when_asserting_boolean_null_value_to_be_equal_to_false()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            bool? nullableBoolean = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                nullableBoolean.Should().Be(false, "we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected False because we want to test the failure message, but found <null>.");
        }
    }
}