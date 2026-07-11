using System.Diagnostics;

namespace FluentAssertions.Equivalency.Steps;

[StackTraceHidden]
public class ReferenceEqualityEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        return ReferenceEquals(comparands.Subject, comparands.Expectation)
            ? EquivalencyResult.EquivalencyProven
            : EquivalencyResult.ContinueWithNext;
    }
}
