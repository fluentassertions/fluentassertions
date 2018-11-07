using System;
using System.Collections.Generic;
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
        private readonly List<string> segments = new List<string>();

        public MemberPath(Type declaringType, string dottedPath)
        {
            this.declaringType = declaringType;
            this.dottedPath = dottedPath;
            segments.AddRange(Segmentize(dottedPath));
        }

        public bool IsParentOrChildOf(string candidate)
        {
            return IsParent(candidate) || IsChild(candidate);
        }

        public bool IsSameAs(string candidate, Type memberDeclaringType)
        {
            string[] candidateSegments = Segmentize(candidate);

            return memberDeclaringType == declaringType && candidateSegments.SequenceEqual(segments);
        }

        private bool IsChild(string candidate)
        {
            string[] candidateSegments = Segmentize(candidate);

            return candidateSegments.Length > segments.Count &&
                   candidateSegments.Take(segments.Count).SequenceEqual(segments);
        }

        private bool IsParent(string candidate)
        {
            string[] candidateSegments = Segmentize(candidate);

            return candidateSegments.Length < segments.Count
                   && candidateSegments.SequenceEqual(segments.Take(candidateSegments.Length));
        }

        private static string[] Segmentize(string dottedPath)
        {
            return dottedPath.Split(new[] { '.', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override string ToString()
        {
            return dottedPath;
        }
    }
}
