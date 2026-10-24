using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class UriAssertionSpecs
{
    public class BeAbsolute
    {
        [Fact]
        public void Succeeds_for_absolute_uri()
        {
            // Arrange
            var subject = new Uri("https://example.com/a");

            // Act / Assert
            subject.Should().BeAbsolute();
        }

        [Fact]
        public void An_absolute_uri_is_required()
        {
            // Arrange
            var subject = new Uri("/a/b", UriKind.Relative);

            // Act
            Action act = () => subject.Should().BeAbsolute("of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject to be absolute because of reasons, but found /a/b.");
        }

        [Fact]
        public void Fails_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act
            Action act = () => subject.Should().BeAbsolute();

            // Assert
            act.Should().Throw<XunitException>();
        }
    }

    public class BeRelative
    {
        [Fact]
        public void Succeeds_for_relative_uri()
        {
            // Arrange
            var subject = new Uri("/a/b", UriKind.Relative);

            // Act / Assert
            subject.Should().BeRelative();
        }

        [Fact]
        public void A_relative_uri_is_required()
        {
            // Arrange
            var subject = new Uri("https://example.com/a");

            // Act
            Action act = () => subject.Should().BeRelative("of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject to be relative because of reasons, but found https://example.com/a.");
        }

        [Fact]
        public void Fails_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act
            Action act = () => subject.Should().BeRelative();

            // Assert
            act.Should().Throw<XunitException>();
        }
    }
}
