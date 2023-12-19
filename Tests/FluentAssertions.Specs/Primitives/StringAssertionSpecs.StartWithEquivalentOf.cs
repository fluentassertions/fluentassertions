using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]StartWithEquivalentOf specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class StartWithEquivalent
    {
        [Fact]
        public void When_start_of_string_differs_by_case_only_it_should_not_throw()
        {
            // Arrange
            string actual = "ABC";
            string expectedPrefix = "Ab";

            // Act / Assert
            actual.Should().StartWithEquivalentOf(expectedPrefix);
        }

        [Fact]
        public void When_start_of_string_does_not_meet_equivalent_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().StartWithEquivalentOf("bc", "because it should start");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to start with equivalent of \"bc\" because it should start, but \"ABC\" differs near \"ABC\" (index 0).");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void
            When_start_of_string_does_not_meet_equivalent_and_one_of_them_is_long_it_should_display_both_strings_on_separate_line()
        {
            // Act
            Action act = () => "ABCDEFGHI".Should().StartWithEquivalentOf("abcddfghi", "it should {0}", "start");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to start with equivalent of " +
                "*\"abcddfghi\" because it should start, but " +
                "*\"ABCDEFGHI\" differs near \"EFG\" (index 4).");
        }

        [Fact]
        public void When_start_of_string_is_compared_with_equivalent_of_null_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().StartWithEquivalentOf(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare string start equivalence with <null>.*");
        }

        [Fact]
        public void When_start_of_string_is_compared_with_equivalent_of_empty_string_it_should_not_throw()
        {
            // Act
            Action act = () => "ABC".Should().StartWithEquivalentOf("");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_start_of_string_is_compared_with_equivalent_of_string_that_is_longer_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().StartWithEquivalentOf("abcdef");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to start with equivalent of " +
                "\"abcdef\", but " +
                "\"ABC\" is too short.");
        }

        [Fact]
        public void When_string_start_is_compared_with_equivalent_and_actual_value_is_null_then_it_should_throw()
        {
            // Act
            string someString = null;
            Action act = () => someString.Should().StartWithEquivalentOf("AbC");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected someString to start with equivalent of \"AbC\", but found <null>.");
        }
    }

    public class NotStartWithEquivalent
    {
        [Fact]
        public void When_asserting_string_does_not_start_with_equivalent_of_a_value_and_it_does_not_it_should_succeed()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotStartWithEquivalentOf("Bc");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_string_does_not_start_with_equivalent_of_a_value_but_it_does_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotStartWithEquivalentOf("aB", "because of some reason");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected value that does not start with equivalent of \"aB\" because of some reason, but found \"ABC\".");
        }

        [Fact]
        public void When_asserting_string_does_not_start_with_equivalent_of_a_value_that_is_null_it_should_throw()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotStartWithEquivalentOf(null);

            // Assert
            action.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare start of string with <null>.*");
        }

        [Fact]
        public void When_asserting_string_does_not_start_with_equivalent_of_a_value_that_is_empty_it_should_throw()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotStartWithEquivalentOf("");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected value that does not start with equivalent of \"\", but found \"ABC\".");
        }

        [Fact]
        public void When_asserting_string_does_not_start_with_equivalent_of_a_value_and_actual_value_is_null_it_should_throw()
        {
            // Arrange
            string someString = null;

            // Act
            Action act = () => someString.Should().NotStartWithEquivalentOf("ABC");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString that does not start with equivalent of \"ABC\", but found <null>.");
        }
    }
}
