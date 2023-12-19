using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class ContainInOrder
    {
        [Fact]
        public void When_a_collection_does_not_contain_a_range_twice_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "one", "three", "twelve", "two", "two" };

            // Act
            Action act = () => collection.Should().ContainInOrder("one", "two", "one", "one", "two");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"one\", \"three\", \"twelve\", \"two\", \"two\"} to contain items {\"one\", \"two\", \"one\", \"one\", \"two\"} in order, but \"one\" (index 3) did not appear (in the right order).");
        }

        [Fact]
        public void When_a_collection_does_not_contain_an_ordered_item_it_should_throw_with_a_clear_explanation()
        {
            // Act
            Action act = () => new[] { "one", "two", "three" }.Should().ContainInOrder(new[] { "four", "one" }, "we failed");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to contain items {\"four\", \"one\"} in order because we failed, " +
                "but \"four\" (index 0) did not appear (in the right order).");
        }

        [Fact]
        public void When_asserting_collection_contains_some_values_in_order_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> strings = null;

            // Act
            Action act =
                () => strings.Should()
                    .ContainInOrder(new[] { "string4" }, "because we're checking how it reacts to a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected strings to contain {\"string4\"} in order because we're checking how it reacts to a null subject, but found <null>.");
        }

        [Fact]
        public void When_collection_contains_null_value_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", null, "two", "string" };

            // Act / Assert
            collection.Should().ContainInOrder("one", null, "string");
        }

        [Fact]
        public void When_passing_in_null_while_checking_for_ordered_containment_it_should_throw_with_a_clear_explanation()
        {
            // Act
            Action act = () => new[] { "one", "two", "three" }.Should().ContainInOrder(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify ordered containment against a <null> collection.*");
        }

        [Fact]
        public void When_the_first_collection_contains_a_duplicate_item_without_affecting_the_order_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three", "two" };

            // Act / Assert
            collection.Should().ContainInOrder("one", "two", "three");
        }

        [Fact]
        public void When_two_collections_contain_the_same_duplicate_items_in_the_same_order_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "one", "two", "twelve", "two", "two" };

            // Act / Assert
            collection.Should().ContainInOrder("one", "two", "one", "two", "twelve", "two", "two");
        }

        [Fact]
        public void When_two_collections_contain_the_same_items_but_in_different_order_it_should_throw_with_a_clear_explanation()
        {
            // Act
            Action act = () =>
                new[] { "one", "two", "three" }.Should().ContainInOrder(new[] { "three", "one" }, "because we said so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to contain items {\"three\", \"one\"} in order because we said so, but \"one\" (index 1) did not appear (in the right order).");
        }

        [Fact]
        public void When_two_collections_contain_the_same_items_in_the_same_order_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "two", "three" };

            // Act / Assert
            collection.Should().ContainInOrder("one", "two", "three");
        }
    }

    public class NotContainInOrder
    {
        [Fact]
        public void When_two_collections_contain_the_same_items_but_in_different_order_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should().NotContainInOrder("two", "one");
        }

        [Fact]
        public void When_a_collection_does_not_contain_an_ordered_item_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should().NotContainInOrder("four", "one");
        }

        [Fact]
        public void When_a_collection_contains_less_items_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two" };

            // Act / Assert
            collection.Should().NotContainInOrder("one", "two", "three");
        }

        [Fact]
        public void When_a_collection_does_not_contain_a_range_twice_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "one", "three", "twelve", "two", "two" };

            // Act / Assert
            collection.Should().NotContainInOrder("one", "two", "one", "one", "two");
        }

        [Fact]
        public void When_asserting_collection_does_not_contain_some_values_in_order_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().NotContainInOrder("four");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Cannot verify absence of ordered containment in a <null> collection.");
        }

        [Fact]
        public void When_two_collections_contain_the_same_items_in_the_same_order_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "two", "three" };

            // Act
            Action act = () => collection.Should().NotContainInOrder(new[] { "one", "two", "three" }, "that's what we expect");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"two\", \"three\"} to not contain items {\"one\", \"two\", \"three\"} " +
                "in order because that's what we expect, but items appeared in order ending at index 3.");
        }

        [Fact]
        public void When_collection_contains_contain_the_same_items_in_the_same_order_with_null_value_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", null, "two", "three" };

            // Act
            Action act = () => collection.Should().NotContainInOrder("one", null, "three");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", <null>, \"two\", \"three\"} to not contain items {\"one\", <null>, \"three\"} in order, " +
                "but items appeared in order ending at index 3.");
        }

        [Fact]
        public void When_the_first_collection_contains_a_duplicate_item_without_affecting_the_order_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three", "two" };

            // Act
            Action act = () => collection.Should().NotContainInOrder("one", "two", "three");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\", \"two\"} to not contain items {\"one\", \"two\", \"three\"} in order, " +
                "but items appeared in order ending at index 2.");
        }

        [Fact]
        public void When_two_collections_contain_the_same_duplicate_items_in_the_same_order_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "one", "twelve", "two" };

            // Act
            Action act = () => collection.Should().NotContainInOrder("one", "two", "one", "twelve", "two");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"one\", \"twelve\", \"two\"} to not contain items " +
                "{\"one\", \"two\", \"one\", \"twelve\", \"two\"} in order, but items appeared in order ending at index 4.");
        }

        [Fact]
        public void When_passing_in_null_while_checking_for_absence_of_ordered_containment_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().NotContainInOrder(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify absence of ordered containment against a <null> collection.*");
        }
    }
}
