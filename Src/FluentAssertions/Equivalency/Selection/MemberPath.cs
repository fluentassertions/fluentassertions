using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Encapsulates a dotted candidate to a (nested) member of a type. 
    /// </summary>
    internal class MemberPath
    {
        private readonly List<string> segments = new List<string>();

        public MemberPath(string dottedPath)
        {
            segments.AddRange(Segmentize(dottedPath));
        }

        public bool IsParentOrChildOf(string candidate)
        {
            return IsParent(candidate) || IsChild(candidate);
        }

        private bool IsChild(string candidate)
        {
            return Segmentize(candidate).Take(segments.Count).SequenceEqual(segments);
        }

        private bool IsParent(string candidate)
        {
            string[] candidateSegments = Segmentize(candidate);

            return candidateSegments.SequenceEqual(segments.Take(candidateSegments.Length));
        }

        private static string[] Segmentize(string dottedPath)
        {
            return dottedPath.Split(new[] { '.', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}