using System;
using System.Linq;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Encapsulates a dotted candidate to a (nested) member of a type as well as the
    /// declaring type of the deepest member.
    /// </summary>
    internal class MemberPath
    {
        private readonly Type declaringType;
        private readonly string dottedPath;

        private string[] segments;

        public MemberPath(Type declaringType, string dottedPath)
        {
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
            if (!candidate.declaringType.IsSameOrInherits(declaringType))
            {
                return false;
            }

            string[] segments = GetSegments();
            string[] candidateSegments = candidate.GetSegments();

            return candidateSegments.SequenceEqual(segments);
        }

        private bool IsParentOf(MemberPath candidate)
        {
            string[] candidateSegments = candidate.segments;

            return candidateSegments.Length > segments.Length &&
                   candidateSegments.Take(segments.Length).SequenceEqual(segments);
        }

        private bool IsChildOf(MemberPath candidate)
        {
            string[] candidateSegments = candidate.segments;

            return candidateSegments.Length < segments.Length
                   && candidateSegments.SequenceEqual(segments.Take(candidateSegments.Length));
        }

        private string[] GetSegments() =>
            segments ??= dottedPath.Split(new[] { '.', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

        public override string ToString()
        {
            return dottedPath;
        }
    }
}
