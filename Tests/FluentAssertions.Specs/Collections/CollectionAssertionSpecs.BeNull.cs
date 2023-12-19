using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The [Not]BeNull specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class BeNull
    {
        [Fact]
        public void When_collection_is_expected_to_be_null_and_it_is_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> someCollection = null;

            // Act / Assert
            someCollection.Should().BeNull();
        }

        [Fact]
        public void When_collection_is_expected_to_be_null_and_it_isnt_it_should_throw()
        {
            // Arrange
            IEnumerable<string> someCollection = new string[0];

            // Act
            Action act = () => someCollection.Should().BeNull("because {0} is valid", "null");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someCollection to be <null> because null is valid, but found {empty}.");
        }
    }

    public class NotBeNull
    {
        [Fact]
        public void When_collection_is_not_expected_to_be_null_and_it_isnt_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> someCollection = new string[0];

            // Act / Assert
            someCollection.Should().NotBeNull();
        }

        [Fact]
        public void When_collection_is_not_expected_to_be_null_and_it_is_it_should_throw()
        {
            // Arrange
            IEnumerable<string> someCollection = null;

            // Act
            Action act = () => someCollection.Should().NotBeNull("because {0} should not", "someCollection");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someCollection not to be <null> because someCollection should not.");
        }
    }
}
