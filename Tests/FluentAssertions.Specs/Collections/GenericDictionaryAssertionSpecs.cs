using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class GenericDictionaryAssertionSpecs
    {
        // If you try to implement support for IReadOnlyDictionary, these tests should still succeed.
        #region Sanity Checks

        [Fact]
        public void When_the_same_dictionaries_are_expected_to_be_the_same_it_should_not_fail()
        {
            // Arrange
            IDictionary<int, string> dictionary = new Dictionary<int, string>();
            IDictionary<int, string> referenceToDictionary = dictionary;

            // Act / Assert
            dictionary.Should().BeSameAs(referenceToDictionary);
        }

        [Fact]
        public void When_the_same_custom_dictionaries_are_expected_to_be_the_same_it_should_not_fail()
        {
            // Arrange
            IDictionary<int, string> dictionary = new DictionaryNotImplementingIReadOnlyDictionary<int, string>();
            IDictionary<int, string> referenceToDictionary = dictionary;

            // Act / Assert
            dictionary.Should().BeSameAs(referenceToDictionary);
        }

        [Fact]
        public void When_object_type_is_exactly_equal_to_the_specified_type_it_should_not_fail()
        {
            // Arrange
            IDictionary<int, string> dictionary = new DictionaryNotImplementingIReadOnlyDictionary<int, string>();

            // Act / Assert
            dictionary.Should().BeOfType<DictionaryNotImplementingIReadOnlyDictionary<int, string>>();
        }

        [Fact]
        public void When_a_dictionary_does_not_implement_the_read_only_interface_it_should_have_dictionary_assertions()
        {
            // Arrange
            IDictionary<int, string> dictionary = new DictionaryNotImplementingIReadOnlyDictionary<int, string>();

            // Act / Assert
            dictionary.Should().NotContainKey(0, "Dictionaries not implementing IReadOnlyDictionary<TKey, TValue> "
                + "should be supported at least until Fluent Assertions 6.0.");
        }

        #endregion

        #region Be Null

        [Fact]
        public void When_dictionary_is_expected_to_be_null_and_it_is_it_should_not_throw()
        {
            // Arrange
            IDictionary<int, string> someDictionary = null;

            // Act / Assert
            someDictionary.Should().BeNull();
        }

        [Fact]
        public void When_a_custom_dictionary_implementation_is_expected_not_to_be_null_and_it_is_it_should_not_throw()
        {
            // Arrange
            var dictionary = new TrackingTestDictionary();

            // Act / Assert
            dictionary.Should().NotBeNull();
        }

        [Fact]
        public void When_dictionary_is_expected_to_be_null_and_it_isnt_it_should_throw()
        {
            // Arrange
            var someDictionary = new Dictionary<int, string>();

            // Act
            Action act = () => someDictionary.Should().BeNull("because {0} is valid", "null");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someDictionary to be <null> because null is valid, but found {empty}.");
        }

        [Fact]
        public void When_dictionary_is_not_expected_to_be_null_and_it_isnt_it_should_not_throw()
        {
            // Arrange
            IDictionary<int, string> someDictionary = new Dictionary<int, string>();

            // Act / Assert
            someDictionary.Should().NotBeNull();
        }

        [Fact]
        public void When_dictionary_is_not_expected_to_be_null_and_it_is_it_should_throw()
        {
            // Arrange
            IDictionary<int, string> someDictionary = null;

            // Act
            Action act = () => someDictionary.Should().NotBeNull("because {0} should not", "someDictionary");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someDictionary not to be <null> because someDictionary should not.");
        }

        #endregion

        #region Have Count

        [Fact]
        public void Should_succeed_when_asserting_dictionary_has_a_count_that_equals_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act / Assert
            dictionary.Should().HaveCount(3);
        }

        [Fact]
        public void Should_fail_when_asserting_dictionary_has_a_count_that_is_different_from_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().HaveCount(4);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_dictionary_has_a_count_that_is_different_from_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action action = () => dictionary.Should().HaveCount(4, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected dictionary {[1, One], [2, Two], [3, Three]} to contain 4 item(s) because we want to test the failure message, but found 3.");
        }

        [Fact]
        public void When_dictionary_has_a_count_larger_than_the_minimum_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act / Assert
            dictionary.Should().HaveCount(c => c >= 3);
        }

        [Fact]
        public void When_dictionary_has_a_count_that_not_matches_the_predicate_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().HaveCount(c => c >= 4, "a minimum of 4 is required");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two], [3, Three]} to have a count (c >= 4) because a minimum of 4 is required, but count is 3.");
        }

        [Fact]
        public void When_dictionary_count_is_matched_against_a_null_predicate_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().HaveCount(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare collection count against a <null> predicate.*");
        }

        [Fact]
        public void When_dictionary_count_is_matched_and_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().HaveCount(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain 1 item(s) because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_dictionary_count_is_matched_against_a_predicate_and_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().HaveCount(c => c < 3, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain (c < 3) items because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Not Have Count

        [Fact]
        public void Should_succeed_when_asserting_dictionary_has_a_count_different_from_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act / Assert
            dictionary.Should().NotHaveCount(2);
        }

        [Fact]
        public void Should_fail_when_asserting_dictionary_has_a_count_that_equals_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().NotHaveCount(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_dictionary_has_a_count_that_equals_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action action = () => dictionary.Should().NotHaveCount(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*not contain*3*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_dictionary_count_is_same_than_and_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().NotHaveCount(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*not contain*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Count Greater Than

        [Fact]
        public void Should_succeed_when_asserting_dictionary_has_a_count_greater_than_less_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act / Assert
            dictionary.Should().HaveCountGreaterThan(2);
        }

        [Fact]
        public void Should_fail_when_asserting_dictionary_has_a_count_greater_than_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().HaveCountGreaterThan(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_dictionary_has_a_count_greater_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action action = () => dictionary.Should().HaveCountGreaterThan(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*more than*3*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_dictionary_count_is_greater_than_and_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().HaveCountGreaterThan(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*more than*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Count Greater Or Equal To

        [Fact]
        public void Should_succeed_when_asserting_dictionary_has_a_count_greater_or_equal_to_less_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act / Assert
            dictionary.Should().HaveCountGreaterOrEqualTo(3);
        }

        [Fact]
        public void Should_fail_when_asserting_dictionary_has_a_count_greater_or_equal_to_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().HaveCountGreaterOrEqualTo(4);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_dictionary_has_a_count_greater_or_equal_to_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action action = () => dictionary.Should().HaveCountGreaterOrEqualTo(4, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*at least*4*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_dictionary_count_is_greater_or_equal_to_and_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().HaveCountGreaterOrEqualTo(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*at least*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Count Less Than

        [Fact]
        public void Should_succeed_when_asserting_dictionary_has_a_count_less_than_less_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act / Assert
            dictionary.Should().HaveCountLessThan(4);
        }

        [Fact]
        public void Should_fail_when_asserting_dictionary_has_a_count_less_than_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().HaveCountLessThan(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_dictionary_has_a_count_less_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action action = () => dictionary.Should().HaveCountLessThan(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*fewer than*3*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_dictionary_count_is_less_than_and_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().HaveCountLessThan(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*fewer than*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Count Less Or Equal To

        [Fact]
        public void Should_succeed_when_asserting_dictionary_has_a_count_less_or_equal_to_less_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act / Assert
            dictionary.Should().HaveCountLessOrEqualTo(3);
        }

        [Fact]
        public void Should_fail_when_asserting_dictionary_has_a_count_less_or_equal_to_the_number_of_items()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action act = () => dictionary.Should().HaveCountLessOrEqualTo(2);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_dictionary_has_a_count_less_or_equal_to_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };

            // Act
            Action action = () => dictionary.Should().HaveCountLessOrEqualTo(2, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*at most*2*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_dictionary_count_is_less_or_equal_to_and_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().HaveCountLessOrEqualTo(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*at most*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Same Count

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
            IEnumerable collection = new[] { 4, 5, 6 };

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
            IEnumerable collection = new[] { 4, 6 };

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
            IEnumerable collection = new[] { 4, 6 };

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
            IEnumerable collection = new[] { 1, 2, 3 };

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
            IEnumerable collection = null;

            // Act
            Action act = () => dictionary.Should().HaveSameCount(collection);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify count against a <null> collection.*");
        }

        #endregion

        #region Not Have Same Count

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
            IEnumerable collection = new[] { 4, 6 };

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
            IEnumerable collection = new[] { 4, 5, 6 };

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
            IEnumerable collection = new[] { 4, 5, 6 };

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
            IEnumerable collection = new[] { 1, 2, 3 };

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
            IEnumerable collection = null;

            // Act
            Action act = () => dictionary.Should().NotHaveSameCount(collection);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify count against a <null> collection.*");
        }

        [Fact]
        public void When_asserting_dictionary_and_collection_to_not_have_same_count_but_both_reference_the_same_object_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };
            IEnumerable collection = dictionary;

            // Act
            Action act = () => dictionary.Should().NotHaveSameCount(collection,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*not have the same count*because we want to test the behaviour with same objects*but they both reference the same object.");
        }

        #endregion

        #region Be Empty

        [Fact]
        public void Should_succeed_when_asserting_dictionary_without_items_is_empty()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            // Act / Assert
            dictionary.Should().BeEmpty();
        }

        [Fact]
        public void Should_fail_when_asserting_dictionary_with_items_is_empty()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One"
            };

            // Act
            Action act = () => dictionary.Should().BeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_dictionary_with_items_is_empty()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One"
            };

            // Act
            Action act = () => dictionary.Should().BeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected dictionary to be empty because we want to test the failure message, but found {[1, One]}.");
        }

        [Fact]
        public void When_asserting_dictionary_with_items_is_not_empty_it_should_succeed()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One"
            };

            // Act / Assert
            dictionary.Should().NotBeEmpty();
        }

        [Fact]
        public void When_asserting_dictionary_with_items_is_not_empty_it_should_enumerate_the_dictionary_only_once()
        {
            // Arrange
            var trackingDictionary = new TrackingTestDictionary(new KeyValuePair<int, string>(1, "One"));

            // Act
            trackingDictionary.Should().NotBeEmpty();

            // Assert
            trackingDictionary.Enumerator.LoopCount.Should().Be(1);
        }

        [Fact]
        public void When_asserting_dictionary_without_items_is_not_empty_it_should_fail()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            // Act
            Action act = () => dictionary.Should().NotBeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_dictionary_without_items_is_not_empty_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            // Act
            Action act = () => dictionary.Should().NotBeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected dictionary not to be empty because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_dictionary_to_be_empty_but_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().BeEmpty("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_dictionary_to_be_not_empty_but_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().NotBeEmpty("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary not to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Equal

        [Fact]
        public void Should_succeed_when_asserting_dictionary_is_equal_to_the_same_dictionary()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            var dictionary2 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            dictionary1.Should().Equal(dictionary2);
        }

        [Fact]
        public void Should_succeed_when_asserting_dictionary_with_null_value_is_equal_to_the_same_dictionary()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = null
            };
            var dictionary2 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = null
            };

            // Act / Assert
            dictionary1.Should().Equal(dictionary2);
        }

        [Fact]
        public void When_asserting_dictionaries_to_be_equal_but_subject_dictionary_misses_a_value_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            var dictionary2 = new Dictionary<int, string>
            {
                [1] = "One",
                [22] = "Two"
            };

            // Act
            Action act = () => dictionary1.Should().Equal(dictionary2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary1 to be equal to {[1, One], [22, Two]} because we want to test the failure message, but could not find keys {22}.");
        }

        [Fact]
        public void When_asserting_dictionaries_to_be_equal_but_subject_dictionary_has_extra_key_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three"
            };
            var dictionary2 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary1.Should().Equal(dictionary2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary1 to be equal to {[1, One], [2, Two]} because we want to test the failure message, but found additional keys {3}.");
        }

        [Fact]
        public void When_two_dictionaries_are_not_equal_by_values_it_should_throw_using_the_reason()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            var dictionary2 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Three"
            };

            // Act
            Action act = () => dictionary1.Should().Equal(dictionary2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary1 to be equal to {[1, One], [2, Three]} because we want to test the failure message, but {[1, One], [2, Two]} differs at key 2.");
        }

        [Fact]
        public void When_asserting_dictionaries_to_be_equal_but_subject_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary1 = null;
            var dictionary2 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary1.Should().Equal(dictionary2, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary1 to be equal to {[1, One], [2, Two]} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_dictionaries_to_be_equal_but_expected_dictionary_is_null_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            Dictionary<int, string> dictionary2 = null;

            // Act
            Action act = () => dictionary1.Should().Equal(dictionary2, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare dictionary with <null>.*")
                .And.ParamName.Should().Be("expected");
        }

        [Fact]
        public void When_an_empty_dictionary_is_compared_for_equality_to_a_non_empty_dictionary_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>();
            var dictionary2 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary1.Should().Equal(dictionary2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary1 to be equal to {[1, One], [2, Two]}, but could not find keys {1, 2}.");
        }

        [Fact]
        public void Should_succeed_when_asserting_dictionary_is_not_equal_to_a_dictionary_with_different_key()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            var dictionary2 = new Dictionary<int, string>
            {
                [1] = "One",
                [22] = "Two"
            };

            // Act / Assert
            dictionary1.Should().NotEqual(dictionary2);
        }

        [Fact]
        public void Should_succeed_when_asserting_dictionary_is_not_equal_to_a_dictionary_with_different_value()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = null
            };
            var dictionary2 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            dictionary1.Should().NotEqual(dictionary2);
        }

        [Fact]
        public void When_two_equal_dictionaries_are_not_expected_to_be_equal_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            var dictionary2 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary1.Should().NotEqual(dictionary2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect dictionaries {[1, One], [2, Two]} and {[1, One], [2, Two]} to be equal.");
        }

        [Fact]
        public void When_two_equal_dictionaries_are_not_expected_to_be_equal_it_should_report_a_clear_explanation()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            var dictionary2 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary1.Should().NotEqual(dictionary2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect dictionaries {[1, One], [2, Two]} and {[1, One], [2, Two]} to be equal because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_dictionaries_not_to_be_equal_subject_but_dictionary_is_null_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary1 = null;
            var dictionary2 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act =
                () => dictionary1.Should().NotEqual(dictionary2, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionaries not to be equal because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_dictionaries_not_to_be_equal_but_expected_dictionary_is_null_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            Dictionary<int, string> dictionary2 = null;

            // Act
            Action act =
                () => dictionary1.Should().NotEqual(dictionary2, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare dictionary with <null>.*")
                .And.ParamName.Should().Be("unexpected");
        }

        [Fact]
        public void When_asserting_dictionaries_not_to_be_equal_subject_but_both_dictionaries_reference_the_same_object_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            var dictionary2 = dictionary1;

            // Act
            Action act =
                () => dictionary1.Should().NotEqual(dictionary2, "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionaries not to be equal because we want to test the behaviour with same objects, but they both reference the same object.");
        }

        #endregion

        #region ContainKey

        [Fact]
        public void Should_succeed_when_asserting_dictionary_contains_a_key_from_the_dictionary()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            dictionary.Should().ContainKey(1);
        }

        [Fact]
        public void When_a_dictionary_has_custom_equality_comparer_the_contains_key_assertion_should_work_accordingly()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["One"] = "One",
                ["Two"] = "Two"
            };

            // Act

            // Assert
            dictionary.Should().ContainKey("One");
            dictionary.Should().ContainKey("ONE");
            dictionary.Should().ContainKey("one");
        }

        [Fact]
        public void Should_succeed_when_asserting_dictionary_contains_multiple_keys_from_the_dictionary()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            dictionary.Should().ContainKeys(2, 1);
        }

        [Fact]
        public void When_a_dictionary_does_not_contain_single_key_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainKey(3, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to contain key 3 because we do.");
        }

        [Fact]
        public void When_the_requested_key_exists_it_should_allow_continuation_with_the_value()
        {
            // Arrange
            var dictionary = new Dictionary<string, MyClass>
            {
                ["Key"] = new MyClass { SomeProperty = 3 }
            };

            // Act
            Action act = () => dictionary.Should().ContainKey("Key").WhichValue.Should().Be(4);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*4*3*.");
        }

        public class MyClass
        {
            public int SomeProperty { get; set; }

            protected bool Equals(MyClass other)
            {
                return SomeProperty == other.SomeProperty;
            }

            public override bool Equals(object obj)
            {
                if (obj is null)
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != this.GetType())
                {
                    return false;
                }

                return Equals((MyClass)obj);
            }

            public override int GetHashCode()
            {
                return SomeProperty;
            }
        }

        [Fact]
        public void When_a_dictionary_does_not_contain_a_list_of_keys_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainKeys(new[] { 2, 3 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to contain key {2, 3} because we do, but could not find {3}.");
        }

        [Fact]
        public void When_the_contents_of_a_dictionary_are_checked_against_an_empty_list_of_keys_it_should_throw_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainKeys(new int[0]);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify key containment against an empty sequence*");
        }

        [Fact]
        public void When_dictionary_does_not_contain_a_key_that_is_not_in_the_dictionary_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKey(4);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_dictionary_does_not_contain_multiple_keys_from_the_dictionary_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKeys(3, 4);

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_dictionary_contains_an_unexpected_key_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKey(1, "because we {0} like it", "don't");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} not to contain key 1 because we don't like it, but found it anyhow.");
        }

        [Fact]
        public void When_asserting_dictionary_does_not_contain_key_against_null_dictionary_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should()
                .NotContainKey(1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary not to contain key 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_a_dictionary_contains_a_list_of_keys_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKeys(new[] { 2, 3 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to not contain key {2, 3} because we do, but found {2}.");
        }

        [Fact]
        public void When_a_dictionary_contains_exactly_one_of_the_keys_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKeys(new[] { 2 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to not contain key 2 because we do.");
        }

        [Fact]
        public void When_the_noncontents_of_a_dictionary_are_checked_against_an_empty_list_of_keys_it_should_throw_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKeys(new int[0]);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify key containment against an empty sequence*");
        }

        [Fact]
        public void When_a_dictionary_checks_a_list_of_keys_not_to_be_present_it_will_honor_the_case_sensitive_equality_comparer_of_the_dictionary()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["ONE"] = "One",
                ["TWO"] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKeys(new[] { "One", "Two" });

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_a_dictionary_checks_a_list_of_keys_not_to_be_present_it_will_honor_the_case_insensitive_equality_comparer_of_the_dictionary()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["ONE"] = "One",
                ["TWO"] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainKeys(new[] { "One", "Two" });

            // Assert
            act.Should().Throw<XunitException>();
        }
        #endregion

        #region ContainValue

        [Fact]
        public void When_dictionary_contains_expected_value_it_should_succeed()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainValue("One");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_dictionary_contains_expected_null_value_it_should_succeed()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = null
            };

            // Act
            Action act = () => dictionary.Should().ContainValue(null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_specified_value_exists_it_should_allow_continuation_using_that_value()
        {
            // Arrange
            var myClass = new MyClass()
            {
                SomeProperty = 0
            };

            var dictionary = new Dictionary<int, MyClass>
            {
                [1] = myClass
            };

            // Act
            Action act = () => dictionary.Should().ContainValue(myClass).Which.SomeProperty.Should().BeGreaterThan(0);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*greater*0*0*");
        }

        [Fact]
        public void When_multiple_matches_for_the_specified_value_exist_continuation_using_the_matched_value_should_fail()
        {
            // Arrange
            var myClass = new MyClass { SomeProperty = 0 };

            var dictionary = new Dictionary<int, MyClass>
            {
                [1] = myClass,
                [2] = new MyClass { SomeProperty = 0 }
            };

            // Act
            Action act =
                () =>
                    dictionary.Should()
                        .ContainValue(new MyClass { SomeProperty = 0 })
                        .Which.Should()
                        .BeSameAs(myClass);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_dictionary_contains_multiple_values_from_the_dictionary_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainValues("Two", "One");

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_a_dictionary_does_not_contain_single_value_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainValue("Three", "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to contain value \"Three\" because we do.");
        }

        [Fact]
        public void When_a_dictionary_does_not_contain_a_number_of_values_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainValues(new[] { "Two", "Three" }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to contain value {\"Two\", \"Three\"} because we do, but could not find {\"Three\"}.");
        }

        [Fact]
        public void When_the_contents_of_a_dictionary_are_checked_against_an_empty_list_of_values_it_should_throw_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainValues(new string[0]);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify value containment with an empty sequence*");
        }

        [Fact]
        public void When_dictionary_does_not_contain_a_value_that_is_not_in_the_dictionary_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainValue("Three");

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_dictionary_contains_an_unexpected_value_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainValue("One", "because we {0} like it", "don't");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} not to contain value \"One\" because we don't like it, but found it anyhow.");
        }

        [Fact]
        public void When_asserting_dictionary_does_not_contain_value_against_null_dictionary_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should()
                .NotContainValue("One", "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary not to contain value \"One\" because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_dictionary_does_not_contain_multiple_values_that_is_not_in_the_dictionary_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainValues("Three", "Four");

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_a_dictionary_contains_a_exactly_one_of_the_values_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainValues(new[] { "Two" }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to not contain value \"Two\" because we do.");
        }

        [Fact]
        public void When_a_dictionary_contains_a_number_of_values_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainValues(new[] { "Two", "Three" }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to not contain value {\"Two\", \"Three\"} because we do, but found {\"Two\"}.");
        }

        [Fact]
        public void When_the_noncontents_of_a_dictionary_are_checked_against_an_empty_list_of_values_it_should_throw_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainValues(new string[0]);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify value containment with an empty sequence*");
        }

        #endregion

        #region Contain

        [Fact]
        public void Should_succeed_when_asserting_dictionary_contains_single_key_value_pair()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "One")
            };

            // Act / Assert
            dictionary.Should().Contain(keyValuePairs);
        }

        [Fact]
        public void Should_succeed_when_asserting_dictionary_contains_multiple_key_value_pair()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two",
                [3] = "Three",
                [4] = "Four"
            };

            var expectedKeyValuePair1 = new KeyValuePair<int, string>(2, "Two");
            var expectedKeyValuePair2 = new KeyValuePair<int, string>(3, "Three");

            // Act / Assert
            dictionary.Should().Contain(expectedKeyValuePair1, expectedKeyValuePair2);
        }

        [Fact]
        public void Should_succeed_when_asserting_dictionary_does_not_contain_single_key_value_pair()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(3, "Three")
            };

            // Act / Assert
            dictionary.Should().NotContain(keyValuePairs);
        }

        [Fact]
        public void Should_succeed_when_asserting_dictionary_does_not_contain_multiple_key_value_pair()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var unexpectedKeyValuePair1 = new KeyValuePair<int, string>(3, "Three");
            var unexpectedKeyValuePair2 = new KeyValuePair<int, string>(4, "Four");

            // Act / Assert
            dictionary.Should().NotContain(unexpectedKeyValuePair1, unexpectedKeyValuePair2);
        }

        [Fact]
        public void Should_succeed_when_asserting_dictionary_does_not_contain_single_key_value_pair_with_existing_key_but_different_value()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "Two")
            };

            // Act / Assert
            dictionary.Should().NotContain(keyValuePairs);
        }

        [Fact]
        public void Should_succeed_when_asserting_dictionary_contains_multiple_key_value_pairs()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "One"),
                new KeyValuePair<int, string>(2, "Two")
            };

            // Act / Assert
            dictionary.Should().Contain(keyValuePairs);
        }

        [Fact]
        public void Should_succeed_when_asserting_dictionary_does_not_contain_multiple_key_value_pairs()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(3, "Three"),
                new KeyValuePair<int, string>(4, "Four")
            };

            // Act / Assert
            dictionary.Should().NotContain(keyValuePairs);
        }

        [Fact]
        public void Should_succeed_when_asserting_dictionary_does_not_contain_multiple_key_value_pairs_with_existing_keys_but_different_values()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "Three"),
                new KeyValuePair<int, string>(2, "Four")
            };

            // Act / Assert
            dictionary.Should().NotContain(keyValuePairs);
        }

        [Fact]
        public void When_a_dictionary_does_not_contain_single_value_for_key_value_pairs_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "One"),
                new KeyValuePair<int, string>(2, "Three")
            };

            // Act
            Action act = () => dictionary.Should().Contain(keyValuePairs, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain value \"Three\" at key 2 because we do, but found \"Two\".");
        }

        [Fact]
        public void When_a_dictionary_does_not_contain_multiple_values_for_key_value_pairs_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "Two"),
                new KeyValuePair<int, string>(2, "Three")
            };

            // Act
            Action act = () => dictionary.Should().Contain(keyValuePairs, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain {[1, Two], [2, Three]} because we do, but dictionary differs at keys {1, 2}.");
        }

        [Fact]
        public void When_a_dictionary_does_not_contain_single_key_for_key_value_pairs_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(3, "Three")
            };

            // Act
            Action act = () => dictionary.Should().Contain(keyValuePairs, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to contain key 3 because we do.");
        }

        [Fact]
        public void When_a_dictionary_does_not_contain_multiple_keys_for_key_value_pairs_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "One"),
                new KeyValuePair<int, string>(3, "Three"),
                new KeyValuePair<int, string>(4, "Four")
            };

            // Act
            Action act = () => dictionary.Should().Contain(keyValuePairs, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to contain key(s) {1, 3, 4} because we do, but could not find keys {3, 4}.");
        }

        [Fact]
        public void When_a_dictionary_does_contain_single_key_value_pair_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "One")
            };

            // Act
            Action act = () => dictionary.Should().NotContain(keyValuePairs, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to not contain value \"One\" at key 1 because we do, but found it anyhow.");
        }

        [Fact]
        public void When_a_dictionary_does_contain_multiple_key_value_pairs_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "One"),
                new KeyValuePair<int, string>(2, "Two")
            };

            // Act
            Action act = () => dictionary.Should().NotContain(keyValuePairs, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to not contain key/value pairs {[1, One], [2, Two]} because we do, but found them anyhow.");
        }

        [Fact]
        public void When_asserting_dictionary_contains_key_value_pairs_against_null_dictionary_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;
            List<KeyValuePair<int, string>> keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "One"),
                new KeyValuePair<int, string>(1, "Two")
            };

            // Act
            Action act = () => dictionary.Should().Contain(keyValuePairs,
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain key/value pairs {[1, One], [1, Two]} because we want to test the behaviour with a null subject, but dictionary is <null>.");
        }

        [Fact]
        public void When_asserting_dictionary_contains_key_value_pairs_but_expected_key_value_pairs_are_empty_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            List<KeyValuePair<int, string>> keyValuePairs = new List<KeyValuePair<int, string>>();

            // Act
            Action act = () => dictionary1.Should().Contain(keyValuePairs, "because we want to test the behaviour with an empty set of key/value pairs");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify key containment against an empty collection of key/value pairs*");
        }

        [Fact]
        public void When_asserting_dictionary_contains_key_value_pairs_but_expected_key_value_pairs_are_null_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            List<KeyValuePair<int, string>> keyValuePairs = null;

            // Act
            Action act = () => dictionary1.Should().Contain(keyValuePairs, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare dictionary with <null>.*")
                .And.ParamName.Should().Be("expected");
        }

        [Fact]
        public void When_asserting_dictionary_does_not_contain_key_value_pairs_against_null_dictionary_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;
            List<KeyValuePair<int, string>> keyValuePairs = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "One"),
                new KeyValuePair<int, string>(1, "Two")
            };

            // Act
            Action act = () => dictionary.Should().NotContain(keyValuePairs,
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to not contain key/value pairs {[1, One], [1, Two]} because we want to test the behaviour with a null subject, but dictionary is <null>.");
        }

        [Fact]
        public void When_asserting_dictionary_does_not_contain_key_value_pairs_but_expected_key_value_pairs_are_empty_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            List<KeyValuePair<int, string>> keyValuePair = new List<KeyValuePair<int, string>>();

            // Act
            Action act = () => dictionary1.Should().NotContain(keyValuePair, "because we want to test the behaviour with an empty set of key/value pairs");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify key containment against an empty collection of key/value pairs*");
        }

        [Fact]
        public void When_asserting_dictionary_does_not_contain_key_value_pairs_but_expected_key_value_pairs_are_null_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };
            List<KeyValuePair<int, string>> keyValuePairs = null;

            // Act
            Action act = () => dictionary1.Should().NotContain(keyValuePairs, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare dictionary with <null>.*")
                .And.ParamName.Should().Be("items");
        }

        [Fact]
        public void When_dictionary_contains_expected_value_at_specific_key_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            dictionary.Should().Contain(1, "One");
        }

        [Fact]
        public void When_dictionary_contains_expected_null_at_specific_key_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = null
            };

            // Act / Assert
            dictionary.Should().Contain(1, null);
        }

        [Fact]
        public void When_dictionary_contains_expected_key_value_pairs_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            var items = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "One"),
                new KeyValuePair<int, string>(2, "Two")
            };
            dictionary.Should().Contain(items);
        }

        [Fact]
        public void When_dictionary_contains_expected_key_value_pair_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            var item = new KeyValuePair<int, string>(1, "One");
            dictionary.Should().Contain(item);
        }

        [Fact]
        public void When_dictionary_does_not_contain_the_expected_value_at_specific_key_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            var item = new KeyValuePair<int, string>(1, "Two");
            Action act = () => dictionary.Should().Contain(item, "we put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain value \"Two\" at key 1 because we put it there, but found \"One\".");
        }

        [Fact]
        public void When_dictionary_does_not_contain_the_key_value_pairs_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var items = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "Two"),
                new KeyValuePair<int, string>(2, "Three")
            };

            // Act
            Action act = () => dictionary.Should().Contain(items, "we put them {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain {[1, Two], [2, Three]} because we put them there, but dictionary differs at keys {1, 2}.");
        }

        [Fact]
        public void When_dictionary_does_not_contain_the_key_value_pair_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().Contain(1, "Two", "we put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain value \"Two\" at key 1 because we put it there, but found \"One\".");
        }

        [Fact]
        public void When_dictionary_does_not_contain_an_value_at_the_specific_key_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().Contain(3, "Two", "we put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain value \"Two\" at key 3 because we put it there, but the key was not found.");
        }

        [Fact]
        public void When_asserting_dictionary_contains_value_at_specific_key_against_null_dictionary_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().Contain(1, "One",
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain value \"One\" at key 1 because we want to test the behaviour with a null subject, but dictionary is <null>.");
        }

        [Fact]
        public void When_dictionary_does_not_contain_unexpected_value_or_key_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            dictionary.Should().NotContain(3, "Three");
        }

        [Fact]
        public void When_dictionary_does_not_contain_unexpected_value_at_existing_key_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            dictionary.Should().NotContain(2, "Three");
        }

        [Fact]
        public void When_dictionary_does_not_have_the_unexpected_value_but_null_at_existing_key_it_should_succeed()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = null
            };

            // Act
            Action action = () => dictionary.Should().NotContain(1, "other");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_dictionary_does_not_contain_unexpected_key_value_pairs_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            var items = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(3, "Three"),
                new KeyValuePair<int, string>(4, "Four")
            };
            dictionary.Should().NotContain(items);
        }

        [Fact]
        public void When_dictionary_does_not_contain_unexpected_key_value_pair_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            var item = new KeyValuePair<int, string>(3, "Three");
            dictionary.Should().NotContain(item);
        }

        [Fact]
        public void When_dictionary_contains_the_unexpected_value_at_specific_key_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            var item = new KeyValuePair<int, string>(1, "One");
            Action act = () => dictionary.Should().NotContain(item, "we put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary not to contain value \"One\" at key 1 because we put it there, but found it anyhow.");
        }

        [Fact]
        public void When_dictionary_contains_the_key_value_pairs_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            var items = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "One"),
                new KeyValuePair<int, string>(2, "Two")
            };
            Action act = () => dictionary.Should().NotContain(items, "we did not put them {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to not contain key/value pairs {[1, One], [2, Two]} because we did not put them there, but found them anyhow.");
        }

        [Fact]
        public void When_dictionary_contains_the_key_value_pair_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContain(1, "One", "we did not put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary not to contain value \"One\" at key 1 because we did not put it there, but found it anyhow.");
        }

        [Fact]
        public void When_asserting_dictionary_does_not_contain_value_at_specific_key_against_null_dictionary_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () => dictionary.Should().NotContain(1, "One",
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary not to contain value \"One\" at key 1 because we want to test the behaviour with a null subject, but dictionary is <null>.");
        }

        #endregion

        #region Miscellaneous

        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act / Assert
            dictionary.Should()
                .HaveCount(2)
                .And.ContainKey(1)
                .And.ContainValue("Two");
        }

        #endregion

        [Theory]
        [MemberData(nameof(DictionariesData))]
        public void When_comparing_dictionary_like_collections_for_equality_it_should_succeed<T1, T2>(T1 subject, T2 expected)
            where T1 : IEnumerable<KeyValuePair<int, int>>
            where T2 : IEnumerable<KeyValuePair<int, int>>
        {
            // Act
            Action act = () => subject.Should().Equal(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(DictionariesData))]
        public void When_comparing_dictionary_like_collections_for_inequality_it_should_throw<T1, T2>(T1 subject, T2 expected)
            where T1 : IEnumerable<KeyValuePair<int, int>>
            where T2 : IEnumerable<KeyValuePair<int, int>>
        {
            // Act
            Action act = () => subject.Should().NotEqual(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        public static IEnumerable<object[]> DictionariesData()
        {
            return from x in Dictionaries()
                   from y in Dictionaries()
                   select new[] { x, y };
        }

        [Theory]
        [MemberData(nameof(SingleDictionaryData))]
        public void When_a_dictionary_like_collection_contains_the_expected_key_it_should_succeed<T>(T subject)
            where T : IEnumerable<KeyValuePair<int, int>>
        {
            // Act
            Action act = () => subject.Should().ContainKey(1);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(SingleDictionaryData))]
        public void When_a_dictionary_like_collection_contains_the_expected_value_it_should_succeed<T>(T subject)
            where T : IEnumerable<KeyValuePair<int, int>>
        {
            // Act
            Action act = () => subject.Should().ContainValue(42);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(SingleDictionaryData))]
        public void When_using_a_dictionary_like_collection_it_should_preserve_reference_equality<T>(T subject)
            where T : IEnumerable<KeyValuePair<int, int>>
        {
            // Act
            Action act = () => subject.Should().BeSameAs(subject);

            // Assert
            act.Should().NotThrow();
        }

        public static IEnumerable<object[]> SingleDictionaryData() =>
            Dictionaries().Select(x => new[] { x });

        private static object[] Dictionaries()
        {
            return new object[]
            {
                new Dictionary<int, int>() { [1] = 42 },
                new TrueReadOnlyDictionary<int, int>(new Dictionary<int, int>() { [1] = 42 }),
                new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(1, 42) }
            };
        }

        /// <summary>
        /// This class only implements <see cref="IReadOnlyDictionary{TKey, TValue}"/>,
        /// as <see cref="ReadOnlyDictionary{TKey, TValue}"/> also implements <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        private class TrueReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        {
            private readonly IReadOnlyDictionary<TKey, TValue> dictionary;

            public TrueReadOnlyDictionary(IReadOnlyDictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
            }

            public TValue this[TKey key] => dictionary[key];

            public IEnumerable<TKey> Keys => dictionary.Keys;

            public IEnumerable<TValue> Values => dictionary.Values;

            public int Count => dictionary.Count;

            public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();

            public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);

            IEnumerator IEnumerable.GetEnumerator() => dictionary.GetEnumerator();
        }
    }

    internal class TrackingTestDictionary : IDictionary<int, string>
    {
        private readonly IDictionary<int, string> entries;

        public TrackingTestDictionary(params KeyValuePair<int, string>[] entries)
        {
            this.entries = entries.ToDictionary(e => e.Key, e => e.Value);
            Enumerator = new TrackingDictionaryEnumerator(entries);
        }

        public TrackingDictionaryEnumerator Enumerator { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
        {
            Enumerator.IncreaseEnumerationCount();
            Enumerator.Reset();
            return Enumerator;
        }

        public void Add(KeyValuePair<int, string> item)
        {
            entries.Add(item);
        }

        public void Clear()
        {
            entries.Clear();
        }

        public bool Contains(KeyValuePair<int, string> item)
        {
            return entries.Contains(item);
        }

        public void CopyTo(KeyValuePair<int, string>[] array, int arrayIndex)
        {
            entries.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<int, string> item)
        {
            return entries.Remove(item);
        }

        public int Count
        {
            get { return entries.Count; }
        }

        public bool IsReadOnly
        {
            get { return entries.IsReadOnly; }
        }

        public bool ContainsKey(int key)
        {
            return entries.ContainsKey(key);
        }

        public void Add(int key, string value)
        {
            entries.Add(key, value);
        }

        public bool Remove(int key)
        {
            return entries.Remove(key);
        }

        public bool TryGetValue(int key, out string value)
        {
            return entries.TryGetValue(key, out value);
        }

        public string this[int key]
        {
            get { return entries[key]; }
            set { entries[key] = value; }
        }

        public ICollection<int> Keys
        {
            get { return entries.Keys; }
        }

        public ICollection<string> Values
        {
            get { return entries.Values; }
        }
    }

    internal class TrackingDictionaryEnumerator : IEnumerator<KeyValuePair<int, string>>
    {
        private readonly KeyValuePair<int, string>[] values;
        private int loopCount;
        private int index;

        public TrackingDictionaryEnumerator(KeyValuePair<int, string>[] values)
        {
            index = -1;
            this.values = values;
        }

        public int LoopCount
        {
            get { return loopCount; }
        }

        public void IncreaseEnumerationCount()
        {
            loopCount++;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            index++;
            return index < values.Length;
        }

        public void Reset()
        {
            index = -1;
        }

        public KeyValuePair<int, string> Current
        {
            get { return values[index]; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }

    internal class DictionaryNotImplementingIReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        public TValue this[TKey key] { get => dictionary[key]; set => throw new NotImplementedException(); }

        public ICollection<TKey> Keys => dictionary.Keys;

        public ICollection<TValue> Values => dictionary.Values;

        public int Count => dictionary.Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).IsReadOnly;

        public void Add(TKey key, TValue value) => throw new NotImplementedException();

        public void Add(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public bool Contains(KeyValuePair<TKey, TValue> item) => dictionary.Contains(item);

        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new NotImplementedException();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();

        public bool Remove(TKey key) => throw new NotImplementedException();

        public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

        public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => dictionary.GetEnumerator();
    }
}
