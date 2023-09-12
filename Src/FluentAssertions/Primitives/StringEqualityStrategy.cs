using System;
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
        if (!ValidateAgainstSuperfluousWhitespace(assertion, subject, expected) ||
            !ValidateAgainstLengthDifferences(assertion, subject, expected))
        {
            return;
        }

        int indexOfMismatch = subject.IndexOfFirstMismatch(expected, comparisonMode);

        if (indexOfMismatch != -1)
        {
            assertion.FailWith(
                ExpectationDescription + "{0}{reason}, but {1} differs near " + subject.IndexedSegmentAt(indexOfMismatch) + ".",
                expected, subject);
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
}
