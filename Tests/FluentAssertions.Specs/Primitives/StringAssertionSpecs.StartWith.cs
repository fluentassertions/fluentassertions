using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]StartWith specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class StartWith
    {
        [Fact]
        public void When_asserting_string_starts_with_the_same_value_it_should_not_throw()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().StartWith("AB");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_string_does_not_start_with_expected_phrase_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().StartWith("ABB", "it should {0}", "start");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to start with \"ABB\" because it should start," +
                " but \"ABC\" differs near \"C\" (index 2).");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void
            When_string_does_not_start_with_expected_phrase_and_one_of_them_is_long_it_should_display_both_strings_on_separate_line()
        {
            // Act
            Action act = () => "ABCDEFGHI".Should().StartWith("ABCDDFGHI", "it should {0}", "start");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to start with " +
                "*\"ABCDDFGHI\" because it should start, but " +
                "*\"ABCDEFGHI\" differs near \"EFG\" (index 4).");
        }

        [Fact]
        public void When_string_start_is_compared_with_null_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().StartWith(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare start of string with <null>.*");
        }

        [Fact]
        public void When_string_start_is_compared_with_empty_string_it_should_not_throw()
        {
            // Act
            Action act = () => "ABC".Should().StartWith("");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_string_start_is_compared_with_string_that_is_longer_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().StartWith("ABCDEF");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to start with \"ABCDEF\", but \"ABC\" is too short.");
        }

        [Fact]
        public void When_string_start_is_compared_and_actual_value_is_null_then_it_should_throw()
        {
            // Act
            string someString = null;
            Action act = () => someString.Should().StartWith("ABC");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected someString to start with \"ABC\", but found <null>.");
        }
    }

    public class NotStartWith
    {
        [Fact]
        public void When_asserting_string_does_not_start_with_a_value_and_it_does_not_it_should_succeed()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotStartWith("DE");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_string_does_not_start_with_a_value_but_it_does_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotStartWith("AB", "because of some reason");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected value that does not start with \"AB\" because of some reason, but found \"ABC\".");
        }

        [Fact]
        public void When_asserting_string_does_not_start_with_a_value_that_is_null_it_should_throw()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotStartWith(null);

            // Assert
            action.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare start of string with <null>.*");
        }

        [Fact]
        public void When_asserting_string_does_not_start_with_a_value_that_is_empty_it_should_throw()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotStartWith("");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected value that does not start with \"\", but found \"ABC\".");
        }

        [Fact]
        public void When_asserting_string_does_not_start_with_a_value_and_actual_value_is_null_it_should_throw()
        {
            // Act
            string someString = null;
            Action act = () => someString.Should().NotStartWith("ABC");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString that does not start with \"ABC\", but found <null>.");
        }
    }
}
