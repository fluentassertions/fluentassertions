using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Specs
{
    /// <summary>
    /// Summary description for CustomAssertionSpecs
    /// </summary>
    [TestClass]
    public class NumericAssertionSpecs
    {
        [TestMethod]
        public void Should_succeed_when_asserting_positive_value_to_be_positive()
        {
            (1).Should().BePositive();
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_negative_value_to_be_positive()
        {
            (-1).Should().BePositive();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_negative_value_to_be_positive()
        {
            var assertions = (-1).Should();
            assertions.ShouldThrow(x => x.BePositive("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected positive value because we want to test the failure message, but found <-1>");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_negative_value_to_be_negative()
        {
            (-1).Should().BeNegative();
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_positive_value_to_be_negative()
        {
            1.Should().BeNegative();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_positive_value_to_be_negative()
        {
            var assertions = (1).Should();
            assertions.ShouldThrow(x => x.BeNegative("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected negative value because we want to test the failure message, but found <1>");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_equal_to_same_value()
        {
            1.Should().Be(1);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_value_to_be_equal_to_different_value()
        {
            1.Should().Be(2);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_equal_to_different_value()
        {
            var assertions = 1.Should();
            assertions.ShouldThrow(x => x.Be(2, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage(@"Expected <2> because we want to test the failure message, but found <1>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_not_equal_to_different_value()
        {
            1.Should().NotBe(2);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_value_to_be_not_equal_to_the_same_value()
        {
            1.Should().NotBe(1);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_not_equal_to_the_same_value()
        {
            var assertions = 1.Should();
            assertions.ShouldThrow(x => x.NotBe(1, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage(@"Did not expect <1> because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_greater_than_smaller_value()
        {
            2.Should().BeGreaterThan(1);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_value_to_be_greater_than_greater_value()
        {
            2.Should().BeGreaterThan(3);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_value_to_be_greater_than_same_value()
        {
            2.Should().BeGreaterThan(2);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_greater_than_greater_value()
        {
            var assertions = 2.Should();
            assertions.ShouldThrow(x => x.BeGreaterThan(3, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage(@"Expected a value greater than <3> because we want to test the failure message, but found <2>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_greater_or_equal_to_smaller_value()
        {
            2.Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_greater_or_equal_to_same_value()
        {
            2.Should().BeGreaterOrEqualTo(2);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_value_to_be_greater_or_equal_to_greater_value()
        {
            2.Should().BeGreaterOrEqualTo(3);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_greater_or_equal_to_greater_value()
        {
            var assertions = 2.Should();
            assertions.ShouldThrow(x => x.BeGreaterOrEqualTo(3, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage(@"Expected a value greater or equal to <3> because we want to test the failure message, but found <2>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_less_than_greater_value()
        {
            1.Should().BeLessThan(2);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_value_to_be_less_than_smaller_value()
        {
            2.Should().BeLessThan(1);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_value_to_be_less_than_same_value()
        {
            2.Should().BeLessThan(2);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_less_than_smaller_value()
        {
            var assertions = 2.Should();
            assertions.ShouldThrow(x => x.BeLessThan(1, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage(@"Expected a value less than <1> because we want to test the failure message, but found <2>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_less_or_equal_to_greater_value()
        {
            1.Should().BeLessOrEqualTo(2);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_value_to_be_less_or_equal_to_same_value()
        {
            2.Should().BeLessOrEqualTo(2);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_value_to_be_less_or_equal_to_smaller_value()
        {
            2.Should().BeLessOrEqualTo(1);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_value_to_be_less_or_equal_to_smaller_value()
        {
            var assertions = 2.Should();
            assertions.ShouldThrow(x => x.BeLessOrEqualTo(1, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage(@"Expected a value less or equal to <1> because we want to test the failure message, but found <2>.");
        }

        [TestMethod]
        public void Should_suppor_chaining_constraints_with_and()
        {
            2.Should()
                .BePositive()
                .And
                .BeGreaterThan(1)
                .And
                .BeLessThan(3);
        }
    }
}