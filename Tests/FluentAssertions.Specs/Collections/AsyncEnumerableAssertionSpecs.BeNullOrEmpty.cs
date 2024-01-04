using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <content>
/// The [Not]BeNullOrEmpty specs.
/// </content>
public partial class AsyncEnumerableAssertionSpecs
{
    public class BeNullOrEmpty
    {
        [Fact]
        public void
            When_asserting_a_null_collection_to_be_null_or_empty_it_should_succeed()
        {
            // Arrange
            IAsyncEnumerable<int> collection = null;

            // Act / Assert
            collection.Should().BeNullOrEmpty();
        }

        [Fact]
        public void
            When_asserting_an_empty_collection_to_be_null_or_empty_it_should_succeed()
        {
            // Arrange
            IEnumerable<int> collection = [];

            // Act / Assert
            collection.ToAsyncEnumerable().Should().BeNullOrEmpty();
        }

        [Fact]
        public void
            When_asserting_non_empty_collection_to_be_null_or_empty_it_should_throw()
        {
            // Arrange
            int[] array = [1, 2, 3];
            var collection = array.ToAsyncEnumerable();

            // Act
            Action act = () => collection.Should().BeNullOrEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to be null or empty because we want to test the failure message, but found {1, 2, 3}.");
        }

        [Fact]
        public void When_asserting_collection_to_be_null_or_empty_it_should_enumerate_only_once()
        {
            // Arrange
            var collection = new CountingGenericEnumerable<int>([]);

            // Act
            collection.ToAsyncEnumerable().Should().BeNullOrEmpty();

            // Assert
            collection.GetEnumeratorCallCount.Should().Be(1);
        }
    }

    public class NotBeNullOrEmpty
    {
        [Fact]
        public void
            When_asserting_non_empty_collection_to_not_be_null_or_empty_it_should_succeed()
        {
            // Arrange
            object[] collection = [new object()];

            // Act / Assert
            collection.ToAsyncEnumerable().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void
            When_asserting_null_collection_to_not_be_null_or_empty_it_should_throw()
        {
            // Arrange
            IAsyncEnumerable<int> collection = null;

            // Act
            Action act = () => collection.Should().NotBeNullOrEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_empty_collection_to_not_be_null_or_empty_it_should_throw()
        {
            // Arrange
            int[] collection = [];

            // Act
            Action act = () => collection.ToAsyncEnumerable().Should().NotBeNullOrEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_collection_to_not_be_null_or_empty_it_should_enumerate_only_once()
        {
            // Arrange
            var collection = new CountingGenericEnumerable<int>([42]);

            // Act
            collection.ToAsyncEnumerable().Should().NotBeNullOrEmpty();

            // Assert
            collection.GetEnumeratorCallCount.Should().Be(1);
        }
    }
}
