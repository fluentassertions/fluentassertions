using System;
using System.Globalization;

#if NET6_0_OR_GREATER

namespace FluentAssertions.Formatting
{
    public class TimeOnlyValueFormatter : IValueFormatter
    {
        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="string"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
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
}

#endif
