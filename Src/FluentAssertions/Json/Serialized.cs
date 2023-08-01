#if NET6_0_OR_GREATER

using System.Text.Json;

namespace FluentAssertions.Json;

public sealed class Serialized
{
    public JsonElement Value { get; }

    internal Serialized(JsonElement subject)
    {
        Value = subject;
    }
}

#endif
