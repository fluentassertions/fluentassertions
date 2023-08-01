#if NET6_0_OR_GREATER

using System.Text.Json;
using FluentAssertions.Execution;

namespace FluentAssertions.Json;

public class JsonElementAssertions
{
    public JsonElement Subject { get; }

    public JsonElementAssertions(JsonElement subject)
    {
        Subject = subject;
    }

    public void Be(long jsonNumber)
    {
        Execute.Assertion
            .ForCondition(Subject.ValueKind == JsonValueKind.Number
                && Subject.GetInt64() == jsonNumber)
            .FailWith("Expected {context:JSON} to be a number with value {0}, but got {1} instead.", jsonNumber, Subject.GetInt64());
    }
}

#endif
