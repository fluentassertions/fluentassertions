#if !NET6_0_OR_GREATER
using System;
#endif
using System.Globalization;
using System.Linq;
using FluentAssertions.Equivalency.Execution;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// Executes a single equivalency assertion on two collections, optionally recursive and with or without strict ordering.
/// </summary>
internal class EnumerableEquivalencyValidator(
    AssertionChain assertionChain,
    IValidateChildNodeEquivalency parent,
    IEquivalencyValidationContext context)
{
    private const int FailedItemsFastFailThreshold = 10;

    public bool Recursive { get; init; }

    public OrderingRuleCollection OrderingRules { get; init; }

    public void Execute<T>(object[] subject, T[] expectation)
    {
        if (AssertIsNotNull(expectation, subject))
        {
            if (Recursive)
            {
                using var _ = context.Tracer.WriteBlock(member =>
                    string.Create(CultureInfo.InvariantCulture,
                        $"Structurally comparing {subject} and expectation {expectation} at {member.Expectation}"));

                AssertElementGraphEquivalency(subject, expectation, context.CurrentNode);
            }
            else
            {
                using var _ = context.Tracer.WriteBlock(member =>
                    string.Create(CultureInfo.InvariantCulture,
                        $"Comparing subject {subject} and expectation {expectation} at {member.Expectation} using simple value equality"));

                subject.Should().BeEquivalentTo(expectation);
            }
        }
    }

    private bool AssertIsNotNull(object expectation, object[] subject)
    {
        assertionChain
            .ForCondition(expectation is not null)
            .FailWith("Expected {context:subject} to be <null>, but found {0}.", [subject]);

        return assertionChain.Succeeded;
    }

    private void AssertElementGraphEquivalency<T>(object[] subjects, T[] expectations, INode currentNode)
    {
        if (OrderingRules.IsOrderingStrictFor(new ObjectInfo(new Comparands(subjects, expectations, typeof(T[])), currentNode)))
        {
            AssertElementGraphEquivalencyWithStrictOrdering(subjects, expectations);
        }
        else
        {
            var task = new LooselyOrderedEquivalencyTask<T>(assertionChain, parent, context);
            task.Execute(subjects, expectations);
        }
    }

    private void AssertElementGraphEquivalencyWithStrictOrdering<T>(object[] subjects, T[] expectations)
    {
        int failedCount = 0;

        foreach (int index in Enumerable.Range(0, expectations.Length))
        {
            T expectation = expectations[index];

            using var _ = context.Tracer.WriteBlock(member =>
                string.Create(CultureInfo.InvariantCulture,
                    $"Strictly comparing expectation {expectation} at {member.Expectation} to item with index {index} in {subjects}"));

            bool succeeded = StrictlyMatchAgainst(subjects, expectation, index);
            if (!succeeded)
            {
                failedCount++;
                if (failedCount >= FailedItemsFastFailThreshold)
                {
                    context.Tracer.WriteLine(member =>
                        $"Aborting strict order comparison of collections after {FailedItemsFastFailThreshold} items failed at {member.Expectation}");

                    break;
                }
            }
        }
    }

    private bool StrictlyMatchAgainst<T>(object[] subjects, T expectation, int expectationIndex)
    {
        using var scope = new AssertionScope();
        object subject = subjects[expectationIndex];
        IEquivalencyValidationContext equivalencyValidationContext = context.AsCollectionItem<T>(expectationIndex);

        parent.AssertEquivalencyOf(new Comparands(subject, expectation, typeof(T)), equivalencyValidationContext);

        bool failed = scope.HasFailures();
        return !failed;
    }
}
