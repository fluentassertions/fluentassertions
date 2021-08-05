using System;
using System.Linq;
using System.Reflection;
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
        {
            this.reflectedType = reflectedType;
            this.declaringType = declaringType;
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
            if ((declaringType == candidate.declaringType) || declaringType.IsAssignableFrom(candidate.reflectedType))
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

        private string[] Segments => segments ??= dottedPath.Split(new[] { '.', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

        public override string ToString()
        {
            return dottedPath;
        }
    }
}
