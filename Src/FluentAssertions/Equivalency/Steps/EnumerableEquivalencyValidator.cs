using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions.Equivalency.Execution;
using FluentAssertions.Equivalency.Tracing;
using FluentAssertions.Execution;
using static FluentAssertions.Common.StringExtensions;

namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// Executes a single equivalency assertion on two collections, optionally recursive and with or without strict ordering.
/// </summary>
[System.Diagnostics.StackTraceHidden]
internal class EnumerableEquivalencyValidator(
    AssertionChain assertionChain,
    IValidateChildNodeEquivalency parent,
    IEquivalencyValidationContext context)
{
    private readonly Tracer tracer = context.Tracer;

    public required bool Recursive { get; init; }

    public required OrderingRuleCollection OrderingRules { get; init; }

    public void Execute<T>(object[] subjects, T[] expectations)
    {
        if (AssertIsNotNull(expectations, subjects))
        {
            if (Recursive)
            {
                using var _ = tracer.WriteBlock(member =>
                    Invariant($"Structurally comparing {subjects} and expectation {expectations} at {member.Expectation}"));

                ExecuteRecursiveAssertion(subjects, expectations);
            }
            else
            {
                using var _ = tracer.WriteBlock(member =>
                    Invariant(
                        $"Comparing subject {subjects} and expectation {expectations} at {member.Expectation} using simple value equality"));

                subjects.Should().BeEquivalentTo(expectations);
            }
        }
    }

    private bool AssertIsNotNull<T>(T[] expectations, object[] subjects)
    {
        assertionChain
            .ForCondition(expectations is not null)
            .FailWith("Expected {context:subject} to be <null>, but found {0}.", [subjects]);

        return assertionChain.Succeeded;
    }

    private void ExecuteRecursiveAssertion<T>(object[] subjects, T[] expectation)
    {
        List<object> remainingSubjects = new(subjects);
        List<T> remainingExpectations = new(expectation);

        bool isOrderingStrict = IsOrderingStrictFor(remainingSubjects, remainingExpectations, context.CurrentNode);
        if (isOrderingStrict)
        {
            new StrictlyOrderedEquivalencyStrategy<T>(parent, context)
                .FindAndRemoveMatches(remainingSubjects, remainingExpectations);
        }
        else
        {
            new LooselyOrderedEquivalencyStrategy<T>(assertionChain, parent, context)
                .FindAndRemoveMatches(remainingSubjects, remainingExpectations);
        }

        ReportRemainingOrMissingItems(remainingSubjects, remainingExpectations, subjects, expectation.Length,
            isOrderingStrict ? "in order" : "in any order");
    }

    private bool IsOrderingStrictFor<T>(List<object> subjects, List<T> expectations, INode currentNode)
    {
        return OrderingRules.IsOrderingStrictFor(new ObjectInfo(new Comparands(subjects, expectations, typeof(T[])),
            currentNode));
    }

#pragma warning disable CA1305

    private void ReportRemainingOrMissingItems<T>(List<object> remainingSubjects, List<T> remainingExpectations,
        object[] allSubjects,
        int expectationLength, string orderingDescription)
    {
        if (remainingSubjects.Count > 0 || remainingExpectations.Count > 0)
        {
            StringBuilder message = new();
            string phrase = expectationLength switch
            {
                0 => "be an empty collection",
                1 => "contain exactly one item",
                _ => $"contain exactly {expectationLength} items {orderingDescription}"
            };

            message.Append(
                $"Expected {{context:collection}} to {phrase}{{reason}}, but ");

            if (remainingExpectations.Count > 0)
            {
                message.Append("it misses {0}");
                if (remainingSubjects.Count > 0)
                {
                    message.Append(" and ");
                }
            }

            if (remainingSubjects.Count > 0)
            {
                phrase = remainingSubjects.Count > 1 ? "extraneous items" : "one extraneous item";
                message.Append($"found {phrase} {{1}}");
            }

            if (context.Options.EnableFullDump)
            {
                message.AppendLine();
                message.AppendLine();
                message.AppendLine("Full dump of {context:subject}: {2}");
            }

            assertionChain.FailWith(message.ToString(), remainingExpectations,
                remainingSubjects.Count == 1 ? remainingSubjects.Single() : remainingSubjects, allSubjects);
        }
    }
}

