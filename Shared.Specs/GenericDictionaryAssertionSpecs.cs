using System;
using System.Collections;
using System.Collections.Generic;

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System.Linq;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class GenericDictionaryAssertionSpecs
    {
        #region Be Null

        [TestMethod]
        public void When_dictionary_is_expected_to_be_null_and_it_is_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IDictionary<int, string> someDictionary = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            someDictionary.Should().BeNull();
        }

        [TestMethod]
        public void When_a_custom_dictionary_implementation_is_expected_not_to_be_null_and_it_is_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new TrackingTestDictionary();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            dictionary.Should().NotBeNull();
        }

        [TestMethod]
        public void When_dictionary_is_expected_to_be_null_and_it_isnt_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var someDictionary = new Dictionary<int, string>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someDictionary.Should().BeNull("because {0} is valid", "null");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary to be <null> because null is valid, but found {empty}.");
        }

        [TestMethod]
        public void When_dictionary_is_not_expected_to_be_null_and_it_isnt_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IDictionary<int, string> someDictionary = new Dictionary<int, string>();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            someDictionary.Should().NotBeNull();
        }

        [TestMethod]
        public void When_dictionary_is_not_expected_to_be_null_and_it_is_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IDictionary<int, string> someDictionary = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someDictionary.Should().NotBeNull("because {0} should not", "someDictionary");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary not to be <null> because someDictionary should not.");
        }

        #endregion

        #region Have Count

        [TestMethod]
        public void Should_succeed_when_asserting_dictionary_has_a_count_that_equals_the_number_of_items()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" }
            };
            dictionary.Should().HaveCount(3);
        }

        [TestMethod]
        public void Should_fail_when_asserting_dictionary_has_a_count_that_is_different_from_the_number_of_items()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" }
            };

            Action act = () => dictionary.Should().HaveCount(4);
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void
            When_dictionary_has_a_count_that_is_different_from_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => dictionary.Should().HaveCount(4, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected dictionary {[1, One], [2, Two], [3, Three]} to have 4 item(s) because we want to test the failure message, but found 3.");
        }

        [TestMethod]
        public void When_dictionary_has_a_count_larger_than_the_minimum_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            dictionary.Should().HaveCount(c => c >= 3);
        }

        [TestMethod]
        public void When_dictionary_has_a_count_that_not_matches_the_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().HaveCount(c => c >= 4, "a minimum of 4 is required");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two], [3, Three]} to have a count (c >= 4) because a minimum of 4 is required, but count is 3.");
        }

        [TestMethod]
        public void When_dictionary_count_is_matched_against_a_null_predicate_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().HaveCount(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage(
                "Cannot compare dictionary count against a <null> predicate.");
        }

        [TestMethod]
        public void When_dictionary_count_is_matched_and_dictionary_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Dictionary<int, string> dictionary = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().HaveCount(1, "we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected 1 item(s) because we want to test the behaviour with a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_dictionary_count_is_matched_against_a_predicate_and_dictionary_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Dictionary<int, string> dictionary = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().HaveCount(c => c < 3, "we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary to have (c < 3) items because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Be Empty

        [TestMethod]
        public void Should_succeed_when_asserting_dictionary_without_items_is_empty()
        {
            var dictionary = new Dictionary<int, string>();
            dictionary.Should().BeEmpty();
        }

        [TestMethod]
        public void Should_fail_when_asserting_dictionary_with_items_is_empty()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" }
            };

            Action act = () => dictionary.Should().BeEmpty();
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_dictionary_with_items_is_empty()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" }
            };
            var assertions = dictionary.Should();
            assertions.Invoking(x => x.BeEmpty("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected dictionary to not have any items because we want to test the failure message, but found 1.");
        }

        [TestMethod]
        public void When_asserting_dictionary_with_items_is_not_empty_it_should_succeed()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" }
            };
            dictionary.Should().NotBeEmpty();
        }

        [TestMethod]
        public void When_asserting_dictionary_with_items_is_not_empty_it_should_enumerate_the_dictionary_only_once()
        {
            var trackingDictionary = new TrackingTestDictionary(new KeyValuePair<int, string>(1, "One"));
            trackingDictionary.Should().NotBeEmpty();

            trackingDictionary.Enumerator.LoopCount.Should().Be(1);
        }

        [TestMethod]
        public void When_asserting_dictionary_without_items_is_not_empty_it_should_fail()
        {
            var dictionary = new Dictionary<int, string>();

            Action act = () => dictionary.Should().NotBeEmpty();
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void When_asserting_dictionary_without_items_is_not_empty_it_should_fail_with_descriptive_message_()
        {
            var dictionary = new Dictionary<int, string>();
            var assertions = dictionary.Should();
            assertions.Invoking(x => x.NotBeEmpty("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected one or more items because we want to test the failure message, but found none.");
        }

        [TestMethod]
        public void When_asserting_dictionary_to_be_empty_but_dictionary_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Dictionary<int, string> dictionary = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().BeEmpty("because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_dictionary_to_be_not_empty_but_dictionary_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Dictionary<int, string> dictionary = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().NotBeEmpty("because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary not to be empty because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Equal

        [TestMethod]
        public void Should_succeed_when_asserting_dictionary_is_equal_to_the_same_dictionary()
        {
            var dictionary1 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            var dictionary2 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            dictionary1.Should().Equal(dictionary2);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_dictionary_with_null_value_is_equal_to_the_same_dictionary()
        {
            var dictionary1 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, null }
            };
            var dictionary2 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, null }
            };
            dictionary1.Should().Equal(dictionary2);
        }

        [TestMethod]
        public void When_asserting_dictionaries_to_be_equal_but_subject_dictionary_misses_a_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            var dictionary2 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 22, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.Should().Equal(dictionary2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary to be equal to {[1, One], [22, Two]} because we want to test the failure message, but could not find keys {22}.");
        }

        [TestMethod]
        public void When_asserting_dictionaries_to_be_equal_but_subject_dictionary_has_extra_key_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" }
            };
            var dictionary2 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.Should().Equal(dictionary2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary to be equal to {[1, One], [2, Two]} because we want to test the failure message, but found additional keys {3}.");
        }

        [TestMethod]
        public void When_two_dictionaries_are_not_equal_by_values_it_should_throw_using_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            var dictionary2 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Three" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.Should().Equal(dictionary2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary to be equal to {[1, One], [2, Three]} because we want to test the failure message, but {[1, One], [2, Two]} differs at key 2.");
        }

        [TestMethod]
        public void When_asserting_dictionaries_to_be_equal_but_subject_dictionary_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Dictionary<int, string> dictionary1 = null;
            var dictionary2 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.Should().Equal(dictionary2, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary to be equal to {[1, One], [2, Two]} because we want to test the behaviour with a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_dictionaries_to_be_equal_but_expected_dictionary_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            Dictionary<int, string> dictionary2 = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.Should().Equal(dictionary2, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentNullException>().WithMessage(
                "Cannot compare dictionary with <null>.\r\nParameter name: expected");
        }

        [TestMethod]
        public void When_an_empty_dictionary_is_compared_for_equality_to_a_non_empty_dictionary_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<int, string>();
            var dictionary2 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.Should().Equal(dictionary2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary to be equal to {[1, One], [2, Two]}, but could not find keys {1, 2}.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_dictionary_is_not_equal_to_a_dictionary_with_different_key()
        {
            var dictionary1 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            var dictionary2 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 22, "Two" }
            };

            dictionary1.Should().NotEqual(dictionary2);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_dictionary_is_not_equal_to_a_dictionary_with_different_value()
        {
            var dictionary1 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, null }
            };
            var dictionary2 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            dictionary1.Should().NotEqual(dictionary2);
        }

        [TestMethod]
        public void When_two_equal_dictionaries_are_not_expected_to_be_equal_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            var dictionary2 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.Should().NotEqual(dictionary2);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Did not expect dictionaries {[1, One], [2, Two]} and {[1, One], [2, Two]} to be equal.");
        }

        [TestMethod]
        public void When_two_equal_dictionaries_are_not_expected_to_be_equal_it_should_report_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            var dictionary2 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary1.Should().NotEqual(dictionary2, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Did not expect dictionaries {[1, One], [2, Two]} and {[1, One], [2, Two]} to be equal because we want to test the failure message.");
        }


        [TestMethod]
        public void When_asserting_dictionaries_not_to_be_equal_subject_but_dictionary_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Dictionary<int, string> dictionary1 = null;
            var dictionary2 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => dictionary1.Should().NotEqual(dictionary2, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionaries not to be equal because we want to test the behaviour with a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_dictionaries_not_to_be_equal_but_expected_dictionary_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary1 = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            Dictionary<int, string> dictionary2 = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => dictionary1.Should().NotEqual(dictionary2, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentNullException>().WithMessage(
                "Cannot compare dictionary with <null>.\r\nParameter name: unexpected");
        }

        #endregion

        #region ContainKey

        [TestMethod]
        public void Should_succeed_when_asserting_dictionary_contains_a_key_from_the_dictionary()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            dictionary.Should().ContainKey(1);
        }
        
        [TestMethod]
        public void When_a_dictionary_has_custom_equality_comparer_the_contains_key_assertion_should_work_accordingly()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "One", "One" },
                { "Two", "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------


            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            dictionary.Should().ContainKey("One");
            dictionary.Should().ContainKey("ONE");
            dictionary.Should().ContainKey("one");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_dictionary_contains_multiple_keys_from_the_dictionary()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            dictionary.Should().ContainKeys(2, 1);
        }

        [TestMethod]
        public void When_a_dictionary_does_not_contain_single_key_it_should_throw_with_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().ContainKey(3, "because {0}", "we do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to contain key 3 because we do.");
        }

        [TestMethod]
        public void When_the_requested_key_exists_it_should_allow_continuation_with_the_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<string, MyClass>
            {
                { "Key", new MyClass { SomeProperty = 3} },
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().ContainKey("Key").WhichValue.Should().Be(4);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage("Expected*4*3*.");
        }

        public class MyClass
        {
            public int SomeProperty { get; set; }
        }

        [TestMethod]
        public void When_a_dictionary_does_not_contain_a_list_of_keys_it_should_throw_with_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().ContainKeys(new[] { 2, 3 }, "because {0}", "we do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to contain key {2, 3} because we do, but could not find {3}.");
        }

        [TestMethod]
        public void When_the_contents_of_a_dictionary_are_checked_against_an_empty_list_of_keys_it_should_throw_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().ContainKeys(new int[0]);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>().WithMessage(
                "Cannot verify key containment against an empty dictionary");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_dictionary_does_not_contain_a_key_that_is_not_in_the_dictionary()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            dictionary.Should().NotContainKey(4);
        }

        [TestMethod]
        public void When_dictionary_contains_an_unexpected_key_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().NotContainKey(1, "because we {0} like it", "don't");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Dictionary {[1, One], [2, Two]} should not contain key 1 because we don't like it, but found it anyhow.");
        }

        [TestMethod]
        public void When_asserting_dictionary_does_not_contain_key_against_null_dictionary_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Dictionary<int, string> dictionary = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should()
                .NotContainKey(1, "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary not to contain key 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region ContainValue

        [TestMethod]
        public void When_dictionary_contains_expected_value_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().ContainValue("One");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_dictionary_contains_expected_null_value_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, null }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().ContainValue(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_the_specified_value_exists_it_should_allow_continuation_using_that_value()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var myClass = new MyClass()
            {
                SomeProperty = 0
            };
            
            var dictionary = new Dictionary<int, MyClass>
            {
                { 1, myClass }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().ContainValue(myClass).Which.SomeProperty.Should().BeGreaterThan(0);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage("Expected*greater*0*0*");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_dictionary_contains_multiple_values_from_the_dictionary()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            dictionary.Should().ContainValues("Two", "One");
        }

        [TestMethod]
        public void When_a_dictionary_does_not_contain_single_value_it_should_throw_with_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().ContainValue("Three", "because {0}", "we do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to contain value \"Three\" because we do.");
        }

        [TestMethod] public void When_a_dictionary_does_not_contain_a_number_of_values_it_should_throw_with_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().ContainValues(new[] { "Two", "Three" }, "because {0}", "we do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary {[1, One], [2, Two]} to contain value {\"Two\", \"Three\"} because we do, but could not find {\"Three\"}.");
        }

        [TestMethod]
        public void When_the_contents_of_a_dictionary_are_checked_against_an_empty_list_of_values_it_should_throw_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().ContainValues(new string[0]);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>().WithMessage(
                "Cannot verify value containment against an empty dictionary");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_dictionary_does_not_contain_a_value_that_is_not_in_the_dictionary()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };
            dictionary.Should().NotContainValue("Three");
        }

        [TestMethod]
        public void When_dictionary_contains_an_unexpected_value_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().NotContainValue("One", "because we {0} like it", "don't");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Dictionary {[1, One], [2, Two]} should not contain value \"One\" because we don't like it, but found it anyhow.");
        }

        [TestMethod]
        public void When_asserting_dictionary_does_not_contain_value_against_null_dictionary_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Dictionary<int, string> dictionary = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should()
                .NotContainValue("One", "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary not to contain value \"One\" because we want to test the behaviour with a null subject, but found <null>.");
        }

        #endregion

        #region Contain

        [TestMethod]
        public void When_dictionary_contains_expected_value_at_specific_key_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            dictionary.Should().Contain(1, "One");
        }

        [TestMethod]
        public void When_dictionary_contains_expected_null_at_specific_key_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, null }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            dictionary.Should().Contain(1, null);
        }

        [TestMethod]
        public void When_dictionary_contains_expected_key_value_pair_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            var item = new KeyValuePair<int, string>(1, "One");
            dictionary.Should().Contain(item);
        }

        [TestMethod]
        public void When_dictionary_does_not_contain_the_expected_value_at_specific_key_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var item = new KeyValuePair<int, string>(1, "Two");
            Action act = () => dictionary.Should().Contain(item, "we put it {0}", "there");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary to contain value \"Two\" at key 1 because we put it there, but found \"One\".");
        }

        [TestMethod]
        public void When_dictionary_does_not_contain_the_key_value_pair_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().Contain(1, "Two", "we put it {0}", "there");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary to contain value \"Two\" at key 1 because we put it there, but found \"One\".");
        }

        [TestMethod]
        public void When_dictionary_does_not_contain_an_value_at_the_specific_key_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().Contain(3, "Two", "we put it {0}", "there");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------            
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary to contain value \"Two\" at key 3 because we put it there, but the key was not found.");
        }

        [TestMethod]
        public void When_asserting_dictionary_contains_value_at_specific_key_against_null_dictionary_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Dictionary<int, string> dictionary = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().Contain(1, "One",
                "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary to contain value \"One\" at key 1 because we want to test the behaviour with a null subject, but dictionary is <null>.");
        }

        [TestMethod]
        public void When_dictionary_does_not_contain_unexpected_value_or_key_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            dictionary.Should().NotContain(3, "Three");
        }

        [TestMethod]
        public void When_dictionary_does_not_contain_unexpected_value_at_existing_key_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            dictionary.Should().NotContain(2, "Three");
        }

        [TestMethod]
        public void When_dictionary_does_not_have_the_unexpected_value_but_null_at_existing_key_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, null },
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => dictionary.Should().NotContain(1, "other");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_dictionary_does_not_contain_unexpected_key_value_pair_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            var item = new KeyValuePair<int, string>(3, "Three");
            dictionary.Should().NotContain(item);
        }

        [TestMethod]
        public void When_dictionary_contains_the_unexpected_value_at_specific_key_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var item = new KeyValuePair<int, string>(1, "One");
            Action act = () => dictionary.Should().NotContain(item, "we put it {0}", "there");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary not to contain value \"One\" at key 1 because we put it there, but found it anyhow.");
        }

        [TestMethod]
        public void When_dictionary_contains_the_key_value_pair_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().NotContain(1, "One", "we did not put it {0}", "there");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary not to contain value \"One\" at key 1 because we did not put it there, but found it anyhow.");
        }

        [TestMethod]
        public void When_asserting_dictionary_does_not_contain_value_at_specific_key_against_null_dictionary_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Dictionary<int, string> dictionary = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => dictionary.Should().NotContain(1, "One",
                "because we want to test the behaviour with a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected dictionary not to contain value \"One\" at key 1 because we want to test the behaviour with a null subject, but dictionary is <null>.");
        }

        #endregion

        #region Miscellaneous

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            };

            dictionary.Should()
                .HaveCount(2)
                .And.ContainKey(1)
                .And.ContainValue("Two");
        }

        #endregion
    }

    internal class TrackingTestDictionary : IDictionary<int, string>
    {
        private readonly TrackingDictionaryEnumerator enumerator;
        private readonly IDictionary<int, string> entries;

        public TrackingTestDictionary(params KeyValuePair<int, string>[] entries)
        {
            this.entries = entries.ToDictionary(e => e.Key, e => e.Value);
            enumerator = new TrackingDictionaryEnumerator(entries);
        }

        public TrackingDictionaryEnumerator Enumerator
        {
            get { return enumerator; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
        {
            enumerator.IncreaseEnumerationCount();
            enumerator.Reset();
            return enumerator;
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
}