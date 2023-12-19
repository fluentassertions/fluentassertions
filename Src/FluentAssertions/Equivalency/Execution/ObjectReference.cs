using System;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentAssertionsAsync.Common;
using static System.FormattableString;

namespace FluentAssertionsAsync.Equivalency.Execution;

/// <summary>
/// Represents  an object tracked by the <see cref="CyclicReferenceDetector"/> including it's location within an object graph.
/// </summary>
internal class ObjectReference
{
    private readonly object @object;
    private readonly string path;
    private readonly bool? compareByMembers;
    private string[] pathElements;

    public ObjectReference(object @object, string path, bool? compareByMembers = null)
    {
        this.@object = @object;
        this.path = path;
        this.compareByMembers = compareByMembers;
    }

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.
    /// </summary>
    /// <returns>
    /// true if the specified <see cref="object"/> is equal to the current <see cref="object"/>; otherwise, false.
    /// </returns>
    /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="object"/>. </param>
    /// <filterpriority>2</filterpriority>
    public override bool Equals(object obj)
    {
        return obj is ObjectReference other
            && ReferenceEquals(@object, other.@object) && IsParentOrChildOf(other);
    }

    private string[] GetPathElements() => pathElements
        ??= path.ToUpperInvariant().Replace("][", "].[", StringComparison.Ordinal)
            .Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

    private bool IsParentOrChildOf(ObjectReference other)
    {
        string[] elements = GetPathElements();
        string[] otherPath = other.GetPathElements();

        int commonElements = Math.Min(elements.Length, otherPath.Length);
        int longerPathAdditionalElements = Math.Max(elements.Length, otherPath.Length) - commonElements;

        return longerPathAdditionalElements > 0 && otherPath.Take(commonElements).SequenceEqual(elements.Take(commonElements));
    }

    /// <summary>
    /// Serves as a hash function for a particular type.
    /// </summary>
    /// <returns>
    /// A hash code for the current <see cref="object"/>.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public override int GetHashCode()
    {
        return RuntimeHelpers.GetHashCode(@object);
    }

    public override string ToString()
    {
        return Invariant($"{{\"{path}\", {@object}}}");
    }

    public bool CompareByMembers => compareByMembers ?? @object?.GetType().OverridesEquals() == false;
}
