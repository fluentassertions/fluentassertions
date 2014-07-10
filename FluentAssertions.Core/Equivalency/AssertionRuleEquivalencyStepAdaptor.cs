namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Adaptor allowing an IAssertionRule to be used where a IEquivalencyStep is required.
    /// </summary>
    internal class AssertionRuleEquivalencyStepAdaptor : IEquivalencyStep
    {
        private readonly IAssertionRule assertionRule;

        public AssertionRuleEquivalencyStepAdaptor(IAssertionRule assertionRule)
        {
            this.assertionRule = assertionRule;
        }

        public bool CanHandle(EquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return (context.PropertyInfo != null);
        }

        public bool Handle(
            EquivalencyValidationContext context,
            IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            return assertionRule.AssertEquality(context);
        }

        public override string ToString()
        {
            return assertionRule.ToString();
        }
    }
}