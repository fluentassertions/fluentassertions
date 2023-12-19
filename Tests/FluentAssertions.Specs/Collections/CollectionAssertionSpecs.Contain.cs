using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The [Not]Contain specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class Contain
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_contains_an_item_from_the_collection()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().Contain(1);
        }

        [Fact]
        public void Should_succeed_when_asserting_collection_contains_multiple_items_from_the_collection_in_any_order()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().Contain(new[] { 2, 1 });
        }

        [Fact]
        public void When_a_collection_does_not_contain_single_item_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

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
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().Contain(1, "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_a_collection_does_not_contain_another_collection_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Contain(new[] { 3, 4, 5 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to contain {3, 4, 5} because we do, but could not find {4, 5}.");
        }

        [Fact]
        public void When_a_collection_does_not_contain_a_single_element_collection_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Contain(new[] { 4 }, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to contain 4 because we do.");
        }

        [Fact]
        public void
            When_a_collection_does_not_contain_other_collection_with_assertion_scope_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().Contain(new[] { 4 }).And.Contain(new[] { 5, 6 });
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*to contain 4*to contain {5, 6}*");
        }

        [Fact]
        public void When_the_contents_of_a_collection_are_checked_against_an_empty_collection_it_should_throw_clear_explanation()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Contain(new int[0]);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify containment against an empty collection*");
        }

        [Fact]
        public void When_asserting_collection_does_contain_a_list_of_items_against_null_collection_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().Contain(new[] { 1, 2 }, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection to contain {1, 2} *failure message*, but found <null>.");
        }

        [Fact]
        public void When_injecting_a_null_predicate_into_Contain_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new int[] { };

            // Act
            Action act = () => collection.Should().Contain(predicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("predicate");
        }

        [Fact]
        public void When_collection_does_not_contain_an_expected_item_matching_a_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Contain(item => item > 3, "at least {0} item should be larger than 3", 1);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to have an item matching (item > 3) because at least 1 item should be larger than 3.");
        }

        [Fact]
        public void When_collection_does_contain_an_expected_item_matching_a_predicate_it_should_allow_chaining_it()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().Contain(item => item == 2).Which.Should().BeGreaterThan(4);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*greater*4*2*");
        }

        [Fact]
        public void When_collection_does_contain_an_expected_item_matching_a_predicate_it_should_not_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().Contain(item => item == 2);
        }

        [Fact]
        public void When_a_collection_of_strings_contains_the_expected_string_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> strings = new[] { "string1", "string2", "string3" };

            // Act / Assert
            strings.Should().Contain("string2");
        }

        [Fact]
        public void When_a_collection_of_strings_does_not_contain_the_expected_string_it_should_throw()
        {
            // Arrange
            IEnumerable<string> strings = new[] { "string1", "string2", "string3" };

            // Act
            Action act = () => strings.Should().Contain("string4", "because {0} is required", "4");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected strings {\"string1\", \"string2\", \"string3\"} to contain \"string4\" because 4 is required.");
        }

        [Fact]
        public void When_asserting_collection_contains_some_values_but_collection_is_null_it_should_throw()
        {
            // Arrange
            const IEnumerable<string> strings = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                strings.Should().Contain("string4", "because we're checking how it reacts to a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected strings to contain \"string4\" because we're checking how it reacts to a null subject, but found <null>.");
        }

        [Fact]
        public void When_the_multiple_matching_objects_exists_it_continuation_using_the_matched_value_should_fail()
        {
            // Arrange
            DateTime now = DateTime.Now;

            IEnumerable<DateTime> collection = new[] { now, DateTime.SpecifyKind(now, DateTimeKind.Unspecified) };

            // Act
            Action act = () => collection.Should().Contain(now).Which.Kind.Should().Be(DateTimeKind.Local);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_collection_contains_values_according_to_predicate_but_collection_is_null_it_should_throw()
        {
            // Arrange
            const IEnumerable<string> strings = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                strings.Should().Contain(x => x == "xxx", "because we're checking how it reacts to a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected strings to contain (x == \"xxx\") because we're checking how it reacts to a null subject, but found <null>.");
        }
    }

    public class NotContain
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_does_not_contain_an_item_that_is_not_in_the_collection()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

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
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotContain(1, "because we {0} like it, but found it anyhow", "don't");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to not contain 1 because we don't like it, but found it anyhow.");
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
                .WithParameterName("predicate");
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
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotContain(1, "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to not contain 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_collection_contains_unexpected_item_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should()
                .NotContain(new[] { 2 }, "because we {0} like them", "don't");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to not contain 2 because we don't like them.");
        }

        [Fact]
        public void When_collection_contains_unexpected_items_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should()
                .NotContain(new[] { 1, 2, 4 }, "because we {0} like them", "don't");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection {1, 2, 3} to not contain {1, 2, 4} because we don't like them, but found {1, 2}.");
        }

        [Fact]
        public void When_asserting_multiple_collection_in_assertion_scope_all_should_be_reported()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotContain(new[] { 1, 2 }).And.NotContain(new[] { 3 });
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*to not contain {1, 2}*to not contain 3*");
        }

        [Fact]
        public void When_asserting_collection_to_not_contain_an_empty_collection_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().NotContain(Array.Empty<int>());

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Cannot verify*");
        }

        [Fact]
        public void When_asserting_collection_does_not_contain_predicate_item_against_null_collection_it_should_fail()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotContain(item => item == 4, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection not to contain (item == 4) *failure message*, but found <null>.");
        }

        [Fact]
        public void When_asserting_collection_does_not_contain_a_list_of_items_against_null_collection_it_should_fail()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotContain(new[] { 1, 2, 4 }, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection to not contain {1, 2, 4} *failure message*, but found <null>.");
        }

        [Fact]
        public void
            When_asserting_collection_doesnt_contain_values_according_to_predicate_but_collection_is_null_it_should_throw()
        {
            // Arrange
            const IEnumerable<string> strings = null;

            // Act
            Action act =
                () => strings.Should().NotContain(x => x == "xxx", "because we're checking how it reacts to a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected strings not to contain (x == \"xxx\") because we're checking how it reacts to a null subject, but found <null>.");
        }

        [Fact]
        public void When_a_collection_does_not_contain_the_expected_item_it_should_not_be_enumerated_twice()
        {
            // Arrange
            var collection = new OneTimeEnumerable<int>(1, 2, 3);

            // Act
            Action act = () => collection.Should().Contain(4);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection*to contain 4.");
        }

        [Fact]
        public void When_a_collection_contains_the_unexpected_item_it_should_not_be_enumerated_twice()
        {
            // Arrange
            var collection = new OneTimeEnumerable<int>(1, 2, 3);

            // Act
            Action act = () => collection.Should().NotContain(2);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection*to not contain 2.");
        }
    }
}
