using System;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The HaveElementAt specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class HaveElementAt
    {
        [Fact]
        public void When_collection_has_expected_element_at_specific_index_it_should_not_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().HaveElementAt(1, 2);
        }

        [Fact]
        public void When_collection_does_not_have_the_expected_element_at_specific_index_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveElementAt(1, 3, "we put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected 3 at index 1 because we put it there, but found 2.");
        }

        [Fact]
        public void When_collection_does_not_have_an_element_at_the_specific_index_it_should_throw()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            Action act = () => collection.Should().HaveElementAt(4, 3, "we put it {0}", "there");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected 3 at index 4 because we put it there, but found no element.");
        }

        [Fact]
        public void When_asserting_collection_has_element_at_specific_index_against_null_collection_it_should_throw()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().HaveElementAt(1, 1, "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to have element at index 1 because we want to test the behaviour with a null subject, but found <null>.");
        }

        [Fact]
        public void When_element_at_specific_index_was_found_it_should_allow_chaining()
        {
            // Arrange
            var expectedElement = new
            {
                SomeProperty = "hello"
            };

            var collection = new[]
            {
                expectedElement
            };

            // Act
            Action act = () => collection.Should()
                .HaveElementAt(0, expectedElement)
                .Which
                .Should().BeAssignableTo<string>();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*assignable*string*");
        }
    }
}
