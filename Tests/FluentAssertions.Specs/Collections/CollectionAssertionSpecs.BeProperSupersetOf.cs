using System;
using System.Collections.Generic;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <content>
/// The BeProperSupersetOf specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class BeProperSupersetOf
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_contains_multiple_items_from_the_collection_in_any_order()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().BeProperSupersetOf(new[] { 2, 1 });
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_contains_multiple_items_with_duplicates_from_the_collection_in_any_order()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().BeProperSupersetOf(new[] { 2, 1, 1, 1, 2 });
        }

        [Fact]
        public void When_collection_is_not_a_proper_superset_of_another_it_should_throw_with_the_reason()
        {
            // Arrange
            var superset = new[] { 1, 2, 3, 4 };
            var subset = new[] { 4, 3, 2, 1 };

            // Act
            Action act = () => superset.Should().BeProperSupersetOf(subset, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected superset to be a proper superset of {1, 2, 3, 4} because we want to test the failure message, " +
                "but items {4, 3, 2, 1} are equivalent to the superset");
        }

        [Fact]
        public void When_collection_is_not_a_proper_subset_of_another_with_duplicates_it_should_throw_with_the_reason()
        {
            // Arrange
            var subset = new[] { 1, 1, 1, 2, 2, 3, 3, 4 };
            var superset = new[] { 4, 3, 2, 1 };

            // Act
            Action act = () => superset.Should().BeProperSupersetOf(subset, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected superset to be a proper superset of {4, 3, 2, 1} because we want to test the failure message, " +
                "but items {1, 1, 1, 2, 2, 3, 3, 4} are equivalent to the superset");
        }

        [Fact]
        public void When_collection_with_duplicates_is_not_a_proper_subset_of_another_with_duplicates_it_should_throw_with_the_reason()
        {
            // Arrange
            var subset = new[] { 1, 1, 1, 2, 2, 3, 3, 4 };
            var superset = new[] { 4, 4, 4, 3, 3, 2, 1, 1 };

            // Act
            Action act = () => superset.Should().BeProperSupersetOf(subset, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected superset to be a proper superset of {4, 3, 2, 1} because we want to test the failure message, " +
                "but items {1, 1, 1, 2, 2, 3, 3, 4} are equivalent to the superset");
        }

        [Fact]
        public void When_a_collection_does_not_contain_another_collection_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().BeProperSupersetOf(new[] { 3, 4, 5 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to be a proper superset of {3, 4, 5} because we do, but could not find {4, 5}.");
        }

        [Fact]
        public void When_a_collection_does_not_contain_a_single_element_collection_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().BeProperSupersetOf(new[] { 4 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to be a proper superset of 4 because we do.");
        }

        [Fact]
        public void
            When_a_collection_does_not_contain_other_collection_with_assertion_scope_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().BeProperSupersetOf(new[] { 4 }).And.BeProperSupersetOf(new[] { 5, 6 });
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*to be a proper superset of 4*to be a proper superset of {5, 6}*");
        }

        [Fact]
        public void When_the_contents_of_a_collection_are_checked_against_an_empty_collection_it_should_throw_clear_explanation()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().BeProperSupersetOf(new int[0]);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify containment against an empty collection*");
        }

        [Fact]
        public void When_asserting_collection_does_contain_a_list_of_items_against_null_collection_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().BeProperSupersetOf(new[] { 1, 2 }, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection to be a proper superset of {1, 2} *failure message*, but found <null>.");
        }
    }
}
