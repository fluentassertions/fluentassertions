using System;
using System.Collections.Generic;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <content>
/// The [Not]BeEqualTo specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class BeEqualTo
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_collection()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [1, 2, 3];

            // Act / Assert
            collection1.Should().BeEqualTo(collection2);
        }

        [Fact]
        public void When_both_collections_are_null_it_should_succeed()
        {
            // Arrange
            int[] nullColl = null;

            // Act
            Action act = () => nullColl.Should().BeEqualTo(null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_two_collections_are_not_equal_because_one_item_differs_it_should_throw_using_the_reason()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [1, 2, 5];

            // Act
            Action act = () => collection1.Should().BeEqualTo(collection2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 to be equal to {1, 2, 5} because we want to test the failure message, but {1, 2, 3} differs at index 2.");
        }

        [Fact]
        public void When_asserting_collections_to_be_equal_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = null;
            int[] collection1 = [1, 2, 3];

            // Act
            Action act = () =>
                collection.Should().BeEqualTo(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to be equal to {1, 2, 3} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collections_to_be_equal_but_expected_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = [1, 2, 3];
            int[] collection1 = null;

            // Act
            Action act = () =>
                collection.Should().BeEqualTo(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare collection with <null>.*")
                .WithParameterName("expectation");
        }

        [Fact]
        public void When_all_items_match_according_to_a_predicate_it_should_succeed()
        {
            // Arrange
            var actual = new List<string> { "ONE", "TWO", "THREE", "FOUR" };

            var expected = new[]
            {
                new { Value = "One" },
                new { Value = "Two" },
                new { Value = "Three" },
                new { Value = "Four" }
            };

            // Act
            Action action = () => actual.Should().BeEqualTo(expected,
                (a, e) => string.Equals(a, e.Value, StringComparison.OrdinalIgnoreCase));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_any_item_does_not_match_according_to_a_predicate_it_should_throw()
        {
            // Arrange
            var actual = new List<string> { "ONE", "TWO", "THREE", "FOUR" };

            var expected = new[]
            {
                new { Value = "One" },
                new { Value = "Two" },
                new { Value = "Three" },
                new { Value = "Five" }
            };

            // Act
            Action action = () => actual.Should().BeEqualTo(expected,
                (a, e) => string.Equals(a, e.Value, StringComparison.OrdinalIgnoreCase));

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("*Expected*equal to*, but*differs at index 3.*");
        }
    }

    public class NotBeEqualTo
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_is_not_equal_to_a_different_collection()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [3, 1, 2];

            // Act / Assert
            collection1.Should().NotBeEqualTo(collection2);
        }

        [Fact]
        public void When_two_equal_collections_are_not_expected_to_be_equal_it_should_throw()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [1, 2, 3];

            // Act
            Action act = () => collection1.Should().NotBeEqualTo(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect collections {1, 2, 3} and {1, 2, 3} to be equal.");
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equal_subject_but_both_collections_are_null_it_should_throw()
        {
            // Arrange
            int[] collection1 = null;
            int[] collection2 = null;

            // Act
            Action act = () =>
                collection1.Should().NotBeEqualTo(collection2,
                    "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare collection with <null>.*")
                .WithParameterName("unexpected");
        }
    }
}
