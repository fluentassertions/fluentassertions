using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

/// <content>
/// The [Not]Be specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class Be
    {
        [Fact]
        public void When_both_values_are_the_same_it_should_not_throw()
        {
            // Act / Assert
            "ABC".Should().Be("ABC");
        }

        [Fact]
        public void When_both_subject_and_expected_are_null_it_should_succeed()
        {
            // Arrange
            string actualString = null;
            string expectedString = null;

            // Act
            Action act = () => actualString.Should().Be(expectedString);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_two_strings_differ_unexpectedly_it_should_throw()
        {
            // Act
            Action act = () => "ADC".Should().Be("ABC", "because we {0}", "do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be \"ABC\" because we do, but \"ADC\" differs near \"DC\" (index 1).");
        }

        [Fact]
        public void When_two_strings_differ_unexpectedly_containing_double_curly_closing_braces_it_should_throw()
        {
            // Act
            const string expect = "}}";
            const string actual = "}}}}";
            Action act = () => actual.Should().Be(expect);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*to be \"}}\" with a length of 2, but \"}}}}\" has a length of 4*");
        }

        [Fact]
        public void When_two_strings_differ_unexpectedly_containing_double_curly_opening_braces_it_should_throw()
        {
            // Act
            const string expect = "{{";
            const string actual = "{{{{";
            Action act = () => actual.Should().Be(expect);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*to be \"{{\" with a length of 2, but \"{{{{\" has a length of 4*");
        }

        [Fact]
        public void When_the_expected_string_is_shorter_than_the_actual_string_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().Be("AB");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*index 2*");
        }

        [Fact]
        public void When_the_expected_string_is_longer_than_the_actual_string_it_should_throw()
        {
            // Act
            Action act = () => "AB".Should().Be("ABC");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*index 1*");
        }

        [Fact]
        public void When_the_expected_string_is_empty_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().Be("");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*index 0*");
        }

        [Fact]
        public void When_the_subject_string_is_empty_it_should_throw()
        {
            // Act
            Action act = () => "".Should().Be("ABC");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*index 0*");
        }

        [Fact]
        public void When_string_is_expected_to_equal_null_it_should_throw()
        {
            // Act
            Action act = () => "AB".Should().Be(null);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be <null>, but found \"AB\".");
        }

        [Fact]
        public void When_string_is_expected_to_be_null_it_should_throw()
        {
            // Act
            Action act = () => "AB".Should().BeNull("we like {0}", "null");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be <null> because we like null, but found \"AB\".");
        }

        [Fact]
        public void When_the_expected_string_is_null_then_it_should_throw()
        {
            // Act
            string someString = null;
            Action act = () => someString.Should().Be("ABC");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString to be \"ABC\", but found <null>.");
        }

        [Fact]
        public void When_the_expected_string_is_the_same_but_with_trailing_spaces_it_should_throw_with_clear_error_message()
        {
            // Act
            Action act = () => "ABC".Should().Be("ABC ", "because I say {0}", "so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be \"ABC \" because I say so, but it misses some extra whitespace at the end.");
        }

        [Fact]
        public void
            When_the_actual_string_is_the_same_as_the_expected_but_with_trailing_spaces_it_should_throw_with_clear_error_message()
        {
            // Act
            Action act = () => "ABC ".Should().Be("ABC", "because I say {0}", "so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be \"ABC\" because I say so, but it has unexpected whitespace at the end.");
        }

        [Fact]
        public void When_two_strings_differ_and_one_of_them_is_long_it_should_display_both_strings_on_separate_line()
        {
            // Act
            Action act = () => "1234567890".Should().Be("0987654321");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("""
                Expected string to be the same string, but they differ at index 0:
                   ↓ (actual)
                  "1234567890"
                  "0987654321"
                   ↑ (expected).
                """);
        }

        [Fact]
        public void When_two_strings_differ_and_one_of_them_is_multiline_it_should_display_both_strings_on_separate_line()
        {
            // Act
            Action act = () => "A\r\nB".Should().Be("A\r\nC");

            // Assert
            act.Should().Throw<XunitException>().Which.Message.Should().Be("""
                Expected string to be the same string, but they differ on line 2 and column 1 (index 3):
                        ↓ (actual)
                  "A\r\nB"
                  "A\r\nC"
                        ↑ (expected).
                """);
        }

        [Fact]
        public void Use_arrows_for_text_longer_than_8_characters()
        {
            const string subject = "this is a long text that differs in between two words";
            const string expected = "this is a long text which differs in between two words";

            // Act
            Action act = () => subject.Should().Be(expected, "because we use arrows now");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("""
                Expected subject to be the same string because we use arrows now, but they differ at index 20:
                                   ↓ (actual)
                  "…is a long text that…"
                  "…is a long text which…"
                                   ↑ (expected).
                """);
        }

        [Fact]
        public void Only_add_ellipsis_for_long_text()
        {
            const string subject = "this is a long text that differs";
            const string expected = "this was too short";

            // Act
            Action act = () => subject.Should().Be(expected, "because we use arrows now");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("""
                Expected subject to be the same string because we use arrows now, but they differ at index 5:
                        ↓ (actual)
                  "this is a long text that…"
                  "this was too short"
                        ↑ (expected).
                """);
        }

        [Theory]
        [InlineData("ThisIsUsedTo Check a difference after 5 characters")]
        [InlineData("ThisIsUsedTo CheckADifferenc e after 15 characters")]
        public void Will_look_for_a_word_boundary_between_5_and_15_characters_before_the_mismatching_index_to_highlight_the_mismatch(string expected)
        {
            const string subject = "ThisIsUsedTo CheckADifferenceInThe WordBoundaryAlgorithm";

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*\"…CheckADifferenceInThe*");
        }

        [Theory]
        [InlineData("ThisIsUsedTo Chec k a difference after 4 characters", "\"…sedTo CheckADifferen")]
        [InlineData("ThisIsUsedTo CheckADifference after 16 characters", "\"…Difference")]
        public void Will_fallback_to_10_characters_if_no_word_boundary_can_be_found_before_the_mismatching_index(
                string expected, string expectedMessagePart)
        {
            const string subject = "ThisIsUsedTo CheckADifferenceInThe WordBoundaryAlgorithm";

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>().WithMessage($"*{expectedMessagePart}*");
        }

        [Theory]
        [InlineData("ThisLongTextIsUsedToCheckADifferenceAtTheEnd after 10 + 5 characters")]
        [InlineData("ThisLongTextIsUsedToCheckADifferen after 10 + 15 characters")]
        public void Will_look_for_a_word_boundary_between_15_and_25_characters_after_the_mismatching_index_to_highlight_the_mismatch(string expected)
        {
            const string subject = "ThisLongTextIsUsedToCheckADifferenceAtTheEndOfThe WordBoundaryAlgorithm";

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*AtTheEndOfThe…\"*");
        }

        [Fact]
        public void An_empty_string_is_always_shorter_than_a_long_text()
        {
            // Act
            Action act = () => "".Should().Be("ThisIsALongText");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*length*15*differs*");
        }

        [Fact]
        public void A_mismatch_below_index_11_includes_all_text_preceding_the_index_in_the_failure()
        {
            // Act
            Action act = () => "This is a long text".Should().Be("This is a text that differs at index 10");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*\"This is a long*");
        }

        [Theory]
        [InlineData("ThisLongTextIsUsedToCheckADifferenceAtTheEndO after 10 + 4 characters", "eAtTheEndOfThe WordB…\"")]
        [InlineData("ThisLongTextIsUsedToCheckADiffere after 10 + 16 characters", "ckADifferenceAtTheEn…\"")]
        public void Will_fallback_to_20_characters_if_no_word_boundary_can_be_found_after_the_mismatching_index(
                string expected, string expectedMessagePart)
        {
            const string subject = "ThisLongTextIsUsedToCheckADifferenceAtTheEndOfThe WordBoundaryAlgorithm";

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>().WithMessage($"*{expectedMessagePart}*");
        }

        [Fact]
        public void Mismatches_in_multiline_text_includes_the_line_number()
        {
            var expectedIndex = 100 + (4 * Environment.NewLine.Length);

            var subject = """
            @startuml
            Alice -> Bob : Authentication Request
            Bob --> Alice : Authentication Response

            Alice -> Bob : Another authentication Request
            Alice <-- Bob : Another authentication Response
            @enduml
            """;

            var expected = """
            @startuml
            Alice -> Bob : Authentication Request
            Bob --> Alice : Authentication Response

            Alice -> Bob : Invalid authentication Request
            Alice <-- Bob : Another authentication Response
            @enduml
            """;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>().WithMessage($"""
                Expected subject to be the same string, but they differ on line 5 and column 16 (index {expectedIndex}):
                             ↓ (actual)
                  "…-> Bob : Another…"
                  "…-> Bob : Invalid…"
                             ↑ (expected).
                """);
        }
    }

    public class NotBe
    {
        [Fact]
        public void When_different_strings_are_expected_to_differ_it_should_not_throw()
        {
            // Arrange
            string actual = "ABC";
            string unexpected = "DEF";

            // Act / Assert
            actual.Should().NotBe(unexpected);
        }

        [Fact]
        public void When_equal_strings_are_expected_to_differ_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().NotBe("ABC", "because we don't like {0}", "ABC");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string not to be \"ABC\" because we don't like ABC.");
        }

        [Fact]
        public void When_non_empty_string_is_not_equal_to_empty_it_should_not_throw()
        {
            // Arrange
            string actual = "ABC";
            string unexpected = "";

            // Act / Assert
            actual.Should().NotBe(unexpected);
        }

        [Fact]
        public void When_empty_string_is_not_supposed_to_be_equal_to_empty_it_should_throw()
        {
            // Arrange
            string actual = "";
            string unexpected = "";

            // Act
            Action act = () => actual.Should().NotBe(unexpected);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected actual not to be \"\".");
        }

        [Fact]
        public void When_valid_string_is_not_supposed_to_be_null_it_should_not_throw()
        {
            // Arrange
            string actual = "ABC";
            string unexpected = null;

            // Act / Assert
            actual.Should().NotBe(unexpected);
        }

        [Fact]
        public void When_null_string_is_not_supposed_to_be_equal_to_null_it_should_throw()
        {
            // Act
            string someString = null;
            Action act = () => someString.Should().NotBe(null);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString not to be <null>.");
        }

        [Fact]
        public void When_null_string_is_not_supposed_to_be_null_it_should_throw()
        {
            // Act
            string someString = null;
            Action act = () => someString.Should().NotBeNull("we don't like {0}", "null");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString not to be <null> because we don't like null.");
        }
    }
}
