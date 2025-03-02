using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringWildcardMatchingStrategy : IStringComparisonStrategy
{
    public void AssertForEquality(AssertionChain assertionChain, string subject, string expected)
    {
        bool isMatch = IsMatch(subject, expected);
        if (isMatch != Negate)
        {
            return;
        }

        if (IsLongOrMultiline(subject) || IsLongOrMultiline(expected))
        {
            expected = RenderAsIndentedBlock(expected);
            subject = RenderAsIndentedBlock(subject);

            assertionChain
                .FailWith($$"""
                            {{ExpectationDescription}}

                            {0},

                            {reason}but

                            {1}

                            {{OutcomeDescription}}.

                            """, expected.AsNonFormattable(),
                    subject.AsNonFormattable());
        }
        else
        {
            assertionChain.FailWith($"{ExpectationDescription} {{0}}{{reason}}, but {{1}} {OutcomeDescription}.", expected,
                    subject);
        }
    }

    /// <inheritdoc />
    public void AssertNeitherIsNull(AssertionChain assertionChain, string subject, string expected)
    {
        if (subject is null || expected is null)
        {
            assertionChain.FailWith($"{ExpectationDescription} {{0}}{{reason}}, but found {{1}}.", expected, subject);
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
        return "^"
            + Regex.Escape(wildcardExpression)
                .Replace("\\*", ".*", StringComparison.Ordinal)
                .Replace("\\?", ".", StringComparison.Ordinal)
            + "$";
    }

    private static string RenderAsIndentedBlock(string message)
    {
        string[] lines = message.Split(["\r\n", "\n", "\r"], StringSplitOptions.None);

        return "    \"" + string.Join(Environment.NewLine + "    ", lines) + "\"";
    }

    private static bool IsLongOrMultiline(string message)
    {
        return message.Length > 80 || message.Contains(Environment.NewLine, StringComparison.Ordinal);
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

    private string ExpectationDescription
    {
        get
        {
            var builder = new StringBuilder();

            builder
                .Append(Negate ? "Did not expect " : "Expected ")
                .Append("{context:string}")
                .Append(IgnoreCase ? " to match the equivalent of" : " to match");

            return builder.ToString();
        }
    }

    private string OutcomeDescription
    {
        get { return Negate ? "matches" : "does not"; }
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
