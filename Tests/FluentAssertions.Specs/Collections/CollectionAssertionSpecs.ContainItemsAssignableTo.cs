using System;
using System.Collections.Generic;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections
{
    /// <content>
    /// The ContainItemsAssignableTo specs.
    /// </content>
    public partial class CollectionAssertionSpecs
    {
        #region Contain Items Assignable To

        [Fact]
        public void Should_succeed_when_asserting_collection_with_all_items_of_same_type_only_contains_item_of_one_type()
        {
            // Arrange
            var collection = new[] { "1", "2", "3" };

            // Act / Assert
            collection.Should().ContainItemsAssignableTo<string>();
        }

        [Fact]
        public void Should_fail_when_asserting_collection_with_items_of_different_types_only_contains_item_of_one_type()
        {
            // Arrange
            var collection = new List<object>
            {
                1,
                "2"
            };

            // Act
            Action act = () => collection.Should().ContainItemsAssignableTo<string>();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_collection_contains_anything_other_than_strings_it_should_throw_and_report_details()
        {
            // Arrange
            var collection = new List<object>
            {
                1,
                "2"
            };

            // Act
            Action act = () => collection.Should().ContainItemsAssignableTo<string>();

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain only items of type System.String, but item 1 at index 0 is of type System.Int32.");
        }

        [Fact]
        public void When_a_collection_contains_anything_other_than_strings_it_should_use_the_reason()
        {
            // Arrange
            var collection = new List<object>
            {
                1,
                "2"
            };

            // Act
            Action act = () => collection.Should().ContainItemsAssignableTo<string>(
                "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to contain only items of type System.String because we want to test the failure message" +
                        ", but item 1 at index 0 is of type System.Int32.");
        }

        [Fact]
        public void When_asserting_collection_contains_item_assignable_to_against_null_collection_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().ContainItemsAssignableTo<string>("because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain element assignable to type System.String because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion
    }
}
