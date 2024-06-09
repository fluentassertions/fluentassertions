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
        public void Upper_case_characters_are_okay()
        {
            // Arrange
            string actual = "ABC";

            // Act / Assert
            actual.Should().BeUpperCased();
        }

        [Fact]
        public void The_empty_string_is_okay()
        {
            // Arrange
            string actual = "";

            // Act / Assert
            actual.Should().BeUpperCased();
        }

        [Fact]
        public void A_lower_case_string_is_not_okay()
        {
            // Arrange
            string actual = "abc";

            // Act
            Action act = () => actual.Should().BeUpperCased();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Upper_case_and_caseless_characters_are_ok()
        {
            // Arrange
            string actual = "A1!";

            // Act / Assert
            actual.Should().BeUpperCased();
        }

        [Fact]
        public void Caseless_characters_are_okay()
        {
            // Arrange
            string actual = "1!漢字";

            // Act / Assert
            actual.Should().BeUpperCased();
        }

        [Fact]
        public void The_assertion_fails_with_a_descriptive_message()
        {
            // Arrange
            string actual = "abc";

            // Act
            Action act = () => actual.Should().BeUpperCased("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected all alphabetic characters in actual to be upper-case because we want to test the failure message, but found \"abc\".");
        }

        [Fact]
        public void The_null_string_is_not_okay()
        {
            // Arrange
            string nullString = null;

            // Act
            Action act = () => nullString.Should().BeUpperCased("because strings should never be {0}", "null");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected all alphabeticc characters in nullString to be upper-case because strings should never be null, but found <null>.");
        }
    }

    public class NotBeUpperCased
    {
        [Fact]
        public void A_mixed_case_string_is_okay()
        {
            // Arrange
            string actual = "aBc";

            // Act / Assert
            actual.Should().NotBeUpperCased();
        }

        [Fact]
        public void The_null_string_is_okay()
        {
            // Arrange
            string actual = null;

            // Act / Assert
            actual.Should().NotBeUpperCased();
        }

        [Fact]
        public void The_empty_string_is_okay()
        {
            // Arrange
            string actual = "";

            // Act / Assert
            actual.Should().NotBeUpperCased();
        }

        [Fact]
        public void A_string_of_all_upper_case_characters_is_not_okay()
        {
            // Arrange
            string actual = "ABC";

            // Act
            Action act = () => actual.Should().NotBeUpperCased();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Upper_case_characters_with_lower_case_characters_are_okay()
        {
            // Arrange
            string actual = "Ab1!";

            // Act / Assert
            actual.Should().NotBeUpperCased();
        }

        [Fact]
        public void All_cased_characters_being_upper_case_is_not_okay()
        {
            // Arrange
            string actual = "A1B!";

            // Act
            Action act = () => actual.Should().NotBeUpperCased();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Caseless_characters_are_okay()
        {
            // Arrange
            string actual = "1!漢字";

            // Act / Assert
            actual.Should().NotBeUpperCased();
        }

        [Fact]
        public void The_assertion_fails_with_a_descriptive_message()
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
