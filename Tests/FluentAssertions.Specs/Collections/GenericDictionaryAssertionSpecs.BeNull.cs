using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    public class BeNull
    {
        [Fact]
        public void When_dictionary_is_expected_to_be_null_and_it_is_it_should_not_throw()
        {
            // Arrange
            IDictionary<int, string> someDictionary = null;

            // Act / Assert
            someDictionary.Should().BeNull();
        }

        [Fact]
        public void When_dictionary_is_expected_to_be_null_and_it_isnt_it_should_throw()
        {
            // Arrange
            var someDictionary = new Dictionary<int, string>();

            // Act
            Action act = () => someDictionary.Should().BeNull("because {0} is valid", "null");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someDictionary to be <null> because null is valid, but found {empty}.");
        }
    }

    public class NotBeNull
    {
        [Fact]
        public void When_a_custom_dictionary_implementation_is_expected_not_to_be_null_and_it_is_it_should_not_throw()
        {
            // Arrange
            var dictionary = new TrackingTestDictionary();

            // Act / Assert
            dictionary.Should().NotBeNull();
        }

        [Fact]
        public void When_dictionary_is_not_expected_to_be_null_and_it_isnt_it_should_not_throw()
        {
            // Arrange
            IDictionary<int, string> someDictionary = new Dictionary<int, string>();

            // Act / Assert
            someDictionary.Should().NotBeNull();
        }

        [Fact]
        public void When_dictionary_is_not_expected_to_be_null_and_it_is_it_should_throw()
        {
            // Arrange
            IDictionary<int, string> someDictionary = null;

            // Act
            Action act = () => someDictionary.Should().NotBeNull("because {0} should not", "someDictionary");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someDictionary not to be <null> because someDictionary should not.");
        }
    }
}
