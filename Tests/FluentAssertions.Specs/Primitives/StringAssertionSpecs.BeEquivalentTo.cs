using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]BeEquivalentTo specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class BeEquivalentTo
    {
        [Fact]
        public void When_strings_are_the_same_while_ignoring_case_it_should_not_throw()
        {
            // Arrange
            string actual = "ABC";
            string expectedEquivalent = "abc";

            // Act / Assert
            actual.Should().BeEquivalentTo(expectedEquivalent);
        }

        [Fact]
        public void When_strings_differ_other_than_by_case_it_should_throw()
        {
            // Act
            Action act = () => "ADC".Should().BeEquivalentTo("abc", "we will test {0} + {1}", 1, 2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be equivalent to \"abc\" because we will test 1 + 2, but \"ADC\" differs near \"DC\" (index 1).");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_non_null_string_is_expected_to_be_equivalent_to_null_it_should_throw()
        {
            // Act
            Action act = () => "ABCDEF".Should().BeEquivalentTo(null);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be equivalent to <null>, but found \"ABCDEF\".");
        }

        [Fact]
        public void When_non_empty_string_is_expected_to_be_equivalent_to_empty_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().BeEquivalentTo("");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be equivalent to \"\" with a length of 0, but \"ABC\" has a length of 3, differs near \"ABC\" (index 0).");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_string_is_equivalent_but_too_short_it_should_throw()
        {
            // Act
            Action act = () => "AB".Should().BeEquivalentTo("ABCD");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be equivalent to \"ABCD\" with a length of 4, but \"AB\" has a length of 2*");
        }

        [Fact]
        public void When_string_equivalence_is_asserted_and_actual_value_is_null_then_it_should_throw()
        {
            // Act
            string someString = null;
            Action act = () => someString.Should().BeEquivalentTo("abc", "we will test {0} + {1}", 1, 2);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected someString to be equivalent to \"abc\" because we will test 1 + 2, but found <null>.");
        }

        [Fact]
        public void
            When_the_expected_string_is_equivalent_to_the_actual_string_but_with_trailing_spaces_it_should_throw_with_clear_error_message()
        {
            // Act
            Action act = () => "ABC".Should().BeEquivalentTo("abc ", "because I say {0}", "so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be equivalent to \"abc \" because I say so, but it misses some extra whitespace at the end.");
        }

        [Fact]
        public void
            When_the_actual_string_equivalent_to_the_expected_but_with_trailing_spaces_it_should_throw_with_clear_error_message()
        {
            // Act
            Action act = () => "ABC ".Should().BeEquivalentTo("abc", "because I say {0}", "so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be equivalent to \"abc\" because I say so, but it has unexpected whitespace at the end.");
        }
    }

    public class NotBeEquivalentTo
    {
        [Fact]
        public void When_strings_are_the_same_while_ignoring_case_it_should_throw()
        {
            // Arrange
            string actual = "ABC";
            string unexpected = "abc";

            // Act
            Action action = () => actual.Should().NotBeEquivalentTo(unexpected, "because I say {0}", "so");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected actual not to be equivalent to \"abc\" because I say so, but they are.");
        }

        [Fact]
        public void When_strings_differ_other_than_by_case_it_should_not_throw()
        {
            // Act
            Action act = () => "ADC".Should().NotBeEquivalentTo("abc");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_non_null_string_is_expected_to_be_equivalent_to_null_it_should_not_throw()
        {
            // Act
            Action act = () => "ABCDEF".Should().NotBeEquivalentTo(null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_non_empty_string_is_expected_to_be_equivalent_to_empty_it_should_not_throw()
        {
            // Act
            Action act = () => "ABC".Should().NotBeEquivalentTo("");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_is_equivalent_but_too_short_it_should_not_throw()
        {
            // Act
            Action act = () => "AB".Should().NotBeEquivalentTo("ABCD");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_equivalence_is_asserted_and_actual_value_is_null_then_it_should_not_throw()
        {
            // Arrange
            string someString = null;

            // Act
            Action act = () => someString.Should().NotBeEquivalentTo("abc");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_expected_string_is_equivalent_to_the_actual_string_but_with_trailing_spaces_it_should_not_throw()
        {
            // Act
            Action act = () => "ABC".Should().NotBeEquivalentTo("abc ");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_actual_string_equivalent_to_the_expected_but_with_trailing_spaces_it_should_not_throw()
        {
            // Act
            Action act = () => "ABC ".Should().NotBeEquivalentTo("abc");

            // Assert
            act.Should().NotThrow();
        }
    }
}
