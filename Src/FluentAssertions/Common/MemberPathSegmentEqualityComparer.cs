using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FluentAssertionsAsync.Common;

/// <summary>
/// Compares two segments of a <see cref="MemberPath"/>.
/// Sets the <see cref="AnyIndexQualifier"/> equal with any numeric index qualifier.
/// All other comparisons are default string equality.
/// </summary>
internal class MemberPathSegmentEqualityComparer : IEqualityComparer<string>
{
    private const string AnyIndexQualifier = "*";
    private static readonly Regex IndexQualifierRegex = new("^[0-9]+$");

    /// <summary>
    /// Compares two segments of a <see cref="MemberPath"/>.
    /// </summary>
    /// <param name="x">Left part of the comparison.</param>
    /// <param name="y">Right part of the comparison.</param>
    /// <returns>True if segments are equal, false if not.</returns>
    public bool Equals(string x, string y)
    {
        if (x == AnyIndexQualifier)
        {
            return IsIndexQualifier(y);
        }

        if (y == AnyIndexQualifier)
        {
            return IsIndexQualifier(x);
        }

        return x == y;
    }

    private static bool IsIndexQualifier(string segment) =>
        segment == AnyIndexQualifier || IndexQualifierRegex.IsMatch(segment);

    public int GetHashCode(string obj)
    {
#if NET6_0_OR_GREATER || NETSTANDARD2_1
        return obj.GetHashCode(StringComparison.Ordinal);
#else
        return obj.GetHashCode();
#endif
    }
}
