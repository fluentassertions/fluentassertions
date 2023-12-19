using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]ContainAll specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class ContainAll
    {
        [Fact]
        public void When_containment_of_all_strings_in_a_null_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().ContainAll(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot*containment*null*")
                .WithParameterName("values");
        }

        [Fact]
        public void When_containment_of_all_strings_in_an_empty_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().ContainAll();

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Cannot*containment*empty*")
                .WithParameterName("values");
        }

        [Fact]
        public void When_containment_of_all_strings_in_a_collection_is_asserted_and_all_strings_are_present_it_should_succeed()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().ContainAll(red, green, yellow);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_containment_of_all_strings_in_a_collection_is_asserted_and_equivalent_but_not_exact_matches_exist_for_all_it_should_throw()
        {
            // Arrange
            const string redLowerCase = "red";
            const string redUpperCase = "RED";
            const string greenWithoutWhitespace = "green";
            const string greenWithWhitespace = "  green ";
            var testString = $"{redLowerCase} {greenWithoutWhitespace}";

            // Act
            Action act = () => testString.Should().ContainAll(redUpperCase, greenWithWhitespace);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain*{redUpperCase}*{greenWithWhitespace}*");
        }

        [Fact]
        public void
            When_containment_of_all_strings_in_a_collection_is_asserted_and_none_of_the_strings_are_present_it_should_throw()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string blue = "blue";
            var testString = $"{red} {green}";

            // Act
            Action act = () => testString.Should().ContainAll(yellow, blue);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain*{yellow}*{blue}*");
        }

        [Fact]
        public void
            When_containment_of_all_strings_in_a_collection_is_asserted_with_reason_and_assertion_fails_then_failure_message_should_contain_reason()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string blue = "blue";
            var testString = $"{red} {green}";

            // Act
            Action act = () => testString.Should().ContainAll(new[] { yellow, blue }, "some {0} reason", "special");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain*{yellow}*{blue}*because some special reason*");
        }

        [Fact]
        public void
            When_containment_of_all_strings_in_a_collection_is_asserted_and_only_some_of_the_strings_are_present_it_should_throw()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string blue = "blue";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().ContainAll(red, blue, green);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain*{blue}*");
        }
    }

    public class NotContainAll
    {
        [Fact]
        public void When_exclusion_of_all_strings_in_null_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().NotContainAll(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot*containment*null*")
                .WithParameterName("values");
        }

        [Fact]
        public void When_exclusion_of_all_strings_in_an_empty_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().NotContainAll();

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Cannot*containment*empty*")
                .WithParameterName("values");
        }

        [Fact]
        public void
            When_exclusion_of_all_strings_in_a_collection_is_asserted_and_all_strings_in_collection_are_present_it_should_throw()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().NotContainAll(red, green, yellow);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*not*{testString}*contain all*{red}*{green}*{yellow}*");
        }

        [Fact]
        public void When_exclusion_of_all_strings_is_asserted_with_reason_and_assertion_fails_then_error_message_contains_reason()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().NotContainAll(new[] { red, green, yellow }, "some {0} reason", "special");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*not*{testString}*contain all*{red}*{green}*{yellow}*because*some special reason*");
        }

        [Fact]
        public void
            When_exclusion_of_all_strings_in_a_collection_is_asserted_and_only_some_of_the_strings_in_collection_are_present_it_should_succeed()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string purple = "purple";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().NotContainAll(red, green, yellow, purple);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_exclusion_of_all_strings_in_a_collection_is_asserted_and_none_of_the_strings_in_the_collection_are_present_it_should_succeed()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string purple = "purple";
            var testString = $"{red} {green}";

            // Act
            Action act = () => testString.Should().NotContainAll(yellow, purple);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_exclusion_of_all_strings_in_a_collection_is_asserted_and_equivalent_but_not_exact_strings_are_present_in_collection_it_should_succeed()
        {
            // Arrange
            const string redWithoutWhitespace = "red";
            const string redWithWhitespace = "  red ";
            const string lowerCaseGreen = "green";
            const string upperCaseGreen = "GREEN";
            var testString = $"{redWithoutWhitespace} {lowerCaseGreen}";

            // Act
            Action act = () => testString.Should().NotContainAll(redWithWhitespace, upperCaseGreen);

            // Assert
            act.Should().NotThrow();
        }
    }
}
