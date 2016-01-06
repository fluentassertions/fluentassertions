using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Encapsulates a dotted path to a (nested) member of a type. 
    /// </summary>
    internal class MemberPath
    {
        private readonly List<string> segments = new List<string>();

        public MemberPath(string dottedPath)
        {
            segments.AddRange(dottedPath.Split('.'));
        }

        public bool StartsWith(string subPath)
        {
            string[] subPathSegments = subPath.Split('.');
            return segments.Take(subPathSegments.Length).SequenceEqual(subPathSegments);
        }
    }
}