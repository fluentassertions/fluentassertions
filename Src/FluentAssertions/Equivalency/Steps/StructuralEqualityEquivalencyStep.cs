using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps
{
    public class StructuralEqualityEquivalencyStep : IEquivalencyStep
    {
        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
            IEquivalencyValidator nestedValidator)
        {
            if (!context.CurrentNode.IsRoot && !context.Options.IsRecursive)
            {
                return EquivalencyResult.ContinueWithNext;
            }

            bool expectationIsNotNull = AssertionScope.Current
                .ForCondition(comparands.Expectation is not null)
                .BecauseOf(context.Reason)
                .FailWith(
                    "Expected {context:subject} to be <null>{reason}, but found {0}.",
                    comparands.Subject);

            bool subjectIsNotNull = AssertionScope.Current
                .ForCondition(comparands.Subject is not null)
                .BecauseOf(context.Reason)
                .FailWith(
                    "Expected {context:object} to be {0}{reason}, but found {1}.",
                    comparands.Expectation,
                    comparands.Subject);

            if (expectationIsNotNull && subjectIsNotNull)
            {
                IMember[] selectedMembers = GetMembersFromExpectation(context.CurrentNode, comparands, context.Options).ToArray();
                if (context.CurrentNode.IsRoot && !selectedMembers.Any())
                {
                    throw new InvalidOperationException(
                        "No members were found for comparison. " +
                        "Please specify some members to include in the comparison or choose a more meaningful assertion.");
                }

                foreach (IMember selectedMember in selectedMembers)
                {
                    AssertMemberEquality(comparands, context, nestedValidator, selectedMember, context.Options);
                }
            }

            return EquivalencyResult.AssertionCompleted;
        }

        private static void AssertMemberEquality(Comparands comparands, IEquivalencyValidationContext context,
            IEquivalencyValidator parent, IMember selectedMember, IEquivalencyAssertionOptions options)
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

                parent.RecursivelyAssertEquality(nestedComparands, context.AsNestedMember(selectedMember));
            }
        }

        private static IMember FindMatchFor(IMember selectedMember, INode currentNode, object subject,
            IEquivalencyAssertionOptions config)
        {
            IEnumerable<IMember> query =
                from rule in config.MatchingRules
                let match = rule.Match(selectedMember, subject, currentNode, config)
                where match is not null
                select match;

            return query.FirstOrDefault();
        }

        private static IEnumerable<IMember> GetMembersFromExpectation(INode currentNode, Comparands comparands,
            IEquivalencyAssertionOptions options)
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
}
