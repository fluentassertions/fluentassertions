namespace FluentAssertions.Equivalency
{
    internal class CollectionMemberAssertionRuleDecorator : IEquivalencyStep
    {
        private readonly IEquivalencyStep eqivalencyStep;

        public CollectionMemberAssertionRuleDecorator(IEquivalencyStep equivalencyStep)
        {
            eqivalencyStep = equivalencyStep;
        }

        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return eqivalencyStep.CanHandle(CreateAdjustedCopy(context), config);
        }

        public bool Handle(
            IEquivalencyValidationContext context,
            IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            var equivalencyValidationContext = CreateAdjustedCopy(context);

            return eqivalencyStep.Handle(equivalencyValidationContext, parent, config);
        }

        private static EquivalencyValidationContext CreateAdjustedCopy(IEquivalencyValidationContext context)
        {
            return new EquivalencyValidationContext
            {
                CompileTimeType = context.CompileTimeType,
                Expectation = context.Expectation,
                SelectedMemberDescription = context.SelectedMemberDescription,
                SelectedMemberInfo = context.SelectedMemberInfo,
                SelectedMemberPath = CollectionMemberSubjectInfo.GetAdjustedPropertyPath(context.SelectedMemberPath),
                Because = context.Because,
                BecauseArgs = context.BecauseArgs,
                Subject = context.Subject
            };
        }

        public override string ToString()
        {
            return eqivalencyStep.ToString();
        }
    }
}