using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Equivalency.Selection;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// Ensures that types that are marked as value types are treated as such.
/// </summary>
[System.Diagnostics.StackTraceHidden]
public class ValueTypeEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        var options = context.Options;
        Type expectationType = comparands.GetExpectedType(options);
        EqualityStrategy strategy = options.GetEqualityStrategy(expectationType);

        if (strategy is not (EqualityStrategy.Equals or EqualityStrategy.ForceEquals))
        {
            return EquivalencyResult.ContinueWithNext;
        }

        if (ReportConflictIfAny(context, comparands, options, expectationType, strategy))
        {
            return EquivalencyResult.EquivalencyProven;
        }

        ApplyValueSemantics(comparands, context, expectationType, strategy);
        return EquivalencyResult.EquivalencyProven;
    }

    /// <summary>
    /// Checks for any selection rule that targets members of the current node, which would be silently
    /// ignored under value-semantic comparison. Reports the conflict and returns <c>true</c> if one is found.
    /// </summary>
    private static bool ReportConflictIfAny(
        IEquivalencyValidationContext context,
        Comparands comparands,
        IEquivalencyOptions options,
        Type expectationType,
        EqualityStrategy strategy)
    {
        // Path-based rules can be checked cheaply by comparing path strings (also handles deep paths like o.Child.Text).
        IMemberSelectionRule conflictingRule = options.SelectionRules
            .OfType<IPathBasedSelectionRule>()
            .FirstOrDefault(rule => rule.SelectsMembersOf(context.CurrentNode));

        // Non-path rules (based on predicates) require evaluating them against the actual member list
        conflictingRule ??= FindConflictingNonPathRule(context.CurrentNode, comparands, options);

        if (conflictingRule is not null)
        {
            ReportConflict(context, expectationType, conflictingRule, strategy);
        }

        return conflictingRule is not null;
    }

    /// <summary>
    /// Finds the first non-path selection rule that would modify the member list of <paramref name="currentNode"/>,
    /// indicating it would be silently ignored due to value-semantic comparison.
    /// </summary>
    private static IMemberSelectionRule FindConflictingNonPathRule(
        INode currentNode, Comparands comparands, IEquivalencyOptions options)
    {
        var selectionContext = new MemberSelectionContext(comparands.CompileTimeType, comparands.RuntimeType, options);
        IList<IMember> allMembers = GetAllMembers(currentNode, selectionContext);

        // If the type has no accessible members, no selection rule can meaningfully conflict.
        if (allMembers.Count == 0)
        {
            return null;
        }

        return options.SelectionRules
            .Where(rule => !IsInfrastructureRule(rule) && rule is not IPathBasedSelectionRule)
            .FirstOrDefault(rule => RuleAffectsMembers(rule, allMembers, GetFilteredMembers(rule, currentNode, allMembers, selectionContext)));
    }

    /// <summary>
    /// Applies <paramref name="rule"/> in isolation to determine whether it modifies the member list.
    /// Inclusion rules are run from an empty starting set; exclusion rules are run on the full member list.
    /// </summary>
    private static IList<IMember> GetFilteredMembers(
        IMemberSelectionRule rule, INode currentNode, IList<IMember> allMembers, MemberSelectionContext context)
    {
        IEnumerable<IMember> seed = rule.IncludesMembers ? [] : allMembers;
        return rule.SelectMembers(currentNode, seed, context).ToList();
    }

    /// <summary>Gets all properties and fields of the current node's type, respecting the configured visibility.</summary>
    private static IList<IMember> GetAllMembers(INode currentNode, MemberSelectionContext context)
    {
        IEnumerable<IMember> members = new AllPropertiesSelectionRule().SelectMembers(currentNode, [], context);
        return new AllFieldsSelectionRule().SelectMembers(currentNode, members, context).ToList();
    }

    /// <summary>
    /// Returns <c>true</c> if <paramref name="rule"/> actually affects the member set of the current value type.
    /// For inclusion rules, a conflict exists only when the rule selects at least one member of this type,
    /// meaning the user intended to compare this value type member-by-member.
    /// For exclusion rules, a conflict exists when the rule removes at least one member from the full set.
    /// </summary>
    private static bool RuleAffectsMembers(IMemberSelectionRule rule, IList<IMember> allMembers, IList<IMember> filteredMembers) =>
        rule.IncludesMembers
            ? filteredMembers.Count > 0
            : MemberSetChanged(allMembers, filteredMembers);

    /// <summary>Returns <c>true</c> if <paramref name="after"/> represents a different set of members than <paramref name="before"/>.</summary>
    private static bool MemberSetChanged(IList<IMember> before, IList<IMember> after) =>
        before.Count != after.Count || before.Except(after).Any();

    /// <summary>Returns <c>true</c> for rules that are part of the default selection pipeline, not user-configured.</summary>
    private static bool IsInfrastructureRule(IMemberSelectionRule rule) =>
        rule is AllPropertiesSelectionRule or AllFieldsSelectionRule or ExcludeNonBrowsableMembersRule;

    /// <summary>Reports an assertion failure for a selection rule that conflicts with value semantics.</summary>
    private static void ReportConflict(
        IEquivalencyValidationContext context,
        Type expectationType,
        IMemberSelectionRule conflictingRule,
        EqualityStrategy strategy)
    {
        Reason reason = context.Reason;
        bool isGeneric = expectationType.IsGenericType;

        string cause = strategy == EqualityStrategy.ForceEquals
            ? $"{expectationType} is compared by value (because ComparingByValue was configured), "
            : $"{expectationType} is compared by value (because it overrides Equals), ";

        string suggestion = strategy == EqualityStrategy.ForceEquals
            ? "Either remove the ComparingByValue configuration, "
            : isGeneric
                ? "Either call the ComparingByMembers(Type) overload to force member-wise comparison, "
                : $"Either call ComparingByMembers<{expectationType.Name}>() to force member-wise comparison, ";

        string message =
            cause +
            $"so the {conflictingRule} selection rule does not apply. " +
            suggestion +
            "or remove the selection rule." +
            reason.FormattedMessage;

        AssertionChain.GetOrCreate().For(context).FailWith(message, reason.Arguments);
    }

    /// <summary>
    /// Traces the value-semantic comparison strategy and performs the equality assertion.
    /// </summary>
    private static void ApplyValueSemantics(
        Comparands comparands,
        IEquivalencyValidationContext context,
        Type expectationType,
        EqualityStrategy strategy)
    {
        context.Tracer.WriteLine(member =>
        {
            string strategyName = strategy == EqualityStrategy.Equals
                ? $"{expectationType} overrides Equals"
                : "we are forced to use Equals";

            return $"Treating {member.Expectation.Description} as a value type because {strategyName}.";
        });

        var reason = context.Reason;

        AssertionChain.GetOrCreate()
            .For(context)
            .ReuseOnce();

        comparands.Subject.Should().Be(comparands.Expectation, reason.FormattedMessage, reason.Arguments);
    }
}

