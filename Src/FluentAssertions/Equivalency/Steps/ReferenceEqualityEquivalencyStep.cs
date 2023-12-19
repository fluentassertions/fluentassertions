using System.Threading.Tasks;

namespace FluentAssertionsAsync.Equivalency.Steps;

public class ReferenceEqualityEquivalencyStep : IEquivalencyStep
{
    public Task<EquivalencyResult> HandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        return Task.FromResult(ReferenceEquals(comparands.Subject, comparands.Expectation)
            ? EquivalencyResult.AssertionCompleted
            : EquivalencyResult.ContinueWithNext);
    }
}
