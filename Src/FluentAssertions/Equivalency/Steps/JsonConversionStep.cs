#if NET6_0_OR_GREATER

using System;
using System.Globalization;
using System.Text.Json.Nodes;

namespace FluentAssertions.Equivalency.Steps;

public class JsonConversionStep : IEquivalencyStep
{
    /// <inheritdoc />
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        if (comparands.Subject is JsonValue json)
        {
            if (json.TryGetValue(out long longValue))
            {
                comparands.Subject = longValue;
            }
            else if (json.TryGetValue(out ulong ulongValue))
            {
                comparands.Subject = ulongValue;
            }
            else if (json.TryGetValue(out double doubleValue))
            {
                comparands.Subject = doubleValue;
            }
            else if (json.TryGetValue(out bool boolValue))
            {
                comparands.Subject = boolValue;
            }
            else if (json.TryGetValue(out string stringValue))
            {
                string[] iso8601Formats =
                {
                    "yyyy-MM-ddTHH:mm:ssZ",
                    "yyyy-MM-ddTHH:mm:ss.fffZ",
                    "yyyy-MM-ddTHH:mm:ss",
                    "yyyy-MM-ddTHH:mm:ss.fff",
                    "yyyy-MM-dd"
                };

                var style = stringValue.EndsWith('Z') ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.AssumeLocal;
                if (DateTime.TryParseExact(stringValue, iso8601Formats, CultureInfo.InvariantCulture, style,
                        out DateTime exactResult))
                {
                    comparands.Subject = exactResult;
                }
                else
                {
                    comparands.Subject = stringValue;
                }
            }
            else
            {
                // We don't need to do anything
            }
        }

        return EquivalencyResult.ContinueWithNext;
    }
}

#endif
