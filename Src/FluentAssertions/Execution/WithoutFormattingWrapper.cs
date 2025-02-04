using FluentAssertions.Formatting;

namespace FluentAssertions.Execution;

/// <summary>
/// Wrapper to tell the <see cref="Formatter"/> not to apply any value formatters on this string.
/// </summary>
internal class WithoutFormattingWrapper(string value)
{
    public override string ToString() => value;
}
