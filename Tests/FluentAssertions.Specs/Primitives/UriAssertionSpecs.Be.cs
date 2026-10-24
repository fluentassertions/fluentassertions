using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class UriAssertionSpecs
{
    public class Be
    {
        [Fact]
        public void Succeeds_when_the_uri_matches()
        {
            // Arrange
            var subject = new Uri("https://example.com/a");

            // Act / Assert
            subject.Should().Be("https://example.com/a");
        }

        [Fact]
        public void Ignores_the_case_of_the_scheme_and_host()
        {
            // Arrange
            var subject = new Uri("https://example.com/a");

            // Act / Assert
            subject.Should().Be("HTTPS://EXAMPLE.com/a");
        }

        [Fact]
        public void The_uri_must_match()
        {
            // Arrange
            var subject = new Uri("https://example.com/a");

            // Act
            Action act = () => subject.Should().Be("https://example.com/b", "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject to be \"https://example.com/b\" because of reasons, but found \"https://example.com/a\".");
        }

        [Fact]
        public void The_failure_message_shows_the_original_string_of_the_uri()
        {
            // Arrange
            var subject = new Uri("HTTPS://EXAMPLE.com/a");

            // Act
            Action act = () => subject.Should().Be("https://example.com/b");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*but found \"HTTPS://EXAMPLE.com/a\".");
        }

        [Fact]
        public void Fails_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act
            Action act = () => subject.Should().Be("https://example.com/");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Can_still_compare_with_another_uri()
        {
            // Arrange
            var subject = new Uri("https://example.com/a");

            // Act / Assert
            subject.Should().Be(new Uri("https://example.com/a"));
        }
    }

    public class NotBe
    {
        [Fact]
        public void Succeeds_when_the_uri_differs()
        {
            // Arrange
            var subject = new Uri("https://example.com/a");

            // Act / Assert
            subject.Should().NotBe("https://example.com/b");
        }

        [Fact]
        public void The_uri_must_differ()
        {
            // Arrange
            var subject = new Uri("https://example.com/a");

            // Act
            Action act = () => subject.Should().NotBe("https://example.com/a", "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect subject to be \"https://example.com/a\" because of reasons.");
        }

        [Fact]
        public void Can_still_compare_with_another_uri()
        {
            // Arrange
            var subject = new Uri("https://example.com/a");

            // Act / Assert
            subject.Should().NotBe(new Uri("https://example.com/b"));
        }

        [Fact]
        public void The_other_uri_must_differ()
        {
            // Arrange
            var subject = new Uri("https://example.com/a");

            // Act
            Action act = () => subject.Should().NotBe(new Uri("https://example.com/a"));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void The_uri_must_not_equal_the_same_uri_with_a_different_case_in_the_scheme_and_host()
        {
            // Arrange
            var subject = new Uri("https://example.com/a");

            // Act
            Action act = () => subject.Should().NotBe("HTTPS://EXAMPLE.com/a");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Succeeds_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act / Assert
            subject.Should().NotBe("https://example.com/a");
        }

        [Fact]
        public void Succeeds_when_the_expected_string_is_not_a_valid_uri()
        {
            // Arrange
            var subject = new Uri("https://example.com/a");

            // Act / Assert
            subject.Should().NotBe("https://exa mple:xx");
        }
    }
}
