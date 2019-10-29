using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions.Collections;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class CollectionAssertionSpecs
    {
        [Fact]
        public void Should_have_parameterless_constructor()
        {
            // Arrange
            Type type = typeof(CollectionAssertions<,>);

            // Act / Assert
            type.Should().HaveDefaultConstructor();
        }

        #region Be Null

        [Fact]
        public void When_collection_is_expected_to_be_null_and_it_is_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> someCollection = null;

            // Act / Assert
            someCollection.Should().BeNull();
        }

        [Fact]
        public void When_a_custom_enumerable_implementation_is_expected_not_to_be_null_and_it_is_it_should_not_throw()
        {
            // Arrange
            var enumerable = new CustomEnumerable();

            // Act / Assert
            enumerable.Should().NotBeNull();
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
#if NETCOREAPP1_1
                "Expected collection to be <null> because null is valid, but found {empty}.");
#else
                "Expected someCollection to be <null> because null is valid, but found {empty}.");
#endif
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
        public void When_collection_is_not_expected_to_be_null_and_it_is_it_should_throw()
        {
            // Arrange
            IEnumerable<string> someCollection = null;

            // Act
            Action act = () => someCollection.Should().NotBeNull("because {0} should not", "someCollection");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection not to be <null> because someCollection should not.");
#else
                "Expected someCollection not to be <null> because someCollection should not.");
