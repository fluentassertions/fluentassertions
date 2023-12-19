using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The NotContainNulls specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class NotContainNulls
    {
        [Fact]
        public void When_collection_does_not_contain_nulls_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().NotContainNulls();
        }

        [Fact]
        public void When_collection_contains_nulls_that_are_unexpected_it_should_throw()
        {
            // Arrange
            var collection = new[] { new object(), null };

            // Act
            Action act = () => collection.Should().NotContainNulls("because they are {0}", "evil");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s because they are evil, but found one at index 1.");
        }

        [Fact]
        public void When_collection_contains_nulls_that_are_unexpected_it_supports_chaining()
        {
            // Arrange
            var collection = new[] { new object(), null };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotContainNulls().And.HaveCount(c => c > 1);
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*but found one*");
        }

        [Fact]
        public void When_collection_contains_multiple_nulls_that_are_unexpected_it_should_throw()
        {
            // Arrange
            var collection = new[] { new object(), null, new object(), null };

            // Act
            Action act = () => collection.Should().NotContainNulls("because they are {0}", "evil");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s*because they are evil*{1, 3}*");
        }

        [Fact]
        public void When_collection_contains_multiple_nulls_that_are_unexpected_it_supports_chaining()
        {
            // Arrange
            var collection = new[] { new object(), null, new object(), null };

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotContainNulls().And.HaveCount(c => c > 1);
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*but found several*");
        }

        [Fact]
        public void When_asserting_collection_to_not_contain_nulls_but_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () => collection.Should().NotContainNulls("because we want to test the behaviour with a null subject");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s because we want to test the behaviour with a null subject, but collection is <null>.");
        }

        [Fact]
        public void When_injecting_a_null_predicate_into_NotContainNulls_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new SomeClass[] { };

            // Act
            Action act = () => collection.Should().NotContainNulls<string>(predicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("predicate");
        }

        [Fact]
        public void When_collection_does_not_contain_nulls_with_a_predicate_it_should_not_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "one" },
                new SomeClass { Text = "two" },
                new SomeClass { Text = "three" }
            };

            // Act / Assert
            collection.Should().NotContainNulls(e => e.Text);
        }

        [Fact]
        public void When_collection_contains_nulls_that_are_unexpected_with_a_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "" },
                new SomeClass { Text = null }
            };

            // Act
            Action act = () => collection.Should().NotContainNulls(e => e.Text, "because they are {0}", "evil");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s*on e.Text*because they are evil*Text = <null>*");
        }

        [Fact]
        public void When_collection_contains_multiple_nulls_that_are_unexpected_with_a_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = new[]
            {
                new SomeClass { Text = "" },
                new SomeClass { Text = null },
                new SomeClass { Text = "" },
                new SomeClass { Text = null }
            };

            // Act
            Action act = () => collection.Should().NotContainNulls(e => e.Text, "because they are {0}", "evil");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s*on e.Text*because they are evil*Text = <null>*Text = <null>*");
        }

        [Fact]
        public void When_asserting_collection_to_not_contain_nulls_but_collection_is_null_it_should_fail()
        {
            // Arrange
            SomeClass[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotContainNulls("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s *failure message*, but collection is <null>.");
        }

        [Fact]
        public void When_asserting_collection_to_not_contain_nulls_with_predicate_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<SomeClass> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotContainNulls(e => e.Text, "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection not to contain <null>s because we want to test the behaviour with a null subject, but collection is <null>.");
        }
    }
}
