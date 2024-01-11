using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <content>
/// The [Not]Equal specs.
/// </content>
public partial class AsyncEnumerableAssertionSpecs
{
    public class Equal
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_collection()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [1, 2, 3];

            // Act / Assert
            collection1.ToAsyncEnumerable().Should().Equal(collection2.ToAsyncEnumerable());
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_list_of_elements()
        {
            // Arrange
            int[] collection = [1, 2, 3];

            // Act / Assert
            collection.ToAsyncEnumerable().Should().Equal(1, 2, 3);
        }

        [Fact]
        public void When_both_collections_are_null_it_should_succeed()
        {
            // Arrange
            IAsyncEnumerable<int> nullColl = null;
            IAsyncEnumerable<int> nullExpected = null;

            // Act
            Action act = () => nullColl.Should().Equal(nullExpected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_two_collections_containing_nulls_are_equal_it_should_not_throw()
        {
            // Arrange
            var subject = new List<string> { "aaa", null };
            var expected = new List<string> { "aaa", null };

            // Act
            Action action = () => subject.ToAsyncEnumerable().Should().Equal(expected);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_two_collections_are_not_equal_because_one_item_differs_it_should_throw_using_the_reason()
        {
            // Arrange
            int[] array1 = [1, 2, 3];
            var collection1 = array1.ToAsyncEnumerable();
            int[] array2 = [1, 2, 5];
            var collection2 = array2.ToAsyncEnumerable();

            // Act
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 to be equal to {1, 2, 5} because we want to test the failure message, but {1, 2, 3} differs at index 2.");
        }

        [Fact]
        public void
            When_two_collections_are_not_equal_because_the_actual_collection_contains_more_items_it_should_throw_using_the_reason()
        {
            // Arrange
            int[] array1 = [1, 2, 3];
            var collection1 = array1.ToAsyncEnumerable();
            int[] collection2 = [1, 2];

            // Act
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 to be equal to {1, 2} because we want to test the failure message, but {1, 2, 3} contains 1 item(s) too many.");
        }

        [Fact]
        public void
            When_two_collections_are_not_equal_because_the_actual_collection_contains_less_items_it_should_throw_using_the_reason()
        {
            // Arrange
            int[] array1 = [1, 2, 3];
            var collection1 = array1.ToAsyncEnumerable();
            int[] collection2 = [1, 2, 3, 4];

            // Act
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 to be equal to {1, 2, 3, 4} because we want to test the failure message, but {1, 2, 3} contains 1 item(s) less.");
        }

        [Fact]
        public void When_two_multidimensional_collections_are_not_equal_and_it_should_format_the_collections_properly()
        {
            // Arrange
            object[][] array1 = [[1, 2], [3, 4]];
            var collection1 = array1.ToAsyncEnumerable();
            object[][] collection2 = [[5, 6], [7, 8]];

            // Act
            Action act = () => collection1.Should().Equal(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 to be equal to {{5, 6}, {7, 8}}, but {{1, 2}, {3, 4}} differs at index 0.");
        }

        [Fact]
        public void When_asserting_collections_to_be_equal_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            IAsyncEnumerable<int> collection = null;
            int[] collection1 = [1, 2, 3];

            // Act
            Action act = () =>
                collection.Should().Equal(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to be equal to {1, 2, 3} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collections_to_be_equal_but_expected_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] array = [1, 2, 3];
            var collection = array.ToAsyncEnumerable();
            IAsyncEnumerable<int> collection1 = null;

            // Act
            Action act = () =>
                collection.Should().Equal(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare collection with <null>.*")
                .WithParameterName("expectation");
        }

        [Fact]
        public void When_an_empty_collection_is_compared_for_equality_to_a_non_empty_collection_it_should_throw()
        {
            // Arrange
            int[] array1 = [];
            var collection1 = array1.ToAsyncEnumerable();
            int[] collection2 = [1, 2, 3];

            // Act
            Action act = () => collection1.Should().Equal(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 to be equal to {1, 2, 3}, but found empty collection.");
        }

        [Fact]
        public void When_a_non_empty_collection_is_compared_for_equality_to_an_empty_collection_it_should_throw()
        {
            // Arrange
            int[] array1 = [1, 2, 3];
            var collection1 = array1.ToAsyncEnumerable();
            int[] collection2 = [];

            // Act
            Action act = () => collection1.Should().Equal(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 to be equal to {empty}, but found {1, 2, 3}.");
        }

        [Fact]
        public void When_all_items_match_according_to_a_predicate_it_should_succeed()
        {
            // Arrange
            var actual = new List<string> { "ONE", "TWO", "THREE", "FOUR" };

            var expected = new[]
            {
                new { Value = "One" },
                new { Value = "Two" },
                new { Value = "Three" },
                new { Value = "Four" }
            };

            // Act
            Action action = () => actual.ToAsyncEnumerable().Should().Equal(expected,
                (a, e) => string.Equals(a, e.Value, StringComparison.OrdinalIgnoreCase));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_any_item_does_not_match_according_to_a_predicate_it_should_throw()
        {
            // Arrange
            var array = new List<string> { "ONE", "TWO", "THREE", "FOUR" };
            var actual = array.ToAsyncEnumerable();

            var expected = new[]
            {
                new { Value = "One" },
                new { Value = "Two" },
                new { Value = "Three" },
                new { Value = "Five" }
            };

            // Act
            Action action = () => actual.Should().Equal(expected,
                (a, e) => string.Equals(a, e.Value, StringComparison.OrdinalIgnoreCase));

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("*Expected*equal to*, but*differs at index 3.*");
        }

        [Fact]
        public void When_both_collections_are_empty_it_should_them_as_equal()
        {
            // Arrange
            var actual = new List<string>().ToAsyncEnumerable();
            var expected = new List<string>().ToAsyncEnumerable();

            // Act
            Action act = () => actual.Should().Equal(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_identical_collections_to_be_equal_it_should_enumerate_the_subject_only_once()
        {
            // Arrange
            var actual = new CountingAsyncEnumerable<int>([1, 2, 3]);
            int[] expected = [1, 2, 3];

            // Act
            actual.Should().Equal(expected);

            // Assert
            actual.GetEnumeratorCallCount.Should().Be(1);
        }

        [Fact]
        public void When_asserting_identical_collections_to_not_be_equal_it_should_enumerate_the_subject_only_once()
        {
            // Arrange
            var actual = new CountingAsyncEnumerable<int>([1, 2, 3]);
            int[] expected = [1, 2, 3];

            // Act
            try
            {
                actual.Should().NotEqual(expected);
            }
            catch
            {
                /* we don't care about the exception, we just need to check the enumeration count */
            }

            // Assert
            actual.GetEnumeratorCallCount.Should().Be(1);
        }

        [Fact]
        public void When_asserting_different_collections_to_be_equal_it_should_enumerate_the_subject_once()
        {
            // Arrange
            var actual = new CountingAsyncEnumerable<int>([1, 2, 3]);
            int[] expected = [1, 2, 4];

            // Act
            try
            {
                actual.Should().Equal(expected);
            }
            catch
            {
                /* we don't care about the exception, we just need to check the enumeration count */
            }

            // Assert
            actual.GetEnumeratorCallCount.Should().Be(1);
        }

        [Fact]
        public void When_asserting_different_collections_to_not_be_equal_it_should_enumerate_the_subject_only_once()
        {
            // Arrange
            var actual = new CountingAsyncEnumerable<int>([1, 2, 3]);
            int[] expected = [1, 2, 4];

            // Act
            actual.Should().NotEqual(expected.ToAsyncEnumerable());

            // Assert
            actual.GetEnumeratorCallCount.Should().Be(1);
        }

        [Fact]
        public void
            When_asserting_equality_with_a_collection_built_from_params_arguments_that_are_assignable_to_the_subjects_type_parameter_it_should_succeed_by_treating_the_arguments_as_of_that_type()
        {
            // Arrange
            byte[] byteArray = [0xfe, 0xdc, 0xba, 0x98, 0x76, 0x54, 0x32, 0x10];

            // Act
            Action act = () => byteArray.ToAsyncEnumerable().Should().Equal(0xfe, 0xdc, 0xba, 0x98, 0x76, 0x54, 0x32, 0x10);

            // Assert
            act.Should().NotThrow();
        }
    }

    public class NotEqual
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_is_not_equal_to_a_different_collection()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [3, 1, 2];

            // Act / Assert
            collection1.ToAsyncEnumerable().Should()
                .NotEqual(collection2.ToAsyncEnumerable());
        }

        [Fact]
        public void When_two_equal_collections_are_not_expected_to_be_equal_it_should_throw()
        {
            // Arrange
            int[] array1 = [1, 2, 3];
            var collection1 = array1.ToAsyncEnumerable();
            int[] collection2 = [1, 2, 3];

            // Act
            Action act = () => collection1.Should().NotEqual(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect collections {1, 2, 3} and {1, 2, 3} to be equal.");
        }

        [Fact]
        public void When_two_equal_collections_are_not_expected_to_be_equal_it_should_report_a_clear_explanation()
        {
            // Arrange
            int[] array1 = [1, 2, 3];
            var collection1 = array1.ToAsyncEnumerable();
            int[] collection2 = [1, 2, 3];

            // Act
            Action act = () => collection1.Should().NotEqual(collection2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect collections {1, 2, 3} and {1, 2, 3} to be equal because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equal_subject_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IAsyncEnumerable<int> collection = null;
            int[] collection1 = [1, 2, 3];

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotEqual(collection1, "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collections not to be equal because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equal_but_expected_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] array = [1, 2, 3];
            var collection = array.ToAsyncEnumerable();
            int[] collection1 = null;

            // Act
            Action act =
                () => collection.Should().NotEqual(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare collection with <null>.*")
                .WithParameterName("unexpected");
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equal_but_both_collections_reference_the_same_object_it_should_throw()
        {
            string[] array1 = ["one", "two", "three"];
            var collection1 = array1.ToAsyncEnumerable();
            var collection2 = collection1;

            // Act
            Action act = () =>
                collection1.Should().NotEqual(collection2, "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collections not to be equal because we want to test the behaviour with same objects, but they both reference the same object.");
        }
    }
}