#endif
        }

        #endregion

        #region Have Count

        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_that_equals_the_number_of_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCount(3);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_that_is_different_from_the_number_of_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCount(4);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_has_a_count_that_is_different_from_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().HaveCount(4, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to contain 4 item(s) because we want to test the failure message, but found 3.");
        }

        [Fact]
        public void When_collection_has_a_count_larger_than_the_minimum_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCount(c => c >= 3);
        }

        [Fact]
        public void When_collection_has_a_count_that_not_matches_the_predicate_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCount(c => c >= 4, "a minimum of 4 is required");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to have a count (c >= 4) because a minimum of 4 is required, but count is 3.");
        }

        [Fact]
        public void When_collection_count_is_matched_against_a_null_predicate_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCount(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot compare collection count against a <null> predicate.*");
        }

        [Fact]
        public void When_collection_count_is_matched_and_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should().HaveCount(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain 1 item(s) because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_collection_count_is_matched_against_a_predicate_and_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act =
                () => collection.Should().HaveCount(c => c < 3, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain (c < 3) items because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_counting_nongeneric_enumerable_it_should_enumerate()
        {
            // Arrange
            var collection = new CountingNonGenericEnumerable(new[] { "1", "2", "3" });

            // Act
            collection.Should().HaveCount(3);

            // Assert
            collection.GetEnumeratorCallCount.Should().Be(1);
        }

        [Fact]
        public void When_counting_nongeneric_collection_it_should_not_enumerate()
        {
            // Arrange
            var collection = new CountingNonGenericCollection(new[] { 1, 2, 3 });

            // Act
            collection.Should().HaveCount(3);

            // Assert
            collection.GetCountCallCount.Should().Be(1);
            collection.GetEnumeratorCallCount.Should().Be(0);
        }

        [Fact]
        public void When_counting_generic_enumerable_it_should_enumerate()
        {
            // Arrange
            var enumerable = new CountingGenericEnumerable<int>(new[] { 1, 2, 3 });

            // Act
            enumerable.Should().HaveCount(3);

            // Assert
            enumerable.GetEnumeratorCallCount.Should().Be(1);
        }

        [Fact]
        public void When_counting_generic_collection_it_should_not_enumerate()
        {
            // Arrange
            var collection = new CountingGenericCollection<int>(new[] { 1, 2, 3 });

            // Act
            collection.Should().HaveCount(3);

            // Assert
            collection.GetCountCallCount.Should().Be(1);
            collection.GetEnumeratorCallCount.Should().Be(0);
        }

        #endregion

        #region Not Have Count

        [Fact]
        public void Should_succeed_when_asserting_collection_ofT_has_a_count_different_from_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().NotHaveCount(2);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_ofT_has_a_count_that_equals_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotHaveCount(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_ofT_has_a_count_that_equals_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().NotHaveCount(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*not contain*3*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_collection_ofT_count_is_same_than_and_collection_ofT_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().NotHaveCount(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*not contain*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Count Greater Than

        [Fact]
        public void Should_succeed_when_asserting_collection_ofT_has_a_count_greater_than_less_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCountGreaterThan(2);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_ofT_has_a_count_greater_than_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCountGreaterThan(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_ofT_has_a_count_greater_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().HaveCountGreaterThan(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*more than*3*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_collection_ofT_count_is_greater_than_and_collection_ofT_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().HaveCountGreaterThan(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*more than*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Count Greater Or Equal To

        [Fact]
        public void Should_succeed_when_asserting_collection_ofT_has_a_count_greater_or_equal_to_less_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCountGreaterOrEqualTo(3);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_ofT_has_a_count_greater_or_equal_to_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCountGreaterOrEqualTo(4);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_ofT_has_a_count_greater_or_equal_to_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().HaveCountGreaterOrEqualTo(4, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*at least*4*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_collection_ofT_count_is_greater_or_equal_to_and_collection_ofT_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().HaveCountGreaterOrEqualTo(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*at least*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Count Less Than

        [Fact]
        public void Should_succeed_when_asserting_collection_ofT_has_a_count_less_than_less_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCountLessThan(4);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_ofT_has_a_count_less_than_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCountLessThan(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_ofT_has_a_count_less_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().HaveCountLessThan(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*fewer than*3*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_collection_ofT_count_is_less_than_and_collection_ofT_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().HaveCountLessThan(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*fewer than*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Count Less Or Equal To

        [Fact]
        public void Should_succeed_when_asserting_collection_ofT_has_a_count_less_or_equal_to_less_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCountLessOrEqualTo(3);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_ofT_has_a_count_less_or_equal_to_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCountLessOrEqualTo(2);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_ofT_has_a_count_less_or_equal_to_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().HaveCountLessOrEqualTo(2, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*at most*2*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_collection_ofT_count_is_less_or_equal_to_and_collection_ofT_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            Action act = () => collection.Should().HaveCountLessOrEqualTo(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*at most*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Not Have Count

        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_different_from_the_number_of_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().NotHaveCount(2);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_that_equals_the_number_of_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotHaveCount(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_has_a_count_that_equals_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().NotHaveCount(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*not contain*3*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_collection_count_is_same_than_and_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should().NotHaveCount(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*not contain*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Count Greater Than

        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_greater_than_less_the_number_of_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCountGreaterThan(2);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_greater_than_the_number_of_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCountGreaterThan(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_has_a_count_greater_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().HaveCountGreaterThan(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*more than*3*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_collection_count_is_greater_than_and_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should().HaveCountGreaterThan(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*more than*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Count Greater Or Equal To

        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_greater_or_equal_to_less_the_number_of_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCountGreaterOrEqualTo(3);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_greater_or_equal_to_the_number_of_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCountGreaterOrEqualTo(4);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_has_a_count_greater_or_equal_to_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().HaveCountGreaterOrEqualTo(4, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*at least*4*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_collection_count_is_greater_or_equal_to_and_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should().HaveCountGreaterOrEqualTo(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*at least*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Count Less Than

        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_less_than_less_the_number_of_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCountLessThan(4);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_less_than_the_number_of_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCountLessThan(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_has_a_count_less_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().HaveCountLessThan(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*fewer than*3*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_collection_count_is_less_than_and_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should().HaveCountLessThan(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*fewer than*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Have Count Less Or Equal To

        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_less_or_equal_to_less_the_number_of_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCountLessOrEqualTo(3);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_less_or_equal_to_the_number_of_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCountLessOrEqualTo(2);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_has_a_count_less_or_equal_to_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().HaveCountLessOrEqualTo(2, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*at most*2*because we want to test the failure message*3*");
        }

        [Fact]
        public void When_collection_count_is_less_or_equal_to_and_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should().HaveCountLessOrEqualTo(1, "we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*at most*1*we want to test the behaviour with a null subject*found <null>*");
        }

        #endregion

        #region Be Empty

        [Fact]
        public void When_collection_is_empty_as_expected_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new int[0];

            // Act / Assert
            collection.Should().BeEmpty();
        }

        [Fact]
        public void When_collection_is_not_empty_unexpectedly_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().BeEmpty("that's what we expect");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to be empty because that's what we expect, but found*1*2*3*");
        }

        [Fact]
        public void When_asserting_collection_with_items_is_not_empty_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().NotBeEmpty();
        }

        [Fact]
        public void When_asserting_collection_with_items_is_not_empty_it_should_enumerate_the_collection_only_once()
        {
            // Arrange
            var trackingEnumerable = new TrackingTestEnumerable(1, 2, 3);

            // Act
            trackingEnumerable.Should().NotBeEmpty();

            // Assert
            trackingEnumerable.Enumerator.LoopCount.Should().Be(1);
        }

        [Fact]
        public void When_asserting_collection_without_items_is_not_empty_it_should_fail()
        {
            // Arrange
            IEnumerable collection = new int[0];

            // Act
            Action act = () => collection.Should().NotBeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_collection_without_items_is_not_empty_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            IEnumerable collection = new int[0];

            // Act
            Action act = () => collection.Should().NotBeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection not to be empty because we want to test the failure message.");
        }

        [Fact]
        public void When_asserting_collection_to_be_empty_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should().BeEmpty("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_to_be_empty_it_should_enumerate_only_once()
        {
            // Arrange
            var collection = new CountingGenericEnumerable<int>(new int[0]);

            // Act
            collection.Should().BeEmpty();

            // Assert
            collection.GetEnumeratorCallCount.Should().Be(1);
        }

        #endregion

        #region Not Be Empty

        [Fact]
        public void When_asserting_collection_to_be_not_empty_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should().NotBeEmpty("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_to_be_not_empty_it_should_enumerate_only_once()
        {
            // Arrange
            var collection = new CountingGenericEnumerable<int>(new[] { 42 });

            // Act
            collection.Should().NotBeEmpty();

            // Assert
            collection.GetEnumeratorCallCount.Should().Be(1);
        }

        #endregion

        #region Be Null Or Empty

        [Fact]
        public void
            When_asserting_a_null_collection_to_be_null_or_empty_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = null;

            // Act / Assert
            collection.Should().BeNullOrEmpty();
        }

        [Fact]
        public void
            When_asserting_an_empty_collection_to_be_null_or_empty_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new int[0];

            // Act / Assert
            collection.Should().BeNullOrEmpty();
        }

        [Fact]
        public void
            When_asserting_non_empty_collection_to_be_null_or_empty_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().BeNullOrEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to be null or empty because we want to test the failure message, but found {1, 2, 3}.");
        }

        [Fact]
        public void When_asserting_collection_to_be_null_or_empty_it_should_enumerate_only_once()
        {
            // Arrange
            var collection = new CountingGenericEnumerable<int>(new int[0]);

            // Act
            collection.Should().BeNullOrEmpty();

            // Assert
            collection.GetEnumeratorCallCount.Should().Be(1);
        }

        #endregion

        #region Not Be Null Or Empty

        [Fact]
        public void
            When_asserting_non_empty_collection_to_not_be_null_or_empty_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { new object() };

            // Act / Assert
            collection.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void
            When_asserting_null_collection_to_not_be_null_or_empty_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should().NotBeNullOrEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_empty_collection_to_not_be_null_or_empty_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new int[0];

            // Act
            Action act = () => collection.Should().NotBeNullOrEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_collection_to_not_be_null_or_empty_it_should_enumerate_only_once()
        {
            // Arrange
            var collection = new CountingGenericEnumerable<int>(new[] { 42 });

            // Act
            collection.Should().NotBeNullOrEmpty();

            // Assert
            collection.GetEnumeratorCallCount.Should().Be(1);
        }

        #endregion

        #region Be Equal

        [Fact]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_collection()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 3 };

            // Act / Assert
            collection1.Should().Equal(collection2);
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_list_of_elements()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().Equal(1, 2, 3);
        }

        [Fact]
        public void When_both_collections_are_null_it_should_succeed()
        {
            // Arrange
            IEnumerable nullColl = null;

            // Act
            Action act = () => nullColl.Should().Equal(null);

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
            Action action = () => subject.Should().Equal(expected);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_two_collections_are_not_equal_because_one_item_differs_it_should_throw_using_the_reason()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 5 };

            // Act
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to be equal to {1, 2, 5} because we want to test the failure message, but {1, 2, 3} differs at index 2.");
#else
                "Expected collection1 to be equal to {1, 2, 5} because we want to test the failure message, but {1, 2, 3} differs at index 2.");
#endif
        }

        [Fact]
        public void When_two_collections_are_not_equal_because_the_actual_collection_contains_more_items_it_should_throw_using_the_reason()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2 };

            // Act
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to be equal to {1, 2} because we want to test the failure message, but {1, 2, 3} contains 1 item(s) too many.");
#else
                "Expected collection1 to be equal to {1, 2} because we want to test the failure message, but {1, 2, 3} contains 1 item(s) too many.");
#endif
        }

        [Fact]
        public void When_two_collections_are_not_equal_because_the_actual_collection_contains_less_items_it_should_throw_using_the_reason()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 3, 4 };

            // Act
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to be equal to {1, 2, 3, 4} because we want to test the failure message, but {1, 2, 3} contains 1 item(s) less.");
#else
                "Expected collection1 to be equal to {1, 2, 3, 4} because we want to test the failure message, but {1, 2, 3} contains 1 item(s) less.");
#endif
        }

        [Fact]
        public void When_two_multidimensional_collections_are_not_equal_and_it_should_format_the_collections_properly()
        {
            // Arrange
            IEnumerable collection1 = new[] { new[] { 1, 2 }, new[] { 3, 4 } };
            IEnumerable collection2 = new[] { new[] { 5, 6 }, new[] { 7, 8 } };

            // Act
            Action act = () => collection1.Should().Equal(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to be equal to {{5, 6}, {7, 8}}, but {{1, 2}, {3, 4}} differs at index 0.");
#else
                "Expected collection1 to be equal to {{5, 6}, {7, 8}}, but {{1, 2}, {3, 4}} differs at index 0.");
#endif
        }

        [Fact]
        public void When_asserting_collections_to_be_equal_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;
            IEnumerable collection1 = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Equal(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to be equal to {1, 2, 3} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collections_to_be_equal_but_expected_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable collection1 = null;

            // Act
            Action act = () => collection.Should().Equal(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare collection with <null>.*")
                .And.ParamName.Should().Be("expectation");
        }

        [Fact]
        public void When_an_empty_collection_is_compared_for_equality_to_a_non_empty_collection_it_should_throw()
        {
            // Arrange
            var collection1 = new int[0];
            var collection2 = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection1.Should().Equal(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to be equal to {1, 2, 3}, but found empty collection.");
#else
                "Expected collection1 to be equal to {1, 2, 3}, but found empty collection.");
#endif
        }

        [Fact]
        public void When_a_non_empty_collection_is_compared_for_equality_to_an_empty_collection_it_should_throw()
        {
            // Arrange
            var collection1 = new[] { 1, 2, 3 };
            var collection2 = new int[0];

            // Act
            Action act = () => collection1.Should().Equal(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to be equal to {empty}, but found {1, 2, 3}.");
#else
                "Expected collection1 to be equal to {empty}, but found {1, 2, 3}.");
#endif
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
            Action action = () => actual.Should().Equal(expected,
                (a, e) => string.Equals(a, e.Value, StringComparison.CurrentCultureIgnoreCase));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_any_item_does_not_match_according_to_a_predicate_it_should_throw()
        {
            // Arrange
            var actual = new List<string> { "ONE", "TWO", "THREE", "FOUR" };
            var expected = new[]
            {
               new { Value = "One" },
               new { Value = "Two" },
               new { Value = "Three" },
               new { Value = "Five" }
           };

            // Act
            Action action = () => actual.Should().Equal(expected,
                (a, e) => string.Equals(a, e.Value, StringComparison.CurrentCultureIgnoreCase));

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("*Expected*equal to*, but*differs at index 3.*");
        }

        [Fact]
        public void When_both_collections_are_empty_it_should_them_as_equal()
        {
            // Arrange
            var actual = new List<string>();
            var expected = new List<string>();

            // Act
            Action act = () => actual.Should().Equal(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_identical_collections_to_be_equal_it_should_enumerate_the_subject_only_once()
        {
            // Arrange
            var actual = new CountingGenericCollection<int>(new[] { 1, 2, 3 });
            var expected = new[] { 1, 2, 3 };

            // Act
            actual.Should().Equal(expected);

            // Assert
            actual.GetEnumeratorCallCount.Should().Be(1);
        }

        [Fact]
        public void When_asserting_identical_collections_to_not_be_equal_it_should_enumerate_the_subject_only_once()
        {
            // Arrange
            var actual = new CountingGenericCollection<int>(new[] { 1, 2, 3 });
            var expected = new[] { 1, 2, 3 };

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
        public void When_asserting_different_collections_to_be_equal_it_should_enumerate_the_subject_at_most_twice()
        {
            // Arrange
            var actual = new CountingGenericCollection<int>(new[] { 1, 2, 3 });
            var expected = new[] { 1, 2, 4 };

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
            actual.GetEnumeratorCallCount.Should().BeLessOrEqualTo(2);
        }

        [Fact]
        public void When_asserting_different_collections_to_not_be_equal_it_should_enumerate_the_subject_only_once()
        {
            // Arrange
            var actual = new CountingGenericCollection<int>(new[] { 1, 2, 3 });
            var expected = new[] { 1, 2, 4 };

            // Act
            actual.Should().NotEqual(expected);

            // Assert
            actual.GetEnumeratorCallCount.Should().Be(1);
        }

        #endregion

        #region Not Be Equal

        [Fact]
        public void Should_succeed_when_asserting_collection_is_not_equal_to_a_different_collection()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1, 2 };

            // Act / Assert
            collection1.Should()
                .NotEqual(collection2);
        }

        [Fact]
        public void When_two_equal_collections_are_not_expected_to_be_equal_it_should_throw()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 3 };

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
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 3 };

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
            IEnumerable collection = null;
            IEnumerable collection1 = new[] { 1, 2, 3 };

            // Act
            Action act =
                () => collection.Should().NotEqual(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collections not to be equal because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equal_but_expected_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable collection1 = null;

            // Act
            Action act =
                () => collection.Should().NotEqual(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare collection with <null>.*")
                .And.ParamName.Should().Be("unexpected");
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equal_but_both_collections_reference_the_same_object_it_should_throw()
        {
            IEnumerable collection1 = new[] { "one", "two", "three" };
            IEnumerable collection2 = collection1;

            // Act
            Action act = () => collection1.Should().NotEqual(collection2, "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collections not to be equal because we want to test the behaviour with same objects, but they both reference the same object.");
        }

        #endregion

        #region Be Equivalent To

        [Fact]
        public void When_two_collections_contain_the_same_elements_it_should_treat_them_as_equivalent()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1, 2 };

            // Act / Assert
            collection1.Should().BeEquivalentTo(collection2);
        }

        [Fact]
        public void When_a_collection_contains_same_elements_it_should_treat_it_as_equivalent()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().BeEquivalentTo(3, 1, 2);
        }

        [Fact]
        public void When_character_collections_are_equivalent_it_should_not_throw()
        {
            // Arrange
            char[] list1 = ("abc123ab").ToCharArray();
            char[] list2 = ("abc123ab").ToCharArray();

            // Act / Assert
            list1.Should().BeEquivalentTo(list2);
        }

        [Fact]
        public void When_collections_are_not_equivalent_it_should_throw()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2 };

            // Act
            Action act = () => collection1.Should().BeEquivalentTo(collection2, "we treat {0} alike", "all");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*collection*2 item(s)*we treat all alike, but *1 item(s) more than*");
        }

        [Fact]
        public void When_collections_with_duplicates_are_not_equivalent_it_should_throw()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3, 1 };
            IEnumerable collection2 = new[] { 1, 2, 3, 3 };

            // Act
            Action act = () => collection1.Should().BeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected item[3] to be 3, but found 1*");
        }

        [Fact]
        public void When_testing_for_equivalence_against_empty_collection_it_should_throw()
        {
            // Arrange
            IEnumerable subject = new[] { 1, 2, 3 };
            IEnumerable otherCollection = new int[0];

            // Act
            Action act = () => subject.Should().BeEquivalentTo(otherCollection);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject to be a collection with 0 item(s), but*contains 3 item(s)*");
        }

        [Fact]
        public void When_two_collections_are_both_empty_it_should_treat_them_as_equivalent()
        {
            // Arrange
            IEnumerable subject = new int[0];
            IEnumerable otherCollection = new int[0];

            // Act
            Action act = () => subject.Should().BeEquivalentTo(otherCollection);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_testing_for_equivalence_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = null;

            // Act
            Action act = () => collection1.Should().BeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*<null>*but found {1, 2, 3}*");
        }

        [Fact]
        public void When_asserting_collections_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;
            IEnumerable collection1 = new[] { 1, 2, 3 };

            // Act
            Action act =
                () => collection.Should().BeEquivalentTo(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected a collection, but subject is <null>*");
#else
                "Expected a collection, but collection is <null>*");
#endif
        }

        [Fact]
        public void When_collection_is_not_equivalent_to_another_smaller_collection_it_should_succeed()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1 };

            // Act / Assert
            collection1.Should().NotBeEquivalentTo(collection2);
        }

        [Fact]
        public void When_large_collection_is_equivalent_to_another_equally_size_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection1 = Enumerable.Repeat(1, 10000);
            IEnumerable collection2 = Enumerable.Repeat(1, 10000);

            // Act
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_is_not_equivalent_to_another_equally_sized_collection_it_should_succeed()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1, 4 };

            // Act / Assert
            collection1.Should().NotBeEquivalentTo(collection2);
        }

        [Fact]
        public void When_collections_are_unexpectedly_equivalent_it_should_throw()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1, 2 };

            // Act
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection {1, 2, 3} not*equivalent*{3, 1, 2}.");
#else
                "Expected collection1 {1, 2, 3} not*equivalent*{3, 1, 2}.");
