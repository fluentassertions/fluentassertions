using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class StringAssertionSpecs
{
    public class NotHaveLineCount
    {
        [Fact]
        public void Succeeds_for_a_string_with_a_different_number_of_lines()
        {
            // Arrange
            string actual = "one\ntwo\nthree";

            // Act / Assert
            actual.Should().NotHaveLineCount(2);
        }

        [Fact]
        public void Fails_for_a_string_with_the_unexpected_number_of_lines()
        {
            // Arrange
            string actual = "one\ntwo\nthree";

            // Act
            Action act = () => actual.Should().NotHaveLineCount(3, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected actual to not have 3 line(s) *failure message*, but found 3.");
        }

        [Fact]
        public void Fails_for_null_string()
        {
            // Arrange
            string actual = null;

            // Act
            Action act = () => actual.Should().NotHaveLineCount(1, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected actual to not have 1 line(s) *failure message*, but found <null>.");
        }
    }
}
