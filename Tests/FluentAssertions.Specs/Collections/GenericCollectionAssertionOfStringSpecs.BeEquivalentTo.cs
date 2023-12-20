using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class BeEquivalentToAsync
    {
        [Fact]
        public async Task When_asserting_collections_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };

            // Act
            Func<Task> act = () => collection.Should()
                    .BeEquivalentToAsync(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("Expected collection*not to be <null>*");
        }

        [Fact]
        public async Task When_collections_with_duplicates_are_not_equivalent_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three", "one" };
            IEnumerable<string> collection2 = new[] { "one", "two", "three", "three" };

            // Act
            Func<Task> act = () => collection1.Should().BeEquivalentToAsync(collection2);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected collection1[3]*to be \"three\" with a length of 5, but \"one\" has a length of 3*");
        }

        [Fact]
        public async Task When_testing_for_equivalence_against_empty_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> subject = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new string[0];

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(otherCollection);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected subject*to be a collection with 0 item(s), but*contains 3 item(s)*");
        }

        [Fact]
        public async Task When_testing_for_equivalence_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = null;

            // Act
            Func<Task> act = () => collection1.Should().BeEquivalentToAsync(collection2);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected collection1*to be <null>, but found {\"one\", \"two\", \"three\"}*");
        }

        [Fact]
        public async Task When_two_collections_are_both_empty_it_should_treat_them_as_equivalent()
        {
            // Arrange
            IEnumerable<string> subject = new string[0];
            IEnumerable<string> otherCollection = new string[0];

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(otherCollection);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_two_collections_contain_the_same_elements_it_should_treat_them_as_equivalent()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "three", "two", "one" };

            // Act / Assert
            await collection1.Should().BeEquivalentToAsync(collection2);
        }

        [Fact]
        public async Task When_two_arrays_contain_the_same_elements_it_should_treat_them_as_equivalent()
        {
            // Arrange
            string[] array1 = { "one", "two", "three" };
            string[] array2 = { "three", "two", "one" };

            // Act / Assert
            await array1.Should().BeEquivalentToAsync(array2);
        }
    }

    public class NotBeEquivalentTo
    {
        [Fact]
        public async Task Should_succeed_when_asserting_collection_is_not_equivalent_to_a_different_collection()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "three", "one" };

            // Act / Assert
            await collection1.Should().NotBeEquivalentToAsync(collection2);
        }

        [Fact]
        public async Task When_asserting_collections_not_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> actual = null;
            IEnumerable<string> expectation = new[] { "one", "two", "three" };

            // Act
            Func<Task> act = () => actual.Should().NotBeEquivalentToAsync(expectation,
                "because we want to test the behaviour with a null subject");

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected actual not to be equivalent because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public async Task When_collections_are_unexpectedly_equivalent_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "three", "one", "two" };

            // Act
            Func<Task> act = () => collection1.Should().NotBeEquivalentToAsync(collection2);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected collection1 {\"one\", \"two\", \"three\"} not*equivalent*{\"three\", \"one\", \"two\"}.");
        }

        [Fact]
        public async Task When_non_empty_collection_is_not_expected_to_be_equivalent_to_an_empty_collection_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new string[0];

            // Act
            Func<Task> act = () => collection1.Should().NotBeEquivalentToAsync(collection2);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_testing_collections_not_to_be_equivalent_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = null;

            // Act
            Func<Task> act = () => collection1.Should().NotBeEquivalentToAsync(collection2);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithMessage(
                "Cannot verify inequivalence against a <null> collection.*");
        }

        [Fact]
        public async Task When_testing_collections_not_to_be_equivalent_against_same_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> collection1 = collection;

            // Act
            Func<Task> act = () => collection.Should().NotBeEquivalentToAsync(collection1,
                "because we want to test the behaviour with same objects");

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "*not to be equivalent*because we want to test the behaviour with same objects*but they both reference the same object.");
        }
    }
}
