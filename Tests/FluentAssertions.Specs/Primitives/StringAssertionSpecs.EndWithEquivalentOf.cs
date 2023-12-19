using System;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]EndWithEquivalentOf specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class EndWithEquivalent
    {
        [Fact]
        public void When_suffix_of_string_differs_by_case_only_it_should_not_throw()
        {
            // Arrange
            string actual = "ABC";
            string expectedSuffix = "bC";

            // Act / Assert
            actual.Should().EndWithEquivalentOf(expectedSuffix);
        }

        [Fact]
        public void When_end_of_string_differs_by_case_only_it_should_not_throw()
        {
            // Arrange
            string actual = "ABC";
            string expectedSuffix = "AbC";

            // Act / Assert
            actual.Should().EndWithEquivalentOf(expectedSuffix);
        }

        [Fact]
        public void When_end_of_string_does_not_meet_equivalent_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().EndWithEquivalentOf("ab", "because it should end");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string that ends with equivalent of \"ab\" because it should end, but found \"ABC\".");
        }

        [Fact]
        public void When_end_of_string_is_compared_with_equivalent_of_null_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().EndWithEquivalentOf(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare string end equivalence with <null>.*");
        }

        [Fact]
        public void When_end_of_string_is_compared_with_equivalent_of_empty_string_it_should_not_throw()
        {
            // Act
            Action act = () => "ABC".Should().EndWithEquivalentOf("");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_ending_is_compared_with_equivalent_of_string_that_is_longer_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().EndWithEquivalentOf("00abc");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to end with equivalent of " +
                "\"00abc\", but " +
                "\"ABC\" is too short.");
        }

        [Fact]
        public void When_string_ending_is_compared_with_equivalent_and_actual_value_is_null_then_it_should_throw()
        {
            // Arrange
            string someString = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                someString.Should().EndWithEquivalentOf("abC");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString that ends with equivalent of \"abC\", but found <null>.");
        }
    }

    public class NotEndWithEquivalent
    {
        [Fact]
        public void When_asserting_string_does_not_end_with_equivalent_of_a_value_and_it_does_not_it_should_succeed()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotEndWithEquivalentOf("aB");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_string_does_not_end_with_equivalent_of_a_value_but_it_does_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotEndWithEquivalentOf("Bc", "because of some {0}", "reason");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected value that does not end with equivalent of \"Bc\" because of some reason, but found \"ABC\".");
        }

        [Fact]
        public void When_asserting_string_does_not_end_with_equivalent_of_a_value_that_is_null_it_should_throw()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotEndWithEquivalentOf(null);

            // Assert
            action.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare end of string with <null>.*");
        }

        [Fact]
        public void When_asserting_string_does_not_end_with_equivalent_of_a_value_that_is_empty_it_should_throw()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotEndWithEquivalentOf("");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected value that does not end with equivalent of \"\", but found \"ABC\".");
        }

        [Fact]
        public void When_asserting_string_does_not_end_with_equivalent_of_a_value_and_actual_value_is_null_it_should_throw()
        {
            // Arrange
            string someString = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                someString.Should().NotEndWithEquivalentOf("Abc", "some {0}", "reason");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString that does not end with equivalent of \"Abc\"*some reason*, but found <null>.");
        }
    }
}
