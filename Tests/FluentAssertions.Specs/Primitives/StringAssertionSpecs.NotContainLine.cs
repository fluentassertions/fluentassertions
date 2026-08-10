using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class StringAssertionSpecs
{
    public class NotContainLine
    {
        [Fact]
        public void Succeeds_when_the_string_does_not_contain_the_unexpected_line()
        {
            // Arrange
            string log = "Starting up\nReady";

            // Act / Assert
            log.Should().NotContainLine("Error");
        }

        [Fact]
        public void Succeeds_for_a_partial_match_that_is_not_a_full_line()
        {
            // Arrange
            string log = "Connected to database";

            // Act / Assert
            log.Should().NotContainLine("Connected");
        }

        [Fact]
        public void Succeeds_for_null_string()
        {
            // Arrange
            string log = null;

            // Act / Assert
            log.Should().NotContainLine("Ready");
        }

        [Fact]
        public void Fails_when_the_string_contains_the_unexpected_line()
        {
            // Arrange
            string log = "Error";

            // Act
            Action act = () => log.Should().NotContainLine("Error", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect log \"Error\" to contain line \"Error\" *failure message*.");
        }

        [Fact]
        public void An_unexpected_line_is_required()
        {
            // Act
            Action act = () => "a".Should().NotContainLine(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert string line containment against <null>.*")
                .WithParameterName("unexpectedLine");
        }
    }
}
