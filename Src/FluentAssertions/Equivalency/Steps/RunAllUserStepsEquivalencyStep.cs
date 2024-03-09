using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// Represents a composite equivalency step that passes the execution to all user-supplied steps that can handle the
/// current context.
/// </summary>
public class RunAllUserStepsEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, AssertionChain assertionChain, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency nestedValidator)
    {
        foreach (IEquivalencyStep step in context.Options.UserEquivalencySteps)
        {
            if (step.Handle(comparands, assertionChain, context, nestedValidator) == EquivalencyResult.EquivalencyProven)
            {
                return EquivalencyResult.EquivalencyProven;
            }
        }

        return EquivalencyResult.ContinueWithNext;
    }
}
