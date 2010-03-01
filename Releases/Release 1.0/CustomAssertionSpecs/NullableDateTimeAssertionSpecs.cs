using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class NullableDateTimeAssertionSpecs
    {
        [TestMethod]
        public void Should_succeed_when_asserting_nullable_datetime_value_with_a_value_to_have_a_value()
        {
            DateTime? nullableDateTime = DateTime.Today;
            nullableDateTime.Should()
                .HaveValue();
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void Should_fail_when_asserting_nullable_datetime_value_without_a_value_to_have_a_value()
        {
            DateTime? nullableDateTime = null;
            nullableDateTime.Should()
                .HaveValue();
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_datetime_value_without_a_value_to_be_null()
        {
            DateTime? nullableDateTime = null;
            nullableDateTime.Should()
                .NotHaveValue();
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void Should_fail_when_asserting_nullable_datetime_value_with_a_value_to_be_null()
        {
            DateTime? nullableDateTime = DateTime.Today;
            nullableDateTime.Should()
                .NotHaveValue();
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_numeric_value_equals_the_same_value()
        {
            DateTime? nullableDateTimeA = DateTime.Today;
            DateTime? nullableDateTimeB = DateTime.Today;
            nullableDateTimeA.Should().Equal(nullableDateTimeB);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_numeric_null_value_equals_null()
        {
            DateTime? nullableDateTimeA = null;
            DateTime? nullableDateTimeB = null;
            nullableDateTimeA.Should().Equal(nullableDateTimeB);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Should_fail_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            DateTime? nullableDateTimeA = DateTime.Today;
            DateTime? nullableDateTimeB = DateTime.Today.AddDays(2);
            nullableDateTimeA.Should().Equal(nullableDateTimeB);
        }

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);
            DateTime? nullableDateTime = DateTime.Today;
            nullableDateTime.Should()
                .HaveValue()
                .And
                .BeAfter(yesterday);
        }
    }
}