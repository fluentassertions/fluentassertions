using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The [Not]BeEmpty specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class BeEmpty
    {
        [Fact]
        public void When_collection_is_empty_as_expected_it_should_not_throw()
        {
            // Arrange
            var collection = new int[0];

            // Act / Assert
            collection.Should().BeEmpty();
        }

        [Fact]
        public void When_collection_is_not_empty_unexpectedly_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

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
            var collection = new[] { 1, 2, 3 };

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
            var collection = new int[0];

            // Act
            Action act = () => collection.Should().NotBeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_collection_without_items_is_not_empty_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var collection = new int[0];

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
            IEnumerable<object> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().BeEmpty("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection to be empty *failure message*, but found <null>.");
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

        [Fact]
        public void When_asserting_collection_to_not_be_empty_but_collection_is_null_it_should_throw()
        {
            // Arrange
            IEnumerable<object> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().NotBeEmpty("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection not to be empty *failure message*, but found <null>.");
        }

        [Fact]
        public void When_asserting_an_infinite_collection_to_be_empty_it_should_throw_correctly()
        {
            // Arrange
            var collection = new InfiniteEnumerable();

            // Act
            Action act = () => collection.Should().BeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }
    }

    public class NotBeEmpty
    {
        [Fact]
        public void When_asserting_collection_to_be_not_empty_but_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = null;

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
    }

    private sealed class InfiniteEnumerable : IEnumerable<object>
    {
        public IEnumerator<object> GetEnumerator() => new InfiniteEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class InfiniteEnumerator : IEnumerator<object>
    {
        public bool MoveNext() => true;

        public void Reset() { }

        public object Current => new();

        object IEnumerator.Current => Current;

        public void Dispose() { }
    }
}
