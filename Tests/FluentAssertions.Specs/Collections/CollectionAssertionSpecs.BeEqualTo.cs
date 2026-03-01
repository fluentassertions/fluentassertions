using System;
using System.Collections.Generic;
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
        public void Two_collections_with_the_same_elements_in_the_same_order_are_okay()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [1, 2, 3];

            // Act / Assert
            collection1.Should().BeEqualTo(collection2);
        }

        [Fact]
        public void Two_null_collections_are_okay()
        {
            // Arrange
            int[] nullColl = null;

            // Act
            Action act = () => nullColl.Should().BeEqualTo(null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Fails_with_a_descriptive_message_for_a_differing_item()
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
        public void Fails_for_a_null_subject_collection()
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
        public void Throws_for_a_null_expected_collection()
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
        public void Succeeds_using_custom_equality_comparison()
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
        public void Fails_using_custom_equality_comparison_for_a_differing_item()
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
        public void Succeeds_for_collections_with_different_elements()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [3, 1, 2];

            // Act / Assert
            collection1.Should().NotBeEqualTo(collection2);
        }

        [Fact]
        public void Fails_for_collections_with_the_same_elements_in_the_same_order()
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
        public void Throws_for_a_null_unexpected_collection()
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
