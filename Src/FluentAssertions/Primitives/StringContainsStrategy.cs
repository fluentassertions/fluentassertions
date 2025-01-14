using System.Collections.Generic;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringContainsStrategy : IStringComparisonStrategy
{
    private readonly IEqualityComparer<string> comparer;
    private readonly OccurrenceConstraint occurrenceConstraint;

    public StringContainsStrategy(IEqualityComparer<string> comparer, OccurrenceConstraint occurrenceConstraint)
    {
        this.comparer = comparer;
        this.occurrenceConstraint = occurrenceConstraint;
    }

    public string ExpectationDescription => "Expected {context:string} {0} to contain the equivalent of ";

    public void ValidateAgainstMismatch(AssertionChain assertionChain, string subject, string expected)
    {
        int actual = subject.CountSubstring(expected, comparer);

        assertionChain
            .ForConstraint(occurrenceConstraint, actual)
            .FailWith(
                $"{ExpectationDescription}{{1}} {{expectedOccurrence}}{{reason}}, but found it {actual.Times()}.",
                subject, expected);
    }
}
