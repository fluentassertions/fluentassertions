#if NET6_0_OR_GREATER

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace FluentAssertions.Specialized;

[StackTraceHidden]
internal static class JsonValueExtensions
{
    public static bool IsNumeric(this JsonValue value)
    {
#if NET8_0_OR_GREATER
        return value.GetValueKind() == JsonValueKind.Number;
#else
        return GetJsonElement(value).ValueKind == JsonValueKind.Number;

        static JsonElement GetJsonElement(JsonValue value)
        {
            if (value.TryGetValue(out JsonElement element))
            {
                return element;
            }

            // Manually constructed JsonNode value, does not have an underlying JsonElement
            return JsonSerializer.SerializeToElement(value);
        }
#endif
    }
}

#endif

