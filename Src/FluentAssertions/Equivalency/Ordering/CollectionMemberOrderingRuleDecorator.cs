namespace FluentAssertions.Equivalency.Ordering
{
    internal class CollectionMemberOrderingRuleDecorator : IOrderingRule
    {
        private readonly IOrderingRule orderingRule;

        public CollectionMemberOrderingRuleDecorator(IOrderingRule orderingRule)
        {
            this.orderingRule = orderingRule;
        }

        public OrderStrictness Evaluate(IObjectInfo objectInfo)
        {
            return orderingRule.Evaluate(new CollectionMemberObjectInfo(objectInfo));
        }

        public override string ToString()
        {
            return orderingRule.ToString();
        }
    }
}
