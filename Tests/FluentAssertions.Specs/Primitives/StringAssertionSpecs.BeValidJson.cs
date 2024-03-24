#if NET6_0_OR_GREATER
using System;
using System.Text.Json;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class StringAssertionSpecs
{
    public class BeValidJson
    {
        [Fact]
        public void Allow_consecutive_assertions()
        {
            // Arrange
            string subject = """{ "id": 42, "admin": true }""";

            // Act
            object which = subject.Should().BeValidJson().Which;

            // Assert
            which.Should().BeAssignableTo<JsonDocument>();
        }

        [Fact]
        public void Fail_for_null_string()
        {
            // Arrange
            string subject = null;

            // Act
            Action act = () => subject.Should().BeValidJson("null is not allowed");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be valid JSON because null is not allowed, but found null.");
        }

        [Fact]
        public void Fail_for_empty_string()
        {
            // Arrange
            string subject = "";

            // Act
            Action act = () => subject.Should().BeValidJson("empty string is not allowed");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be valid JSON because empty string is not allowed, but parsing failed*");
        }

        [Fact]
        public void Fail_for_invalid_string()
        {
            // Arrange
            string subject = "invalid json";

            // Act
            Action act = () => subject.Should().BeValidJson("we like {0}", "JSON");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be valid JSON because we like JSON, but parsing failed*");
        }

        [Theory]
        [InlineData("""{{"id":1}""")]
        [InlineData("""{"id":1}}""")]
        [InlineData("""[[{"id":1}]""")]
        [InlineData("""[{"id":1}]]""")]
        public void Fail_for_string_with_unmatched_paranthesis(string subject)
        {
            // Act
            Action act = () => subject.Should().BeValidJson("it contains unmatched paranthesis");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be valid JSON because it contains unmatched paranthesis, but parsing failed*");
        }

        [Fact]
        public void Succeed_for_empty_object_string()
        {
            // Arrange
            string subject = "{}";

            // Act / Assert
            subject.Should().BeValidJson();
        }

        [Theory]
        [InlineData("""{ "id": 42, "admin": true }""")]
        [InlineData("""[{ "id": 1 }, { "id": 2 }]""")]
        public void Succeed_for_valid_string(string subject)
        {
            // Act / Assert
            subject.Should().BeValidJson();
        }

        [Fact]
        public void Fail_with_trailing_commas_when_not_allowed_by_provided_options()
        {
            // Arrange
            string subject = """{"values":[1,2,3,]}""";

            // Act
            Action act = () => subject.Should().BeValidJson(
                new JsonDocumentOptions { AllowTrailingCommas = false },
                "trailing commas are not allowed");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be valid JSON because trailing commas are not allowed, but parsing failed*");
        }

        [Fact]
        public void Succeed_with_trailing_commas_when_allowed_by_provided_options()
        {
            // Arrange
            string subject = """{"values":[1,2,3,]}""";

            // Act / Assert
            subject.Should().BeValidJson(
                new JsonDocumentOptions { AllowTrailingCommas = true });
        }
    }
}
#endif
