using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class Equal
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_collection()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two", "three" };

            // Act / Assert
            collection1.Should().Equal(collection2);
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_list_of_elements()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should().Equal("one", "two", "three");
        }

        [Fact]
        public void When_all_items_match_according_to_a_predicate_it_should_succeed()
        {
            // Arrange
            var actual = new List<string> { "ONE", "TWO", "THREE", "FOUR" };
            var expected = new List<string> { "One", "Two", "Three", "Four" };

            // Act
            Action action = () => actual.Should().Equal(expected,
                (a, e) => string.Equals(a, e, StringComparison.OrdinalIgnoreCase));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_an_empty_collection_is_compared_for_equality_to_a_non_empty_collection_it_should_throw()
        {
            // Arrange
            var collection1 = new string[0];
            IEnumerable<string> collection2 = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection1.Should().Equal(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 to be equal to {\"one\", \"two\", \"three\"}, but found empty collection.");
        }

        [Fact]
        public void When_any_item_does_not_match_according_to_a_predicate_it_should_throw()
        {
            // Arrange
            var actual = new List<string> { "ONE", "TWO", "THREE", "FOUR" };
            var expected = new List<string> { "One", "Two", "Three", "Five" };

            // Act
            Action action = () => actual.Should().Equal(expected,
                (a, e) => string.Equals(a, e, StringComparison.OrdinalIgnoreCase));

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Expected*equal to*, but*differs at index 3.");
        }

        [Fact]
        public void When_injecting_a_null_comparer_it_should_throw()
        {
            // Arrange
            var actual = new List<string>();
            var expected = new List<string>();

            // Act
            Action action = () => actual.Should().Equal(expected, equalityComparison: null);

            // Assert
            action
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("equalityComparison");
        }

        [Fact]
        public void When_asserting_collections_to_be_equal_but_expected_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> collection1 = null;

            // Act
            Action act = () =>
                collection.Should().Equal(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare collection with <null>.*")
                .WithParameterName("expectation");
        }

        [Fact]
        public void When_asserting_collections_to_be_equal_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };

            // Act
            Action act = () =>
                collection.Should().Equal(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to be equal to {\"one\", \"two\", \"three\"} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_both_collections_are_null_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> nullColl = null;

            // Act
            Action act = () => nullColl.Should().Equal(null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_two_collections_are_not_equal_because_one_item_differs_it_should_throw_using_the_reason()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two", "five" };

            // Act
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 to be equal to {\"one\", \"two\", \"five\"} because we want to test the failure message, but {\"one\", \"two\", \"three\"} differs at index 2.");
        }

        [Fact]
        public void
            When_two_collections_are_not_equal_because_the_actual_collection_contains_less_items_it_should_throw_using_the_reason()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two", "three", "four" };

            // Act
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 to be equal to {\"one\", \"two\", \"three\", \"four\"} because we want to test the failure message, but {\"one\", \"two\", \"three\"} contains 1 item(s) less.");
        }

        [Fact]
        public void
            When_two_collections_are_not_equal_because_the_actual_collection_contains_more_items_it_should_throw_using_the_reason()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two" };

            // Act
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 to be equal to {\"one\", \"two\"} because we want to test the failure message, but {\"one\", \"two\", \"three\"} contains 1 item(s) too many.");
        }

        [Fact]
        public void When_two_collections_containing_nulls_are_equal_it_should_not_throw()
        {
            // Arrange
            var subject = new List<string> { "aaa", null };
            var expected = new List<string> { "aaa", null };

            // Act
            Action action = () => subject.Should().Equal(expected);

            // Assert
            action.Should().NotThrow();
        }
    }

    public class NotEqual
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_is_not_equal_to_a_different_collection()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "three", "one", "two" };

            // Act / Assert
            collection1.Should()
                .NotEqual(collection2);
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equal_but_both_collections_reference_the_same_object_it_should_throw()
        {
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = collection1;

            // Act
            Action act = () =>
                collection1.Should().NotEqual(collection2, "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collections not to be equal because we want to test the behaviour with same objects, but they both reference the same object.");
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equal_but_expected_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> collection1 = null;

            // Act
            Action act =
                () => collection.Should().NotEqual(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare collection with <null>.*")
                .WithParameterName("unexpected");
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equal_subject_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };

            // Act
            Action act =
                () => collection.Should().NotEqual(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collections not to be equal because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_two_equal_collections_are_not_expected_to_be_equal_it_should_report_a_clear_explanation()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection1.Should().NotEqual(collection2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect collections {\"one\", \"two\", \"three\"} and {\"one\", \"two\", \"three\"} to be equal because we want to test the failure message.");
        }

        [Fact]
        public void When_two_equal_collections_are_not_expected_to_be_equal_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection1.Should().NotEqual(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect collections {\"one\", \"two\", \"three\"} and {\"one\", \"two\", \"three\"} to be equal.");
        }
    }
}
