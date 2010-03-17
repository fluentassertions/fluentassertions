using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class NullableNumericAssertionSpecs
    {
        [TestMethod]
        public void Should_succeed_when_asserting_nullable_numeric_value_with_value_to_have_a_value()
        {
            int? nullableInteger = 1;
            nullableInteger.Should().HaveValue();
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_nullable_numeric_value_without_a_value_to_have_a_value()
        {
            int? nullableInteger = null;
            nullableInteger.Should().HaveValue();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_without_a_value_to_have_a_value()
        {
            int? nullableInteger = null;
            var assertions = nullableInteger.Should();
            assertions.ShouldThrow(x => x.HaveValue("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected a value because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_numeric_value_without_a_value_to_be_null()
        {
            int? nullableInteger = null;
            nullableInteger.Should().NotHaveValue();
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_nullable_numeric_value_with_a_value_to_be_null()
        {
            int? nullableInteger = 1;
            nullableInteger.Should().NotHaveValue();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_with_a_value_to_be_null()
        {
            int? nullableInteger = 1;
            var assertions = nullableInteger.Should();
            assertions.ShouldThrow(x => x.NotHaveValue("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Did not expect a value because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_numeric_value_equals_an_equal_value()
        {
            int? nullableIntegerA = 1;
            int? nullableIntegerB = 1;
            nullableIntegerA.Should().Be(nullableIntegerB);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_numeric_null_value_equals_null()
        {
            int? nullableIntegerA = null;
            int? nullableIntegerB = null;
            nullableIntegerA.Should().Be(nullableIntegerB);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            int? nullableIntegerA = 1;
            int? nullableIntegerB = 2;
            nullableIntegerA.Should().Be(nullableIntegerB);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            int? nullableIntegerA = 1;
            int? nullableIntegerB = 2;
            var assertions = nullableIntegerA.Should();
            assertions.ShouldThrow(x => x.Be(nullableIntegerB, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected value <2> because we want to test the failure message, but found <1>.");
        }

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            int? nullableInteger = 1;
            nullableInteger.Should()
                .HaveValue()
                .And
                .BePositive();
        }
    }
}