using System;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentAssertions.Common;
using static System.FormattableString;

namespace FluentAssertions.Equivalency.Execution;

/// <summary>
/// Represents  an object tracked by the <see cref="CyclicReferenceDetector"/> including it's location within an object graph.
/// </summary>
internal class ObjectReference
{
    private readonly object @object;
    private readonly string path;
    private string[] pathElements;

    public ObjectReference(object @object, string path, EqualityStrategy equalityStrategy)
    {
        this.@object = @object;
        this.path = path;

        CompareByMembers = equalityStrategy is EqualityStrategy.Members or EqualityStrategy.ForceMembers;
    }

    public ObjectReference(object @object, string path)
    {
        this.@object = @object;
        this.path = path;

        CompareByMembers = @object?.GetType().OverridesEquals() == false;
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
            .Split('.', StringSplitOptions.RemoveEmptyEntries);

    private bool IsParentOrChildOf(ObjectReference other)
    {
        string[] elements = GetPathElements();
        string[] otherPath = other.GetPathElements();

        int commonElements = Math.Min(elements.Length, otherPath.Length);
        int longerPathAdditionalElements = Math.Max(elements.Length, otherPath.Length) - commonElements;

        return longerPathAdditionalElements > 0 && otherPath
            .Take(commonElements)
            .SequenceEqual(elements.Take(commonElements), StringComparer.Ordinal);
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

    /// <summary>
    /// Indicates whether the equality comparison for the object should be based on its members rather than its default
    /// implementation of <see cref="object.Equals(object)"/>
    /// </summary>
    /// <remarks>
    /// This property returns <see langword="true"/>  if the equality strategy is set to <see cref="EqualityStrategy.Members"/> or
    /// <see cref="EqualityStrategy.ForceMembers"/>, and the object does not override its <see cref="object.Equals(object)"/> method.
    /// Otherwise, it returns <see langword="false"/>
    /// </remarks>
    public bool CompareByMembers { get; }
}
