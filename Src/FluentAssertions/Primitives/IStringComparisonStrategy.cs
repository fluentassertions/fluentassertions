using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

/// <summary>
/// The strategy used for comparing two <see langword="string" />s.
/// </summary>
internal interface IStringComparisonStrategy
{
    /// <summary>
    /// The prefix for the message when the assertion fails.
    /// </summary>
    string ExpectationDescription { get; }

    /// <summary>
    /// Asserts that the <paramref name="subject"/> matches the <paramref name="expected"/> value.
    /// </summary>
    void ValidateAgainstMismatch(IAssertionScope assertion, string subject, string expected);
}
