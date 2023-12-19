using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]BeNullOrEmpty specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class BeNullOrEmpty
    {
        [Fact]
        public void When_a_null_string_is_expected_to_be_null_or_empty_it_should_not_throw()
        {
            // Arrange
            string str = null;

            // Act / Assert
            str.Should().BeNullOrEmpty();
        }

        [Fact]
        public void When_an_empty_string_is_expected_to_be_null_or_empty_it_should_not_throw()
        {
            // Arrange
            string str = "";

            // Act / Assert
            str.Should().BeNullOrEmpty();
        }

        [Fact]
        public void When_a_valid_string_is_expected_to_be_null_or_empty_it_should_throw()
        {
            // Arrange
            string str = "hello";

            // Act
            Action act = () => str.Should().BeNullOrEmpty("it was not initialized {0}", "yet");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected str to be <null> or empty because it was not initialized yet, but found \"hello\".");
        }
    }

    public class NotBeNullOrEmpty
    {
        [Fact]
        public void When_a_valid_string_is_expected_to_be_not_null_or_empty_it_should_not_throw()
        {
            // Arrange
            string str = "Hello World";

            // Act / Assert
            str.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void When_an_empty_string_is_not_expected_to_be_null_or_empty_it_should_throw()
        {
            // Arrange
            string str = "";

            // Act
            Action act = () => str.Should().NotBeNullOrEmpty("a valid string is expected for {0}", "str");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected str not to be <null> or empty because a valid string is expected for str, but found \"\".");
        }

        [Fact]
        public void When_a_null_string_is_not_expected_to_be_null_or_empty_it_should_throw()
        {
            // Arrange
            string str = null;

            // Act
            Action act = () => str.Should().NotBeNullOrEmpty("a valid string is expected for {0}", "str");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected str not to be <null> or empty because a valid string is expected for str, but found <null>.");
        }
    }
}
