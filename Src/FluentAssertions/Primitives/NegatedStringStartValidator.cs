using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class NegatedStringStartValidator : IStringMismatchValidator
{
    private readonly StringComparison stringComparison;

    public NegatedStringStartValidator(StringComparison stringComparison)
    {
        this.stringComparison = stringComparison;
    }

    public string ExpectationDescription
    {
        get
        {
            string predicateDescription = IgnoreCase ? "start with equivalent of" : "start with";
            return "Expected {context:string} that does not " + predicateDescription + " ";
        }
    }

    private bool IgnoreCase
        => stringComparison == StringComparison.OrdinalIgnoreCase;

    public void ValidateAgainstMismatch(IAssertionScope assertion, string subject, string expected)
    {
        bool isMatch = subject.StartsWith(expected, stringComparison);

        if (isMatch)
        {
            assertion.FailWith(ExpectationDescription + "{0}{reason}, but found {1}.",
                expected, subject);
        }
    }
}
