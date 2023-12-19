using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]Match specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class Match
    {
        [Fact]
        public void When_a_string_does_not_match_a_wildcard_pattern_it_should_throw()
        {
            // Arrange
            string subject = "hello world!";

            // Act
            Action act = () => subject.Should().Match("h*earth!", "that's the universal greeting");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected subject to match*\"h*earth!\" because that's the universal greeting, but*\"hello world!\" does not.");
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

        [Fact]
        public void When_a_string_is_matched_against_null_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().Match(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithMessage("Cannot match string against <null>. Provide a wildcard pattern or use the BeNull method.*")
                .WithParameterName("wildcardPattern");
        }

        [Fact]
        public void Null_does_not_match_to_any_string()
        {
            // Arrange
            string subject = null;

            // Act
            Action act = () => subject.Should().Match("*");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to match *, but found <null>.");
        }

        [Fact]
        public void When_a_string_is_matched_against_an_empty_string_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().Match(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage(
                    "Cannot match string against an empty string. Provide a wildcard pattern or use the BeEmpty method.*")
                .WithParameterName("wildcardPattern");
        }
    }

    public class NotMatch
    {
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

        [Fact]
        public void When_a_string_is_negatively_matched_against_null_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().NotMatch(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithMessage("Cannot match string against <null>. Provide a wildcard pattern or use the NotBeNull method.*")
                .WithParameterName("wildcardPattern");
        }

        [Fact]
        public void When_a_string_is_negatively_matched_against_an_empty_string_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            string subject = "hello world";

            // Act
            Action act = () => subject.Should().NotMatch(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage(
                    "Cannot match string against an empty string. Provide a wildcard pattern or use the NotBeEmpty method.*")
                .WithParameterName("wildcardPattern");
        }
    }
}
