using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The OnlyHaveUniqueItems specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class OnlyHaveUniqueItems
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_with_unique_items_contains_only_unique_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3, 4 };

            // Act / Assert
            collection.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void When_a_collection_contains_duplicate_items_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3, 3 };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems("{0} don't like {1}", "we", "duplicates");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we don't like duplicates, but item 3 is not unique.");
        }

        [Fact]
        public void When_a_collection_contains_duplicate_items_it_supports_chaining()
        {
            // Arrange
            var collection = new[] { 1, 2, 3, 3 };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().OnlyHaveUniqueItems().And.HaveCount(c => c > 1);
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*but item 3 is not unique.");
        }

        [Fact]
        public void When_a_collection_contains_multiple_duplicate_items_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 2, 3, 3 };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems("{0} don't like {1}", "we", "duplicates");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we don't like duplicates, but items {2, 3} are not unique.");
        }

        [Fact]
        public void When_a_collection_contains_multiple_duplicate_items_it_supports_chaining()
        {
            // Arrange
            var collection = new[] { 1, 2, 2, 3, 3 };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().OnlyHaveUniqueItems().And.HaveCount(c => c > 1);
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*but items {2, 3} are not unique.");
        }

        [Fact]
        public void When_asserting_collection_to_only_have_unique_items_but_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().OnlyHaveUniqueItems("because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_injecting_a_null_predicate_into_OnlyHaveUniqueItems_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new SomeClass[] { };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems<string>(predicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("predicate");
        }

        [Fact]
        public void Should_succeed_when_asserting_with_a_predicate_a_collection_with_unique_items_contains_only_unique_items()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "one" },
                new SomeClass { Text = "two" },
                new SomeClass { Text = "three" },
                new SomeClass { Text = "four" }
            };

            // Act / Assert
            collection.Should().OnlyHaveUniqueItems(e => e.Text);
        }

        [Fact]
        public void When_a_collection_contains_duplicate_items_with_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "one" },
                new SomeClass { Text = "two" },
                new SomeClass { Text = "three" },
                new SomeClass { Text = "three" }
            };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems(e => e.Text, "{0} don't like {1}", "we", "duplicates");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items*on e.Text*because we don't like duplicates, but item*three*is not unique.");
        }

        [Fact]
        public void When_a_collection_contains_multiple_duplicate_items_with_a_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "one" },
                new SomeClass { Text = "two" },
                new SomeClass { Text = "two" },
                new SomeClass { Text = "three" },
                new SomeClass { Text = "three" }
            };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems(e => e.Text, "{0} don't like {1}", "we", "duplicates");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items*on e.Text*because we don't like duplicates, but items*two*two*three*three*are not unique.");
        }

        [Fact]
        public void When_a_collection_contains_multiple_duplicates_on_different_properties_all_should_be_reported()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "one", Number = 1 },
                new SomeClass { Text = "two", Number = 2 },
                new SomeClass { Text = "two", Number = 2 },
                new SomeClass { Text = "three", Number = 3 },
                new SomeClass { Text = "three", Number = 4 }
            };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().OnlyHaveUniqueItems(e => e.Text).And.OnlyHaveUniqueItems(e => e.Number);
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*have unique items on e.Text*have unique items on e.Number*");
        }

        [Fact]
        public void
            When_asserting_with_a_predicate_a_collection_to_only_have_unique_items_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().OnlyHaveUniqueItems(e => e.Text, "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we want to test the behaviour with a null subject, but found <null>.");
        }
    }
}
