namespace FluentAssertions.Equivalency.Steps
{
    public class SimpleEqualityEquivalencyStep : IEquivalencyStep
    {
        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            if (!context.Options.IsRecursive && !context.CurrentNode.IsRoot)
            {
                comparands.Subject.Should().Be(comparands.Expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

                return EquivalencyResult.AssertionCompleted;
            }

            return EquivalencyResult.ContinueWithNext;
        }
    }
}
