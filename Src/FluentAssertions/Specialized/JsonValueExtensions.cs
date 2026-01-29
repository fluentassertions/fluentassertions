#if NET6_0_OR_GREATER

using System.Text.Json;
using System.Text.Json.Nodes;

namespace FluentAssertions.Specialized;

[System.Diagnostics.StackTraceHidden]
internal static class JsonValueExtensions
{
    public static bool IsNumeric(this JsonValue value)
    {
        return value.GetValue<JsonElement>().ValueKind == JsonValueKind.Number;
    }
}

#endif

