using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

/// <summary>
/// The strategy used for comparing two <see langword="string" />s.
/// </summary>
internal interface IStringComparisonStrategy
{
    string ExpectationDescription { get; }

    void ValidateAgainstMismatch(IAssertionScope assertion, string subject, string expected);
}
