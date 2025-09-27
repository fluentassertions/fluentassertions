#if NET6_0_OR_GREATER

using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using FluentAssertions.Specialized;
using JetBrains.Annotations;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace FluentAssertions;

/// <summary>
/// Contains an extension method for custom assertions in unit tests related to System.Text.Json objects.
/// </summary>
[DebuggerNonUserCode]
public static class JsonAssertionExtensions
{
    /// <summary>
    /// Returns an object that provides various assertion APIs that act on a <see cref="JsonNode"/>.
    /// </summary>
    [Pure]
    public static JsonNodeAssertions<T> Should<T>([NotNull] this T jsonNode)
        where T : JsonNode
    {
        return new JsonNodeAssertions<T>(jsonNode, AssertionChain.GetOrCreate());
    }

    /// <summary>
    /// Return an object that provides various assertion APIs that treat a <see cref="JsonArray"/>
    /// as a collection of <see cref="JsonNode"/>s.
    /// </summary>
    [Pure]
    public static GenericCollectionAssertions<JsonNode> Should([NotNull] this JsonArray jsonArray)
    {
        return new GenericCollectionAssertions<JsonNode>(jsonArray?.ToArray(), AssertionChain.GetOrCreate());
    }
}

#endif
