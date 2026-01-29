namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// Represents a composite equivalency step that passes the execution to all user-supplied steps that can handle the
/// current context.
/// </summary>
[System.Diagnostics.StackTraceHidden]
public class RunAllUserStepsEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        foreach (IEquivalencyStep step in context.Options.UserEquivalencySteps)
        {
            if (step.Handle(comparands, context, valueChildNodes) == EquivalencyResult.EquivalencyProven)
            {
                return EquivalencyResult.EquivalencyProven;
            }
        }

        return EquivalencyResult.ContinueWithNext;
    }
}
