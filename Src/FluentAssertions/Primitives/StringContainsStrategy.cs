using System.Collections.Generic;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal class StringContainsStrategy : IStringComparisonStrategy
{
    private readonly IEqualityComparer<string> comparer;
    private readonly OccurrenceConstraint occurrenceConstraint;
    private readonly bool ignoringCase;

    public StringContainsStrategy(IEqualityComparer<string> comparer, OccurrenceConstraint occurrenceConstraint)
    {
        this.comparer = comparer;
        this.occurrenceConstraint = occurrenceConstraint;
        ignoringCase = comparer.Equals("A", "a");
    }

    public string ExpectationDescription
    {
        get
        {
            string predicateDescription = ignoringCase ? "contain the equivalent of" : "contain";
            return "Expected {context:string} to " + predicateDescription + " ";
        }
    }

    public void ValidateAgainstMismatch(IAssertionScope assertion, string subject, string expected)
    {
        int actual = subject.CountSubstring(expected, comparer);

        assertion
            .ForConstraint(occurrenceConstraint, actual)
            .FailWith(
                $"Expected {{context:string}} {{0}} to contain the equivalent of {{1}} {{expectedOccurrence}}{{reason}}, but found it {actual.Times()}.",
                subject, expected);
    }
}
