namespace FluentAssertions.Equivalency
{
    internal class CollectionMemberAssertionRuleDecorator : IEquivalencyStep
    {
        private readonly IEquivalencyStep equivalencyStep;

        public CollectionMemberAssertionRuleDecorator(IEquivalencyStep equivalencyStep)
        {
            this.equivalencyStep = equivalencyStep;
        }

        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return equivalencyStep.CanHandle(CreateAdjustedCopy(context), config);
        }

        public bool Handle(
            IEquivalencyValidationContext context,
            IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            EquivalencyValidationContext equivalencyValidationContext = CreateAdjustedCopy(context);

            return equivalencyStep.Handle(equivalencyValidationContext, parent, config);
        }

        private static EquivalencyValidationContext CreateAdjustedCopy(IEquivalencyValidationContext context)
        {
            return new EquivalencyValidationContext(context.CurrentNode)
            {
                CompileTimeType = context.CompileTimeType,
                Expectation = context.Expectation,
                Reason = context.Reason,
                Subject = context.Subject
            };
        }

        public override string ToString()
        {
            return equivalencyStep.ToString();
        }
    }
}
