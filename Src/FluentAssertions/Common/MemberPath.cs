using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Equivalency;

namespace FluentAssertions.Common
{
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

        public MemberPath(IMember member, string parentPath)
            : this(member.ReflectedType, member.DeclaringType, parentPath.Combine(member.Name))
        {
        }

        public MemberPath(Type reflectedType, Type declaringType, string dottedPath)
            : this(dottedPath)
        {
            this.reflectedType = reflectedType;
            this.declaringType = declaringType;
        }

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
            if ((declaringType == candidate.declaringType) || declaringType?.IsAssignableFrom(candidate.reflectedType) == true)
            {
                string[] candidateSegments = candidate.Segments;

                return candidateSegments.SequenceEqual(Segments);
            }

            return false;
        }

        private bool IsParentOf(MemberPath candidate)
        {
            string[] candidateSegments = candidate.Segments;

            return candidateSegments.Length > Segments.Length &&
                   candidateSegments.Take(Segments.Length).SequenceEqual(Segments);
        }

        private bool IsChildOf(MemberPath candidate)
        {
            string[] candidateSegments = candidate.Segments;

            return candidateSegments.Length < Segments.Length
                   && candidateSegments.SequenceEqual(Segments.Take(candidateSegments.Length));
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
                   && GetParentSegments().SequenceEqual(path.GetParentSegments());
        }

        private IEnumerable<string> GetParentSegments() => Segments.Take(Segments.Length - 1);

        /// <summary>
        /// Gets a value indicating whether the current path contains an indexer like `[1]` instead of `[]`.
        /// </summary>
        public bool GetContainsSpecificCollectionIndex() => dottedPath.ContainsSpecificCollectionIndex();

        /// <summary>
        /// Returns a copy of the current object as if it represented an un-indexed item in a collection.
        /// </summary>
        public MemberPath WithCollectionAsRoot()
        {
            return new MemberPath(reflectedType, declaringType, "[]." + dottedPath);
        }

        private string[] Segments => segments ??= dottedPath.Split(new[] { '.', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// Returns the name of the member the current path points to without its parent path.
        /// </summary>
        public string MemberName => Segments.Last();

        public override string ToString()
        {
            return dottedPath;
        }
    }
}
