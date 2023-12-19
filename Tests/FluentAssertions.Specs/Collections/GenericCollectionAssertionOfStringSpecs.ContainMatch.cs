using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class ContainMatch
    {
        [Fact]
        public void When_collection_contains_a_match_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().ContainMatch("* failed");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_collection_contains_multiple_matches_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test failed", "pack failed" };

            // Act
            Action action = () => collection.Should().ContainMatch("* failed");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_collection_contains_multiple_matches_which_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test failed", "pack failed" };

            // Act
            Action action = () => _ = collection.Should().ContainMatch("* failed").Which;

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("More than one object found.  FluentAssertions cannot determine which object is meant.*")
                .WithMessage("*Found objects:*\"test failed\"*\"pack failed\"");
        }

        [Fact]
        public void When_collection_does_not_contain_a_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().ContainMatch("* stopped", "because {0}", "we do");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection {\"build succeded\", \"test failed\"} to contain a match of \"* stopped\" because we do.");
        }

        [Fact]
        public void When_collection_contains_a_match_that_differs_in_casing_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().ContainMatch("* Failed");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection {\"build succeded\", \"test failed\"} to contain a match of \"* Failed\".");
        }

        [Fact]
        public void When_asserting_empty_collection_for_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { };

            // Act
            Action action = () => collection.Should().ContainMatch("* failed");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection {empty} to contain a match of \"* failed\".");
        }

        [Fact]
        public void When_asserting_null_collection_for_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action action = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().ContainMatch("* failed", "because {0}", "we do");
            };

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to contain a match of \"* failed\" because we do, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_to_have_null_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().ContainMatch(null);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithMessage(
                    "Cannot match strings in collection against <null>. Provide a wildcard pattern or use the Contain method.*")
                .WithParameterName("wildcardPattern");
        }

        [Fact]
        public void When_asserting_collection_to_have_empty_string_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().ContainMatch(string.Empty);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage(
                    "Cannot match strings in collection against an empty string. Provide a wildcard pattern or use the Contain method.*")
                .WithParameterName("wildcardPattern");
        }
    }

    public class NotContainMatch
    {
        [Fact]
        public void When_collection_doesnt_contain_a_match_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test" };

            // Act
            Action action = () => collection.Should().NotContainMatch("* failed");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_collection_doesnt_contain_multiple_matches_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test", "pack" };

            // Act
            Action action = () => collection.Should().NotContainMatch("* failed");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_collection_contains_a_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().NotContainMatch("* failed", "because {0}", "it shouldn't");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Did not expect collection {\"build succeded\", \"test failed\"} to contain a match of \"* failed\" because it shouldn't.");
        }

        [Fact]
        public void When_collection_contains_multiple_matches_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build failed", "test failed" };

            // Act
            Action action = () => collection.Should().NotContainMatch("* failed", "because {0}", "it shouldn't");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Did not expect collection {\"build failed\", \"test failed\"} to contain a match of \"* failed\" because it shouldn't.");
        }

        [Fact]
        public void When_collection_contains_a_match_with_different_casing_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().NotContainMatch("* Failed");

            // Assert
            action.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_asserting_collection_to_not_have_null_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().NotContainMatch(null);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .WithMessage(
                    "Cannot match strings in collection against <null>. Provide a wildcard pattern or use the NotContain method.*")
                .WithParameterName("wildcardPattern");
        }

        [Fact]
        public void When_asserting_collection_to_not_have_empty_string_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().NotContainMatch(string.Empty);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage(
                    "Cannot match strings in collection against an empty string. Provide a wildcard pattern or use the NotContain method.*")
                .WithParameterName("wildcardPattern");
        }

        [Fact]
        public void When_asserting_null_collection_to_not_have_null_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action action = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotContainMatch("* Failed", "we want to test the failure {0}", "message");
            };

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to contain a match of \"* failed\" *failure message*, but found <null>.");
        }
    }
}
