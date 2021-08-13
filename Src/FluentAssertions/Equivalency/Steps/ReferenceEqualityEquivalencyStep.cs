namespace FluentAssertions.Equivalency.Steps
{
    public class ReferenceEqualityEquivalencyStep : IEquivalencyStep
    {
        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            return ReferenceEquals(comparands.Subject, comparands.Expectation) ? EquivalencyResult.AssertionCompleted : EquivalencyResult.ContinueWithNext;
        }
    }
}
