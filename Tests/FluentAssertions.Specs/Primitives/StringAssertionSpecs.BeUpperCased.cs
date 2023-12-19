using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]BeUpperCased specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class BeUpperCased
    {
        [Fact]
        public void When_an_upper_case_string_is_supposed_to_be_in_upper_case_only_it_should_not_throw()
        {
            // Arrange
            string actual = "ABC";

            // Act / Assert
            actual.Should().BeUpperCased();
        }

        [Fact]
        public void When_a_non_upper_string_is_supposed_to_be_upper_it_should_throw()
        {
            // Arrange
            string actual = "abc";

            // Act
            Action act = () => actual.Should().BeUpperCased();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_an_upper_case_string_with_numbers_is_supposed_to_be_in_upper_case_only_it_should_throw()
        {
            // Arrange
            string actual = "A1";

            // Act
            Action act = () => actual.Should().BeUpperCased();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_non_upper_string_is_supposed_to_be_upper_it_should_fail_with_descriptive_message()
        {
            // Arrange
            string actual = "abc";

            // Act
            Action act = () => actual.Should().BeUpperCased("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                @"Expected all characters in actual to be upper cased because we want to test the failure message, but found ""abc"".");
        }

        [Fact]
        public void When_checking_for_an_upper_string_and_it_is_null_it_should_throw()
        {
            // Arrange
            string nullString = null;

            // Act
            Action act = () => nullString.Should().BeUpperCased("because strings should never be {0}", "null");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected all characters in nullString to be upper cased because strings should never be null, but found <null>.");
        }
    }

    public class NotBeUpperCased
    {
        [Fact]
        public void When_a_non_upper_string_is_supposed_to_be_non_upper_it_should_succeed()
        {
            // Arrange
            string actual = "abc";

            // Act / Assert
            actual.Should().NotBeUpperCased();
        }

        [Fact]
        public void When_a_null_string_is_not_supposed_to_be_upper_it_should_succeed()
        {
            // Arrange
            string actual = null;

            // Act / Assert
            actual.Should().NotBeUpperCased();
        }

        [Fact]
        public void When_an_upper_string_is_not_supposed_to_be_upper_it_should_throw()
        {
            // Arrange
            string actual = "ABC";

            // Act
            Action act = () => actual.Should().NotBeUpperCased();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_an_upper_case_string_with_numbers_is_not_supposed_to_be_in_upper_case_only_it_should_succeed()
        {
            // Arrange
            string actual = "a1";

            // Act / Assert
            actual.Should().NotBeUpperCased();
        }

        [Fact]
        public void When_an_upper_string_is_not_supposed_to_be_upper_it_should_fail_with_descriptive_message()
        {
            // Arrange
            string actual = "ABC";

            // Act
            Action act = () => actual.Should().NotBeUpperCased("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect any characters in actual to be upper cased because we want to test the failure message.");
        }
    }
}
