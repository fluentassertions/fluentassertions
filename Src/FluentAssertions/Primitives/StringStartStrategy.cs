using System.Collections.Generic;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringStartStrategy : IStringComparisonStrategy
{
    private readonly IEqualityComparer<string> comparer;
    private readonly bool useEquivalencyMessage;

    public StringStartStrategy(IEqualityComparer<string> comparer, bool useEquivalencyMessage)
    {
        this.comparer = comparer;
        this.useEquivalencyMessage = useEquivalencyMessage;
    }

    public string ExpectationDescription
    {
        get
        {
            string predicateDescription = useEquivalencyMessage ? "start with equivalent of" : "start with";
            return "Expected {context:string} to " + predicateDescription + " ";
        }
    }

    public void ValidateAgainstMismatch(IAssertionScope assertion, string subject, string expected)
    {
        if (!assertion
                .ForCondition(subject.Length >= expected.Length)
                .FailWith(ExpectationDescription + "{0}{reason}, but {1} is too short.", expected, subject))
        {
            return;
        }

        int indexOfMismatch = subject.IndexOfFirstMismatch(expected, comparer);

        if (indexOfMismatch < 0 || indexOfMismatch >= expected.Length)
        {
            return;
        }

        assertion.FailWith(
            ExpectationDescription + "{0}{reason}, but {1} differs near " + subject.IndexedSegmentAt(indexOfMismatch) +
            ".",
            expected, subject);
    }
}
