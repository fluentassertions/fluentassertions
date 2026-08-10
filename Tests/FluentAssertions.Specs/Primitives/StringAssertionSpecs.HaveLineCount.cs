using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class StringAssertionSpecs
{
    public class HaveLineCount
    {
        [Fact]
        public void Succeeds_for_a_string_with_the_expected_number_of_lines()
        {
            // Arrange
            string actual = "Starting up\nConnected to database\nReady";

            // Act / Assert
            actual.Should().HaveLineCount(3);
        }

        [Fact]
        public void Succeeds_for_a_single_line_string()
        {
            // Arrange
            string actual = "just one line";

            // Act / Assert
            actual.Should().HaveLineCount(1);
        }

        [Fact]
        public void Treats_different_line_endings_as_equivalent()
        {
            // Arrange
            string actual = "one\r\ntwo\nthree\rfour";

            // Act / Assert
            actual.Should().HaveLineCount(4);
        }

        [Fact]
        public void A_trailing_line_terminator_introduces_an_additional_empty_line()
        {
            // Arrange
            string actual = "one\ntwo\n";

            // Act / Assert
            actual.Should().HaveLineCount(3);
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
