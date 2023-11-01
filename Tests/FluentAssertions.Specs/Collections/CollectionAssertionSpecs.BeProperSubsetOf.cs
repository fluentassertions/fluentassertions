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
        public void When_collection_is_proper_subset_of_a_specified_collection_it_should_not_throw()
        {
            // Arrange
            var subset = new[] { 1, 2 };
            var superset = new[] { 1, 2, 3 };

            // Act / Assert
            subset.Should().BeProperSubsetOf(superset);
        }

        [Fact]
        public void When_collection_is_not_a_proper_subset_of_another_it_should_throw_with_the_reason()
        {
            // Arrange
            var subset = new[] { 1, 2, 3, 4 };
            var superset = new[] { 4, 3, 2, 1 };

            // Act
            Action act = () => subset.Should().BeProperSubsetOf(superset, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subset to be a proper subset of {1, 2, 3, 4} because we want to test the failure message, " +
                "but items {4, 3, 2, 1} are equivalent to the subset");
        }

        [Fact]
        public void
            When_a_collection_is_not_a_proper_subset_of_other_collection_with_assertion_scope_it_should_throw_with_clear_explanation()
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
        public void When_an_empty_collection_is_tested_against_a_proper_superset_it_should_succeed()
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
        public void When_a_subset_is_tested_against_a_null_superset_it_should_throw_with_a_clear_explanation()
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
