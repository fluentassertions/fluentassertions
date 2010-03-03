using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class CollectionAssertionSpecs
    {
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
                .And.WithMessage("Expected <4> items because we want to test the failure message, but found <3>.");
        }

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
                .And.WithMessage("Expected no items because we want to test the failure message, but found <3>.");
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
                .And.WithMessage("Expected one or more items because we want to test the failure message.");
        }

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
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_is_equal_to_a_different_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1, 2 };
            collection1.Should().Equal(collection2);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_is_equal_to_a_different_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1, 2 };
            var assertions = collection1.Should();
            assertions.ShouldThrow(x => x.Equal(collection2, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected collections to be equal because we want to test the failure message.");
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
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_is_not_equal_to_the_same_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 3 };
            collection1.Should().NotEqual(collection2);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_is_not_equal_to_the_same_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 3 };
            var assertions = collection1.Should();
            assertions.ShouldThrow(x => x.NotEqual(collection2, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Did not expect collections to be equal because we want to test the failure message.");
        }

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
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_is_equivalent_to_a_different_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2 };
            collection1.Should().BeEquivalentTo(collection2);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_is_equivalent_to_a_different_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2 };
            var assertions = collection1.Should();
            assertions.ShouldThrow(x => x.BeEquivalentTo(collection2, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected collections to be equivalent because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_not_equivalent_to_a_different_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1 };
            collection1.Should().NotBeEquivalentTo(collection2);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_is_not_equivalent_to_an_equivalent_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1, 2 };
            collection1.Should().NotBeEquivalentTo(collection2);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_is_not_equivalent_to_an_equivalent_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 3, 1, 2 };
            var assertions = collection1.Should();
            assertions.ShouldThrow(x => x.NotBeEquivalentTo(collection2, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Did not expect collections to be equivalent because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_subset_of_a_superset_collection()
        {
            IEnumerable collection1 = new[] { 1, 2 };
            IEnumerable collection2 = new[] { 1, 2, 3 };
            collection1.Should().BeSubsetOf(collection2);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_is_subset_of_a_different_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 4, 5 };
            collection1.Should().BeSubsetOf(collection2);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_is_subset_of_a_different_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 3 };
            IEnumerable collection2 = new[] { 1, 2, 4, 5 };
            var assertions = collection1.Should();
            assertions.ShouldThrow(x => x.BeSubsetOf(collection2, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage(
                "Expected current collection to be a subset of the supplied collection because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_is_not_subset_of_different_collection()
        {
            IEnumerable collection1 = new[] { 1, 2, 4 };
            IEnumerable collection2 = new[] { 1, 2, 3 };
            collection1.Should().NotBeSubsetOf(collection2);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_is_not_subset_of_a_superset_collection()
        {
            IEnumerable collection1 = new[] { 1, 2 };
            IEnumerable collection2 = new[] { 1, 2, 3 };
            collection1.Should().NotBeSubsetOf(collection2);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_is_not_subset_of_a_superset_collection()
        {
            IEnumerable collection1 = new[] { 1, 2 };
            IEnumerable collection2 = new[] { 1, 2, 3 };
            var assertions = collection1.Should();
            assertions.ShouldThrow(x => x.NotBeSubsetOf(collection2, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage(
                "Did not expect current collection to be a subset of the supplied collection because we want to test the failure message.");
        }

        #region (Not)Contain

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
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_contains_an_item_that_is_not_in_the_collection()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().Contain(4);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_contains_multiple_items_that_are_not_in_the_collection()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().Contain(new[] { 4, 5 }) ;
        }        
       
        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_contains_an_item_that_is_not_in_the_collection()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            var assertions = collection.Should();
            assertions.ShouldThrow(x => x.Contain(4, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected current collection to contain <4> because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_contains_multiple_items_that_are_not_in_the_collection()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            var assertions = collection.Should();
            assertions.ShouldThrow(x => x.Contain(new[] { 4, 5}, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected current collection to contain <4, 5> because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_does_not_contain_an_item_that_is_not_in_the_collection()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().NotContain(4);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_does_not_contain_an_item_from_the_collection()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().NotContain(1);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_does_not_contain_an_item_from_the_collection()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            var assertions = collection.Should();
            assertions.ShouldThrow(x => x.NotContain(1, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Did not expect current collection to contain <1> because we want to test the failure message.");
        }

        #endregion

        #region ContainInOrder
        
        [TestMethod]
        public void Should_succeed_asserting_collection_when_collection_contains_multiples_item_in_same_order()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().ContainInOrder(new[] { 1, 3 });
        }        
        
        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_asserting_collection_when_collection_contains_multiple_items_in_different_order()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().ContainInOrder(new[] { 3, 1 });
        }
        
        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void Should_fail_asserting_collection_when_collection_does_not_contain_an_element()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().ContainInOrder(new[] { 4 });
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_collection_contains_multiple_items_in_different_order()
        {
            IEnumerable actual = new[] { 1, 2, 3 };
            IEnumerable expected = new[] { 3, 1 };
            var assertions = actual.Should();
            
            assertions.ShouldThrow(x => x.ContainInOrder(expected, "because of {0}", "the test"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage(
                "Expected current collection to contain <3, 1> in that order because of the test.");
        }

        #endregion

        [TestMethod]
        public void Should_succeed_when_asserting_collection_without_nulls_does_not_contain_nulls()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().NotContainNulls();
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_with_nulls_does_not_contain_nulls()
        {
            IEnumerable collection = new[] { new object(), null };
            collection.Should().NotContainNulls();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_with_nulls_does_not_contain_nulls()
        {
            IEnumerable collection = new[] { new object(), null };
            var assertions = collection.Should();
            assertions.ShouldThrow(x => x.NotContainNulls("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage(
                "Did not expect current collection to contain null values because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_with_all_items_of_same_type_only_contains_item_of_one_type()
        {
            IEnumerable collection = new[] { "1", "2", "3" };
            collection.Should().OnlyContainItemsOfType<string>();
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_with_items_of_different_types_only_contains_item_of_one_type()
        {
            IEnumerable collection = new List<object> { 1, "2" };
            collection.Should().OnlyContainItemsOfType<string>();
        }

        [TestMethod]
        public void
            Should_fail_with_descriptive_message_when_asserting_collection_with_items_of_different_types_only_contains_item_of_one_type
            ()
        {
            IEnumerable collection = new List<object> { 1, "2" };
            var assertions = collection.Should();
            assertions.ShouldThrow(x => x.OnlyContainItemsOfType<string>("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage(
                "Expected only <System.String> items in current collection because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_collection_with_unique_items_contains_only_unique_items()
        {
            IEnumerable collection = new[] { 1, 2, 3, 4 };
            collection.Should().OnlyHaveUniqueItems();
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_collection_with_duplicate_items_contains_only_unique_items()
        {
            IEnumerable collection = new[] { 1, 2, 3, 3 };
            collection.Should().OnlyHaveUniqueItems();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_collection_with_duplicate_items_contains_only_unique_items
            ()
        {
            IEnumerable collection = new[] { 1, 2, 3, 3 };
            var assertions = collection.Should();
            assertions.ShouldThrow(x => x.OnlyHaveUniqueItems("because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected only unique items in current collection because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_element_at_index_is_the_same_element()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().HaveElementAt(1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_element_at_index_is_a_different_element()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should().HaveElementAt(1, 3);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_element_at_index_is_a_different_element()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            var assertions = collection.Should();
            assertions.ShouldThrow(x => x.HaveElementAt(1, 3, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected <3> at the supplied index because we want to test the failure message, but found <2>.");
        }

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            IEnumerable collection = new[] { 1, 2, 3 };
            collection.Should()
                .HaveCount(3)
                .And
                .HaveElementAt(1, 2)
                .And
                .NotContain(4);
        }

        #region HaveSameCount

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
                .And.WithMessage("Expected collection to have <2> items, but found <3>.");
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
                .And.WithMessage("Expected collection to have <2> items because we want to test the reason, but found <3>.");
        }

        #endregion

    }
}