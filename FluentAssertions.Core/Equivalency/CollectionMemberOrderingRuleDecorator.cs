namespace FluentAssertions.Equivalency
{
    internal class CollectionMemberOrderingRuleDecorator : IOrderingRule
    {
        private readonly IOrderingRule orderingRule;

        public CollectionMemberOrderingRuleDecorator(IOrderingRule orderingRule)
        {
            this.orderingRule = orderingRule;
        }

        public bool AppliesTo(ISubjectInfo subjectInfo)
        {
            return orderingRule.AppliesTo(new CollectionMemberSubjectInfo(subjectInfo));
        }
    }
}