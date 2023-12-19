using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class AllSatisfy
    {
        [Fact]
        public void All_items_satisfying_inspector_should_succeed()
        {
            // Arrange
            string[] collection = { "John", "John" };

            // Act / Assert
            collection.Should().AllSatisfy(value => value.Should().Be("John"));
        }

        [Fact]
        public void Any_items_not_satisfying_inspector_should_throw()
        {
            // Arrange
            string[] collection = { "Jack", "Jessica" };

            // Act
            Action act = () => collection.Should()
                .AllSatisfy(
                    value => value.Should().Be("John"),
                    "because we want to test the failure {0}",
                    "message");

            // Assert
            act.Should()
                .Throw<XunitException>()
                .WithMessage(
                    "Expected collection to contain only items satisfying the inspector because we want to test the failure message:"
                    + "*John*Jack"
                    + "*John*Jessica*");
        }
    }
}
