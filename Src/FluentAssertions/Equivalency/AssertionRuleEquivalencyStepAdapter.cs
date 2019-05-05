namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Adapter allowing an IAssertionRule to be used where a IEquivalencyStep is required.
    /// </summary>
    internal class AssertionRuleEquivalencyStepAdapter : IEquivalencyStep
    {
        private readonly IAssertionRule assertionRule;

        public AssertionRuleEquivalencyStepAdapter(IAssertionRule assertionRule)
        {
            this.assertionRule = assertionRule;
        }

        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return true;
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            return assertionRule.AssertEquality(context);
        }

        public override string ToString()
        {
            return assertionRule.ToString();
        }
    }
}
