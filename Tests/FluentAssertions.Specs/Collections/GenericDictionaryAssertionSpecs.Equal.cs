using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    public class Equal
    {
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
                "Expected dictionary1 to be equal to {[1] = \"One\", [22] = \"Two\"} because we want to test the failure message, but could not find keys {22}.");
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
                "Expected dictionary1 to be equal to {[1] = \"One\", [2] = \"Two\"} because we want to test the failure message, but found additional keys {3}.");
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
                "Expected dictionary1 to be equal to {[1] = \"One\", [2] = \"Three\"} because we want to test the failure message, but {[1] = \"One\", [2] = \"Two\"} differs at key 2.");
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
            Action act = () =>
            {
                using var _ = new AssertionScope();
                dictionary1.Should().Equal(dictionary2, "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary1 to be equal to {[1] = \"One\", [2] = \"Two\"} because we want to test the behaviour with a null subject, but found <null>.");
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
            Action act = () =>
                dictionary1.Should().Equal(dictionary2, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare dictionary with <null>.*")
                .WithParameterName("expected");
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
                "Expected dictionary1 to be equal to {[1] = \"One\", [2] = \"Two\"}, but could not find keys {1, 2}.");
        }
    }

    public class NotEqual
    {
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
                "Did not expect dictionaries {[1] = \"One\", [2] = \"Two\"} and {[1] = \"One\", [2] = \"Two\"} to be equal.");
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
                "Did not expect dictionaries {[1] = \"One\", [2] = \"Two\"} and {[1] = \"One\", [2] = \"Two\"} to be equal because we want to test the failure message.");
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
            Action act = () =>
            {
                using var _ = new AssertionScope();
                dictionary1.Should().NotEqual(dictionary2, "because we want to test the behaviour with a null subject");
            };

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
                .WithParameterName("unexpected");
        }

        [Fact]
        public void
            When_asserting_dictionaries_not_to_be_equal_subject_but_both_dictionaries_reference_the_same_object_it_should_throw()
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
    }
}
