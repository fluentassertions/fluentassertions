using System.Threading.Tasks;

namespace FluentAssertionsAsync.Equivalency.Steps;

public class SimpleEqualityEquivalencyStep : IEquivalencyStep
{
    public Task<EquivalencyResult> HandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        if (!context.Options.IsRecursive && !context.CurrentNode.IsRoot)
        {
            comparands.Subject.Should().Be(comparands.Expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

            return Task.FromResult(EquivalencyResult.AssertionCompleted);
        }

        return Task.FromResult(EquivalencyResult.ContinueWithNext);
    }
}
