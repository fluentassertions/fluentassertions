using System;
using System.Globalization;

namespace FluentAssertionsAsync.Formatting;

public class DoubleValueFormatter : IValueFormatter
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
        return value is double;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        formattedGraph.AddFragment(Format(value));
    }

    private static string Format(object value)
    {
        double doubleValue = (double)value;

        if (double.IsPositiveInfinity(doubleValue))
        {
            return nameof(Double) + "." + nameof(double.PositiveInfinity);
        }

        if (double.IsNegativeInfinity(doubleValue))
        {
            return nameof(Double) + "." + nameof(double.NegativeInfinity);
        }

        if (double.IsNaN(doubleValue))
        {
            return doubleValue.ToString(CultureInfo.InvariantCulture);
        }

        string formattedValue = doubleValue.ToString("R", CultureInfo.InvariantCulture);

        return !formattedValue.Contains('.', StringComparison.Ordinal) && !formattedValue.Contains('E', StringComparison.Ordinal)
            ? formattedValue + ".0"
            : formattedValue;
    }
}
