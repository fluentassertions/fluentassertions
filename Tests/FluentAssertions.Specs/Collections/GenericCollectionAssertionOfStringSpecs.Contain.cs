using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class Contain
    {
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
        public void When_the_contents_of_a_collection_are_checked_against_an_empty_collection_it_should_throw_clear_explanation()
        {
            // Arrange
            IEnumerable<string> collection = new[] { "one", "two", "three" };

            // Act
            Action act = () => collection.Should().Contain(new string[0]);

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
    }

    public class NotContain
    {
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
    }
}
