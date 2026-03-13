using System;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions.Equivalency.Selection;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// Ensures that types that are marked as value types are treated as such.
/// </summary>
[System.Diagnostics.StackTraceHidden]
public class ValueTypeEquivalencyStep : IEquivalencyStep
{
    // Used to strip leading collection indexers (e.g. "[0]") from paths when the root object is a collection,
    // so that member selection rules which are defined relative to the root type still apply correctly.
    private static readonly Regex RootIndexPattern = new(@"^\[[0-9]+]");

    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        Type expectationType = comparands.GetExpectedType(context.Options);
        EqualityStrategy strategy = context.Options.GetEqualityStrategy(expectationType);

        bool canHandle = strategy is EqualityStrategy.Equals or EqualityStrategy.ForceEquals;

        if (canHandle)
        {
            if (strategy == EqualityStrategy.Equals)
            {
                // Normalize the current path by removing any leading collection index qualifier (e.g. "[0]"),
                // consistent with how SelectMemberByPathSelectionRule normalizes paths during member selection.
                string currentPath = RootIndexPattern.Replace(
                    context.CurrentNode.Expectation.PathAndName, string.Empty);

                SelectMemberByPathSelectionRule conflictingRule = context.Options.SelectionRules
                    .OfType<SelectMemberByPathSelectionRule>()
                    .FirstOrDefault(rule => IsApplicableAt(rule.MemberPath.ToString(), currentPath));

                if (conflictingRule is not null)
                {
                    AssertionChain.GetOrCreate()
                        .For(context)
                        .FailWith(
                            $"{expectationType.Name} is compared by value (because it overrides Equals), " +
                            $"so the {conflictingRule} selection rule does not apply. " +
                            $"Either call ComparingByMembers<{expectationType.Name}>() to force member-wise comparison, " +
                            $"or remove the selection rule.");

                    return EquivalencyResult.EquivalencyProven;
                }
            }

            context.Tracer.WriteLine(member =>
            {
                string strategyName = strategy == EqualityStrategy.Equals
                    ? $"{expectationType} overrides Equals"
                    : "we are forced to use Equals";

                return $"Treating {member.Expectation.Description} as a value type because {strategyName}.";
            });

            AssertionChain.GetOrCreate()
                .For(context)
                .ReuseOnce();

            comparands.Subject.Should().Be(comparands.Expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

            return EquivalencyResult.EquivalencyProven;
        }

        return EquivalencyResult.ContinueWithNext;
    }

    private static bool IsApplicableAt(string rulePath, string currentPath)
    {
        if (string.IsNullOrEmpty(currentPath))
        {
            // At root level, only single-segment paths apply (no dots or collection indexers)
            return IsSingleSegmentPath(rulePath);
        }

        // At a nested level, the rule applies if its path starts with the current path followed by a dot or collection indexer
        return rulePath.StartsWith(currentPath + ".", StringComparison.Ordinal)
            || rulePath.StartsWith(currentPath + "[", StringComparison.Ordinal);
    }

    private static bool IsSingleSegmentPath(string path) =>
        !path.Contains('.', StringComparison.Ordinal) && !path.Contains('[', StringComparison.Ordinal);
}

