#if NET6_0_OR_GREATER
using System.Text.Json;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Json;

public partial class JsonElementAssertionsSpecs
{
    public class HaveElement
    {
        [Fact]
        public void Succeed_for_json_object_with_matching_element()
        {
            // Arrange
            var subject = JsonDocument.Parse("""{ "id": 42 }""");

            // Act / Assert
            subject.Should().HaveElement("id");
        }

        [Fact]
        public void Succeed_for_nested_json_object_with_matching_elements()
        {
            // Arrange
            var subject = JsonDocument.Parse("""{ "id": { "name": "foo" } }""");

            // Act / Assert
            subject.Should().HaveElement("id").Which.Should().HaveElement("name");
        }

        [Fact]
        public void Fail_for_json_object_without_matching_element()
        {
            // Arrange
            var subject = JsonDocument.Parse("""{ "id": 42 }""");

            // Act
            var act = () => subject.Should().HaveElement("name", "because element is not defined");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element { \"id\": 42 } to have element \"name\" because element is not defined, but no such element was found.");
        }

        [Fact]
        public void Fail_for_json_object_with_case_different_matching_element()
        {
            // Arrange
            var subject = JsonDocument.Parse("""{ "id": 42 }""");

            // Act
            var act = () => subject.Should().HaveElement("ID", "because casing is different");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element { \"id\": 42 } to have element \"ID\" because casing is different, but no such element was found.");
        }

        [Fact]
        public void Fail_for_null()
        {
            // Arrange
            JsonElement? subject = null;

            // Act
            var act = () => subject.Should().HaveElement("id", "it is null");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element to have element \"id\" because it is null, but it was <null>.");
        }
    }

    public class NotHaveElement
    {
        [Fact]
        public void Succeed_for_json_object_without_matching_element()
        {
            // Arrange
            var subject = JsonDocument.Parse("""{ "id": 42 }""");

            // Act / Assert
            subject.Should().NotHaveElement("name");
        }

        [Fact]
        public void Fail_for_json_object_with_matching_element()
        {
            // Arrange
            var subject = JsonDocument.Parse("""{ "id": 42 }""");

            // Act / Assert
            subject.Should().Invoking(x => x.NotHaveElement("id", "because element is defined"))
                .Should().Throw<XunitException>()
                .WithMessage("Expected JSON element { \"id\": 42 } to have no element \"id\" because element is defined, but element was found with value 42.");
        }

        [Fact]
        public void Succeed_for_null()
        {
            // Arrange
            JsonElement? subject = null;

            // Act / Assert
            subject.Should().NotHaveElement("id");
        }
    }
}
#endif
