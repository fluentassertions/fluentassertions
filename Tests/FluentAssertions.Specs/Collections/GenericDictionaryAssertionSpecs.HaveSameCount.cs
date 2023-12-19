using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    public class HaveSameCount
    {
        [Fact]
        public void When_dictionary_and_collection_have_the_same_number_elements_it_should_succeed()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            var collection = new[] { 4, 5, 6 };

            // Act / Assert
            dictionary.Should().HaveSameCount(collection);
        }

        [Fact]
        public void When_dictionary_and_collection_do_not_have_the_same_number_of_elements_it_should_fail()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            var collection = new[] { 4, 6 };

            // Act
            Action act = () => dictionary.Should().HaveSameCount(collection);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to have 2 item(s), but found 3.");
        }

        [Fact]
        public void When_comparing_item_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            var collection = new[] { 4, 6 };

            // Act
            Action act = () => dictionary.Should().HaveSameCount(collection, "we want to test the {0}", "reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to have 2 item(s) because we want to test the reason, but found 3.");
        }

        [Fact]
        public void When_asserting_dictionary_and_collection_have_same_count_against_null_dictionary_it_should_throw()
        {
            // Arrange
            Dictionary<string, int> dictionary = null;
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => dictionary.Should().HaveSameCount(collection,
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to have the same count as {1, 2, 3} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_dictionary_and_collection_have_same_count_against_a_null_collection_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            int[] collection = null;

            // Act
            Action act = () => dictionary.Should().HaveSameCount(collection);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify count against a <null> collection.*");
        }
    }

    public class NotHaveSameCount
    {
        [Fact]
        public void When_asserting_not_same_count_and_collections_have_different_number_elements_it_should_succeed()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            var collection = new[] { 4, 6 };

            // Act / Assert
            dictionary.Should().NotHaveSameCount(collection);
        }

        [Fact]
        public void When_asserting_not_same_count_and_both_collections_have_the_same_number_elements_it_should_fail()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            var collection = new[] { 4, 5, 6 };

            // Act
            Action act = () => dictionary.Should().NotHaveSameCount(collection);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to not have 3 item(s), but found 3.");
        }

        [Fact]
        public void When_comparing_not_same_item_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            var collection = new[] { 4, 5, 6 };

            // Act
            Action act = () => dictionary.Should().NotHaveSameCount(collection, "we want to test the {0}", "reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to not have 3 item(s) because we want to test the reason, but found 3.");
        }

        [Fact]
        public void When_asserting_dictionary_and_collection_to_not_have_same_count_against_null_dictionary_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => dictionary.Should().NotHaveSameCount(collection,
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to not have the same count as {1, 2, 3} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_dictionary_and_collection_to_not_have_same_count_against_a_null_collection_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            int[] collection = null;

            // Act
            Action act = () => dictionary.Should().NotHaveSameCount(collection);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify count against a <null> collection.*");
        }

        [Fact]
        public void
            When_asserting_dictionary_and_collection_to_not_have_same_count_but_both_reference_the_same_object_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            var collection = dictionary;

            // Act
            Action act = () => dictionary.Should().NotHaveSameCount(collection,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*not have the same count*because we want to test the behaviour with same objects*but they both reference the same object.");
        }
    }
}
