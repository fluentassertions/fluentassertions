using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class StringAssertionSpecs
    {
        [TestMethod]
        public void Should_succeed_when_asserting_string_to_be_equal_to_the_same_value()
        {
            "ABC".Should().Equal("ABC");
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_string_to_be_equal_to_different_value()
        {
            "ABC".Should().Equal("DEF");
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_string_to_be_equal_to_different_value()
        {
            var assertions = "ABC".Should();
            assertions.ShouldThrow(x => x.Equal("DEF", "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected \"DEF\" because we want to test the failure message, but found \"ABC\".");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_string_is_not_equal_to_a_different_value()
        {
            "ABC".Should().NotEqual("DEF");
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_string_is_not_equal_to_the_same_value()
        {
            "ABC".Should().NotEqual("ABC");
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_string_is_not_equal_to_the_same_value()
        {
            var assertions = "ABC".Should();
            assertions.ShouldThrow(x => x.NotEqual("ABC", "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Did not expect \"ABC\" because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_string_starts_with_the_same_value()
        {
            "ABC".Should().StartWith("AB");
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_string_starts_with_a_different_value()
        {
            "ABC".Should().StartWith("BC");
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_string_starts_with_a_different_value()
        {
            var assertions = "ABC".Should();
            assertions.ShouldThrow(x => x.StartWith("BC", "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected string starting with \"BC\" because we want to test the failure message, but found \"ABC\".");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_string_ends_with_the_same_value()
        {
            "ABC".Should().EndWith("BC");
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_string_ends_with_a_different_value()
        {
            "ABC".Should().EndWith("AB");
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_string_ends_with_a_different_value()
        {
            var assertions = "ABC".Should();
            assertions.ShouldThrow(x => x.EndWith("AB", "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected string ending with \"AB\" because we want to test the failure message, but found \"ABC\".");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_string_starts_with_an_equivalent_value()
        {
            "ABC".Should().StartWithEquivalent("Ab");
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_string_starts_with_a_non_equivalent_value()
        {
            "ABC".Should().StartWithEquivalent("bc");
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_string_starts_with_a_non_equivalent_value()
        {
            var assertions = "ABC".Should();
            assertions.ShouldThrow(x => x.StartWithEquivalent("bc", "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage(
                "Expected string starting with equivalent of \"bc\" because we want to test the failure message, but found \"ABC\".");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_string_ends_with_an_equivalent_value()
        {
            "ABC".Should().EndWithEquivalent("bc");
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_string_ends_with_a_non_equivalent_value()
        {
            "ABC".Should().EndWithEquivalent("ab");
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_string_ends_with_a_non_equivalent_value()
        {
            var assertions = "ABC".Should();
            assertions.ShouldThrow(x => x.EndWithEquivalent("ab", "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage(
                "Expected string ending with equivalent of \"ab\" because we want to test the failure message, but found \"ABC\".");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_string_contains_a_value_that_is_part_of_the_string()
        {
            "ABCDEF".Should().Contain("BCD");
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_string_contains_a_value_that_is_not_part_of_the_string()
        {
            "ABCDEF".Should().Contain("XYZ");
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_string_contains_a_value_that_is_not_part_of_the_string()
        {
            var assertions = "ABCDEF".Should();
            assertions.ShouldThrow(x => x.Contain("XYZ", "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected string containing \"XYZ\" because we want to test the failure message, but found \"ABCDEF\".");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_string_to_be_equivalent_to_the_same_value()
        {
            "ABC".Should().BeEquivalentTo("abc");
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_string_to_be_equivalent_to_different_value()
        {
            "ABC".Should().BeEquivalentTo("def");
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_string_to_be_equivalent_to_different_value()
        {
            var assertions = "ABC".Should();
            assertions.ShouldThrow(x => x.BeEquivalentTo("def", "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected string equivalent to \"def\" because we want to test the failure message, but found \"ABC\".");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_empty_string_to_be_empty()
        {
            "".Should().BeEmpty();
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_non_empty_string_to_be_empty()
        {
            "ABC".Should().BeEmpty();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_non_empty_string_to_be_empty()
        {
            var assertions = "ABC".Should();
            assertions.ShouldThrow(x => x.BeEmpty("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected empty string because we want to test the failure message, but found \"ABC\".");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_non_empty_string_to_be_filled()
        {
            "ABC".Should().NotBeEmpty();
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_empty_string_to_be_filled()
        {
            "".Should().NotBeEmpty();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_empty_string_to_be_filled()
        {
            var assertions = "".Should();
            assertions.ShouldThrow(x => x.NotBeEmpty("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Did not expect empty string because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_string_length_to_be_equal_to_the_same_value()
        {
            "ABC".Should().HaveLength(3);
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_string_length_to_be_equal_to_different_value()
        {
            "ABC".Should().HaveLength(1);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_string_length_to_be_equal_to_different_value()
        {
            var assertions = "ABC".Should();
            assertions.ShouldThrow(x => x.HaveLength(1, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage(
                "Expected string with length <1> because we want to test the failure message, but found string \"ABC\".");
        }

        [TestMethod]
        public void Should_support_chaining_assertions_with_and()
        {
            "ABCDEFGHI".Should()
                .StartWith("AB").And
                .EndWith("HI").And
                .Contain("EF").And
                .HaveLength(9);
        }
    }
}