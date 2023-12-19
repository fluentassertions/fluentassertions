using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The EndWith specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class EndWith
    {
        [Fact]
        public void When_collection_does_not_end_with_a_specific_element_it_should_throw()
        {
            // Arrange
            var collection = new[] { "john", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith("ryan", "of some reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*end*ryan*because of some reason*but*mike*");
        }

        [Fact]
        public void When_collection_does_end_with_a_specific_element_and_because_format_is_incorrect_it_should_not_fail()
        {
            // Arrange
            var collection = new[] { "john", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith("mike", "of some reason {0,abc}", 1, 2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_does_not_end_with_a_specific_element_in_a_sequence_it_should_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith(new[] { "bill", "ryan", "mike" }, "of some reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*end*ryan*because of some reason*but*differs at index 2*");
        }

        [Fact]
        public void When_collection_does_not_end_with_a_null_sequence_it_should_throw()
        {
            // Arrange
            var collection = new[] { "john" };

            // Act
            Action act = () => collection.Should().EndWith((IEnumerable<string>)null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("expectation");
        }

        [Fact]
        public void When_collection_does_not_end_with_a_null_sequence_using_a_comparer_it_should_throw()
        {
            // Arrange
            var collection = new[] { "john" };

            // Act
            Action act = () => collection.Should().EndWith((IEnumerable<string>)null, (_, _) => true);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("expectation");
        }

        [Fact]
        public void
            When_collection_does_not_end_with_a_specific_element_in_a_sequence_using_custom_equality_comparison_it_should_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith(new[] { "bill", "ryan", "mike" },
                (s1, s2) => string.Equals(s1, s2, StringComparison.Ordinal), "of some reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*end*ryan*because of some reason*but*differs at index 2*");
        }

        [Fact]
        public void When_collection_ends_with_the_specific_element_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith("mike");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_ends_with_the_specific_sequence_of_elements_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith(new[] { "jane", "mike" });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_collection_ends_with_the_specific_sequence_of_elements_using_custom_equality_comparison_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith(new[] { "JaNe", "mIkE" },
                (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_ends_with_the_specific_null_element_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "jane", "mike", null };

            // Act
            Action act = () => collection.Should().EndWith((string)null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_ends_with_the_specific_sequence_with_null_elements_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", null, "mike", null };

            // Act
            Action act = () => collection.Should().EndWith(new[] { "jane", null, "mike", null });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_collection_ends_with_the_specific_sequence_with_null_elements_using_custom_equality_comparison_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", null, "mike", null };

            // Act
            Action act = () => collection.Should().EndWith(new[] { "JaNe", null, "mIkE", null },
                (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_ends_with_null_but_that_wasnt_expected_it_should_throw()
        {
            // Arrange
            var collection = new[] { "jane", "mike", null };

            // Act
            Action act = () => collection.Should().EndWith("john");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*end*john*but*null*");
        }

        [Fact]
        public void When_null_collection_is_expected_to_end_with_an_element_it_should_throw()
        {
            // Arrange
            string[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().EndWith("john");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*end*john*but*collection*null*");
        }

        [Fact]
        public void When_non_empty_collection_ends_with_the_empty_sequence_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith(new string[] { });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_empty_collection_ends_with_the_empty_sequence_it_should_not_throw()
        {
            // Arrange
            var collection = new string[] { };

            // Act
            Action act = () => collection.Should().EndWith(new string[] { });

            // Assert
            act.Should().NotThrow();
        }
    }
}
