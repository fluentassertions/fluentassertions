using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

/// <content>
/// The [Not]MatchEquivalentOf specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class MatchEquivalentOf
    {
        [Fact]
        public void Can_ignore_casing_while_checking_a_string_to_match_another()
        {
            // Arrange
            string actual = "test";
            string expect = "T*T";

            // Act / Assert
            actual.Should().MatchEquivalentOf(expect, o => o.IgnoringCase());
        }

        [Fact]
        public void Can_ignore_leading_whitespace_while_checking_a_string_to_match_another()
        {
            // Arrange
            string actual = "  test";
            string expect = "t*t";

            // Act / Assert
            actual.Should().MatchEquivalentOf(expect, o => o.IgnoringLeadingWhitespace());
        }

        [Fact]
        public void Can_ignore_trailing_whitespace_while_checking_a_string_to_match_another()
        {
            // Arrange
            string actual = "test  ";
            string expect = "t*t";

            // Act / Assert
            actual.Should().MatchEquivalentOf(expect, o => o.IgnoringTrailingWhitespace());
        }

        [Fact]
        public void Can_ignore_all_newlines_while_checking_a_string_to_match_another()
        {
            // Arrange
            string actual = "\rA\nB\r\nC\n";
            string expect = "A?C\r";

            // Act / Assert
            actual.Should().MatchEquivalentOf(expect, o => o.IgnoringAllNewlines());
        }

        [Fact]
        public void Can_ignore_newline_style_while_checking_a_string_to_match_another()
        {
            // Arrange
            string actual = "\rA\nB\r\nC\n";
            string expect = "\nA\r\n?\nC\r";

            // Act / Assert
            actual.Should().MatchEquivalentOf(expect, o => o.IgnoringNewlineStyle());
        }

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

        [Fact]
        public void When_a_string_is_matched_against_the_equivalent_of_null_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().MatchEquivalentOf(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithMessage("Cannot match string against <null>. Provide a wildcard pattern or use the BeNull method.*")
                .WithParameterName("wildcardPattern");
        }

        [Fact]
        public void When_a_string_is_matched_against_the_equivalent_of_an_empty_string_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().MatchEquivalentOf(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage(
                    "Cannot match string against an empty string. Provide a wildcard pattern or use the BeEmpty method.*")
                .WithParameterName("wildcardPattern");
        }
    }

    public class NotMatchEquivalentOf
    {
        [Fact]
        public void Can_ignore_casing_while_checking_a_string_to_not_match_another()
        {
            // Arrange
            string actual = "test";
            string expect = "T*T";

            // Act
            Action act = () => actual.Should().NotMatchEquivalentOf(expect, o => o.IgnoringCase());

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Can_ignore_leading_whitespace_while_checking_a_string_to_not_match_another()
        {
            // Arrange
            string actual = "  test";
            string expect = "t*t";

            // Act
            Action act = () => actual.Should().NotMatchEquivalentOf(expect, o => o.IgnoringLeadingWhitespace());

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Can_ignore_trailing_whitespace_while_checking_a_string_to_not_match_another()
        {
            // Arrange
            string actual = "test  ";
            string expect = "t*t";

            // Act
            Action act = () => actual.Should().NotMatchEquivalentOf(expect, o => o.IgnoringTrailingWhitespace());

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Can_ignore_all_newlines_while_checking_a_string_to_not_match_another()
        {
            // Arrange
            string actual = "\rA\nB\r\nC\n";
            string expect = "\nA\r\n?\nC\r";

            // Act
            Action act = () => actual.Should().NotMatchEquivalentOf(expect, o => o.IgnoringAllNewlines());

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Can_ignore_newline_style_while_checking_a_string_to_not_match_another()
        {
            // Arrange
            string actual = "\rA\nB\r\nC\n";
            string expect = "\nA\r\n?\n\nC\r";

            // Act
            Action act = () => actual.Should().NotMatchEquivalentOf(expect, o => o.IgnoringNewlineStyle());

            // Assert
            act.Should().Throw<XunitException>();
        }

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

        [Fact]
        public void
            When_a_string_is_negatively_matched_against_the_equivalent_of_null_pattern_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().NotMatchEquivalentOf(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithMessage("Cannot match string against <null>. Provide a wildcard pattern or use the NotBeNull method.*")
                .WithParameterName("wildcardPattern");
        }

        [Fact]
        public void
            When_a_string_is_negatively_matched_against_the_equivalent_of_an_empty_string_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().NotMatchEquivalentOf(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage(
                    "Cannot match string against an empty string. Provide a wildcard pattern or use the NotBeEmpty method.*")
                .WithParameterName("wildcardPattern");
        }

        [Fact]
        public void Does_not_treat_escaped_newlines_as_newlines()
        {
            // Arrange
            string actual = "te\r\nst";
            string expect = "te\\r\\nst";

            // Act / Assert
            actual.Should().NotMatchEquivalentOf(expect);
        }
    }
}
