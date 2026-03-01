#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions.Execution;

namespace FluentAssertions.Collections;

/// <summary>
/// Contains a number of methods to assert that a <see cref="Span{T}"/> or <see cref="ReadOnlySpan{T}"/> matches an expected set of values.
/// </summary>
/// <remarks>
/// Note: Assertion methods convert spans to arrays for comparison, which involves heap allocation.
/// This is necessary to integrate with the existing assertion infrastructure.
/// </remarks>
[DebuggerNonUserCode]
public class SpanAssertions<T> : GenericCollectionAssertions<T[], T, SpanAssertions<T>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpanAssertions{T}"/> class.
    /// </summary>
    /// <param name="actualValue">The values of the span under test.</param>
    /// <param name="assertionChain">The assertion chain that is used to build fluent continuations.</param>
    public SpanAssertions(T[] actualValue, AssertionChain assertionChain)
        : base(actualValue, assertionChain)
    {
    }

    /// <summary>
    /// Asserts that the current span is equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected values to compare against.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <returns>
    /// An <see cref="AndConstraint{TParent}"/> which can be used to chain assertions.
    /// </returns>
    public AndConstraint<SpanAssertions<T>> BeEqualTo(Span<T> expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return Equal(expected.ToArray(), because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the current span is equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected values to compare against.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <returns>
    /// An <see cref="AndConstraint{TParent}"/> which can be used to chain assertions.
    /// </returns>
    public AndConstraint<SpanAssertions<T>> BeEqualTo(ReadOnlySpan<T> expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return Equal(expected.ToArray(), because, becauseArgs);
    }
}
#endif
