using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

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
        public void When_an_empty_string_is_supposed_to_be_upper_it_should_succeed()
        {
            // Arrange
            string actual = "";

            // Act / Assert
            actual.Should().BeUpperCased();
        }

        [Fact]
        public void When_a_lower_case_string_is_supposed_to_be_upper_it_should_throw()
        {
            // Arrange
            string actual = "abc";

            // Act
            Action act = () => actual.Should().BeUpperCased();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_an_upper_case_string_with_non_alplha_chars_is_supposed_to_be_in_upper_case_only_it_should_succeed()
        {
            // Arrange
            string actual = "A1!";

            // Act / Assert
            actual.Should().BeUpperCased();
        }

        [Fact]
        public void When_a_string_with_caseless_chars_is_supposed_to_be_upper_it_should_succeed()
        {
            // Arrange
            string actual = "1!漢字";

            // Act / Assert
            actual.Should().BeUpperCased();
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
                @"Expected all alpha characters in actual to be upper-case because we want to test the failure message, but found ""abc"".");
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
                "Expected all alpha characters in nullString to be upper-case because strings should never be null, but found <null>.");
        }
    }

    public class NotBeUpperCased
    {
        [Fact]
        public void When_a_mixed_case_string_is_not_supposed_to_be_uppered_it_should_succeed()
        {
            // Arrange
            string actual = "aBc";

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
        public void When_an_empty_string_is_not_supposed_to_be_upper_it_should_succeed()
        {
            // Arrange
            string actual = "";

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
        public void When_a_string_containing_lower_chars_is_not_supposed_to_be_uppered_it_should_succeed()
        {
            // Arrange
            string actual = "Ab1!";

            // Act / Assert
            actual.Should().NotBeUpperCased();
        }

        [Fact]
        public void When_a_string_in_which_all_alpha_chars_are_upper_is_not_supposed_to_be_uppered_only_it_should_throw()
        {
            // Arrange
            string actual = "A1B!";

            // Act
            Action act = () => actual.Should().NotBeUpperCased();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_string_with_only_caseless_chars_is_not_supposed_to_be_uppered_it_should_succeed()
        {
            // Arrange
            string actual = "1!漢字";

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
                "Expected some characters in actual to be lower-case because we want to test the failure message.");
        }
    }
}
