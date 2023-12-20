using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The Satisfy specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class Satisfy
    {
        [Fact]
        public void When_collection_element_at_each_position_matches_predicate_at_same_position_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Satisfy(
                element => element == 1,
                element => element == 2,
                element => element == 3);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_element_at_each_position_matches_predicate_at_reverse_position_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Satisfy(
                element => element == 3,
                element => element == 2,
                element => element == 1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_one_element_does_not_have_matching_predicate_Satisfy_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2 };

            // Act
            Action act = () => collection.Should().Satisfy(
                element => element == 3,
                element => element == 1,
                element => element == 2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                @"Expected collection to satisfy all predicates, but:
*The following predicates did not have matching elements:
*(element == 3)");
        }

        [Fact]
        public void
            When_some_predicates_have_multiple_matching_elements_and_most_restricitve_predicates_are_last_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3, 4 };

            // Act
            Action act = () => collection.Should().Satisfy(
                element => element == 1 || element == 2 || element == 3 || element == 4,
                element => element == 1 || element == 2 || element == 3,
                element => element == 1 || element == 2,
                element => element == 1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_some_predicates_have_multiple_matching_elements_and_most_restricitve_predicates_are_first_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3, 4 };

            // Act
            Action act = () => collection.Should().Satisfy(
                element => element == 1,
                element => element == 1 || element == 2,
                element => element == 1 || element == 2 || element == 3,
                element => element == 1 || element == 2 || element == 3 || element == 4);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_second_predicate_matches_first_and_last_element_and_solution_exists_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Satisfy(
                element => element == 1,
                element => element == 1 || element == 3,
                element => element == 2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_assertion_fails_then_failure_message_must_contain_predicates_without_matching_elements_and_elements_without_matching_predicates()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "one", Number = 1 },
                new SomeClass { Text = "two", Number = 3 },
                new SomeClass { Text = "three", Number = 3 },
                new SomeClass { Text = "four", Number = 4 },
            };

            // Act
            Action act = () => collection.Should().Satisfy(
                new Expression<Func<SomeClass, bool>>[]
                {
                    element => element.Text == "four" && element.Number == 4,
                    element => element.Text == "two" && element.Number == 2,
                },
                because: "we want to test formatting ({0})",
                becauseArgs: "args");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                @"Expected collection to satisfy all predicates because we want to test formatting (args), but:
*The following predicates did not have matching elements:
*(element.Text == ""two"") AndAlso (element.Number == 2)
*The following elements did not match any predicate:
*Index: 0, Element:*FluentAssertionsAsync.Specs.Collections.CollectionAssertionSpecs+SomeClass*{*Number = 1*Text = ""one""*}
*Index: 1, Element:*FluentAssertionsAsync.Specs.Collections.CollectionAssertionSpecs+SomeClass*{*Number = 3*Text = ""two""*}
*Index: 2, Element:*FluentAssertionsAsync.Specs.Collections.CollectionAssertionSpecs+SomeClass*{*Number = 3*Text = ""three""*}");
        }

        [Fact]
        public void When_Satisfy_asserting_against_null_inspectors_it_should_throw_with_clear_explanation()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2 };

            // Act
            Action act = () => collection.Should().Satisfy(null);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify against a <null> collection of predicates*");
        }

        [Fact]
        public void When_asserting_against_empty_inspectors_should_throw_with_clear_explanation()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2 };

            // Act
            Action act = () => collection.Should().Satisfy();

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify against an empty collection of predicates*");
        }

        [Fact]
        public void When_asserting_collection_which_is_null_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();

                collection.Should().Satisfy(
                    new Expression<Func<int, bool>>[]
                    {
                        element => element == 1,
                        element => element == 2,
                    },
                    because: "we want to test the failure message ({0})",
                    becauseArgs: "args");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to satisfy all predicates because we want to test the failure message (args), but collection is <null>.");
        }

        [Fact]
        public void When_asserting_collection_which_is_empty_should_throw()
        {
            // Arrange
            var collection = Enumerable.Empty<int>();

            // Act
            Action act = () => collection.Should().Satisfy(
                new Expression<Func<int, bool>>[]
                {
                    element => element == 1,
                    element => element == 2,
                },
                because: "we want to test the failure message ({0})",
                becauseArgs: "args");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to satisfy all predicates because we want to test the failure message (args), but collection is empty.");
        }

        [Fact]
        public void When_elements_are_integers_assertion_fails_then_failure_message_must_be_readable()
        {
            // Arrange
            var collection = new List<int> { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Satisfy(
                element => element == 2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                @"Expected collection to satisfy all predicates, but:
*The following elements did not match any predicate:
*Index: 0, Element: 1
*Index: 2, Element: 3");
        }
    }

    private class SomeClass
    {
        public string Text { get; set; }

        public int Number { get; set; }
    }
}
