using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class BeSubsetOf
    {
        [Fact]
        public void When_a_subset_is_tested_against_a_null_superset_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            IEnumerable<string> subset = new[] { "one", "two", "three" };
            IEnumerable<string> superset = null;

            // Act
            Action act = () => subset.Should().BeSubsetOf(superset);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify a subset against a <null> collection.*");
        }

        [Fact]
        public void When_an_empty_collection_is_tested_against_a_superset_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> subset = new string[0];
            IEnumerable<string> superset = new[] { "one", "two", "four", "five" };

            // Act
            Action act = () => subset.Should().BeSubsetOf(superset);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_collection_to_be_subset_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };

            // Act
            Action act =
                () => collection.Should().BeSubsetOf(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to be a subset of {\"one\", \"two\", \"three\"} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_collection_is_not_a_subset_of_another_it_should_throw_with_the_reason()
        {
            // Arrange
            IEnumerable<string> subset = new[] { "one", "two", "three", "six" };
            IEnumerable<string> superset = new[] { "one", "two", "four", "five" };

            // Act
            Action act = () => subset.Should().BeSubsetOf(superset, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subset to be a subset of {\"one\", \"two\", \"four\", \"five\"} because we want to test the failure message, " +
                "but items {\"three\", \"six\"} are not part of the superset.");
        }

        [Fact]
        public void When_collection_is_subset_of_a_specified_collection_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> subset = new[] { "one", "two" };
            IEnumerable<string> superset = new[] { "one", "two", "three" };

            // Act / Assert
            subset.Should().BeSubsetOf(superset);
        }
    }

    public class NotBeSubsetOf
    {
        [Fact]
        public void Should_fail_when_asserting_collection_is_not_subset_of_a_superset_collection()
        {
            // Arrange
            IEnumerable<string> subject = new[] { "one", "two" };
            IEnumerable<string> otherSet = new[] { "one", "two", "three" };

            // Act
            Action act = () => subject.Should().NotBeSubsetOf(otherSet, "because I'm {0}", "mistaken");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect subject {\"one\", \"two\"} to be a subset of {\"one\", \"two\", \"three\"} because I'm mistaken.");
        }

        [Fact]
        public void When_a_set_is_expected_to_be_not_a_subset_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> subject = new[] { "one", "two", "four" };
            IEnumerable<string> otherSet = new[] { "one", "two", "three" };

            // Act / Assert
            subject.Should().NotBeSubsetOf(otherSet);
        }

        [Fact]
        public void When_an_empty_set_is_not_supposed_to_be_a_subset_of_another_set_it_should_throw()
        {
            // Arrange
            IEnumerable<string> subject = new string[] { };
            IEnumerable<string> otherSet = new[] { "one", "two", "three" };

            // Act
            Action act = () => subject.Should().NotBeSubsetOf(otherSet);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect subject {empty} to be a subset of {\"one\", \"two\", \"three\"}.");
        }

        [Fact]
        public void When_asserting_collection_to_not_be_subset_against_same_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = collection;

            // Act
            Action act = () => collection.Should().NotBeSubsetOf(otherCollection,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect*to be a subset of*because we want to test the behaviour with same objects*but they both reference the same object.");
        }
    }
}
