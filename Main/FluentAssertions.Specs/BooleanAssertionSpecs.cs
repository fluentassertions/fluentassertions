using System;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

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
        public void Should_fail_when_asserting_boolean_value_false_is_true()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                false.Should().BeTrue();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_false_is_true()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                false.Should().BeTrue("because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected True because we want to test the failure message, but found False.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_boolean_value_false_is_false()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                false.Should().BeFalse();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_fail_when_asserting_boolean_value_true_is_false()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                true.Should().BeFalse();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_true_is_false()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                true.Should().BeFalse("we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected False because we want to test the failure message, but found True.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_boolean_value_to_be_equal_to_the_same_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                false.Should().Be(false);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_fail_when_asserting_boolean_value_to_be_equal_to_a_different_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                false.Should().Be(true);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_to_be_equal_to_a_different_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                false.Should().Be(true, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected True because we want to test the failure message, but found False.");
        }
    }
}