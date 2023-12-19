using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]BeNullOrWhiteSpace specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class BeNullOrWhitespace
    {
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n\r")]
        [Theory]
        public void When_correctly_asserting_null_or_whitespace_it_should_not_throw(string actual)
        {
            // Assert
            actual.Should().BeNullOrWhiteSpace();
        }

        [InlineData("a")]
        [InlineData(" a ")]
        [Theory]
        public void When_correctly_asserting_not_null_or_whitespace_it_should_not_throw(string actual)
        {
            // Assert
            actual.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void When_a_valid_string_is_expected_to_be_null_or_whitespace_it_should_throw()
        {
            // Act
            Action act = () =>
                " abc  ".Should().BeNullOrWhiteSpace();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected string to be <null> or whitespace, but found \" abc  \".");
        }
    }

    public class NotBeNullOrWhitespace
    {
        [Fact]
        public void When_a_null_string_is_expected_to_not_be_null_or_whitespace_it_should_throw()
        {
            // Arrange
            string nullString = null;

            // Act
            Action act = () =>
                nullString.Should().NotBeNullOrWhiteSpace();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected nullString not to be <null> or whitespace, but found <null>.");
        }

        [Fact]
        public void When_an_empty_string_is_expected_to_not_be_null_or_whitespace_it_should_throw()
        {
            // Act
            Action act = () =>
                "".Should().NotBeNullOrWhiteSpace();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected string not to be <null> or whitespace, but found \"\".");
        }

        [Fact]
        public void When_a_whitespace_string_is_expected_to_not_be_null_or_whitespace_it_should_throw()
        {
            // Act
            Action act = () =>
                "   ".Should().NotBeNullOrWhiteSpace();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected string not to be <null> or whitespace, but found \"   \".");
        }
    }
}
