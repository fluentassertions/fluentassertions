using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Inlining;

/// <summary>
/// Represents a condition-based inline equivalency assertion that evaluates a specified condition against a subject during object equivalency checks.
/// </summary>
/// <typeparam name="T">The expected type of the subject being asserted.</typeparam>
[SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")]
internal class ConditionBasedInlineAssertion<T>(Expression<Func<T, bool>> condition) : IInlineEquivalencyAssertion
{
    /// <inheritdoc />
    public void Execute(AssertionChain assertionChain, Comparands comparands)
    {
        if (condition is null)
        {
            throw new ArgumentNullException(nameof(condition), "A boolean condition is required");
        }

        assertionChain
            .ForCondition(comparands.Subject is T)
            .FailWith("Expected {context:subject} to be of type {0}, but found {1}.", typeof(T), comparands.Subject?.GetType())
            .Then
            .ForCondition(condition.Compile()((T)comparands.Subject))
            .FailWith("Expected {context:subject} to meet condition {0}, but it did not.", condition);
    }
}
