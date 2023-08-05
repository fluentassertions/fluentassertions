using System;
using System.Text.Json;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Json;

public class JsonElementAssertionsSpecs
{
    public class HaveValueKind
    {
        [Fact]
        public void Throws_for_unexcepected_value_kind()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act
            Action act = () => options.Should().Serialize(42)
                .And.Value.Should().HaveValueKind(JsonValueKind.String, because: "why not?");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected options to have value kind JsonValueKind.String {value: 3} because why not?, but found JsonValueKind.Number {value: 4}.");
        }

        [Fact]
        public void Guards_excepected_value_kinds()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act + assert
            options.Should().Serialize(42).And.Value.Should().HaveValueKind(JsonValueKind.Number);
        }
    }

    public class BeNull
    {
        [Fact]
        public void Throws_for_other_elements()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act
            Action act = () => options.Should().Serialize("null").And.Value.Should().BeNull(because: "why not?");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("TODO");
        }

        [Fact]
        public void Guards_null_node()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act + assert
            options.Should().Serialize<object>(null).And.Value.Should().BeNull();
        }
    }

    public class BeString
    {
        [Fact]
        public void Throws_for_other_elements()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act
            Action act = () => options.Should().Serialize<object>(null).And.Value.Should().BeString("null", because: "why not?");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("TODO");
        }

        [Fact]
        public void Guards_string_node()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act + assert
            options.Should().Serialize(Guid.Parse("61088fce-43c5-4327-b591-0d7e862075d8")).And.Value.Should().BeString("61088fce-43c5-4327-b591-0d7e862075d8");
        }
    }

    public class BeNumber
    {
        [Fact]
        public void Throws_for_other_elements()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act
            Action act = () => options.Should().Serialize("42").And.Value.Should().BeNumber(42, because: "why not?");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("TODO");
        }

        [Fact]
        public void Guards_integer_number_node()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act + assert
            options.Should().Serialize(42).And.Value.Should().BeNumber(42);
        }

        [Fact]
        public void Guards_floating_point_number_node()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act + assert
            options.Should().Serialize(42.17).And.Value.Should().BeNumber(42.17m);
        }
    }

    public class BeTrue
    {
        [Fact]
        public void Throws_for_other_elements()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act
            Action act = () => options.Should().Serialize("true").And.Value.Should().BeTrue(because: "why not?");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("TODO");
        }

        [Fact]
        public void Guards_true_node()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act + assert
            options.Should().Serialize(true).And.Value.Should().BeTrue();
        }
    }

    public class BeFalse
    {
        [Fact]
        public void Throws_for_other_elements()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act
            Action act = () => options.Should().Serialize("false").And.Value.Should().BeFalse(because: "why not?");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("TODO");
        }

        [Fact]
        public void Guards_false_node()
        {
            // Arrange
            JsonSerializerOptions options = new();

            // Act + assert
            options.Should().Serialize(false).And.Value.Should().BeFalse();
        }
    }
}
