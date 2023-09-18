using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions.Formatting;

namespace FluentAssertions.Common;

internal static class StringExtensions
{
    /// <summary>
    /// Finds the first index at which the <paramref name="value"/> does not match the <paramref name="expected"/>
    /// string anymore, accounting for the specified <paramref name="stringComparison"/>.
    /// </summary>
    public static int IndexOfFirstMismatch(this string value, string expected, StringComparison stringComparison)
    {
        Func<char, char, bool> comparer = GetCharComparer(stringComparison);

        for (int index = 0; index < value.Length; index++)
        {
            if (index >= expected.Length || !comparer(value[index], expected[index]))
            {
                return index;
            }
        }

        return -1;
    }

    private static Func<char, char, bool> GetCharComparer(StringComparison stringComparison) =>
        stringComparison == StringComparison.Ordinal
            ? (x, y) => x == y
            : (x, y) => char.ToUpperInvariant(x) == char.ToUpperInvariant(y);

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
    /// Replaces all characters that might conflict with formatting placeholders with their escaped counterparts.
    /// </summary>
    internal static string UnescapePlaceholders(this string value) =>
        value.Replace("{{", "{", StringComparison.Ordinal).Replace("}}", "}", StringComparison.Ordinal);

    /// <summary>
    /// Joins a string with one or more other strings using a specified separator.
    /// </summary>
    /// <remarks>
    /// Any string that is empty (including the original string) is ignored.
    /// </remarks>
    public static string Combine(this string @this, string other, string separator = ".")
    {
        if (@this.Length == 0)
        {
            return other.Length != 0 ? other : string.Empty;
        }

        if (other.Length == 0)
        {
            return @this;
        }

        if (other.StartsWith('['))
        {
            separator = string.Empty;
        }

        return @this + separator + other;
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
            .Replace("\r", string.Empty, StringComparison.Ordinal)
            .Replace("\\r\\n", string.Empty, StringComparison.Ordinal);
    }

    /// <summary>
    /// Counts the number of times a substring appears within a string by using the specified <see cref="StringComparison"/>.
    /// </summary>
    /// <param name="str">The string to search in.</param>
    /// <param name="substring">The substring to search for.</param>
    /// <param name="comparisonType">The <see cref="StringComparison"/> option to use for comparison.</param>
    public static int CountSubstring(this string str, string substring, StringComparison comparisonType)
    {
        string actual = str ?? string.Empty;
        string search = substring ?? string.Empty;

        int count = 0;
        int index = 0;

        while ((index = actual.IndexOf(search, index, comparisonType)) >= 0)
        {
            index += search.Length;
            count++;
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

    /// <summary>
    /// Get the mismatch segment between <paramref name="expected"/> and <paramref name="subject"/> for long strings,
    /// when they differ at index <paramref name="firstIndexOfMismatch"/>.
    /// </summary>
    public static string GetMismatchSegmentForLongStrings(this string subject, string expected, int firstIndexOfMismatch)
    {
        int trimStart = CalculateSegmentStart(subject, firstIndexOfMismatch);

        int whiteSpaceCount = (firstIndexOfMismatch - trimStart) + 3;
        const string prefix = "  ";

        if (trimStart > 0)
        {
            whiteSpaceCount++;
        }

        var visibleText = subject.Substring(trimStart, firstIndexOfMismatch - trimStart);
        whiteSpaceCount += visibleText.Count(c => c == '\r' || c == '\n');

        var sb = new StringBuilder();

        sb.Append(' ', whiteSpaceCount).AppendLine("\u2193 (actual)")
            .Append(prefix).Append('\"').AppendVisibleText(subject, trimStart).Append('\"').AppendLine()
            .Append(prefix).Append('\"').AppendVisibleText(expected, trimStart).Append('\"').AppendLine()
            .Append(' ', whiteSpaceCount).Append("\u2191 (expected)");

        return sb.ToString();
    }

    private static StringBuilder AppendVisibleText(this StringBuilder sb, string subject, int trimStart)
    {
        var subjectLength = CalculateSegmentLength(subject.Substring(trimStart));

        if (trimStart > 0)
        {
            sb.Append('\u2026');
        }

        sb.Append(subject
            .Substring(trimStart, subjectLength)
            .Replace("\r", "\\r", StringComparison.OrdinalIgnoreCase)
            .Replace("\n", "\\n", StringComparison.OrdinalIgnoreCase));

        if (subject.Length > (trimStart + subjectLength))
        {
            sb.Append('\u2026');
        }

        return sb;
    }

    /// <summary>
    /// Calculates the start index of the visible segment from <paramref name="value"/> when highlighting the difference at <paramref name="index"/>.<br />
    /// Either keep the last 10 characters before <paramref name="index"/> or a word begin (separated by whitespace) between 15 and 5 characters before <paramref name="index"/>.
    /// </summary>
    private static int CalculateSegmentStart(string value, int index)
    {
        if (index <= 10)
        {
            return 0;
        }

        var wordSearchBegin = Math.Max(index - 16, 0);

        var wordIndex = value.Substring(wordSearchBegin, 8)
            .IndexOf(' ', StringComparison.OrdinalIgnoreCase);

        if (wordIndex > 0)
        {
            return wordSearchBegin + wordIndex + 1;
        }

        return index - 10;
    }

    /// <summary>
    /// Calculates how many characters to keep in <paramref name="value"/>.<br />
    /// If a word end is found between 15 and 25 characters, use this word end, otherwise keep 20 characters.
    /// </summary>
    private static int CalculateSegmentLength(string value)
    {
        var word = value.Substring(0, Math.Min(24, value.Length))
            .LastIndexOf(' ');

        if (word > 16)
        {
            return word;
        }

        return Math.Min(20, value.Length);
    }
}
