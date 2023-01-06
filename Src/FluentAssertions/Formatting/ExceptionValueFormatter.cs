using System;
using static System.FormattableString;

namespace FluentAssertions.Formatting;

public class ExceptionValueFormatter : IValueFormatter
{
    /// <summary>
    /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value for which to create a <see cref="string"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <see langword="false"/>.
    /// </returns>
    public bool CanHandle(object value)
    {
        return value is Exception;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        var exception = (Exception)value;

        formattedGraph.AddFragment(Invariant($"{exception.GetType().FullName} with message \"{exception.Message}\""));

        if (exception.StackTrace is not null)
        {
            foreach (string line in exception.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                formattedGraph.AddLine("  " + line);
            }
        }
    }
}
