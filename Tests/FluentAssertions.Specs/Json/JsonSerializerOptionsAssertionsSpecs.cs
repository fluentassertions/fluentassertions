#if NET6_0_OR_GREATER

using System.Text.Json;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Json;

public class JsonSerializerOptionsAssertionsSpecs
{
    public class Deserialize
    {
        [Fact]
        public void Invalid_JSON()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.General);
            var invalidJson = @"""Hello, World!""";

            invalidJson.Invoking(_ => options.Should().Deserialize<int>(invalidJson, because: "why not?"))
                .Should().Throw<XunitException>();
        }

        [Fact]
        public void As_value_type()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.General);
            options.Should().Deserialize<int>("42").And.Value.Should().Be(42);
        }
    }

    public class Serialize
    {
        [Fact]
        public void Value()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.General);

            options.Should().Serialize(true).And.Value.Should().Be(42);
        }
    }
}

#endif
