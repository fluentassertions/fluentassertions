using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class BeEquivalentTo
    {
        [Fact]
        public void When_asserting_collections_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };

            // Act
            Action act =
                () => collection.Should()
                    .BeEquivalentTo(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection*not to be <null>*");
        }

        [Fact]
        public void When_collections_with_duplicates_are_not_equivalent_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three", "one" };
            IEnumerable<string> collection2 = new[] { "one", "two", "three", "three" };

            // Act
            Action act = () => collection1.Should().BeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1[3]*to be \"three\" with a length of 5, but \"one\" has a length of 3*");
        }

        [Fact]
        public void When_testing_for_equivalence_against_empty_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> subject = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new string[0];

            // Act
            Action act = () => subject.Should().BeEquivalentTo(otherCollection);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject*to be a collection with 0 item(s), but*contains 3 item(s)*");
        }

        [Fact]
        public void When_testing_for_equivalence_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = null;

            // Act
            Action act = () => collection1.Should().BeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1*to be <null>, but found {\"one\", \"two\", \"three\"}*");
        }

        [Fact]
        public void When_two_collections_are_both_empty_it_should_treat_them_as_equivalent()
        {
            // Arrange
            IEnumerable<string> subject = new string[0];
            IEnumerable<string> otherCollection = new string[0];

            // Act
            Action act = () => subject.Should().BeEquivalentTo(otherCollection);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_two_collections_contain_the_same_elements_it_should_treat_them_as_equivalent()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "three", "two", "one" };

            // Act / Assert
            collection1.Should().BeEquivalentTo(collection2);
        }

        [Fact]
        public void When_two_arrays_contain_the_same_elements_it_should_treat_them_as_equivalent()
        {
            // Arrange
            string[] array1 = { "one", "two", "three" };
            string[] array2 = { "three", "two", "one" };

            // Act / Assert
            array1.Should().BeEquivalentTo(array2);
        }
    }

    public class NotBeEquivalentTo
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_is_not_equivalent_to_a_different_collection()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "three", "one" };

            // Act / Assert
            collection1.Should().NotBeEquivalentTo(collection2);
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> actual = null;
            IEnumerable<string> expectation = new[] { "one", "two", "three" };

            // Act
            Action act = () => actual.Should().NotBeEquivalentTo(expectation,
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected actual not to be equivalent because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_collections_are_unexpectedly_equivalent_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "three", "one", "two" };

            // Act
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection1 {\"one\", \"two\", \"three\"} not*equivalent*{\"three\", \"one\", \"two\"}.");
        }

        [Fact]
        public void When_non_empty_collection_is_not_expected_to_be_equivalent_to_an_empty_collection_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new string[0];

            // Act
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_testing_collections_not_to_be_equivalent_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = null;

            // Act
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify inequivalence against a <null> collection.*");
        }

        [Fact]
        public void When_testing_collections_not_to_be_equivalent_against_same_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> collection1 = collection;

            // Act
            Action act = () => collection.Should().NotBeEquivalentTo(collection1,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*not to be equivalent*because we want to test the behaviour with same objects*but they both reference the same object.");
        }
    }
}
