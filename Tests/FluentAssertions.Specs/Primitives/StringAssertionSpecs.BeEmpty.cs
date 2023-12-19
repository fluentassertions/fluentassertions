using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The [Not]BeEmpty specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class BeEmpty
    {
        [Fact]
        public void Should_succeed_when_asserting_empty_string_to_be_empty()
        {
            // Arrange
            string actual = "";

            // Act / Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void Should_fail_when_asserting_non_empty_string_to_be_empty()
        {
            // Arrange
            string actual = "ABC";

            // Act
            Action act = () => actual.Should().BeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_non_empty_string_to_be_empty()
        {
            // Arrange
            string actual = "ABC";

            // Act
            Action act = () => actual.Should().BeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected actual to be empty because we want to test the failure message, but found \"ABC\".");
        }

        [Fact]
        public void When_checking_for_an_empty_string_and_it_is_null_it_should_throw()
        {
            // Arrange
            string nullString = null;

            // Act
            Action act = () => nullString.Should().BeEmpty("because strings should never be {0}", "null");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected nullString to be empty because strings should never be null, but found <null>.");
        }
    }

    public class NotBeEmpty
    {
        [Fact]
        public void Should_succeed_when_asserting_non_empty_string_to_be_filled()
        {
            // Arrange
            string actual = "ABC";

            // Act / Assert
            actual.Should().NotBeEmpty();
        }

        [Fact]
        public void When_asserting_null_string_to_not_be_empty_it_should_succeed()
        {
            // Arrange
            string actual = null;

            // Act / Assert
            actual.Should().NotBeEmpty();
        }

        [Fact]
        public void Should_fail_when_asserting_empty_string_to_be_filled()
        {
            // Arrange
            string actual = "";

            // Act
            Action act = () => actual.Should().NotBeEmpty();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_empty_string_to_be_filled()
        {
            // Arrange
            string actual = "";

            // Act
            Action act = () => actual.Should().NotBeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect actual to be empty because we want to test the failure message.");
        }
    }
}
