using System;
using System.Linq.Expressions;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Inlining;

/// <summary>
/// Represents a condition-based inline equivalency assertion that evaluates a specified condition against a subject during object equivalency checks.
/// </summary>
/// <typeparam name="T">The expected type of the subject being asserted.</typeparam>
internal class ConditionBasedInlineAssertion<T>(Expression<Func<T, bool>> condition) : IInlineEquivalencyAssertion
{
    /// <inheritdoc />
    public void Execute(AssertionChain assertionChain, Comparands comparands)
    {
        assertionChain
            .ForCondition(comparands.Subject is T)
            .FailWith("Expected {context:subject} to be of type {0}, but found {1}.", typeof(T), comparands.Subject?.GetType())
            .Then
            .Given(() => (T)comparands.Subject)
            .ForCondition(subject => condition.Compile()(subject))
            .FailWith("Expected {context:subject} to meet condition {0}, but it did not.", condition);
    }
}
