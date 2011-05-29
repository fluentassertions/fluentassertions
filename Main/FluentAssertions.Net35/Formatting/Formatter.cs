using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Formatting
{
    /// <summary>
    /// Provides services for formatting an object being used in an assertion in a human readable format.
    /// </summary>
    public static class Formatter
    {
        /// <summary>
        ///   A list of objects responsible for formatting the objects represented by placeholders.
        /// </summary>
        public static readonly List<IValueFormatter> formatters = new List<IValueFormatter>
        {
            new NullValueFormatter(),
            new DateTimeValueFormatter(),
            new TimeSpanValueFormatter(),
            new NumericValueFormatter(),
            new StringValueFormatter(),
            new ExpressionValueFormatter(),
            new EnumerableValueFormatter(),
            new DefaultValueFormatter()
        };

        /// <summary>
        ///   Returns a human-readable representation of a particular object.
        /// </summary>
        public static string ToString(object value)
        {
            var formatter = formatters.First(f => f.CanHandle(value));

            return formatter.ToString(value);
        }

        /// <summary>
        ///   Returns a human-readable representation of a particular object that starts on a new line.
        /// </summary>
        public static string ToStringLine(object value)
        {
            return Environment.NewLine + ToString(value);
        }
    }
}
