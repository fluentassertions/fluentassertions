using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

/// <content>
/// The HaveCountGreaterThanOrEqualTo specs.
/// </content>
public partial class AsyncEnumerableAssertionSpecs
{
    public class HaveCountGreaterThanOrEqualTo
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_greater_than_or_equal_to_less_the_number_of_items()
        {
            // Arrange
            int[] collection = [1, 2, 3];

            // Act / Assert
            collection.ToAsyncEnumerable().Should().HaveCountGreaterThanOrEqualTo(3);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_greater_than_or_equal_to_the_number_of_items()
        {
            // Arrange
            int[] collection = [1, 2, 3];

            // Act
            Action act = () => collection.ToAsyncEnumerable().Should().HaveCountGreaterThanOrEqualTo(4);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_collection_has_a_count_greater_than_or_equal_to_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            int[] array = [1, 2, 3];
            var collection = array.ToAsyncEnumerable();

            // Act
            Action action = () =>
                collection.Should().HaveCountGreaterThanOrEqualTo(4, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to contain at least 4 item(s) because we want to test the failure message, but found 3: {1, 2, 3}.");
        }

        [Fact]
        public void When_collection_count_is_greater_than_or_equal_to_and_collection_is_null_it_should_throw()
        {
            // Arrange
            IAsyncEnumerable<int> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().HaveCountGreaterThanOrEqualTo(1, "we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*at least*1*we want to test the behaviour with a null subject*found <null>*");
        }

        [Fact]
        public void Chaining_after_one_assertion()
        {
            // Arrange
            int[] collection = [1, 2, 3];

            // Act / Assert
            collection.ToAsyncEnumerable().Should().HaveCountGreaterThanOrEqualTo(3).And.Contain(1);
        }
    }
}
