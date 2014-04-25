using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions.Common;

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class NullableSimpleTimeSpanAssertionSpecs
    {
        [TestMethod]
        public void Should_succeed_when_asserting_nullable_TimeSpan_value_with_a_value_to_have_a_value()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            TimeSpan? nullableTimeSpan = new TimeSpan();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            nullableTimeSpan.Should().HaveValue();
        }

        [TestMethod]
        public void Should_fail_when_asserting_nullable_TimeSpan_value_without_a_value_to_have_a_value()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            TimeSpan? nullableTimeSpan = null;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => nullableTimeSpan.Should().HaveValue();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_TimeSpan_value_without_a_value_to_have_a_value()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            TimeSpan? nullableTimeSpan = null;
            var assertions = nullableTimeSpan.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            assertions.Invoking(x => x.HaveValue("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_TimeSpan_value_without_a_value_to_be_null()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            TimeSpan? nullableTimeSpan = null;

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            nullableTimeSpan.Should().NotHaveValue();
        }

        [TestMethod]
        public void Should_fail_when_asserting_nullable_TimeSpan_value_with_a_value_to_be_null()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            TimeSpan? nullableTimeSpan = new TimeSpan();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => nullableTimeSpan.Should().NotHaveValue();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_TimeSpan_value_with_a_value_to_be_null()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            TimeSpan? nullableTimeSpan = 1.Seconds();
            var assertions = nullableTimeSpan.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            assertions.Invoking(x => x.NotHaveValue("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found 1s.");
        }

        [TestMethod]
        public void When_asserting_a_nullable_TimeSpan_is_equal_to_a_different_nullable_TimeSpan_it_should_should_throw_appropriately()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            TimeSpan? nullableTimeSpanA = 1.Seconds();
            TimeSpan? nullableTimeSpanB = 2.Seconds();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => nullableTimeSpanA.Should().Be(nullableTimeSpanB);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_a_nullable_TimeSpan_is_equal_to_another_a_nullable_TimeSpan_but_it_is_null_it_should_fail_with_a_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            TimeSpan? nullableTimeSpanA = null;
            TimeSpan? nullableTimeSpanB = 1.Seconds();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action =
                () =>
                    nullableTimeSpanA.Should()
                        .Be(nullableTimeSpanB, "because we want to test the failure {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected 1s because we want to test the failure message, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_a_nullable_TimeSpan_is_equal_to_another_a_nullable_TimeSpan_and_both_are_null_it_should_succed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            TimeSpan? nullableTimeSpanA = null;
            TimeSpan? nullableTimeSpanB = null;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => nullableTimeSpanA.Should().Be(nullableTimeSpanB);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_nullable_TimeSpan_is_equal_to_the_same_nullable_TimeSpan_it_should_succed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            TimeSpan? nullableTimeSpanA = new TimeSpan();
            TimeSpan? nullableTimeSpanB = new TimeSpan();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => nullableTimeSpanA.Should().Be(nullableTimeSpanB);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            TimeSpan? nullableTimeSpan = new TimeSpan();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            nullableTimeSpan.Should()
                .HaveValue()
                .And
                .Be(new TimeSpan());
        }
    }
}
