using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Inlining;

/// <summary>
/// Defines an interface for inline equivalency assertions, which are used to compare objects
/// during the equivalency validation process with specific conditions or custom logic.
/// </summary>
public interface IInlineEquivalencyAssertion
{
    /// <summary>
    /// Executes the inline equivalency assertion process on the specified comparands
    /// using a provided assertion chain. This method is utilized to perform a
    /// customized comparison between objects during equivalency validation.
    /// </summary>
    /// <param name="assertionChain">An <see cref="AssertionChain"/> instance
    /// used to track and assert conditions during the equivalency assertion process.
    /// This enables the chaining of multiple assertions with contextual explanations.</param>
    /// <param name="comparands">A <see cref="Comparands"/> instance
    /// that holds the pair of objects being compared, as well as their associated contextual metadata.</param>
    void Execute(AssertionChain assertionChain, Comparands comparands);
}
