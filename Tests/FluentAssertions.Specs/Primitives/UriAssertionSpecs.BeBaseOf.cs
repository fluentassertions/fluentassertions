using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class UriAssertionSpecs
{
    public class BeBaseOf
    {
        [Fact]
        public void Succeeds_when_the_other_uri_starts_with_the_subject()
        {
            // Arrange
            var subject = new Uri("https://example.com/api/");

            // Act / Assert
            subject.Should().BeBaseOf(new Uri("https://example.com/api/orders"));
        }

        [Fact]
        public void The_other_uri_must_start_with_the_subject()
        {
            // Arrange
            var subject = new Uri("https://example.com/api/");

            // Act
            Action act = () => subject.Should().BeBaseOf(new Uri("https://other.com/"), "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject https://example.com/api/ to be a base of https://other.com/ because of reasons.");
        }

        [Fact]
        public void A_relative_uri_is_never_a_base()
        {
            // Arrange
            var subject = new Uri("/api/", UriKind.Relative);

            // Act
            Action act = () => subject.Should().BeBaseOf(new Uri("https://other.com/"));

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject to be an absolute URI with a base, but found /api/.");
        }

        [Fact]
        public void The_other_uri_is_required()
        {
            // Arrange
            var subject = new Uri("https://example.com/api/");

            // Act
            Action act = () => subject.Should().BeBaseOf(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("expected");
        }
    }

    public class NotBeBaseOf
    {
        [Fact]
        public void Succeeds_when_the_other_uri_does_not_start_with_the_subject()
        {
            // Arrange
            var subject = new Uri("https://example.com/api/");

            // Act / Assert
            subject.Should().NotBeBaseOf(new Uri("https://other.com/api/orders"));
        }

        [Fact]
        public void The_other_uri_must_not_start_with_the_subject()
        {
            // Arrange
            var subject = new Uri("https://example.com/api/");

            // Act
            Action act = () => subject.Should().NotBeBaseOf(new Uri("https://example.com/api/orders"), "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect subject https://example.com/api/ to be a base of https://example.com/api/orders " +
                "because of reasons.");
        }

        [Fact]
        public void The_other_uri_is_required()
        {
            // Arrange
            var subject = new Uri("https://example.com/api/");

            // Act
            Action act = () => subject.Should().NotBeBaseOf(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("unexpected");
        }

        [Fact]
        public void Fails_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act
            Action act = () => subject.Should().NotBeBaseOf(new Uri("https://example.com/"));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Fails_for_relative_uri()
        {
            // Arrange
            var subject = new Uri("/a", UriKind.Relative);

            // Act
            Action act = () => subject.Should().NotBeBaseOf(new Uri("https://example.com/"));

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected subject to be an absolute URI*");
        }
    }
}
