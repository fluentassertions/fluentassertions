using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class UriAssertionSpecs
{
    public class HavePath
    {
        [Fact]
        public void Succeeds_when_the_path_matches_without_the_query_string()
        {
            // Arrange
            var subject = new Uri("https://example.com/orders/42?page=2");

            // Act / Assert
            subject.Should().HavePath("/orders/42");
        }

        [Fact]
        public void Compares_the_unescaped_path()
        {
            // Arrange
            var subject = new Uri("https://example.com/a%20b/c");

            // Act / Assert
            subject.Should().HavePath("/a b/c");
        }

        [Fact]
        public void The_path_must_match_with_the_same_casing()
        {
            // Arrange
            var subject = new Uri("https://example.com/orders/42");

            // Act
            Action act = () => subject.Should().HavePath("/Orders/42", "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject https://example.com/orders/42 to have path \"/Orders/42\" because of reasons, " +
                "but found \"/orders/42\".");
        }
    }

    public class NotHavePath
    {
        [Fact]
        public void Succeeds_when_the_path_differs()
        {
            // Arrange
            var subject = new Uri("https://example.com/orders/42");

            // Act / Assert
            subject.Should().NotHavePath("/orders");
        }

        [Fact]
        public void The_path_must_differ()
        {
            // Arrange
            var subject = new Uri("https://example.com/orders/42");

            // Act
            Action act = () => subject.Should().NotHavePath("/orders/42", "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect subject https://example.com/orders/42 to have path \"/orders/42\" because of reasons.");
        }

        [Fact]
        public void Fails_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act
            Action act = () => subject.Should().NotHavePath("/a");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Fails_for_relative_uri()
        {
            // Arrange
            var subject = new Uri("/a", UriKind.Relative);

            // Act
            Action act = () => subject.Should().NotHavePath("/a");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected subject to be an absolute URI*");
        }
    }
}
