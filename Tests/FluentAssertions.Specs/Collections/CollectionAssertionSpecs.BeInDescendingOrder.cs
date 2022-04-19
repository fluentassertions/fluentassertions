using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections
{
    /// <content>
    /// The [Not]BeInDescendingOrder specs.
    /// </content>
    public partial class CollectionAssertionSpecs
    {
        public class BeInDescendingOrder
        {
            [Fact]
            public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_it_should_succeed()
            {
                // Arrange
                var collection = new[] { "z", "y", "x" };

                // Act / Assert
                collection.Should().BeInDescendingOrder();
            }

            [Fact]
            public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_it_should_throw()
            {
                // Arrange
                var collection = new[] { "z", "x", "y" };

                // Act
                Action action = () => collection.Should().BeInDescendingOrder("because letters are ordered");

                // Assert
                action.Should().Throw<XunitException>()
                    .WithMessage("Expected collection to be in descending order because letters are ordered," +
                        " but found {\"z\", \"x\", \"y\"} where item at index 1 is in wrong order.");
            }
        }

        public class NotBeInDescendingOrder
        {
            [Fact]
            public void When_asserting_the_items_in_an_unordered_collection_are_not_in_descending_order_it_should_succeed()
            {
                // Arrange
                var collection = new[] { "x", "y", "x" };

                // Act / Assert
                collection.Should().NotBeInDescendingOrder();
            }

            [Fact]
            public void When_asserting_the_items_in_an_unordered_collection_are_not_in_descending_order_using_the_given_comparer_it_should_succeed()
            {
                // Arrange
                var collection = new[] { "x", "y", "x" };

                // Act / Assert
                collection.Should().NotBeInDescendingOrder(Comparer<object>.Default);
            }

            [Fact]
            public void When_asserting_the_items_in_a_descending_ordered_collection_are_not_in_descending_order_it_should_throw()
            {
                // Arrange
                var collection = new[] { "c", "b", "a" };

                // Act
                Action action = () => collection.Should().NotBeInDescendingOrder("because numbers are not ordered");

                // Assert
                action.Should().Throw<XunitException>()
                    .WithMessage("Did not expect collection to be in descending order because numbers are not ordered," +
                        " but found {\"c\", \"b\", \"a\"}.");
            }

            [Fact]
            public void When_asserting_the_items_in_a_descending_ordered_collection_are_not_in_descending_order_using_the_given_comparer_it_should_throw()
            {
                // Arrange
                var collection = new[] { "c", "b", "a" };

                // Act
                Action action = () => collection.Should().NotBeInDescendingOrder(Comparer<object>.Default, "because numbers are not ordered");

                // Assert
                action.Should().Throw<XunitException>()
                    .WithMessage("Did not expect collection to be in descending order because numbers are not ordered," +
                        " but found {\"c\", \"b\", \"a\"}.");
            }
        }
    }
}
