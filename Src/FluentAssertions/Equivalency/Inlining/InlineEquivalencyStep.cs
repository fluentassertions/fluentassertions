using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Inlining;

/// <summary>
/// An implementation of <see cref="IEquivalencyStep"/> that enables inline equivalency assertions.
/// </summary>
/// <remarks>
/// This step checks if the `Expectation` in the provided <see cref="Comparands"/>
/// implements the <see cref="IInlineEquivalencyAssertion"/> interface. If so, it delegates
/// the equivalency comparison to the specified inline equivalency assertion logic.
/// Otherwise, it signals the equivalency process to continue with the next step in the chain.
/// This step allows users to define custom equivalency behaviors inline during assertion
/// execution.
/// </remarks>
public class InlineEquivalencyStep : IEquivalencyStep
{
    /// <inheritdoc />
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        if (comparands.Expectation is IInlineEquivalencyAssertion equivalencyAssertion)
        {
            var assertionChain = AssertionChain.GetOrCreate().For(context);

            equivalencyAssertion.Execute(assertionChain, comparands);

            return EquivalencyResult.EquivalencyProven;
        }

        return EquivalencyResult.ContinueWithNext;
    }
}
