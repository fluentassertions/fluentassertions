using System;
using System.Globalization;

namespace FluentAssertionsAsync.Formatting;

public class EnumValueFormatter : IValueFormatter
{
    /// <summary>
    /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value for which to create a <see cref="string"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <see langword="false"/>.
    /// </returns>
    public virtual bool CanHandle(object value)
    {
        return value is Enum;
    }

    /// <inheritdoc />
    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        string typePart = value.GetType().Name;
        string namePart = value.ToString().Replace(", ", "|", StringComparison.Ordinal);

        string valuePart = Convert.ToDecimal(value, CultureInfo.InvariantCulture)
            .ToString(CultureInfo.InvariantCulture);

        formattedGraph.AddFragment($"{typePart}.{namePart} {{value: {valuePart}}}");
    }
}
