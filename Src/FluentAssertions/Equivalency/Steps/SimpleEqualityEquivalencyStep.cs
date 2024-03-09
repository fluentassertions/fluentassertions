using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

public class SimpleEqualityEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, AssertionChain assertionChain, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency nestedValidator)
    {
        if (!context.Options.IsRecursive && !context.CurrentNode.IsRoot)
        {
            comparands.Subject.Should().Be(comparands.Expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

            return EquivalencyResult.EquivalencyProven;
        }

        return EquivalencyResult.ContinueWithNext;
    }
}
