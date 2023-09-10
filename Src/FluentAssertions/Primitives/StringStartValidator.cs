using System;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringStartValidator : IStringMismatchValidator
{
    private readonly StringComparison stringComparison;

    public StringStartValidator(StringComparison stringComparison)
    {
        this.stringComparison = stringComparison;
    }

    public string ExpectationDescription
    {
        get
        {
            string predicateDescription = IgnoreCase ? "start with equivalent of" : "start with";
            return "Expected {context:string} to " + predicateDescription + " ";
        }
    }

    private bool IgnoreCase
        => stringComparison == StringComparison.OrdinalIgnoreCase;

    public void ValidateAgainstMismatch(IAssertionScope assertion, string subject, string expected)
    {
        if (assertion
            .ForCondition(subject.Length >= expected.Length)
            .FailWith(ExpectationDescription + "{0}{reason}, but {1} is too short.", expected, subject))
        {
            bool isMismatch = !subject.StartsWith(expected, stringComparison);

            if (isMismatch)
            {
                int indexOfMismatch = subject.IndexOfFirstMismatch(expected, stringComparison);

                assertion.FailWith(
                    ExpectationDescription + "{0}{reason}, but {1} differs near " + subject.IndexedSegmentAt(indexOfMismatch) +
                    ".",
                    expected, subject);
            }
        }
    }
}
