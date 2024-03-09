using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

public class ReferenceEqualityEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, AssertionChain assertionChain, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency nestedValidator)
    {
        return ReferenceEquals(comparands.Subject, comparands.Expectation)
            ? EquivalencyResult.EquivalencyProven
            : EquivalencyResult.ContinueWithNext;
    }
}
