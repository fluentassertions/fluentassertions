using System;
using System.Globalization;

namespace FluentAssertionsAsync.Formatting;

public class SingleValueFormatter : IValueFormatter
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
        return value is float;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        float singleValue = (float)value;

        if (float.IsPositiveInfinity(singleValue))
        {
            formattedGraph.AddFragment(nameof(Single) + "." + nameof(float.PositiveInfinity));
        }
        else if (float.IsNegativeInfinity(singleValue))
        {
            formattedGraph.AddFragment(nameof(Single) + "." + nameof(float.NegativeInfinity));
        }
        else if (float.IsNaN(singleValue))
        {
            formattedGraph.AddFragment(singleValue.ToString(CultureInfo.InvariantCulture));
        }
        else
        {
            formattedGraph.AddFragment(singleValue.ToString("R", CultureInfo.InvariantCulture) + "F");
        }
    }
}
