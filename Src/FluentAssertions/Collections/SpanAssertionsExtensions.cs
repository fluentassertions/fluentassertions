#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions.Collections;
using FluentAssertions.Common;
using FluentAssertions.Primitives;

// ReSharper disable once CheckNamespace
#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FluentAssertions;

/// <summary>
/// Provides string-specific assertion extensions for <see cref="SpanAssertions{Char}"/>.
/// </summary>
[DebuggerNonUserCode]
public static class SpanAssertionsExtensions
{
    /// <summary>
    /// Asserts that a <see cref="Span{Char}"/> or <see cref="ReadOnlySpan{Char}"/> is exactly the same as <paramref name="expected"/>,
    /// including the casing and any leading or trailing whitespace.
    /// </summary>
    /// <param name="assertions">The <see cref="SpanAssertions{Char}"/> on which the assertion is executed.</param>
    /// <param name="expected">The expected string.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <returns>
    /// An <see cref="AndConstraint{TParent}"/> which can be used to chain assertions.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public static AndConstraint<SpanAssertions<char>> Be([NotNull] this SpanAssertions<char> assertions, string expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected);

        var subject = new string(assertions.Subject!);

        var stringEqualityValidator = new StringValidator(assertions.CurrentAssertionChain,
            new StringEqualityStrategy(StringComparer.Ordinal, "be"),
            because, becauseArgs);

        stringEqualityValidator.Validate(subject, expected);

        return new AndConstraint<SpanAssertions<char>>(assertions);
    }
}

#endif
