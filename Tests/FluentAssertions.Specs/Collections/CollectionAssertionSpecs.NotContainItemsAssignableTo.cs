using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class CollectionAssertionSpecs
{
    public class NotContainItemsAssignableTo
    {
        [Fact]
        public void Succeeds_when_the_collection_does_not_contain_items_of_the_unexpected_type()
        {
            // Arrange
            var collection = new[] { "1", "2", "3" };

            // Act / Assert
            collection.Should().NotContainItemsAssignableTo<int>();
        }

        [Fact]
        public void Throws_when_the_collection_contains_an_item_of_the_unexpected_type()
        {
            // Arrange
            var collection = new object[] { 1, "2", "3" };

            // Act
            var act = () => collection
                .Should()
                .NotContainItemsAssignableTo<int>(
                    "because we want test that collection does not contain object of {0} type", typeof(int).FullName);

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage(
                    "Expected collection to not contain any elements assignable to type \"System.Int32\" " +
                    "because we want test that collection does not contain object of System.Int32 type, " +
                    "but found {System.Int32, System.String, System.String}.");
        }

        [Fact]
        public void Succeeds_when_collection_is_empty()
        {
            // Arrange
            var collection = Array.Empty<int>();

            // Act / Assert
            collection.Should().NotContainItemsAssignableTo<int>();
        }

        [Fact]
        public void Throws_when_the_passed_type_argument_is_null()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            var act = () => collection.Should().NotContainItemsAssignableTo(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Succeed_when_type_as_parameter_is_valid_type()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().NotContainItemsAssignableTo(typeof(string));
        }

        [Fact]
        public void Throws_when_the_collection_is_null()
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
