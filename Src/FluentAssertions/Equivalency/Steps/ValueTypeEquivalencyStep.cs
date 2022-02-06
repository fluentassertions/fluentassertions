using System;

namespace FluentAssertions.Equivalency.Steps
{
    /// <summary>
    /// Ensures that types that are marked as value types are treated as such.
    /// </summary>
    public class ValueTypeEquivalencyStep : IEquivalencyStep
    {
        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
            IEquivalencyValidator nestedValidator)
        {
            Type expectationType = comparands.GetExpectedType(context.Options);
            EqualityStrategy strategy = context.Options.GetEqualityStrategy(expectationType);

            bool canHandle = strategy == EqualityStrategy.Equals || strategy == EqualityStrategy.ForceEquals;

            if (canHandle)
            {
                context.Tracer.WriteLine(member =>
                {
                    string strategyName = strategy == EqualityStrategy.Equals ? "Equals must be used" : "object overrides Equals";

                    return $"Treating {member.Description} as a value type because {strategyName}.";
                });

                comparands.Subject.Should()
                    .Be(comparands.Expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

                return EquivalencyResult.AssertionCompleted;
            }
            else
            {
                return EquivalencyResult.ContinueWithNext;
            }
        }
    }
}
