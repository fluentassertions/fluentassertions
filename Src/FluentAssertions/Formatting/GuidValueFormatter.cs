using System;
using System.Collections.Generic;

namespace FluentAssertions.Formatting
{
    public class GuidValueFormatter : IValueFormatter
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
            return value is Guid;
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext multiline, FormatChild formatChild)
        {
            return "{" + value + "}";
        }
    }
}