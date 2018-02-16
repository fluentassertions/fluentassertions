namespace FluentAssertions.Equivalency.Ordering
{
    internal class CollectionMemberOrderingRuleDecorator : IOrderingRule
    {
        private readonly IOrderingRule orderingRule;

        public CollectionMemberOrderingRuleDecorator(IOrderingRule orderingRule)
        {
            this.orderingRule = orderingRule;
        }

        public OrderStrictness Evaluate(IMemberInfo memberInfo)
        {
            return orderingRule.Evaluate(new CollectionMemberMemberInfo(memberInfo));
        }

        public override string ToString()
        {
            return orderingRule.ToString();
        }
    }
}
