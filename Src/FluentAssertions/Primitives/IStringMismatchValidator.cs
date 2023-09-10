using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

internal interface IStringMismatchValidator
{
    void ValidateAgainstMismatch(IAssertionScope assertion, string subject, string expected);

    string ExpectationDescription { get; }
}
