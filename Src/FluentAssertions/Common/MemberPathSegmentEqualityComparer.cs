using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Compares two segments of a <see cref="MemberPath"/>.
    /// Sets the <see cref="AnyIndexQualifier"/> equal with any numeric index qualifier.
    /// All other comparisons are default string equality.
    /// </summary>
    internal class MemberPathSegmentEqualityComparer : IEqualityComparer<string>
    {
        private const string AnyIndexQualifier = "*";
        private static readonly Regex IndexQualifierRegex = new(@"^\d+$");

        /// <summary>
        /// Compares two segments of a <see cref="MemberPath"/>.
        /// </summary>
        /// <param name="x">Left part of the comparison.</param>
        /// <param name="y">Right part of the comparison.</param>
        /// <returns>True if segments are equal, false if not.</returns>
        public bool Equals(string x, string y)
        {
            if (x == null || y == null)
            {
                return x == y;
            }

            if (x == AnyIndexQualifier)
            {
                return EqualsAnyIndexQualifier(y);
            }

            if (y == AnyIndexQualifier)
            {
                return EqualsAnyIndexQualifier(x);
            }

            return x == y;
        }

        private static bool EqualsAnyIndexQualifier(string segment)
            => segment == AnyIndexQualifier || IndexQualifierRegex.IsMatch(segment);

        public int GetHashCode(string obj)
        {
#if NETCOREAPP2_1_OR_GREATER
            return obj.GetHashCode(System.StringComparison.Ordinal);
#else
            return obj.GetHashCode();
#endif
        }
    }
}
