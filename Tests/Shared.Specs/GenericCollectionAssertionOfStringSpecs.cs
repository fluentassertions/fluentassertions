using System;
using System.Collections;
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
        #region Be Null

        [Fact]
        public void When_collection_is_expected_to_be_null_and_it_is_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> someCollection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            someCollection.Should().BeNull();
        }

        [Fact]
        public void When_collection_is_expected_to_be_null_and_it_isnt_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> someCollection = new string[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someCollection.Should().BeNull("because {0} is valid", "null");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to be <null> because null is valid, but found {empty}.");
        }

        [Fact]
        public void When_collection_is_not_expected_to_be_null_and_it_isnt_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> someCollection = new string[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            someCollection.Should().NotBeNull();
        }

        [Fact]
        public void When_collection_is_not_expected_to_be_null_and_it_is_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> someCollection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someCollection.Should().NotBeNull("because {0} should not", "someCollection");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection not to be <null> because someCollection should not.");
        }

        #endregion

        #region Have Count

        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_that_equals_the_number_of_items()
        {
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            collection.Should().HaveCount(3);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_that_is_different_from_the_number_of_items()
        {
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            Action act = () => collection.Should().HaveCount(4);

            act.ShouldThrow<XunitException>();
        }

        [Fact]
        public void When_collection_has_a_count_that_is_different_from_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => collection.Should().HaveCount(4, "because we want to test the failure {0}", "message");
            
            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<XunitException>()
                .WithMessage("Expected collection to contain 4 item(s) because we want to test the failure message, but found 3.");
        }

        [Fact]
        public void When_collection_has_a_count_larger_than_the_minimum_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().HaveCount(c => c >= 3);
        }

        [Fact]
        public void When_collection_has_a_count_that_not_matches_the_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveCount(c => c >= 4, "a minimum of 4 is required");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to have a count (c >= 4) because a minimum of 4 is required, but count is 3.");
        }

        [Fact]
        public void When_collection_count_is_matched_against_a_null_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveCount(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot compare collection count against a <null> predicate.");
        }

        [Fact]
        public void When_collection_count_is_matched_and_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveCount(1, "we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to contain 1 item(s) because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_collection_count_is_matched_against_a_predicate_and_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection.Should().HaveCount(c => c < 3, "we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to contain (c < 3) items because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Be Empty

        [Fact]
        public void Should_succeed_when_asserting_collection_without_items_is_empty()
        {
            IEnumerable<string> collection = new string[0];
            collection.Should().BeEmpty();
        }

        [Fact]
        public void Should_fail_when_asserting_collection_with_items_is_empty()
        {
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            Action act = () => collection.Should().BeEmpty();

            act.ShouldThrow<XunitException>();
        }

        [Fact]
        public void When_the_collection_is_not_empty_unexpectedly_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeEmpty("because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<XunitException>()
                .WithMessage("Expected collection to be empty because we want to test the failure message, but found*one*two*three*");
        }

        [Fact]
        public void When_asserting_collection_with_items_is_not_empty_it_should_succeed()
        {
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            collection.Should().NotBeEmpty();
        }

        [Fact]
        public void When_asserting_collection_without_items_is_not_empty_it_should_fail()
        {
            IEnumerable<string> collection = new string[0];
            Action act = () => collection.Should().NotBeEmpty();
            act.ShouldThrow<XunitException>();
        }

        [Fact]
        public void When_asserting_collection_without_items_is_not_empty_it_should_fail_with_descriptive_message_()
        {
            IEnumerable<string> collection = new string[0];
            var assertions = collection.Should();
            assertions.Invoking(x => x.NotBeEmpty("because we want to test the failure {0}", "message"))
                .ShouldThrow<XunitException>()
                .WithMessage("Expected collection not to be empty because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_collection_to_be_empty_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeEmpty("because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Not Be Empty

        [Fact]
        public void When_asserting_collection_to_be_not_empty_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotBeEmpty("because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection not to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Be Equal

        [Fact]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_collection()
        {
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two", "three" };
            collection1.Should().Equal(collection2);
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_list_of_elements()
        {
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            collection.Should().Equal("one", "two", "three");
        }

        [Fact]
        public void When_both_collections_are_null_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> nullColl = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => nullColl.Should().Equal(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_two_collections_containing_nulls_are_equal_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new List<string> { "aaa", null };
            var expected = new List<string> { "aaa", null };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => subject.Should().Equal(expected);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [Fact]
        public void When_two_collections_are_not_equal_because_one_item_differs_it_should_throw_using_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two", "five" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to be equal to {\"one\", \"two\", \"five\"} because we want to test the failure message, but {\"one\", \"two\", \"three\"} differs at index 2.");
        }

        [Fact]
        public void When_two_collections_are_not_equal_because_the_actual_collection_contains_more_items_it_should_throw_using_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to be equal to {\"one\", \"two\"} because we want to test the failure message, but {\"one\", \"two\", \"three\"} contains 1 item(s) too many.");
        }

        [Fact]
        public void When_two_collections_are_not_equal_because_the_actual_collection_contains_less_items_it_should_throw_using_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two", "three", "four" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to be equal to {\"one\", \"two\", \"three\", \"four\"} because we want to test the failure message, but {\"one\", \"two\", \"three\"} contains 1 item(s) less.");
        }

        [Fact]
        public void When_asserting_collections_to_be_equal_but_subject_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Equal(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to be equal because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collections_to_be_equal_but_expected_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> collection1 = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Equal(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentNullException>().WithMessage(
                "Cannot compare collection with <null>.\r\nParameter name: expectation");
        }

        [Fact]
        public void When_an_empty_collection_is_compared_for_equality_to_a_non_empty_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection1 = new string[0];
            IEnumerable<string> collection2 = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().Equal(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to be equal to {\"one\", \"two\", \"three\"}, but found empty collection.");
        }

        [Fact] 
        public void When_all_items_match_according_to_a_predicate_it_should_succeed() 
        { 
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
           var actual = new List<string> { "ONE", "TWO", "THREE", "FOUR" }; 
           var expected = new List<string> { "One", "Two", "Three", "Four" }; 

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => actual.Should().Equal(expected,
                (a, e) => string.Equals(a, e, StringComparison.CurrentCultureIgnoreCase));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [Fact]
        public void When_any_item_does_not_match_according_to_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var actual = new List<string> { "ONE", "TWO", "THREE", "FOUR" };
            var expected = new List<string> { "One", "Two", "Three", "Five" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => actual.Should().Equal(expected,
                (a, e) => string.Equals(a, e, StringComparison.CurrentCultureIgnoreCase));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<XunitException>()
                .WithMessage("Expected*equal to*, but*differs at index 3.");
        } 

        #endregion

        #region Not Be Equal

        [Fact]
        public void Should_succeed_when_asserting_collection_is_not_equal_to_a_different_collection()
        {
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "three", "one", "two" };
            collection1.Should()
                .NotEqual(collection2);
        }

        [Fact]
        public void When_two_equal_collections_are_not_expected_to_be_equal_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotEqual(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Did not expect collections {\"one\", \"two\", \"three\"} and {\"one\", \"two\", \"three\"} to be equal.");
        }

        [Fact]
        public void When_two_equal_collections_are_not_expected_to_be_equal_it_should_report_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotEqual(collection2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Did not expect collections {\"one\", \"two\", \"three\"} and {\"one\", \"two\", \"three\"} to be equal because we want to test the failure message.");
        }


        [Fact]
        public void When_asserting_collections_not_to_be_equal_subject_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection.Should().NotEqual(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collections not to be equal because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equal_but_expected_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> collection1 = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection.Should().NotEqual(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentNullException>().WithMessage(
                "Cannot compare collection with <null>.\r\nParameter name: unexpected");
        }

        #endregion

        #region Be Equivalent To

        [Fact]
        public void When_two_collections_contain_the_same_elements_it_should_treat_them_as_equivalent()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "three", "two", "one" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection1.Should().BeEquivalentTo(collection2);
        }

        [Fact]
        public void When_a_collection_contains_same_elements_it_should_treat_it_as_equivalent()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().BeEquivalentTo("three", "two", "one");
        }

        [Fact]
        public void When_collections_are_not_equivalent_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "one", "two" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().BeEquivalentTo(collection2, "we treat {0} alike", "all");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "*collection {\"one\", \"two\", \"three\"} to be equivalent to {\"one\", \"two\"}*too many*");
        }
        
        [Fact]
        public void When_collections_with_duplicates_are_not_equivalent_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three", "one" };
            IEnumerable<string> collection2 = new[] { "one", "two", "three", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().BeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\", \"one\"} to be equivalent to {\"one\", \"two\", \"three\", \"three\"}, but it misses {\"three\"}.");
        }

        [Fact]
        public void When_testing_for_equivalence_against_empty_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> subject = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new string[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEquivalentTo(otherCollection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "*collection {\"one\", \"two\", \"three\"} to be equivalent to {empty}, but*");
        }
        
        [Fact]
        public void When_two_collections_are_both_empty_it_should_treat_them_as_equivalent()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> subject = new string[0];
            IEnumerable<string> otherCollection = new string[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeEquivalentTo(otherCollection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_testing_for_equivalence_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().BeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot verify equivalence against a <null> collection.");
        }

        [Fact]
        public void When_asserting_collections_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection.Should().BeEquivalentTo(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to be equivalent to {\"one\", \"two\", \"three\"} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_is_not_equivalent_to_a_different_collection()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "three", "one" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection1.Should().NotBeEquivalentTo(collection2);
        }

        [Fact]
        public void When_collections_are_unexpectingly_equivalent_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new[] { "three", "one", "two" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} not be equivalent with collection {\"three\", \"one\", \"two\"}.");
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    collection.Should().NotBeEquivalentTo(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection not to be equivalent because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_non_empty_collection_is_not_expected_to_be_equivalent_to_an_empty_collection_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = new string[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_testing_collections_not_to_be_equivalent_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };
            IEnumerable<string> collection2 = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot verify inequivalence against a <null> collection.");
        }

        #endregion

        #region Be Subset Of

        [Fact]
        public void When_collection_is_subset_of_a_specified_collection_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> subset = new [] { "one", "two" };
            IEnumerable<string> superset = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subset.Should().BeSubsetOf(superset);
        }

        [Fact]
        public void When_collection_is_not_a_subset_of_another_it_should_throw_with_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> subset = new [] { "one", "two", "three", "six" };
            IEnumerable<string> superset = new [] { "one", "two", "four", "five" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subset.Should().BeSubsetOf(superset, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to be a subset of {\"one\", \"two\", \"four\", \"five\"} because we want to test the failure message, " +
                    "but items {\"three\", \"six\"} are not part of the superset.");
        }

        [Fact]
        public void When_an_empty_collection_is_tested_against_a_superset_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> subset = new string[0];
            IEnumerable<string> superset = new [] { "one", "two", "four", "five" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subset.Should().BeSubsetOf(superset);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_a_subset_is_tested_against_a_null_superset_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> subset = new [] { "one", "two", "three" };
            IEnumerable<string> superset = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subset.Should().BeSubsetOf(superset);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot verify a subset against a <null> collection.");
        }

        [Fact]
        public void When_a_set_is_expected_to_be_not_a_subset_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> subject = new [] { "one", "two", "four" };
            IEnumerable<string> otherSet = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().NotBeSubsetOf(otherSet);
        }
        
        [Fact]
        public void When_an_empty_set_is_not_supposed_to_be_a_subset_of_another_set_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> subject = new string[] {};
            IEnumerable<string> otherSet = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBeSubsetOf(otherSet);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage("Did not expect collection {empty} to be a subset of {\"one\", \"two\", \"three\"}.");
        }

        [Fact]
        public void Should_fail_when_asserting_collection_is_not_subset_of_a_superset_collection()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> subject = new [] { "one", "two" };
            IEnumerable<string> otherSet = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBeSubsetOf(otherSet, "because I'm {0}", "mistaken");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Did not expect collection {\"one\", \"two\"} to be a subset of {\"one\", \"two\", \"three\"} because I'm mistaken.");
        }

        [Fact]
        public void When_asserting_collection_to_be_subset_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection.Should().BeSubsetOf(collection1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to be a subset of {\"one\", \"two\", \"three\"} because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Contain

        [Fact]
        public void When_asserting_collection_contains_an_item_from_the_collection_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain("one");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }
        
        [Fact]
        public void When_the_expected_object_exists_it_should_allow_chaining_additional_assertions()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain("one").Which.Should().HaveLength(4);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage("Expected*length*4*3*");
        }

        [Fact]
        public void When_asserting_collection_contains_multiple_items_from_the_collection_in_any_order_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(new [] { "two", "one" });

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow<XunitException>();
        }

        [Fact]
        public void When_a_collection_does_not_contain_single_item_it_should_throw_with_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain("four", "because {0}", "we do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to contain \"four\" because we do.");
        }

        [Fact]
        public void When_a_collection_does_not_contain_another_collection_it_should_throw_with_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(new [] { "three", "four", "five" }, "because {0}", "we do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to contain {\"three\", \"four\", \"five\"} because we do, but could not find {\"four\", \"five\"}.");
        }

        [Fact]
        public void When_the_contents_of_a_collection_are_checked_against_an_empty_collection_it_should_throw_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(new int[0]);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>().WithMessage(
                "Cannot verify containment against an empty collection");
        }

        #endregion

        #region Not Contain

        [Fact]
        public void When_collection_does_not_contain_an_item_that_is_not_in_the_collection_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotContain("four");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow<XunitException>();
        }

        [Fact]
        public void When_collection_contains_an_unexpected_item_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotContain("one", "because we {0} like it, but found it anyhow", "don't");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to not contain \"one\" because we don't like it, but found it anyhow.");
        }

        [Fact]
        public void When_collection_does_contain_an_unexpected_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotContain(item => item == "two", "because {0}s are evil", "two");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to not have any items matching (item == \"two\") because twos are evil.");
        }

        [Fact]
        public void When_collection_does_not_contain_an_unexpected_item_matching_a_predicate_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().NotContain(item => item == "four");
        }

        [Fact]
        public void When_asserting_collection_does_not_contain_item_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should()
                .NotContain("one", "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to not contain \"one\" because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Contain In Order

        [Fact]
        public void When_two_collections_contain_the_same_items_in_the_same_order_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().ContainInOrder("one", "two", "three");
        }

        [Fact]
        public void When_collection_contains_null_value_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new string[] { "one", null, "two", "string" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().ContainInOrder(new string[] { "one", null, "string" });
        }

        [Fact]
        public void When_the_first_collection_contains_a_duplicate_item_without_affecting_the_order_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three", "two" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().ContainInOrder("one", "two", "three");
        }

        [Fact]
        public void When_two_collections_contain_the_same_duplicate_items_in_the_same_order_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "one", "two", "twelve", "two", "two" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().ContainInOrder("one", "two", "one", "two", "twelve", "two", "two" );
        }

        [Fact]
        public void When_a_collection_does_not_contain_a_range_twice_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "one", "three", "twelve", "two", "two" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainInOrder( "one", "two", "one", "one", "two");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"one\", \"three\", \"twelve\", \"two\", \"two\"} to contain items {\"one\", \"two\", \"one\", \"one\", \"two\"} in order, but \"one\" (index 3) did not appear (in the right order).");
        }

        [Fact]
        public void When_two_collections_contain_the_same_items_but_in_different_order_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new [] { "one", "two", "three" }.Should().ContainInOrder(new [] { "three", "one" }, "because we said so");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to contain items {\"three\", \"one\"} in order because we said so, but \"one\" (index 1) did not appear (in the right order).");
        }

        [Fact]
        public void When_a_collection_does_not_contain_an_ordered_item_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new [] { "one", "two", "three" }.Should().ContainInOrder(new [] { "four", "one" }, "we failed");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection {\"one\", \"two\", \"three\"} to contain items {\"four\", \"one\"} in order because we failed, " +
                    "but \"four\" (index 0) did not appear (in the right order).");
        }

        [Fact]
        public void When_passing_in_null_while_checking_for_ordered_containment_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new [] { "one", "two", "three" }.Should().ContainInOrder(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot verify ordered containment against a <null> collection.");
        }

        [Fact]
        public void When_asserting_collection_contains_some_values_in_order_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> strings = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => strings.Should().ContainInOrder(new[] { "string4" }, "because we're checking how it reacts to a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to contain {\"string4\"} in order because we're checking how it reacts to a null subject, but found <null>.");
        }

        #endregion

        #region (Not) Intersect

        [Fact]
        public void When_asserting_the_items_in_an_two_intersecting_collections_intersect_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------      
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new[] { "three", "four", "five" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().IntersectWith(otherCollection);
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_non_intersecting_collections_intersect_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------      
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new[] { "four", "five" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => collection.Should().IntersectWith(otherCollection, "they should share items");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<XunitException>()
                .WithMessage("Expected collection to intersect with {\"four\", \"five\"} because they should share items," +
                    " but {\"one\", \"two\", \"three\"} does not contain any shared items.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_non_intersecting_collections_do_not_intersect_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------      
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new[] { "four", "five" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().NotIntersectWith(otherCollection);
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_intersecting_collections_do_not_intersect_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------      
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = new[] { "two", "three", "four" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => collection.Should().NotIntersectWith(otherCollection, "they should not share items");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<XunitException>()
                .WithMessage("Did not expect collection to intersect with {\"two\", \"three\", \"four\"} because they should not share items," +
                    " but found the following shared items {\"two\", \"three\"}.");
        }

        #endregion

        #region Not Contain Nulls

        [Fact]
        public void When_collection_does_not_contain_nulls_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().NotContainNulls();
        }

        [Fact]
        public void When_collection_contains_nulls_that_are_unexpected_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "", null };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotContainNulls("because they are {0}", "evil");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection not to contain nulls because they are evil, but found one at index 1.");
        }

        [Fact]
        public void When_asserting_collection_to_not_contain_nulls_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotContainNulls("because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection not to contain nulls because we want to test the behaviour with a null subject, but collection is <null>.");
        }

        #endregion

        #region Only Have Unique Items

        [Fact]
        public void Should_succeed_when_asserting_collection_with_unique_items_contains_only_unique_items()
        {
            IEnumerable<string> collection = new [] { "one", "two", "three", "four" };
            collection.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void When_a_collection_contains_duplicate_items_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().OnlyHaveUniqueItems("{0} don't like {1}", "we", "duplicates");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to only have unique items because we don't like duplicates, but item \"three\" is not unique.");
        }

        [Fact]
        public void When_asserting_collection_to_only_have_unique_items_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => collection.Should().OnlyHaveUniqueItems("because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to only have unique items because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Have Element At

        [Fact]
        public void When_collection_has_expected_element_at_specific_index_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().HaveElementAt(1, "two");
        }

        [Fact]
        public void When_collection_does_not_have_the_expected_element_at_specific_index_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveElementAt(1, "three", "we put it {0}", "there");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected \"three\" at index 1 because we put it there, but found \"two\".");
        }

        [Fact]
        public void When_collection_does_not_have_an_element_at_the_specific_index_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveElementAt(4, "three", "we put it {0}", "there");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------            
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected \"three\" at index 4 because we put it there, but found no element.");
        }

        [Fact]
        public void When_asserting_collection_has_element_at_specific_index_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveElementAt(1, "one",
                "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to have element at index 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Miscellaneous

        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            IEnumerable<string> collection = new [] { "one", "two", "three" };
            collection.Should()
                .HaveCount(3)
                .And
                .HaveElementAt(1, "two")
                .And.NotContain("four");
        }

        #endregion

        #region Have Same Count

        [Fact]
        public void When_both_collections_have_the_same_number_elements_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<string> firstCollection = new [] { "one", "two", "three" };
            IEnumerable<string> secondCollection = new [] { "four", "five", "six" };

            var extensions = firstCollection.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            extensions.HaveSameCount(secondCollection);
        }

        [Fact]
        public void When_both_collections_do_not_have_the_same_number_of_elements_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<string> firstCollection = new [] { "one", "two", "three" };
            IEnumerable<string> secondCollection = new [] { "four", "six" };

            var extensions = firstCollection.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            extensions
                .Invoking(e => e.HaveSameCount(secondCollection))
                .ShouldThrow<XunitException>()
                .WithMessage("Expected collection to have 2 item(s), but found 3.");
        }

        [Fact]
        public void When_comparing_item_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<string> firstCollection = new [] { "one", "two", "three" };
            IEnumerable<string> secondCollection = new [] { "four", "six" };

            var extensions = firstCollection.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            extensions
                .Invoking(e => e.HaveSameCount(secondCollection, "we want to test the {0}", "reason"))
                .ShouldThrow<XunitException>()
                .WithMessage("Expected collection to have 2 item(s) because we want to test the reason, but found 3.");
        }

        [Fact]
        public void When_asserting_collections_to_have_same_count_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new [] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveSameCount(collection1,
                "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to have the same count as {\"one\", \"two\", \"three\"} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collections_to_have_same_count_against_an_other_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new [] { "one", "two", "three" };
            IEnumerable<string> otherCollection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveSameCount(otherCollection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot verify count against a <null> collection.");
        }

        #endregion

        #region Not Have Same Count

        [Fact]
        public void When_asserting_not_same_count_and_collections_have_different_number_elements_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<string> firstCollection = new[] { "one", "two", "three" };
            IEnumerable<string> secondCollection = new[] { "four", "six" };

            var extensions = firstCollection.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            extensions.NotHaveSameCount(secondCollection);
        }

        [Fact]
        public void When_asserting_not_same_count_and_both_collections_have_the_same_number_elements_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<string> firstCollection = new[] { "one", "two", "three" };
            IEnumerable<string> secondCollection = new[] { "four", "five", "six" };

            var extensions = firstCollection.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            extensions
                .Invoking(e => e.NotHaveSameCount(secondCollection))
                .ShouldThrow<XunitException>()
                .WithMessage("Expected collection to not have 3 item(s), but found 3.");
        }

        [Fact]
        public void When_comparing_not_same_item_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<string> firstCollection = new[] { "one", "two", "three" };
            IEnumerable<string> secondCollection = new[] { "four", "five", "six" };

            var extensions = firstCollection.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            extensions
                .Invoking(e => e.NotHaveSameCount(secondCollection, "we want to test the {0}", "reason"))
                .ShouldThrow<XunitException>()
                .WithMessage("Expected collection to not have 3 item(s) because we want to test the reason, but found 3.");
        }

        [Fact]
        public void When_asserting_collections_to_not_have_same_count_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = null;
            IEnumerable<string> collection1 = new[] { "one", "two", "three" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotHaveSameCount(collection1,
                "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected collection to not have the same count as {\"one\", \"two\", \"three\"} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collections_to_not_have_same_count_against_an_other_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> collection = new[] { "one", "two", "three" };
            IEnumerable<string> otherCollection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotHaveSameCount(otherCollection);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot verify count against a <null> collection.");
        }

        #endregion

        [Fact]
        public void
            When_using_StringCollectionAssertions_the_AndConstraint_should_have_the_correct_type()
        {

            var methodInfo =
                typeof(StringCollectionAssertions).GetMethods(
                    BindingFlags.Public | BindingFlags.Instance);


            var methods =
                from method in methodInfo
                where !method.IsSpecialName // Exclude Properties
                where method.DeclaringType != typeof (object)
                select new {method.Name, method.ReturnType};


            methods.Should()
                .OnlyContain(
                    method =>
                        typeof(AndConstraint<StringCollectionAssertions>)
                            .GetTypeInfo()
                            .IsAssignableFrom(method.ReturnType.GetTypeInfo()));

        }

        [Fact]
        public void When_asserting_a_string_collection_contains_an_element_it_should_allow_specifying_the_reason_via_named_parameter()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var expected = new List<string> {"hello", "world"};
            var actual = new List<string> { "hello", "world" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => expected.Should().Contain(actual, because: "they are in the collection");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }
    }
}
