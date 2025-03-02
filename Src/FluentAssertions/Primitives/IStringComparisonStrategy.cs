using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

/// <summary>
/// The strategy used for comparing two <see langword="string" />s.
/// </summary>
internal interface IStringComparisonStrategy
{
    /// <summary>
    /// Asserts that neither the <paramref name="subject"/> nor the <paramref name="expected"/> strings are null.
    /// </summary>
    void AssertNeitherIsNull(AssertionChain assertionChain, string subject, string expected);

    /// <summary>
    /// Asserts that the <paramref name="subject"/> matches the <paramref name="expected"/> value.
    /// </summary>
    void AssertForEquality(AssertionChain assertionChain, string subject, string expected);
}
