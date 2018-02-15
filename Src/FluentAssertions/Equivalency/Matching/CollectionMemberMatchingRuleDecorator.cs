namespace FluentAssertions.Equivalency.Matching
{
    internal class CollectionMemberMatchingRuleDecorator : IMemberMatchingRule
    {
        private readonly IMemberMatchingRule matchingRule;

        public CollectionMemberMatchingRuleDecorator(IMemberMatchingRule matchingRule)
        {
            this.matchingRule = matchingRule;
        }

        public SelectedMemberInfo Match(SelectedMemberInfo expectedMember, object subject, string memberPath,
            IEquivalencyAssertionOptions config)
        {
            return matchingRule.Match(expectedMember, subject, memberPath, config);
        }

        public override string ToString()
        {
            return matchingRule.ToString();
        }
    }
}