#endif
        }

        [Fact]
        public void When_asserting_collections_not_to_be_equivalent_but_subject_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable actual = null;
            IEnumerable expectation = new[] { 1, 2, 3 };

            // Act
            Action act = () =>
                    actual.Should().NotBeEquivalentTo(expectation, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*be equivalent because we want to test the behaviour with a null subject, but found <null>*");
        }

        [Fact]
        public void When_non_empty_collection_is_not_expected_to_be_equivalent_to_an_empty_collection_it_should_succeed()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new int[0];

            // Act
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_testing_collections_not_to_be_equivalent_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = null;

            // Act
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot verify inequivalence against a <null> collection.*")
                .And.ParamName.Should().Be("unexpected");
        }

        [Fact]
        public void When_testing_collections_not_to_be_equivalent_against_same_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable collection1 = collection;

            // Act
            Action act = () => collection.Should().NotBeEquivalentTo(collection1,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*not to be equivalent*because we want to test the behaviour with same objects*but they both reference the same object.");
        }

        #endregion

        #region Contain Equivalent Of

        [Fact]
        public void When_collection_contains_object_equal_of_another_it_should_succeed()
        {
            // Arrange
            var item = new Customer { Name = "John" };
            IEnumerable collection = new[] { new Customer { Name = "Jane" }, item };

            // Act / Assert
            collection.Should().ContainEquivalentOf(item);
        }

        [Fact]
        public void When_collection_contains_object_equivalent_of_another_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { new Customer { Name = "Jane" }, new Customer { Name = "John" } };
            var item = new Customer { Name = "John" };

            // Act / Assert
            collection.Should().ContainEquivalentOf(item);
        }

        [Fact]
        public void When_character_collection_does_contain_equivalent_it_should_succeed()
        {
            // Arrange
            char[] collection = ("abc123ab").ToCharArray();
            char item = 'c';

            // Act / Assert
            collection.Should().ContainEquivalentOf(item);
        }

        [Fact]
        public void When_string_collection_does_contain_same_string_with_other_case_it_should_throw()
        {
            // Arrange
            string[] collection = new[] { "a", "b", "c" };
            string item = "C";

            // Act
            Action act = () => collection.Should().ContainEquivalentOf(item);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected collection {\"a\", \"b\", \"c\"} to contain equivalent of \"C\".*");
        }

        [Fact]
        public void When_string_collection_does_contain_same_string_it_should_throw_with_a_useful_message()
        {
            // Arrange
            string[] collection = new[] { "a" };
            string item = "b";

            // Act
            Action act = () => collection.Should().ContainEquivalentOf(item, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public void When_collection_does_not_contain_object_equivalent_of_another_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            int item = 4;

            // Act
            Action act = () => collection.Should().ContainEquivalentOf(item);

            // Act / Assert
            act.Should().Throw<XunitException>().WithMessage("Expected collection {1, 2, 3} to contain equivalent of 4.*");
        }

        [Fact]
        public void When_asserting_collection_to_contain_equivalent_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;
            int expectation = 1;

            // Act
            Action act = () =>
                collection.Should().ContainEquivalentOf(expectation, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain equivalent of 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_collection_contains_equivalent_null_object_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3, (int?)null };
            int? item = null;

            // Act
            Action act = () => collection.Should().ContainEquivalentOf(item);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_does_not_contain_equivalent_null_object_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            int? item = null;

            // Act
            Action act = () => collection.Should().ContainEquivalentOf(item);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected collection {1, 2, 3} to contain equivalent of <null>.*");
        }

        [Fact]
        public void When_empty_collection_does_not_contain_equivalent_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new int[0];
            int item = 1;

            // Act
            Action act = () => collection.Should().ContainEquivalentOf(item);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected collection {empty} to contain equivalent of 1.*");
        }

        [Fact]
        public void When_collection_does_not_contain_equivalent_because_of_second_property_it_should_throw()
        {
            // Arrange
            IEnumerable subject = new[]
            {
                new Customer
                {
                    Name = "John",
                    Age = 18
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 18
                }
            };
            var item = new Customer { Name = "John", Age = 20 };

            // Act
            Action act = () => subject.Should().ContainEquivalentOf(item);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_does_contain_equivalent_by_including_single_property_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[]
            {
                new Customer
                {
                    Name = "John",
                    Age = 18
                },
                new Customer
                {
                    Name = "Jane",
                    Age = 18
                }
            };
            var item = new Customer { Name = "John", Age = 20 };

            // Act / Assert
            collection.Should().ContainEquivalentOf(item, options => options.Including(x => x.Name));
        }

        [Fact]
        public void When_injecting_a_null_config_to_ContainEquivalentOf_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;
            object item = null;

            // Act
            Action act = () => collection.Should().ContainEquivalentOf(item, config: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("config");
        }

        [Fact]
        public void When_collection_contains_object_equivalent_of_boxed_object_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            object boxedValue = 2;

            // Act / Assert
            collection.Should().ContainEquivalentOf(boxedValue);
        }

        #endregion

        #region Be Subset Of

        [Fact]
        public void When_collection_is_subset_of_a_specified_collection_it_should_not_throw()
        {
            // Arrange
            IEnumerable subset = new[] { 1, 2 };
            IEnumerable superset = new[] { 1, 2, 3 };

            // Act / Assert
            subset.Should().BeSubsetOf(superset);
        }

        [Fact]
        public void When_collection_is_not_a_subset_of_another_it_should_throw_with_the_reason()
        {
            // Arrange
            IEnumerable subset = new[] { 1, 2, 3, 6 };
            IEnumerable superset = new[] { 1, 2, 4, 5 };

            // Act
            Action act = () => subset.Should().BeSubsetOf(superset, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(

#if NETCOREAPP1_1
                "Expected collection to be a subset of {1, 2, 4, 5} because we want to test the failure message, " +
                "but items {3, 6} are not part of the superset.");
#else
                "Expected subset to be a subset of {1, 2, 4, 5} because we want to test the failure message, " +
                    "but items {3, 6} are not part of the superset.");
#endif

        }

        [Fact]
        public void When_an_empty_collection_is_tested_against_a_superset_it_should_succeed()
        {
            // Arrange
            IEnumerable subset = new int[0];
            IEnumerable superset = new[] { 1, 2, 4, 5 };

            // Act
            Action act = () => subset.Should().BeSubsetOf(superset);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_subset_is_tested_against_a_null_superset_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            IEnumerable subset = new[] { 1, 2, 3 };
            IEnumerable superset = null;

            // Act
            Action act = () => subset.Should().BeSubsetOf(superset);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify a subset against a <null> collection.*");
        }

        [Fact]
        public void When_a_set_is_expected_to_be_not_a_subset_it_should_succeed()
        {
            // Arrange
            IEnumerable subject = new[] { 1, 2, 4 };
            IEnumerable otherSet = new[] { 1, 2, 3 };

            // Act / Assert
            subject.Should().NotBeSubsetOf(otherSet);
        }

        [Fact]
        public void When_an_empty_set_is_not_supposed_to_be_a_subset_of_another_set_it_should_throw()
        {
            // Arrange
            IEnumerable subject = new int[] { };
            IEnumerable otherSet = new[] { 1, 2, 3 };

            // Act
            Action act = () => subject.Should().NotBeSubsetOf(otherSet);

            // Assert
            act.Should().Throw<XunitException>()
#if NETCOREAPP1_1
                .WithMessage("Did not expect collection {empty} to be a subset of {1, 2, 3}.");
#else
                .WithMessage("Did not expect subject {empty} to be a subset of {1, 2, 3}.");
#endif
        }

        [Fact]
        public void Should_fail_when_asserting_collection_is_not_subset_of_a_superset_collection()
        {
            // Arrange
            IEnumerable subject = new[] { 1, 2 };
            IEnumerable otherSet = new[] { 1, 2, 3 };

            // Act
            Action act = () => subject.Should().NotBeSubsetOf(otherSet, "because I'm {0}", "mistaken");

            // Assert
#if NETCOREAPP1_1
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect collection {1, 2} to be a subset of {1, 2, 3} because I'm mistaken.");
#else
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect subject {1, 2} to be a subset of {1, 2, 3} because I'm mistaken.");
#endif
        }

        [Fact]
        public void When_asserting_collection_to_be_subset_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;
            IEnumerable collection1 = new[] { 1, 2, 3 };

            // Act
            Action act =
                () => collection.Should().BeSubsetOf(collection1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to be a subset of {1, 2, 3} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_to_not_be_subset_against_same_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable collection1 = collection;

            // Act
            Action act = () => collection.Should().NotBeSubsetOf(collection1,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect*to be a subset of*because we want to test the behaviour with same objects*but they both reference the same object.");
        }

        #endregion

        #region Contain

        [Fact]
        public void Should_succeed_when_asserting_collection_contains_an_item_from_the_collection()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().Contain(1);
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_contains_multiple_items_from_the_collection_in_any_order()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().Contain(new[] { 2, 1 });
        }

        [Fact]
        public void When_a_collection_does_not_contain_single_item_it_should_throw_with_clear_explanation()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Contain(4, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to contain 4 because we do.");
        }

        [Fact]
        public void When_asserting_collection_does_contain_item_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should()
                .Contain(1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain {1} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_a_collection_does_not_contain_another_collection_it_should_throw_with_clear_explanation()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Contain(new[] { 3, 4, 5 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to contain {3, 4, 5} because we do, but could not find {4, 5}.");
        }

        [Fact]
        public void When_the_contents_of_a_collection_are_checked_against_an_empty_collection_it_should_throw_clear_explanation()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Contain(new int[0]);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify containment against an empty collection*");
        }

        #endregion

        #region Not Contain

        [Fact]
        public void Should_succeed_when_asserting_collection_does_not_contain_an_item_that_is_not_in_the_collection()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().NotContain(4);
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_does_not_contain_any_items_that_is_not_in_the_collection()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().NotContain(new[] { 4, 5 });
        }

        [Fact]
        public void When_collection_contains_an_unexpected_item_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotContain(1, "because we {0} like it, but found it anyhow", "don't");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to not contain element 1 because we don't like it, but found it anyhow.");
        }

        [Fact]
        public void When_injecting_a_null_predicate_into_NotContain_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new int[] { };

            // Act
            Action act = () => collection.Should().NotContain(predicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("predicate");
        }

        [Fact]
        public void When_collection_does_contain_an_unexpected_item_matching_a_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotContain(item => item == 2, "because {0}s are evil", 2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to not have any items matching (item == 2) because 2s are evil,*{2}*");
        }

        [Fact]
        public void When_collection_does_not_contain_an_unexpected_item_matching_a_predicate_it_should_not_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().NotContain(item => item == 4);
        }

        [Fact]
        public void When_asserting_collection_does_not_contain_item_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should()
                .NotContain(1, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to not contain {1} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_collection_contains_unexpected_items_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should()
                .NotContain(new[] { 1, 2, 4 }, "because we {0} like them", "don't");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to not contain {1, 2, 4} because we don't like them, but found {1, 2}.");
        }

        #endregion

        #region Contain In Order

        [Fact]
        public void When_two_collections_contain_the_same_items_in_the_same_order_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 2, 3 };

            // Act / Assert
            collection.Should().ContainInOrder(1, 2, 3);
        }

        [Fact]
        public void When_collection_contains_null_value_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new object[] { 1, null, 2, "string" };

            // Act / Assert
            collection.Should().ContainInOrder(new object[] { 1, null, "string" });
        }

        [Fact]
        public void When_the_first_collection_contains_a_duplicate_item_without_affecting_the_order_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3, 2 };

            // Act / Assert
            collection.Should().ContainInOrder(1, 2, 3);
        }

        [Fact]
        public void When_two_collections_contain_the_same_duplicate_items_in_the_same_order_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 1, 2, 12, 2, 2 };

            // Act / Assert
            collection.Should().ContainInOrder(1, 2, 1, 2, 12, 2, 2);
        }

        [Fact]
        public void When_a_collection_does_not_contain_a_range_twice_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 1, 3, 12, 2, 2 };

            // Act
            Action act = () => collection.Should().ContainInOrder(1, 2, 1, 1, 2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 1, 3, 12, 2, 2} to contain items {1, 2, 1, 1, 2} in order, but 1 (index 3) did not appear (in the right order).");
        }

        [Fact]
        public void When_two_collections_contain_the_same_items_but_in_different_order_it_should_throw_with_a_clear_explanation()
        {
            // Act
            Action act = () => new[] { 1, 2, 3 }.Should().ContainInOrder(new[] { 3, 1 }, "because we said so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to contain items {3, 1} in order because we said so, but 1 (index 1) did not appear (in the right order).");
        }

        [Fact]
        public void When_a_collection_does_not_contain_an_ordered_item_it_should_throw_with_a_clear_explanation()
        {
            // Act
            Action act = () => new[] { 1, 2, 3 }.Should().ContainInOrder(new[] { 4, 1 }, "we failed");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to contain items {4, 1} in order because we failed, " +
                    "but 4 (index 0) did not appear (in the right order).");
        }

        [Fact]
        public void When_passing_in_null_while_checking_for_ordered_containment_it_should_throw_with_a_clear_explanation()
        {
            // Act
            Action act = () => new[] { 1, 2, 3 }.Should().ContainInOrder(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify ordered containment against a <null> collection.*");
        }

        [Fact]
        public void When_asserting_collection_contains_some_values_in_order_but_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] ints = null;

            // Act
            Action act =
                () => ints.Should().ContainInOrder(new[] { 4 }, "because we're checking how it reacts to a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to contain {4} in order because we're checking how it reacts to a null subject, but found <null>.");
