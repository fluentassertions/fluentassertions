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

    /// <summary>
    ///     Asserts that the current <see cref="JsonElement"/> is the JSON null node.
    /// </summary>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public AndConstraint<JsonElementAssertions> BeNull(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.ValueKind == JsonValueKind.Null)
            .FailWith("Expected {context:JSON} to be a JSON null{reason}, but found {0}.", Subject);

        return new(this);
    }

    /// <summary>
    ///     Asserts that the current <see cref="JsonElement"/> is the JSON string node.
    /// </summary>
    /// <param name="value">The value of the JSON string node.</param>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public AndConstraint<JsonElementAssertions> BeString(string value, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.ValueKind == JsonValueKind.String && Subject.GetString() == value)
            .FailWith("Expected {context:JSON} to be a JSON string {0}{reason}, but found {1}.", value, Subject);

        return new(this);
    }

    /// <summary>
    ///     Asserts that the current <see cref="JsonElement"/> is the JSON number node.
    /// </summary>
    /// <param name="value">The value of the JSON number node.</param>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public AndConstraint<JsonElementAssertions> BeNumber(decimal value, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.ValueKind == JsonValueKind.Number && Subject.GetDecimal() == value)
            .FailWith("Expected {context:JSON} to be a JSON string {0}{reason}, but found {1}.", value, Subject);

        return new(this);
    }

    /// <inheritdoc cref="BeNumber(decimal, string, object[])"/>
    public AndConstraint<JsonElementAssertions> BeNumber(long value, string because = "", params object[] becauseArgs)
        => BeNumber((decimal)value, because, becauseArgs);

    /// <summary>
    ///     Asserts that the current <see cref="JsonElement"/> is the JSON true node.
    /// </summary>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public AndConstraint<JsonElementAssertions> BeTrue(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.ValueKind == JsonValueKind.True)
            .FailWith("Expected {context:JSON} to be a JSON true{reason}, but found {0}.", Subject);

        return new(this);
    }

    /// <summary>
    ///     Asserts that the current <see cref="JsonElement"/> is the JSON false node.
    /// </summary>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public AndConstraint<JsonElementAssertions> BeFalse(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.ValueKind == JsonValueKind.False)
            .FailWith("Expected {context:JSON} to be JSON false{reason}, but found {0}.", Subject);

        return new(this);
    }


}
