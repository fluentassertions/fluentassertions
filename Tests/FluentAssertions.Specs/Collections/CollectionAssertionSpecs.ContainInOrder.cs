using System;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The [Not]ContainInOrder specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class ContainInOrder
    {
        [Fact]
        public void When_two_collections_contain_the_same_items_in_the_same_order_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 2, 3 };

            // Act / Assert
            collection.Should().ContainInOrder(1, 2, 3);
        }

        [Fact]
        public void When_collection_contains_null_value_it_should_not_throw()
        {
            // Arrange
            var collection = new object[] { 1, null, 2, "string" };

            // Act / Assert
            collection.Should().ContainInOrder(1, null, "string");
        }

        [Fact]
        public void When_the_first_collection_contains_a_duplicate_item_without_affecting_the_order_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3, 2 };

            // Act / Assert
            collection.Should().ContainInOrder(1, 2, 3);
        }

        [Fact]
        public void When_two_collections_contain_the_same_duplicate_items_in_the_same_order_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 1, 2, 12, 2, 2 };

            // Act / Assert
            collection.Should().ContainInOrder(1, 2, 1, 2, 12, 2, 2);
        }

        [Fact]
        public void When_a_collection_does_not_contain_a_range_twice_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 1, 3, 12, 2, 2 };

            // Act
            Action act = () => collection.Should().ContainInOrder(1, 2, 1, 1, 2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 1, 3, 12, 2, 2} to contain items {1, 2, 1, 1, 2} in order, but 1 (index 3) did not appear (in the right order).");
        }

        [Fact]
        public void When_two_collections_contain_the_same_items_but_in_different_order_it_should_throw_with_a_clear_explanation()
        {
            // Act
            Action act = () => new[] { 1, 2, 3 }.Should().ContainInOrder(new[] { 3, 1 }, "because we said so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to contain items {3, 1} in order because we said so, but 1 (index 1) did not appear (in the right order).");
        }

        [Fact]
        public void When_a_collection_does_not_contain_an_ordered_item_it_should_throw_with_a_clear_explanation()
        {
            // Act
            Action act = () => new[] { 1, 2, 3 }.Should().ContainInOrder(new[] { 4, 1 }, "we failed");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to contain items {4, 1} in order because we failed, " +
                "but 4 (index 0) did not appear (in the right order).");
        }

        [Fact]
        public void When_a_collection_does_not_contain_items_with_assertion_scope_all_items_are_reported()
        {
            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                new[] { 1, 2, 3 }.Should().ContainInOrder(4).And.ContainInOrder(5);
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*but 4 (index 0)*but 5 (index 0)*");
        }

        [Fact]
        public void When_passing_in_null_while_checking_for_ordered_containment_it_should_throw_with_a_clear_explanation()
        {
            // Act
            Action act = () => new[] { 1, 2, 3 }.Should().ContainInOrder(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify ordered containment against a <null> collection.*");
        }

        [Fact]
        public void Collections_contain_the_empty_sequence()
        {
            // Assert
            new[] { 1 }.Should().ContainInOrder();
        }

        [Fact]
        public void Collections_do_not_not_contain_the_empty_sequence()
        {
            // Assert
            new[] { 1 }.Should().NotContainInOrder();
        }

        [Fact]
        public void When_asserting_collection_contains_some_values_in_order_but_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] ints = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                ints.Should().ContainInOrder(new[] { 4 }, "because we're checking how it reacts to a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected ints to contain {4} in order because we're checking how it reacts to a null subject, but found <null>.");
        }
    }

    public class NotContainInOrder
    {
        [Fact]
        public void When_two_collections_contain_the_same_items_but_in_different_order_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().NotContainInOrder(2, 1);
        }

        [Fact]
        public void When_a_collection_does_not_contain_an_ordered_item_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().NotContainInOrder(4, 1);
        }

        [Fact]
        public void When_a_collection_contains_less_items_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2 };

            // Act / Assert
            collection.Should().NotContainInOrder(1, 2, 3);
        }

        [Fact]
        public void When_a_collection_does_not_contain_a_range_twice_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 1, 3, 12, 2, 2 };

            // Act / Assert
            collection.Should().NotContainInOrder(1, 2, 1, 1, 2);
        }

        [Fact]
        public void When_asserting_collection_does_not_contain_some_values_in_order_but_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () => collection.Should().NotContainInOrder(4);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Cannot verify absence of ordered containment in a <null> collection.");
        }

        [Fact]
        public void When_two_collections_contain_the_same_items_in_the_same_order_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 2, 3 };

            // Act
            Action act = () => collection.Should().NotContainInOrder(new[] { 1, 2, 3 }, "that's what we expect");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 2, 3} to not contain items {1, 2, 3} in order because that's what we expect, " +
                "but items appeared in order ending at index 3.");
        }

        [Fact]
        public void When_collection_is_null_then_not_contain_in_order_should_fail()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotContainInOrder(new[] { 1, 2, 3 }, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Cannot verify absence of ordered containment in a <null> collection.");
        }

        [Fact]
        public void When_collection_contains_contain_the_same_items_in_the_same_order_with_null_value_it_should_throw()
        {
            // Arrange
            var collection = new object[] { 1, null, 2, "string" };

            // Act
            Action act = () => collection.Should().NotContainInOrder(1, null, "string");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, <null>, 2, \"string\"} to not contain items {1, <null>, \"string\"} in order, " +
                "but items appeared in order ending at index 3.");
        }

        [Fact]
        public void When_the_first_collection_contains_a_duplicate_item_without_affecting_the_order_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3, 2 };

            // Act
            Action act = () => collection.Should().NotContainInOrder(1, 2, 3);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3, 2} to not contain items {1, 2, 3} in order, " +
                "but items appeared in order ending at index 2.");
        }

        [Fact]
        public void When_two_collections_contain_the_same_duplicate_items_in_the_same_order_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 1, 2, 12, 2, 2 };

            // Act
            Action act = () => collection.Should().NotContainInOrder(1, 2, 1, 2, 12, 2, 2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 1, 2, 12, 2, 2} to not contain items {1, 2, 1, 2, 12, 2, 2} in order, " +
                "but items appeared in order ending at index 6.");
        }

        [Fact]
        public void When_passing_in_null_while_checking_for_absence_of_ordered_containment_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotContainInOrder(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify absence of ordered containment against a <null> collection.*");
        }
    }
}
