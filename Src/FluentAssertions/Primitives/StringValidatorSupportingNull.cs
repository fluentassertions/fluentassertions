using System.Diagnostics.CodeAnalysis;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringValidatorSupportingNull
{
    private readonly IStringComparisonStrategy comparisonStrategy;
    private AssertionChain assertionChain;

    public StringValidatorSupportingNull(AssertionChain assertionChain, IStringComparisonStrategy comparisonStrategy,
        [StringSyntax("CompositeFormat")] string because, object[] becauseArgs)
    {
        this.comparisonStrategy = comparisonStrategy;
        this.assertionChain = assertionChain.BecauseOf(because, becauseArgs);
    }

    public void Validate(string subject, string expected)
    {
        if (expected?.IsLongOrMultiline() == true ||
            subject?.IsLongOrMultiline() == true)
        {
            assertionChain = assertionChain.UsingLineBreaks;
        }

        comparisonStrategy.AssertForEquality(assertionChain, subject, expected);
    }
}
