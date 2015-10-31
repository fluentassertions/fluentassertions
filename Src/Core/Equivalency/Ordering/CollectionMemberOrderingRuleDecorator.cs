namespace FluentAssertions.Equivalency.Ordering
{
    internal class CollectionMemberOrderingRuleDecorator : IOrderingRule
    {
        private readonly IOrderingRule orderingRule;

        public CollectionMemberOrderingRuleDecorator(IOrderingRule orderingRule)
        {
            this.orderingRule = orderingRule;
        }

        public OrderStrictness Evaluate(ISubjectInfo subjectInfo)
        {
            return orderingRule.Evaluate(new CollectionMemberSubjectInfo(subjectInfo));
        }

        public override string ToString()
        {
            return orderingRule.ToString();
        }
    }
}