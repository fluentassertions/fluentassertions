using System;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The HaveCountLessThan specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class HaveCountLessThan
    {
        [Fact]
        public void Should_succeed_when_asserting_collection_has_a_count_less_than_less_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveCountLessThan(4);
        }

        [Fact]
        public void Should_fail_when_asserting_collection_has_a_count_less_than_the_number_of_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveCountLessThan(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_collection_has_a_count_less_than_the_number_of_items_it_should_fail_with_descriptive_message_()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action action = () => collection.Should().HaveCountLessThan(3, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to contain fewer than 3 item(s) because we want to test the failure message, but found 3: {1, 2, 3}.");
        }

        [Fact]
        public void When_collection_count_is_less_than_and_collection_is_null_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().HaveCountLessThan(1, "we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*fewer than*1*we want to test the behaviour with a null subject*found <null>*");
        }
    }
}
