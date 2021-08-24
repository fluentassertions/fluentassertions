using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections
{
    /// <summary>
    /// Collection assertion specs.
    /// </summary>
    public partial class CollectionAssertionSpecs
    {
        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should()
                .HaveCount(3)
                .And
                .HaveElementAt(1, 2)
                .And.NotContain(4);
        }

        #region Be In Ascending/Descending Order

        #region Empty Collection - is always ordered, in both directions

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

        #endregion

        #region Single Element Collection - is always ordered, in both directions

        [Fact]
        public void When_asserting_single_element_collection_with_no_parameters_ordered_in_ascending_it_should_succeed()
        {
            // Arrange
            var collection = new int[] { 42 };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_single_element_collection_with_no_parameters_not_be_ordered_in_ascending_it_should_throw()
        {
            // Arrange
            var collection = new int[] { 42 };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in ascending order, but found {42}.");
        }

        [Fact]
        public void When_asserting_single_element_collection_with_no_parameters_ordered_in_descending_it_should_succeed()
        {
            // Arrange
            var collection = new int[] { 42 };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_single_element_collection_with_no_parameters_not_be_ordered_in_descending_it_should_throw()
        {
            // Arrange
            var collection = new int[] { 42 };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to be in descending order, but found {42}.");
        }

        [Fact]
        public void When_asserting_single_element_collection_by_property_expression_ordered_in_ascending_it_should_succeed()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new SomeClass { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Number);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_single_element_collection_by_property_expression_to_not_be_ordered_in_ascending_it_should_throw()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new SomeClass { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Number);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_single_element_collection_by_property_expression_ordered_in_descending_it_should_succeed()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new SomeClass { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Number);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_single_element_collection_by_property_expression_to_not_be_ordered_in_descending_it_should_throw()
        {
            // Arrange
            var collection = new SomeClass[]
            {
                new SomeClass { Text = "a", Number = 1 }
            };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Number);

            // Assert
            act.Should().Throw<XunitException>();
        }

        #endregion

        #region Multi Element Collection - No Parameter / Comparer

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { "z", "x", "y" };

            // Act
            Action action = () => collection.Should().BeInDescendingOrder(Comparer<object>.Default, "because letters are ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to be in descending order because letters are ordered," +
                    " but found {\"z\", \"x\", \"y\"} where item at index 1 is in wrong order.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[] { "z", "y", "x" };

            // Act / Assert
            collection.Should().BeInDescendingOrder(Comparer<object>.Default);
        }

        [Fact]
        public void When_asserting_the_items_in_a_descending_ordered_collection_are_not_ordered_descending_using_the_given_comparer_it_should_throw()
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
        public void When_asserting_the_items_not_in_an_descendingly_ordered_collection_are_not_ordered_descending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_the_items_in_a_ascending_ordered_collection_are_not_ordered_ascending_using_the_given_comparer_it_should_throw()
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
        public void When_asserting_the_items_not_in_an_ascendingly_ordered_collection_are_not_ordered_ascending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 3, 2, 1 };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(Comparer<int>.Default);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region Multi Element Collection - Property Expression

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_specified_property_it_should_throw()
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
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*a*b*c*");
        }

        [Fact]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_specified_property_it_should_succeed()
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
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
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
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_specified_property_it_should_throw()
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
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*c*b*a*");
        }

        [Fact]
        public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_specified_property_it_should_succeed()
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
        public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
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
        public void When_asserting_the_items_in_a_descending_ordered_collection_are_not_ordered_descending_using_the_specified_property_it_should_throw()
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
        public void When_asserting_the_items_in_an_ordered_collection_are_not_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "C", Numeric = 1 },
                new { Text = "b", Numeric = 2 },
                new { Text = "A", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().NotBeInDescendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*C*b*A*not be ordered*Text*should not be sorted*C*b*A*");
        }

        [Fact]
        public void When_asserting_the_items_not_in_an_descendingly_ordered_collection_are_not_ordered_descending_using_the_specified_property_it_should_succeed()
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
        public void When_asserting_the_items_not_in_an_descendingly_ordered_collection_are_not_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
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
        public void When_asserting_the_items_in_a_ascending_ordered_collection_are_not_ordered_ascending_using_the_specified_property_it_should_throw()
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
        public void When_asserting_the_items_in_an_ordered_collection_are_not_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[]
            {
                new { Text = "A", Numeric = 1 },
                new { Text = "b", Numeric = 2 },
                new { Text = "C", Numeric = 3 }
            };

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should not be sorted");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*A*b*C*not be ordered*Text*should not be sorted*A*b*C*");
        }

        [Fact]
        public void When_asserting_the_items_not_in_an_ascendingly_ordered_collection_are_not_ordered_ascending_using_the_specified_property_it_should_succeed()
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
        public void When_asserting_the_items_not_in_an_ascendingly_ordered_collection_are_not_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
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

        #endregion

        #region Multi Element Collection - Using Lambda

        #region Be In Ascending Order

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
            Action act = () => strings.Should().BeInAscendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*ascending*of reasons*index 1*");
        }

        #endregion

        #region Not Be In Ascending Order

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
            Action act = () => strings.Should().NotBeInAscendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*ascending*of reasons*but found*");
        }

        #endregion

        #region Be In Descending Order

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
            Action act = () => strings.Should().BeInDescendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*descending*of reasons*index 0*");
        }

        #endregion

        #region Not Be In Descending Order

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
            Action act = () => strings.Should().NotBeInDescendingOrder((sut, exp) => sut.Last().CompareTo(exp.Last()), "of {0}", "reasons");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage("Did not expect*descending*of reasons*but found*");
        }

        private class ByLastCharacterComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return x.Last().CompareTo(y.Last());
            }
        }

        #endregion

        #endregion

        #region Null Collection

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
        public void When_asserting_the_items_in_a_null_collection_are_not_ordered_using_the_specified_property_and_the_given_comparer_it_should_throw()
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
        public void When_asserting_the_items_in_a_null_collection_are_ordered_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            // Arrange
            const IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*Text*found*null*");
        }

        #endregion

        #region Null Parameter

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

        #endregion

        #region Invalid Expression

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

        [Fact]
        public void When_asserting_the_items_in_ay_collection_are_not_ordered_using_an_invalid_property_expression_it_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<SomeClass>();

            // Act
            Action act = () => collection.Should().NotBeInAscendingOrder(o => o.GetHashCode());

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Expression*o.GetHashCode()*cannot be used to select a member*");
        }

        #endregion

        #region Then be in order

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_subsequent_ascending_assertion_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (1, "a"),
                (2, "b"),
                (2, "c"),
                (3, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInAscendingOrder(x => x.Item1)
                .And
                .ThenBeInAscendingOrder(x => x.Item2);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_not_ordered_according_to_the_subsequent_ascending_assertion_it_should_fail()
        {
            // Arrange
            var collection = new[]
            {
                (1, "a"),
                (2, "b"),
                (2, "c"),
                (3, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInAscendingOrder(x => x.Item1)
                .And
                .BeInAscendingOrder(x => x.Item2);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection * to be ordered \"by Item2\"*");
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_subsequent_ascending_assertion_with_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (1, "a"),
                (2, "B"),
                (2, "b"),
                (3, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInAscendingOrder(x => x.Item1)
                .And
                .ThenBeInAscendingOrder(x => x.Item2, StringComparer.InvariantCultureIgnoreCase);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_multiple_subsequent_ascending_assertions_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (1, "a", 1.1),
                (2, "b", 1.2),
                (2, "c", 1.3),
                (3, "a", 1.1)
            };

            // Act
            Action action = () => collection.Should()
                .BeInAscendingOrder(x => x.Item1)
                .And
                .ThenBeInAscendingOrder(x => x.Item2)
                .And
                .ThenBeInAscendingOrder(x => x.Item3);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_subsequent_descending_assertion_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (3, "a"),
                (2, "c"),
                (2, "b"),
                (1, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInDescendingOrder(x => x.Item1)
                .And
                .ThenBeInDescendingOrder(x => x.Item2);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_not_ordered_according_to_the_subsequent_descending_assertion_it_should_fail()
        {
            // Arrange
            var collection = new[]
            {
                (3, "a"),
                (2, "c"),
                (2, "b"),
                (1, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInDescendingOrder(x => x.Item1)
                .And
                .BeInDescendingOrder(x => x.Item2);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection * to be ordered \"by Item2\"*");
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_subsequent_descending_assertion_with_comparer_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (3, "a"),
                (2, "b"),
                (2, "B"),
                (1, "a")
            };

            // Act
            Action action = () => collection.Should()
                .BeInDescendingOrder(x => x.Item1)
                .And
                .ThenBeInDescendingOrder(x => x.Item2, StringComparer.InvariantCultureIgnoreCase);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_the_collection_is_ordered_according_to_the_multiple_subsequent_descending_assertions_it_should_succeed()
        {
            // Arrange
            var collection = new[]
            {
                (3, "a", 1.1),
                (2, "c", 1.3),
                (2, "b", 1.2),
                (1, "a", 1.1)
            };

            // Act
            Action action = () => collection.Should()
                .BeInDescendingOrder(x => x.Item1)
                .And
                .ThenBeInDescendingOrder(x => x.Item2)
                .And
                .ThenBeInDescendingOrder(x => x.Item3);

            // Assert
            action.Should().NotThrow();
        }

        #endregion

        #endregion
    }

    internal class CountingGenericEnumerable<TElement> : IEnumerable<TElement>
    {
        private readonly IEnumerable<TElement> backingSet;

        public CountingGenericEnumerable(IEnumerable<TElement> backingSet)
        {
            this.backingSet = backingSet;
            GetEnumeratorCallCount = 0;
        }

        public int GetEnumeratorCallCount { get; private set; }

        public IEnumerator<TElement> GetEnumerator()
        {
            GetEnumeratorCallCount++;
            return backingSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class CountingGenericCollection<TElement> : ICollection<TElement>
    {
        private readonly ICollection<TElement> backingSet;

        public CountingGenericCollection(ICollection<TElement> backingSet)
        {
            this.backingSet = backingSet;
        }

        public int GetEnumeratorCallCount { get; private set; }

        public IEnumerator<TElement> GetEnumerator()
        {
            GetEnumeratorCallCount++;
            return backingSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TElement item) { throw new NotImplementedException(); }

        public void Clear() { throw new NotImplementedException(); }

        public bool Contains(TElement item) { throw new NotImplementedException(); }

        public void CopyTo(TElement[] array, int arrayIndex) { throw new NotImplementedException(); }

        public bool Remove(TElement item) { throw new NotImplementedException(); }

        public int GetCountCallCount { get; private set; }

        public int Count
        {
            get
            {
                GetCountCallCount++;
                return backingSet.Count;
            }
        }

        public bool IsReadOnly { get; private set; }
    }

    internal class TrackingTestEnumerable : IEnumerable<int>
    {
        private readonly int[] values;

        public TrackingTestEnumerable(params int[] values)
        {
            this.values = values;
            Enumerator = new TrackingEnumerator(this.values);
        }

        public TrackingEnumerator Enumerator { get; }

        public IEnumerator<int> GetEnumerator()
        {
            Enumerator.IncreaseEnumerationCount();
            Enumerator.Reset();
            return Enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class TrackingEnumerator : IEnumerator<int>
    {
        private readonly int[] values;
        private int loopCount;
        private int index;

        public TrackingEnumerator(int[] values)
        {
            index = -1;

            this.values = values;
        }

        public int LoopCount
        {
            get { return loopCount; }
        }

        public void IncreaseEnumerationCount()
        {
            loopCount++;
        }

        public bool MoveNext()
        {
            index++;
            return index < values.Length;
        }

        public void Reset()
        {
            index = -1;
        }

        public void Dispose() { }

        object IEnumerator.Current => Current;

        public int Current => values[index];
    }

    internal class OneTimeEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> items;
        private int enumerations;

        public OneTimeEnumerable(params T[] items) => this.items = items;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            if (enumerations++ > 0)
            {
                throw new InvalidOperationException("OneTimeEnumerable can be enumerated one time only");
            }

            return items.GetEnumerator();
        }
    }

    internal class SomeClass
    {
        public string Text { get; set; }

        public int Number { get; set; }
    }
}
