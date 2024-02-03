#if NET6_0_OR_GREATER
namespace FluentAssertions.Equivalency.Steps;

using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentAssertions.Execution;

public class JsonElementEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
    {
        JsonElement? subject = comparands.Subject switch
        {
            JsonElement s => s,
            JsonDocument d => d.RootElement,
            JsonNode n => NodeToElement(n),
            _ => null,
        };

        JsonElement? expectation = comparands.Expectation switch
        {
            JsonElement s => s,
            JsonDocument d => d.RootElement,
            JsonNode n => NodeToElement(n),
            _ => null,
        };

        if (subject is not null && expectation is not null)
        {
            HandleElementCompare(subject.Value, expectation.Value, context, nestedValidator);
            return EquivalencyResult.AssertionCompleted;
        }

        return EquivalencyResult.ContinueWithNext;
    }

    private static JsonElement? NodeToElement(JsonNode jsonNode)
    {
        using var document = JsonDocument.Parse(jsonNode.ToJsonString());
        return document.RootElement.Clone();
    }

    private static void HandleElementCompare(JsonElement subject, JsonElement expectation, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
    {
        AssertionScope.Current
            .ForCondition(subject.ValueKind == expectation.ValueKind)
            .FailWith("Expected type to be {0}, but found {1}", expectation.ValueKind, subject.ValueKind);

        switch (subject.ValueKind)
        {
            case JsonValueKind.Array:
                HandleArrayCompare(subject, expectation, context, nestedValidator);
                break;
            case JsonValueKind.Object:
                HandleObjectCompare(subject, expectation, context, nestedValidator);
                break;
            case JsonValueKind.String:
                HandleStringCompare(subject, expectation, context, nestedValidator);
                break;
            case JsonValueKind.Number:
                HandleNumberCompare(subject, expectation, context, nestedValidator);
                break;
            default:
                // true, false, null, undefined has no value to compare,
                // so if both have the same ValueKind, the json are equal
                break;
        }
    }

    private static void HandleNumberCompare(JsonElement subject, JsonElement expectation, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
    {
        // Json does not specify a range for numbers, but allows scientific syntax
        // (1E2 for 100), so we can't just compare strings.
        // The solution here is to first check if the numbers are equal in 64bit floating point
        // then compare the actual raw text to get a proper error message.
        if (!NumbersEqualsIgnoreInfAndNan(subject.GetDouble(), expectation.GetDouble()))
        {
            var subjectString = subject.GetRawText();
            var expectationString = expectation.GetRawText();
            nestedValidator.RecursivelyAssertEquality(new Comparands(subjectString, expectationString, typeof(double)), context);
        }
    }

    private static bool NumbersEqualsIgnoreInfAndNan(double actual, double expected)
    {
        if (
            double.IsInfinity(actual) ||
            double.IsInfinity(expected) ||
            double.IsNaN(actual) ||
            double.IsNaN(expected))
        {
            return false;
        }

        return actual.Equals(expected);
    }

    private static void HandleStringCompare(JsonElement subject, JsonElement expectation, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
    {
        var subjectString = subject.GetString();
        var expectationString = expectation.GetString();
        nestedValidator.RecursivelyAssertEquality(new Comparands(subjectString, expectationString, typeof(string)), context);
    }

    private static void HandleObjectCompare(JsonElement subject, JsonElement expectation, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
    {
        int expectedCount = 0;
        foreach (var expectedProperty in expectation.EnumerateObject())
        {
            var expectedPropertyName = expectedProperty.Name;
            expectedCount++;
            if (!subject.TryGetProperty(expectedPropertyName, out var subjectElement))
            {
                AssertionScope.Current
                    .ForCondition(true)
                    .FailWith("Expected property {0} not found", expectedPropertyName);
            }

            nestedValidator.RecursivelyAssertEquality(
                new Comparands(subjectElement, expectedProperty.Value, typeof(JsonElement)),
                context.AsDictionaryItem<string, JsonElement>(expectedPropertyName));
        }

        // All expected properties are found, now check if there are any extra properties in the subject
        var subjectCount = subject.EnumerateObject().Count();
        AssertionScope.Current
            .ForCondition(subjectCount == expectedCount)
            .FailWith("Expected object to have length {0}, but found {0} properties", expectedCount, subjectCount);
    }

    private static void HandleArrayCompare(JsonElement subject, JsonElement expectation, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
    {
        AssertionScope.Current
            .ForCondition(subject.GetArrayLength() == expectation.GetArrayLength())
            .FailWith("Expected array length {0} but found {1}", expectation.GetArrayLength(), subject.GetArrayLength());

        int index = 0;
        foreach (var (expectedElement, subjectElement) in expectation.EnumerateArray().Zip(subject.EnumerateArray()))
        {
            index++;
            nestedValidator.RecursivelyAssertEquality(
                new Comparands(subjectElement, expectedElement, typeof(JsonElement)),
                context.AsCollectionItem<JsonElement>(index));
        }
    }
}
#endif
