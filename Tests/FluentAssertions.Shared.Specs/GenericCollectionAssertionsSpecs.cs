using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class GenericCollectionAssertionsSpecs
    {
        [TestMethod]
        public void When_asserting_equality_with_a_collection_built_from_params_arguments_that_are_assignable_to_the_subjects_type_parameter_it_should_succeed_by_treating_the_arguments_as_of_that_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            byte[] byteArray = { 0xfe, 0xdc, 0xba, 0x98, 0x76, 0x54, 0x32, 0x10 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => byteArray.Should().Equal(0xfe, 0xdc, 0xba, 0x98, 0x76, 0x54, 0x32, 0x10);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #region (Not) Contain

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
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Collection {1, 2, 3} should have an item matching (item > 3) because at least 1 item should be larger than 3.");
        }

        [TestMethod]
        public void When_collection_does_contain_an_expected_item_matching_a_predicate_it_should_allow_chaining_it()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(item => item == 2).Which.Should().BeGreaterThan(4);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected*greater*4*2*");
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
            IEnumerable<string> strings = new[] { "string1", "string2", "string3" };

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
            IEnumerable<string> strings = new[] { "string1", "string2", "string3" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().Contain("string4", "because {0} is required", "4");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {\"string1\", \"string2\", \"string3\"} to contain \"string4\" because 4 is required.");
        }

        [TestMethod]
        public void When_a_collection_does_not_contain_the_combination_of_a_collection_and_a_single_item_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<object> strings = new[] { "string1", "string2" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().Contain(strings, new object[] {"string3"});

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {\"string1\", \"string2\"} to contain {\"string1\", \"string2\", \"string3\"}, but could not find {\"string3\"}.");
        }

        [TestMethod]
        public void When_asserting_collection_contains_some_values_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            const IEnumerable<string> strings = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().Contain("string4", "because we're checking how it reacts to a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to contain \"string4\" because we're checking how it reacts to a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_the_multiple_matching_objects_exists_it_continuation_using_the_matched_value_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            DateTime now = DateTime.Now;

            IEnumerable<DateTime> collection = new[] { now, DateTime.SpecifyKind(now, DateTimeKind.Unspecified) };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().Contain(now).Which.Kind.Should().Be(DateTimeKind.Local);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_collection_contains_values_according_to_predicate_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            const IEnumerable<string> strings = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().Contain(x => x == "xxx", "because we're checking how it reacts to a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to contain (x == \"xxx\") because we're checking how it reacts to a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_collection_doesnt_contain_values_according_to_predicate_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            const IEnumerable<string> strings = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => strings.Should().NotContain(x => x == "xxx", "because we're checking how it reacts to a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection not to contain (x == \"xxx\") because we're checking how it reacts to a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_a_collection_does_contain_the_combination_of_a_collection_and_a_single_item_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<object> strings = new[] { "string1", "string2" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().NotContain(new[] { "string3", "string4" }, new object[] { "string2" });

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection {\"string1\", \"string2\"} to not contain {\"string3\", \"string4\", \"string2\"}, but found {\"string2\"}.");
        }

        #endregion

        #region Only Contain (Predicate)

        [TestMethod]
        public void When_a_collection_contains_items_not_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 2, 12, 3, 11, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().OnlyContain(i => i <= 10, "10 is the maximum");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected collection to contain only items matching (i <= 10) because 10 is the maximum, but {12, 11} do(es) not match.");
        }

        [TestMethod]
        public void When_a_collection_is_empty_and_should_contain_only_items_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<string> strings = Enumerable.Empty<string>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().OnlyContain(e => e.Length > 0);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection to contain only items matching (e.Length > 0), but the collection is empty.");
        }

        [TestMethod]
        public void When_a_collection_contains_only_items_matching_a_predicate_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 2, 9, 3, 8, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().OnlyContain(i => i <= 10);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region Contain Single

        [TestMethod]
        public void When_a_collection_contains_a_single_item_matching_a_predicate_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2, 3 };
            Expression<Func<int, bool>> expression = (item => (item == 2));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle(expression);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_empty_collection_contains_a_single_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = Enumerable.Empty<int>();
            Expression<Func<int, bool>> expression = (item => (item == 2));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle(expression);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            string expectedMessage =
                string.Format("Expected collection to contain a single item matching {0}, " +
                              "but the collection is empty.", expression.Body);

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        [TestMethod]
        public void When_asserting_a_null_collection_contains_a_single_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            const IEnumerable<int> collection = null;
            Expression<Func<int, bool>> expression = (item => (item == 2));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle(expression);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            string expectedMessage =
                string.Format("Expected collection to contain a single item matching {0}, " +
                              "but found <null>.", expression.Body);

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        [TestMethod]
        public void When_non_empty_collection_does_not_contain_a_single_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 3 };
            Expression<Func<int, bool>> expression = (item => (item == 2));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle(expression);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            string expectedMessage =
                string.Format("Expected collection to contain a single item matching {0}, " +
                              "but no such item was found.", expression.Body);

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        [TestMethod]
        public void When_non_empty_collection_contains_more_than_a_single_item_matching_a_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2, 2, 2, 3 };
            Expression<Func<int, bool>> expression = (item => (item == 2));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle(expression);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            string expectedMessage =
                string.Format("Expected collection to contain a single item matching {0}, " +
                              "but 3 such items were found.", expression.Body);

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        [TestMethod]
        public void When_single_item_matching_a_predicate_is_found_it_should_allow_continuation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle(item => (item == 2)).Which.Should().BeGreaterThan(4);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage("Expected*greater*4*2*");
        }

        #endregion

        #region Contain Single

        [TestMethod]
        public void When_a_collection_contains_a_single_item_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_empty_collection_contains_a_single_item_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = Enumerable.Empty<int>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle("more is not allowed");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection to contain a single item because more is not allowed, but the collection is empty.");
        }

        [TestMethod]
        public void When_asserting_a_null_collection_contains_a_single_item_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            const IEnumerable<int> collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle("more is not allowed");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------

            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection to contain a single item because more is not allowed, but found <null>.");
        }

        [TestMethod]
        public void When_non_empty_collection_does_not_contain_a_single_item_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            const string expectedMessage = "Expected collection to contain a single item, but found {1, 3}.";

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        [TestMethod]
        public void When_non_empty_collection_contains_more_than_a_single_item_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 1, 2 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            const string expectedMessage = "Expected collection to contain a single item, but found {1, 2}.";

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        [TestMethod]
        public void When_single_item_is_found_it_should_allow_continuation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IEnumerable<int> collection = new[] { 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().ContainSingle().Which.Should().BeGreaterThan(4);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            const string expectedMessage = "Expected a value greater than 4, but found 3.";

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        #endregion

        #region Be In Ascending/Descending Order

        [TestMethod]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_specified_property_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text, "it should be sorted");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*a*b*c*");
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_given_comparer_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new[] { 2, 3, 1 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(Comparer<int>.Default, "it should be sorted");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection*2*3*1*ordered*should be sorted*1*2*3*");
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should be sorted");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*a*b*c*");
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_specified_property_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Numeric);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_given_comparer_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(Comparer<int>.Default);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_ascendingly_ordered_collection_are_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Numeric, Comparer<int>.Default);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_specified_property_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Text, "it should be sorted");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*c*b*a*");
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_given_comparer_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new[] { 1, 2, 3 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInDescendingOrder(Comparer<int>.Default, "it should be sorted");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection*1*2*3*ordered*should be sorted*3*2*1*");
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_unordered_collection_are_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new[]
            {
                new { Text = "b", Numeric = 1 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 3 }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase, "it should be sorted");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected collection*b*c*a*ordered*Text*should be sorted*c*b*a*");
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_specified_property_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new[]
            {
                new { Text = "b", Numeric = 3 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 1 }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Numeric);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_given_comparer_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new[] { 3, 2, 1 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInDescendingOrder(Comparer<int>.Default);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_descendingly_ordered_collection_are_ordered_descending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = new[]
            {
                new { Text = "b", Numeric = 3 },
                new { Text = "c", Numeric = 2 },
                new { Text = "a", Numeric = 1 }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInDescendingOrder(o => o.Numeric, Comparer<int>.Default);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_empty_collection_are_ordered_ascending_using_the_specified_property_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = Enumerable.Empty<SomeClass>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_empty_collection_are_ordered_ascending_using_the_given_comparer_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = Enumerable.Empty<SomeClass>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(Comparer<SomeClass>.Default);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_the_items_in_an_empty_collection_are_ordered_ascending_using_the_specified_property_and_the_given_comparer_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = Enumerable.Empty<SomeClass>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_the_items_in_a_collection_are_ordered_and_the_specified_property_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = Enumerable.Empty<SomeClass>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder((Expression<Func<SomeClass, string>>)null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentNullException>()
                .WithMessage("Cannot assert collection ordering without specifying a property*propertyExpression*");
        }

        [TestMethod]
        public void When_asserting_the_items_in_a_collection_are_ordered_and_the_given_comparer_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = Enumerable.Empty<SomeClass>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder((IComparer<SomeClass>)null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentNullException>()
                .WithMessage("Cannot assert collection ordering without specifying a comparer*comparer*");
        }

        [TestMethod]
        public void When_asserting_the_items_in_a_null_collection_are_ordered_using_the_specified_property_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            const IEnumerable<SomeClass> collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected*Text*found*null*");
        }

        [TestMethod]
        public void When_asserting_the_items_in_a_null_collection_are_ordered_using_the_given_comparer_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            const IEnumerable<SomeClass> collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(Comparer<SomeClass>.Default);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected*found*null*");
        }

        [TestMethod]
        public void When_asserting_the_items_in_a_null_collection_are_ordered_using_the_specified_property_and_the_given_comparer_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            const IEnumerable<SomeClass> collection = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(o => o.Text, StringComparer.OrdinalIgnoreCase);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected*Text*found*null*");
        }

        [TestMethod]
        public void When_asserting_the_items_in_ay_collection_are_ordered_using_an_invalid_property_expression_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var collection = Enumerable.Empty<SomeClass>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => collection.Should().BeInAscendingOrder(o => o.GetHashCode());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>()
                .WithMessage("Expression*o.GetHashCode()*cannot be used to select a member*");
        }

        private class SomeClass
        {
            public string Text { get; set; }
        }

        #endregion
    }
}