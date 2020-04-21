using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Collections;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class GenericCollectionAssertionOfStringSpecs
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
        public void Should_fail_when_asserting_collection_with_items_is_empty()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().BeEmpty();

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
        public void Should_succeed_when_asserting_collection_is_not_equivalent_to_a_different_collection()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "three", "one" };

            // Act / Assert
            collection1.Should().NotBeEquivalentTo(collection2);
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_with_unique_items_contains_only_unique_items()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three", "four" };

            // Act / Assert
            collection.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_without_items_is_empty()
        {
            // Arrange
            IEnumerable<string> collection = new string[0];

            // Act / Assert
            collection.Should().BeEmpty();
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
        public void When_a_collection_contains_duplicate_items_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three", "three" };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems("{0} don't like {1}", "we", "duplicates");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we don't like duplicates, but item \"three\" is not unique.");
        }

        [Fact]
        public void When_a_collection_contains_multiple_duplicate_items_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "two", "three", "three" };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems("{0} don't like {1}", "we", "duplicates");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we don't like duplicates, but items {\"two\", \"three\"} are not unique.");
        }

        [Fact]
        public void When_a_collection_does_not_contain_a_range_twice_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "one", "three", "twelve", "two", "two" };

            // Act
            Action act = () => collection.Should().ContainInOrder("one", "two", "one", "one", "two");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"one\", \"three\", \"twelve\", \"two\", \"two\"} to contain items {\"one\", \"two\", \"one\", \"one\", \"two\"} in order, but \"one\" (index 3) did not appear (in the right order).");
        }

        [Fact]
        public void When_a_collection_does_not_contain_an_ordered_item_it_should_throw_with_a_clear_explanation()
        {
            // Act
            Action act = () => new[] { "one", "two", "three" }.Should().ContainInOrder(new[] { "four", "one" }, "we failed");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to contain items {\"four\", \"one\"} in order because we failed, " +
                "but \"four\" (index 0) did not appear (in the right order).");
        }

        [Fact]
        public void When_a_collection_does_not_contain_another_collection_it_should_throw_with_clear_explanation()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().Contain(new[] { "three", "four", "five" }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to contain {\"three\", \"four\", \"five\"} because we do, but could not find {\"four\", \"five\"}.");
        }

        [Fact]
        public void When_a_collection_does_not_contain_single_item_it_should_throw_with_clear_explanation()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().Contain("four", "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to contain \"four\" because we do.");
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
                .Which.ParamName.Should().Be("equalityComparison");
        }

        [Fact]
        public void
            When_asserting_a_string_collection_contains_an_element_it_should_allow_specifying_the_reason_via_named_parameter()
        {
            // Arrange
            var expected = new List<string> { "hello", "world" };
            var actual = new List<string> { "hello", "world" };

            // Act
            Action act = () => expected.Should().Contain(actual, "they are in the collection");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_collection_contains_an_item_from_the_collection_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().Contain("one");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_collection_contains_multiple_items_from_the_collection_in_any_order_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().Contain(new[] { "two", "one" });

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_asserting_collection_contains_some_values_in_order_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> strings = null;

            // Act
            Action act =
                () => strings.Should()
                    .ContainInOrder(new[] { "string4" }, "because we're checking how it reacts to a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected strings to contain {\"string4\"} in order because we're checking how it reacts to a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_does_not_contain_item_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should()
                .NotContain("one", "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to not contain \"one\" because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_has_element_at_specific_index_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().HaveElementAt(1, "one",
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to have element at index 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_to_be_empty_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().BeEmpty("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_to_be_not_empty_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().NotBeEmpty("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to be empty because we want to test the behaviour with a null subject, but found <null>.");
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

        [Fact]
        public void When_asserting_collection_to_not_contain_nulls_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().NotContainNulls("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s because we want to test the behaviour with a null subject, but collection is <null>.");
        }

        [Fact]
        public void When_asserting_collection_to_not_intersect_with_same_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = collection;

            // Act
            Action act = () => collection.Should().NotIntersectWith(otherCollection,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*to intersect with*because we want to test the behaviour with same objects*but they both reference the same object.");
        }

        [Fact]
        public void When_asserting_collection_to_only_have_unique_items_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act =
                () => collection.Should().OnlyHaveUniqueItems("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_with_items_is_not_empty_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should().NotBeEmpty();
        }

        [Fact]
        public void When_asserting_collection_without_items_is_not_empty_it_should_fail()
        {
            // Arrange
            IEnumerable<string> collection = new string[0];

            // Act
            Action act = () => collection.Should().NotBeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_collection_without_items_is_not_empty_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            IEnumerable<string> collection = new string[0];

            // Act
            Action act = () => collection.Should().NotBeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection not to be empty because we want to test the failure message.");
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
                .And.ParamName.Should().Be("unexpected");
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
                .And.ParamName.Should().Be("expectation");
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
                .WithMessage("Expected collection not to be <null>*");
        }

        [Fact]
        public void When_asserting_collections_to_have_same_count_against_an_other_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = null;

            // Act
            Action act = () => collection.Should().HaveSameCount(otherCollection);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify count against a <null> collection.*");
        }

        [Fact]
        public void When_asserting_collections_to_have_same_count_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().HaveSameCount(collection1,
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to have the same count as {\"one\", \"two\", \"three\"} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collections_to_not_have_same_count_against_an_other_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = null;

            // Act
            Action act = () => collection.Should().NotHaveSameCount(otherCollection);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify count against a <null> collection.*");
        }

        [Fact]
        public void When_asserting_collections_to_not_have_same_count_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().NotHaveSameCount(collection1,
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to not have the same count as {\"one\", \"two\", \"three\"} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void
            When_asserting_collections_to_not_have_same_count_but_both_collections_references_the_same_object_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = collection;

            // Act
            Action act = () => collection.Should().NotHaveSameCount(otherCollection,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*not have the same count*because we want to test the behaviour with same objects*but they both reference the same object.");
        }

        [Fact]
        public void When_asserting_not_same_count_and_both_collections_have_the_same_number_elements_it_should_fail()
        {
            // Arrange
            IEnumerable<string> firstCollection = new[] { "one", "two", "three" };
            IEnumerable<string> secondCollection = new[] { "four", "five", "six" };

            // Act
            Action act = () => firstCollection.Should().NotHaveSameCount(secondCollection);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected firstCollection to not have 3 item(s), but found 3.");
        }

        [Fact]
        public void When_asserting_not_same_count_and_collections_have_different_number_elements_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> firstCollection = new[] { "one", "two", "three" };
            IEnumerable<string> secondCollection = new[] { "four", "six" };

            // Act / Assert
            firstCollection.Should().NotHaveSameCount(secondCollection);
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_intersecting_collections_do_not_intersect_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new[] { "two", "three", "four" };

            // Act
            Action action = () => collection.Should().NotIntersectWith(otherCollection, "they should not share items");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Did not expect collection to intersect with {\"two\", \"three\", \"four\"} because they should not share items," +
                    " but found the following shared items {\"two\", \"three\"}.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_intersecting_collections_intersect_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new[] { "three", "four", "five" };

            // Act / Assert
            collection.Should().IntersectWith(otherCollection);
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_non_intersecting_collections_do_not_intersect_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new[] { "four", "five" };

            // Act / Assert
            collection.Should().NotIntersectWith(otherCollection);
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_non_intersecting_collections_intersect_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new[] { "four", "five" };

            // Act
            Action action = () => collection.Should().IntersectWith(otherCollection, "they should share items");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to intersect with {\"four\", \"five\"} because they should share items," +
                             " but {\"one\", \"two\", \"three\"} does not contain any shared items.");
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
        public void When_both_collections_do_not_have_the_same_number_of_elements_it_should_fail()
        {
            // Arrange
            IEnumerable<string> firstCollection = new[] { "one", "two", "three" };
            IEnumerable<string> secondCollection = new[] { "four", "six" };

            // Act
            Action act = () => firstCollection.Should().HaveSameCount(secondCollection);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected firstCollection to have 2 item(s), but found 3.");
        }

        [Fact]
        public void When_both_collections_have_the_same_number_elements_it_should_succeed()
        {
            // Arrange
            IEnumerable<string> firstCollection = new[] { "one", "two", "three" };
            IEnumerable<string> secondCollection = new[] { "four", "five", "six" };

            // Act / Assert
            firstCollection.Should().HaveSameCount(secondCollection);
        }

        [Fact]
        public void When_collection_contains_an_unexpected_item_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().NotContain("one", "because we {0} like it, but found it anyhow", "don't");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to not contain \"one\" because we don't like it, but found it anyhow.");
        }

        [Fact]
        public void When_collection_contains_multiple_nulls_that_are_unexpected_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "", null, "", null };

            // Act
            Action act = () => collection.Should().NotContainNulls("because they are {0}", "evil");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s*because they are evil*{1, 3}*");
        }

        [Fact]
        public void When_collection_contains_null_value_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", null, "two", "string" };

            // Act / Assert
            collection.Should().ContainInOrder("one", null, "string");
        }

        [Fact]
        public void When_collection_contains_nulls_that_are_unexpected_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "", null };

            // Act
            Action act = () => collection.Should().NotContainNulls("because they are {0}", "evil");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s because they are evil, but found one at index 1.");
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
        public void When_collection_does_contain_an_unexpected_item_matching_a_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().NotContain(item => item == "two", "because {0}s are evil", "two");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to not have any items matching (item == \"two\") because twos are evil,*{\"two\"}*");
        }

        [Fact]
        public void When_collection_does_not_contain_an_item_that_is_not_in_the_collection_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().NotContain("four");

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_collection_does_not_contain_an_unexpected_item_matching_a_predicate_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should().NotContain(item => item == "four");
        }

        [Fact]
        public void When_collection_does_not_contain_nulls_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should().NotContainNulls();
        }

        [Fact]
        public void When_collection_does_not_have_an_element_at_the_specific_index_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().HaveElementAt(4, "three", "we put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected \"three\" at index 4 because we put it there, but found no element.");
        }

        [Fact]
        public void When_collection_does_not_have_the_expected_element_at_specific_index_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().HaveElementAt(1, "three", "we put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected \"three\" at index 1 because we put it there, but found \"two\".");
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
                    "Expected collection {\"one\", \"two\", \"three\"} to contain 4 item(s) because we want to test the failure message, but found 3.");
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
                "Expected collection {\"one\", \"two\", \"three\"} to have a count (c >= 4) because a minimum of 4 is required, but count is 3.");
        }

        [Fact]
        public void When_collection_has_expected_element_at_specific_index_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act / Assert
            collection.Should().HaveElementAt(1, "two");
        }

        [Fact]
        public void When_collection_is_expected_to_be_null_and_it_is_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> someCollection = null;

            // Act / Assert
            someCollection.Should().BeNull();
        }

        [Fact]
        public void When_collection_is_expected_to_be_null_and_it_isnt_it_should_throw()
        {
            // Arrange
            IEnumerable<string> someCollection = new string[0];

            // Act
            Action act = () => someCollection.Should().BeNull("because {0} is valid", "null");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someCollection to be <null> because null is valid, but found {empty}.");
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
        public void When_collection_is_not_expected_to_be_null_and_it_is_it_should_throw()
        {
            // Arrange
            IEnumerable<string> someCollection = null;

            // Act
            Action act = () => someCollection.Should().NotBeNull("because {0} should not", "someCollection");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someCollection not to be <null> because someCollection should not.");
        }

        [Fact]
        public void When_collection_is_not_expected_to_be_null_and_it_isnt_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> someCollection = new string[0];

            // Act / Assert
            someCollection.Should().NotBeNull();
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
        public void When_collections_with_duplicates_are_not_equivalent_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection1 = new[] { "one", "two", "three", "one" };
            IEnumerable<string> collection2 = new[] { "one", "two", "three", "three" };

            // Act
            Action act = () => collection1.Should().BeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected item[3] to be \"three\" with a length of 5, but \"one\" has a length of 3*");
        }

        [Fact]
        public void When_comparing_item_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            // Arrange
            IEnumerable<string> firstCollection = new[] { "one", "two", "three" };
            IEnumerable<string> secondCollection = new[] { "four", "six" };

            // Act
            Action act = () => firstCollection.Should().HaveSameCount(secondCollection, "we want to test the {0}", "reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected firstCollection to have 2 item(s) because we want to test the reason, but found 3.");
        }

        [Fact]
        public void When_comparing_not_same_item_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            // Arrange
            IEnumerable<string> firstCollection = new[] { "one", "two", "three" };
            IEnumerable<string> secondCollection = new[] { "four", "five", "six" };

            // Act
            Action act = () => firstCollection.Should().NotHaveSameCount(secondCollection, "we want to test the {0}", "reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected firstCollection to not have 3 item(s) because we want to test the reason, but found 3.");
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
        public void When_passing_in_null_while_checking_for_ordered_containment_it_should_throw_with_a_clear_explanation()
        {
            // Act
            Action act = () => new[] { "one", "two", "three" }.Should().ContainInOrder(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify ordered containment against a <null> collection.*");
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
                "Expected subject to be a collection with 0 item(s), but*contains 3 item(s)*");
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
                "Expected collection1 to be <null>, but found {\"one\", \"two\", \"three\"}*");
        }

        [Fact]
        public void When_the_collection_is_not_empty_unexpectedly_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().BeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to be empty because we want to test the failure message, but found*one*two*three*");
        }

        [Fact]
        public void When_the_contents_of_a_collection_are_checked_against_an_empty_collection_it_should_throw_clear_explanation()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().Contain(new int[0]);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify containment against an empty collection*");
        }

        [Fact]
        public void When_the_expected_object_exists_it_should_allow_chaining_additional_assertions()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().Contain("one").Which.Should().HaveLength(4);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*length*4*3*");
        }

        [Fact]
        public void When_the_first_collection_contains_a_duplicate_item_without_affecting_the_order_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three", "two" };

            // Act / Assert
            collection.Should().ContainInOrder("one", "two", "three");
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
        public void When_two_collections_contain_the_same_duplicate_items_in_the_same_order_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "one", "two", "twelve", "two", "two" };

            // Act / Assert
            collection.Should().ContainInOrder("one", "two", "one", "two", "twelve", "two", "two");
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
        public void When_two_collections_contain_the_same_items_but_in_different_order_it_should_throw_with_a_clear_explanation()
        {
            // Act
            Action act = () =>
                new[] { "one", "two", "three" }.Should().ContainInOrder(new[] { "three", "one" }, "because we said so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to contain items {\"three\", \"one\"} in order because we said so, but \"one\" (index 1) did not appear (in the right order).");
        }

        [Fact]
        public void When_two_collections_contain_the_same_items_in_the_same_order_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "two", "three" };

            // Act / Assert
            collection.Should().ContainInOrder("one", "two", "three");
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

        [Fact]
        public void When_using_StringCollectionAssertions_the_AndConstraint_should_have_the_correct_type()
        {
            // Arrange
            MethodInfo[] methodInfo =
                typeof(StringCollectionAssertions<IEnumerable<string>>).GetMethods(
                    BindingFlags.Public | BindingFlags.Instance);

            // Act
            var methods =
                from method in methodInfo
                where !method.IsSpecialName // Exclude Properties
                where method.DeclaringType != typeof(object)
                select new { method.Name, method.ReturnType };

            // Assert
            methods.Should().OnlyContain(method =>
                typeof(AndConstraint<StringCollectionAssertions<IEnumerable<string>>>)
                    .IsAssignableFrom(method.ReturnType));
        }

        #region ContainMatch

        [Fact]
        public void When_collection_contains_a_match_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().ContainMatch("* failed");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_collection_contains_multiple_matches_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { "build succeded", "test failed", "pack failed" };

            // Act
            Action action = () => collection.Should().ContainMatch("* failed");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_collection_contains_multiple_matches_which_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { "build succeded", "test failed", "pack failed" };

            // Act
            Action action = () =>
            {
                string item = collection.Should().ContainMatch("* failed").Which;
            };

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("More than one object found.  FluentAssertions cannot determine which object is meant.*")
                .WithMessage("*Found objects:*\"test failed\"*\"pack failed\"");
        }

        [Fact]
        public void When_collection_does_not_contain_a_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().ContainMatch("* stopped", "because {0}", "we do");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection {\"build succeded\", \"test failed\"} to contain a match of \"* stopped\" because we do.");
        }

        [Fact]
        public void When_collection_contains_a_match_that_differs_in_casing_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().ContainMatch("* Failed");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection {\"build succeded\", \"test failed\"} to contain a match of \"* Failed\".");
        }

        [Fact]
        public void When_asserting_empty_collection_for_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { };

            // Act
            Action action = () => collection.Should().ContainMatch("* failed");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection {empty} to contain a match of \"* failed\".");
        }

        [Fact]
        public void When_asserting_null_collection_for_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action action = () => collection.Should().ContainMatch("* failed", "because {0}", "we do");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to contain a match of \"* failed\" because we do, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_to_have_null_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().ContainMatch(null);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection {\"build succeded\", \"test failed\"} to contain a match of <null>.");
        }

        #endregion

        #region NotContainMatch

        [Fact]
        public void When_collection_doesnt_contain_a_match_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { "build succeded", "test" };

            // Act
            Action action = () => collection.Should().NotContainMatch("* failed");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_collection_doesnt_contain_multiple_matches_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { "build succeded", "test", "pack" };

            // Act
            Action action = () => collection.Should().NotContainMatch("* failed");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_collection_contains_a_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().NotContainMatch("* failed", "because {0}", "it shouldn't");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection {\"build succeded\", \"test failed\"} to contain a match of \"* failed\" because it shouldn't.");
        }

        [Fact]
        public void When_collection_contains_multiple_matches_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { "build failed", "test failed" };

            // Act
            Action action = () => collection.Should().NotContainMatch("* failed", "because {0}", "it shouldn't");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection {\"build failed\", \"test failed\"} to contain a match of \"* failed\" because it shouldn't.");
        }

        [Fact]
        public void When_collection_contains_a_match_with_different_casing_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> collection = new string[] { "build succeded", "test failed" };

            // Act
            Action action = () => collection.Should().NotContainMatch("* Failed");

            // Assert
            action.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_asserting_null_collection_for_not_match_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action action = () => collection.Should().NotContainMatch(null);

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to contain a match of <null>, but found <null>.");
        }

        #endregion

        #region SatisfyRespectively

        [Fact]
        public void When_string_collection_satisfies_all_inspectors_it_should_succeed()
        {
            // Arrange
            string[] collection = new[] { "John", "Jane" };

            // Act / Assert
            collection.Should().SatisfyRespectively(
                value => value.Should().Be("John"),
                value => value.Should().Be("Jane")
            );
        }

        [Fact]
        public void When_string_collection_does_not_satisfy_all_inspectors_it_should_throw()
        {
            // Arrange
            string[] collection = new[] { "Jack", "Jessica" };

            // Act
            Action act = () => collection.Should().SatisfyRespectively(new Action<string>[]
            {
                value => value.Should().Be("John"),
                value => value.Should().Be("Jane")
            }, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to satisfy all inspectors because we want to test the failure message, but some inspectors are not satisfied"
                + "*John*Jack"
                + "*Jane*Jessica*");
        }

        #endregion
    }
}