#else
                "Expected ints to contain {4} in order because we're checking how it reacts to a null subject, but found <null>.");
#endif

        }

        #endregion

        #region (Not) be in order

        [Fact]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 2, 3 };

            // Act / Assert
            collection.Should().BeInAscendingOrder();
        }

        [Fact]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 2, 3 };

            // Act / Assert
            collection.Should().BeInAscendingOrder(Comparer<object>.Default);
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 6, 12, 15, 12, 17, 26 };

            // Act
            Action action = () => collection.Should().BeInAscendingOrder("because numbers are ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to contain items in ascending order because numbers are ordered," +
                    " but found {1, 6, 12, 15, 12, 17, 26} where item at index 3 is in wrong order.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 6, 12, 15, 12, 17, 26 };

            // Act
            Action action = () => collection.Should().BeInAscendingOrder(Comparer<object>.Default, "because numbers are ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to contain items in ascending order because numbers are ordered," +
                    " but found {1, 6, 12, 15, 12, 17, 26} where item at index 3 is in wrong order.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_not_in_ascending_order_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 5, 3 };

            // Act / Assert
            collection.Should().NotBeInAscendingOrder();
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_not_in_ascending_order_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 5, 3 };

            // Act / Assert
            collection.Should().NotBeInAscendingOrder(Comparer<object>.Default);
        }

        [Fact]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_not_in_ascending_order_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 2, 3 };

            // Act
            Action action = () => collection.Should().NotBeInAscendingOrder("because numbers are not ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to contain items in ascending order because numbers are not ordered," +
                    " but found {1, 2, 2, 3}.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_not_in_ascending_order_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 2, 3 };

            // Act
            Action action = () => collection.Should().NotBeInAscendingOrder(Comparer<object>.Default, "because numbers are not ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to contain items in ascending order because numbers are not ordered," +
                    " but found {1, 2, 2, 3}.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { "z", "y", "x" };

            // Act / Assert
            collection.Should().BeInDescendingOrder();
        }

        [Fact]
        public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { "z", "y", "x" };

            // Act / Assert
            collection.Should().BeInDescendingOrder(Comparer<object>.Default);
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "z", "x", "y" };

            // Act
            Action action = () => collection.Should().BeInDescendingOrder("because letters are ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to contain items in descending order because letters are ordered," +
                    " but found {\"z\", \"x\", \"y\"} where item at index 1 is in wrong order.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "z", "x", "y" };

            // Act
            Action action = () => collection.Should().BeInDescendingOrder(Comparer<object>.Default, "because letters are ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to contain items in descending order because letters are ordered," +
                    " but found {\"z\", \"x\", \"y\"} where item at index 1 is in wrong order.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_not_in_descending_order_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { "x", "y", "x" };

            // Act / Assert
            collection.Should().NotBeInDescendingOrder();
        }

        [Fact]
        public void When_asserting_the_items_in_an_unordered_collection_are_not_in_descending_order_using_the_given_comparer_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { "x", "y", "x" };

            // Act / Assert
            collection.Should().NotBeInDescendingOrder(Comparer<object>.Default);
        }

        [Fact]
        public void When_asserting_the_items_in_a_descending_ordered_collection_are_not_in_descending_order_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "c", "b", "a" };

            // Act
            Action action = () => collection.Should().NotBeInDescendingOrder("because numbers are not ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to contain items in descending order because numbers are not ordered," +
                    " but found {\"c\", \"b\", \"a\"}.");
        }

        [Fact]
        public void When_asserting_the_items_in_a_descending_ordered_collection_are_not_in_descending_order_using_the_given_comparer_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "c", "b", "a" };

            // Act
            Action action = () => collection.Should().NotBeInDescendingOrder(Comparer<object>.Default, "because numbers are not ordered");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to contain items in descending order because numbers are not ordered," +
                    " but found {\"c\", \"b\", \"a\"}.");
        }

        #endregion

        #region (Not) Intersect

        [Fact]
        public void When_asserting_the_items_in_an_two_intersecting_collections_intersect_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable otherCollection = new[] { 3, 4, 5 };

            // Act / Assert
            collection.Should().IntersectWith(otherCollection);
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_non_intersecting_collections_intersect_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable otherCollection = new[] { 4, 5 };

            // Act
            Action action = () => collection.Should().IntersectWith(otherCollection, "they should share items");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected collection to intersect with {4, 5} because they should share items," +
                    " but {1, 2, 3} does not contain any shared items.");
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_non_intersecting_collections_do_not_intersect_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable otherCollection = new[] { 4, 5 };

            // Act / Assert
            collection.Should().NotIntersectWith(otherCollection);
        }

        [Fact]
        public void When_asserting_the_items_in_an_two_intersecting_collections_do_not_intersect_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable otherCollection = new[] { 2, 3, 4 };

            // Act
            Action action = () => collection.Should().NotIntersectWith(otherCollection, "they should not share items");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect collection to intersect with {2, 3, 4} because they should not share items," +
                    " but found the following shared items {2, 3}.");
        }

        [Fact]
        public void When_asserting_collection_to_not_intersect_with_same_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable otherCollection = collection;

            // Act
            Action act = () => collection.Should().NotIntersectWith(otherCollection,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect*to intersect with*because we want to test the behaviour with same objects*but they both reference the same object.");
        }

        #endregion

        #region Not Contain Nulls

        [Fact]
        public void When_collection_does_not_contain_nulls_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().NotContainNulls();
        }

        [Fact]
        public void When_collection_contains_nulls_that_are_unexpected_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { new object(), null };

            // Act
            Action act = () => collection.Should().NotContainNulls("because they are {0}", "evil");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s because they are evil, but found one at index 1.");
        }

        [Fact]
        public void When_collection_contains_multiple_nulls_that_are_unexpected_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { new object(), null, new object(), null };

            // Act
            Action act = () => collection.Should().NotContainNulls("because they are {0}", "evil");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s*because they are evil*{1, 3}*");
        }

        [Fact]
        public void When_asserting_collection_to_not_contain_nulls_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should().NotContainNulls("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s because we want to test the behaviour with a null subject, but collection is <null>.");
        }

        #endregion

        #region Contain Items Assignable To

        [Fact]
        public void Should_succeed_when_asserting_collection_with_all_items_of_same_type_only_contains_item_of_one_type()
        {
            // Arrange
            IEnumerable collection = new[] { "1", "2", "3" };

            // Act / Assert
            collection.Should().ContainItemsAssignableTo<string>();
        }

        [Fact]
        public void Should_fail_when_asserting_collection_with_items_of_different_types_only_contains_item_of_one_type()
        {
            // Arrange
            IEnumerable collection = new List<object>
            {
                1,
                "2"
            };

            // Act
            Action act = () => collection.Should().ContainItemsAssignableTo<string>();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_collection_contains_anything_other_than_strings_it_should_throw_and_report_details()
        {
            // Arrange
            IEnumerable collection = new List<object>
            {
                1,
                "2"
            };

            // Act
            Action act = () => collection.Should().ContainItemsAssignableTo<string>();

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain only items of type System.String, but item 1 at index 0 is of type System.Int32.");
        }

        [Fact]
        public void When_a_collection_contains_anything_other_than_strings_it_should_use_the_reason()
        {
            // Arrange
            IEnumerable collection = new List<object>
            {
                1,
                "2"
            };

            // Act
            Action act = () => collection.Should().ContainItemsAssignableTo<string>(
                "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to contain only items of type System.String because we want to test the failure message" +
                        ", but item 1 at index 0 is of type System.Int32.");
        }

        [Fact]
        public void When_asserting_collection_contains_item_assignable_to_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should()
                .ContainItemsAssignableTo<string>("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain element assignable to type System.String because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Only Have Unique Items

        [Fact]
        public void Should_succeed_when_asserting_collection_with_unique_items_contains_only_unique_items()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3, 4 };

            // Act / Assert
            collection.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void When_a_collection_contains_duplicate_items_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3, 3 };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems("{0} don't like {1}", "we", "duplicates");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we don't like duplicates, but item 3 is not unique.");
        }

        [Fact]
        public void When_a_collection_contains_multiple_duplicate_items_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 2, 3, 3 };

            // Act
            Action act = () => collection.Should().OnlyHaveUniqueItems("{0} don't like {1}", "we", "duplicates");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we don't like duplicates, but items {2, 3} are not unique.");
        }

        [Fact]
        public void When_asserting_collection_to_only_have_unique_items_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act =
                () => collection.Should().OnlyHaveUniqueItems("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to only have unique items because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Start/End With

        [Fact]
        public void When_collection_does_not_start_with_a_specific_element_it_should_throw()
        {
            // Arrange
            var collection = new[] { "john", "jane", "mike" };

            // Act
            Action act = () => collection.Should().StartWith("ryan", "of some reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*start*ryan*because of some reason*but*john*");
        }

        [Fact]
        public void When_collection_does_not_start_with_a_specific_element_in_a_sequence_it_should_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().StartWith(new[] { "john", "ryan", "jane" }, "of some reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*start*ryan*because of some reason*but*differs at index 1*");
        }

        [Fact]
        public void When_collection_does_not_start_with_a_specific_element_in_a_sequence_using_custom_equality_comparison_it_should_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().StartWith(new[] { "john", "ryan", "jane" }, (s1, s2) => string.Equals(s1, s2, StringComparison.Ordinal), "of some reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*start*ryan*because of some reason*but*differs at index 1*");
        }

        [Fact]
        public void When_collection_starts_with_the_specific_element_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "jane", "mike" };

            // Act
            Action act = () => collection.Should().StartWith("john");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_starts_with_the_specific_sequence_of_elements_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().StartWith(new[] { "john", "bill" });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_starts_with_the_specific_sequence_of_elements_using_custom_equality_comparison_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().StartWith(new[] { "JoHn", "bIlL" }, (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_starts_with_the_specific_null_element_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { null, "jane", "mike" };

            // Act
            Action act = () => collection.Should().StartWith(null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_non_empty_collection_starts_with_the_empty_sequence_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "jane", "mike" };

            // Act
            Action act = () => collection.Should().StartWith(new string[] { });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_empty_collection_starts_with_the_empty_sequence_it_should_not_throw()
        {
            // Arrange
            var collection = new string[] { };

            // Act
            Action act = () => collection.Should().StartWith(new string[] { });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_starts_with_the_specific_sequence_with_null_elements_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { null, "john", null, "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().StartWith(new[] { null, "john", null, "bill" });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_starts_with_the_specific_sequence_with_null_elements_using_custom_equality_comparison_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { null, "john", null, "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().StartWith(new[] { null, "JoHn", null, "bIlL" }, (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_starts_with_null_but_that_wasnt_expected_it_should_throw()
        {
            // Arrange
            var collection = new[] { null, "jane", "mike" };

            // Act
            Action act = () => collection.Should().StartWith("john");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*start*john*but*null*");
        }

        [Fact]
        public void When_null_collection_is_expected_to_start_with_an_element_it_should_throw()
        {
            // Arrange
            string[] collection = null;

            // Act
            Action act = () => collection.Should().StartWith("john");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*start*john*but*collection*null*");
        }

        [Fact]
        public void When_collection_does_not_end_with_a_specific_element_it_should_throw()
        {
            // Arrange
            var collection = new[] { "john", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith("ryan", "of some reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*end*ryan*because of some reason*but*mike*");
        }

        [Fact]
        public void When_collection_does_end_with_a_specific_element_and_because_format_is_incorrect_it_should_not_fail()
        {
            // Arrange
            var collection = new[] { "john", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith("mike", "of some reason {0,abc}", 1, 2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_does_not_end_with_a_specific_element_in_a_sequence_it_should_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith(new[] { "bill", "ryan", "mike" }, "of some reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*end*ryan*because of some reason*but*differs at index 2*");
        }

        [Fact]
        public void When_collection_does_not_end_with_a_specific_element_in_a_sequence_using_custom_equality_comparison_it_should_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith(new[] { "bill", "ryan", "mike" }, (s1, s2) => string.Equals(s1, s2, StringComparison.Ordinal), "of some reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*end*ryan*because of some reason*but*differs at index 2*");
        }

        [Fact]
        public void When_collection_ends_with_the_specific_element_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith("mike");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_ends_with_the_specific_sequence_of_elements_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith(new[] { "jane", "mike" });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_ends_with_the_specific_sequence_of_elements_using_custom_equality_comparison_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith(new[] { "JaNe", "mIkE" }, (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_ends_with_the_specific_null_element_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "jane", "mike", null };

            // Act
            Action act = () => collection.Should().EndWith(null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_ends_with_the_specific_sequence_with_null_elements_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", null, "mike", null };

            // Act
            Action act = () => collection.Should().EndWith(new[] { "jane", null, "mike", null });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_ends_with_the_specific_sequence_with_null_elements_using_custom_equality_comparison_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "john", "bill", "jane", null, "mike", null };

            // Act
            Action act = () => collection.Should().EndWith(new[] { "JaNe", null, "mIkE", null }, (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_collection_ends_with_null_but_that_wasnt_expected_it_should_throw()
        {
            // Arrange
            var collection = new[] { "jane", "mike", null };

            // Act
            Action act = () => collection.Should().EndWith("john");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*end*john*but*null*");
        }

        [Fact]
        public void When_null_collection_is_expected_to_end_with_an_element_it_should_throw()
        {
            // Arrange
            string[] collection = null;

            // Act
            Action act = () => collection.Should().EndWith("john");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*end*john*but*collection*null*");
        }

        [Fact]
        public void When_non_empty_collection_ends_with_the_empty_sequence_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { "jane", "mike" };

            // Act
            Action act = () => collection.Should().EndWith(new string[] { });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_empty_collection_ends_with_the_empty_sequence_it_should_not_throw()
        {
            // Arrange
            var collection = new string[] { };

            // Act
            Action act = () => collection.Should().EndWith(new string[] { });

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region Have Element At

        [Fact]
        public void When_collection_has_expected_element_at_specific_index_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveElementAt(1, 2);
        }

        [Fact]
        public void When_collection_does_not_have_the_expected_element_at_specific_index_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveElementAt(1, 3, "we put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected 3 at index 1 because we put it there, but found 2.");
        }

        [Fact]
        public void When_collection_does_not_have_an_element_at_the_specific_index_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveElementAt(4, 3, "we put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected 3 at index 4 because we put it there, but found no element.");
        }

        [Fact]
        public void When_asserting_collection_has_element_at_specific_index_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;

            // Act
            Action act = () => collection.Should().HaveElementAt(1, 1,
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to have element at index 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_element_at_specific_index_was_found_it_should_allow_chaining()
        {
            // Arrange
            var expectedElement = new
            {
                SomeProperty = "hello"
            };

            var collection = new[]
            {
                expectedElement
            };

            // Act
            Action act = () => collection.Should()
                .HaveElementAt(0, expectedElement)
                .Which
                .Should().BeAssignableTo<string>();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*assignable*string*");
        }

        #endregion

        #region HaveElementPreceding

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_collection_has_the_correct_element_preceding_another_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding("mick", "cris");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_collection_has_the_wrong_element_preceding_another_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding("john", "cris", "because of some reason");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*cris*precede*john*because*reason*found*mick*");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_nothing_is_preceding_an_element_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding("cris", "jane");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*jane*precede*cris*found*nothing*");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_expecting_an_element_to_precede_another_but_collection_is_empty_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new string[0];

            // Act
            Action act = () => collection.Should().HaveElementPreceding("mick", "cris");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*cris*precede*mick*collection*empty*");
        }

        [Fact]
        public void When_a_null_element_is_preceding_another_element_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { null, "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding("mick", null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_a_null_element_is_not_preceding_another_element_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding("mick", null);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*null*precede*mick*but found*cris*");
        }

        [Fact]
        public void When_an_element_is_preceding_a_null_element_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "mick", null, "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding(null, "mick");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_element_is_not_preceding_a_null_element_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "mick", null, "john" };

            // Act
            Action act = () => collection.Should().HaveElementPreceding(null, "cris");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*cris*precede*null*but found*mick*");
        }

        #endregion

        #region HaveElementSucceeding

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_collection_has_the_correct_element_succeeding_another_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding("cris", "mick");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_collection_has_the_wrong_element_succeeding_another_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding("mick", "cris", "because of some reason");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*cris*succeed*mick*because*reason*found*john*");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_nothing_is_succeeding_an_element_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding("john", "jane");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*jane*succeed*john*found*nothing*");
        }

        [Fact]
        public void When_expecting_an_element_to_succeed_another_but_the_collection_is_empty_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new string[0];

            // Act
            Action act = () => collection.Should().HaveElementSucceeding("mick", "cris");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*cris*succeed*mick*collection*empty*");
        }

        [Fact]
        public void When_a_null_element_is_succeeding_another_element_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "mick", null, "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding("mick", null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_null_element_is_not_succeeding_another_element_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "cris", "mick", "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding("mick", null);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*null*succeed*mick*but found*john*");
        }

        [Fact]
        public void When_an_element_is_succeeding_a_null_element_it_should_not_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "mick", null, "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding(null, "john");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_element_is_not_succeeding_a_null_element_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { "mick", null, "john" };

            // Act
            Action act = () => collection.Should().HaveElementSucceeding(null, "cris");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*cris*succeed*null*but found*john*");
        }

        #endregion

        #region Miscellaneous

        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should()
                .HaveCount(3)
                .And
                .HaveElementAt(1, 2)
                .And.NotContain(4);
        }

        #endregion

        #region Have Same Count

        [Fact]
        public void When_both_collections_have_the_same_number_elements_it_should_succeed()
        {
            // Arrange
            IEnumerable firstCollection = new[] { 1, 2, 3 };
            IEnumerable secondCollection = new[] { 4, 5, 6 };

            // Act / Assert
            firstCollection.Should().HaveSameCount(secondCollection);
        }

        [Fact]
        public void When_both_collections_do_not_have_the_same_number_of_elements_it_should_fail()
        {
            // Arrange
            IEnumerable firstCollection = new[] { 1, 2, 3 };
            IEnumerable secondCollection = new[] { 4, 6 };

            // Act
            Action act = () => firstCollection.Should().HaveSameCount(secondCollection);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to have 2 item(s), but found 3.");
#else
                "Expected firstCollection to have 2 item(s), but found 3.");
