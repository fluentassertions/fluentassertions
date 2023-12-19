using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    public class Contain
    {
        [Fact]
        public void Should_succeed_when_asserting_dictionary_contains_single_key_value_pair()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>
            {
                new(1, "One")
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
        public void Should_succeed_when_asserting_dictionary_contains_multiple_key_value_pairs()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>
            {
                new(1, "One"),
                new(2, "Two")
            };

            // Act / Assert
            dictionary.Should().Contain(keyValuePairs);
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

            var keyValuePairs = new List<KeyValuePair<int, string>>
            {
                new(1, "One"),
                new(2, "Three")
            };

            // Act
            Action act = () => dictionary.Should().Contain(keyValuePairs, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain value \"Three\" at key 2 because we do, but found \"Two\".");
        }

        [Fact]
        public void
            When_a_dictionary_does_not_contain_multiple_values_for_key_value_pairs_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>
            {
                new(1, "Two"),
                new(2, "Three")
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

            var keyValuePairs = new List<KeyValuePair<int, string>>
            {
                new(3, "Three")
            };

            // Act
            Action act = () => dictionary.Should().Contain(keyValuePairs, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1] = \"One\", [2] = \"Two\"} to contain key 3 because we do.");
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

            var keyValuePairs = new List<KeyValuePair<int, string>>
            {
                new(1, "One"),
                new(3, "Three"),
                new(4, "Four")
            };

            // Act
            Action act = () => dictionary.Should().Contain(keyValuePairs, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1] = \"One\", [2] = \"Two\"} to contain key(s) {1, 3, 4} because we do, but could not find keys {3, 4}.");
        }

        [Fact]
        public void When_asserting_dictionary_contains_key_value_pairs_against_null_dictionary_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            List<KeyValuePair<int, string>> keyValuePairs = new()
            {
                new KeyValuePair<int, string>(1, "One"),
                new KeyValuePair<int, string>(1, "Two")
            };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                dictionary.Should().Contain(keyValuePairs, "because we want to test the behaviour with a null subject");
            };

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

            List<KeyValuePair<int, string>> keyValuePairs = new();

            // Act
            Action act = () => dictionary1.Should().Contain(keyValuePairs,
                "because we want to test the behaviour with an empty set of key/value pairs");

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
            Action act = () =>
                dictionary1.Should().Contain(keyValuePairs, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare dictionary with <null>.*")
                .WithParameterName("expected");
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
            var items = new List<KeyValuePair<int, string>>
            {
                new(1, "One"),
                new(2, "Two")
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

            var items = new List<KeyValuePair<int, string>>
            {
                new(1, "Two"),
                new(2, "Three")
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
            Action act = () =>
            {
                using var _ = new AssertionScope();
                dictionary.Should().Contain(1, "One", "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to contain value \"One\" at key 1 because we want to test the behaviour with a null subject, but dictionary is <null>.");
        }

        [Fact]
        public void When_a_dictionary_like_collection_contains_the_default_key_it_should_succeed()
        {
            // Arrange
            var subject = new List<KeyValuePair<int, int>>
                { new(0, 0) };

            // Act
            Action act = () => subject.Should().Contain(0, 0);

            // Assert
            act.Should().NotThrow();
        }
    }

    public class NotContain
    {
        [Fact]
        public void Should_succeed_when_asserting_dictionary_does_not_contain_single_key_value_pair()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>
            {
                new(3, "Three")
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
        public void
            Should_succeed_when_asserting_dictionary_does_not_contain_single_key_value_pair_with_existing_key_but_different_value()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>
            {
                new(1, "Two")
            };

            // Act / Assert
            dictionary.Should().NotContain(keyValuePairs);
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

            var keyValuePairs = new List<KeyValuePair<int, string>>
            {
                new(3, "Three"),
                new(4, "Four")
            };

            // Act / Assert
            dictionary.Should().NotContain(keyValuePairs);
        }

        [Fact]
        public void
            Should_succeed_when_asserting_dictionary_does_not_contain_multiple_key_value_pairs_with_existing_keys_but_different_values()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            var keyValuePairs = new List<KeyValuePair<int, string>>
            {
                new(1, "Three"),
                new(2, "Four")
            };

            // Act / Assert
            dictionary.Should().NotContain(keyValuePairs);
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

            var keyValuePairs = new List<KeyValuePair<int, string>>
            {
                new(1, "One")
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

            var keyValuePairs = new List<KeyValuePair<int, string>>
            {
                new(1, "One"),
                new(2, "Two")
            };

            // Act
            Action act = () => dictionary.Should().NotContain(keyValuePairs, "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to not contain key/value pairs {[1, One], [2, Two]} because we do, but found them anyhow.");
        }

        [Fact]
        public void When_asserting_dictionary_does_not_contain_key_value_pairs_against_null_dictionary_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            List<KeyValuePair<int, string>> keyValuePairs = new()
            {
                new KeyValuePair<int, string>(1, "One"),
                new KeyValuePair<int, string>(1, "Two")
            };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                dictionary.Should().NotContain(keyValuePairs, "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary to not contain key/value pairs {[1, One], [1, Two]} because we want to test the behaviour with a null subject, but dictionary is <null>.");
        }

        [Fact]
        public void
            When_asserting_dictionary_does_not_contain_key_value_pairs_but_expected_key_value_pairs_are_empty_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            List<KeyValuePair<int, string>> keyValuePair = new();

            // Act
            Action act = () => dictionary1.Should().NotContain(keyValuePair,
                "because we want to test the behaviour with an empty set of key/value pairs");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(
                "Cannot verify key containment against an empty collection of key/value pairs*");
        }

        [Fact]
        public void
            When_asserting_dictionary_does_not_contain_key_value_pairs_but_expected_key_value_pairs_are_null_it_should_throw()
        {
            // Arrange
            var dictionary1 = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            List<KeyValuePair<int, string>> keyValuePairs = null;

            // Act
            Action act = () =>
                dictionary1.Should().NotContain(keyValuePairs, "because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot compare dictionary with <null>.*")
                .WithParameterName("items");
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
            var items = new List<KeyValuePair<int, string>>
            {
                new(3, "Three"),
                new(4, "Four")
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
            var items = new List<KeyValuePair<int, string>>
            {
                new(1, "One"),
                new(2, "Two")
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
            Action act = () =>
            {
                using var _ = new AssertionScope();
                dictionary.Should().NotContain(1, "One", "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary not to contain value \"One\" at key 1 because we want to test the behaviour with a null subject, but dictionary is <null>.");
        }
    }
}
