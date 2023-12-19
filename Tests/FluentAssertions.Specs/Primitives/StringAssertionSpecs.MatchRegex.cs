using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]MatchRegex specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class MatchRegex
    {
        [Fact]
        public void When_a_string_matches_a_regular_expression_string_it_should_not_throw()
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
        public void When_a_string_does_not_match_a_regular_expression_string_it_should_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().MatchRegex("h.*\\sworld?$", "that's the universal greeting");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected subject to match regex*\"h.*\\sworld?$\" because that's the universal greeting, but*\"hello world!\" does not match.");
        }

        [Fact]
        public void When_a_null_string_is_matched_against_a_regex_string_it_should_throw_with_a_clear_explanation()
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
        public void When_a_string_is_matched_against_a_null_regex_string_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().MatchRegex((string)null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot match string against <null>. Provide a regex pattern or use the BeNull method.*")
                .WithParameterName("regularExpression");
        }

        [Fact]
        public void When_a_string_is_matched_against_an_invalid_regex_string_it_should_throw_with_a_clear_explanation()
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
        public void When_a_string_is_matched_against_an_invalid_regex_string_it_should_only_have_one_failure_message()
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

        [Fact]
        public void When_a_string_is_matched_against_an_empty_regex_string_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().MatchRegex(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage("Cannot match string against an empty string. Provide a regex pattern or use the BeEmpty method.*")
                .WithParameterName("regularExpression");
        }

        [Fact]
        public void When_a_string_matches_a_regular_expression_it_should_not_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            // ReSharper disable once StringLiteralTypo
            Action act = () => subject.Should().MatchRegex(new Regex("h.*\\sworld.$"));

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
            Action act = () => subject.Should().MatchRegex(new Regex("h.*\\sworld?$"), "that's the universal greeting");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected subject to match regex*\"h.*\\sworld?$\" because that's the universal greeting, but*\"hello world!\" does not match.");
        }

        [Fact]
        public void When_a_null_string_is_matched_against_a_regex_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                subject.Should().MatchRegex(new Regex(".*"), "because it should be a string");
            };

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
            Action act = () => subject.Should().MatchRegex((Regex)null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot match string against <null>. Provide a regex pattern or use the BeNull method.*")
                .WithParameterName("regularExpression");
        }

        [Fact]
        public void When_a_string_is_matched_against_an_empty_regex_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().MatchRegex(new Regex(string.Empty));

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage("Cannot match string against an empty string. Provide a regex pattern or use the BeEmpty method.*")
                .WithParameterName("regularExpression");
        }

        [Fact]
        public void When_a_string_is_matched_and_the_count_of_matches_fits_into_the_expected_it_passes()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().MatchRegex(new Regex("hello.*"), AtLeast.Once());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_string_is_matched_and_the_count_of_matches_do_not_fit_the_expected_it_fails()
        {
            // Arrange
            string subject = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt " +
                "ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et " +
                "ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

            // Act
            Action act = () => subject.Should().MatchRegex("Lorem.*", Exactly.Twice());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject*Lorem*to match regex*\"Lorem.*\" exactly 2 times, but found it 1 time*");
        }

        [Fact]
        public void When_a_string_is_matched_and_the_expected_count_is_zero_and_string_not_matches_it_passes()
        {
            // Arrange
            string subject = "a";

            // Act
            Action act = () => subject.Should().MatchRegex("b", Exactly.Times(0));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_string_is_matched_and_the_expected_count_is_zero_and_string_matches_it_fails()
        {
            // Arrange
            string subject = "a";

            // Act
            Action act = () => subject.Should().MatchRegex("a", Exactly.Times(0));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject*a*to match regex*\"a\" exactly 0 times, but found it 1 time*");
        }

        [Fact]
        public void When_the_subject_is_null_it_fails()
        {
            // Arrange
            string subject = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                subject.Should().MatchRegex(".*", Exactly.Times(0), "because it should be a string");
            };

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("Expected subject to match regex*\".*\" because it should be a string, but it was <null>.");
        }

        [Fact]
        public void When_the_subject_is_empty_and_expected_count_is_zero_it_passes()
        {
            // Arrange
            string subject = string.Empty;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                subject.Should().MatchRegex("a", Exactly.Times(0));
            };

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_subject_is_empty_and_expected_count_is_more_than_zero_it_fails()
        {
            // Arrange
            string subject = string.Empty;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                subject.Should().MatchRegex(".+", AtLeast.Once());
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject*to match regex* at least 1 time, but found it 0 times*");
        }

        [Fact]
        public void When_regex_is_null_it_fails_and_ignores_occurrences()
        {
            // Arrange
            string subject = "a";

            // Act
            Action act = () => subject.Should().MatchRegex((Regex)null, Exactly.Times(0));

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithMessage("Cannot match string against <null>. Provide a regex pattern or use the BeNull method.*")
                .WithParameterName("regularExpression");
        }

        [Fact]
        public void When_regex_is_empty_it_fails_and_ignores_occurrences()
        {
            // Arrange
            string subject = "a";

            // Act
            Action act = () => subject.Should().MatchRegex(string.Empty, Exactly.Times(0));

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage("Cannot match string against an empty string. Provide a regex pattern or use the BeEmpty method.*")
                .WithParameterName("regularExpression");
        }

        [Fact]
        public void When_regex_is_invalid_it_fails_and_ignores_occurrences()
        {
            // Arrange
            string subject = "a";

            // Act
#pragma warning disable RE0001 // Invalid regex pattern
            Action act = () => subject.Should().MatchRegex(".**", Exactly.Times(0));
#pragma warning restore RE0001 // Invalid regex pattern

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("Cannot match subject against \".**\" because it is not a valid regular expression.*");
        }
    }

    public class NotMatchRegex
    {
        [Fact]
        public void When_a_string_does_not_match_a_regular_expression_string_and_it_shouldnt_it_should_not_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().NotMatchRegex(".*earth.*");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_string_matches_a_regular_expression_string_but_it_shouldnt_it_should_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().NotMatchRegex(".*world.*", "because that's illegal");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Did not expect subject to match regex*\".*world.*\" because that's illegal, but*\"hello world!\" matches.");
        }

        [Fact]
        public void When_a_null_string_is_negatively_matched_against_a_regex_string_it_should_throw_with_a_clear_explanation()
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
        public void When_a_string_is_negatively_matched_against_a_null_regex_string_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().NotMatchRegex((string)null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot match string against <null>. Provide a regex pattern or use the NotBeNull method.*")
                .WithParameterName("regularExpression");
        }

        [Fact]
        public void When_a_string_is_negatively_matched_against_an_invalid_regex_string_it_should_throw_with_a_clear_explanation()
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
        public void When_a_string_is_negatively_matched_against_an_invalid_regex_string_it_only_contain_one_failure_message()
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

        [Fact]
        public void When_a_string_is_negatively_matched_against_an_empty_regex_string_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().NotMatchRegex(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage(
                    "Cannot match string against an empty regex pattern. Provide a regex pattern or use the NotBeEmpty method.*")
                .WithParameterName("regularExpression");
        }

        [Fact]
        public void When_a_string_does_not_match_a_regular_expression_and_it_shouldnt_it_should_not_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().NotMatchRegex(new Regex(".*earth.*"));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_string_matches_a_regular_expression_but_it_shouldnt_it_should_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().NotMatchRegex(new Regex(".*world.*"), "because that's illegal");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Did not expect subject to match regex*\".*world.*\" because that's illegal, but*\"hello world!\" matches.");
        }

        [Fact]
        public void When_a_null_string_is_negatively_matched_against_a_regex_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                subject.Should().NotMatchRegex(new Regex(".*"), "because it should not be a string");
            };

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
            Action act = () => subject.Should().NotMatchRegex((Regex)null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot match string against <null>. Provide a regex pattern or use the NotBeNull method.*")
                .WithParameterName("regularExpression");
        }

        [Fact]
        public void When_a_string_is_negatively_matched_against_an_empty_regex_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().NotMatchRegex(new Regex(string.Empty));

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage(
                    "Cannot match string against an empty regex pattern. Provide a regex pattern or use the NotBeEmpty method.*")
                .WithParameterName("regularExpression");
        }
    }
}
