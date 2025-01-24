using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions.Formatting;

namespace FluentAssertions.Common;

internal static class StringExtensions
{
    /// <summary>
    /// Finds the first index at which the <paramref name="value"/> does not match the <paramref name="expected"/>
    /// string anymore, accounting for the specified <paramref name="comparer"/>.
    /// </summary>
    public static int IndexOfFirstMismatch(this string value, string expected, IEqualityComparer<string> comparer)
    {
        for (int index = 0; index < value.Length; index++)
        {
            if (index >= expected.Length || !comparer.Equals(value[index..(index + 1)], expected[index..(index + 1)]))
            {
                return index;
            }
        }

        return -1;
    }

    /// <summary>
    /// Gets the quoted three characters at the specified index of a string, including the index itself.
    /// </summary>
    public static string IndexedSegmentAt(this string value, int index)
    {
        int length = Math.Min(value.Length - index, 3);
        string formattedString = Formatter.ToString(value.Substring(index, length));

        return $"{formattedString} (index {index})".EscapePlaceholders();
    }

    /// <summary>
    /// Replaces all numeric indices from a path like "property[0].nested" and returns "property[].nested"
    /// </summary>
    public static string WithoutSpecificCollectionIndices(this string indexedPath)
    {
        return Regex.Replace(indexedPath, @"\[[0-9]+\]", "[]");
    }

    /// <summary>
    /// Determines whether a string contains a specific index like `[0]` instead of just `[]`.
    /// </summary>
    public static bool ContainsSpecificCollectionIndex(this string indexedPath)
    {
        return Regex.IsMatch(indexedPath, @"\[[0-9]+\]");
    }

    /// <summary>
    /// Replaces all characters that might conflict with formatting placeholders with their escaped counterparts.
    /// </summary>
    public static string EscapePlaceholders(this string value) =>
        value.Replace("{", "{{", StringComparison.Ordinal).Replace("}", "}}", StringComparison.Ordinal);

    /// <summary>
    /// Joins a string with another string using the specified separator.
    /// </summary>
    /// <remarks>
    /// Any string that is empty or null (including the original string) is ignored. Also, if the second
    /// string starts with an index operator, the separator is omitted.
    /// </remarks>
    public static string Combine(this string @this, string other, string separator = ".")
    {
        if (@this.IsNullOrEmpty())
        {
            return !other.IsNullOrEmpty() ? other : string.Empty;
        }

        if (other is null || other.StartsWith('['))
        {
            separator = string.Empty;
        }

        return @this + separator + (other ?? "");
    }

    /// <summary>
    /// Changes the first character of a string to uppercase.
    /// </summary>
    public static string Capitalize(this string @this)
    {
        if (@this.Length == 0)
        {
            return @this;
        }

        char[] charArray = @this.ToCharArray();
        charArray[0] = char.ToUpperInvariant(charArray[0]);
        return new string(charArray);
    }

    /// <summary>
    /// Appends tab character at the beginning of each line in a string.
    /// </summary>
    /// <param name="this"></param>
    public static string IndentLines(this string @this)
    {
        return string.Join(Environment.NewLine,
            @this.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => $"\t{x}"));
    }

    public static string RemoveNewLines(this string @this)
    {
        return @this.Replace("\n", string.Empty, StringComparison.Ordinal)
            .Replace("\r", string.Empty, StringComparison.Ordinal);
    }

    public static string RemoveNewlineStyle(this string @this)
    {
        return @this.Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace("\r", "\n", StringComparison.Ordinal);
    }

    public static string RemoveTrailingWhitespaceFromLines(this string input)
    {
        // This regex matches whitespace characters (\s) that are followed by a line ending (\r?\n)
        return Regex.Replace(input, @"[ \t]+(?=\r?\n)", string.Empty);
    }

    /// <summary>
    /// Counts the number of times the <paramref name="substring"/> appears within a string by using the specified <paramref name="comparer"/>.
    /// </summary>
    public static int CountSubstring(this string str, string substring, IEqualityComparer<string> comparer)
    {
        string actual = str ?? string.Empty;
        string search = substring ?? string.Empty;

        int count = 0;
        int maxIndex = actual.Length - search.Length;
        for (int index = 0; index <= maxIndex; index++)
        {
            if (comparer.Equals(actual[index..(index + search.Length)], search))
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Determines if the <paramref name="value"/> is longer than 8 characters or contains an <see cref="Environment.NewLine"/>.
    /// </summary>
    public static bool IsLongOrMultiline(this string value)
    {
        const int humanReadableLength = 8;
        return value.Length > humanReadableLength || value.Contains(Environment.NewLine, StringComparison.Ordinal);
    }

    public static bool IsNullOrEmpty([NotNullWhen(false)] this string value)
    {
        return string.IsNullOrEmpty(value);
    }
}
