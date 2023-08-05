using System.Diagnostics;
using System.Text.Json;
using FluentAssertions.Execution;

namespace FluentAssertions.Json;

/// <summary>
/// Contains a number of methods to assert that an <see cref="JsonElement" /> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class JsonElementAssertions
{
    public JsonElement Subject { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="JsonElementAssertions" /> class.
    /// </summary>
    /// <param name="subject">The subject.</param>
    public JsonElementAssertions(JsonElement subject)
    {
        Subject = subject;
    }

    /// <summary>
    ///     Asserts that the current <see cref="JsonElement"/> has the specified <see cref="JsonValueKind"/>.
    /// </summary>
    /// <param name="valueKind">The JSON string.</param>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public AndConstraint<JsonElementAssertions> HaveValueKind(JsonValueKind valueKind, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.ValueKind == valueKind)
            .FailWith("Expected {context:JSON} to have value kind {0}{reason}, but found {1}.", valueKind, Subject.ValueKind);

        return new(this);
    }

    public void Be(long jsonNumber)
    {
        Execute.Assertion
            .ForCondition(Subject.ValueKind == JsonValueKind.Number
                && Subject.GetInt64() == jsonNumber)
            .FailWith("Expected {context:JSON} to be a number with value {0}, but got {1} instead.", jsonNumber, Subject.GetInt64());
    }
}
