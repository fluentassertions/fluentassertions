using System;
using System.Linq;
using System.Text;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringEqualityStrategy : IStringComparisonStrategy
{
    private readonly StringComparison comparisonMode;

    public StringEqualityStrategy(StringComparison comparisonMode)
    {
        this.comparisonMode = comparisonMode;
    }

    private bool ValidateAgainstSuperfluousWhitespace(IAssertionScope assertion, string subject, string expected)
    {
        return assertion
            .ForCondition(!(expected.Length > subject.Length && expected.TrimEnd().Equals(subject, comparisonMode)))
            .FailWith(ExpectationDescription + "{0}{reason}, but it misses some extra whitespace at the end.", expected)
            .Then
            .ForCondition(!(subject.Length > expected.Length && subject.TrimEnd().Equals(expected, comparisonMode)))
            .FailWith(ExpectationDescription + "{0}{reason}, but it has unexpected whitespace at the end.", expected);
    }

    private bool ValidateAgainstLengthDifferences(IAssertionScope assertion, string subject, string expected)
    {
        return assertion
            .ForCondition(subject.Length == expected.Length)
            .FailWith(() =>
            {
                string mismatchSegment = GetMismatchSegmentForStringsOfDifferentLengths(subject, expected);

                string message = ExpectationDescription +
                    "{0} with a length of {1}{reason}, but {2} has a length of {3}, differs near " + mismatchSegment + ".";

                return new FailReason(message, expected, expected.Length, subject, subject.Length);
            });
    }

    private string GetMismatchSegmentForStringsOfDifferentLengths(string subject, string expected)
    {
        int indexOfMismatch = subject.IndexOfFirstMismatch(expected, comparisonMode);

        // If there is no difference it means that expected starts with subject and subject is shorter than expected
        if (indexOfMismatch == -1)
        {
            // Subject is shorter so we point at its last character.
            // We would like to point at next character as it is the real
            // index of first mismatch, but we need to point at character existing in
            // subject, so the next best thing is the last subject character.
            indexOfMismatch = Math.Max(0, subject.Length - 1);
        }

        return subject.IndexedSegmentAt(indexOfMismatch);
    }

    public void ValidateAgainstMismatch(IAssertionScope assertion, string subject, string expected)
    {
        if (!ValidateAgainstSuperfluousWhitespace(assertion, subject, expected))
        {
            return;
        }

        if (expected.IsLongOrMultiline() || subject.IsLongOrMultiline())
        {
            int indexOfMismatch = subject.IndexOfFirstMismatch(expected, comparisonMode);

            if (indexOfMismatch == -1)
            {
                ValidateAgainstLengthDifferences(assertion, subject, expected);
                return;
            }

            string lineInfo = $"at index {indexOfMismatch}";
            var matchingString = subject[..indexOfMismatch];
            int lineNumber = matchingString.Count(c => c == '\n');

            if (lineNumber > 0)
            {
                var lastLineIndex = matchingString.LastIndexOf('\n');
                var column = matchingString.Length - lastLineIndex;
                lineInfo = $"on line {lineNumber + 1} and column {column} (index {indexOfMismatch})";
            }

            assertion.FailWith(
                ExpectationDescription + "the same string{reason}, but they differ " + lineInfo + ":" + Environment.NewLine
                + GetMismatchSegmentForLongStrings(subject, expected, indexOfMismatch) + ".");
        }
        else if (ValidateAgainstLengthDifferences(assertion, subject, expected))
        {
            int indexOfMismatch = subject.IndexOfFirstMismatch(expected, comparisonMode);

            if (indexOfMismatch != -1)
            {
                assertion.FailWith(
                    ExpectationDescription + "{0}{reason}, but {1} differs near " + subject.IndexedSegmentAt(indexOfMismatch) +
                    ".",
                    expected, subject);
            }
        }
    }

    public string ExpectationDescription
    {
        get
        {
            string predicateDescription = IgnoreCase ? "be equivalent to" : "be";
            return "Expected {context:string} to " + predicateDescription + " ";
        }
    }

    private bool IgnoreCase
        => comparisonMode == StringComparison.OrdinalIgnoreCase;

    /// <summary>
    /// Get the mismatch segment between <paramref name="expected"/> and <paramref name="subject"/> for long strings,
    /// when they differ at index <paramref name="firstIndexOfMismatch"/>.
    /// </summary>
    private static string GetMismatchSegmentForLongStrings(string subject, string expected, int firstIndexOfMismatch)
    {
        int trimStart = CalculateSegmentStart(subject, firstIndexOfMismatch);

        int whiteSpaceCount = (firstIndexOfMismatch - trimStart) + 3;
        const string prefix = "  ";

        if (trimStart > 0)
        {
            whiteSpaceCount++;
        }

        var visibleText = subject.Substring(trimStart, firstIndexOfMismatch - trimStart);
        whiteSpaceCount += visibleText.Count(c => c is '\r' or '\n');

        var sb = new StringBuilder();

        sb.Append(' ', whiteSpaceCount).AppendLine("\u2193 (actual)")
            .Append(prefix).Append('\"');

        AppendVisibleText(sb, subject, trimStart).Append('\"').AppendLine()
            .Append(prefix).Append('\"');

        AppendVisibleText(sb, expected, trimStart).Append('\"').AppendLine()
            .Append(' ', whiteSpaceCount).Append("\u2191 (expected)");

        return sb.ToString();
    }

    private static StringBuilder AppendVisibleText(StringBuilder sb, string subject, int trimStart)
    {
        var subjectLength = CalculateSegmentLength(subject[trimStart..]);

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
        var word = value[..Math.Min(24, value.Length)]
            .LastIndexOf(' ');

        if (word > 16)
        {
            return word;
        }

        return Math.Min(20, value.Length);
    }
}
