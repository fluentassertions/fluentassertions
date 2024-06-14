#if NET6_0_OR_GREATER
#nullable enable
using System;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace FluentAssertions.Json;

[StructLayout(LayoutKind.Auto)]
public readonly struct JsonComparatorOptions : IEquatable<JsonComparatorOptions>
{
    public JsonComparatorOptions()
    {
    }

    public JsonCommentHandling CommentHandling { get; init; } = JsonCommentHandling.Skip;

    public bool AllowTrailingCommas { get; init; } = true;

    public int MaxDepth { get; init; } = 64;

    public bool LooseObjectOrderComparison { get; init; }

    public static implicit operator JsonReaderOptions(JsonComparatorOptions options) => new JsonReaderOptions()
    {
        CommentHandling = options.CommentHandling,
        AllowTrailingCommas = options.AllowTrailingCommas,
        MaxDepth = options.MaxDepth,
    };

// Analyzers require all structs to implement equality.
    public override bool Equals(object? obj)
    {
        return obj is JsonComparatorOptions other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)CommentHandling, AllowTrailingCommas, MaxDepth, LooseObjectOrderComparison);
    }

    public static bool operator ==(JsonComparatorOptions left, JsonComparatorOptions right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(JsonComparatorOptions left, JsonComparatorOptions right)
    {
        return !(left == right);
    }

    public bool Equals(JsonComparatorOptions other) => CommentHandling == other.CommentHandling && AllowTrailingCommas == other.AllowTrailingCommas && MaxDepth == other.MaxDepth && LooseObjectOrderComparison == other.LooseObjectOrderComparison;
}
#endif
