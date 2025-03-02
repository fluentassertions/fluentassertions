using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringValidator
{
    private readonly IStringComparisonStrategy comparisonStrategy;
    private AssertionChain assertionChain;

    public StringValidator(AssertionChain assertionChain, IStringComparisonStrategy comparisonStrategy, string because,
        object[] becauseArgs)
    {
        this.comparisonStrategy = comparisonStrategy;
        this.assertionChain = assertionChain.BecauseOf(because, becauseArgs);
    }

    public void Validate(string subject, string expected)
    {
        if (expected is null && subject is null)
        {
            return;
        }

        comparisonStrategy.AssertNeitherIsNull(assertionChain, subject, expected);

        if (assertionChain.Succeeded)
        {
            if (expected.IsLongOrMultiline() || subject.IsLongOrMultiline())
            {
                assertionChain = assertionChain.UsingLineBreaks;
            }

            comparisonStrategy.AssertForEquality(assertionChain, subject, expected);
        }
    }
}
