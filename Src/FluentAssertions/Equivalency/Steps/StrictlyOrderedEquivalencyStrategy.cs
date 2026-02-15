using System.Collections.Generic;
using FluentAssertions.Equivalency.Tracing;
using FluentAssertions.Execution;
using static FluentAssertions.Common.StringExtensions;

namespace FluentAssertions.Equivalency.Steps;

[System.Diagnostics.StackTraceHidden]
internal class StrictlyOrderedEquivalencyStrategy<TExpectation>(
    IValidateChildNodeEquivalency parent,
    IEquivalencyValidationContext context)
{
    private const int FailedItemsFastFailThreshold = 10;
    private readonly Tracer tracer = context.Tracer;

    public void FindAndRemoveMatches(List<object> subjects, List<TExpectation> expectations)
    {
        int failedCount = 0;

        int index = 0;
        for (; index < expectations.Count && index < subjects.Count; index++)
        {
            TExpectation expectation = expectations[index];

            int tempIndex = index;
            using var _ = tracer.WriteBlock(member =>
                Invariant($"Strictly comparing expectation {expectation} at {member.Expectation} to item with index {tempIndex} in {subjects}"));

            bool succeeded = StrictlyMatchAgainst(subjects, expectation, index);
            if (!succeeded)
            {
                failedCount++;
                if (failedCount >= FailedItemsFastFailThreshold)
                {
                    tracer.WriteLine(member =>
                        $"Aborting strict order comparison of collections after {FailedItemsFastFailThreshold} items failed at {member.Expectation}");

                    break;
                }
            }
        }

        subjects.RemoveRange(0, index);
        expectations.RemoveRange(0, index);
    }

    private bool StrictlyMatchAgainst<T>(List<object> subjects, T expectation, int expectationIndex)
    {
        using var scope = new AssertionScope();

        object subject = subjects[expectationIndex];
        IEquivalencyValidationContext equivalencyValidationContext = context.AsCollectionItem<T>(expectationIndex);

        parent.AssertEquivalencyOf(new Comparands(subject, expectation, typeof(T)), equivalencyValidationContext);

        return !scope.HasFailures();
    }
}
