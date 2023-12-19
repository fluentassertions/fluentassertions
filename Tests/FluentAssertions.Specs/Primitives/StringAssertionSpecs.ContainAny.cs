using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]ContainAny specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class ContainAny
    {
        [Fact]
        public void When_containment_of_any_string_in_a_null_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().ContainAny(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot*containment*null*")
                .WithParameterName("values");
        }

        [Fact]
        public void When_containment_of_any_string_in_an_empty_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().ContainAny();

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Cannot*containment*empty*")
                .WithParameterName("values");
        }

        [Fact]
        public void
            When_containment_of_any_string_in_a_collection_is_asserted_and_all_of_the_strings_are_present_it_should_succeed()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().ContainAny(red, green, yellow);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_containment_of_any_string_in_a_collection_is_asserted_and_only_some_of_the_strings_are_present_it_should_succeed()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string blue = "blue";
            var testString = $"{red} {green}";

            // Act
            Action act = () => testString.Should().ContainAny(red, blue, green);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_containment_of_any_string_in_a_collection_is_asserted_and_none_of_the_strings_are_present_it_should_throw()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string blue = "blue";
            const string purple = "purple";
            var testString = $"{red} {green}";

            // Act
            Action act = () => testString.Should().ContainAny(blue, purple);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain at least one of*{blue}*{purple}*");
        }

        [Fact]
        public void
            When_containment_of_any_string_in_a_collection_is_asserted_and_there_are_equivalent_but_not_exact_matches_it_should_throw()
        {
            // Arrange
            const string redLowerCase = "red";
            const string redUpperCase = "RED";
            const string greenWithoutWhitespace = "green";
            const string greenWithWhitespace = "   green";
            var testString = $"{redLowerCase} {greenWithoutWhitespace}";

            // Act
            Action act = () => testString.Should().ContainAny(redUpperCase, greenWithWhitespace);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain at least one of*{redUpperCase}*{greenWithWhitespace}*");
        }

        [Fact]
        public void
            When_containment_of_any_string_in_a_collection_is_asserted_with_reason_and_assertion_fails_then_failure_message_contains_reason()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string blue = "blue";
            const string purple = "purple";
            var testString = $"{red} {green}";

            // Act
            Action act = () => testString.Should().ContainAny(new[] { blue, purple }, "some {0} reason", "special");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*{testString}*contain at least one of*{blue}*{purple}*because some special reason*");
        }
    }

    public class NotContainAny
    {
        [Fact]
        public void When_exclusion_of_any_string_in_null_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().NotContainAny(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot*containment*null*")
                .WithParameterName("values");
        }

        [Fact]
        public void When_exclusion_of_any_string_in_an_empty_collection_is_asserted_it_should_throw_an_argument_exception()
        {
            // Act
            Action act = () => "a".Should().NotContainAny();

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Cannot*containment*empty*")
                .WithParameterName("values");
        }

        [Fact]
        public void When_exclusion_of_any_string_in_a_collection_is_asserted_and_all_of_the_strings_are_present_it_should_throw()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().NotContainAny(red, green, yellow);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*not*{testString}*contain any*{red}*{green}*{yellow}*");
        }

        [Fact]
        public void
            When_exclusion_of_any_string_in_a_collection_is_asserted_and_only_some_of_the_strings_are_present_it_should_throw()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string purple = "purple";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().NotContainAny(red, purple, green);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*not*{testString}*contain any*{red}*{green}*");
        }

        [Fact]
        public void When_exclusion_of_any_strings_is_asserted_with_reason_and_assertion_fails_then_error_message_contains_reason()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            var testString = $"{red} {green} {yellow}";

            // Act
            Action act = () => testString.Should().NotContainAny(new[] { red }, "some {0} reason", "special");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage($"*not*{testString}*contain any*{red}*because*some special reason*");
        }

        [Fact]
        public void
            When_exclusion_of_any_string_in_a_collection_is_asserted_and_there_are_equivalent_but_not_exact_matches_it_should_succeed()
        {
            // Arrange
            const string redLowerCase = "red";
            const string redUpperCase = "RED";
            const string greenWithoutWhitespace = "green";
            const string greenWithWhitespace = " green  ";
            var testString = $"{redLowerCase} {greenWithoutWhitespace}";

            // Act
            Action act = () => testString.Should().NotContainAny(redUpperCase, greenWithWhitespace);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_exclusion_of_any_string_in_a_collection_is_asserted_and_none_of_the_strings_are_present_it_should_succeed()
        {
            // Arrange
            const string red = "red";
            const string green = "green";
            const string yellow = "yellow";
            const string purple = "purple";
            var testString = $"{red} {green}";

            // Act
            Action act = () => testString.Should().NotContainAny(yellow, purple);

            // Assert
            act.Should().NotThrow();
        }
    }
}
