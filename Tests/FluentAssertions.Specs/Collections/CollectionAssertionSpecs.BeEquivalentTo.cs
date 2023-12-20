using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The [Not]BeEquivalentToAsync specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class BeEquivalentToAsync
    {
        [Fact]
        public async Task When_two_collections_contain_the_same_elements_it_should_treat_them_as_equivalent()
        {
            // Arrange
            var collection1 = new[] { 1, 2, 3 };
            var collection2 = new[] { 3, 1, 2 };

            // Act / Assert
            await collection1.Should().BeEquivalentToAsync(collection2);
        }

        [Fact]
        public async Task When_a_collection_contains_same_elements_it_should_treat_it_as_equivalent()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            await collection.Should().BeEquivalentToAsync(new[] { 3, 1, 2 });
        }

        [Fact]
        public async Task When_character_collections_are_equivalent_it_should_not_throw()
        {
            // Arrange
            char[] list1 = "abc123ab".ToCharArray();
            char[] list2 = "abc123ab".ToCharArray();

            // Act / Assert
            await list1.Should().BeEquivalentToAsync(list2);
        }

        [Fact]
        public async Task When_collections_are_not_equivalent_it_should_throw()
        {
            // Arrange
            var collection1 = new[] { 1, 2, 3 };
            var collection2 = new[] { 1, 2 };

            // Act
            Func<Task> act = () => collection1.Should().BeEquivalentToAsync(collection2, "we treat {0} alike", "all");

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected*collection*2 item(s)*we treat all alike, but *1 item(s) more than*");
        }

        [Fact]
        public async Task When_collections_with_duplicates_are_not_equivalent_it_should_throw()
        {
            // Arrange
            var collection1 = new[] { 1, 2, 3, 1 };
            var collection2 = new[] { 1, 2, 3, 3 };

            // Act
            Func<Task> act = () => collection1.Should().BeEquivalentToAsync(collection2);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected collection1[3]*to be 3, but found 1*");
        }

        [Fact]
        public async Task When_testing_for_equivalence_against_empty_collection_it_should_throw()
        {
            // Arrange
            var subject = new[] { 1, 2, 3 };
            var otherCollection = new int[0];

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(otherCollection);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected subject to be a collection with 0 item(s), but*contains 3 item(s)*");
        }

        [Fact]
        public async Task When_two_collections_are_both_empty_it_should_treat_them_as_equivalent()
        {
            // Arrange
            var subject = new int[0];
            var otherCollection = new int[0];

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(otherCollection);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_testing_for_equivalence_against_null_collection_it_should_throw()
        {
            // Arrange
            var collection1 = new[] { 1, 2, 3 };
            int[] collection2 = null;

            // Act
            Func<Task> act = () => collection1.Should().BeEquivalentToAsync(collection2);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected*<null>*but found {1, 2, 3}*");
        }

        [Fact]
        public async Task When_asserting_collections_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = null;
            var collection1 = new[] { 1, 2, 3 };

            // Act
            Func<Task> act =
                () => collection.Should()
                    .BeEquivalentToAsync(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected collection not to be <null>*");
        }

        [Fact]
        public async Task Default_immutable_arrays_should_be_equivalent()
        {
            // Arrange
            ImmutableArray<string> collection = default;
            ImmutableArray<string> collection1 = default;

            // Act / Assert
            await collection.Should().BeEquivalentToAsync(collection1);
        }

        [Fact]
        public async Task Default_immutable_lists_should_be_equivalent()
        {
            // Arrange
            ImmutableList<string> collection = default;
            ImmutableList<string> collection1 = default;

            // Act / Assert
            await collection.Should().BeEquivalentToAsync(collection1);
        }
    }

    public class NotBeEquivalentToAsync
    {
        [Fact]
        public async Task When_collection_is_not_equivalent_to_another_smaller_collection_it_should_succeed()
        {
            // Arrange
            var collection1 = new[] { 1, 2, 3 };
            var collection2 = new[] { 3, 1 };

            // Act / Assert
            await collection1.Should().NotBeEquivalentToAsync(collection2);
        }

        [Fact]
        public async Task When_large_collection_is_equivalent_to_another_equally_size_collection_it_should_throw()
        {
            // Arrange
            var collection1 = Enumerable.Repeat(1, 10000);
            var collection2 = Enumerable.Repeat(1, 10000);

            // Act
            Func<Task> act = () => collection1.Should().NotBeEquivalentToAsync(collection2);

            // Assert
            await act.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        public async Task When_collection_is_not_equivalent_to_another_equally_sized_collection_it_should_succeed()
        {
            // Arrange
            var collection1 = new[] { 1, 2, 3 };
            var collection2 = new[] { 3, 1, 4 };

            // Act / Assert
            await collection1.Should().NotBeEquivalentToAsync(collection2);
        }

        [Fact]
        public async Task When_collections_are_unexpectedly_equivalent_it_should_throw()
        {
            // Arrange
            var collection1 = new[] { 1, 2, 3 };
            var collection2 = new[] { 3, 1, 2 };

            // Act
            Func<Task> act = () => collection1.Should().NotBeEquivalentToAsync(collection2);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected collection1 {1, 2, 3} not*equivalent*{3, 1, 2}.");
        }

        [Fact]
        public async Task When_asserting_collections_not_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] actual = null;
            var expectation = new[] { 1, 2, 3 };

            // Act
            Func<Task> act = async () =>
            {
                using var _ = new AssertionScope();
                await actual.Should().NotBeEquivalentToAsync(expectation, "because we want to test the behaviour with a null subject");
            };

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "*be equivalent because we want to test the behaviour with a null subject, but found <null>*");
        }

        [Fact]
        public async Task When_non_empty_collection_is_not_expected_to_be_equivalent_to_an_empty_collection_it_should_succeed()
        {
            // Arrange
            var collection1 = new[] { 1, 2, 3 };
            var collection2 = new int[0];

            // Act
            Func<Task> act = () => collection1.Should().NotBeEquivalentToAsync(collection2);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_testing_collections_not_to_be_equivalent_against_null_collection_it_should_throw()
        {
            // Arrange
            var collection1 = new[] { 1, 2, 3 };
            int[] collection2 = null;

            // Act
            Func<Task> act = () => collection1.Should().NotBeEquivalentToAsync(collection2);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Cannot verify inequivalence against a <null> collection.*")
                .WithParameterName("unexpected");
        }

        [Fact]
        public async Task When_testing_collections_not_to_be_equivalent_against_same_collection_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };
            var collection1 = collection;

            // Act
            Func<Task> act = () => collection.Should().NotBeEquivalentToAsync(collection1,
                "because we want to test the behaviour with same objects");

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "*not to be equivalent*because we want to test the behaviour with same objects*but they both reference the same object.");
        }

        [Fact]
        public async Task When_a_collections_is_equivalent_to_an_approximate_copy_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1.0, 2.0, 3.0 };
            var collection1 = new[] { 1.5, 2.5, 3.5 };

            // Act
            Func<Task> act = () => collection.Should().NotBeEquivalentToAsync(collection1, opt => opt
                    .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.5))
                    .WhenTypeIs<double>(),
                "because we want to test the failure {0}", "message");

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "*not to be equivalent*because we want to test the failure message*");
        }

        [Fact]
        public async Task When_asserting_collections_not_to_be_equivalent_with_options_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] actual = null;
            int[] expectation = { 1, 2, 3 };

            // Act
            Func<Task> act = async () =>
            {
                using var _ = new AssertionScope();
                await actual.Should().NotBeEquivalentToAsync(expectation, opt => opt, "we want to test the failure {0}", "message");
            };

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("Expected actual not to be equivalent *failure message*, but found <null>.*");
        }

        [Fact]
        public async Task Default_immutable_array_should_not_be_equivalent_to_initialized_immutable_array()
        {
            // Arrange
            ImmutableArray<string> collection = default;
            ImmutableArray<string> collection1 = ImmutableArray.Create("a", "b", "c");

            // Act / Assert
            await collection.Should().NotBeEquivalentToAsync(collection1);
        }

        [Fact]
        public async Task Immutable_array_should_not_be_equivalent_to_default_immutable_array()
        {
            // Arrange
            ImmutableArray<string> collection = ImmutableArray.Create("a", "b", "c");
            ImmutableArray<string> collection1 = default;

            // Act / Assert
            await collection.Should().NotBeEquivalentToAsync(collection1);
        }
    }
}
