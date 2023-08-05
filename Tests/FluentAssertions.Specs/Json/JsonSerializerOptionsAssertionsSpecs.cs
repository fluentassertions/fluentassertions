using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Json;

public class JsonSerializerOptionsAssertionsSpecs
{
    public class Deserialize
    {
        [Fact]
        public void Throws_for_null_options()
        {
            // Arrange
            JsonSerializerOptions options = null;

            // Act
            Action act = () => options.Should().Deserialize<int>("42");

            act.Should().Throw<XunitException>()
                .WithMessage("Can not use options to deserialize from JSON as it is <null>.");
        }

        [Fact]
        public void Throws_for_null_JSON()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act
            Action act = () => options.Should().Deserialize<int>(json: null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.*");
        }

        [Fact]
        public void Throws_for_invalid_JSON()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act
            Action act = () => options.Should().Deserialize<int>(json: @"""Hello, World!""", because: "why not?");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected options to deserialize \"\"Hello, World!\"\" because why not?, but it failed: " +
                    "*The JSON value could not be converted to System.Int32.*");
        }

        [Fact]
        public void Guards_valid_JSON()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act + assert
            options.Should().Deserialize<int>("42");
        }
    }

    public class Serialize
    {
        [Fact]
        public void Throws_for_null_options()
        {
            // Arrange
            JsonSerializerOptions options = null;

            // Act
            Action act = () => options.Should().Serialize(42);

            act.Should().Throw<XunitException>()
                .WithMessage("Can not use options to serialize to JSON as it is <null>.");
        }

        [Fact]
        public void Throws_for_unserializable_value()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act
            Action act = () => options.Should().Serialize(new Unserializable(), because: "Why not?");

            act.Should().Throw<XunitException>()
                .WithMessage("Expected options to serialize * because Why not?, but it failed: *");
        }

        [Fact]
        public void Guards_serializable_values()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act + assert
            options.Should().Serialize(42);
        }

        private sealed class Unserializable
        {
            [JsonPropertyName("name")]
            public string Value => throw new NotSupportedException();
        }
    }
}
