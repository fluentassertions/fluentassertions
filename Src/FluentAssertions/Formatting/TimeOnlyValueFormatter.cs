#if NET6_0_OR_GREATER

using System;
using System.Globalization;

namespace FluentAssertionsAsync.Formatting;

public class TimeOnlyValueFormatter : IValueFormatter
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
        return value is TimeOnly;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        var timeOnly = (TimeOnly)value;
        formattedGraph.AddFragment(timeOnly.ToString("<HH:mm:ss.fff>", CultureInfo.InvariantCulture));
    }
}

#endif
