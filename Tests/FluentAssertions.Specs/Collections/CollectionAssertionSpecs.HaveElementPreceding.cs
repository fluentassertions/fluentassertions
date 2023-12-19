using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The HaveElementPreceding specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class HaveElementPreceding
    {
        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_collection_has_the_correct_element_preceding_another_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding("mick", "cris");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_collection_has_the_wrong_element_preceding_another_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding("john", "cris", "because of some reason");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*cris*precede*john*because*reason*found*mick*");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_nothing_is_preceding_an_element_it_should_throw()
        {
            // Arrange
            var collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding("cris", "jane");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*jane*precede*cris*found*nothing*");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_expecting_an_element_to_precede_another_but_collection_is_empty_it_should_throw()
        {
            // Arrange
            var collection = new string[0];

            // Act
            Action act = () => collection.Should().HaveElementPreceding("mick", "cris");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*cris*precede*mick*collection*empty*");
        }

        [Fact]
        public void When_a_null_element_is_preceding_another_element_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { null, "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding("mick", null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_a_null_element_is_not_preceding_another_element_it_should_throw()
        {
            // Arrange
            var collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding("mick", null);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*null*precede*mick*but found*cris*");
        }

        [Fact]
        public void When_an_element_is_preceding_a_null_element_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "mick", null, "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding(null, "mick");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_element_is_not_preceding_a_null_element_it_should_throw()
        {
            // Arrange
            var collection = new[] { "mick", null, "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding(null, "cris");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*cris*precede*null*but found*mick*");
        }

        [Fact]
        public void When_collection_is_null_then_have_element_preceding_should_fail()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().HaveElementPreceding("mick", "cris", "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to have \"cris\" precede \"mick\" *failure message*, but the collection is <null>.");
        }
    }
}