#endif
        }

        [Fact]
        public void When_comparing_item_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            // Arrange
            IEnumerable firstCollection = new[] { 1, 2, 3 };
            IEnumerable secondCollection = new[] { 4, 6 };

            // Act
            Action act = () => firstCollection.Should().HaveSameCount(secondCollection, "we want to test the {0}", "reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to have 2 item(s) because we want to test the reason, but found 3.");
#else
                "Expected firstCollection to have 2 item(s) because we want to test the reason, but found 3.");
#endif
        }

        [Fact]
        public void When_asserting_collections_to_have_same_count_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;
            IEnumerable collection1 = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveSameCount(collection1,
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to have the same count as {1, 2, 3} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collections_to_have_same_count_against_an_other_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable otherCollection = null;

            // Act
            Action act = () => collection.Should().HaveSameCount(otherCollection);

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
            IEnumerable firstCollection = new[] { 1, 2, 3 };
            IEnumerable secondCollection = new[] { 4, 6 };

            // Act / Assert
            firstCollection.Should().NotHaveSameCount(secondCollection);
        }

        [Fact]
        public void When_asserting_not_same_count_and_both_collections_have_the_same_number_elements_it_should_fail()
        {
            // Arrange
            IEnumerable firstCollection = new[] { 1, 2, 3 };
            IEnumerable secondCollection = new[] { 4, 5, 6 };

            // Act
            Action act = () => firstCollection.Should().NotHaveSameCount(secondCollection);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to not have 3 item(s), but found 3.");
#else
                "Expected firstCollection to not have 3 item(s), but found 3.");
