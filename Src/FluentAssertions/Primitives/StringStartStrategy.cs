using System.Collections.Generic;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringStartStrategy : IStringComparisonStrategy
{
    private readonly IEqualityComparer<string> comparer;
    private readonly string predicateDescription;

    public StringStartStrategy(IEqualityComparer<string> comparer, string predicateDescription)
    {
        this.comparer = comparer;
        this.predicateDescription = predicateDescription;
    }

    public string ExpectationDescription => "Expected {context:string} to " + predicateDescription + " ";

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
