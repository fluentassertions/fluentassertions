using System.Threading.Tasks;

namespace FluentAssertionsAsync.Equivalency.Steps;

/// <summary>
/// Represents a composite equivalency step that passes the execution to all user-supplied steps that can handle the
/// current context.
/// </summary>
public class RunAllUserStepsEquivalencyStep : IEquivalencyStep
{
    public async Task<EquivalencyResult> HandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        foreach (IEquivalencyStep step in context.Options.UserEquivalencySteps)
        {
            if (await step.HandleAsync(comparands, context, nestedValidator) == EquivalencyResult.AssertionCompleted)
            {
                return EquivalencyResult.AssertionCompleted;
            }
        }

        return EquivalencyResult.ContinueWithNext;
    }
}
