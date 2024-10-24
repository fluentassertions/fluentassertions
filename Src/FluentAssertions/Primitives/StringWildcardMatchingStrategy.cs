using System;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringWildcardMatchingStrategy : IStringComparisonStrategy
{
    public void ValidateAgainstMismatch(AssertionChain assertionChain, string subject, string expected)
    {
        bool isMatch = IsMatch(subject, expected);

        if (isMatch != Negate)
        {
            return;
        }

        if (Negate)
        {
            assertionChain.FailWith($"{ExpectationDescription}but {{1}} matches.", expected, subject);
        }
        else
        {
            assertionChain.FailWith($"{ExpectationDescription}but {{1}} does not.", expected, subject);
        }
    }

    private bool IsMatch(string subject, string expected)
    {
        RegexOptions options = IgnoreCase
            ? RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
            : RegexOptions.None;

        string input = CleanNewLines(subject);
        string pattern = ConvertWildcardToRegEx(CleanNewLines(expected));

        return Regex.IsMatch(input, pattern, options | RegexOptions.Singleline);
    }

    private static string ConvertWildcardToRegEx(string wildcardExpression)
    {
        return $"^{Regex.Escape(wildcardExpression)
            .Replace("\\*", ".*", StringComparison.Ordinal)
            .Replace("\\?", ".", StringComparison.Ordinal)}$";
    }

    private string CleanNewLines(string input)
    {
        if (IgnoreAllNewlines)
        {
            return input.RemoveNewLines();
        }

        if (IgnoreNewlineStyle)
        {
            return input.RemoveNewlineStyle();
        }

        return input;
    }

    public string ExpectationDescription
    {
        get
        {
            var builder = new StringBuilder();

            builder
                .Append(Negate ? "Did not expect " : "Expected ")
                .Append("{context:string}")
                .Append(IgnoreCase ? " to match the equivalent of" : " to match")
                .Append(" {0}{reason}, ");

            return builder.ToString();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the subject should not match the pattern.
    /// </summary>
    public bool Negate { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the matching process should ignore any casing difference.
    /// </summary>
    public bool IgnoreCase { get; init; }

    /// <summary>
    /// Ignores all newline differences
    /// </summary>
    public bool IgnoreAllNewlines { get; init; }

    /// <summary>
    /// Ignores the difference between environment newline differences
    /// </summary>
    public bool IgnoreNewlineStyle { get; init; }
}
