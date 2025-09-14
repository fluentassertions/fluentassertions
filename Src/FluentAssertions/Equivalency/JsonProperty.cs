#if NET6_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency;

/// <summary>
/// Represents a JSON property in the context of equivalency assertions. This class is used to encapsulate
/// information about a specific JSON property, its parent object, and its counterpart in an expectation object
/// when performing comparisons.
/// </summary>
internal class JsonProperty(JsonNode property, JsonObject parent, INode expectationParent) : IMember
{
    // SMELL: A lot of properties are required by the IMember interface, but they are not used. In the future
    // we need to change the interface that IMemberMatchingRule returns so we don't need all this.

    /// <inheritdoc />
    public GetSubjectId GetSubjectId
    {
        get => expectationParent.GetSubjectId;
    }

    /// <inheritdoc />
    public Type Type
    {
        get => property.GetType();
    }

    /// <inheritdoc />
    public Type ParentType
    {
        get => parent.GetType();
    }

    /// <inheritdoc />
    public Pathway Subject
    {
        get;
        set;
    }

    /// <inheritdoc />
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public Pathway Expectation { get; }

    /// <inheritdoc />
    public int Depth
    {
        get => 0;
    }

    /// <inheritdoc />
    public bool IsRoot
    {
        get => false;
    }

    /// <inheritdoc />
    public bool RootIsCollection
    {
        get => false;
    }

    /// <inheritdoc />
    public void AdjustForRemappedSubject(IMember subjectMember)
    {
    }

    /// <inheritdoc />
    public Type DeclaringType
    {
        get => parent.GetType();
    }

    /// <inheritdoc />
    public Type ReflectedType
    {
        get => parent.GetType();
    }

    /// <inheritdoc />
    public object GetValue(object obj) => property;

    /// <inheritdoc />
    public CSharpAccessModifier GetterAccessibility
    {
        get => CSharpAccessModifier.Public;
    }

    /// <inheritdoc />
    public CSharpAccessModifier SetterAccessibility
    {
        get => CSharpAccessModifier.Public;
    }

    /// <inheritdoc />
    public bool IsBrowsable
    {
        get => false;
    }

    /// <summary>
    /// Tries to find a JSON property on the <paramref name="subject"/> object which name matches that
    /// of the .NET member identified by <paramref name="expectedMember"/>.
    /// </summary>
    /// <remarks>
    /// Whether the name matching is case-sensitive or not is determined by <paramref name="nameComparisonMode"/>.
    /// </remarks>
    /// <returns>
    /// An instance of <see cref="JsonProperty"/> if a matching property was found, or <c>null</c> otherwise.
    /// </returns>
    public static JsonProperty Find(IMember expectedMember, INode parent, object subject, StringComparison nameComparisonMode)
    {
        if (subject is not JsonObject jsonNode)
        {
            return null;
        }

        JsonProperty property = null;

        // Use the name of the expectation as the name of the JSON property to look for
        string propertyName = expectedMember.Expectation.Name;

        // Find the JSON property with the same name as the .NET member
        KeyValuePair<string, JsonNode> match = jsonNode
            .AsEnumerable()
            .SingleOrDefault(x => x.Key.Equals(propertyName, nameComparisonMode));

        if (match.Key is not null)
        {
            // Build an IMember that represents the JSON property
            property = new JsonProperty(jsonNode[match.Key], jsonNode, parent)
            {
                Subject = new Pathway(jsonNode.GetPath(), propertyName, pathAndName => $"JSON property {pathAndName}"),
            };
        }

        return property;
    }

    public override string ToString() => Subject.Description;
}

#endif
