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
