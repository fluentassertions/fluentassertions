using System.Threading.Tasks;

namespace FluentAssertionsAsync.Equivalency;

/// <summary>
///  Convenient implementation of <see cref="IEquivalencyStep"/> that will only invoke
/// </summary>
public abstract class EquivalencyStep<T> : IEquivalencyStep
{
    public async Task<EquivalencyResult> HandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        if (!typeof(T).IsAssignableFrom(comparands.GetExpectedType(context.Options)))
        {
            return EquivalencyResult.ContinueWithNext;
        }

        return await OnHandleAsync(comparands, context, nestedValidator);
    }

    /// <summary>
    /// Implements <see cref="IEquivalencyStep.HandleAsync"/>, but only gets called when the expected type matches <typeparamref name="T"/>.
    /// </summary>
    protected abstract Task<EquivalencyResult> OnHandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator);
}
