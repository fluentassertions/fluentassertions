#if NET6_0_OR_GREATER
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Json;

/// <summary>
/// Contains a number of methods to assert that a <see cref="JsonElement"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class JsonElementAssertions : JsonElementAssertions<JsonElementAssertions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonElementAssertions"/> class.
    /// </summary>
    public JsonElementAssertions(JsonElement? value)
        : base(value)
    {
    }
}

/// <summary>
/// Contains a number of methods to assert that a <see cref="JsonElement"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class JsonElementAssertions<TAssertions> : ReferenceTypeAssertions<JsonElement?, TAssertions>
    where TAssertions : JsonElementAssertions<TAssertions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonElementAssertions{TAssertions}"/> class.
    /// </summary>
    public JsonElementAssertions(JsonElement? value)
        : base(value)
    {
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => nameof(JsonElement);

    /// <summary>
    /// Asserts that the number of items in the current <see cref="JsonElement" /> matches the supplied <paramref name="expected" /> amount.
    /// </summary>
    /// <param name="expected">The expected number of items in the current <see cref="JsonElement" />.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveCount(int expected,
        string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected JSON element to contain {0} item(s){reason}, but it was <null>.", expected);

        Execute.Assertion
            .ForCondition(Subject?.ValueKind is JsonValueKind.Array or JsonValueKind.Object)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected JSON element {0} to contain {1} item(s){reason}, but it is of type {2}.", Subject, expected, Subject.Value.ValueKind);

        int? count = Subject.Value.ValueKind switch
        {
            JsonValueKind.Array => Subject.Value.GetArrayLength(),
            JsonValueKind.Object => Subject.Value.EnumerateObject().Count(),
            _ => 0
        };
        Execute.Assertion
            .ForCondition(count == expected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected JSON element {0} to contain {1} item(s){reason}, but found {2}.", Subject, expected, count);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the number of items in the current <see cref="JsonElement" /> does not match the supplied <paramref name="unexpected" /> amount.
    /// </summary>
    /// <param name="unexpected">The number of items not expected in the current <see cref="JsonElement" />.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveCount(int unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected JSON element to not contain {0} item(s){reason}, but it was <null>.", unexpected);

        Execute.Assertion
            .ForCondition(Subject?.ValueKind is JsonValueKind.Array or JsonValueKind.Object)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected JSON element {0} to not contain {1} item(s){reason}, but it is of type {2}.", Subject, unexpected, Subject.Value.ValueKind);

        int? count = Subject.Value.ValueKind switch
        {
            JsonValueKind.Array => Subject.Value.GetArrayLength(),
            JsonValueKind.Object => Subject.Value.EnumerateObject().Count(),
            _ => 0
        };
        Execute.Assertion
            .ForCondition(count != unexpected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected JSON element {0} to not contain {1} item(s){reason}, but found them.", Subject, unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonElement" /> has a direct child element with the specified
    /// <paramref name="expected" /> name.
    /// </summary>
    /// <param name="expected">
    /// The name of the expected child element
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, JsonElement> HaveElement(string expected,
        string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected JSON element to have element \"" + expected.EscapePlaceholders() + "\"{reason}, but it was <null>.", expected);

        JsonElement property = default;
        bool? isFound = Subject?.TryGetProperty(expected, out property);

        Execute.Assertion
            .ForCondition(isFound == true)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected JSON element {0} to have element \"" + expected.EscapePlaceholders() + "\"{reason}" +
                      ", but no such element was found.", Subject);

        return new AndWhichConstraint<TAssertions, JsonElement>((TAssertions)this, property);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonElement" /> has no direct child element with the specified
    /// <paramref name="expected" /> name.
    /// </summary>
    /// <param name="expected">
    /// The name of the expected child element
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveElement(string expected,
        string because = "", params object[] becauseArgs)
    {
        JsonElement property = default;
        bool? isFound = Subject?.TryGetProperty(expected, out property);

        Execute.Assertion
            .ForCondition(isFound != true)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected JSON element {0} to have no element \"" + expected.EscapePlaceholders() + "\"{reason}" +
                      ", but element was found with value {1}.", Subject, property);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }
}
#endif
