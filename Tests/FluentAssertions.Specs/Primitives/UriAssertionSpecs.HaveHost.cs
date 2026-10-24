using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class UriAssertionSpecs
{
    public class HaveHost
    {
        [Fact]
        public void Succeeds_when_the_host_matches_ignoring_case()
        {
            // Arrange
            var subject = new Uri("https://api.example.com/");

            // Act / Assert
            subject.Should().HaveHost("API.example.com");
        }

        [Fact]
        public void The_host_must_match()
        {
            // Arrange
            var subject = new Uri("https://api.example.com/");

            // Act
            Action act = () => subject.Should().HaveHost("other.com", "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject https://api.example.com/ to have host \"other.com\" because of reasons, " +
                "but found \"api.example.com\".");
        }

        [Fact]
        public void A_relative_uri_has_no_host()
        {
            // Arrange
            var subject = new Uri("/a", UriKind.Relative);

            // Act
            Action act = () => subject.Should().HaveHost("example.com");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject to be an absolute URI with a host, but found /a.");
        }
    }

    public class NotHaveHost
    {
        [Fact]
        public void Succeeds_when_the_host_differs()
        {
            // Arrange
            var subject = new Uri("https://api.example.com/");

            // Act / Assert
            subject.Should().NotHaveHost("example.com");
        }

        [Fact]
        public void The_host_must_differ_ignoring_case()
        {
            // Arrange
            var subject = new Uri("https://api.example.com/");

            // Act
            Action act = () => subject.Should().NotHaveHost("API.example.com", "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect subject https://api.example.com/ to have host \"API.example.com\" because of reasons.");
        }

        [Fact]
        public void Fails_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act
            Action act = () => subject.Should().NotHaveHost("example.com");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Fails_for_relative_uri()
        {
            // Arrange
            var subject = new Uri("/a", UriKind.Relative);

            // Act
            Action act = () => subject.Should().NotHaveHost("example.com");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected subject to be an absolute URI*");
        }
    }
}
