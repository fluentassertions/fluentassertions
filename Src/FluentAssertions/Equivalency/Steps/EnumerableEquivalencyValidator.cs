#if !NET6_0_OR_GREATER
using System;
#endif
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions.Equivalency.Execution;
using FluentAssertions.Equivalency.Tracing;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// Executes a single equivalency assertion on two collections, optionally recursive and with or without strict ordering.
/// </summary>
[System.Diagnostics.StackTraceHidden]
internal class EnumerableEquivalencyValidator
{
    private const int FailedItemsFastFailThreshold = 10;

    #region Private Definitions

    private readonly AssertionChain assertionChain;
    private readonly IValidateChildNodeEquivalency parent;
    private readonly IEquivalencyValidationContext context;

    #endregion

    public EnumerableEquivalencyValidator(AssertionChain assertionChain, IValidateChildNodeEquivalency parent,
        IEquivalencyValidationContext context)
    {
        this.assertionChain = assertionChain;
        this.parent = parent;
        this.context = context;
    }

    public required bool Recursive { get; init; }

    public required OrderingRuleCollection OrderingRules { get; init; }

    public void Execute<T>(object[] subject, T[] expectation)
    {
        if (AssertIsNotNull(expectation, subject) && AssertCollectionsHaveSameCount(subject, expectation))
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

    private bool AssertCollectionsHaveSameCount<T>(ICollection<object> subject, ICollection<T> expectation)
    {
        assertionChain
            .AssertEitherCollectionIsNotEmpty(subject, expectation)
            .Then
            .AssertCollectionHasEnoughItems(subject, expectation)
            .Then
            .AssertCollectionHasNotTooManyItems(subject, expectation);

        return assertionChain.Succeeded;
    }

    private void AssertElementGraphEquivalency<T>(object[] subjects, T[] expectations, INode currentNode)
    {
        unmatchedSubjectIndexes = Enumerable.Range(0, subjects.Length).ToList();

        if (OrderingRules.IsOrderingStrictFor(new ObjectInfo(new Comparands(subjects, expectations, typeof(T[])), currentNode)))
        {
            AssertElementGraphEquivalencyWithStrictOrdering(subjects, expectations);
        }
        else
        {
            AssertElementGraphEquivalencyWithLooseOrdering(subjects, expectations);
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

    private void AssertElementGraphEquivalencyWithLooseOrdering<T>(object[] subjects, T[] expectations)
    {
        int failedCount = 0;

        foreach (int index in Enumerable.Range(0, expectations.Length))
        {
            T expectation = expectations[index];

            using var _ = context.Tracer.WriteBlock(member =>
                string.Create(CultureInfo.InvariantCulture,
                    $"Finding the best match of {expectation} within all items in {subjects} at {member.Expectation}[{index}]"));

            bool succeeded = LooselyMatchAgainst(subjects, expectation, index);

            if (!succeeded)
            {
                failedCount++;

                if (failedCount >= FailedItemsFastFailThreshold)
                {
                    context.Tracer.WriteLine(member =>
                        $"Fail failing loose order comparison of collection after {FailedItemsFastFailThreshold} items failed at {member.Expectation}");

                    break;
                }
            }
        }
    }

    private List<int> unmatchedSubjectIndexes;

    private bool LooselyMatchAgainst<T>(IList<object> subjects, T expectation, int expectationIndex)
    {
        var results = new AssertionResultSet();
        int index = 0;

        GetTraceMessage getMessage = member =>
            $"Comparing subject at {member.Subject}[{index}] with the expectation at {member.Expectation}[{expectationIndex}]";

        int indexToBeRemoved = -1;

        for (var metaIndex = 0; metaIndex < unmatchedSubjectIndexes.Count; metaIndex++)
        {
            index = unmatchedSubjectIndexes[metaIndex];
            object subject = subjects[index];

            using var _ = context.Tracer.WriteBlock(getMessage);
            string[] failures = TryToMatch(subject, expectation, expectationIndex);

            results.AddSet(index, failures);

            if (results.ContainsSuccessfulSet())
            {
                context.Tracer.WriteLine(_ => "It's a match");
                indexToBeRemoved = metaIndex;
                break;
            }

            context.Tracer.WriteLine(_ => $"Contained {failures.Length} failures");
        }

        if (indexToBeRemoved != -1)
        {
            unmatchedSubjectIndexes.RemoveAt(indexToBeRemoved);
        }

        foreach (string failure in results.GetTheFailuresForTheSetWithTheFewestFailures(expectationIndex))
        {
            assertionChain.AddPreFormattedFailure(failure);
        }

        return indexToBeRemoved != -1;
    }

    private string[] TryToMatch<T>(object subject, T expectation, int expectationIndex)
    {
        using var scope = new AssertionScope();

        parent.AssertEquivalencyOf(new Comparands(subject, expectation, typeof(T)), context.AsCollectionItem<T>(expectationIndex));

        return scope.Discard();
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

