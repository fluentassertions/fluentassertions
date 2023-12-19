using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Equivalency.Steps;

public class StructuralEqualityEquivalencyStep : IEquivalencyStep
{
    public async Task<EquivalencyResult> HandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        if (!context.CurrentNode.IsRoot && !context.Options.IsRecursive)
        {
            return EquivalencyResult.ContinueWithNext;
        }

        if (comparands.Expectation is null)
        {
            AssertionScope.Current
                .BecauseOf(context.Reason)
                .FailWith(
                    "Expected {context:subject} to be <null>{reason}, but found {0}.",
                    comparands.Subject);
        }
        else if (comparands.Subject is null)
        {
            AssertionScope.Current
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
                await AssertMemberEqualityAsync(comparands, context, nestedValidator, selectedMember, context.Options);
            }
        }

        return EquivalencyResult.AssertionCompleted;
    }

    private static async Task AssertMemberEqualityAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator parent, IMember selectedMember, IEquivalencyOptions options)
    {
        IMember matchingMember = FindMatchFor(selectedMember, context.CurrentNode, comparands.Subject, options);

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

            await parent.RecursivelyAssertEqualityAsync(nestedComparands, context.AsNestedMember(selectedMember));
        }
    }

    private static IMember FindMatchFor(IMember selectedMember, INode currentNode, object subject,
        IEquivalencyOptions config)
    {
        IEnumerable<IMember> query =
            from rule in config.MatchingRules
            let match = rule.Match(selectedMember, subject, currentNode, config)
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
        IEnumerable<IMember> members = Enumerable.Empty<IMember>();

        foreach (IMemberSelectionRule rule in options.SelectionRules)
        {
            members = rule.SelectMembers(currentNode, members,
                new MemberSelectionContext(comparands.CompileTimeType, comparands.RuntimeType, options));
        }

        return members;
    }
}
