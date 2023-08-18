using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

public partial class CollectionAssertionSpecs
{
    public class NotContainItemsAssignableTo
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_not_contains_items_assignable_to_type()
        {
            // Arrange
            var collection = new[] { "1", "2", "3" };

            // Act / Assert
            collection.Should().NotContainItemsAssignableTo<int>();
        }

        [Fact]
        public void Should_throw_when_asserting_collection_contains_item_assignable_to_type()
        {
            // Arrange
            var collection = new object[] { 1, "2", "3" };

            // Act
            var act = () => collection.Should().NotContainItemsAssignableTo<int>();

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage(
                    "Expected collection to not contain any elements assignable to type \"System.Int32\", but found {System.Int32, System.String, System.String}.");
        }

        [Fact]
        public void Should_throw_when_passed_type_argument_is_null()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            var act = () => collection.Should().NotContainItemsAssignableTo(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_collection_is_null()
        {
            // Arrange
            int[] collection = null;

            // Act
            var act = () => collection.Should().NotContainItemsAssignableTo<int>();

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage(
                    "Expected collection to not contain any elements assignable to type \"System.Int32\", but found <null>.");
        }
    }
}
