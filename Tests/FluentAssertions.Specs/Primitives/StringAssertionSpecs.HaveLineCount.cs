using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class StringAssertionSpecs
{
    public class HaveLineCount
    {
        [Theory]
        [InlineData("Starting up\nConnected to database\nReady", 3)]
        [InlineData("just one line", 1)]
        [InlineData("one\r\ntwo\nthree\rfour", 4)]
        [InlineData("one\ntwo\n", 3)]
        [InlineData("", 1)]
        [InlineData("\n", 2)]
        public void Succeeds_for_a_string_with_the_expected_number_of_lines(string actual, int expectedLineCount)
        {
            // Act / Assert
            actual.Should().HaveLineCount(expectedLineCount);
        }

        [Fact]
        public void Fails_for_a_string_with_a_different_number_of_lines()
        {
            // Arrange
            string actual = "one\ntwo\nthree";

            // Act
            Action act = () => actual.Should().HaveLineCount(2, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected actual to have 2 line(s) *failure message*, but found 3.");
        }

        [Fact]
        public void Fails_for_null_string()
        {
            // Arrange
            string actual = null;

            // Act
            Action act = () => actual.Should().HaveLineCount(1, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected actual to have 1 line(s) *failure message*, but found <null>.");
        }
    }
}
