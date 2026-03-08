using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections;

public partial class GenericCollectionAssertionOfStringSpecs
{
    public class BeEqualTo
    {
        [Fact]
        public void Succeeds_for_collections_with_same_elements_in_same_order()
        {
            // Arrange
            IEnumerable<string> collection1 = ["one", "two", "three"];
            IEnumerable<string> collection2 = ["one", "two", "three"];

            // Act / Assert
            collection1.Should().BeEqualTo(collection2);
        }

        [Fact]
        public void Succeeds_for_params_with_same_elements_in_same_order()
        {
            // Arrange
            IEnumerable<string> collection = ["one", "two", "three"];

            // Act / Assert
            collection.Should().BeEqualTo("one", "two", "three");
        }

        [Fact]
        public void Fails_for_params_with_a_differing_element()
        {
            // Arrange
            IEnumerable<string> collection = ["one", "two", "three"];

            // Act
            System.Action act = () => collection.Should().BeEqualTo("one", "two", "five");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to be equal to {\"one\", \"two\", \"five\"}, but {\"one\", \"two\", \"three\"} differs at index 2.");
        }
    }
}
