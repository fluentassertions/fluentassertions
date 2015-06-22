namespace FluentAssertions.Equivalency.Matching
{
    internal class CollectionMemberMatchingRuleDecorator : IMemberMatchingRule
    {
        private readonly IMemberMatchingRule matchingRule;

        public CollectionMemberMatchingRuleDecorator(IMemberMatchingRule matchingRule)
        {
            this.matchingRule = matchingRule;
        }

        public SelectedMemberInfo Match(SelectedMemberInfo subjectMember, object expectation, string memberPath,
            IEquivalencyAssertionOptions config)
        {
            return matchingRule.Match(subjectMember, expectation, memberPath, config);
        }

        public override string ToString()
        {
            return matchingRule.ToString();
        }
    }
}