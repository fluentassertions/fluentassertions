using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
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
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_nullable_boolean_value_without_a_value_to_have_a_value()
        {
            bool? nullableBoolean = null;
            nullableBoolean.Should().HaveValue();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_boolean_value_without_a_value_to_have_a_value()
        {
            bool? nullableBoolean = null;
            var assertions = nullableBoolean.Should();
            assertions.ShouldThrow(x => x.HaveValue("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected a value because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_nullable_boolean_value_without_a_value_to_be_null()
        {
            bool? nullableBoolean = null;
            nullableBoolean.Should().NotHaveValue();
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_nullable_boolean_value_with_a_value_to_be_null()
        {
            bool? nullableBoolean = true;
            nullableBoolean.Should().NotHaveValue();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_nullable_boolean_value_with_a_value_to_be_null()
        {
            bool? nullableBoolean = true;
            var assertions = nullableBoolean.Should();
            assertions.ShouldThrow(x => x.NotHaveValue("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Did not expect a value because we want to test the failure message, but found <True>.");
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