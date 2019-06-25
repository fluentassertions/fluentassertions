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
                .FailWith(
                    Resources.Common_ExpectedSubjectToBeNull + Resources.Common_CommaButFoundXFormat,
                    context.Subject);

            bool subjectIsNotNull =
                AssertionScope.Current.ForCondition(
                    !(context.Subject is null))
                    .FailWith(
                        Resources.Object_ExpectedObjectToBeXFormat + Resources.Common_CommaButFoundYFormat,
                        context.Expectation,
                        context.Subject);

            SelectedMemberInfo[] selectedMembers = GetSelectedMembers(context, config).ToArray();
            if (context.IsRoot && !selectedMembers.Any())
            {
                throw new InvalidOperationException(Resources.Equivalency_NoMembersWereFoundForComparison);
            }

            if (expectationIsNotNull && subjectIsNotNull)
            {
                foreach (var selectedMemberInfo in selectedMembers)
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
            var query =
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

            foreach (var selectionRule in config.SelectionRules)
            {
                members = selectionRule.SelectMembers(members, context, config);
            }

            return members;
        }
    }
}
