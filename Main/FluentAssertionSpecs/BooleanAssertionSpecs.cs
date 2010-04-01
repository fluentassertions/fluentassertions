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
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_boolean_value_false_is_true()
        {
            false.Should().BeTrue();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_false_is_true()
        {
            var assertions = false.Should();
            assertions.Invoking(x => x.BeTrue("because we want to test the failure {0}", "message"))
                .ShouldThrow<SpecificationMismatchException>()
                .WithMessage("Expected <True> because we want to test the failure message, but found <False>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_boolean_value_false_is_false()
        {
            false.Should().BeFalse();
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_boolean_value_true_is_false()
        {
            true.Should().BeFalse();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_true_is_false()
        {
            var assertions = true.Should();
            assertions.Invoking(x => x.BeFalse("because we want to test the failure {0}", "message"))
                .ShouldThrow<SpecificationMismatchException>()
                .WithMessage("Expected <False> because we want to test the failure message, but found <True>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_boolean_value_to_be_equal_to_the_same_value()
        {
            false.Should().Equal(false);
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_boolean_value_to_be_equal_to_a_different_value()
        {
            false.Should().Equal(true);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_boolean_value_to_be_equal_to_a_different_value()
        {
            var assertions = false.Should();
            assertions.Invoking(x => x.Equal(true, "because we want to test the failure {0}", "message"))
                .ShouldThrow<SpecificationMismatchException>()
                .WithMessage("Expected <True> because we want to test the failure message, but found <False>.");
        }
    }
}