using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class UriAssertionSpecs
{
    public class HaveFragment
    {
        [Fact]
        public void Succeeds_when_the_fragment_matches_without_the_hash()
        {
            // Arrange
            var subject = new Uri("https://example.com/#top");

            // Act / Assert
            subject.Should().HaveFragment("top");
        }

        [Fact]
        public void Compares_the_unescaped_fragment()
        {
            // Arrange
            var subject = new Uri("https://example.com/#a%20b");

            // Act / Assert
            subject.Should().HaveFragment("a b");
        }

        [Fact]
        public void The_fragment_must_match()
        {
            // Arrange
            var subject = new Uri("https://example.com/#top");

            // Act
            Action act = () => subject.Should().HaveFragment("bottom", "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject https://example.com/#top to have fragment \"bottom\" because of reasons, " +
                "but found \"top\".");
        }
    }

    public class NotHaveFragment
    {
        [Fact]
        public void Succeeds_when_the_fragment_differs()
        {
            // Arrange
            var subject = new Uri("https://example.com/#top");

            // Act / Assert
            subject.Should().NotHaveFragment("bottom");
        }

        [Fact]
        public void The_fragment_must_differ()
        {
            // Arrange
            var subject = new Uri("https://example.com/#top");

            // Act
            Action act = () => subject.Should().NotHaveFragment("top", "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect subject https://example.com/#top to have fragment \"top\" because of reasons.");
        }

        [Fact]
        public void Fails_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act
            Action act = () => subject.Should().NotHaveFragment("x");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Fails_for_relative_uri()
        {
            // Arrange
            var subject = new Uri("/a", UriKind.Relative);

            // Act
            Action act = () => subject.Should().NotHaveFragment("x");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected subject to be an absolute URI*");
        }
    }
}
