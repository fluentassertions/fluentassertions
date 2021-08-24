using System;
using System.Collections.Generic;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections
{
    /// <content>
    /// The [Not]BeInAscendingOrder specs.
    /// </content>
    public partial class CollectionAssertionSpecs
    {
        #region Be In AscendingOrder

        [Fact]
        public void When_asserting_a_null_collection_to_be_in_ascending_order_it_should_throw()
        {
            // Arrange
            List<int> result = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                result.Should().BeInAscendingOrder();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*but found <null>*");
        }

        [Fact]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 2, 3 };

            // Act / Assert
            collection.Should().BeInAscendingOrder();
        }

        [Fact]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 2, 3 };

            // Act / Assert
            collection.Should().BeInAscendingOrder(Comparer<int>.Default);
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 6, 12, 15, 12, 17, 26 };

            // Act
            Action action = () => collection.Should().BeInAscendingOrder("because numbers are ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to be in ascending order because numbers are ordered," +
                    " but found {1, 6, 12, 15, 12, 17, 26} where item at index 3 is in wrong order.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 6, 12, 15, 12, 17, 26 };

            // Act
            Action action = () => collection.Should().BeInAscendingOrder(Comparer<int>.Default, "because numbers are ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to be in ascending order because numbers are ordered," +
                    " but found {1, 6, 12, 15, 12, 17, 26} where item at index 3 is in wrong order.");
        }

        #endregion

        #region Not Be In Ascending Order

        [Fact]
        public void When_asserting_a_null_collection_to_not_be_in_ascending_order_it_should_throw()
        {
            // Arrange
            List<int> result = null;

            // Act
            Action act = () => result.Should().NotBeInAscendingOrder();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*but found <null>*");
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_not_in_ascending_order_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 5, 3 };

            // Act / Assert
            collection.Should().NotBeInAscendingOrder();
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_not_in_ascending_order_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 5, 3 };

            // Act / Assert
            collection.Should().NotBeInAscendingOrder(Comparer<int>.Default);
        }

        [Fact]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_not_in_ascending_order_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 2, 3 };

            // Act
            Action action = () => collection.Should().NotBeInAscendingOrder("because numbers are not ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in ascending order because numbers are not ordered," +
                    " but found {1, 2, 2, 3}.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_not_in_ascending_order_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 2, 3 };

            // Act
            Action action = () => collection.Should().NotBeInAscendingOrder(Comparer<int>.Default, "because numbers are not ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in ascending order because numbers are not ordered," +
                    " but found {1, 2, 2, 3}.");
        }

        #endregion
    }
}
