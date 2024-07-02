using System;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <content>
/// The BeProperSubsetOf specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class BeProperSubsetOf
    {
        [Fact]
        public void A_collection_with_only_items_of_a_superset_but_misses_some_is_a_proper_subset()
        {
            // Arrange
            var subset = new[] { 1, 2 };
            var superset = new[] { 1, 2, 3 };

            // Act / Assert
            subset.Should().BeProperSubsetOf(superset);
        }

        [Fact]
        public void A_collection_with_duplicates_and_only_items_of_a_superset_with_duplicates_but_misses_some_is_a_proper_subset()
        {
            // Arrange
            var subset = new[] { 1, 1, 1, 2, 2, 3, 3 };
            var superset = new[] { 1, 2, 3, 3, 3, 4 };

            // Act / Assert
            subset.Should().BeProperSubsetOf(superset);
        }

        [Fact]
        public void A_collection_with_all_items_of_a_superset_but_has_extra_items_is_not_a_proper_subset()
        {
            // Arrange
            var subset = new[] { 1, 2, 3, 4 };
            var superset = new[] { 4, 3, 2, 1 };

            // Act
            Action act = () => subset.Should().BeProperSubsetOf(superset, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subset to be a proper subset of {4, 3, 2, 1} because we want to test the failure message, " +
                "but items {1, 2, 3, 4} are equivalent to the superset {4, 3, 2, 1}");
        }

        [Fact]
        public void A_collection_with_all_items_and_duplicates_of_a_superset_but_has_extra_items_is_not_a_proper_subset()
        {
            // Arrange
            var subset = new[] { 1, 1, 1, 2, 2, 3, 3, 4 };
            var superset = new[] { 4, 3, 2, 1 };

            // Act
            Action act = () => subset.Should().BeProperSubsetOf(superset, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subset to be a proper subset of {4, 3, 2, 1} because we want to test the failure message, " +
                "but items {1, 1, 1, 2, 2, 3, 3, 4} are equivalent to the superset {4, 3, 2, 1}");
        }

        [Fact]
        public void A_collection_with_duplicates_is_not_a_proper_subset_of_a_superset_with_duplicates()
        {
            // Arrange
            var subset = new[] { 1, 1, 1, 2, 2, 3, 3, 4 };
            var superset = new[] { 4, 4, 4, 3, 3, 2, 1, 1 };

            // Act
            Action act = () => subset.Should().BeProperSubsetOf(superset, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subset to be a proper subset of {4, 3, 2, 1} because we want to test the failure message, " +
                "but items {1, 1, 1, 2, 2, 3, 3, 4} are equivalent to the superset {4, 4, 4, 3, 3, 2, 1, 1}");
        }

        [Fact]
        public void A_collection_that_is_not_a_proper_subset_of_other_collection_with_assertion_scope()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().BeProperSubsetOf(new[] { 4 }).And.BeProperSubsetOf(new[] { 5, 6 });
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*to be a proper subset of {4}*to be a proper subset of {5, 6}*");
        }

        [Fact]
        public void A_empty_collection_tested_against_a_proper_superset()
        {
            // Arrange
            var subset = new int[0];
            var superset = new[] { 1, 2, 4, 5 };

            // Act
            Action act = () => subset.Should().BeProperSubsetOf(superset);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void A_non_empty_subset_tested_against_a_null_superset()
        {
            // Arrange
            var subset = new[] { 1, 2, 3 };
            int[] superset = null;

            // Act
            Action act = () => subset.Should().BeProperSubsetOf(superset);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify a proper subset against a <null> collection.*");
        }
    }
}
