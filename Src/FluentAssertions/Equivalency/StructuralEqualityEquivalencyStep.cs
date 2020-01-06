using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class StructuralEqualityEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return (context.IsRoot || config.IsRecursive);
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            bool expectationIsNotNull = AssertionScope.Current
                .ForCondition(!(context.Expectation is null))
                .BecauseOf(context.Because, context.BecauseArgs)
                .FailWith(
                    "Expected {context:subject} to be <null>{reason}, but found {0}.",
                    context.Subject);

            bool subjectIsNotNull = AssertionScope.Current
                .ForCondition(!(context.Subject is null))
                .BecauseOf(context.Because, context.BecauseArgs)
                .FailWith(
                    "Expected {context:object} to be {0}{reason}, but found {1}.",
                    context.Expectation,
                    context.Subject);

            if (expectationIsNotNull && subjectIsNotNull)
            {
                SelectedMemberInfo[] selectedMembers = GetSelectedMembers(context, config).ToArray();
                if (context.IsRoot && !selectedMembers.Any())
                {
                    throw new InvalidOperationException(
                        "No members were found for comparison. " +
                        "Please specify some members to include in the comparison or choose a more meaningful assertion.");
                }

                foreach (SelectedMemberInfo selectedMemberInfo in selectedMembers)
                {
                    AssertMemberEquality(context, parent, selectedMemberInfo, config);
                }
            }

            return true;
        }

        private static void AssertMemberEquality(IEquivalencyValidationContext context, IEquivalencyValidator parent, SelectedMemberInfo selectedMemberInfo, IEquivalencyAssertionOptions config)
        {
            SelectedMemberInfo matchingMember = FindMatchFor(selectedMemberInfo, context, config);
            if (matchingMember != null)
            {
                IEquivalencyValidationContext nestedContext =
                    context.CreateForNestedMember(selectedMemberInfo, matchingMember);

                if (nestedContext != null)
                {
                    parent.AssertEqualityUsing(nestedContext);
                }
            }
        }

        private static SelectedMemberInfo FindMatchFor(SelectedMemberInfo selectedMemberInfo, IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            IEnumerable<SelectedMemberInfo> query =
                from rule in config.MatchingRules
                let match = rule.Match(selectedMemberInfo, context.Subject, context.SelectedMemberDescription, config)
                where match != null
                select match;

            return query.FirstOrDefault();
        }

        internal IEnumerable<SelectedMemberInfo> GetSelectedMembers(IEquivalencyValidationContext context,
            IEquivalencyAssertionOptions config)
        {
            IEnumerable<SelectedMemberInfo> members = Enumerable.Empty<SelectedMemberInfo>();

            foreach (IMemberSelectionRule selectionRule in config.SelectionRules)
            {
                members = selectionRule.SelectMembers(members, context, config);
            }

            return members;
        }
    }
}
