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
            return context.CurrentNode.IsRoot || config.IsRecursive;
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            bool expectationIsNotNull = AssertionScope.Current
                .ForCondition(!(context.Expectation is null))
                .BecauseOf(context.Reason)
                .FailWith(
                    "Expected {context:subject} to be <null>{reason}, but found {0}.",
                    context.Subject);

            bool subjectIsNotNull = AssertionScope.Current
                .ForCondition(!(context.Subject is null))
                .BecauseOf(context.Reason)
                .FailWith(
                    "Expected {context:object} to be {0}{reason}, but found {1}.",
                    context.Expectation,
                    context.Subject);

            if (expectationIsNotNull && subjectIsNotNull)
            {
                IMember[] selectedMembers = GetMembersFromExpectation(context, config).ToArray();
                if (context.CurrentNode.IsRoot && !selectedMembers.Any())
                {
                    throw new InvalidOperationException(
                        "No members were found for comparison. " +
                        "Please specify some members to include in the comparison or choose a more meaningful assertion.");
                }

                foreach (IMember selectedMember in selectedMembers)
                {
                    AssertMemberEquality(context, parent, selectedMember, config);
                }
            }

            return true;
        }

        private static void AssertMemberEquality(IEquivalencyValidationContext context, IEquivalencyValidator parent, IMember selectedMember, IEquivalencyAssertionOptions config)
        {
            IMember matchingMember = FindMatchFor(selectedMember, context, config);
            if (matchingMember is not null)
            {
                IEquivalencyValidationContext nestedContext =
                    context.AsNestedMember(selectedMember, matchingMember);

                if (nestedContext is not null)
                {
                    parent.AssertEqualityUsing(nestedContext);
                }
            }
        }

        private static IMember FindMatchFor(IMember selectedMember, IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            IEnumerable<IMember> query =
                from rule in config.MatchingRules
                let match = rule.Match(selectedMember, context.Subject, context.CurrentNode, config)
                where match is not null
                select match;

            return query.FirstOrDefault();
        }

        private static IEnumerable<IMember> GetMembersFromExpectation(IEquivalencyValidationContext context,
            IEquivalencyAssertionOptions config)
        {
            IEnumerable<IMember> members = Enumerable.Empty<IMember>();

            foreach (IMemberSelectionRule rule in config.SelectionRules)
            {
                members = rule.SelectMembers(context.CurrentNode, members, new MemberSelectionContext
                {
                    CompileTimeType = context.CompileTimeType,
                    RuntimeType = context.RuntimeType,
                    Options = config
                });
            }

            return members;
        }
    }
}
