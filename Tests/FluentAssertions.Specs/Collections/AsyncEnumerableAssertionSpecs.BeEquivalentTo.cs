using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <content>
/// The [Not]BeEquivalentTo specs.
/// </content>
public partial class AsyncEnumerableAssertionSpecs
{
    public class BeEquivalentTo
    {
        [Fact]
        public void When_two_collections_contain_the_same_elements_it_should_treat_them_as_equivalent()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [3, 1, 2];

            // Act / Assert
            collection1.ToAsyncEnumerable().Should().BeEquivalentTo(collection2.ToAsyncEnumerable());
        }

        [Fact]
        public void When_a_collection_contains_same_elements_it_should_treat_it_as_equivalent()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [3, 1, 2];

            // Act / Assert
            collection1.ToAsyncEnumerable().Should().BeEquivalentTo(collection2.ToAsyncEnumerable());
        }

        [Fact]
        public void When_character_collections_are_equivalent_it_should_not_throw()
        {
            // Arrange
            char[] list1 = "abc123ab".ToCharArray();
            char[] list2 = "abc123ab".ToCharArray();

            // Act / Assert
            list1.ToAsyncEnumerable().Should().BeEquivalentTo(list2.ToAsyncEnumerable());
        }

        [Fact]
        public void When_collections_are_not_equivalent_it_should_throw()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [1, 2];

            // Act
            Action act = () => collection1.ToAsyncEnumerable().Should().BeEquivalentTo(collection2.ToAsyncEnumerable(), "we treat {0} alike", "all");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*collection*2 item(s)*we treat all alike, but *1 item(s) more than*");
        }

        [Fact]
        public void When_collections_with_duplicates_are_not_equivalent_it_should_throw()
        {
            // Arrange
            int[] array1 = [1, 2, 3, 1];
            var collection1 = array1.ToAsyncEnumerable();
            int[] collection2 = [1, 2, 3, 3];

            // Act
            Action act = () => collection1.Should().BeEquivalentTo(collection2.ToAsyncEnumerable());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1[3]*to be 3, but found 1*");
        }

        [Fact]
        public void When_testing_for_equivalence_against_empty_collection_it_should_throw()
        {
            // Arrange
            int[] array = [1, 2, 3];
            var subject = array.ToAsyncEnumerable();
            int[] otherCollection = [];

            // Act
            Action act = () => subject.Should().BeEquivalentTo(otherCollection.ToAsyncEnumerable());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject to be a collection with 0 item(s), but*contains 3 item(s)*");
        }

        [Fact]
        public void When_two_collections_are_both_empty_it_should_treat_them_as_equivalent()
        {
            // Arrange
            int[] subject = [];
            int[] otherCollection = [];

            // Act
            Action act = () => subject.ToAsyncEnumerable().Should().BeEquivalentTo(otherCollection.ToAsyncEnumerable());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_testing_for_equivalence_against_null_collection_it_should_throw()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            IAsyncEnumerable<int> collection2 = null;

            // Act
            Action act = () => collection1.ToAsyncEnumerable().Should().BeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*<null>*but found {1, 2, 3}*");
        }

        [Fact]
        public void When_asserting_collections_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            IAsyncEnumerable<int> collection = null;
            int[] collection1 = [1, 2, 3];

            // Act
            Action act =
                () => collection.Should()
                    .BeEquivalentTo(collection1.ToAsyncEnumerable(), "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to be <null>*");
        }
    }

    public class NotBeEquivalentTo
    {
        [Fact]
        public void When_collection_is_not_equivalent_to_another_smaller_collection_it_should_succeed()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [3, 1];

            // Act / Assert
            collection1.ToAsyncEnumerable().Should().NotBeEquivalentTo(collection2.ToAsyncEnumerable());
        }

        [Fact]
        public void When_large_collection_is_equivalent_to_another_equally_size_collection_it_should_throw()
        {
            // Arrange
            var collection1 = AsyncEnumerable.Repeat(1, 10000);
            var collection2 = AsyncEnumerable.Repeat(1, 10000);

            // Act
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_is_not_equivalent_to_another_equally_sized_collection_it_should_succeed()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [3, 1, 4];

            // Act / Assert
            collection1.ToAsyncEnumerable().Should().NotBeEquivalentTo(collection2.ToAsyncEnumerable());
        }

        [Fact]
        public void When_collections_are_unexpectedly_equivalent_it_should_throw()
        {
            // Arrange
            int[] array = [1, 2, 3];
            var collection1 = array.ToAsyncEnumerable();
            int[] collection2 = [3, 1, 2];

            // Act
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2.ToAsyncEnumerable());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 {1, 2, 3} not*equivalent*{3, 1, 2}.");
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            IAsyncEnumerable<int> actual = null;
            int[] expectation = [1, 2, 3];

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                actual.Should().NotBeEquivalentTo(expectation.ToAsyncEnumerable(), "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*be equivalent because we want to test the behaviour with a null subject, but found <null>*");
        }

        [Fact]
        public void When_non_empty_collection_is_not_expected_to_be_equivalent_to_an_empty_collection_it_should_succeed()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            int[] collection2 = [];

            // Act
            Action act = () => collection1.ToAsyncEnumerable().Should().NotBeEquivalentTo(collection2.ToAsyncEnumerable());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_testing_collections_not_to_be_equivalent_against_null_collection_it_should_throw()
        {
            // Arrange
            int[] collection1 = [1, 2, 3];
            IAsyncEnumerable<int> collection2 = null;

            // Act
            Action act = () => collection1.ToAsyncEnumerable().Should().NotBeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot verify inequivalence against a <null> collection.*")
                .WithParameterName("unexpected");
        }

        [Fact]
        public void When_testing_collections_not_to_be_equivalent_against_same_collection_it_should_throw()
        {
            // Arrange
            int[] array = [1, 2, 3];
            var collection = array.ToAsyncEnumerable();
            var collection1 = collection;

            // Act
            Action act = () => collection.Should().NotBeEquivalentTo(collection1,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*not to be equivalent*because we want to test the behaviour with same objects*but they both reference the same object.");
        }

        [Fact]
        public void When_a_collections_is_equivalent_to_an_approximate_copy_it_should_throw()
        {
            // Arrange
            double[] collection = [1.0, 2.0, 3.0];
            double[] collection1 = [1.5, 2.5, 3.5];

            // Act
            Action act = () => collection.ToAsyncEnumerable().Should().NotBeEquivalentTo(collection1.ToAsyncEnumerable(), opt => opt
                    .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.5))
                    .WhenTypeIs<double>(),
                "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*not to be equivalent*because we want to test the failure message*");
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equivalent_with_options_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            IAsyncEnumerable<int> actual = null;
            int[] expectation = [1, 2, 3];

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                actual.Should().NotBeEquivalentTo(expectation.ToAsyncEnumerable(), opt => opt, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected actual not to be equivalent *failure message*, but found <null>.*");
        }
    }
}
