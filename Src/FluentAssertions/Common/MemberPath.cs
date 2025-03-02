using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Equivalency;

namespace FluentAssertions.Common;

/// <summary>
/// Encapsulates a dotted candidate to a (nested) member of a type as well as the
/// declaring type of the deepest member.
/// </summary>
internal class MemberPath
{
    private readonly string dottedPath;
    private readonly Type reflectedType;
    private readonly Type declaringType;

    private string[] segments;

    private static readonly MemberPathSegmentEqualityComparer MemberPathSegmentEqualityComparer = new();

    public MemberPath(IMember member, string parentPath)
        : this(member.ReflectedType, member.DeclaringType, parentPath.Combine(member.Expectation.Name))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemberPath"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="dottedPath"/> is <see langword="null"/>.</exception>
    public MemberPath(Type reflectedType, Type declaringType, string dottedPath)
        : this(dottedPath)
    {
        this.reflectedType = reflectedType;
        this.declaringType = declaringType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemberPath"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="dottedPath"/> is <see langword="null"/>.</exception>
    public MemberPath(string dottedPath)
    {
        Guard.ThrowIfArgumentIsNull(
            dottedPath, nameof(dottedPath),
            "A member path cannot be null");

        this.dottedPath = dottedPath;
    }

    /// <summary>
    /// Gets a value indicating whether the current object represents a child member of the <paramref name="candidate"/>
    /// or that it is the parent of that candidate.
    /// </summary>
    public bool IsParentOrChildOf(MemberPath candidate)
    {
        return IsParentOf(candidate) || IsChildOf(candidate);
    }

    public bool IsSameAs(MemberPath candidate)
    {
        if (declaringType == candidate.declaringType || declaringType?.IsAssignableFrom(candidate.reflectedType) == true)
        {
            string[] candidateSegments = candidate.Segments;

            return candidateSegments.SequenceEqual(Segments, MemberPathSegmentEqualityComparer);
        }

        return false;
    }

    private bool IsParentOf(MemberPath candidate)
    {
        string[] candidateSegments = candidate.Segments;

        return candidateSegments.Length > Segments.Length &&
            candidateSegments.Take(Segments.Length).SequenceEqual(Segments, MemberPathSegmentEqualityComparer);
    }

    private bool IsChildOf(MemberPath candidate)
    {
        string[] candidateSegments = candidate.Segments;

        return candidateSegments.Length < Segments.Length
            && candidateSegments.SequenceEqual(Segments.Take(candidateSegments.Length),
                MemberPathSegmentEqualityComparer);
    }

    public MemberPath AsParentCollectionOf(MemberPath nextPath)
    {
        var extendedDottedPath = dottedPath.Combine(nextPath.dottedPath, "[]");
        return new MemberPath(nextPath.reflectedType, nextPath.declaringType, extendedDottedPath);
    }

    /// <summary>
    /// Determines whether the current path is the same as <paramref name="path"/> when ignoring any specific indexes.
    /// </summary>
    public bool IsEquivalentTo(string path)
    {
        return path.WithoutSpecificCollectionIndices() == dottedPath.WithoutSpecificCollectionIndices();
    }

    public bool HasSameParentAs(MemberPath path)
    {
        return Segments.Length == path.Segments.Length
            && GetParentSegments().SequenceEqual(path.GetParentSegments(), MemberPathSegmentEqualityComparer);
    }

    private IEnumerable<string> GetParentSegments() => Segments.Take(Segments.Length - 1);

    /// <summary>
    /// Gets a value indicating whether the current path contains an indexer like `[1]` instead of `[]`.
    /// </summary>
    public bool GetContainsSpecificCollectionIndex() => dottedPath.ContainsSpecificCollectionIndex();

    private string[] Segments =>
        segments ??= dottedPath
            .Replace("[]", "[*]", StringComparison.Ordinal)
            .Split(['.', '[', ']'], StringSplitOptions.RemoveEmptyEntries);

    /// <summary>
    /// Returns a copy of the current object as if it represented an un-indexed item in a collection.
    /// </summary>
    public MemberPath WithCollectionAsRoot()
    {
        return new MemberPath(reflectedType, declaringType, "[]." + dottedPath);
    }

    /// <summary>
    /// Returns the name of the member the current path points to without its parent path.
    /// </summary>
    public string MemberName => Segments[^1];

    public override string ToString()
    {
        return dottedPath;
    }
}
