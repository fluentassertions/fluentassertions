using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class UriAssertionSpecs
{
    public class HavePort
    {
        [Fact]
        public void Succeeds_when_the_explicit_port_matches()
        {
            // Arrange
            var subject = new Uri("https://example.com:8443/");

            // Act / Assert
            subject.Should().HavePort(8443);
        }

        [Fact]
        public void Succeeds_for_the_default_port_of_the_scheme()
        {
            // Arrange
            var subject = new Uri("https://example.com/");

            // Act / Assert
            subject.Should().HavePort(443);
        }

        [Fact]
        public void The_port_must_match()
        {
            // Arrange
            var subject = new Uri("https://example.com:8443/");

            // Act
            Action act = () => subject.Should().HavePort(443, "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject https://example.com:8443/ to have port 443 because of reasons, but found 8443.");
        }
    }

    public class NotHavePort
    {
        [Fact]
        public void Succeeds_when_the_port_differs()
        {
            // Arrange
            var subject = new Uri("https://example.com:8443/");

            // Act / Assert
            subject.Should().NotHavePort(443);
        }

        [Fact]
        public void The_port_must_differ()
        {
            // Arrange
            var subject = new Uri("https://example.com:8443/");

            // Act
            Action act = () => subject.Should().NotHavePort(8443, "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect subject https://example.com:8443/ to have port 8443 because of reasons.");
        }

        [Fact]
        public void Fails_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act
            Action act = () => subject.Should().NotHavePort(80);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Fails_for_relative_uri()
        {
            // Arrange
            var subject = new Uri("/a", UriKind.Relative);

            // Act
            Action act = () => subject.Should().NotHavePort(80);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected subject to be an absolute URI*");
        }
    }
}
