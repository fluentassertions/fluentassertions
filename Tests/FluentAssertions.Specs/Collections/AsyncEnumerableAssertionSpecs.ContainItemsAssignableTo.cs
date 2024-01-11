using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <content>
/// The ContainItemsAssignableTo specs.
/// </content>
public partial class AsyncEnumerableAssertionSpecs
{
    public class ContainItemsAssignableTo
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_with_all_items_of_same_type_only_contains_item_of_one_type()
        {
            // Arrange
            string[] collection = ["1", "2", "3"];

            // Act / Assert
            collection.ToAsyncEnumerable().Should().ContainItemsAssignableTo<string>();
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_with_items_of_different_types_contains_item_of_expected_type()
        {
            // Arrange
            var collection = new List<object>
            {
                1,
                "2"
            };

            // Act / Assert
            collection.ToAsyncEnumerable().Should().ContainItemsAssignableTo<string>();
        }

        [Fact]
        public void When_asserting_collection_contains_item_assignable_to_against_null_collection_it_should_throw()
        {
            // Arrange
            IAsyncEnumerable<int> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().ContainItemsAssignableTo<string>("because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain at least one element assignable to type \"System.String\" because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_a_collection_is_empty_an_exception_should_be_thrown()
        {
            // Arrange
            int[] array = [];
            var collection = array.ToAsyncEnumerable();

            // Act
            Action act = () => collection.Should().ContainItemsAssignableTo<int>();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to contain at least one element assignable to type \"System.Int32\", but found {empty}.");
        }

        [Fact]
        public void Should_throw_exception_when_asserting_collection_for_missing_item_type()
        {
            var array = new object[] { "1", 1.0m };
            var collection = array.ToAsyncEnumerable();

            Action act = () => collection.Should().ContainItemsAssignableTo<int>();

            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to contain at least one element assignable to type \"System.Int32\", but found {System.String, System.Decimal}.");
        }
    }
}
