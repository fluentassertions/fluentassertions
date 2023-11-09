using System.Collections.Generic;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringEndStrategy : IStringComparisonStrategy
{
    private readonly IEqualityComparer<string> comparer;
    private readonly bool useEquivalencyMessage;

    public StringEndStrategy(IEqualityComparer<string> comparer, bool useEquivalencyMessage)
    {
        this.comparer = comparer;
        this.useEquivalencyMessage = useEquivalencyMessage;
    }

    public string ExpectationDescription
    {
        get
        {
            string predicateDescription = useEquivalencyMessage ? "end with equivalent of" : "end with";
            return "Expected {context:string} to " + predicateDescription + " ";
        }
    }

    public void ValidateAgainstMismatch(IAssertionScope assertion, string subject, string expected)
    {
        if (!assertion
                .ForCondition(subject!.Length >= expected.Length)
                .FailWith(ExpectationDescription + "{0}{reason}, but {1} is too short.", expected, subject))
        {
            return;
        }

        int indexOfMismatch = subject.Substring(subject.Length - expected.Length).IndexOfFirstMismatch(expected, comparer);

        if (indexOfMismatch < 0)
        {
            return;
        }

        assertion.FailWith(
            ExpectationDescription + "{0}{reason}, but {1} differs near " + subject.IndexedSegmentAt(indexOfMismatch) +
            ".",
            expected, subject);
    }
}
