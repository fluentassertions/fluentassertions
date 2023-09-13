using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringValidator
{
    private const int HumanReadableLength = 8;

    private readonly IStringComparisonStrategy comparisonStrategy;
    private IAssertionScope assertion;

    public StringValidator(IStringComparisonStrategy comparisonStrategy, string because, object[] becauseArgs)
    {
        this.comparisonStrategy = comparisonStrategy;
        assertion = Execute.Assertion.BecauseOf(because, becauseArgs);
    }

    public void Validate(string subject, string expected)
    {
        if (expected is null && subject is null)
        {
            return;
        }

        if (!ValidateAgainstNulls(subject, expected))
        {
            return;
        }

        if (IsLongOrMultiline(expected) || IsLongOrMultiline(subject))
        {
            assertion = assertion.UsingLineBreaks;
        }

        comparisonStrategy.ValidateAgainstMismatch(assertion, subject, expected);
    }

    private bool ValidateAgainstNulls(string subject, string expected)
    {
        if (expected is null == subject is null)
        {
            return true;
        }

        assertion.FailWith(comparisonStrategy.ExpectationDescription + "{0}{reason}, but found {1}.", expected, subject);
        return false;
    }

    private static bool IsLongOrMultiline(string value)
    {
        return value.Length > HumanReadableLength || value.Contains(Environment.NewLine, StringComparison.Ordinal);
    }
}
