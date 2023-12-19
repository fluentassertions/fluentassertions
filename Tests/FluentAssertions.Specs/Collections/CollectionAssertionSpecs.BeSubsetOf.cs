using System;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The [Not]BeSubsetOf specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class BeSubsetOf
    {
        [Fact]
        public void When_collection_is_subset_of_a_specified_collection_it_should_not_throw()
        {
            // Arrange
            var subset = new[] { 1, 2 };
            var superset = new[] { 1, 2, 3 };

            // Act / Assert
            subset.Should().BeSubsetOf(superset);
        }

        [Fact]
        public void When_collection_is_not_a_subset_of_another_it_should_throw_with_the_reason()
        {
            // Arrange
            var subset = new[] { 1, 2, 3, 6 };
            var superset = new[] { 1, 2, 4, 5 };

            // Act
            Action act = () => subset.Should().BeSubsetOf(superset, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subset to be a subset of {1, 2, 4, 5} because we want to test the failure message, " +
                "but items {3, 6} are not part of the superset.");
        }

        [Fact]
        public void When_an_empty_collection_is_tested_against_a_superset_it_should_succeed()
        {
            // Arrange
            var subset = new int[0];
            var superset = new[] { 1, 2, 4, 5 };

            // Act
            Action act = () => subset.Should().BeSubsetOf(superset);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_subset_is_tested_against_a_null_superset_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            var subset = new[] { 1, 2, 3 };
            int[] superset = null;

            // Act
            Action act = () => subset.Should().BeSubsetOf(superset);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify a subset against a <null> collection.*");
        }

        [Fact]
        public void When_a_set_is_expected_to_be_not_a_subset_it_should_succeed()
        {
            // Arrange
            var subject = new[] { 1, 2, 4 };
            var otherSet = new[] { 1, 2, 3 };

            // Act / Assert
            subject.Should().NotBeSubsetOf(otherSet);
        }
    }

    public class NotBeSubsetOf
    {
        [Fact]
        public void When_an_empty_set_is_not_supposed_to_be_a_subset_of_another_set_it_should_throw()
        {
            // Arrange
            var subject = new int[] { };
            var otherSet = new[] { 1, 2, 3 };

            // Act
            Action act = () => subject.Should().NotBeSubsetOf(otherSet);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect subject {empty} to be a subset of {1, 2, 3}.");
        }

        [Fact]
        public void Should_fail_when_asserting_collection_is_not_subset_of_a_superset_collection()
        {
            // Arrange
            var subject = new[] { 1, 2 };
            var otherSet = new[] { 1, 2, 3 };

            // Act
            Action act = () => subject.Should().NotBeSubsetOf(otherSet, "because I'm {0}", "mistaken");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect subject {1, 2} to be a subset of {1, 2, 3} because I'm mistaken.");
        }

        [Fact]
        public void When_asserting_collection_to_be_subset_against_null_collection_it_should_throw()
        {
            // Arrange
            int[] collection = null;
            var collection1 = new[] { 1, 2, 3 };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().BeSubsetOf(collection1, "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to be a subset of {1, 2, 3} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_to_not_be_subset_against_same_collection_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };
            var collection1 = collection;

            // Act
            Action act = () => collection.Should().NotBeSubsetOf(collection1,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect*to be a subset of*because we want to test the behaviour with same objects*but they both reference the same object.");
        }

        [Fact]
        public void When_asserting_collection_to_not_be_subset_against_null_collection_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotBeSubsetOf(new[] { 1, 2, 3 }, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Cannot assert a <null> collection against a subset.");
        }
    }
}
