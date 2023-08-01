#if NET6_0_OR_GREATER

using System;
using System.Diagnostics.Contracts;
using System.Text.Json;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Json;

public class JsonSerializerOptionsAssertions : ReferenceTypeAssertions<JsonSerializerOptions, JsonSerializerOptionsAssertions>
{
    public JsonSerializerOptionsAssertions(JsonSerializerOptions subject)
        : base(subject)
    {
    }

    [Pure]
    public AndConstraint<Deserialized<T>> Deserialize<T>(string json, string because = "", params object[] becauseArgs)
    {
        try
        {
            return new(new(JsonSerializer.Deserialize<T>(json, Subject)));
        }
        catch (Exception exception)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the options} to deserialize {0}{reason}, but it failed: {1}.", json, exception.Message);

            throw;
        }
    }

    protected override string Identifier => "options";

    [Pure]
    public AndConstraint<Serialized> Serialize<T>(T value)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, Subject);
        using var doc = JsonDocument.Parse(bytes);
        return new(new(doc.RootElement.Clone()));
    }
}

#endif
