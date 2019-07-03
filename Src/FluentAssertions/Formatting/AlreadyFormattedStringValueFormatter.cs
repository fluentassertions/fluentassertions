using System;
using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    public class AlreadyFormattedStringValueFormatter : IValueFormatter
    {
        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return value is AlreadyFormattedString;
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            string prefix = context.UseLineBreaks
                ? Environment.NewLine
                : "";

            return prefix + (value as AlreadyFormattedString)?.Value;
        }
    }
}
