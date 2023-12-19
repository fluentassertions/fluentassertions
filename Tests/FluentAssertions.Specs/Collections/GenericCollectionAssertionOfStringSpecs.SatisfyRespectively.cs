using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class SatisfyRespectively
    {
        [Fact]
        public void When_string_collection_satisfies_all_inspectors_it_should_succeed()
        {
            // Arrange
            string[] collection = { "John", "Jane" };

            // Act / Assert
            collection.Should().SatisfyRespectively(
                value => value.Should().Be("John"),
                value => value.Should().Be("Jane")
            );
        }

        [Fact]
        public void When_string_collection_does_not_satisfy_all_inspectors_it_should_throw()
        {
            // Arrange
            string[] collection = { "Jack", "Jessica" };

            // Act
            Action act = () => collection.Should().SatisfyRespectively(new Action<string>[]
            {
                value => value.Should().Be("John"),
                value => value.Should().Be("Jane")
            }, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to satisfy all inspectors because we want to test the failure message, but some inspectors are not satisfied"
                + "*John*Jack"
                + "*Jane*Jessica*");
        }
    }
}
