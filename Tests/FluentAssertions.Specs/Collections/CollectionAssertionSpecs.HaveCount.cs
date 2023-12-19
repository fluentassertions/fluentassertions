using System;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The [Not]HaveCount specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class HaveCount
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_that_equals_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCount(3);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_that_is_different_from_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCount(4);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_collection_has_a_count_that_is_different_from_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().HaveCount(4, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to contain 4 item(s) because we want to test the failure message, but found 3: {1, 2, 3}.");
        }

        [Fact]
        public void When_collection_has_a_count_larger_than_the_minimum_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCount(c => c >= 3);
        }

        [Fact]
        public void When_asserting_a_collection_with_incorrect_predicates_in_assertion_scope_all_are_reported()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().HaveCount(c => c > 3).And.HaveCount(c => c < 3);
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*to have a count (c > 3)*to have a count (c < 3)*");
        }

        [Fact]
        public void When_collection_has_a_count_that_not_matches_the_predicate_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCount(c => c >= 4, "a minimum of 4 is required");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to have a count (c >= 4) because a minimum of 4 is required, but count is 3: {1, 2, 3}.");
        }

        [Fact]
        public void When_collection_count_is_matched_against_a_null_predicate_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCount(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare collection count against a <null> predicate.*");
        }

        [Fact]
        public void When_collection_count_is_matched_and_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().HaveCount(1, "we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain 1 item(s) because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_collection_count_is_matched_against_a_predicate_and_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().HaveCount(c => c < 3, "we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain (c < 3) items because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_collection_count_is_matched_against_a_predicate_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCount(c => (c % 2) == 1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_count_is_matched_against_a_predicate_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCount(c => (c % 2) == 0);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_counting_generic_enumerable_it_should_enumerate()
        {
            // Arrange
            var enumerable = new CountingGenericEnumerable<int>(new[] { 1, 2, 3 });

            // Act
            enumerable.Should().HaveCount(3);

            // Assert
            enumerable.GetEnumeratorCallCount.Should().Be(1);
        }

        [Fact]
        public void When_counting_generic_collection_it_should_not_enumerate()
        {
            // Arrange
            var collection = new CountingGenericCollection<int>(new[] { 1, 2, 3 });

            // Act
            collection.Should().HaveCount(3);

            // Assert
            collection.GetCountCallCount.Should().Be(1);
            collection.GetEnumeratorCallCount.Should().Be(0);
        }
    }

    public class NotHaveCount
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_different_from_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().NotHaveCount(2);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_that_equals_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotHaveCount(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_has_a_count_that_equals_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().NotHaveCount(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*not contain*3*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_collection_count_is_same_than_and_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotHaveCount(1, "we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*not contain*1*we want to test the behaviour with a null subject*found <null>*");
        }
    }
}
