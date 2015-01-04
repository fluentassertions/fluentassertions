using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Requires the expectation object to have a member with the exact same name.
    /// </summary>
    internal class MustMatchByNameRule : IMemberMatchingRule
    {
        public ISelectedMemberInfo Match(ISelectedMemberInfo subjectMember, object expectation, string memberPath, IEquivalencyAssertionOptions config)
        {
            ISelectedMemberInfo compareeSelectedMemberInfoInfo = null;

            if (config.IncludeProperties)
            {
                compareeSelectedMemberInfoInfo = SelectedMemberInfo.Create(expectation.GetType()
                    .FindProperty(subjectMember.Name, subjectMember.MemberType));
            }

            if ((compareeSelectedMemberInfoInfo == null) && ExpectationImplementsMemberExplicitly(expectation, subjectMember))
            {
                compareeSelectedMemberInfoInfo = subjectMember;
            }
            
            if (compareeSelectedMemberInfoInfo == null)
            {
                string path = (memberPath.Length > 0) ? memberPath + "." : "member ";

                Execute.Assertion.FailWith(
                    "Subject has " + path + subjectMember.Name + " that the other object does not have.");
            }

            return compareeSelectedMemberInfoInfo;
        }

        private static bool ExpectationImplementsMemberExplicitly(object expectation, ISelectedMemberInfo subjectMember)
        {
            return subjectMember.DeclaringType.IsInstanceOfType(expectation);
        }

        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return "Match member by name (or throw)";
        }
    }
}