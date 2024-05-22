using System;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <content>
/// The BeSupersetOf specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class BeSupersetOf
    {
        [Fact]
        public void A_collection_contains_multiple_items_from_the_collection_in_any_order()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().BeSupersetOf(new[] { 2, 1 });
        }

        [Fact]
        public void A_collection_does_not_contain_another_collection()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().BeSupersetOf(new[] { 3, 4, 5 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to be a superset of {3, 4, 5} because we do, but could not find {4, 5}.");
        }

        [Fact]
        public void A_collection_does_not_contain_a_single_element_collection()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().BeSupersetOf(new[] { 4 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to be a superset of 4 because we do.");
        }

        [Fact]
        public void A_collection_does_not_contain_other_collection_with_assertion_scope()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().BeSupersetOf(new[] { 4 }).And.BeSupersetOf(new[] { 5, 6 });
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*to be a superset of 4*to be a superset of {5, 6}*");
        }

        [Fact]
        public void A_collection_is_not_a_superset_of_an_empty_collection()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().BeSupersetOf(new int[0]);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify containment against an empty collection*");
        }

        [Fact]
        public void A_null_collection_is_not_a_superset_of_a_collection()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().BeSupersetOf(new[] { 1, 2 }, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection to be a superset of {1, 2} *failure message*, but found <null>.");
        }
    }
}
