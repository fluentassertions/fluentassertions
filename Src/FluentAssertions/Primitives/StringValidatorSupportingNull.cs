using System.Diagnostics.CodeAnalysis;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringValidatorSupportingNull
{
    private readonly IStringComparisonStrategy comparisonStrategy;
    private IAssertionScope assertion;

    public StringValidatorSupportingNull(IStringComparisonStrategy comparisonStrategy, [StringSyntax("CompositeFormat")] string because, object[] becauseArgs)
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

        if (expected?.IsLongOrMultiline() == true ||
            subject?.IsLongOrMultiline() == true)
        {
            assertion = assertion.UsingLineBreaks;
        }

        comparisonStrategy.ValidateAgainstMismatch(assertion, subject, expected);
    }
}
