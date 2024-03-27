#if NET6_0_OR_GREATER
using System;
using System.Text.Json;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Json;

public partial class JsonElementAssertionsSpecs
{
    public class HaveCount
    {
        [Fact]
        public void Fail_for_null_json_element()
        {
            // Arrange
            JsonElement? subject = null;

            // Act
            Action act = () => subject.Should().HaveCount(1, "null is not allowed");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element to contain 1 item(s) because null is not allowed, but it was <null>.");
        }

        [Fact]
        public void Succeed_for_json_object_with_matching_property_count()
        {
            // Arrange
            var subject = JsonDocument.Parse("""{ "id": 42, "admin": true }""");

            // Act / Assert
            subject.Should().HaveCount(2);
        }

        [Fact]
        public void Fail_for_json_object_with_fewer_properties()
        {
            // Arrange
            var subject = JsonDocument.Parse("{ }");

            // Act
            Action act = () => subject.Should().HaveCount(1, "numbers matter");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element * to contain 1 item(s) because numbers matter, but found 0.");
        }

        [Fact]
        public void Fail_for_json_object_with_more_properties()
        {
            // Arrange
            var subject = JsonDocument.Parse("""{ "id": 42, "admin": true }""");

            // Act
            Action act = () => subject.Should().HaveCount(1, "numbers matter");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element * to contain 1 item(s) because numbers matter, but found 2.");
        }

        [Fact]
        public void Succeed_for_json_array_with_matching_array_count()
        {
            // Arrange
            var subject = JsonDocument.Parse("""[ "Hello", "World!" ]""");

            // Act / Assert
            subject.Should().HaveCount(2);
        }

        [Fact]
        public void Fail_for_json_array_with_fewer_array_count()
        {
            // Arrange
            var subject = JsonDocument.Parse("""[ "Hello", "World!" ]""");

            // Act
            Action act = () => subject.Should().HaveCount(3, "numbers matter");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element [ \"Hello\", \"World!\" ] to contain 3 item(s) because numbers matter, but found 2.");
        }

        [Fact]
        public void Fail_for_json_array_with_more_array_count()
        {
            // Arrange
            var subject = JsonDocument.Parse("""[ "Hello", "World!" ]""").RootElement;

            // Act
            Action act = () => subject.Should().HaveCount(1, "numbers matter");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element [ \"Hello\", \"World!\" ] to contain 1 item(s) because numbers matter, but found 2.");
        }

        [Fact]
        public void Fail_for_unsupported_value_type()
        {
            // Arrange
            var subject = JsonDocument.Parse("""{ "id": 42, "admin": true }""")
                .RootElement.GetProperty("id");

            // Act
            Action act = () => subject.Should().HaveCount(0, "type matters");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element 42 to contain 0 item(s) because type matters, but it is of type JsonValueKind.Number*");
        }
    }

    public class NotHaveCount
    {
        [Fact]
        public void Fail_for_null_json_element()
        {
            // Arrange
            JsonElement? subject = null;

            // Act
            Action act = () => subject.Should().NotHaveCount(1, "null is not allowed");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element to not contain 1 item(s) because null is not allowed, but it was <null>.");
        }

        [Fact]
        public void Succeed_for_json_object_with_fewer_properties()
        {
            // Arrange
            var subject = JsonDocument.Parse("""{ "id": 42, "admin": true }""");

            // Act / Assert
            subject.Should().NotHaveCount(3);
        }

        [Fact]
        public void Succeed_for_json_object_with_more_properties()
        {
            // Arrange
            var subject = JsonDocument.Parse("""{ "id": 42, "admin": true }""");

            // Act / Assert
            subject.Should().NotHaveCount(1);
        }

        [Fact]
        public void Fail_for_json_object_with_matching_property_count()
        {
            // Arrange
            var subject = JsonDocument.Parse("{ }");

            // Act
            Action act = () => subject.Should().NotHaveCount(0, "numbers matter");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element * to not contain 0 item(s) because numbers matter, but found them.");
        }

        [Fact]
        public void Succeed_for_json_array_with_fewer_properties()
        {
            // Arrange
            var subject = JsonDocument.Parse("""[ "Hello", "World!" ]""");

            // Act / Assert
            subject.Should().NotHaveCount(3);
        }

        [Fact]
        public void Succeed_for_json_array_with_more_properties()
        {
            // Arrange
            var subject = JsonDocument.Parse("""[ "Hello", "World!" ]""");

            // Act / Assert
            subject.Should().NotHaveCount(1);
        }

        [Fact]
        public void Fail_for_json_array_with_matching_property_count()
        {
            // Arrange
            var subject = JsonDocument.Parse("""[ "Hello", "World!" ]""");

            // Act
            Action act = () => subject.Should().NotHaveCount(2, "numbers matter");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element * to not contain 2 item(s) because numbers matter, but found them.");
        }

        [Fact]
        public void Fail_for_unsupported_value_type()
        {
            // Arrange
            var subject = JsonDocument.Parse("""{ "id": 42, "admin": true }""")
                .RootElement.GetProperty("id");

            // Act
            Action act = () => subject.Should().NotHaveCount(0, "type matters");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected JSON element 42 to not contain 0 item(s) because type matters, but it is of type JsonValueKind.Number*");
        }
    }
}
#endif
