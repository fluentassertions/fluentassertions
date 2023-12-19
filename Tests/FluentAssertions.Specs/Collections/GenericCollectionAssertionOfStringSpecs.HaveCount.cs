using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class HaveCount
    {
        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_that_is_different_from_the_number_of_items()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().HaveCount(4);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_that_equals_the_number_of_items()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should().HaveCount(3);
        }

        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should()
                .HaveCount(3)
                .And
                .HaveElementAt(1, "two")
                .And.NotContain("four");
        }

        [Fact]
        public void When_collection_count_is_matched_against_a_null_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().HaveCount(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare collection count against a <null> predicate.*");
        }

        [Fact]
        public void When_collection_count_is_matched_against_a_predicate_and_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act =
                () => collection.Should().HaveCount(c => c < 3, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain (c < 3) items because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_collection_count_is_matched_and_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().HaveCount(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain 1 item(s) because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_collection_has_a_count_larger_than_the_minimum_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should().HaveCount(c => c >= 3);
        }

        [Fact]
        public void
            When_collection_has_a_count_that_is_different_from_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action action = () => collection.Should().HaveCount(4, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to contain 4 item(s) because we want to test the failure message, but found 3: {\"one\", \"two\", \"three\"}.");
        }

        [Fact]
        public void When_collection_has_a_count_that_not_matches_the_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().HaveCount(c => c >= 4, "a minimum of 4 is required");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to have a count (c >= 4) because a minimum of 4 is required, but count is 3: {\"one\", \"two\", \"three\"}.");
        }
    }
}
