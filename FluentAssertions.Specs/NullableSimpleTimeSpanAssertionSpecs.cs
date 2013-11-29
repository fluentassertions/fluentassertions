using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions.Common;

#if WINRT
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
            TimeSpan? nullableTimeSpan = new TimeSpan();
            nullableTimeSpan.Should().HaveValue();
        }

        [TestMethod]
        public void Should_fail_when_asserting_nullable_TimeSpan_value_without_a_value_to_have_a_value()
        {
            TimeSpan? nullableTimeSpan = null;
            Action act = () => nullableTimeSpan.Should().HaveValue();
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_TimeSpan_value_without_a_value_to_have_a_value()
        {
            TimeSpan? nullableTimeSpan = null;
            var assertions = nullableTimeSpan.Should();
            assertions.Invoking(x => x.HaveValue("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected a value because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_TimeSpan_value_without_a_value_to_be_null()
        {
            TimeSpan? nullableTimeSpan = null;
            nullableTimeSpan.Should().NotHaveValue();
        }

        [TestMethod]
        public void Should_fail_when_asserting_nullable_TimeSpan_value_with_a_value_to_be_null()
        {
            TimeSpan? nullableTimeSpan = new TimeSpan();
            Action act = () => nullableTimeSpan.Should().NotHaveValue();
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_TimeSpan_value_with_a_value_to_be_null()
        {
            TimeSpan? nullableTimeSpan = 1.Seconds();
            var assertions = nullableTimeSpan.Should();
            assertions.Invoking(x => x.NotHaveValue("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect a value because we want to test the failure message, but found 1s.");
        }

        [TestMethod]
        public void Should_fail_when_asserting_nullable_TimeSpan_value_equals_a_different_value()
        {
            TimeSpan? nullableTimeSpanA = 1.Seconds();
            TimeSpan? nullableTimeSpanB = 2.Seconds();

            Action action = () => nullableTimeSpanA.Should().Be(nullableTimeSpanB);

            action.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_TimeSpan_null_value_is_equal_to_another_value()
        {
            TimeSpan? nullableTimeSpanA = null;
            TimeSpan? nullableTimeSpanB = 1.Seconds();

            Action action =
                () =>
                    nullableTimeSpanA.Should()
                        .Be(nullableTimeSpanB, "because we want to test the failure {0}", "message");

            action.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected 1s because we want to test the failure message, but found <null>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_TimeSpan_null_value_equals_null()
        {
            TimeSpan? nullableTimeSpanA = null;
            TimeSpan? nullableTimeSpanB = null;

            Action action = () => nullableTimeSpanA.Should().Be(nullableTimeSpanB);

            action.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_TimeSpan_value_equals_the_same_value()
        {
            TimeSpan? nullableTimeSpanA = new TimeSpan();
            TimeSpan? nullableTimeSpanB = new TimeSpan();

            Action action = () => nullableTimeSpanA.Should().Be(nullableTimeSpanB);

            action.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            TimeSpan? nullableTimeSpan = new TimeSpan();
            nullableTimeSpan.Should()
                .HaveValue()
                .And
                .Be(new TimeSpan());
        }
    }
}
