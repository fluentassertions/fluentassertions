using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Localization;

namespace FluentAssertions.Equivalency.Matching
{
    /// <summary>
    /// Requires the expectation object to have a member with the exact same name.
    /// </summary>
    internal class MustMatchByNameRule : IMemberMatchingRule
    {
        public SelectedMemberInfo Match(SelectedMemberInfo expectedMember, object subject, string memberPath, IEquivalencyAssertionOptions config)
        {
            SelectedMemberInfo compareeSelectedMemberInfoInfo = null;

            if (config.IncludeProperties)
            {
                compareeSelectedMemberInfoInfo = SelectedMemberInfo.Create(subject.GetType()
                    .FindProperty(expectedMember.Name, expectedMember.MemberType));
            }

            if ((compareeSelectedMemberInfoInfo is null) && config.IncludeFields)
            {
                compareeSelectedMemberInfoInfo = SelectedMemberInfo.Create(subject.GetType()
                    .FindField(expectedMember.Name, expectedMember.MemberType));
            }

            if ((compareeSelectedMemberInfoInfo is null) && ExpectationImplementsMemberExplicitly(subject, expectedMember))
            {
                compareeSelectedMemberInfoInfo = expectedMember;
            }

            if (compareeSelectedMemberInfoInfo is null)
            {
                if (memberPath.Length > 0)
                {
                    Execute.Assertion.FailWith(Resources.Member_ExpectationHasMemberPathXDotYThatTheOtherObjectDoesNotHaveFormat,
                        memberPath.ToAlreadyFormattedString(),
                        expectedMember.Name.ToAlreadyFormattedString());
                }
                else
                {
                    Execute.Assertion.FailWith(Resources.Member_ExpectationHasMemberXThatTheOtherObjectDoesNotHaveFormat,
                        expectedMember.Name.ToAlreadyFormattedString());
                }
            }

            return compareeSelectedMemberInfoInfo;
        }

        private static bool ExpectationImplementsMemberExplicitly(object expectation, SelectedMemberInfo subjectMember)
        {
            return subjectMember.DeclaringType.IsInstanceOfType(expectation);
        }

        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return Resources.MatchMemberByNameOrThrow;
        }
    }
}
