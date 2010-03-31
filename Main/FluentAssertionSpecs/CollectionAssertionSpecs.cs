using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class CollectionAssertionSpecs
    {
        #region Be Null

        [TestMethod]
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

        [TestMethod]
        public void When_a_custom_enumerable_implementation_is_expected_not_to_be_null_and_it_is_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var enumerable = new CustomEnumerable();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            enumerable.Should().NotBeNull();
        }

        [TestMethod]
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
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected collection to be <null> because null is valid, but found <empty collection>.");
        }

        [TestMethod]
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

        [TestMethod]
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
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected collection not to be <null> because someCollection should not.");
        }

        #endregion

        #region Have Count

        [TestMethod]
        public void Should_succeed_when_asserting_collection_has_a_count_that_equals_the_number_of_items()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().HaveCount(3);
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_has_a_count_that_is_different_from_the_number_of_items()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().HaveCount(4);
        }

        [TestMethod]
        public void
            Should_fail_with_descriptive_message_when_asserting_collection_has_a_count_that_is_different_from_the_number_of_items()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            var assertions = collection.Should();
            assertions.ShouldThrow(x => x.HaveCount(4, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected <4> items because we want to test the failure message, but found <3>.");
        }

        #endregion

        #region Be Empty

        [TestMethod]
        public void Should_succeed_when_asserting_collection_without_items_is_empty()
        {
            IEnumerable collection = new int[0];
            collection.Should().BeEmpty();
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_with_items_is_empty()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().BeEmpty();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_with_items_is_empty()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            var assertions = collection.Should();
            assertions.ShouldThrow(x => x.BeEmpty("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected no items because we want to test the failure message, but found <3>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_with_items_is_not_empty()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().NotBeEmpty();
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_without_items_is_not_empty()
        {
            IEnumerable collection = new int[0];
            collection.Should().NotBeEmpty();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_without_items_is_not_empty()
        {
            IEnumerable collection = new int[0];
            var assertions = collection.Should();
            assertions.ShouldThrow(x => x.NotBeEmpty("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected one or more items because we want to test the failure message.");
        }

        #endregion

        #region Be Equal

        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 3 };
            collection1.Should().Equal(collection2);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_equal_to_the_same_list_of_elements()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().Equal(1, 2, 3);
        }

        [TestMethod]
        public void When_two_collections_are_not_equal_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 5 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().Equal(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected collection <1, 2, 3> to be equal to <1, 2, 5>, but it differs at index 2");
        }
        
        [TestMethod]
        public void When_two_collections_are_not_equal_it_should_throw_using_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 5 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().Equal(collection2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected collection <1, 2, 3> to be equal to <1, 2, 5> because we want to test the failure message," + 
                " but it differs at index 2");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_not_equal_to_a_different_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1, 2 };
            collection1.Should()
                .NotEqual(collection2);
        }

        [TestMethod]
        public void When_two_equal_collections_are_not_expected_to_be_equal_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotEqual(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Did not expect collections <1, 2, 3> and <1, 2, 3> to be equal.");
        }

        [TestMethod]
        public void When_two_equal_collections_are_not_expected_to_be_equal_it_should_report_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotEqual(collection2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Did not expect collections <1, 2, 3> and <1, 2, 3> to be equal because we want to test the failure message.");
        }

        #endregion

        #region Be Equivalent To

        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_equivalent_to_an_equivalent_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1, 2 };
            collection1.Should().BeEquivalentTo(collection2);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_equivalent_to_an_equivalent_list_of_elements()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().BeEquivalentTo(3, 1, 2);
        }

        [TestMethod]
        public void When_collections_are_not_equivalent_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().BeEquivalentTo(collection2, "we treat {0} alike", "all");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected collection <1, 2, 3> to contain the same items as <1, 2> in any order because we treat all alike.");
        }        
        
        [TestMethod]
        public void When_testing_for_equivalence_against_empty_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new int[0];

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().BeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<ArgumentException>().WithMessage(
                "Cannot verify equivalence against an empty collection.");
        }
        
        [TestMethod]
        public void When_testing_for_equivalence_against_null_collection_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().BeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<NullReferenceException>().WithMessage(
                "Cannot verify equivalence against a <null> collection.");
        }
        
        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_not_equivalent_to_a_different_collection()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection1.Should().NotBeEquivalentTo(collection2);
        }

        [TestMethod]
        public void When_collections_are_unexpectingly_equivalent_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotBeEquivalentTo(collection2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected collection <1, 2, 3> not be equivalent with collection <3, 1, 2>.");
        }

        #endregion

        #region Be Subset Of

        [TestMethod]
        public void When_collection_is_subset_of_a_specified_collection_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subset = new[] { 1, 2 };
            IEnumerable superset = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            subset.Should().BeSubsetOf(superset);
        }

        [TestMethod]
        public void When_collection_is_not_a_subset_of_another_it_should_throw_with_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subset = new[] { 1, 2, 3, 6 };
            IEnumerable superset = new[] { 1, 2, 4, 5 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subset.Should().BeSubsetOf(superset, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected collection to be a subset of <1, 2, 4, 5> because we want to test the failure message, " +
                "but items <3, 6> are not part of the superset.");
        }

        [TestMethod]
        public void When_an_empty_collection_is_tested_against_a_superset_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subset = new int[0];
            IEnumerable superset = new[] { 1, 2, 4, 5 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subset.Should().BeSubsetOf(superset);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected collection to be a subset of <1, 2, 4, 5>, but the subset is empty.");
        }

        [TestMethod]
        public void When_a_subset_is_tested_against_a_null_superset_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable subset = new[] { 1, 2, 3};
            IEnumerable superset = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subset.Should().BeSubsetOf(superset);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<NullReferenceException>().WithMessage(
                "Cannot verify a subset against a <null> collection.");
        }
        
        [TestMethod]
        public void When_a_set_is_expected_to_be_not_a_subset_and_it_isnt_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable otherSet = new[] { 1, 2, 4 };
            IEnumerable superSet = new[] { 1, 2, 3 };
            
            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            otherSet.Should().NotBeSubsetOf(superSet);
        }

        [TestMethod]
        public void Should_fail_when_asserting_collection_is_not_subset_of_a_superset_collection()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection1 = new[] { 1, 2 };
            IEnumerable collection2 = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection1.Should().NotBeSubsetOf(collection2, "because I'm {0}", "mistaken");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected collection <1, 2> not to be a subset of <1, 2, 3> because I'm mistaken, but it is anyhow.");
        }

        #endregion

        #region Contain

        [TestMethod]
        public void Should_succeed_when_asserting_collection_contains_an_item_from_the_collection()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().Contain(1);
        }
        
        [TestMethod]
        public void Should_succeed_when_asserting_collection_contains_multiple_items_from_the_collection_in_any_order()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().Contain(new[] {2, 1});
        }

        [TestMethod]
        public void When_a_collection_does_not_contain_single_item_it_should_throw_with_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(4, "because {0}", "we do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected collection <1, 2, 3> to contain <4> because we do.");
        }

        [TestMethod]
        public void When_a_collection_does_not_contain_another_collection_it_should_throw_with_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(new[]{ 3, 4, 5}, "because {0}", "we do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected collection <1, 2, 3> to contain <3, 4, 5> because we do, but could not find <4, 5>.");
        }

        [TestMethod]
        public void When_the_contents_of_a_collection_are_checked_against_an_empty_collection_it_should_throw_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(new int[0]);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<ArgumentException>().WithMessage(
                "Connect verify containment against an empty collection");
        }

        [TestMethod]
        public void When_collection_does_not_contain_an_expected_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(item => item > 3, "at least {0} item should be larger than 3", 1);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Collection <1, 2, 3> should have an item matching (item > 3) because at least 1 item should be larger than 3.");
        }
        
        [TestMethod]
        public void When_collection_does_contain_an_expected_item_matching_a_predicate_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().Contain(item => item == 2);
        }
        
        [TestMethod]
        public void When_a_collection_of_strings_contains_the_expected_string_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var strings = new[] { "string1", "string2", "string3" };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            strings.Should().Contain("string2");
        }

        [TestMethod]
        public void When_a_collection_of_strings_does_not_contain_the_expected_string_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var strings = new[] { "string1", "string2", "string3" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().Contain("string4", "because {0} is required", "4");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected collection <string1, string2, string3> to contain \"string4\" because 4 is required.");
        }

        #endregion

        #region Not Contain

        [TestMethod]
        public void Should_succeed_when_asserting_collection_does_not_contain_an_item_that_is_not_in_the_collection()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().NotContain(4);
        }
        
        [TestMethod]
        public void When_collection_contains_an_unexpected_item_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotContain(1, "because we {0} like it", "don't");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Collection <1, 2, 3> should not contain <1> because we don't like it, but found it anyhow.");
        }

        [TestMethod]
        public void When_collection_does_contain_an_unexpected_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotContain(item => item == 2, "because {0}s are evil", 2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Collection <1, 2, 3> should not have any items matching (item == 2) because 2s are evil.");
        }

        [TestMethod]
        public void When_collection_does_not_contain_an_unexpected_item_matching_a_predicate_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().NotContain(item => item == 4);
        }

        #endregion

        #region Contain In Order

        [TestMethod]
        public void When_two_collections_contain_the_same_items_in_the_same_order_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().ContainInOrder(new[] { 1, 3 });
        }        
       
        [TestMethod]
        public void When_two_collections_contain_the_same_items_but_in_different_order_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new[] { 1, 2, 3 }.Should().ContainInOrder(new[] {3, 1}, "because we said so");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected items <3, 1> in ordered collection <1, 2, 3> " + 
                "because we said so, but the order did not match.");
        }

        [TestMethod]
        public void When_a_collection_does_not_contain_an_ordered_item_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new[] { 1, 2, 3 }.Should().ContainInOrder(new[] { 4, 1 }, "we failed");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected items <4, 1> in ordered collection <1, 2, 3> " +
                "because we failed, but <4> did not appear.");
        }

        [TestMethod]
        public void When_passing_in_null_while_checking_for_ordered_containment_it_should_throw_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => new[] { 1, 2, 3 }.Should().ContainInOrder(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<NullReferenceException>().WithMessage(
                "Cannot verify ordered containment against a <null> collection.");
        }

        #endregion

        #region Not Contain Nulls

        [TestMethod]
        public void When_collection_does_not_contain_nulls_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().NotContainNulls();
        }

        [TestMethod]
        public void When_collection_contains_nulls_that_are_unexpected_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { new object(), null };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().NotContainNulls("because they are {0}", "evil");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected no <null> in collection because they are evil, but found one at index <1>.");
        }

        #endregion

        #region Contain Items Assignable To

        [TestMethod]
        public void Should_succeed_when_asserting_collection_with_all_items_of_same_type_only_contains_item_of_one_type()
        {
            IEnumerable collection = new[] { "1", "2", "3" };
            collection.Should().ContainItemsAssignableTo<string>();
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_with_items_of_different_types_only_contains_item_of_one_type()
        {
            IEnumerable collection = new List<object> { 1, "2" };
            collection.Should().ContainItemsAssignableTo<string>();
        }

        [TestMethod]
        public void When_a_collection_contains_anything_other_than_strings_it_should_throw_and_report_details()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new List<object> { 1, "2" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainItemsAssignableTo<string>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected only <System.String> items in collection, but item <1> at index 0 is of type <System.Int32>.");
        }       

        [TestMethod]
        public void When_a_collection_contains_anything_other_than_strings_it_should_use_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new List<object> { 1, "2" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainItemsAssignableTo<string>(
                "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>()
                .WithMessage(
                "Expected only <System.String> items in collection because we want to test the failure message" + 
                ", but item <1> at index 0 is of type <System.Int32>.");
        }

        #endregion

        #region Only Have Unique Items

        [TestMethod]
        public void Should_succeed_when_asserting_collection_with_unique_items_contains_only_unique_items()
        {
            IEnumerable collection = new[] { 1, 2, 3, 4 };
            collection.Should().OnlyHaveUniqueItems();
        }

        [TestMethod]
        public void When_a_collection_contains_duplicate_items_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().OnlyHaveUniqueItems("{0} don't like {1}", "we", "duplicates");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected only unique items because we don't like duplicates, but item <3> was found multiple times.");
        }

        #endregion

        #region Have Element At

        [TestMethod]
        public void When_collection_has_expected_element_at_specific_index_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            collection.Should().HaveElementAt(1, 2);

        }

        [TestMethod]
        public void When_collection_does_not_have_the_expected_element_at_specific_index_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };
            
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveElementAt(1, 3, "we put it {0}", "there");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected <3> at index 1 because we put it there, but found <2>.");
        }

        [TestMethod]
        public void When_collection_does_not_have_an_element_at_the_specific_index_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().HaveElementAt(4, 3, "we put it {0}", "there");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------            
            act.ShouldThrow().Exception<SpecificationMismatchException>().WithMessage(
                "Expected <3> at index 4 because we put it there, but found no element.");
        }

        #endregion

        #region Miscellaneous

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should()
                .HaveCount(3)
                .And
                .HaveElementAt(1, 2)
                .And.NotContain(4);
        }

        #endregion

        #region Have Same Count

        [TestMethod]
        public void When_both_collections_have_the_same_number_elements_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable firstCollection = new[] { 1, 2, 3 };
            IEnumerable secondCollection = new[] { 4, 5, 6 };

            var extensions = firstCollection.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            extensions.HaveSameCount(secondCollection);
        }

        [TestMethod]
        public void When_both_collections_do_not_have_the_same_number_of_elements_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable firstCollection = new[] { 1, 2, 3 };
            IEnumerable secondCollection = new[] { 4, 6 };

            var extensions = firstCollection.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            extensions
                .ShouldThrow(e => e.HaveSameCount(secondCollection))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected collection to have <2> items, but found <3>.");
        }        
        
        [TestMethod]
        public void When_comparing_item_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable firstCollection = new[] { 1, 2, 3 };
            IEnumerable secondCollection = new[] { 4, 6 };

            var extensions = firstCollection.Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            extensions
                .ShouldThrow(e => e.HaveSameCount(secondCollection, "we want to test the {0}", "reason"))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected collection to have <2> items because we want to test the reason, but found <3>.");
        }

        #endregion
    }

    internal class CustomEnumerable : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            foreach (string s in new[]{"a", "b", "c"})
            {
                yield return s;
            }
        }
    }
}