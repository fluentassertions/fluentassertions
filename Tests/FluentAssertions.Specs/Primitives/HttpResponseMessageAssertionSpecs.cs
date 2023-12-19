using System;
using System.Net;
using System.Net.Http;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public class HttpResponseMessageAssertionSpecs
{
    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.Accepted)]
    public void Should_succeed_when_status_code_is_successful(HttpStatusCode statusCodeOfResponse)
    {
        // Arrange
        var testee = new HttpResponseMessage(statusCodeOfResponse);

        // Act / Assert
        testee.Should().BeSuccessful();
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_status_code_error_is_successful()
    {
        // Arrange
        Action action = () => new HttpResponseMessage(HttpStatusCode.Gone).Should()
            .BeSuccessful("because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode to be successful (2xx) because we want to test the failure message, but found HttpStatusCode.Gone {value: 410}.");
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_asserting_success_but_response_is_null()
    {
        // Arrange
        Action action = () =>
            ((HttpResponseMessage)null).Should().BeSuccessful("because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode to be successful (2xx) because we want to test the failure message, but HttpResponseMessage was <null>.");
    }

    [Theory]
    [InlineData(HttpStatusCode.Moved)]
    public void Should_succeed_when_status_code_is_redirect(HttpStatusCode statusCodeOfResponse)
    {
        // Arrange
        var testee = new HttpResponseMessage(statusCodeOfResponse);

        // Act / Assert
        testee.Should().BeRedirection();
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_status_code_error_is_redirection()
    {
        // Arrange
        Action action = () => new HttpResponseMessage(HttpStatusCode.Gone).Should()
            .BeRedirection("because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode to be redirection (3xx) because we want to test the failure message, but found HttpStatusCode.Gone {value: 410}.");
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_asserting_redirect_but_response_is_null()
    {
        // Arrange
        Action action = () =>
            ((HttpResponseMessage)null).Should().BeRedirection("because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode to be redirection (3xx) because we want to test the failure message, but HttpResponseMessage was <null>.");
    }

    [Theory]
    [InlineData(HttpStatusCode.Gone)]
    [InlineData(HttpStatusCode.BadRequest)]
    public void Should_succeed_when_status_code_is_client_error(HttpStatusCode statusCodeOfResponse)
    {
        // Arrange
        var testee = new HttpResponseMessage(statusCodeOfResponse);

        // Act / Assert
        testee.Should().HaveClientError();
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_status_code_success_is_client_error()
    {
        // Arrange
        Action action = () => new HttpResponseMessage(HttpStatusCode.OK).Should()
            .HaveClientError("because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode to be client error (4xx) because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.");
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_asserting_client_error_but_response_is_null()
    {
        // Arrange
        Action action = () =>
            ((HttpResponseMessage)null).Should().HaveClientError("because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode to be client error (4xx) because we want to test the failure message, but HttpResponseMessage was <null>.");
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError)]
    public void Should_succeed_when_status_code_is_server_error(HttpStatusCode statusCodeOfResponse)
    {
        // Arrange
        var testee = new HttpResponseMessage(statusCodeOfResponse);

        // Act / Assert
        testee.Should().HaveServerError();
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_status_code_success_is_server_error()
    {
        // Arrange
        Action action = () => new HttpResponseMessage(HttpStatusCode.OK).Should()
            .HaveServerError("because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode to be server error (5xx) because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.");
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_asserting_server_error_but_response_is_null()
    {
        // Arrange
        Action action = () =>
            ((HttpResponseMessage)null).Should().HaveServerError("because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode to be server error (5xx) because we want to test the failure message, but HttpResponseMessage was <null>.");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public void Should_succeed_when_status_code_is_error(HttpStatusCode statusCodeOfResponse)
    {
        // Arrange
        var testee = new HttpResponseMessage(statusCodeOfResponse);

        // Act / Assert
        testee.Should().HaveError();
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_status_code_success_is_error()
    {
        // Arrange
        Action action = () => new HttpResponseMessage(HttpStatusCode.OK).Should()
            .HaveError("because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode to be an error because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.");
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_asserting_error_but_response_is_null()
    {
        // Arrange
        Action action = () =>
            ((HttpResponseMessage)null).Should().HaveError("because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode to be an error because we want to test the failure message, but HttpResponseMessage was <null>.");
    }

    [Fact]
    public void Should_succeed_when_status_code_to_be_equal_to_the_same_value()
    {
        // Arrange
        Action action = () => new HttpResponseMessage(HttpStatusCode.OK).Should().HaveStatusCode(HttpStatusCode.OK);

        // Act / Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_status_code_value_to_be_equal_to_a_different_value()
    {
        // Arrange
        Action action = () => new HttpResponseMessage(HttpStatusCode.OK).Should().HaveStatusCode(HttpStatusCode.Gone,
            "because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode to be HttpStatusCode.Gone {value: 410} because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.*");
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_asserting_certain_status_code_but_response_is_null()
    {
        // Arrange
        Action action = () => ((HttpResponseMessage)null).Should()
            .HaveStatusCode(HttpStatusCode.Gone, "because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode to be HttpStatusCode.Gone {value: 410} because we want to test the failure message, but HttpResponseMessage was <null>.");
    }

    [Fact]
    public void Should_succeed_when_status_code_value_not_to_be_equal_to_the_same_value()
    {
        // Arrange
        Action action = () => new HttpResponseMessage(HttpStatusCode.OK).Should().NotHaveStatusCode(HttpStatusCode.Gone);

        // Act / Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_status_code_value_not_to_be_equal_to_a_different_value()
    {
        // Arrange
        Action action = () => new HttpResponseMessage(HttpStatusCode.OK).Should().NotHaveStatusCode(HttpStatusCode.OK,
            "because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode not to be HttpStatusCode.OK {value: 200} because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.*");
    }

    [Fact]
    public void Should_fail_with_descriptive_message_when_asserting_against_certain_status_code_but_response_is_null()
    {
        // Arrange
        Action action = () => ((HttpResponseMessage)null).Should()
            .NotHaveStatusCode(HttpStatusCode.Gone, "because we want to test the failure {0}", "message");

        // Act / Assert
        action.Should().Throw<XunitException>()
            .WithMessage(
                "Expected HttpStatusCode not to be HttpStatusCode.Gone {value: 410} because we want to test the failure message, but HttpResponseMessage was <null>.");
    }
}
