using System;

namespace FluentAssertions.Equivalency.Matching
{
    [Obsolete]
    internal class ObsoleteMatchingRuleAdapter : IMemberMatchingRule
    {
        private readonly IMatchingRule obsoleteMatchingRule;

        public ObsoleteMatchingRuleAdapter(IMatchingRule obsoleteMatchingRule)
        {
            this.obsoleteMatchingRule = obsoleteMatchingRule;
        }

        public SelectedMemberInfo Match(SelectedMemberInfo subjectMember, object expectation, string memberPath,
            IEquivalencyAssertionOptions config)
        {
            var propertySelectedMemberInfo = subjectMember as PropertySelectedMemberInfo;
            if (propertySelectedMemberInfo != null)
            {
                return
                    SelectedMemberInfo.Create(obsoleteMatchingRule.Match(propertySelectedMemberInfo.PropertyInfo,
                        expectation, memberPath));
            }

            return null;
        }

        public override string ToString()
        {
            return obsoleteMatchingRule.ToString();
        }
    }
}