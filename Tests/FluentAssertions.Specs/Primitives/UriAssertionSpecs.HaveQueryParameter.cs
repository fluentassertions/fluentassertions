using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class UriAssertionSpecs
{
    public class HaveQueryParameter
    {
        [Fact]
        public void Succeeds_when_the_parameter_has_the_expected_value()
        {
            // Arrange
            var subject = new Uri("https://example.com/?page=2&size=10");

            // Act / Assert
            subject.Should().HaveQueryParameter("page", "2");
        }

        [Fact]
        public void Compares_the_unescaped_value()
        {
            // Arrange
            var subject = new Uri("https://example.com/?q=a%20b&r=c+d");

            // Act / Assert
            subject.Should().HaveQueryParameter("q", "a b").And.HaveQueryParameter("r", "c d");
        }

        [Fact]
        public void Succeeds_when_any_parameter_with_that_name_has_the_expected_value()
        {
            // Arrange
            var subject = new Uri("https://example.com/?tag=a&tag=b");

            // Act / Assert
            subject.Should().HaveQueryParameter("tag", "b");
        }

        [Fact]
        public void The_parameter_must_have_the_expected_value()
        {
            // Arrange
            var subject = new Uri("https://example.com/?page=2");

            // Act
            Action act = () => subject.Should().HaveQueryParameter("page", "3", "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject https://example.com/?page=2 to have query parameter \"page\" with value \"3\" " +
                "because of reasons, but found*\"2\"*");
        }

        [Fact]
        public void The_parameter_must_be_present_when_a_value_is_expected()
        {
            // Arrange
            var subject = new Uri("https://example.com/?page=2");

            // Act
            Action act = () => subject.Should().HaveQueryParameter("size", "10");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject https://example.com/?page=2 to have query parameter \"size\" with value \"10\", " +
                "but the parameter was not found.");
        }

        [Fact]
        public void Without_a_value_it_exposes_the_first_value_of_the_parameter()
        {
            // Arrange
            var subject = new Uri("https://example.com/?page=2&flag");

            // Act / Assert
            subject.Should().HaveQueryParameter("page").Which.Should().Be("2");
            subject.Should().HaveQueryParameter("flag").Which.Should().BeEmpty();
        }

        [Fact]
        public void Without_a_value_the_parameter_must_be_present()
        {
            // Arrange
            var subject = new Uri("https://example.com/?page=2");

            // Act
            Action act = () => subject.Should().HaveQueryParameter("size");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject https://example.com/?page=2 to have query parameter \"size\", " +
                "but it was not found.");
        }

        [Fact]
        public void A_relative_uri_has_no_query_parameters()
        {
            // Arrange
            var subject = new Uri("/a?page=2", UriKind.Relative);

            // Act
            Action act = () => subject.Should().HaveQueryParameter("page");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected subject to be an absolute URI with a query parameter, but found /a?page=2.");
        }

        [Fact]
        public void Fails_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act
            Action act = () => subject.Should().HaveQueryParameter("page");

            // Assert
            act.Should().Throw<XunitException>();
        }
    }

    public class NotHaveQueryParameter
    {
        [Fact]
        public void Succeeds_when_the_parameter_is_absent()
        {
            // Arrange
            var subject = new Uri("https://example.com/?page=2");

            // Act / Assert
            subject.Should().NotHaveQueryParameter("size");
        }

        [Fact]
        public void The_parameter_must_be_absent()
        {
            // Arrange
            var subject = new Uri("https://example.com/?page=2");

            // Act
            Action act = () => subject.Should().NotHaveQueryParameter("page", "of {0}", "reasons");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect subject https://example.com/?page=2 to have query parameter \"page\" because of reasons.");
        }

        [Fact]
        public void Fails_for_null_uri()
        {
            // Arrange
            Uri subject = null;

            // Act
            Action act = () => subject.Should().NotHaveQueryParameter("a");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Fails_for_relative_uri()
        {
            // Arrange
            var subject = new Uri("/a", UriKind.Relative);

            // Act
            Action act = () => subject.Should().NotHaveQueryParameter("a");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected subject to be an absolute URI*");
        }
    }
}
