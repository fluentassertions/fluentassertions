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
                .And.Value.Should().HaveValueKind(JsonValueKind.String, because: "Why not?");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected options to have value kind JsonValueKind.String {value: 3} because Why not?, but found JsonValueKind.Number {value: 4}");
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
}