#endif
        }

        [Fact]
        public void When_comparing_not_same_item_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            // Arrange
            IEnumerable firstCollection = new[] { 1, 2, 3 };
            IEnumerable secondCollection = new[] { 4, 5, 6 };

            // Act
            Action act = () => firstCollection.Should().NotHaveSameCount(secondCollection, "we want to test the {0}", "reason");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected collection to not have 3 item(s) because we want to test the reason, but found 3.");
#else
                "Expected firstCollection to not have 3 item(s) because we want to test the reason, but found 3.");
#endif
        }

        [Fact]
        public void When_asserting_collections_to_not_have_same_count_against_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection = null;
            IEnumerable collection1 = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotHaveSameCount(collection1,
                "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to not have the same count as {1, 2, 3} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_asserting_collections_to_not_have_same_count_against_an_other_null_collection_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable otherCollection = null;

            // Act
            Action act = () => collection.Should().NotHaveSameCount(otherCollection);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify count against a <null> collection.*");
        }

        [Fact]
        public void When_asserting_collections_to_not_have_same_count_but_both_collections_references_the_same_object_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new[] { 1, 2, 3 };
            IEnumerable collection1 = collection;

            // Act
            Action act = () => collection.Should().NotHaveSameCount(collection1,
                "because we want to test the behaviour with same objects");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*not have the same count*because we want to test the behaviour with same objects*but they both reference the same object.");
        }

        #endregion

        #region ShouldAllBeAssignableTo

        [Fact]
        public void When_all_of_the_types_in_a_collection_match_expected_type_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new int[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().AllBeAssignableTo<int>();
        }

        [Fact]
        public void When_all_of_the_types_in_a_collection_match_the_type_or_subtype_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new object[] { new Exception(), new ArgumentException() };

            // Act / Assert
            collection.Should().AllBeAssignableTo<Exception>();
        }

        [Fact]
        public void When_one_of_the_types_does_not_match_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            IEnumerable collection = new object[] { 1, "2", 3 };

            // Act
            Action act = () => collection.Should().AllBeAssignableTo<int>("because they are of different type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.Int32\" because they are of different type, but found \"[System.Int32, System.String, System.Int32]\".");
        }

        [Fact]
        public void When_one_of_the_elements_is_null_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            IEnumerable collection = new object[] { 1, null, 3 };

            // Act
            Action act = () => collection.Should().AllBeAssignableTo<int>("because they are of different type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.Int32\" because they are of different type, but found a null element.");
        }

        [Fact]
        public void When_collection_is_of_matching_types_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new Type[] { typeof(Exception), typeof(ArgumentException) };

            // Act / Assert
            collection.Should().AllBeAssignableTo<Exception>();
        }

        [Fact]
        public void When_collection_of_types_contains_one_type_that_does_not_match_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            IEnumerable collection = new Type[] { typeof(int), typeof(string), typeof(int) };

            // Act
            Action act = () => collection.Should().AllBeAssignableTo<int>("because they are of different type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.Int32\" because they are of different type, but found \"[System.Int32, System.String, System.Int32]\".");
        }

        [Fact]
        public void When_collection_of_types_and_objects_are_all_of_matching_types_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new object[] { typeof(int), 2, typeof(int) };

            // Act / Assert
            collection.Should().AllBeAssignableTo<int>();
        }

        [Fact]
        public void When_collection_of_different_types_and_objects_are_all_assignable_to_type_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new object[] { typeof(Exception), new ArgumentException() };

            // Act / Assert
            collection.Should().AllBeAssignableTo<Exception>();
        }

        #endregion

        #region ShouldAllBeOfType

        [Fact]
        public void When_all_of_the_types_in_a_collection_match_expected_type_exactly_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new int[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().AllBeOfType<int>();
        }

        [Fact]
        public void When_one_of_the_types_does_not_match_exactly_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            IEnumerable collection = new object[] { new Exception(), new ArgumentException() };

            // Act
            Action act = () => collection.Should().AllBeOfType<Exception>("because they are of different type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.Exception\" because they are of different type, but found \"[System.Exception, System.ArgumentException]\".");
        }

        [Fact]
        public void When_one_of_the_elements_is_null_for_an_exact_match_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            IEnumerable collection = new object[] { 1, null, 3 };

            // Act
            Action act = () => collection.Should().AllBeOfType<int>("because they are of different type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.Int32\" because they are of different type, but found a null element.");
        }

        [Fact]
        public void When_collection_of_types_match_expected_type_exactly_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new Type[] { typeof(int), typeof(int), typeof(int) };

            // Act / Assert
            collection.Should().AllBeOfType<int>();
        }

        [Fact]
        public void When_collection_of_types_and_objects_match_type_exactly_it_should_succeed()
        {
            // Arrange
            IEnumerable collection = new object[] { typeof(ArgumentException), new ArgumentException() };

            // Act / Assert
            collection.Should().AllBeOfType<ArgumentException>();
        }

        [Fact]
        public void When_collection_of_types_and_objects_do_not_match_type_exactly_it_should_throw()
        {
            // Arrange
            IEnumerable collection = new object[] { typeof(Exception), new ArgumentException() };

            // Act
            Action act = () => collection.Should().AllBeOfType<ArgumentException>();

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.ArgumentException\", but found \"[System.Exception, System.ArgumentException]\".");
        }

        #endregion
    }

    internal class CountingNonGenericEnumerable : IEnumerable
    {
        private readonly IEnumerable _backingSet;

        public CountingNonGenericEnumerable(IEnumerable backingSet)
        {
            _backingSet = backingSet;
        }

        public int GetEnumeratorCallCount { get; private set; }

        public IEnumerator GetEnumerator()
        {
            GetEnumeratorCallCount++;
            return _backingSet.GetEnumerator();
        }
    }

    internal class CountingGenericEnumerable<TElement> : IEnumerable<TElement>
    {
        private readonly IEnumerable<TElement> _backingSet;

        public CountingGenericEnumerable(IEnumerable<TElement> backingSet)
        {
            _backingSet = backingSet;
            GetEnumeratorCallCount = 0;
        }

        public int GetEnumeratorCallCount { get; private set; }

        public IEnumerator<TElement> GetEnumerator()
        {
            GetEnumeratorCallCount++;
            return _backingSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class CountingNonGenericCollection : ICollection
    {
        private readonly ICollection _backingSet;

        public CountingNonGenericCollection(ICollection backingSet)
        {
            _backingSet = backingSet;
        }

        public int GetEnumeratorCallCount { get; private set; }

        public IEnumerator GetEnumerator()
        {
            GetEnumeratorCallCount++;
            return _backingSet.GetEnumerator();
        }

        public void CopyTo(Array array, int index) { throw new NotImplementedException(); }

        public int GetCountCallCount { get; private set; }

        public int Count
        {
            get
            {
                GetCountCallCount++;
                return _backingSet.Count;
            }
        }

        public object SyncRoot { get; private set; }
        public bool IsSynchronized { get { return true; } }
    }

    internal class CountingGenericCollection<TElement> : ICollection<TElement>
    {
        private readonly ICollection<TElement> _backingSet;

        public CountingGenericCollection(ICollection<TElement> backingSet)
        {
            _backingSet = backingSet;
        }

        public int GetEnumeratorCallCount { get; private set; }

        public IEnumerator<TElement> GetEnumerator()
        {
            GetEnumeratorCallCount++;
            return _backingSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TElement item) { throw new NotImplementedException(); }
        public void Clear() { throw new NotImplementedException(); }
        public bool Contains(TElement item) { throw new NotImplementedException(); }
        public void CopyTo(TElement[] array, int arrayIndex) { throw new NotImplementedException(); }
        public bool Remove(TElement item) { throw new NotImplementedException(); }

        public int GetCountCallCount { get; private set; }

        public int Count
        {
            get
            {
                GetCountCallCount++;
                return _backingSet.Count;
            }
        }

        public bool IsReadOnly { get; private set; }
    }

    internal class CustomEnumerable : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            foreach (string s in new[] { "a", "b", "c" })
            {
                yield return s;
            }
        }
    }

    internal class TrackingTestEnumerable : IEnumerable
    {
        private readonly int[] values;

        public TrackingTestEnumerable(params int[] values)
        {
            this.values = values;
            Enumerator = new TrackingEnumerator(this.values);
        }

        public TrackingEnumerator Enumerator { get; }

        public IEnumerator GetEnumerator()
        {
            Enumerator.IncreaseEnumerationCount();
            Enumerator.Reset();
            return Enumerator;
        }
    }

    internal class TrackingEnumerator : IEnumerator
    {
        private readonly int[] values;
        private int loopCount;
        private int index;

        public TrackingEnumerator(int[] values)
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

        public bool MoveNext()
        {
            index++;
            return index < values.Length;
        }

        public void Reset()
        {
            index = -1;
        }

        public object Current
        {
            get { return values[index]; }
        }
    }
}
