using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The HaveElementSucceeding specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class HaveElementSucceeding
    {
        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_collection_has_the_correct_element_succeeding_another_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding("cris", "mick");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_collection_has_the_wrong_element_succeeding_another_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding("mick", "cris", "because of some reason");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*cris*succeed*mick*because*reason*found*john*");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_nothing_is_succeeding_an_element_it_should_throw()
        {
            // Arrange
            var collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding("john", "jane");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*jane*succeed*john*found*nothing*");
        }

        [Fact]
        public void When_expecting_an_element_to_succeed_another_but_the_collection_is_empty_it_should_throw()
        {
            // Arrange
            var collection = new string[0];

            // Act
            Action act = () => collection.Should().HaveElementSucceeding("mick", "cris");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*cris*succeed*mick*collection*empty*");
        }

        [Fact]
        public void When_a_null_element_is_succeeding_another_element_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "mick", null, "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding("mick", null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_null_element_is_not_succeeding_another_element_it_should_throw()
        {
            // Arrange
            var collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding("mick", null);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*null*succeed*mick*but found*john*");
        }

        [Fact]
        public void When_an_element_is_succeeding_a_null_element_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "mick", null, "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding(null, "john");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_element_is_not_succeeding_a_null_element_it_should_throw()
        {
            // Arrange
            var collection = new[] { "mick", null, "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding(null, "cris");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*cris*succeed*null*but found*john*");
        }

        [Fact]
        public void When_collection_is_null_then_have_element_succeeding_should_fail()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().HaveElementSucceeding("mick", "cris", "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to have \"cris\" succeed \"mick\" *failure message*, but the collection is <null>.");
        }
    }
}
