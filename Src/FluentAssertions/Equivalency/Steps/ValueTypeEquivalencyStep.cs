using System;
using System.Threading.Tasks;

namespace FluentAssertionsAsync.Equivalency.Steps;

/// <summary>
/// Ensures that types that are marked as value types are treated as such.
/// </summary>
public class ValueTypeEquivalencyStep : IEquivalencyStep
{
    public Task<EquivalencyResult> HandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        Type expectationType = comparands.GetExpectedType(context.Options);
        EqualityStrategy strategy = context.Options.GetEqualityStrategy(expectationType);

        bool canHandle = strategy is EqualityStrategy.Equals or EqualityStrategy.ForceEquals;

        if (canHandle)
        {
            context.Tracer.WriteLine(member =>
            {
                string strategyName = strategy == EqualityStrategy.Equals
                    ? $"{expectationType} overrides Equals"
                    : "we are forced to use Equals";

                return $"Treating {member.Description} as a value type because {strategyName}.";
            });

            comparands.Subject.Should().Be(comparands.Expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

            return Task.FromResult(EquivalencyResult.AssertionCompleted);
        }

        return Task.FromResult(EquivalencyResult.ContinueWithNext);
    }
}
