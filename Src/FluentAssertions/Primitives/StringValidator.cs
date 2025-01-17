using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringValidator
{
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

        if (expected.IsLongOrMultiline() || subject.IsLongOrMultiline())
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
}
