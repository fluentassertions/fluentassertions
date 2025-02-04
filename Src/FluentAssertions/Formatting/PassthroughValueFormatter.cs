using FluentAssertions.Execution;

namespace FluentAssertions.Formatting;

/// <summary>
/// Ensures that any value wrapped in a <see cref="WithoutFormattingWrapper"/>
/// is passed through as-is.
/// </summary>
internal class PassthroughValueFormatter : IValueFormatter
{
    public bool CanHandle(object value) => value is WithoutFormattingWrapper;

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        formattedGraph.AddFragment(((WithoutFormattingWrapper)value).ToString());
    }
}
