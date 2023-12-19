using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

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

        [Fact]
        public void When_asserting_empty_collection_by_property_expression_ordered_in_descending_it_should_succeed()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Number);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_empty_collection_with_no_parameters_ordered_in_descending_it_should_succeed()
        {
            // Arrange
            var collection = new int[] { };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_single_element_collection_with_no_parameters_ordered_in_descending_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 42 };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_single_element_collection_by_property_expression_ordered_in_descending_it_should_succeed()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new() { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Number);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { "z", "x", "y" };

            // Act
            Action action = () =>
                collection.Should().BeInDescendingOrder(Comparer<object>.Default, "because letters are ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to be in descending order because letters are ordered," +
                    " but found {\"z\", \"x\", \"y\"} where item at index 1 is in wrong order.");
        }

        [Fact]
        public void
            When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[] { "z", "y", "x" };

            // Act / Assert
            collection.Should().BeInDescendingOrder(Comparer<object>.Default);
        }

        [Fact]
        public void
            When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_specified_property_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Text, "it should be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*c*b*a*");
        }

        [Fact]
        public void
            When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () =>
                collection.Should().BeInDescendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*c*b*a*");
        }

        [Fact]
        public void
            When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_specified_property_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 3 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 1 }
            };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Numeric);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 3 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 1 }
            };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Numeric, Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_in_descending_order_it_should_succeed()
        {
            // Arrange
            string[] strings = { "theta", "beta", "alpha" };

            // Act
            Action act = () => strings.Should().BeInDescendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_descending_order_it_should_throw()
        {
            // Arrange
            string[] strings = { "theta", "alpha", "beta" };

            // Act
            Action act = () => strings.Should().BeInDescendingOrder("of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*descending*of reasons*index 1*");
        }

        [Fact]
        public void When_strings_are_in_descending_order_based_on_a_custom_comparer_it_should_succeed()
        {
            // Arrange
            string[] strings = { "roy", "dennis", "barbara" };

            // Act
            Action act = () => strings.Should().BeInDescendingOrder(new ByLastCharacterComparer());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_descending_order_based_on_a_custom_comparer_it_should_throw()
        {
            // Arrange
            string[] strings = { "dennis", "roy", "barbara" };

            // Act
            Action act = () => strings.Should().BeInDescendingOrder(new ByLastCharacterComparer(), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*descending*of reasons*index 0*");
        }

        [Fact]
        public void When_strings_are_in_descending_order_based_on_a_custom_lambda_it_should_succeed()
        {
            // Arrange
            string[] strings = { "roy", "dennis", "barbara" };

            // Act
            Action act = () => strings.Should().BeInDescendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_descending_order_based_on_a_custom_lambda_it_should_throw()
        {
            // Arrange
            string[] strings = { "dennis", "roy", "barbara" };

            // Act
            Action act = () =>
                strings.Should().BeInDescendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*descending*of reasons*index 0*");
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
        public void
            When_asserting_the_items_in_an_unordered_collection_are_not_in_descending_order_using_the_given_comparer_it_should_succeed()
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
        public void
            When_asserting_the_items_in_a_descending_ordered_collection_are_not_in_descending_order_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { "c", "b", "a" };

            // Act
            Action action = () =>
                collection.Should().NotBeInDescendingOrder(Comparer<object>.Default, "because numbers are not ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in descending order because numbers are not ordered," +
                    " but found {\"c\", \"b\", \"a\"}.");
        }

        [Fact]
        public void When_asserting_empty_collection_by_property_expression_to_not_be_ordered_in_descending_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Number);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection {empty} to not be ordered \"by Number\" and not result in {empty}.");
        }

        [Fact]
        public void When_asserting_empty_collection_with_no_parameters_not_be_ordered_in_descending_it_should_throw()
        {
            // Arrange
            var collection = new int[] { };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder("because I say {0}", "so");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in descending order because I say so, but found {empty}.");
        }

        [Fact]
        public void When_asserting_single_element_collection_with_no_parameters_not_be_ordered_in_descending_it_should_throw()
        {
            // Arrange
            var collection = new[] { 42 };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in descending order, but found {42}.");
        }

        [Fact]
        public void
            When_asserting_single_element_collection_by_property_expression_to_not_be_ordered_in_descending_it_should_throw()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new() { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Number);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_the_items_in_a_descending_ordered_collection_are_not_ordered_descending_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { 3, 2, 1 };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(Comparer<int>.Default, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in descending order*should not be sorted*3*2*1*");
        }

        [Fact]
        public void
            When_asserting_the_items_not_in_an_descendingly_ordered_collection_are_not_ordered_descending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_the_items_in_a_descending_ordered_collection_are_not_ordered_descending_using_the_specified_property_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "c", Numeric = 3 },
                new { Text = "b", Numeric = 1 },
                new { Text = "a", Numeric = 2 }
            };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Text, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*b*c*a*not be ordered*Text*should not be sorted*c*b*a*");
        }

        [Fact]
        public void
            When_asserting_the_items_in_an_ordered_collection_are_not_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "C", Numeric = 1 },
                new { Text = "b", Numeric = 2 },
                new { Text = "A", Numeric = 3 }
            };

            // Act
            Action act = () =>
                collection.Should()
                    .NotBeInDescendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*C*b*A*not be ordered*Text*should not be sorted*C*b*A*");
        }

        [Fact]
        public void
            When_asserting_the_items_not_in_an_descendingly_ordered_collection_are_not_ordered_descending_using_the_specified_property_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Numeric);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_the_items_not_in_an_descendingly_ordered_collection_are_not_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Numeric, Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_descending_order_it_should_succeed()
        {
            // Arrange
            string[] strings = { "beta", "theta", "alpha" };

            // Act
            Action act = () => strings.Should().NotBeInDescendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_unexpectedly_in_descending_order_it_should_throw()
        {
            // Arrange
            string[] strings = { "theta", "beta", "alpha" };

            // Act
            Action act = () => strings.Should().NotBeInDescendingOrder("of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*descending*of reasons*but found*");
        }

        [Fact]
        public void When_strings_are_not_in_descending_order_based_on_a_custom_comparer_it_should_succeed()
        {
            // Arrange
            string[] strings = { "roy", "barbara", "dennis" };

            // Act
            Action act = () => strings.Should().NotBeInDescendingOrder(new ByLastCharacterComparer());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_unexpectedly_in_descending_order_based_on_a_custom_comparer_it_should_throw()
        {
            // Arrange
            string[] strings = { "roy", "dennis", "barbara" };

            // Act
            Action act = () => strings.Should().NotBeInDescendingOrder(new ByLastCharacterComparer(), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*descending*of reasons*but found*");
        }

        [Fact]
        public void When_strings_are_not_in_descending_order_based_on_a_custom_lambda_it_should_succeed()
        {
            // Arrange
            string[] strings = { "dennis", "roy", "barbara" };

            // Act
            Action act = () => strings.Should().NotBeInDescendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_unexpectedly_in_descending_order_based_on_a_custom_lambda_it_should_throw()
        {
            // Arrange
            string[] strings = { "roy", "dennis", "barbara" };

            // Act
            Action act = () =>
                strings.Should().NotBeInDescendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*descending*of reasons*but found*");
        }
    }
}
