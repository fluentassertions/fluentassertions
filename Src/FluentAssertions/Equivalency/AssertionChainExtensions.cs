using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency;

internal static class AssertionChainExtensions
{
    /// <summary>
    /// Updates the <see cref="AssertionChain"/> with the relevant information from the current <see cref="IEquivalencyValidationContext"/>, including the correct
    /// caller identification path.
    /// </summary>
    public static AssertionChain For(this AssertionChain chain, IEquivalencyValidationContext context)
    {
        chain.OverrideCallerIdentifier(() => context.CurrentNode.Description);

        return chain
            .WithReportable("configuration", () => context.Options.ToString())
            .BecauseOf(context.Reason);
    }
}
