using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency;

[System.Diagnostics.StackTraceHidden]
internal static class AssertionChainExtensions
{
    /// <summary>
    /// Updates the <see cref="AssertionChain"/> with the relevant information from the current <see cref="IEquivalencyValidationContext"/>, including the correct
    /// caller identification path.
    /// </summary>
    public static AssertionChain For(this AssertionChain chain, IEquivalencyValidationContext context)
    {
        chain.OverrideCallerIdentifier(() => context.CurrentNode.Subject.Description);

        return chain
            .WithReportable("configuration", () => context.Options.ToString())
            .BecauseOf(context.Reason);
    }
}

