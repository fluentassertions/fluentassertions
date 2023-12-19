using System;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]EndWith specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class EndWith
    {
        [Fact]
        public void When_asserting_string_ends_with_a_suffix_it_should_not_throw()
        {
            // Arrange
            string actual = "ABC";
            string expectedSuffix = "BC";

            // Act / Assert
            actual.Should().EndWith(expectedSuffix);
        }

        [Fact]
        public void When_asserting_string_ends_with_the_same_value_it_should_not_throw()
        {
            // Arrange
            string actual = "ABC";
            string expectedSuffix = "ABC";

            // Act / Assert
            actual.Should().EndWith(expectedSuffix);
        }

        [Fact]
        public void When_string_does_not_end_with_expected_phrase_it_should_throw()
        {
            // Act
            Action act = () =>
            {
                using var a = new AssertionScope();
                "ABC".Should().EndWith("AB", "it should");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string \"ABC\" to end with \"AB\" because it should.");
        }

        [Fact]
        public void When_string_ending_is_compared_with_null_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().EndWith(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare string end with <null>.*");
        }

        [Fact]
        public void When_string_ending_is_compared_with_empty_string_it_should_not_throw()
        {
            // Act
            Action act = () => "ABC".Should().EndWith("");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_ending_is_compared_with_string_that_is_longer_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().EndWith("00ABC");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to end with " +
                "\"00ABC\", but " +
                "\"ABC\" is too short.");
        }

        [Fact]
        public void When_string_ending_is_compared_and_actual_value_is_null_then_it_should_throw()
        {
            // Arrange
            string someString = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                someString.Should().EndWith("ABC");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString <null> to end with \"ABC\".");
        }
    }

    public class NotEndWith
    {
        [Fact]
        public void When_asserting_string_does_not_end_with_a_value_and_it_does_not_it_should_succeed()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotEndWith("AB");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_string_does_not_end_with_a_value_but_it_does_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotEndWith("BC", "because of some {0}", "reason");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected value \"ABC\" not to end with \"BC\" because of some reason.");
        }

        [Fact]
        public void When_asserting_string_does_not_end_with_a_value_that_is_null_it_should_throw()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotEndWith(null);

            // Assert
            action.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare end of string with <null>.*");
        }

        [Fact]
        public void When_asserting_string_does_not_end_with_a_value_that_is_empty_it_should_throw()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotEndWith("");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected value \"ABC\" not to end with \"\".");
        }

        [Fact]
        public void When_asserting_string_does_not_end_with_a_value_and_actual_value_is_null_it_should_throw()
        {
            // Arrange
            string someString = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                someString.Should().NotEndWith("ABC", "some {0}", "reason");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString that does not end with \"ABC\"*some reason*, but found <null>.");
        }
    }
}
