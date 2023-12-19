using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The [Not]BeInAscendingOrder specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class BeInAscendingOrder
    {
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
        public void
            When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_given_comparer_it_should_succeed()
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
        public void
            When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_given_comparer_it_should_throw()
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

        [Fact]
        public void Items_can_be_ordered_by_the_identity_function()
        {
            // Arrange
            var collection = new[] { 1, 2 };

            // Act
            Action action = () => collection.Should().BeInAscendingOrder(x => x);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_empty_collection_with_no_parameters_ordered_in_ascending_it_should_succeed()
        {
            // Arrange
            var collection = new int[] { };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_empty_collection_by_property_expression_ordered_in_ascending_it_should_succeed()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Number);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_single_element_collection_with_no_parameters_ordered_in_ascending_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 42 };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_single_element_collection_by_property_expression_ordered_in_ascending_it_should_succeed()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new() { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Number);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Can_use_a_cast_expression_in_the_ordering_expression()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new() { Text = "a", Number = 1 }
            };

            // Act & Assert
            collection.Should().BeInAscendingOrder(o => (float)o.Number);
        }

        [Fact]
        public void Can_use_an_index_into_a_list_in_the_ordering_expression()
        {
            // Arrange
            var collection = new[]
            {
                new List<SomeClass> { new() { Text = "a", Number = 1 } }
            };

            // Act & Assert
            collection.Should().BeInAscendingOrder(o => o[0].Number);
        }

        [Fact]
        public void Can_use_an_index_into_an_array_in_the_ordering_expression()
        {
            // Arrange
            var collection = new[]
            {
                new[] { new SomeClass { Text = "a", Number = 1 } }
            };

            // Act & Assert
            collection.Should().BeInAscendingOrder(o => o[0].Number);
        }

        [Fact]
        public void Unsupported_ordering_expressions_are_invalid()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new() { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Number > 1);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Expression <*> cannot be used to select a member.*");
        }

        [Fact]
        public void
            When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_specified_property_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text, "it should be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*a*b*c*");
        }

        [Fact]
        public void
            When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_throw()
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
                collection.Should().BeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*a*b*c*");
        }

        [Fact]
        public void
            When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_specified_property_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Numeric);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Numeric, Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_in_ascending_order_it_should_succeed()
        {
            // Arrange
            string[] strings = { "alpha", "beta", "theta" };

            // Act
            Action act = () => strings.Should().BeInAscendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_ascending_order_it_should_throw()
        {
            // Arrange
            string[] strings = { "theta", "alpha", "beta" };

            // Act
            Action act = () => strings.Should().BeInAscendingOrder("of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*ascending*of reasons*index 0*");
        }

        [Fact]
        public void When_strings_are_in_ascending_order_according_to_a_custom_comparer_it_should_succeed()
        {
            // Arrange
            string[] strings = { "alpha", "beta", "theta" };

            // Act
            Action act = () => strings.Should().BeInAscendingOrder(new ByLastCharacterComparer());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_ascending_order_according_to_a_custom_comparer_it_should_throw()
        {
            // Arrange
            string[] strings = { "dennis", "roy", "thomas" };

            // Act
            Action act = () => strings.Should().BeInAscendingOrder(new ByLastCharacterComparer(), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*ascending*of reasons*index 1*");
        }

        [Fact]
        public void When_strings_are_in_ascending_order_according_to_a_custom_lambda_it_should_succeed()
        {
            // Arrange
            string[] strings = { "alpha", "beta", "theta" };

            // Act
            Action act = () => strings.Should().BeInAscendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_ascending_order_according_to_a_custom_lambda_it_should_throw()
        {
            // Arrange
            string[] strings = { "dennis", "roy", "thomas" };

            // Act
            Action act = () =>
                strings.Should().BeInAscendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*ascending*of reasons*index 1*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_null_collection_are_ordered_using_the_specified_property_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Text*found*null*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_null_collection_are_ordered_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(Comparer<SomeClass>.Default);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*found*null*");
        }

        [Fact]
        public void
            When_asserting_the_items_in_a_null_collection_are_ordered_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*Text*found*null*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_collection_are_ordered_and_the_specified_property_is_null_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().BeInAscendingOrder((Expression<Func<SomeClass, string>>)null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert collection ordering without specifying a property*")
                .WithParameterName("propertyExpression");
        }

        [Fact]
        public void When_asserting_the_items_in_a_collection_are_ordered_and_the_given_comparer_is_null_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(comparer: null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert collection ordering without specifying a comparer*")
                .WithParameterName("comparer");
        }

        [Fact]
        public void When_asserting_the_items_in_ay_collection_are_ordered_using_an_invalid_property_expression_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.GetHashCode());

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Expression*o.GetHashCode()*cannot be used to select a member*");
        }
    }

    public class NotBeInAscendingOrder
    {
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
        public void
            When_asserting_the_items_in_an_unordered_collection_are_not_in_ascending_order_using_the_given_comparer_it_should_succeed()
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
        public void
            When_asserting_the_items_in_an_ascendingly_ordered_collection_are_not_in_ascending_order_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 2, 3 };

            // Act
            Action action = () =>
                collection.Should().NotBeInAscendingOrder(Comparer<int>.Default, "because numbers are not ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in ascending order because numbers are not ordered," +
                    " but found {1, 2, 2, 3}.");
        }

        [Fact]
        public void When_asserting_empty_collection_by_property_expression_to_not_be_ordered_in_ascending_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Number);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection {empty} to not be ordered \"by Number\" and not result in {empty}.");
        }

        [Fact]
        public void When_asserting_empty_collection_with_no_parameters_not_be_ordered_in_ascending_it_should_throw()
        {
            // Arrange
            var collection = new int[] { };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder("because I say {0}", "so");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in ascending order because I say so, but found {empty}.");
        }

        [Fact]
        public void When_asserting_single_element_collection_with_no_parameters_not_be_ordered_in_ascending_it_should_throw()
        {
            // Arrange
            var collection = new[] { 42 };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in ascending order, but found {42}.");
        }

        [Fact]
        public void
            When_asserting_single_element_collection_by_property_expression_to_not_be_ordered_in_ascending_it_should_throw()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new() { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Number);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_the_items_in_a_ascending_ordered_collection_are_not_ordered_ascending_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(Comparer<int>.Default, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in ascending order*should not be sorted*1*2*3*");
        }

        [Fact]
        public void
            When_asserting_the_items_not_in_an_ascendingly_ordered_collection_are_not_ordered_ascending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 3, 2, 1 };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_the_items_in_a_ascending_ordered_collection_are_not_ordered_ascending_using_the_specified_property_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "a", Numeric = 3 },
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 }
            };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Text, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*a*b*c*not be ordered*Text*should not be sorted*a*b*c*");
        }

        [Fact]
        public void
            When_asserting_the_items_in_an_ordered_collection_are_not_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "A", Numeric = 1 },
                new { Text = "b", Numeric = 2 },
                new { Text = "C", Numeric = 3 }
            };

            // Act
            Action act = () =>
                collection.Should()
                    .NotBeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*A*b*C*not be ordered*Text*should not be sorted*A*b*C*");
        }

        [Fact]
        public void
            When_asserting_the_items_not_in_an_ascendingly_ordered_collection_are_not_ordered_ascending_using_the_specified_property_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 3 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 1 }
            };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Numeric);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_the_items_not_in_an_ascendingly_ordered_collection_are_not_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 3 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 1 }
            };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Numeric, Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_not_in_ascending_order_it_should_succeed()
        {
            // Arrange
            string[] strings = { "beta", "alpha", "theta" };

            // Act
            Action act = () => strings.Should().NotBeInAscendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_in_ascending_order_unexpectedly_it_should_throw()
        {
            // Arrange
            string[] strings = { "alpha", "beta", "theta" };

            // Act
            Action act = () => strings.Should().NotBeInAscendingOrder("of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*ascending*of reasons*but found*");
        }

        [Fact]
        public void When_strings_are_not_in_ascending_order_according_to_a_custom_comparer_it_should_succeed()
        {
            // Arrange
            string[] strings = { "dennis", "roy", "barbara" };

            // Act
            Action act = () => strings.Should().NotBeInAscendingOrder(new ByLastCharacterComparer());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_unexpectedly_in_ascending_order_according_to_a_custom_comparer_it_should_throw()
        {
            // Arrange
            string[] strings = { "dennis", "thomas", "roy" };

            // Act
            Action act = () => strings.Should().NotBeInAscendingOrder(new ByLastCharacterComparer(), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*ascending*of reasons*but found*");
        }

        [Fact]
        public void When_strings_are_not_in_ascending_order_according_to_a_custom_lambda_it_should_succeed()
        {
            // Arrange
            string[] strings = { "roy", "dennis", "thomas" };

            // Act
            Action act = () => strings.Should().NotBeInAscendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_strings_are_unexpectedly_in_ascending_order_according_to_a_custom_lambda_it_should_throw()
        {
            // Arrange
            string[] strings = { "barbara", "dennis", "roy" };

            // Act
            Action act = () =>
                strings.Should().NotBeInAscendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*ascending*of reasons*but found*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_null_collection_are_not_ordered_using_the_specified_property_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotBeInAscendingOrder(o => o.Text);
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Text*found*null*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_null_collection_are_not_ordered_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotBeInAscendingOrder(Comparer<SomeClass>.Default);
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*found*null*");
        }

        [Fact]
        public void
            When_asserting_the_items_in_a_null_collection_are_not_ordered_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Text*found*null*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_collection_are_not_ordered_and_the_specified_property_is_null_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder((Expression<Func<SomeClass, string>>)null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert collection ordering without specifying a property*propertyExpression*");
        }

        [Fact]
        public void When_asserting_the_items_in_a_collection_are_not_ordered_and_the_given_comparer_is_null_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(comparer: null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert collection ordering without specifying a comparer*comparer*");
        }

        [Fact]
        public void
            When_asserting_the_items_in_ay_collection_are_not_ordered_using_an_invalid_property_expression_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.GetHashCode());

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Expression*o.GetHashCode()*cannot be used to select a member*");
        }
    }
}
