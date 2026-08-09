using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class StringAssertionSpecs
{
    public class ContainLine
    {
        [Fact]
        public void Succeeds_when_the_string_contains_the_expected_line()
        {
            // Arrange
            string log = "Starting up\nConnected to database\nReady";

            // Act / Assert
            log.Should().ContainLine("Connected to database");
        }

        [Fact]
        public void Treats_different_line_endings_as_equivalent()
        {
            // Arrange
            string log = "one\r\ntwo\nthree\rfour";

            // Act / Assert
            log.Should().ContainLine("three");
        }

        [Fact]
        public void Does_not_match_a_partial_line()
        {
            // Arrange
            string log = "Connected to database";

            // Act
            Action act = () => log.Should().ContainLine("Connected");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Fails_when_the_string_does_not_contain_the_expected_line()
        {
            // Arrange
            string log = "Ready";

            // Act
            Action act = () => log.Should().ContainLine("Error", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected log \"Ready\" to contain line \"Error\" *failure message*.");
        }

        [Fact]
        public void Fails_for_null_string()
        {
            // Arrange
            string log = null;

            // Act
            Action act = () => log.Should().ContainLine("Ready");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void An_expected_line_is_required()
        {
            // Act
            Action act = () => "a".Should().ContainLine(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot assert string line containment against <null>.*")
                .WithParameterName("expectedLine");
        }
    }
}
