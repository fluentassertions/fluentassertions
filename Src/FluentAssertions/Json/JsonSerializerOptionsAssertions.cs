using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Json;

/// <summary>
///     Contains a number of methods to assert that an <see cref="JsonSerializerOptions" /> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class JsonSerializerOptionsAssertions : ReferenceTypeAssertions<JsonSerializerOptions, JsonSerializerOptionsAssertions>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="JsonSerializerOptionsAssertions" /> class.
    /// </summary>
    /// <param name="subject">The subject.</param>
    public JsonSerializerOptionsAssertions(JsonSerializerOptions subject)
        : base(subject)
    {
    }

    protected override string Identifier => "options";

    /// <summary>
    ///     Asserts that the current <see cref="JsonSerializerOptions"/> can be used to deserialize the specified JSON string.
    /// </summary>
    /// <typeparam name="T">The type to serialize to.</typeparam>
    /// <param name="json">The JSON string.</param>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="json"/> is <see langword="null"/>.</exception>
    public AndWhichConstraint<JsonSerializerOptionsAssertions, T> Deserialize<T>(Stream json, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(json);

        Execute.Assertion
            .ForCondition(Subject is { })
            .FailWith("Can not use {context} to deserialize from JSON as it is <null>.");

        T deserialzed = TryDeserialize<T>(json, out Exception failure);

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(failure is null)
            .FailWith("Expected {context:the options} to deserialize {0}{reason}, but it failed: {1}.", json, failure?.Message);

        return new AndWhichConstraint<JsonSerializerOptionsAssertions, T>(this, deserialzed);
    }

    /// <inheritdoc cref="Deserialize{T}(Stream, string, object[])"/>
    public AndWhichConstraint<JsonSerializerOptionsAssertions, T> Deserialize<T>(string json, string because = "", params object[] becauseArgs)
    {
        Stream stream = json is null ? null : new MemoryStream(Encoding.UTF8.GetBytes(json));
        return Deserialize<T>(stream, because, becauseArgs);
    }

    /// <summary>
    ///     Asserts that the current <see cref="JsonSerializerOptions"/> can be used to serialize the specified value.
    /// </summary>
    /// <typeparam name="T">The type to serialize to.</typeparam>
    /// <param name="value">The value to serialize.</param>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public AndWhichConstraint<JsonSerializerOptionsAssertions, JsonElement> Serialize<T>(T value, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject is { })
            .FailWith("Can not use {context} to serialize to JSON as it is <null>.");

        JsonElement serialized = TrySerialize(value, out Exception failure);

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(failure is null)
            .FailWith("Expected {context:the options} to serialize {0}{reason}, but it failed: {1}.", value, failure?.Message);

        return new AndWhichConstraint<JsonSerializerOptionsAssertions, JsonElement>(this, serialized);
    }

    private T TryDeserialize<T>(Stream json, out Exception failure)
    {
        try
        {
            failure = null;
            return JsonSerializer.Deserialize<T>(json, Subject);
        }
        catch (Exception exception)
        {
            failure = exception;
            return default;
        }
    }

    private JsonElement TrySerialize<T>(T value, out Exception failure)
    {
        try
        {
            failure = null;
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, Subject);
            using var doc = JsonDocument.Parse(bytes);
            return doc.RootElement.Clone();
        }
        catch (Exception exception)
        {
            failure = exception;
            return default;
        }
    }
}
