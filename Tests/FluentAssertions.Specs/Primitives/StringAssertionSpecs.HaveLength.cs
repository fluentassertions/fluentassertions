using System;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

/// <content>
/// The HaveLength specs.
/// </content>
public partial class StringAssertionSpecs
{
    public class HaveLength
    {
        [Fact]
        public void Should_succeed_when_asserting_string_length_to_be_equal_to_the_same_value()
        {
            // Arrange
            string actual = "ABC";

            // Act / Assert
            actual.Should().HaveLength(3);
        }

        [Fact]
        public void When_asserting_string_length_on_null_string_it_should_fail()
        {
            // Arrange
            string actual = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                actual.Should().HaveLength(0, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected actual with length 0 *failure message*, but found <null>.");
        }

        [Fact]
        public void Should_fail_when_asserting_string_length_to_be_equal_to_different_value()
        {
            // Arrange
            string actual = "ABC";

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                actual.Should().HaveLength(1, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected actual with length 1 *failure message*, but found string \"ABC\" with length 3.");
        }
    }
}
