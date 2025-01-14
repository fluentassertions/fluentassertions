using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// Ensures that types that are marked as value types are treated as such.
/// </summary>
public class ValueTypeEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
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

                return $"Treating {member.Expectation.Description} as a value type because {strategyName}.";
            });

            AssertionChain.GetOrCreate()
                .For(context)
                .ReuseOnce();

            comparands.Subject.Should().Be(comparands.Expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

            return EquivalencyResult.EquivalencyProven;
        }

        return EquivalencyResult.ContinueWithNext;
    }
}
