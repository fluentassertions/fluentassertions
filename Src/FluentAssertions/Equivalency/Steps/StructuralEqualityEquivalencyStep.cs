using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

public class StructuralEqualityEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        if (!context.CurrentNode.IsRoot && !context.Options.IsRecursive)
        {
            return EquivalencyResult.ContinueWithNext;
        }

        var assertionChain = AssertionChain.GetOrCreate().For(context);

        if (comparands.Expectation is null)
        {
            assertionChain
                .BecauseOf(context.Reason)
                .FailWith(
                    "Expected {context:subject} to be <null>{reason}, but found {0}.",
                    comparands.Subject);
        }
        else if (comparands.Subject is null)
        {
            assertionChain
                .BecauseOf(context.Reason)
                .FailWith(
                    "Expected {context:object} to be {0}{reason}, but found {1}.",
                    comparands.Expectation,
                    comparands.Subject);
        }
        else
        {
            IMember[] selectedMembers = GetMembersFromExpectation(context.CurrentNode, comparands, context.Options).ToArray();

            if (context.CurrentNode.IsRoot && selectedMembers.Length == 0)
            {
                throw new InvalidOperationException(
                    "No members were found for comparison. " +
                    "Please specify some members to include in the comparison or choose a more meaningful assertion.");
            }

            foreach (IMember selectedMember in selectedMembers)
            {
                AssertMemberEquality(comparands, context, valueChildNodes, selectedMember, context.Options);
            }
        }

        return EquivalencyResult.EquivalencyProven;
    }

    private static void AssertMemberEquality(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency parent, IMember selectedMember, IEquivalencyOptions options)
    {
        var assertionChain = AssertionChain.GetOrCreate().For(context);

        IMember matchingMember = FindMatchFor(selectedMember, context.CurrentNode, comparands.Subject, options, assertionChain);

        if (matchingMember is not null)
        {
            var nestedComparands = new Comparands
            {
                Subject = matchingMember.GetValue(comparands.Subject),
                Expectation = selectedMember.GetValue(comparands.Expectation),
                CompileTimeType = selectedMember.Type
            };

            if (selectedMember.Name != matchingMember.Name)
            {
                // In case the matching process selected a different member on the subject,
                // adjust the current member so that assertion failures report the proper name.
                selectedMember.Name = matchingMember.Name;
            }

            parent.AssertEquivalencyOf(nestedComparands, context.AsNestedMember(selectedMember));
        }
    }

    private static IMember FindMatchFor(IMember selectedMember, INode currentNode, object subject,
        IEquivalencyOptions config, AssertionChain assertionChain)
    {
        IEnumerable<IMember> query =
            from rule in config.MatchingRules
            let match = rule.Match(selectedMember, subject, currentNode, config, assertionChain)
            where match is not null
            select match;

        if (config.IgnoreNonBrowsableOnSubject)
        {
            query = query.Where(member => member.IsBrowsable);
        }

        return query.FirstOrDefault();
    }

    private static IEnumerable<IMember> GetMembersFromExpectation(INode currentNode, Comparands comparands,
        IEquivalencyOptions options)
    {
        IEnumerable<IMember> members = [];

        foreach (IMemberSelectionRule rule in options.SelectionRules)
        {
            members = rule.SelectMembers(currentNode, members,
                new MemberSelectionContext(comparands.RuntimeType, options));
        }

        return members;
    }
}
