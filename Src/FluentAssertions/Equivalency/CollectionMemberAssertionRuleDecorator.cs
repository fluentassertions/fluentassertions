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
            return new EquivalencyValidationContext
            {
                CompileTimeType = context.CompileTimeType,
                Expectation = context.Expectation,
                SelectedMemberDescription = context.SelectedMemberDescription,
                SelectedMemberInfo = context.SelectedMemberInfo,
                SelectedMemberPath = CollectionMemberMemberInfo.GetAdjustedPropertyPath(context.SelectedMemberPath),
                Because = context.Because,
                BecauseArgs = context.BecauseArgs,
                Subject = context.Subject
            };
        }

        public override string ToString()
        {
            return equivalencyStep.ToString();
        }
    }
}
