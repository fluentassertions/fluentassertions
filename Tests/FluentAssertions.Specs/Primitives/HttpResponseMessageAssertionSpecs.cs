using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives
{
    public class HttpResponseMessageAssertionSpecs
    {
        [Theory]
        [MemberData(nameof(GetSuccessStatusCodes))]
        public void Should_succeed_when_asserting_statuscode_is_successful(HttpStatusCode statusCodeOfResponse)
        {
            // Arrange
            var testee = new HttpResponseMessage(statusCodeOfResponse);

            // Act / Assert
            testee.Should().BeSuccessful();
        }

        [Theory]
        [MemberData(nameof(GetRedirectionStatusCodes))]
        public void Should_succeed_when_asserting_statuscode_is_redirect(HttpStatusCode statusCodeOfResponse)
        {
            // Arrange
            var testee = new HttpResponseMessage(statusCodeOfResponse);

            // Act / Assert
            testee.Should().BeRedirection();
        }

        [Theory]
        [MemberData(nameof(GetClientErrorStatusCodes))]
        public void Should_succeed_when_asserting_statuscode_is_client_error(HttpStatusCode statusCodeOfResponse)
        {
            // Arrange
            var testee = new HttpResponseMessage(statusCodeOfResponse);

            // Act / Assert
            testee.Should().BeClientError();
        }

        [Theory]
        [MemberData(nameof(GetServerErrorStatusCodes))]
        public void Should_succeed_when_asserting_statuscode_is_server_error(HttpStatusCode statusCodeOfResponse)
        {
            // Arrange
            var testee = new HttpResponseMessage(statusCodeOfResponse);

            // Act / Assert
            testee.Should().BeServerError();
        }

        [Theory]
        [MemberData(nameof(GetClientErrorStatusCodes))]
        [MemberData(nameof(GetServerErrorStatusCodes))]
        public void Should_succeed_when_asserting_statuscode_is_error(HttpStatusCode statusCodeOfResponse)
        {
            // Arrange
            var testee = new HttpResponseMessage(statusCodeOfResponse);

            // Act / Assert
            testee.Should().BeError();
        }

        [Fact]
        public void Should_fail_when_asserting_statuscode_error_is_successful()
        {
            // Arrange
            var testee = new HttpResponseMessage(HttpStatusCode.Gone);

            // Act
            Action action = () => testee.Should().BeSuccessful();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_statuscode_error_is_successful()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.Gone).Should().BeSuccessful("because we want to test the failure {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Expected HttpStatusCode to be successful (2xx) because we want to test the failure message, but found HttpStatusCode.Gone {value: 410}.");
        }

        [Fact]
        public void Should_fail_when_asserting_statuscode_error_is_redirection()
        {
            // Arrange
            var testee = new HttpResponseMessage(HttpStatusCode.Gone);

            // Act
            Action action = () => testee.Should().BeRedirection();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_statuscode_error_is_redirection()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.Gone).Should().BeRedirection("because we want to test the failure {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Expected HttpStatusCode to be redirection (3xx) because we want to test the failure message, but found HttpStatusCode.Gone {value: 410}.");
        }

        [Fact]
        public void Should_fail_when_asserting_statuscode_success_is_client_error()
        {
            // Arrange
            var testee = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action action = () => testee.Should().BeClientError();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_statuscode_success_is_client_error()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().BeClientError("because we want to test the failure {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Expected HttpStatusCode to be client error (4xx) because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.");
        }

        [Fact]
        public void Should_fail_when_asserting_statuscode_success_is_server_error()
        {
            // Arrange
            var testee = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action action = () => testee.Should().BeServerError();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_statuscode_success_is_server_error()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().BeServerError("because we want to test the failure {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage("Expected HttpStatusCode to be server error (5xx) because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.");
        }

        [Fact]
        public void Should_succeed_when_asserting_statuscode_to_be_equal_to_the_same_value()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().HaveStatusCode(HttpStatusCode.OK);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_statuscode_to_be_equal_to_a_different_value()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().HaveStatusCode(HttpStatusCode.Gone);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_statuscode_value_to_be_equal_to_a_different_value()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().HaveStatusCode(HttpStatusCode.Gone, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected HttpStatusCode to be HttpStatusCode.Gone {value: 410} because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.*");
        }

        [Fact]
        public void Should_succeed_when_asserting_statuscode_value_not_to_be_equal_to_the_same_value()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().NotHaveStatusCode(HttpStatusCode.Gone);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_statuscode_value_not_to_be_equal_to_a_different_value()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().NotHaveStatusCode(HttpStatusCode.OK);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_statuscode_value_not_to_be_equal_to_a_different_value()
        {
            // Act
            Action action = () =>
                new HttpResponseMessage(HttpStatusCode.OK).Should().NotHaveStatusCode(HttpStatusCode.OK, "because we want to test the failure {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected HttpStatusCode not to be HttpStatusCode.OK {value: 200} because we want to test the failure message, but found HttpStatusCode.OK {value: 200}.*");
        }

        public static IEnumerable<object[]> GetSuccessStatusCodes() => GetStatusCodesWithinRange(200, 299);

        public static IEnumerable<object[]> GetRedirectionStatusCodes() => GetStatusCodesWithinRange(300, 399);

        public static IEnumerable<object[]> GetClientErrorStatusCodes() => GetStatusCodesWithinRange(400, 499);

        public static IEnumerable<object[]> GetServerErrorStatusCodes() => GetStatusCodesWithinRange(500, 599);

        private static IEnumerable<object[]> GetStatusCodesWithinRange(int lowerLimit, int upperLimit)
        {
            foreach (HttpStatusCode httpStatusCode in Enum.GetValues(typeof(HttpStatusCode)))
            {
                if ((int)httpStatusCode < lowerLimit || (int)httpStatusCode > upperLimit)
                {
                    continue;
                }

                yield return new object[] { httpStatusCode };
            }
        }
    }
}
