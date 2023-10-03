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

    public void ValidateAgainstMismatch(IAssertionScope assertion, string subject, string expected)
    {
        ValidateAgainstSuperfluousWhitespace(assertion, subject, expected);

        if (expected.IsLongOrMultiline() || subject.IsLongOrMultiline())
        {
            int indexOfMismatch = subject.IndexOfFirstMismatch(expected, comparisonMode);

            if (indexOfMismatch == -1)
            {
                ValidateAgainstLengthDifferences(assertion, subject, expected);
                return;
            }

            string locationDescription = $"at index {indexOfMismatch}";
            var matchingString = subject[..indexOfMismatch];
            int lineNumber = matchingString.Count(c => c == '\n');

            if (lineNumber > 0)
            {
                var indexOfLastNewlineBeforeMismatch = matchingString.LastIndexOf('\n');
                var column = matchingString.Length - indexOfLastNewlineBeforeMismatch;
                locationDescription = $"on line {lineNumber + 1} and column {column} (index {indexOfMismatch})";
            }

            assertion.FailWith(
                ExpectationDescription + "the same string{reason}, but they differ " + locationDescription + ":" +
                Environment.NewLine
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

    private void ValidateAgainstSuperfluousWhitespace(IAssertionScope assertion, string subject, string expected)
    {
        assertion
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

    /// <summary>
    /// Get the mismatch segment between <paramref name="expected"/> and <paramref name="subject"/> for long strings,
    /// when they differ at index <paramref name="firstIndexOfMismatch"/>.
    /// </summary>
    private static string GetMismatchSegmentForLongStrings(string subject, string expected, int firstIndexOfMismatch)
    {
        int trimStart = GetStartIndexOfPhraseToShowBeforeTheMismatchingIndex(subject, firstIndexOfMismatch);
        const string prefix = "  \"";
        const string suffix = "\"";
        const char arrowDown = '\u2193';
        const char arrowUp = '\u2191';

        int whiteSpaceCountBeforeArrow = (firstIndexOfMismatch - trimStart) + prefix.Length;

        if (trimStart > 0)
        {
            whiteSpaceCountBeforeArrow++;
        }

        var visibleText = subject[trimStart..firstIndexOfMismatch];
        whiteSpaceCountBeforeArrow += visibleText.Count(c => c is '\r' or '\n');

        var sb = new StringBuilder();

        sb.Append(' ', whiteSpaceCountBeforeArrow).Append(arrowDown).AppendLine(" (actual)");
        AppendPrefixAndEscapedPhraseToShowWithEllipsisAndSuffix(sb, prefix, subject, trimStart, suffix);
        AppendPrefixAndEscapedPhraseToShowWithEllipsisAndSuffix(sb, prefix, expected, trimStart, suffix);
        sb.Append(' ', whiteSpaceCountBeforeArrow).Append(arrowUp).Append(" (expected)");

        return sb.ToString();
    }

    /// <summary>
    /// Appends the <paramref name="prefix"/>, the escaped visible <paramref name="text"/> phrase decorated with ellipsis and the <paramref name="suffix"/> to the <paramref name="stringBuilder"/>.
    /// </summary>
    /// <remarks>When text phrase starts at <paramref name="indexOfStartingPhrase"/> and with a calculated length omits text on start or end, an ellipsis is added.</remarks>
    private static void AppendPrefixAndEscapedPhraseToShowWithEllipsisAndSuffix(StringBuilder stringBuilder,
        string prefix, string text, int indexOfStartingPhrase, string suffix)
    {
        var subjectLength = GetLengthOfPhraseToShowOrDefaultLength(text[indexOfStartingPhrase..]);
        const char ellipsis = '\u2026';

        stringBuilder.Append(prefix);

        if (indexOfStartingPhrase > 0)
        {
            stringBuilder.Append(ellipsis);
        }

        stringBuilder.Append(text
            .Substring(indexOfStartingPhrase, subjectLength)
            .Replace("\r", "\\r", StringComparison.OrdinalIgnoreCase)
            .Replace("\n", "\\n", StringComparison.OrdinalIgnoreCase));

        if (text.Length > (indexOfStartingPhrase + subjectLength))
        {
            stringBuilder.Append(ellipsis);
        }

        stringBuilder.AppendLine(suffix);
    }

    /// <summary>
    /// Calculates the start index of the visible segment from <paramref name="value"/> when highlighting the difference at <paramref name="indexOfFirstMismatch"/>.
    /// </summary>
    /// <remarks>
    /// Either keep the last 10 characters before <paramref name="indexOfFirstMismatch"/> or a word begin (separated by whitespace) between 15 and 5 characters before <paramref name="indexOfFirstMismatch"/>.
    /// </remarks>
    private static int GetStartIndexOfPhraseToShowBeforeTheMismatchingIndex(string value, int indexOfFirstMismatch)
    {
        const int defaultCharactersToKeep = 10;
        const int minCharactersToKeep = 5;
        const int maxCharactersToKeep = 15;
        const int lengthOfWhitespace = 1;
        const int phraseLengthToCheckForWordBoundary = (maxCharactersToKeep - minCharactersToKeep) + lengthOfWhitespace;

        if (indexOfFirstMismatch <= defaultCharactersToKeep)
        {
            return 0;
        }

        var indexToStartSearchingForWordBoundary = Math.Max(indexOfFirstMismatch - (maxCharactersToKeep + lengthOfWhitespace), 0);

        var indexOfWordBoundary = value
                .IndexOf(' ', indexToStartSearchingForWordBoundary, phraseLengthToCheckForWordBoundary) -
            indexToStartSearchingForWordBoundary;

        if (indexOfWordBoundary >= 0)
        {
            return indexToStartSearchingForWordBoundary + indexOfWordBoundary + lengthOfWhitespace;
        }

        return indexOfFirstMismatch - defaultCharactersToKeep;
    }

    /// <summary>
    /// Calculates how many characters to keep in <paramref name="value"/>.
    /// </summary>
    /// <remarks>
    /// If a word end is found between 15 and 25 characters, use this word end, otherwise keep 20 characters.
    /// </remarks>
    private static int GetLengthOfPhraseToShowOrDefaultLength(string value)
    {
        const int defaultLength = 20;
        const int minLength = 15;
        const int maxLength = 25;
        const int lengthOfWhitespace = 1;

        var indexOfWordBoundary = value
            .LastIndexOf(' ', Math.Min(maxLength + lengthOfWhitespace, value.Length) - 1);

        if (indexOfWordBoundary >= minLength)
        {
            return indexOfWordBoundary;
        }

        return Math.Min(defaultLength, value.Length);
    }
}
