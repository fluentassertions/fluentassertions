#if NET6_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Nodes;
using FluentAssertions.Common;
using FluentAssertions.Configuration;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Specialized;

public class JsonNodeAssertions<T> : ReferenceTypeAssertions<T, JsonNodeAssertions<T>>
    where T : JsonNode
{
    /// <inheritdoc />
    public JsonNodeAssertions(T subject, AssertionChain assertionChain)
        : base(subject, assertionChain)
    {
    }

    /// <inheritdoc />
    protected override string Identifier
    {
        get => "JSON node";
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> contains a property with the specified name.
    /// </summary>
    /// <param name="code">The name of the property to look for.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<JsonNodeAssertions<T>, JsonNode> HaveProperty(string code,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Cannot assert the existence of a property on a <null> JSON node{reason}.")
            .Then
            .ForCondition(Subject?[code] != null)
            .FailWith("Expected {context:JSON node} to have property {0}{reason}.", code);

        return new AndWhichConstraint<JsonNodeAssertions<T>, JsonNode>(this, Subject?[code]);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> does not have a property with the specified name.
    /// </summary>
    /// <param name="code">The name of the property that should not exist.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<JsonNodeAssertions<T>> NotHaveProperty(string code,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Cannot assert the existence of a property on a <null> JSON node{reason}.")
            .Then
            .ForCondition(Subject?[code] is null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:JSON node} to have property {0}{reason}.", code);

        return new AndConstraint<JsonNodeAssertions<T>>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> represents a JSON array.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<JsonNodeAssertions<T>, IEnumerable<JsonNode>> BeAnArray(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is JsonArray)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:JSON node} to be an array{reason}, but {0} is not.", Subject);

        return new AndWhichConstraint<JsonNodeAssertions<T>, IEnumerable<JsonNode>>(this, (Subject as JsonArray)?.AsEnumerable());
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> does not represent a JSON array.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<JsonNodeAssertions<T>> NotBeAnArray(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is not JsonArray)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:JSON node} to be an array{reason}, but {0} is.", Subject);

        return new AndConstraint<JsonNodeAssertions<T>>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> represents a 32-bit signed integer.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<JsonNodeAssertions<T>, long> BeNumeric(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is JsonValue value && value.TryGetValue<int>(out _))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:JSON node} to be an Int32{reason}, but {0} is not.", Subject);

        var actualValue = Subject is JsonValue jsonValue && jsonValue.TryGetValue<long>(out var result) ? result : 0;
        return new AndWhichConstraint<JsonNodeAssertions<T>, long>(this, actualValue);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> does not represent a 32-bit signed integer.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<JsonNodeAssertions<T>> NotBeNumeric(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(!(Subject is JsonValue value && value.TryGetValue<long>(out _)))
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:JSON node} to be an Int32{reason}, but {0} is.", Subject);

        return new AndConstraint<JsonNodeAssertions<T>>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> represents a local <see cref="DateTime"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<JsonNodeAssertions<T>, DateTime> BeLocalDate(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is JsonValue value && value.TryGetValue<DateTime>(out _) && !Subject.ToString().EndsWith('Z'))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:JSON node} to be a local date{reason}, but {0} is not.", Subject);

        var actualValue = Subject is JsonValue jsonValue && jsonValue.TryGetValue<DateTime>(out var result) ? result : default;
        return new AndWhichConstraint<JsonNodeAssertions<T>, DateTime>(this, actualValue);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> does not represent a local <see cref="DateTime"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<JsonNodeAssertions<T>> NotBeLocalDate(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is not JsonValue value || !value.TryGetValue<DateTime>(out _) || Subject.ToString().EndsWith('Z'))
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:JSON node} to be a local date{reason}, but {0} is.", Subject);

        return new AndConstraint<JsonNodeAssertions<T>>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> represents a UTC <see cref="DateTime"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<JsonNodeAssertions<T>, DateTime> BeUtcDate(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is JsonValue value && value.TryGetValue<DateTime>(out _) && Subject.ToString().EndsWith('Z'))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to be a UTC date{reason}, but {0} is not.", Subject);

        var actualValue = Subject is JsonValue jsonValue && jsonValue.TryGetValue<DateTime>(out var result) ? result : default;
        return new AndWhichConstraint<JsonNodeAssertions<T>, DateTime>(this, actualValue);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> does not represent a UTC <see cref="DateTime"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<JsonNodeAssertions<T>> NotBeUtcDate(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is not JsonValue value || !value.TryGetValue<DateTime>(out _) || !Subject.ToString().EndsWith('Z'))
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context} to be a UTC date{reason}, but {0} is.", Subject);

        return new AndConstraint<JsonNodeAssertions<T>>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> represents a boolean value.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<JsonNodeAssertions<T>, bool> BeBool(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is JsonValue value && value.TryGetValue(out bool _))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} to be a boolean{reason}, but {0} is not.", Subject);

        var actualValue = Subject is JsonValue jsonValue && jsonValue.TryGetValue<bool>(out var result) && result;
        return new AndWhichConstraint<JsonNodeAssertions<T>, bool>(this, actualValue);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> does not represent a boolean value.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<JsonNodeAssertions<T>> NotBeBool(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(!(Subject is JsonValue value && value.TryGetValue<bool>(out _)))
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:JSON node} to be a boolean{reason}, but {0} is.", Subject);

        return new AndConstraint<JsonNodeAssertions<T>>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> represents a string value.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<JsonNodeAssertions<T>, string> BeString(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(Subject is JsonValue value && value.TryGetValue(out string _))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:JSON node} to be a string{reason}, but {0} is not.", Subject);

        var actualValue = Subject is JsonValue jsonValue && jsonValue.TryGetValue<string>(out var result) ? result : null;
        return new AndWhichConstraint<JsonNodeAssertions<T>, string>(this, actualValue);
    }

    /// <summary>
    /// Asserts that the current <see cref="JsonNode"/> does not represent a string value.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<JsonNodeAssertions<T>> NotBeString(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .ForCondition(!(Subject is JsonValue value && value.TryGetValue<string>(out _)))
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:JSON node} to be a string{reason}, but {0} is.", Subject);

        return new AndConstraint<JsonNodeAssertions<T>>(this);
    }

    /// <summary>
    /// Asserts a JSON node is equivalent to a given C# object graph.
    /// </summary>
    /// <remarks>
    /// Traverses the properties of the <paramref name="expectation"/> object graph, which can contain nested objects and collections, and compares them to
    /// the properties the JSON node. Two properties are considered equal when their names and values match. The comparison is
    /// recursive, so nested properties and collections are compared in the same way. The comparison is driven by the expectation, so
    /// if the JSON node contains extra properties, they are ignored. If the JSON node has a property that contains an ISO 8601 date (local or UTC), it can be
    /// compared to a <see cref="DateTime"/> property of the expectation.
    ///
    /// The default global configuration can be modified through <see cref="GlobalConfiguration.Equivalency"/>.
    /// </remarks>
    /// <param name="expectation">The expected object graph, such as an anonymous type.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<JsonNodeAssertions<T>> BeEquivalentTo<TExpectation>(TExpectation expectation,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return BeEquivalentTo(expectation, config => config, because, becauseArgs);
    }

    /// <summary>
    /// Asserts a JSON node is equivalent to a given C# object graph.
    /// </summary>
    /// <remarks>
    /// Traverses the properties of the <paramref name="expectation"/> object graph, which can contain nested objects and collections, and compares them to
    /// the properties the JSON node. Two properties are considered equal when their names and values match. The comparison is
    /// recursive, so nested properties and collections are compared in the same way. The comparison is case-sensitive, unless you use
    /// <see cref="SelfReferenceEquivalencyOptions{TSelf}.IgnoreJsonPropertyCasing"/>. The comparison is driven by the expectation, so
    /// if the JSON node contains extra properties, they are ignored. If the JSON node has a property that contains an ISO 8601 date (local or UTC), it can be
    /// compared to a <see cref="DateTime"/> property of the expectation.
    ///
    /// The default configuration can be modified by using the many options available through the <paramref name="config"/> delegate. Also,
    /// global options can be set through <see cref="GlobalConfiguration.Equivalency"/>.
    /// </remarks>
    /// <param name="expectation">The expected object graph, such as an anonymous type.</param>
    /// <param name="config">
    /// A reference to the <see cref="EquivalencyOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<JsonNodeAssertions<T>> BeEquivalentTo<TExpectation>(TExpectation expectation,
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>> config,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        EquivalencyOptions<TExpectation> options =
            config(AssertionConfiguration.Current.Equivalency.CloneDefaults<TExpectation>());

        var context = new EquivalencyValidationContext(Node.From<TExpectation>(() =>
            CurrentAssertionChain.CallerIdentifier), options)
        {
            Reason = new Reason(because, becauseArgs),
            TraceWriter = options.TraceWriter
        };

        var comparands = new Comparands
        {
            Subject = Subject,
            Expectation = expectation,
            CompileTimeType = typeof(TExpectation),
        };

        new EquivalencyValidator().AssertEquality(comparands, context);

        return new AndConstraint<JsonNodeAssertions<T>>(this);
    }

    /// <summary>
    /// Asserts a JSON node is NOT equivalent to a given C# object graph.
    /// </summary>
    /// <remarks>
    /// Traverses the properties of the <paramref name="unexpected"/> object graph, which can contain nested objects and collections, and compares them to
    /// the properties the JSON node. Two properties are considered equal when their names and values match. The comparison is
    /// recursive, so nested properties and collections are compared in the same way. The comparison is driven by the expectation, so
    /// if the JSON node contains extra properties, they are ignored. If the JSON node has a property that contains an ISO 8601 date (local or UTC), it can be
    /// compared to a <see cref="DateTime"/> property of the expectation.
    ///
    /// The default global configuration can be modified through <see cref="GlobalConfiguration.Equivalency"/>.
    /// </remarks>
    /// <param name="unexpected">The expected object graph, such as an anonymous type.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<JsonNodeAssertions<T>> NotBeEquivalentTo<TExpectation>(
        TExpectation unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return NotBeEquivalentTo(unexpected, config => config, because, becauseArgs);
    }

    /// <summary>
    /// Asserts a JSON node is NOT equivalent to a given C# object graph.
    /// </summary>
    /// <remarks>
    /// Traverses the properties of the <paramref name="unexpected"/> object graph, which can contain nested objects and collections, and compares them to
    /// the properties the JSON node. Two properties are considered equal when their names and values match. The comparison is
    /// recursive, so nested properties and collections are compared in the same way. The comparison is case-sensitive, unless you use
    /// <see cref="SelfReferenceEquivalencyOptions{TSelf}.IgnoreJsonPropertyCasing"/>. The comparison is driven by the expectation, so
    /// if the JSON node contains extra properties, they are ignored. If the JSON node has a property that contains an ISO 8601 date (local or UTC), it can be
    /// compared to a <see cref="DateTime"/> property of the expectation.
    ///
    /// The default configuration can be modified by using the many options available through the <paramref name="config"/> delegate. Also,
    /// global options can be set through <see cref="GlobalConfiguration.Equivalency"/>.
    /// </remarks>
    /// <param name="unexpected">The expected object graph, such as an anonymous type.</param>
    /// <param name="config">
    /// A reference to the <see cref="EquivalencyOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<JsonNodeAssertions<T>> NotBeEquivalentTo<TExpectation>(
        TExpectation unexpected,
        Func<EquivalencyOptions<TExpectation>, EquivalencyOptions<TExpectation>> config,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(config);

        bool hasMismatches;

        using (var scope = new AssertionScope())
        {
            BeEquivalentTo(unexpected, config);
            hasMismatches = scope.Discard().Length > 0;
        }

        CurrentAssertionChain
            .ForCondition(hasMismatches)
            .BecauseOf(because, becauseArgs)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Did not expect {context:JSON node} to be equivalent to {0}{reason}, but they are.", unexpected);

        return new AndConstraint<JsonNodeAssertions<T>>(this);
    }
}

#endif
