using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Inlining;

/// <summary>
/// Represents an inline equivalency assertion, which is implemented using an action. This class
/// enables customized assertion logic to be injected during equivalency validation, typically using one
/// of the assertion APIs provided by Fluent Assertions.
/// </summary>
/// <typeparam name="T">The expected type of the subject to which the assertion is applied.</typeparam>
[SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")]
internal class ActionBasedInlineAssertion<T>(Action<T> assertion) : IInlineEquivalencyAssertion
{
    /// <inheritdoc />
    public void Execute(AssertionChain assertionChain, Comparands comparands)
    {
        if (assertion is null)
        {
            throw new ArgumentNullException(nameof(assertion), "An assertion clause is required");
        }

        assertionChain
            .ForCondition(comparands.Subject is T)
            .FailWith("Expected {context:subject} to be of type {0}, but found {1}.", typeof(T), comparands.Subject?.GetType());

        if (assertionChain.Succeeded)
        {
            assertion((T)comparands.Subject);
        }
    }
}
