using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The [Not]BeNullOrEmpty specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class BeNullOrEmpty
    {
        [Fact]
        public void
            When_asserting_a_null_collection_to_be_null_or_empty_it_should_succeed()
        {
            // Arrange
            int[] collection = null;

            // Act / Assert
            collection.Should().BeNullOrEmpty();
        }

        [Fact]
        public void
            When_asserting_an_empty_collection_to_be_null_or_empty_it_should_succeed()
        {
            // Arrange
            var collection = new int[0];

            // Act / Assert
            collection.Should().BeNullOrEmpty();
        }

        [Fact]
        public void
            When_asserting_non_empty_collection_to_be_null_or_empty_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

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
            var collection = new CountingGenericEnumerable<int>(new int[0]);

            // Act
            collection.Should().BeNullOrEmpty();

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
            var collection = new[] { new object() };

            // Act / Assert
            collection.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void
            When_asserting_null_collection_to_not_be_null_or_empty_it_should_throw()
        {
            // Arrange
            int[] collection = null;

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
            var collection = new int[0];

            // Act
            Action act = () => collection.Should().NotBeNullOrEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_collection_to_not_be_null_or_empty_it_should_enumerate_only_once()
        {
            // Arrange
            var collection = new CountingGenericEnumerable<int>(new[] { 42 });

            // Act
            collection.Should().NotBeNullOrEmpty();

            // Assert
            collection.GetEnumeratorCallCount.Should().Be(1);
        }
    }
}
