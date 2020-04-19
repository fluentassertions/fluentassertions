using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class StringAssertionSpecs
    {
        #region Be

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
        public void When_the_actual_string_is_the_same_as_the_expected_but_with_trailing_spaces_it_should_throw_with_clear_error_message()
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
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be*\"0987654321\", but*\"1234567890\" differs near \"123\" (index 0).");
        }

        [Fact]
        public void When_two_strings_differ_and_one_of_them_is_multiline_it_should_display_both_strings_on_separate_line()
        {
            // Act
            Action act = () => "A\r\nB".Should().Be("A\r\nC");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be \r\n\"A\\r\\nC\", but \r\n\"A\\r\\nB\" differs near \"B\" (index 3).");
        }

        #endregion

        #region Not Be

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

        #endregion

        #region Be One Of

        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            string value = "abc";

            // Act
            Action action = () => value.Should().BeOneOf("def", "xyz");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {\"def\", \"xyz\"}, but found \"abc\".");
        }

        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw_with_descriptive_message()
        {
            // Arrange
            string value = "abc";

            // Act
            Action action = () => value.Should().BeOneOf(new[] { "def", "xyz" }, "because those are the valid values");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {\"def\", \"xyz\"} because those are the valid values, but found \"abc\".");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed()
        {
            // Arrange
            string value = "abc";

            // Act
            Action action = () => value.Should().BeOneOf("abc", "def", "xyz");

            // Assert
            action.Should().NotThrow();
        }

        #endregion

        #region Match

        [Fact]
        public void When_a_string_does_not_match_a_wildcard_pattern_it_should_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().Match("h*earth!", "that's the universal greeting");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to match*\"h*earth!\" because that's the universal greeting, but*\"hello world!\" does not.");
        }

        [Fact]
        public void When_a_string_does_match_a_wildcard_pattern_it_should_not_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().Match("h*world?");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_string_does_not_match_a_wildcard_pattern_with_escaped_markers_it_should_throw()
        {
            // Arrange
            string subject = "What! Are you deaf!";

            // Act
            Action act = () => subject.Should().Match(@"What\? Are you deaf\?");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to match*\"What\\? Are you deaf\\?\", but*\"What! Are you deaf!\" does not.");
        }

        [Fact]
        public void When_a_string_does_match_a_wildcard_pattern_but_differs_in_casing_it_should_throw()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().Match("*World*");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to match*\"*World*\", but*\"hello world\" does not.");
        }

        #endregion

        #region Not Match

        [Fact]
        public void When_a_string_does_not_match_a_pattern_and_it_shouldnt_it_should_not_throw()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().NotMatch("*World*");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_string_does_match_a_pattern_but_it_shouldnt_it_should_throw()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().NotMatch("*world*", "because that's illegal");

            // Assert
            act
                .Should().Throw<XunitException>().WithMessage(
                    "Did not expect subject to match*\"*world*\" because that's illegal, but*\"hello world\" matches.");
        }

        #endregion

        #region Match Equivalent Of

        [Fact]
        public void When_a_string_does_not_match_the_equivalent_of_a_wildcard_pattern_it_should_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().MatchEquivalentOf("h*earth!", "that's the universal greeting");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject to match the equivalent of*\"h*earth!\" " +
                "because that's the universal greeting, but*\"hello world!\" does not.");
        }

        [Fact]
        public void When_a_string_does_match_the_equivalent_of_a_wildcard_pattern_it_should_not_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().MatchEquivalentOf("h*WORLD?");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_string_with_newline_matches_the_equivalent_of_a_wildcard_pattern_it_should_not_throw()
        {
            // Arrange
            string subject = "hello\r\nworld!";

            // Act
            Action act = () => subject.Should().MatchEquivalentOf("helloworld!");

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region Not Match Equivalent Of

        [Fact]
        public void When_a_string_is_not_equivalent_to_a_pattern_and_that_is_expected_it_should_not_throw()
        {
            // Arrange
            string subject = "Hello Earth";

            // Act
            Action act = () => subject.Should().NotMatchEquivalentOf("*World*");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_string_does_match_the_equivalent_of_a_pattern_but_it_shouldnt_it_should_throw()
        {
            // Arrange
            string subject = "hello WORLD";

            // Act
            Action act = () => subject.Should().NotMatchEquivalentOf("*world*", "because that's illegal");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Did not expect subject to match the equivalent of*\"*world*\" because that's illegal, " +
                "but*\"hello WORLD\" matches.");
        }

        [Fact]
        public void When_a_string_with_newlines_does_match_the_equivalent_of_a_pattern_but_it_shouldnt_it_should_throw()
        {
            // Arrange
            string subject = "hello\r\nworld!";

            // Act
            Action act = () => subject.Should().NotMatchEquivalentOf("helloworld!");

            // Assert
            act.Should().Throw<XunitException>();
        }

        #endregion

        #region Match Regex

        [Fact]
        public void When_a_string_matches_a_regular_expression_it_should_not_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            // ReSharper disable once StringLiteralTypo
            Action act = () => subject.Should().MatchRegex("h.*\\sworld.$");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_a_string_does_not_match_a_regular_expression_it_should_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().MatchRegex("h.*\\sworld?$", "that's the universal greeting");

            // Assert
            act.Should().Throw<XunitException>()
              .WithMessage("Expected subject to match regex*\"h.*\\sworld?$\" because that's the universal greeting, but*\"hello world!\" does not match.");
        }

        [Fact]
        public void When_a_null_string_is_matched_against_a_regex_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = null;

            // Act
            Action act = () => subject.Should().MatchRegex(".*", "because it should be a string");

            // Assert
            act.Should().Throw<XunitException>()
             .WithMessage("Expected subject to match regex*\".*\" because it should be a string, but it was <null>.");
        }

        [Fact]
        public void When_a_string_is_matched_against_a_null_regex_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().MatchRegex(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithMessage("Cannot match string against <null>.*")
               .And.ParamName.Should().Be("regularExpression");
        }

        [Fact]
        public void When_a_string_is_matched_against_an_invalid_regex_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world!";
            string invalidRegex = ".**"; // Use local variable for this invalid regex to avoid static R# analysis errors

            // Act
            Action act = () => subject.Should().MatchRegex(invalidRegex);

            // Assert
            act.Should().Throw<XunitException>()
             .WithMessage("Cannot match subject against \".**\" because it is not a valid regular expression.*");
        }

        [Fact]
        public void When_a_string_is_matched_against_an_invalid_regex_it_should_only_have_one_failure_message()
        {
            // Arrange
            string subject = "hello world!";
            string invalidRegex = ".**"; // Use local variable for this invalid regex to avoid static R# analysis errors

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                subject.Should().MatchRegex(invalidRegex);
            };

            // Assert
            act.Should().Throw<XunitException>()
                .Which.Message.Should().Contain("is not a valid regular expression")
                    .And.NotContain("does not match");
        }

        #endregion

        #region Not Match Regex

        [Fact]
        public void When_a_string_does_not_match_a_regular_expression_and_it_shouldnt_it_should_not_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().NotMatchRegex(".*earth.*");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_string_matches_a_regular_expression_but_it_shouldnt_it_should_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().NotMatchRegex(".*world.*", "because that's illegal");

            // Assert
            act.Should().Throw<XunitException>()
              .WithMessage("Did not expect subject to match regex*\".*world.*\" because that's illegal, but*\"hello world!\" matches.");
        }

        [Fact]
        public void When_a_null_string_is_negatively_matched_against_a_regex_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = null;

            // Act
            Action act = () => subject.Should().NotMatchRegex(".*", "because it should not be a string");

            // Assert
            act.Should().Throw<XunitException>()
             .WithMessage("Expected subject to not match regex*\".*\" because it should not be a string, but it was <null>.");
        }

        [Fact]
        public void When_a_string_is_negatively_matched_against_a_null_regex_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().NotMatchRegex(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithMessage("Cannot match string against <null>.*")
               .And.ParamName.Should().Be("regularExpression");
        }

        [Fact]
        public void When_a_string_is_negatively_matched_against_an_invalid_regex_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world!";
            string invalidRegex = ".**"; // Use local variable for this invalid regex to avoid static R# analysis errors

            // Act
            Action act = () => subject.Should().NotMatchRegex(invalidRegex);

            // Assert
            act.Should().Throw<XunitException>()
             .WithMessage("Cannot match subject against \".**\" because it is not a valid regular expression.*");
        }

        [Fact]
        public void When_a_string_is_negatively_matched_against_an_invalid_regex_it_only_contain_one_failure_message()
        {
            // Arrange
            string subject = "hello world!";
            string invalidRegex = ".**"; // Use local variable for this invalid regex to avoid static R# analysis errors

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                subject.Should().NotMatchRegex(invalidRegex);
            };

            // Assert
            act.Should().Throw<XunitException>()
                .Which.Message.Should().Contain("is not a valid regular expression")
                    .And.NotContain("matches");
        }

        #endregion

        #region Start With

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
        public void When_string_does_not_start_with_expected_phrase_and_one_of_them_is_long_it_should_display_both_strings_on_separate_line()
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
        public void When_string_start_is_compared_with_empty_string_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().StartWith("");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot compare start of string with empty string.*");
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

        #endregion

        #region Not Start With

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
            action.Should().Throw<ArgumentException>().WithMessage(
                "Cannot compare start of string with empty string.*");
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

        #endregion

        #region End With

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
            Action act = () => "ABC".Should().EndWith("AB", "it should");

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
        public void When_string_ending_is_compared_with_empty_string_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().EndWith("");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot compare string end with empty string.*");
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
            // Act
            string someString = null;
            Action act = () => someString.Should().EndWith("ABC");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString <null> to end with \"ABC\".");
        }

        #endregion

        #region Not End With

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
                value.Should().NotEndWith("BC", "because of some reason");

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
            action.Should().Throw<ArgumentException>().WithMessage(
                "Cannot compare end of string with empty string.*");
        }

        [Fact]
        public void When_asserting_string_does_not_end_with_a_value_and_actual_value_is_null_it_should_throw()
        {
            // Act
            string someString = null;
            Action act = () => someString.Should().NotEndWith("ABC");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString that does not end with \"ABC\", but found <null>.");
        }

        #endregion

        #region Start With Equivalent

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
        public void When_start_of_string_does_not_meet_equivalent_and_one_of_them_is_long_it_should_display_both_strings_on_separate_line()
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
        public void When_start_of_string_is_compared_with_equivalent_of_empty_string_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().StartWithEquivalentOf("");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot compare string start equivalence with empty string.*");
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

        #endregion

        #region Not Start With Equivalent

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
        public void When_asserting_string_does_not_start_with_equivalent_of_a_value_but_it_does_it_should_fail_with_a_descriptive_message()
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
            action.Should().Throw<ArgumentException>().WithMessage(
                "Cannot compare start of string with empty string.*");
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

        #endregion

        #region End With Equivalent

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
        public void When_end_of_string_is_compared_with_equivalent_of_empty_string_it_should_throw()
        {
            // Act
            Action act = () => "ABC".Should().EndWithEquivalentOf("");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot compare string end equivalence with empty string.*");
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
            // Act
            string someString = null;
            Action act = () => someString.Should().EndWithEquivalentOf("abC");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString that ends with equivalent of \"abC\", but found <null>.");
        }

        #endregion

        #region Not End With Equivalent

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
        public void When_asserting_string_does_not_end_with_equivalent_of_a_value_but_it_does_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            string value = "ABC";

            // Act
            Action action = () =>
                value.Should().NotEndWithEquivalentOf("Bc", "because of some reason");

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
            action.Should().Throw<ArgumentException>().WithMessage(
                "Cannot compare end of string with empty string.*");
        }

        [Fact]
        public void When_asserting_string_does_not_end_with_equivalent_of_a_value_and_actual_value_is_null_it_should_throw()
        {
            // Act
            string someString = null;
            Action act = () => someString.Should().NotEndWithEquivalentOf("Abc");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString that does not end with equivalent of \"Abc\", but found <null>.");
        }

        #endregion

        #region Contain

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_string_contains_the_expected_string_it_should_not_throw()
        {
            // Arrange
            string actual = "ABCDEF";
            string expectedSubstring = "BCD";

            // Act / Assert
            actual.Should().Contain(expectedSubstring);
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_string_does_not_contain_an_expected_string_it_should_throw()
        {
            // Act
            Action act = () => "ABCDEF".Should().Contain("XYZ", "that is {0}", "required");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string \"ABCDEF\" to contain \"XYZ\" because that is required.");
        }

        [Fact]
        public void When_containment_is_asserted_against_null_it_should_throw()
        {
            // Act
            Action act = () => "a".Should().Contain(null);

            // Assert
            act
                .Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert string containment against <null>.*")
                .And.ParamName.Should().Be("expected");
        }

        [Fact]
        public void When_containment_is_asserted_against_an_empty_string_it_should_throw()
        {
            // Act
            Action act = () => "a".Should().Contain("");

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot assert string containment against an empty string.*")
                .And.ParamName.Should().Be("expected");
        }

        [Fact]
        public void When_string_containment_is_asserted_and_actual_value_is_null_then_it_should_throw()
        {
            // Act
            string someString = null;
            Action act = () => someString.Should().Contain("XYZ", "that is {0}", "required");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someString <null> to contain \"XYZ\" because that is required.");
        }

        #region Exactly

        [Fact]
        public void When_string_containment_once_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_throw()
        {
            // Arrange
            string actual = "ABCDEF";
            string expectedSubstring = "XYS";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, Exactly.Once(), "that is {0}", "required");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"ABCDEF\" to contain \"XYS\" exactly 1 time because that is required, but found it 0 times.");
        }

        [Fact]
        public void When_containment_once_is_asserted_against_null_it_should_throw_earlier()
        {
            // Arrange
            string actual = "a";
            string expectedSubstring = null;

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, Exactly.Once());

            // Assert
            act
                .Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert string containment against <null>.*");
        }

        [Fact]
        public void When_string_containment_once_is_asserted_and_actual_value_is_null_then_it_should_throw()
        {
            // Arrange
            string actual = null;
            string expectedSubstring = "XYZ";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, Exactly.Once());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * <null> to contain \"XYZ\" exactly 1 time, but found it 0 times.");
        }

        [Fact]
        public void When_string_containment_exactly_is_asserted_and_expected_value_is_negative_it_should_throw()
        {
            // Arrange
            string actual = "ABCDEBCDF";
            string expectedSubstring = "BCD";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, Exactly.Times(-1));

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Expected count cannot be negative.*");
        }

        [Fact]
        public void When_string_containment_exactly_is_asserted_and_actual_value_contains_the_expected_string_exactly_expected_times_it_should_not_throw()
        {
            // Arrange
            string actual = "ABCDEBCDF";
            string expectedSubstring = "BCD";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, Exactly.Times(2));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_exactly_is_asserted_and_actual_value_contains_the_expected_string_but_not_exactly_expected_times_it_should_throw()
        {
            // Arrange
            string actual = "ABCDEBCDF";
            string expectedSubstring = "BCD";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, Exactly.Times(3));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"ABCDEBCDF\" to contain \"BCD\" exactly 3 times, but found it 2 times.");
        }

        #endregion

        #region AtLeast

        [Fact]
        public void When_string_containment_at_least_is_asserted_and_actual_value_contains_the_expected_string_at_least_expected_times_it_should_not_throw()
        {
            // Arrange
            string actual = "ABCDEBCDF";
            string expectedSubstring = "BCD";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, AtLeast.Times(2));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_at_least_is_asserted_and_actual_value_contains_the_expected_string_but_not_at_least_expected_times_it_should_throw()
        {
            // Arrange
            string actual = "ABCDEBCDF";
            string expectedSubstring = "BCD";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, AtLeast.Times(3));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"ABCDEBCDF\" to contain \"BCD\" at least 3 times, but found it 2 times.");
        }

        [Fact]
        public void When_string_containment_at_least_once_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_throw_earlier()
        {
            // Arrange
            string actual = "ABCDEF";
            string expectedSubstring = "XYS";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, AtLeast.Once());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"ABCDEF\" to contain \"XYS\" at least 1 time, but found it 0 times.");
        }

        [Fact]
        public void When_string_containment_at_least_once_is_asserted_and_actual_value_is_null_then_it_should_throw()
        {
            // Arrange
            string actual = null;
            string expectedSubstring = "XYZ";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, AtLeast.Once());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * <null> to contain \"XYZ\" at least 1 time, but found it 0 times.");
        }

        #endregion

        #region MoreThan

        [Fact]
        public void When_string_containment_more_than_is_asserted_and_actual_value_contains_the_expected_string_more_than_expected_times_it_should_not_throw()
        {
            // Arrange
            string actual = "ABCDEBCDF";
            string expectedSubstring = "BCD";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, MoreThan.Times(1));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_more_than_is_asserted_and_actual_value_contains_the_expected_string_but_not_more_than_expected_times_it_should_throw()
        {
            // Arrange
            string actual = "ABCDEBCDF";
            string expectedSubstring = "BCD";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, MoreThan.Times(2));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"ABCDEBCDF\" to contain \"BCD\" more than 2 times, but found it 2 times.");
        }

        [Fact]
        public void When_string_containment_more_than_once_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_throw()
        {
            // Arrange
            string actual = "ABCDEF";
            string expectedSubstring = "XYS";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, MoreThan.Once());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"ABCDEF\" to contain \"XYS\" more than 1 time, but found it 0 times.");
        }

        [Fact]
        public void When_string_containment_more_than_once_is_asserted_and_actual_value_is_null_then_it_should_throw()
        {
            // Arrange
            string actual = null;
            string expectedSubstring = "XYZ";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, MoreThan.Once());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * <null> to contain \"XYZ\" more than 1 time, but found it 0 times.");
        }

        #endregion

        #region AtMost

        [Fact]
        public void When_string_containment_at_most_is_asserted_and_actual_value_contains_the_expected_string_at_most_expected_times_it_should_not_throw()
        {
            // Arrange
            string actual = "ABCDEBCDF";
            string expectedSubstring = "BCD";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, AtMost.Times(2));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_at_most_is_asserted_and_actual_value_contains_the_expected_string_but_not_at_most_expected_times_it_should_throw()
        {
            // Arrange
            string actual = "ABCDEBCDF";
            string expectedSubstring = "BCD";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, AtMost.Times(1));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"ABCDEBCDF\" to contain \"BCD\" at most 1 time, but found it 2 times.");
        }

        [Fact]
        public void When_string_containment_at_most_once_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_not_throw()
        {
            // Arrange
            string actual = "ABCDEF";
            string expectedSubstring = "XYS";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, AtMost.Once());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_at_most_once_is_asserted_and_actual_value_is_null_then_it_should_not_throw()
        {
            // Arrange
            string actual = null;
            string expectedSubstring = "XYZ";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, AtMost.Once());

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region LessThan

        [Fact]
        public void When_string_containment_less_than_is_asserted_and_actual_value_contains_the_expected_string_less_than_expected_times_it_should_not_throw()
        {
            // Arrange
            string actual = "ABCDEBCDF";
            string expectedSubstring = "BCD";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, LessThan.Times(3));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_less_than_is_asserted_and_actual_value_contains_the_expected_string_but_not_less_than_expected_times_it_should_throw()
        {
            // Arrange
            string actual = "ABCDEBCDF";
            string expectedSubstring = "BCD";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, LessThan.Times(2));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"ABCDEBCDF\" to contain \"BCD\" less than 2 times, but found it 2 times.");
        }

        [Fact]
        public void When_string_containment_less_than_twice_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_not_throw()
        {
            // Arrange
            string actual = "ABCDEF";
            string expectedSubstring = "XYS";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, LessThan.Twice());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_less_than_once_is_asserted_and_actual_value_is_null_then_it_should_not_throw()
        {
            // Arrange
            string actual = null;
            string expectedSubstring = "XYZ";

            // Act
            Action act = () => actual.Should().Contain(expectedSubstring, LessThan.Twice());

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #endregion

        #region Be Equivalent To

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
        public void When_the_expected_string_is_equivalent_to_the_actual_string_but_with_trailing_spaces_it_should_throw_with_clear_error_message()
        {
            // Act
            Action act = () => "ABC".Should().BeEquivalentTo("abc ", "because I say {0}", "so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be equivalent to \"abc \" because I say so, but it misses some extra whitespace at the end.");
        }

        [Fact]
        public void When_the_actual_string_equivalent_to_the_expected_but_with_trailing_spaces_it_should_throw_with_clear_error_message()
        {
            // Act
            Action act = () => "ABC ".Should().BeEquivalentTo("abc", "because I say {0}", "so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected string to be equivalent to \"abc\" because I say so, but it has unexpected whitespace at the end.");
        }

        #endregion

        #region Not Be Equivalent To

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

        #endregion

        #region ContainAll

        [Fact]
        public void When_containment_of_all_strings_in_a_null_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().ContainAll(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot*containment*null*")
                .And.ParamName.Should().Be("values");
        }

        [Fact]
        public void When_containment_of_all_strings_in_an_empty_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().ContainAll();

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Cannot*containment*empty*")
                .And.ParamName.Should().Be("values");
        }

        [Fact]
        public void When_containment_of_all_strings_in_a_collection_is_asserted_and_all_strings_are_present_it_should_succeed()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().ContainAll(red, green, yellow);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_containment_of_all_strings_in_a_collection_is_asserted_and_equivalent_but_not_exact_matches_exist_for_all_it_should_throw()
        {
            // Arrange
            const string redLowerCase = "red";
            const string redUpperCase = "RED";
            const string greenWithoutWhitespace = "green";
            const string greenWithWhitespace = "  green ";
            var testString = $"{redLowerCase} {greenWithoutWhitespace}";

            // Act
            Action act = () => testString.Should().ContainAll(redUpperCase, greenWithWhitespace);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain*{redUpperCase}*{greenWithWhitespace}*");
        }

        [Fact]
        public void When_containment_of_all_strings_in_a_collection_is_asserted_and_none_of_the_strings_are_present_it_should_throw()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string blue = "blue";
            var testString = $"{red} {green}";

            // Act
            Action act = () => testString.Should().ContainAll(yellow, blue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain*{yellow}*{blue}*");
        }

        [Fact]
        public void When_containment_of_all_strings_in_a_collection_is_asserted_with_reason_and_assertion_fails_then_failure_message_should_contain_reason()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string blue = "blue";
            var testString = $"{red} {green}";

            const string because = "some {0} reason";
            var becauseArgs = new[] { "special" };
            var expectedErrorReason = string.Format(because, becauseArgs);

            // Act
            Action act = () => testString.Should().ContainAll(new[] { yellow, blue }, because, becauseArgs);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain*{yellow}*{blue}*because {expectedErrorReason}*");
        }

        [Fact]
        public void When_containment_of_all_strings_in_a_collection_is_asserted_and_only_some_of_the_strings_are_present_it_should_throw()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string blue = "blue";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().ContainAll(red, blue, green);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain*{blue}*");
        }

        #endregion

        #region ContainAny

        [Fact]
        public void When_containment_of_any_string_in_a_null_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().ContainAny(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot*containment*null*")
                .And.ParamName.Should().Be("values");
        }

        [Fact]
        public void When_containment_of_any_string_in_an_empty_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().ContainAny();

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Cannot*containment*empty*")
                .And.ParamName.Should().Be("values");
        }

        [Fact]
        public void When_containment_of_any_string_in_a_collection_is_asserted_and_all_of_the_strings_are_present_it_should_succeed()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().ContainAny(red, green, yellow);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_containment_of_any_string_in_a_collection_is_asserted_and_only_some_of_the_strings_are_present_it_should_succeed()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string blue = "blue";
            var testString = $"{red} {green}";

            // Act
            Action act = () => testString.Should().ContainAny(red, blue, green);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_containment_of_any_string_in_a_collection_is_asserted_and_none_of_the_strings_are_present_it_should_throw()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string blue = "blue";
            const string purple = "purple";
            var testString = $"{red} {green}";

            // Act
            Action act = () => testString.Should().ContainAny(blue, purple);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain at least one of*{blue}*{purple}*");
        }

        [Fact]
        public void When_containment_of_any_string_in_a_collection_is_asserted_and_there_are_equivalent_but_not_exact_matches_it_should_throw()
        {
            // Arrange
            const string redLowerCase = "red";
            const string redUpperCase = "RED";
            const string greenWithoutWhitespace = "green";
            const string greenWithWhitespace = "   green";
            var testString = $"{redLowerCase} {greenWithoutWhitespace}";

            // Act
            Action act = () => testString.Should().ContainAny(redUpperCase, greenWithWhitespace);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain at least one of*{redUpperCase}*{greenWithWhitespace}*");
        }

        [Fact]
        public void When_containment_of_any_string_in_a_collection_is_asserted_with_reason_and_assertion_fails_then_failure_message_contains_reason()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string blue = "blue";
            const string purple = "purple";
            var testString = $"{red} {green}";

            const string because = "some {0} reason";
            var becauseArgs = new[] { "special" };
            var expectedErrorReason = string.Format(because, becauseArgs);

            // Act
            Action act = () => testString.Should().ContainAny(new[] { blue, purple }, because, becauseArgs);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain at least one of*{blue}*{purple}*because {expectedErrorReason}*");
        }

        #endregion

        #region Not Contain

        [Fact]
        public void When_string_does_not_contain_the_unexpected_string_it_should_succeed()
        {
            // Act
            Action act = () => "a".Should().NotContain("A");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_string_contains_unexpected_fragment_it_should_throw()
        {
            // Act
            Action act = () => "abcd".Should().NotContain("bc", "it was not expected {0}", "today");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect string \"abcd\" to contain \"bc\" because it was not expected today.");
        }

        [Fact]
        public void When_exclusion_is_asserted_against_null_it_should_throw()
        {
            // Act
            Action act = () => "a".Should().NotContain(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert string containment against <null>.*")
                .And.ParamName.Should().Be("unexpected");
        }

        [Fact]
        public void When_exclusion_is_asserted_against_an_empty_string_it_should_throw()
        {
            // Act
            Action act = () => "a".Should().NotContain("");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Cannot assert string containment against an empty string.*")
                .And.ParamName.Should().Be("unexpected");
        }

        #endregion

        #region NotContainAll

        [Fact]
        public void When_exclusion_of_all_strings_in_null_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().NotContainAll(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot*containment*null*")
                .And.ParamName.Should().Be("values");
        }

        [Fact]
        public void When_exclusion_of_all_strings_in_an_empty_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().NotContainAll();

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Cannot*containment*empty*")
                .And.ParamName.Should().Be("values");
        }

        [Fact]
        public void When_exclusion_of_all_strings_in_a_collection_is_asserted_and_all_strings_in_collection_are_present_it_should_throw()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().NotContainAll(red, green, yellow);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*not*{testString}*contain all*{red}*{green}*{yellow}*");
        }

        [Fact]
        public void When_exclusion_of_all_strings_is_asserted_with_reason_and_assertion_fails_then_error_message_contains_reason()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            var testString = $"{red} {green} {yellow}";

            const string because = "some {0} reason";
            var becauseArgs = new[] { "special" };
            var expectedErrorReason = string.Format(because, becauseArgs);

            // Act
            Action act = () => testString.Should().NotContainAll(new[] { red, green, yellow }, because, becauseArgs);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*not*{testString}*contain all*{red}*{green}*{yellow}*because*{expectedErrorReason}*");
        }

        [Fact]
        public void When_exclusion_of_all_strings_in_a_collection_is_asserted_and_only_some_of_the_strings_in_collection_are_present_it_should_succeed()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string purple = "purple";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().NotContainAll(red, green, yellow, purple);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_exclusion_of_all_strings_in_a_collection_is_asserted_and_none_of_the_strings_in_the_collection_are_present_it_should_succeed()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string purple = "purple";
            var testString = $"{red} {green}";

            // Act
            Action act = () => testString.Should().NotContainAll(yellow, purple);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_exclusion_of_all_strings_in_a_collection_is_asserted_and_equivalent_but_not_exact_strings_are_present_in_collection_it_should_succeed()
        {
            // Arrange
            const string redWithoutWhitespace = "red";
            const string redWithWhitespace = "  red ";
            const string lowerCaseGreen = "green";
            const string upperCaseGreen = "GREEN";
            var testString = $"{redWithoutWhitespace} {lowerCaseGreen}";

            // Act
            Action act = () => testString.Should().NotContainAll(redWithWhitespace, upperCaseGreen);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region NotContainAny
        [Fact]
        public void When_exclusion_of_any_string_in_null_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().NotContainAny(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot*containment*null*")
                .And.ParamName.Should().Be("values");
        }

        [Fact]
        public void When_exclusion_of_any_string_in_an_empty_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().NotContainAny();

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Cannot*containment*empty*")
                .And.ParamName.Should().Be("values");
        }

        [Fact]
        public void When_exclusion_of_any_string_in_a_collection_is_asserted_and_all_of_the_strings_are_present_it_should_throw()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().NotContainAny(red, green, yellow);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*not*{testString}*contain any*{red}*{green}*{yellow}*");
        }

        [Fact]
        public void When_exclusion_of_any_string_in_a_collection_is_asserted_and_only_some_of_the_strings_are_present_it_should_throw()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string purple = "purple";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().NotContainAny(red, purple, green);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*not*{testString}*contain any*{red}*{green}*");
        }

        [Fact]
        public void When_exclusion_of_any_strings_is_asserted_with_reason_and_assertion_fails_then_error_message_contains_reason()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            var testString = $"{red} {green} {yellow}";

            const string because = "some {0} reason";
            var becauseArgs = new[] { "special" };
            var expectedErrorReason = string.Format(because, becauseArgs);

            // Act
            Action act = () => testString.Should().NotContainAny(new[] { red }, because, becauseArgs);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*not*{testString}*contain any*{red}*because*{expectedErrorReason}*");
        }

        [Fact]
        public void When_exclusion_of_any_string_in_a_collection_is_asserted_and_there_are_equivalent_but_not_exact_matches_it_should_succeed()
        {
            // Arrange
            const string redLowerCase = "red";
            const string redUpperCase = "RED";
            const string greenWithoutWhitespace = "green";
            const string greenWithWhitespace = " green  ";
            var testString = $"{redLowerCase} {greenWithoutWhitespace}";

            // Act
            Action act = () => testString.Should().NotContainAny(redUpperCase, greenWithWhitespace);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_exclusion_of_any_string_in_a_collection_is_asserted_and_none_of_the_strings_are_present_it_should_succeed()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string purple = "purple";
            var testString = $"{red} {green}";

            // Act
            Action act = () => testString.Should().NotContainAny(yellow, purple);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region Not Contain Equivalent Of

        [Fact]
        public void Should_fail_when_asserting_string_does_not_contain_equivalent_of_null()
        {
            // Act
            Action act = () =>
                "a".Should().NotContainEquivalentOf(null);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect string to contain equivalent of <null> but found \"a\".");
        }

        [Fact]
        public void Should_fail_when_asserting_string_does_not_contain_equivalent_of_empty()
        {
            // Act
            Action act = () =>
                "a".Should().NotContainEquivalentOf("");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect string to contain equivalent of \"\" but found \"a\".");
        }

        [Fact]
        public void Should_fail_when_asserting_string_does_not_contain_equivalent_of_another_string_but_it_does()
        {
            // Act
            Action act = () =>
                "Hello, world!".Should().NotContainEquivalentOf(", worLD!");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect string to contain equivalent of \", worLD!\" but found \"Hello, world!\".");
        }

        [Fact]
        public void Should_succeed_when_asserting_string_does_not_contain_equivalent_of_another_string()
        {
            // Act
            Action act = () =>
                "aAa".Should().NotContainEquivalentOf("aa ");

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region Contain Equivalent Of

        [InlineData("aa", "A")]
        // ReSharper disable once StringLiteralTypo
        [InlineData("aCCa", "acca")]
        [Theory]
        public void Should_pass_when_contains_equivalent_of(string actual, string equivalentSubstring)
        {
            // Assert
            actual.Should().ContainEquivalentOf(equivalentSubstring);
        }

        [Fact]
        public void Should_fail_contain_equivalent_of_when_not_contains()
        {
            // Act
            Action act = () =>
                "a".Should().ContainEquivalentOf("aa");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected string \"a\" to contain the equivalent of \"aa\".");
        }

        [Fact]
        public void Should_throw_when_null_equivalent_is_expected()
        {
            // Act
            Action act = () =>
                "a".Should().ContainEquivalentOf(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert string containment against <null>.*")
                .And.ParamName.Should().Be("expected");
        }

        [Fact]
        public void Should_throw_when_empty_equivalent_is_expected()
        {
            // Act
            Action act = () =>
                "a".Should().ContainEquivalentOf("");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Cannot assert string containment against an empty string.*")
                .And.ParamName.Should().Be("expected");
        }

        #region Exactly

        [Fact]
        public void When_containment_equivalent_of_once_is_asserted_against_null_it_should_throw_earlier()
        {
            // Arrange
            string actual = "a";
            string expectedSubstring = null;

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, Exactly.Once());

            // Assert
            act
                .Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert string containment against <null>.*");
        }

        [Fact]
        public void When_string_containment_equivalent_of_exactly_once_is_asserted_and_actual_value_is_null_then_it_should_throw_earlier()
        {
            // Arrange
            string actual = null;
            string expectedSubstring = "XyZ";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, Exactly.Once(), "that is {0}", "required");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * <null> to contain equivalent of \"XyZ\" exactly 1 time because that is required, but found it 0 times.");
        }

        [Fact]
        public void When_string_containment_equivalent_of_exactly_is_asserted_and_actual_value_contains_the_expected_string_exactly_expected_times_it_should_not_throw()
        {
            // Arrange
            string actual = "abCDEBcDF";
            string expectedSubstring = "Bcd";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, Exactly.Times(2));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_equivalent_of_exactly_is_asserted_and_actual_value_contains_the_expected_string_but_not_exactly_expected_times_it_should_throw()
        {
            // Arrange
            string actual = "abCDEBcDF";
            string expectedSubstring = "Bcd";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, Exactly.Times(3));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"abCDEBcDF\" to contain equivalent of \"Bcd\" exactly 3 times, but found it 2 times.");
        }

        [Fact]
        public void When_string_containment_equivalent_of_exactly_once_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_throw()
        {
            // Arrange
            string actual = "abCDEf";
            string expectedSubstring = "xyS";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, Exactly.Once());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"abCDEf\" to contain equivalent of \"xyS\" exactly 1 time, but found it 0 times.");
        }

        [Fact]
        public void When_containment_equivalent_of_exactly_once_is_asserted_against_an_empty_string_it_should_throw_earlier()
        {
            // Arrange
            string actual = "a";
            string expectedSubstring = "";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, Exactly.Once());

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot assert string containment against an empty string.*");
        }

        #endregion

        #region AtLeast

        [Fact]
        public void When_string_containment_equivalent_of_at_least_is_asserted_and_actual_value_contains_the_expected_string_at_least_expected_times_it_should_not_throw()
        {
            // Arrange
            string actual = "abCDEBcDF";
            string expectedSubstring = "Bcd";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, AtLeast.Times(2));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_equivalent_of_at_least_is_asserted_and_actual_value_contains_the_expected_string_but_not_at_least_expected_times_it_should_throw()
        {
            // Arrange
            string actual = "abCDEBcDF";
            string expectedSubstring = "Bcd";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, AtLeast.Times(3));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"abCDEBcDF\" to contain equivalent of \"Bcd\" at least 3 times, but found it 2 times.");
        }

        [Fact]
        public void When_string_containment_equivalent_of_at_least_once_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_throw_earlier()
        {
            // Arrange
            string actual = "abCDEf";
            string expectedSubstring = "xyS";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, AtLeast.Once());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"abCDEf\" to contain equivalent of \"xyS\" at least 1 time, but found it 0 times.");
        }

        [Fact]
        public void When_string_containment_equivalent_of_at_least_once_is_asserted_and_actual_value_is_null_then_it_should_throw_earlier()
        {
            // Arrange
            string actual = null;
            string expectedSubstring = "XyZ";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, AtLeast.Once());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * <null> to contain equivalent of \"XyZ\" at least 1 time, but found it 0 times.");
        }

        #endregion

        #region MoreThan

        [Fact]
        public void When_string_containment_equivalent_of_more_than_is_asserted_and_actual_value_contains_the_expected_string_more_than_expected_times_it_should_not_throw()
        {
            // Arrange
            string actual = "abCDEBcDF";
            string expectedSubstring = "Bcd";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, MoreThan.Times(1));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_equivalent_of_more_than_is_asserted_and_actual_value_contains_the_expected_string_but_not_more_than_expected_times_it_should_throw()
        {
            // Arrange
            string actual = "abCDEBcDF";
            string expectedSubstring = "Bcd";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, MoreThan.Times(2));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"abCDEBcDF\" to contain equivalent of \"Bcd\" more than 2 times, but found it 2 times.");
        }

        [Fact]
        public void When_string_containment_equivalent_of_more_than_once_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_throw_earlier()
        {
            // Arrange
            string actual = "abCDEf";
            string expectedSubstring = "xyS";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, MoreThan.Once());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"abCDEf\" to contain equivalent of \"xyS\" more than 1 time, but found it 0 times.");
        }

        [Fact]
        public void When_string_containment_equivalent_of_more_than_once_is_asserted_and_actual_value_is_null_then_it_should_throw_earlier()
        {
            // Arrange
            string actual = null;
            string expectedSubstring = "XyZ";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, MoreThan.Once());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * <null> to contain equivalent of \"XyZ\" more than 1 time, but found it 0 times.");
        }

        #endregion

        #region AtMost

        [Fact]
        public void When_string_containment_equivalent_of_at_most_is_asserted_and_actual_value_contains_the_expected_string_at_most_expected_times_it_should_not_throw()
        {
            // Arrange
            string actual = "abCDEBcDF";
            string expectedSubstring = "Bcd";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, AtMost.Times(2));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_equivalent_of_at_most_is_asserted_and_actual_value_contains_the_expected_string_but_not_at_most_expected_times_it_should_throw()
        {
            // Arrange
            string actual = "abCDEBcDF";
            string expectedSubstring = "Bcd";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, AtMost.Times(1));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"abCDEBcDF\" to contain equivalent of \"Bcd\" at most 1 time, but found it 2 times.");
        }

        [Fact]
        public void When_string_containment_equivalent_of_at_most_once_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_not_throw()
        {
            // Arrange
            string actual = "abCDEf";
            string expectedSubstring = "xyS";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, AtMost.Once());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_equivalent_of_at_most_once_is_asserted_and_actual_value_is_null_then_it_should_not_throw()
        {
            // Arrange
            string actual = null;
            string expectedSubstring = "XyZ";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, AtMost.Once());

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region LessThan

        [Fact]
        public void When_string_containment_equivalent_of_less_than_is_asserted_and_actual_value_contains_the_expected_string_less_than_expected_times_it_should_not_throw()
        {
            // Arrange
            string actual = "abCDEBcDF";
            string expectedSubstring = "Bcd";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, LessThan.Times(3));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_equivalent_of_less_than_is_asserted_and_actual_value_contains_the_expected_string_but_not_less_than_expected_times_it_should_throw()
        {
            // Arrange
            string actual = "abCDEBcDF";
            string expectedSubstring = "Bcd";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, LessThan.Times(2));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected * \"abCDEBcDF\" to contain equivalent of \"Bcd\" less than 2 times, but found it 2 times.");
        }

        [Fact]
        public void When_string_containment_equivalent_of_less_than_twice_is_asserted_and_actual_value_does_not_contain_the_expected_string_it_should_throw()
        {
            // Arrange
            string actual = "abCDEf";
            string expectedSubstring = "xyS";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, LessThan.Twice());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_string_containment_equivalent_of_less_than_twice_is_asserted_and_actual_value_is_null_then_it_should_not_throw()
        {
            // Arrange
            string actual = null;
            string expectedSubstring = "XyZ";

            // Act
            Action act = () => actual.Should().ContainEquivalentOf(expectedSubstring, LessThan.Twice());

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #endregion

        #region (Not) Empty

        [Fact]
        public void Should_succeed_when_asserting_empty_string_to_be_empty()
        {
            // Arrange
            string actual = "";

            // Act / Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void Should_fail_when_asserting_non_empty_string_to_be_empty()
        {
            // Arrange
            string actual = "ABC";

            // Act
            Action act = () => actual.Should().BeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_non_empty_string_to_be_empty()
        {
            // Arrange
            string actual = "ABC";

            // Act
            Action act = () => actual.Should().BeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected actual to be empty because we want to test the failure message, but found \"ABC\".");
        }

        [Fact]
        public void When_checking_for_an_empty_string_and_it_is_null_it_should_throw()
        {
            // Arrange
            string nullString = null;

            // Act
            Action act = () => nullString.Should().BeEmpty("because strings should never be {0}", "null");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected nullString to be empty because strings should never be null, but found <null>.");
        }

        [Fact]
        public void Should_succeed_when_asserting_non_empty_string_to_be_filled()
        {
            // Arrange
            string actual = "ABC";

            // Act / Assert
            actual.Should().NotBeEmpty();
        }

        [Fact]
        public void When_asserting_null_string_to_not_be_empty_it_should_succeed()
        {
            // Arrange
            string actual = null;

            // Act / Assert
            actual.Should().NotBeEmpty();
        }

        [Fact]
        public void Should_fail_when_asserting_empty_string_to_be_filled()
        {
            // Arrange
            string actual = "";

            // Act
            Action act = () => actual.Should().NotBeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_empty_string_to_be_filled()
        {
            // Arrange
            string actual = "";

            // Act
            Action act = () => actual.Should().NotBeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect actual to be empty because we want to test the failure message.");
        }

        #endregion

        #region Length

        [Fact]
        public void Should_succeed_when_asserting_string_length_to_be_equal_to_the_same_value()
        {
            // Arrange
            string actual = "ABC";

            // Act / Assert
            actual.Should().HaveLength(3);
        }

        [Fact]
        public void When_asserting_string_length_on_null_string_it_should_fail()
        {
            // Arrange
            string actual = null;

            // Act
            Action act = () => actual.Should().HaveLength(0);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_when_asserting_string_length_to_be_equal_to_different_value()
        {
            // Arrange
            string actual = "ABC";

            // Act
            Action act = () => actual.Should().HaveLength(1);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_string_length_to_be_equal_to_different_value()
        {
            // Arrange
            string actual = "ABC";

            // Act
            Action act = () => actual.Should().HaveLength(1, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected actual with length 1 because we want to test the failure message, but found string \"ABC\" with length 3.");
        }

        #endregion

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_chaining_multiple_assertions_it_should_assert_all_conditions()
        {
            // Arrange
            string actual = "ABCDEFGHI";
            string prefix = "AB";
            string suffix = "HI";
            string substring = "EF";
            int length = 9;

            // Act / Assert
            actual.Should()
                .StartWith(prefix).And
                .EndWith(suffix).And
                .Contain(substring).And
                .HaveLength(length);
        }

        #region (Not) Null Or Empty

        [Fact]
        public void When_a_valid_string_is_expected_to_be_not_null_or_empty_it_should_not_throw()
        {
            // Arrange
            string str = "Hello World";

            // Act / Assert
            str.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void When_an_empty_string_is_not_expected_to_be_null_or_empty_it_should_throw()
        {
            // Arrange
            string str = "";

            // Act
            Action act = () => str.Should().NotBeNullOrEmpty("a valid string is expected for {0}", "str");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected str not to be <null> or empty because a valid string is expected for str, but found \"\".");
        }

        [Fact]
        public void When_a_null_string_is_not_expected_to_be_null_or_empty_it_should_throw()
        {
            // Arrange
            string str = null;

            // Act
            Action act = () => str.Should().NotBeNullOrEmpty("a valid string is expected for {0}", "str");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected str not to be <null> or empty because a valid string is expected for str, but found <null>.");
        }

        [Fact]
        public void When_a_null_string_is_expected_to_be_null_or_empty_it_should_not_throw()
        {
            // Arrange
            string str = null;

            // Act / Assert
            str.Should().BeNullOrEmpty();
        }

        [Fact]
        public void When_an_empty_string_is_expected_to_be_null_or_empty_it_should_not_throw()
        {
            // Arrange
            string str = "";

            // Act / Assert
            str.Should().BeNullOrEmpty();
        }

        [Fact]
        public void When_a_valid_string_is_expected_to_be_null_or_empty_it_should_throw()
        {
            // Arrange
            string str = "hello";

            // Act
            Action act = () => str.Should().BeNullOrEmpty("it was not initialized {0}", "yet");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected str to be <null> or empty because it was not initialized yet, but found \"hello\".");
        }

        #endregion

        #region (Not) Null Or Whitespace

        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n\r")]
        [Theory]
        public void When_correctly_asserting_null_or_whitespace_it_should_not_throw(string actual)
        {
            // Assert
            actual.Should().BeNullOrWhiteSpace();
        }

        [InlineData("a")]
        [InlineData(" a ")]
        [Theory]
        public void When_correctly_asserting_not_null_or_whitespace_it_should_not_throw(string actual)
        {
            // Assert
            actual.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void When_a_valid_string_is_expected_to_be_null_or_whitespace_it_should_throw()
        {
            // Act
            Action act = () =>
                " abc  ".Should().BeNullOrWhiteSpace();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected string to be <null> or whitespace, but found \" abc  \".");
        }

        [Fact]
        public void When_a_null_string_is_expected_to_not_be_null_or_whitespace_it_should_throw()
        {
            // Arrange
            string nullString = null;

            // Act
            Action act = () =>
                nullString.Should().NotBeNullOrWhiteSpace();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected nullString not to be <null> or whitespace, but found <null>.");
        }

        [Fact]
        public void When_an_empty_string_is_expected_to_not_be_null_or_whitespace_it_should_throw()
        {
            // Act
            Action act = () =>
                "".Should().NotBeNullOrWhiteSpace();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected string not to be <null> or whitespace, but found \"\".");
        }

        [Fact]
        public void When_a_whitespace_string_is_expected_to_not_be_null_or_whitespace_it_should_throw()
        {
            // Act
            Action act = () =>
                "   ".Should().NotBeNullOrWhiteSpace();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected string not to be <null> or whitespace, but found \"   \".");
        }

        #endregion
    }
}
