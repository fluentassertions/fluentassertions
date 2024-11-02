#if NET6_0_OR_GREATER
#nullable enable

using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

public class JsonElementEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(
        Comparands comparands,
        IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        // We sometimes need to parse into JsonDocument, and those need to be disposed.
        JsonDocument? subjectDocument = null;
        JsonDocument? expectationDocument = null;
        try
        {
            // Convert the subject and expectation to JsonElement from the supported types
            JsonElement? subject = null;
            switch (comparands.Subject)
            {
                case JsonElement jsonElement:
                    subject = jsonElement;
                    break;
                case JsonDocument jsonDocument:
                    subject = jsonDocument.RootElement;
                    break;
                case JsonNode jsonNode:
                    // Ensure the document can be properly disposed
                    subjectDocument = JsonDocument.Parse(jsonNode.ToJsonString());
                    subject = subjectDocument.RootElement;
                    break;
            }

            JsonElement? expectation = null;
            switch (comparands.Expectation)
            {
                case JsonElement jsonElement:
                    expectation = jsonElement;
                    break;
                case JsonDocument jsonDocument:
                    expectation = jsonDocument.RootElement;
                    break;
                case JsonNode jsonNode:
                    // Ensure the document can be properly disposed
                    expectationDocument = JsonDocument.Parse(jsonNode.ToJsonString());
                    expectation = expectationDocument.RootElement;
                    break;
            }

            // Should we consider running JsonSerializer.SerializeToDocument(obj) on the other value if only one is a JsonElement like type?
            if (subject.HasValue && expectation.HasValue)
            {
                HandleElementCompare(subject.Value, expectation.Value, context, valueChildNodes);
                return EquivalencyResult.EquivalencyProven;
            }

            return EquivalencyResult.ContinueWithNext;
        }
        finally
        {
            subjectDocument?.Dispose();
            expectationDocument?.Dispose();
        }
    }

    private static void HandleElementCompare(JsonElement subject, JsonElement expectation, IEquivalencyValidationContext context, IValidateChildNodeEquivalency valueChildNodes)
    {
        var assertionChain = AssertionChain.GetOrCreate().For(context);

        assertionChain
            .ForCondition(subject.ValueKind == expectation.ValueKind)
            .FailWith("Expected JsonType to be {0}, but found {1}", expectation.ValueKind.ToString(), subject.ValueKind.ToString());

        switch (subject.ValueKind)
        {
            case JsonValueKind.Array:
                HandleArrayCompare(subject, expectation, context, valueChildNodes, assertionChain);
                break;
            case JsonValueKind.Object:
                HandleObjectCompare(subject, expectation, context, valueChildNodes, assertionChain);
                break;
            case JsonValueKind.String:
                HandleStringCompare(subject, expectation, context, valueChildNodes);
                break;
            case JsonValueKind.Number:
                HandleNumberCompare(subject, expectation, context, valueChildNodes);
                break;
            default:
                // true, false, null, undefined has no value to compare,
                // so if both have the same ValueKind, the json are equal
                break;
        }
    }

    private static void HandleNumberCompare(JsonElement subject, JsonElement expectation, IEquivalencyValidationContext context, IValidateChildNodeEquivalency valueChildNodes)
    {
        // Json does not specify a range for numbers, but allows scientific syntax
        // (1E2 for 100), so we can't just compare strings.
        // The solution here is to run multiple checks
        // 1. Check if they are both representable as 64-bit integers and compare them
        // 2. If not, check if they are equal in 64bit floating point
        // 3. Then compare the actual raw text to get a proper error message.
        if (!NumbersEqualsIgnoreInfAndNan(subject, expectation))
        {
            var subjectString = subject.GetRawText();
            var expectationString = expectation.GetRawText();
            valueChildNodes.AssertEquivalencyOf(new Comparands(subjectString, expectationString, typeof(string)), context);
        }
    }

    private static bool NumbersEqualsIgnoreInfAndNan(JsonElement actual, JsonElement expected)
    {
        // This check ensures that integers between 2^53 and 2^61 are compared with full precision
        if (actual.TryGetInt64(out var actualInt) && expected.TryGetInt64(out var expectedInt))
        {
            return actualInt == expectedInt;
        }

        var actualDouble = actual.GetDouble();
        var expectedDouble = expected.GetDouble();
        if (
            double.IsInfinity(actualDouble) ||
            double.IsInfinity(expectedDouble) ||
            double.IsNaN(actualDouble) ||
            double.IsNaN(expectedDouble))
        {
            return false;
        }

        return actualDouble.Equals(expectedDouble);
    }

    private static void HandleStringCompare(JsonElement subject, JsonElement expectation, IEquivalencyValidationContext context, IValidateChildNodeEquivalency valueChildNodes)
    {
        var subjectString = subject.GetString();
        var expectationString = expectation.GetString();
        valueChildNodes.AssertEquivalencyOf(new Comparands(subjectString, expectationString, typeof(string)), context);
    }

    private static void HandleObjectCompare(JsonElement subject, JsonElement expectation, IEquivalencyValidationContext context, IValidateChildNodeEquivalency valueChildNodes, AssertionChain assertionChain)
    {
        int expectedCount = 0;
        foreach (JsonProperty expectedProperty in expectation.EnumerateObject())
        {
            // How would you configure the property matching to be case insensitive?
            if (!subject.TryGetProperty(expectedProperty.Name, out JsonElement subjectElement))
            {
                assertionChain
                    .ForCondition(true)
                    .FailWith("Expected property {0} not found", expectedProperty.Name);
            }

            valueChildNodes.AssertEquivalencyOf(
                new Comparands(subjectElement, expectedProperty.Value, typeof(JsonElement)),
                context.AsDictionaryItem<string, JsonElement>(expectedProperty.Name));
            expectedCount++;
        }

        // All expected properties are found, now check if there are any extra properties in the subject
        var subjectCount = subject.EnumerateObject().Count();
        assertionChain
            .ForCondition(subjectCount == expectedCount)
            .FailWith("Expected object to have length {0}, but found {0} properties", expectedCount, subjectCount);
    }

    private static void HandleArrayCompare(JsonElement subject, JsonElement expectation, IEquivalencyValidationContext context, IValidateChildNodeEquivalency valueChildNodes, AssertionChain assertionChain)
    {
        assertionChain
            .ForCondition(subject.GetArrayLength() == expectation.GetArrayLength())
            .FailWith("Expected array length {0} but found {1}", expectation.GetArrayLength(), subject.GetArrayLength());

        int index = 0;
        foreach (var (expectedElement, subjectElement) in expectation.EnumerateArray().Zip(subject.EnumerateArray()))
        {
            index++;
            valueChildNodes.AssertEquivalencyOf(
                new Comparands(subjectElement, expectedElement, typeof(JsonElement)),
                context.AsCollectionItem<JsonElement>(index));
        }
    }
}
#endif
