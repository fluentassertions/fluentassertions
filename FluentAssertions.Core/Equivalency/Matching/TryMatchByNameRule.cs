using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Matching
{
    /// <summary>
    /// Finds a member of the expectation with the exact same name, but doesn't require it.
    /// </summary>
    internal class TryMatchByNameRule : IMemberMatchingRule
    {
        public SelectedMemberInfo Match(SelectedMemberInfo subjectMember, object expectation, string memberPath, IEquivalencyAssertionOptions config)
        {
            return expectation.GetType().FindMember(subjectMember.Name, subjectMember.MemberType);
        }

        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return "Try to match member by name";
        }
    }
}