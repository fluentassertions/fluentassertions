using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class UriAssertionSpecs
{
    public class HaveScheme
    {
        [Fact]
        public void Succeeds_when_the_scheme_matches_ignoring_case()
        {
            // Arrange
            var subject = new Uri("https://example.com/");

            // Act / Assert
            subject.Should().HaveScheme("HTTPS");
        }

        [Fact]
        public void The_scheme_must_match()
        {
            // Arrange
            var subject = new Uri("http://example.com/");

            // Act
            Action act = () => subject.Should().HaveScheme("https", "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject http://example.com/ to have scheme \"https\" because of reasons, but found \"http\".");
        }

        [Fact]
        public void A_relative_uri_has_no_scheme()
        {
            // Arrange
            var subject = new Uri("/a", UriKind.Relative);

            // Act
            Action act = () => subject.Should().HaveScheme("https");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject to be an absolute URI with a scheme, but found /a.");
        }

        [Fact]
        public void Fails_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act
            Action act = () => subject.Should().HaveScheme("https");

            // Assert
            act.Should().Throw<XunitException>();
        }
    }

    public class NotHaveScheme
    {
        [Fact]
        public void Succeeds_when_the_scheme_differs()
        {
            // Arrange
            var subject = new Uri("https://example.com/");

            // Act / Assert
            subject.Should().NotHaveScheme("http");
        }

        [Fact]
        public void The_scheme_must_differ_ignoring_case()
        {
            // Arrange
            var subject = new Uri("https://example.com/");

            // Act
            Action act = () => subject.Should().NotHaveScheme("HTTPS", "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect subject https://example.com/ to have scheme \"HTTPS\" because of reasons.");
        }

        [Fact]
        public void Fails_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act
            Action act = () => subject.Should().NotHaveScheme("https");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Fails_for_relative_uri()
        {
            // Arrange
            var subject = new Uri("/a", UriKind.Relative);

            // Act
            Action act = () => subject.Should().NotHaveScheme("https");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected subject to be an absolute URI*");
        }
    }
}
